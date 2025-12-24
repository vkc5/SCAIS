using SCAIS.Core.Database;
using SCAIS.Core.Models.CoursePlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SCAIS.Core.Builders.CoursePlan
{
    public class AdviserAdjustedPlanBuilder : ICoursePlanBuilder
    {
        private CoursePlanProduct _plan;

        public void StartNewPlan(string planId, string studentId, int semesterId)
        {
            _plan = new CoursePlanProduct
            {
                CoursePlanId = planId,
                StudentId = studentId,
                SemesterId = semesterId
            };
        }

        public void SetInitialStatus(string status)
        {
            _plan.Status = status;
        }

        public void LoadSelectedCourses()
        {
            string sql = @"
SELECT c.CourseCode, c.CourseName, c.Credits
FROM dbo.Course_Plan_Items pi
JOIN dbo.Courses c ON c.CourseCode = pi.CourseCode
WHERE pi.CoursePlanID = @CoursePlanID
ORDER BY c.CourseCode;";

            DataTable dt = Db.Query(sql, new SqlParameter("@CoursePlanID", _plan.CoursePlanId));

            _plan.SelectedCourses = dt.AsEnumerable().Select(r => new CoursePlanItem
            {
                CourseCode = r["CourseCode"].ToString(),
                CourseName = r["CourseName"].ToString(),
                Credits = Convert.ToInt32(r["Credits"]),
                IsEligible = true,
                Message = ""
            }).ToList();
        }

        public void LoadEligibleCoursesAutoValidated()
        {
            // all active courses
            string sqlCourses = @"
SELECT DISTINCT c.CourseCode, c.CourseName, c.Credits
FROM dbo.Courses c
WHERE c.IsActive = 1
ORDER BY c.CourseCode;";

            DataTable allCourses = Db.Query(sqlCourses);

            var selectedSet = _plan.SelectedCourses.Select(x => x.CourseCode).ToHashSet();

            var eligibleList = new List<CoursePlanItem>();

            foreach (DataRow row in allCourses.Rows)
            {
                string code = row["CourseCode"].ToString();
                string name = row["CourseName"].ToString();
                int credits = Convert.ToInt32(row["Credits"]);

                var (isEligible, msg) = CheckEligibility(_plan.StudentId, code);

                if (isEligible)
                {
                    eligibleList.Add(new CoursePlanItem
                    {
                        CourseCode = code,
                        CourseName = name,
                        Credits = credits,
                        IsEligible = true,
                        Message = msg
                    });
                }
            }

            _plan.EligibleCourses = eligibleList;

            // NOTE: the "already selected" will be handled by UI checkboxes using selectedSet
            // but you can return selectedSet via UI from SelectedCourses easily.
        }

        public void CalculateTotals()
        {
            // TotalCredits is computed property in CoursePlanProduct
            // so nothing required here, but keep method for pattern correctness.
        }

        public CoursePlanProduct GetResult()
        {
            return _plan;
        }

        private (bool isEligible, string message) CheckEligibility(string studentId, string courseCode)
        {
            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_Check_Course_Eligibility", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_StudentID", studentId);
                cmd.Parameters.AddWithValue("@p_CourseCode", courseCode);

                var pIsEligible = new SqlParameter("@p_IsEligible", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                var pMsg = new SqlParameter("@p_Message", SqlDbType.VarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(pIsEligible);
                cmd.Parameters.Add(pMsg);

                con.Open();
                cmd.ExecuteNonQuery();

                bool eligible = Convert.ToBoolean(pIsEligible.Value);
                string message = pMsg.Value?.ToString() ?? "";

                return (eligible, message);
            }
        }
    }

}

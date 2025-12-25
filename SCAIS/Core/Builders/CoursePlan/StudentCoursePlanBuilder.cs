using SCAIS.Core.Database;
using SCAIS.Core.Models.CoursePlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS.Core.Builders.CoursePlan
{
    public class StudentCoursePlanBuilder : ICoursePlanBuilder
    {
        private CoursePlanProduct _plan;

        public void StartNewPlan(string planId, string studentId, int semesterId)
        {
            _plan = new CoursePlanProduct
            {
                CoursePlanId = planId,
                StudentId = studentId,
                SemesterId = semesterId,
                SelectedCourses = new List<CoursePlanItem>() // student starts empty
            };
        }

        public void SetInitialStatus(string status)
        {
            _plan.Status = status; // usually "Pending"
        }

        public void LoadSelectedCourses()
        {
            // Student creating a new plan => no selected courses loaded from DB here
            // Keep empty; UI will fill SelectedCourses when user picks.
        }

        public void LoadEligibleCoursesAutoValidated()
        {
            // ✅ Only Core + Student specialization courses (and active only)
            string sqlCourses = @"
DECLARE @SpecId INT;

SELECT @SpecId = SpecializationID
FROM dbo.Students
WHERE StudentID = @StudentID;

SELECT DISTINCT c.CourseCode, c.CourseName, c.Credits
FROM dbo.Courses c
WHERE c.IsActive = 1
  AND (
        c.CourseType = 'Core'
        OR (c.SpecializationID IS NOT NULL AND c.SpecializationID = @SpecId)
      )
ORDER BY c.CourseCode;";

            DataTable allCourses = Db.Query(sqlCourses,
                new SqlParameter("@StudentID", _plan.StudentId));

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
        }

        public void CalculateTotals()
        {
            // TotalCredits is computed property in CoursePlanProduct
        }

        public CoursePlanProduct GetResult() => _plan;

        private (bool isEligible, string message) CheckEligibility(string studentId, string courseCode)
        {
            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_Check_Course_Eligibility", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_StudentID", studentId);
                cmd.Parameters.AddWithValue("@p_CourseCode", courseCode);

                var pIsEligible = new SqlParameter("@p_IsEligible", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var pMsg = new SqlParameter("@p_Message", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pIsEligible);
                cmd.Parameters.Add(pMsg);

                con.Open();
                cmd.ExecuteNonQuery();

                return (Convert.ToBoolean(pIsEligible.Value), pMsg.Value?.ToString() ?? "");
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SCAIS.Core.Database;

namespace SCAIS.Student.Pages
{
    public partial class StudentEligibleCoursesPage : UserControl
    {
        // Set from session/login
        public string CurrentStudentId { get; set; } = "STU001";

        public StudentEligibleCoursesPage()
        {
            InitializeComponent();
            SetupGrids();
        }

        private void StudentEligibleCoursesPage_Load(object sender, EventArgs e)
        {
            LoadEligibilityTables();
        }
        // =========================
        // UI SETUP
        // =========================
        private void SetupGrids()
        {
            // Eligible Grid
            dgvEligibleCourses.AllowUserToAddRows = false;
            dgvEligibleCourses.ReadOnly = true;
            dgvEligibleCourses.RowHeadersVisible = false;
            dgvEligibleCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEligibleCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEligibleCourses.AutoGenerateColumns = true;

            // Blocked Grid
            dgvBlockedCourses.AllowUserToAddRows = false;
            dgvBlockedCourses.ReadOnly = true;
            dgvBlockedCourses.RowHeadersVisible = false;
            dgvBlockedCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBlockedCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBlockedCourses.AutoGenerateColumns = true;

            // Optional: style cells (green/red badges-like)
            dgvEligibleCourses.CellFormatting += (s, e) =>
            {
                if (dgvEligibleCourses.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    e.CellStyle.BackColor = System.Drawing.Color.Honeydew;
                    e.CellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.Font = new System.Drawing.Font(dgvEligibleCourses.Font, System.Drawing.FontStyle.Bold);
                }
            };

            dgvBlockedCourses.CellFormatting += (s, e) =>
            {
                if (dgvBlockedCourses.Columns[e.ColumnIndex].Name == "ReasonBlocked" && e.Value != null)
                {
                    e.CellStyle.BackColor = System.Drawing.Color.MistyRose;
                    e.CellStyle.ForeColor = System.Drawing.Color.DarkRed;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    e.CellStyle.Font = new System.Drawing.Font(dgvBlockedCourses.Font, System.Drawing.FontStyle.Bold);
                }
            };
        }

        // =========================
        // MAIN LOAD
        // =========================
        public void LoadEligibilityTables()
        {
            if (string.IsNullOrWhiteSpace(CurrentStudentId))
                return;

            // 1) Load all active courses + prereq/coreq strings
            DataTable all = LoadAllActiveCoursesWithReqs();

            // 2) Build result tables
            DataTable dtEligible = BuildEligibleTableSchema();
            DataTable dtBlocked = BuildBlockedTableSchema();

            foreach (DataRow r in all.Rows)
            {
                string code = r["CourseCode"].ToString();
                string name = r["CourseName"].ToString();
                int credits = Convert.ToInt32(r["Credits"]);

                string prereq = r["Prerequisite"].ToString();
                string coreq = r["Corequisite"].ToString();

                var (isEligible, msg) = CheckEligibility(CurrentStudentId, code);

                if (isEligible)
                {
                    dtEligible.Rows.Add(code, name, credits, prereq, coreq, "Eligible");
                }
                else
                {
                    // msg already comes like: "Missing X prerequisite(s)" or "Course already completed"
                    // You can keep it, or make it more friendly:
                    string reason = msg;
                    dtBlocked.Rows.Add(code, name, credits, prereq, reason);
                }
            }

            // 3) Bind grids
            dgvEligibleCourses.DataSource = dtEligible;
            dgvBlockedCourses.DataSource = dtBlocked;

            // 4) Counters
            lblEligibleCountValue.Text = dtEligible.Rows.Count.ToString();
            lblBlockedCountValue.Text = dtBlocked.Rows.Count.ToString();
        }

        // =========================
        // DB HELPERS
        // =========================

        private DataTable LoadAllActiveCoursesWithReqs()
        {
            string sql = @"
DECLARE @SpecId INT;

SELECT @SpecId = SpecializationID
FROM dbo.Students
WHERE StudentID = @StudentID;

SELECT
    c.CourseCode,
    c.CourseName,
    c.Credits,
    ISNULL(pr.PrereqList, 'None') AS Prerequisite,
    ISNULL(co.CoreqList, 'None')  AS Corequisite
FROM dbo.Courses c
LEFT JOIN (
    SELECT CourseCode, STRING_AGG(PrerequisiteCourseCode, ', ') AS PrereqList
    FROM dbo.Prerequisites
    GROUP BY CourseCode
) pr ON pr.CourseCode = c.CourseCode
LEFT JOIN (
    SELECT CourseCode, STRING_AGG(CorequisiteCourseCode, ', ') AS CoreqList
    FROM dbo.Corequisites
    GROUP BY CourseCode
) co ON co.CourseCode = c.CourseCode
WHERE c.IsActive = 1
  AND (
        c.CourseType = 'Core'
        OR (c.SpecializationID IS NOT NULL AND c.SpecializationID = @SpecId)
      )
ORDER BY c.CourseCode;";

            return Db.Query(sql, new SqlParameter("@StudentID", CurrentStudentId));
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

        // =========================
        // TABLE SCHEMAS
        // =========================

        private DataTable BuildEligibleTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CourseCode");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("Credits", typeof(int));
            dt.Columns.Add("Prerequisite");
            dt.Columns.Add("Corequisite");
            dt.Columns.Add("Status");
            return dt;
        }

        private DataTable BuildBlockedTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CourseCode");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("Credits", typeof(int));
            dt.Columns.Add("Prerequisite");
            dt.Columns.Add("ReasonBlocked");
            return dt;
        }

        // Optional: call this from outside when switching student
        public void FocusStudent(string studentId)
        {
            CurrentStudentId = studentId;
            LoadEligibilityTables();
        }

    }
}

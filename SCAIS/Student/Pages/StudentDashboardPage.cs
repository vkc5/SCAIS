using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCAIS.Core.Database;
using System.Data.SqlClient;

namespace SCAIS.Student.Pages
{
    public partial class StudentDashboardPage : UserControl
    {
        // Set this from login/session
        public string CurrentStudentId { get; set; } = "STU001";

        public StudentDashboardPage()
        {
            InitializeComponent();
            SetupGrids();

        }

        private void StudentDashboardPage_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        public void LoadDashboard()
        {
            LoadStudentInfo();
            LoadStats();
            LoadCurrentCourses();
            LoadUpcomingCourses();
        }

        // ---------------- UI SETUP ----------------
        private void SetupGrids()
        {
            // Current Courses grid
            dgvCurrentCourses.AllowUserToAddRows = false;
            dgvCurrentCourses.ReadOnly = true;
            dgvCurrentCourses.RowHeadersVisible = false;
            dgvCurrentCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurrentCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Upcoming Courses grid
            dgvUpcomingCourses.AllowUserToAddRows = false;
            dgvUpcomingCourses.ReadOnly = true;
            dgvUpcomingCourses.RowHeadersVisible = false;
            dgvUpcomingCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUpcomingCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ---------------- 1) STUDENT INFO ----------------
        private void LoadStudentInfo()
        {
            lblStudentIdValue.Text = CurrentStudentId;

            string sql = @"
SELECT
    s.StudentID,
    u.FullName,
    sp.SpecializationName,
    u2.FullName AS AdviserName
FROM dbo.Students s
JOIN dbo.Users u ON u.UserID = s.UserID
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
LEFT JOIN dbo.Advisee_Assignments aa ON aa.StudentID = s.StudentID AND aa.IsActive = 1
LEFT JOIN dbo.Advisers a ON a.AdviserID = aa.AdviserID
LEFT JOIN dbo.Users u2 ON u2.UserID = a.UserID
WHERE s.StudentID = @stu;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));
            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];
            lblStudentNameValue.Text = r["FullName"]?.ToString() ?? "-";
            lblSpecializationValue.Text = r["SpecializationName"]?.ToString() ?? "-";
            lblAdvisorValue.Text = r["AdviserName"]?.ToString() ?? "-";

            // GPA + TotalCredits (from your stored procedure)
            LoadGpaAndTotalCredits();
        }

        private void LoadGpaAndTotalCredits()
        {
            try
            {
                // 1) Run SP (it updates Students table)
                using (SqlConnection con = new SqlConnection(Db.ConnStr))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_Calculate_Student_GPA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_StudentID", CurrentStudentId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                // 2) Read updated values from Students table
                string sql = @"
SELECT CurrentGPA, TotalCreditsEarned
FROM dbo.Students
WHERE StudentID = @stu;";

                DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

                if (dt.Rows.Count == 0)
                {
                    lblGpaValue.Text = "0.00";
                    lblTotalCreditsValue.Text = "0";
                    return;
                }

                var r = dt.Rows[0];
                decimal gpa = (r["CurrentGPA"] != DBNull.Value) ? Convert.ToDecimal(r["CurrentGPA"]) : 0m;
                int credits = (r["TotalCreditsEarned"] != DBNull.Value) ? Convert.ToInt32(r["TotalCreditsEarned"]) : 0;

                lblGpaValue.Text = gpa.ToString("0.00");
                lblTotalCreditsValue.Text = credits.ToString();
            }
            catch (Exception ex)
            {
                lblGpaValue.Text = "0.00";
                lblTotalCreditsValue.Text = "0";
                // optional:
                // MessageBox.Show("GPA load error: " + ex.Message);
            }
        }


        // ---------------- 2) STATS (COMPLETED / IN-PROGRESS / PENDING) ----------------
        private void LoadStats()
        {
            // NOTE: adapt column names if your tables differ.
            // Assumption:
            // - Student_Courses.Status contains: 'Completed', 'In-Progress', 'Pending'
            string sql = @"
SELECT
    SUM(CASE WHEN sc.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedCount,
    SUM(CASE WHEN sc.Status = 'In-Progress' THEN 1 ELSE 0 END) AS InProgressCount,
    SUM(CASE WHEN sc.Status = 'Pending' THEN 1 ELSE 0 END) AS PendingCount
FROM dbo.Student_Courses sc
WHERE sc.StudentID = @stu;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

            int completed = 0, inprog = 0, pending = 0;
            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                completed = (r["CompletedCount"] != DBNull.Value) ? Convert.ToInt32(r["CompletedCount"]) : 0;
                inprog = (r["InProgressCount"] != DBNull.Value) ? Convert.ToInt32(r["InProgressCount"]) : 0;
                pending = (r["PendingCount"] != DBNull.Value) ? Convert.ToInt32(r["PendingCount"]) : 0;
            }

            lblCompletedCount.Text = completed.ToString();
            lblInProgressCount.Text = inprog.ToString();
            lblPendingCount.Text = pending.ToString();
        }

        // ---------------- 3) CURRENT COURSES GRID ----------------
        private void LoadCurrentCourses()
        {
            // Current = In-Progress
            string sql = @"
SELECT
    sc.CourseCode,
    c.CourseName,
    c.Credits,
    sc.Status
FROM dbo.Student_Courses sc
JOIN dbo.Courses c ON c.CourseCode = sc.CourseCode
WHERE sc.StudentID = @stu
  AND sc.Status = 'In-Progress'
ORDER BY sc.CourseCode;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));
            dgvCurrentCourses.DataSource = dt;
        }

        // ---------------- 4) UPCOMING COURSES GRID ----------------
        private void LoadUpcomingCourses()
        {
            string sql = @"
DECLARE @SpecId INT;

SELECT @SpecId = SpecializationID
FROM dbo.Students
WHERE StudentID = @stu;

SELECT
    c.CourseCode,
    c.CourseName,
    c.Credits
FROM dbo.Courses c
WHERE c.IsActive = 1
  AND (
        c.CourseType = 'Core'
        OR c.SpecializationID = @SpecId
      )
  AND NOT EXISTS (
        SELECT 1
        FROM dbo.Student_Courses sc
        WHERE sc.StudentID = @stu
          AND sc.CourseCode = c.CourseCode
          AND sc.[Status] IN ('Completed', 'In-Progress')
  )
ORDER BY c.CourseCode;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));
            dgvUpcomingCourses.DataSource = dt;
        }

    }
}

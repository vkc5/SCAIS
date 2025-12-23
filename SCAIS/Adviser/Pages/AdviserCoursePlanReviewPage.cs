using SCAIS.Core.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserCoursePlanReviewPage : UserControl
    {
        public string CurrentAdviserId { get; set; } = "ADV001"; // static now

        private string _coursePlanId;
        private string _studentId;
        private int _semesterId;

        private DataTable _dtSelected;
        private DataTable _dtEligible;

        public AdviserCoursePlanReviewPage()
        {
            InitializeComponent();
            SetupGrids();
        }

        private void AdviserCoursePlanReviewPage_Load(object sender, EventArgs e)
        {

        }
        private void SetupGrids()
        {
            // Selected
            dgvSelectedCourses.AllowUserToAddRows = false;
            dgvSelectedCourses.ReadOnly = true;
            dgvSelectedCourses.RowHeadersVisible = false;
            dgvSelectedCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSelectedCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Eligible
            dgvEligibleCourses.AllowUserToAddRows = false;
            dgvEligibleCourses.ReadOnly = true;
            dgvEligibleCourses.RowHeadersVisible = false;
            dgvEligibleCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEligibleCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ✅ called from Page 1
        public void LoadPlan(string coursePlanId)
        {
            _coursePlanId = coursePlanId;

            LoadHeaderAndStudent();
            LoadSelectedCourses();
            LoadEligibleCoursesAutoValidated();
        }

        private void LoadHeaderAndStudent()
        {
            string sql = @"
SELECT
    cp.CoursePlanID,
    cp.StudentID,
    u.FullName AS StudentName,
    sp.SpecializationName,
    cp.SemesterID,
    sem.SemesterName,
    cp.TotalCredits,
    cp.Status
FROM dbo.Course_Plans cp
JOIN dbo.Students s ON cp.StudentID = s.StudentID
JOIN dbo.Users u ON s.UserID = u.UserID
LEFT JOIN dbo.Specializations sp ON s.SpecializationID = sp.SpecializationID
JOIN dbo.Semesters sem ON cp.SemesterID = sem.SemesterID
WHERE cp.CoursePlanID = @CoursePlanID;";

            DataTable dt = Db.Query(sql, new SqlParameter("@CoursePlanID", _coursePlanId));
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Plan not found.");
                return;
            }

            var r = dt.Rows[0];
            _studentId = r["StudentID"].ToString();
            _semesterId = Convert.ToInt32(r["SemesterID"]);

            lblStudentNameId.Text = $"Student: {r["StudentName"]} ({_studentId})";
            lblSpecialization.Text = r["SpecializationName"]?.ToString() ?? "-";

            lblTotalCreditsValue.Text = $"{r["TotalCredits"]} credits";
        }

        private void LoadSelectedCourses()
        {
            string sql = @"
SELECT
    c.CourseCode,
    c.CourseName,
    c.Credits
FROM dbo.Course_Plan_Items pi
JOIN dbo.Courses c ON c.CourseCode = pi.CourseCode
WHERE pi.CoursePlanID = @CoursePlanID
ORDER BY c.CourseCode;";

            _dtSelected = Db.Query(sql, new SqlParameter("@CoursePlanID", _coursePlanId));
            dgvSelectedCourses.DataSource = _dtSelected;

            // Optional: add a computed column "Eligibility" later (after validation)
        }

        private void LoadEligibleCoursesAutoValidated()
        {
            // Get all active courses (simple now)
            string sqlCourses = @"
SELECT DISTINCT c.CourseCode, c.CourseName, c.Credits
FROM dbo.Courses c
WHERE c.IsActive = 1
ORDER BY c.CourseCode;";

            DataTable allCourses = Db.Query(sqlCourses);

            // Build eligible table
            _dtEligible = new DataTable();
            _dtEligible.Columns.Add("CourseCode");
            _dtEligible.Columns.Add("CourseName");
            _dtEligible.Columns.Add("Credits", typeof(int));
            _dtEligible.Columns.Add("EligibilityStatus");
            _dtEligible.Columns.Add("Message");

            foreach (DataRow row in allCourses.Rows)
            {
                string courseCode = row["CourseCode"].ToString();
                string courseName = row["CourseName"].ToString();
                int credits = Convert.ToInt32(row["Credits"]);

                var result = CheckEligibility(_studentId, courseCode);

                // show only eligible courses
                if (result.isEligible)
                {
                    _dtEligible.Rows.Add(courseCode, courseName, credits, "Eligible", result.message);
                }
            }

            dgvEligibleCourses.DataSource = _dtEligible;
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

        private void btnApprove_Click(object sender, EventArgs e)
        {
            SaveDecision("Approved");
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            SaveDecision("Rejected");
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Modify Plan will be implemented next step.");
        }
        private void SaveDecision(string status)
        {
            if (string.IsNullOrWhiteSpace(_coursePlanId)) return;

            string remarks = txtAdviserRemarks.Text.Trim();

            // Update Course_Plans
            string updatePlan = @"
UPDATE dbo.Course_Plans
SET Status = @Status,
    AdviserID = @AdviserID,
    ReviewDate = SYSDATETIME()
WHERE CoursePlanID = @CoursePlanID;";

            Db.Query(updatePlan,
                new SqlParameter("@Status", status),
                new SqlParameter("@AdviserID", CurrentAdviserId),
                new SqlParameter("@CoursePlanID", _coursePlanId)
            );

            // Insert feedback (only if remarks not empty)
            if (!string.IsNullOrWhiteSpace(remarks))
            {
                string insertFeedback = @"
INSERT INTO dbo.Adviser_Feedback (CoursePlanID, AdviserID, Remarks, Decision)
VALUES (@CoursePlanID, @AdviserID, @Remarks, @Decision);";

                // Decision column in your table expects: 'Approved','Rejected','Needs Revision'
                string decisionValue = (status == "Approved") ? "Approved" : "Rejected";

                Db.Query(insertFeedback,
                    new SqlParameter("@CoursePlanID", _coursePlanId),
                    new SqlParameter("@AdviserID", CurrentAdviserId),
                    new SqlParameter("@Remarks", remarks),
                    new SqlParameter("@Decision", decisionValue)
                );
            }

            MessageBox.Show($"Plan {status} successfully.");
        }

    }
}

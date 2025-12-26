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
using System.Collections.Generic;
using SCAIS.Core.Builders.CoursePlan;
using SCAIS.Core.Database;
using SCAIS.Core.Models.CoursePlan;

namespace SCAIS.Student.Pages
{
    public partial class StudentSubmitCoursePlanPage : UserControl
    {
        // ====== INPUT (set these from login/session) ======
        public string CurrentStudentId { get; set; } = "STU001";
        public int CurrentSemesterId { get; set; } = 6; // example: Spring 2026

        // ====== Builder Result ======
        private CoursePlanProduct _plan;

        public StudentSubmitCoursePlanPage()
        {
            InitializeComponent();
            SetupGrids();
        }

        private void StudentSubmitCoursePlanPage_Load(object sender, EventArgs e)
        {
            LoadHeaderInfo();      // Semester + Specialization + StudentID
            BuildStudentPlan();    // Director + StudentCoursePlanBuilder
            BindEligibleGrid();    // Fill left grid
            RefreshSelectedGrid(); // Fill right grid (empty at start)
            UpdateTotals();
            if (!CanSubmitNewPlan(out string msg))
            {
                btnSubmit.Enabled = false;
                dgvEligible.Enabled = false;

                MessageBox.Show(msg, "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // ---------------- UI SETUP ----------------
        private void SetupGrids()
        {
            // Eligible grid (left)
            dgvEligible.AllowUserToAddRows = false;
            dgvEligible.RowHeadersVisible = false;
            dgvEligible.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEligible.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEligible.ReadOnly = false;

            // Selected grid (right)
            dgvSelected.AllowUserToAddRows = false;
            dgvSelected.RowHeadersVisible = false;
            dgvSelected.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSelected.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSelected.ReadOnly = true;

            // Add checkbox column "Pick" to eligible grid
            if (dgvEligible.Columns["Pick"] == null)
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = "Pick",
                    HeaderText = "",
                    Width = 40
                };
                dgvEligible.Columns.Insert(0, chk);
            }

            dgvEligible.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvEligible.IsCurrentCellDirty)
                    dgvEligible.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dgvEligible.CellValueChanged += dgvEligible_CellValueChanged;
        }

        // ---------------- HEADER INFO ----------------
        private void LoadHeaderInfo()
        {
            // StudentId label
            lblStudentIdValue.Text = CurrentStudentId;

            // Semester name
            string semSql = "SELECT SemesterName FROM dbo.Semesters WHERE SemesterID = @sid";
            var semDt = Db.Query(semSql, new SqlParameter("@sid", CurrentSemesterId));
            lblSemesterValue.Text = (semDt.Rows.Count > 0) ? semDt.Rows[0]["SemesterName"].ToString() : "-";

            // Specialization name
            string specSql = @"
SELECT sp.SpecializationName
FROM dbo.Students s
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
WHERE s.StudentID = @stu";
            var specDt = Db.Query(specSql, new SqlParameter("@stu", CurrentStudentId));
            lblSpecializationValue.Text = (specDt.Rows.Count > 0) ? (specDt.Rows[0]["SpecializationName"]?.ToString() ?? "-") : "-";
        }

        // ---------------- BUILDER ----------------
        private void BuildStudentPlan()
        {
            string planId = GeneratePlanId();

            var builder = new StudentCoursePlanBuilder();
            var director = new CoursePlanDirector(builder);

            _plan = director.CreatePlan(planId, CurrentStudentId, CurrentSemesterId, "Pending");
        }

        private string GeneratePlanId()
        {
            while (true)
            {
                string id = "CP" + new Random().Next(0, 999999).ToString("D6");

                var dt = Db.Query("SELECT 1 FROM dbo.Course_Plans WHERE CoursePlanID = @id",
                    new SqlParameter("@id", id));

                if (dt.Rows.Count == 0)
                    return id;
            }
        }

        // ---------------- BIND GRIDS ----------------
        private void BindEligibleGrid()
        {
            if (_plan == null) return;

            // Build DataTable for grid
            DataTable dt = new DataTable();
            dt.Columns.Add("CourseCode");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("Credits", typeof(int));

            foreach (var c in _plan.EligibleCourses)
                dt.Rows.Add(c.CourseCode, c.CourseName, c.Credits);

            dgvEligible.DataSource = dt;

            // Lock all columns except Pick
            foreach (DataGridViewColumn col in dgvEligible.Columns)
                col.ReadOnly = col.Name != "Pick";
        }

        private void dgvEligible_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvEligible.Columns[e.ColumnIndex].Name != "Pick") return;

            RefreshSelectedGrid();
            UpdateTotals();
        }

        private void RefreshSelectedGrid()
        {
            if (_plan == null) return;

            // Rebuild selected list from Pick checkboxes
            _plan.SelectedCourses.Clear();

            foreach (DataGridViewRow row in dgvEligible.Rows)
            {
                bool picked = row.Cells["Pick"].Value is true;
                if (!picked) continue;

                string code = row.Cells["CourseCode"].Value.ToString();
                string name = row.Cells["CourseName"].Value.ToString();
                int credits = Convert.ToInt32(row.Cells["Credits"].Value);

                _plan.SelectedCourses.Add(new CoursePlanItem
                {
                    CourseCode = code,
                    CourseName = name,
                    Credits = credits,
                    IsEligible = true,
                    Message = ""
                });
            }

            // Bind selected grid
            DataTable dt = new DataTable();
            dt.Columns.Add("CourseCode");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("Credits", typeof(int));

            foreach (var c in _plan.SelectedCourses)
                dt.Rows.Add(c.CourseCode, c.CourseName, c.Credits);

            dgvSelected.DataSource = dt;
        }

        private void UpdateTotals()
        {
            if (_plan == null) return;
            lblTotalCreditsValue.Text = _plan.TotalCredits.ToString();
        }

        private void SubmitPlanToDb()
        {
            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // 1) Insert Course_Plans
                        using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Course_Plans
(CoursePlanID, StudentID, AdviserID, SemesterID, SubmissionDate, Status, TotalCredits)
VALUES
(@id, @studentId, NULL, @semesterId, SYSDATETIME(), 'Pending', @totalCredits);", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", _plan.CoursePlanId);
                            cmd.Parameters.AddWithValue("@studentId", _plan.StudentId);
                            cmd.Parameters.AddWithValue("@semesterId", _plan.SemesterId);
                            cmd.Parameters.AddWithValue("@totalCredits", _plan.TotalCredits);
                            cmd.ExecuteNonQuery();
                        }

                        // 2) Insert Course_Plan_Items
                        foreach (var item in _plan.SelectedCourses)
                        {
                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Course_Plan_Items (CoursePlanID, CourseCode, Credits, IsEligible)
VALUES (@id, @code, @credits, 1);", con, tx))
                            {
                                cmd.Parameters.AddWithValue("@id", _plan.CoursePlanId);
                                cmd.Parameters.AddWithValue("@code", item.CourseCode);
                                cmd.Parameters.AddWithValue("@credits", item.Credits);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        MessageBox.Show("Plan submitted successfully!");

                        // Optional: lock after submit
                        btnSubmit.Enabled = false;
                        dgvEligible.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        MessageBox.Show("Submit failed: " + ex.Message);
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_plan == null) return;

            if (_plan.SelectedCourses.Count == 0)
            {
                MessageBox.Show("Please select at least 1 course 🙂", "No Courses Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ✅ New rule: allow only if last status is Rejected OR no plan exists
            if (!CanSubmitNewPlan(out string msg))
            {
                MessageBox.Show(msg, "Already Submitted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SubmitPlanToDb();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblSpecializationValue_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private bool CanSubmitNewPlan(out string friendlyMessage)
        {
            friendlyMessage = "";

            string sql = @"
SELECT TOP 1 Status
FROM dbo.Course_Plans
WHERE StudentID = @stu AND SemesterID = @sem
ORDER BY SubmissionDate DESC;";

            DataTable dt = Db.Query(sql,
                new SqlParameter("@stu", CurrentStudentId),
                new SqlParameter("@sem", CurrentSemesterId));

            if (dt.Rows.Count == 0)
                return true; // no plan => allowed

            string lastStatus = dt.Rows[0]["Status"].ToString();

            // Allowed ONLY if last plan is Rejected
            if (lastStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                return true;

            // Otherwise block with friendly message
            if (lastStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                friendlyMessage =
                    "You already have a request in progress ⏳\n" +
                    "Please wait for your adviser’s decision.";
            }
            else if (lastStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                friendlyMessage =
                    "✅ Your course plan is Approved.\n" +
                    "For more details, please go to the Adviser Feedback tab.";
            }
            else if (lastStatus.Equals("Revised", StringComparison.OrdinalIgnoreCase) ||
                     lastStatus.Equals("Needs Revision", StringComparison.OrdinalIgnoreCase))
            {
                friendlyMessage =
                    "⚠️ Your course plan is Revised.\n" +
                    "For more details, please go to the Adviser Feedback tab.";
            }
            else
            {
                friendlyMessage =
                    "You already submitted a course plan.\n" +
                    "Please check Adviser Feedback for details.";
            }

            return false;
        }

    }
}

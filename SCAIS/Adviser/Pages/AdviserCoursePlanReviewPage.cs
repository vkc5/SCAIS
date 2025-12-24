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
using SCAIS.Core.Builders.CoursePlan;
using SCAIS.Core.Models.CoursePlan;
using System.Linq;

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserCoursePlanReviewPage : UserControl
    {
        public event Action BackRequested;

        public string CurrentAdviserId { get; set; } = "ADV001"; // static now
        private string _planStatus;

        private string _coursePlanId;
        private string _studentId;
        private int _semesterId;

        private DataTable _dtSelected;
        private DataTable _dtEligible;

        public AdviserCoursePlanReviewPage()
        {
            InitializeComponent();
            SetupGrids();
            dgvEligibleCourses.CellValueChanged += EligibleCheckboxChanged;
            dgvEligibleCourses.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvEligibleCourses.IsCurrentCellDirty)
                    dgvEligibleCourses.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
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
            dgvEligibleCourses.ReadOnly = false;
            dgvEligibleCourses.RowHeadersVisible = false;
            dgvEligibleCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEligibleCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Checkbox column for selecting eligible courses
            if (dgvEligibleCourses.Columns["Pick"] == null)
            {
                var chk = new DataGridViewCheckBoxColumn();
                chk.Name = "Pick";
                chk.HeaderText = "";
                chk.Width = 40;
                dgvEligibleCourses.Columns.Insert(0, chk);
            }
        }

        // ✅ called from Page 1
        public void LoadPlan(string planId)
        {
            _coursePlanId = planId;

            LoadHeaderAndStudent();     // still OK (UI labels + status + studentId + semesterId)
            LoadRemarksForPlan();       // still OK

            // ✅ Builder Pattern here
            var builder = new AdviserAdjustedPlanBuilder();
            var director = new CoursePlanDirector(builder);

            CoursePlanProduct plan = director.CreatePlan(
                _coursePlanId,
                _studentId,
                _semesterId,
                _planStatus
            );

            // ✅ bind to grids
            dgvSelectedCourses.DataSource = plan.SelectedCourses;
            dgvEligibleCourses.DataSource = plan.EligibleCourses;

            // ✅ show credits
            lblTotalCreditsValue.Text = $"{plan.TotalCredits} credits";

            // ✅ add checkboxes column (already created in SetupGrids)
            MarkAlreadySelectedCheckboxes(plan.SelectedCourses);

            LockEligibleGridExceptCheckbox();

            ApplyEditRules();
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
            _planStatus = r["Status"].ToString();   // Pending / Approved / Rejected ...
            ApplyEditRules();
        }

        private void ApplyEditRules()
        {
            bool isPending = _planStatus == "Pending";

            // Buttons (view mode)
            btnApprove.Visible = isPending;
            btnReject.Visible = isPending;
            btnModify.Visible = isPending;

            btnSave.Visible = false;

            // Lock grids
            dgvEligibleCourses.Enabled = false;
            dgvSelectedCourses.Enabled = false;

            if (dgvEligibleCourses.Columns["Pick"] != null)
                dgvEligibleCourses.Columns["Pick"].Visible = false;

            // 🔒 Remarks NOT editable by default
            txtAdviserRemarks.ReadOnly = true;
        }

        private void SetEditMode(bool enabled)
        {
            dgvEligibleCourses.Enabled = enabled;
            dgvSelectedCourses.Enabled = enabled;

            // if you use checkbox column:
            var col = dgvEligibleCourses.Columns["Pick"];
            if (col != null)
                col.Visible = enabled;
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
            string sqlCourses = @"
SELECT DISTINCT c.CourseCode, c.CourseName, c.Credits
FROM dbo.Courses c
WHERE c.IsActive = 1
ORDER BY c.CourseCode;";

            DataTable allCourses = Db.Query(sqlCourses);

            _dtEligible = new DataTable();
            _dtEligible.Columns.Add("CourseCode");
            _dtEligible.Columns.Add("CourseName");
            _dtEligible.Columns.Add("Credits", typeof(int));
            _dtEligible.Columns.Add("EligibilityStatus");
            _dtEligible.Columns.Add("Message");

            var selectedSet = GetSelectedCourseCodes(); // 🔑

            foreach (DataRow row in allCourses.Rows)
            {
                string courseCode = row["CourseCode"].ToString();
                string courseName = row["CourseName"].ToString();
                int credits = Convert.ToInt32(row["Credits"]);

                var result = CheckEligibility(_studentId, courseCode);

                if (result.isEligible)
                {
                    _dtEligible.Rows.Add(courseCode, courseName, credits, "Eligible", result.message);
                }
            }

            dgvEligibleCourses.DataSource = _dtEligible;

            // 🔑 CHECK already selected courses
            foreach (DataGridViewRow row in dgvEligibleCourses.Rows)
            {
                string code = row.Cells["CourseCode"].Value.ToString();
                if (selectedSet.Contains(code))
                {
                    row.Cells["Pick"].Value = true;
                }
            }

            LockEligibleGridExceptCheckbox();
        }

        private void LockEligibleGridExceptCheckbox()
        {
            foreach (DataGridViewColumn col in dgvEligibleCourses.Columns)
            {
                col.ReadOnly = col.Name != "Pick";
            }
        }
        private HashSet<string> GetSelectedCourseCodes()
        {
            return _dtSelected
                .AsEnumerable()
                .Select(r => r["CourseCode"].ToString())
                .ToHashSet();
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
            if (_planStatus != "Pending") return;
            if (btnSave.Visible) return; // means edit mode is on

            SaveDecision("Approved");
            _planStatus = "Approved";
            ApplyEditRules();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (_planStatus != "Pending") return;
            if (btnSave.Visible) return; // edit mode is on

            SaveDecision("Rejected");
            _planStatus = "Rejected";
            ApplyEditRules();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (_planStatus != "Pending") return;

            dgvEligibleCourses.Enabled = true;
            dgvSelectedCourses.Enabled = true;

            if (dgvEligibleCourses.Columns["Pick"] != null)
                dgvEligibleCourses.Columns["Pick"].Visible = true;

            // hide other buttons
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnModify.Visible = false;

            // show save
            btnSave.Visible = true;

            // ✅ NOW remarks become editable
            txtAdviserRemarks.ReadOnly = false;

            RefreshSelectedPreview();
        }

        private void SaveDecision(string status)
        {
            if (string.IsNullOrWhiteSpace(_coursePlanId)) return;

            string remarks = txtAdviserRemarks.Text.Trim();

            // 1) Update Course_Plans
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

            // 2) UPSERT feedback per (CoursePlanID + AdviserID)
            if (!string.IsNullOrWhiteSpace(remarks))
            {
                string decisionValue =
                    (status == "Approved") ? "Approved" :
                    (status == "Rejected") ? "Rejected" :
                    "Needs Revision"; // Revised

                string upsertFeedback = @"
IF EXISTS (SELECT 1 FROM dbo.Adviser_Feedback
           WHERE CoursePlanID = @CoursePlanID AND AdviserID = @AdviserID)
BEGIN
    UPDATE dbo.Adviser_Feedback
    SET Remarks = @Remarks,
        Decision = @Decision,
        FeedbackDate = SYSDATETIME()
    WHERE CoursePlanID = @CoursePlanID AND AdviserID = @AdviserID;
END
ELSE
BEGIN
    INSERT INTO dbo.Adviser_Feedback (CoursePlanID, AdviserID, Remarks, Decision)
    VALUES (@CoursePlanID, @AdviserID, @Remarks, @Decision);
END";

                Db.Query(upsertFeedback,
                    new SqlParameter("@CoursePlanID", _coursePlanId),
                    new SqlParameter("@AdviserID", CurrentAdviserId),
                    new SqlParameter("@Remarks", remarks),
                    new SqlParameter("@Decision", decisionValue)
                );
            }

            MessageBox.Show($"Plan {status} successfully.");
        }
        private void SaveSelectedCoursesFromCheckboxes()
        {
            // Create list of selected course codes from Eligible grid (Pick = true)
            var chosen = new System.Collections.Generic.List<string>();
            int totalCredits = 0;

            foreach (DataGridViewRow row in dgvEligibleCourses.Rows)
            {
                bool picked = row.Cells["Pick"].Value != null && (bool)row.Cells["Pick"].Value;
                if (!picked) continue;

                string code = row.Cells["CourseCode"].Value.ToString();
                int credits = Convert.ToInt32(row.Cells["Credits"].Value);

                chosen.Add(code);
                totalCredits += credits;
            }

            // Save into DB
            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // 1) delete old items
                        using (SqlCommand del = new SqlCommand(
                            "DELETE FROM dbo.Course_Plan_Items WHERE CoursePlanID=@id", con, tx))
                        {
                            del.Parameters.AddWithValue("@id", _coursePlanId);
                            del.ExecuteNonQuery();
                        }

                        // 2) insert new items
                        foreach (string code in chosen)
                        {
                            using (SqlCommand ins = new SqlCommand(@"
INSERT INTO dbo.Course_Plan_Items (CoursePlanID, CourseCode, Credits, IsEligible)
SELECT @id, c.CourseCode, c.Credits, 1
FROM dbo.Courses c
WHERE c.CourseCode = @code;", con, tx))
                            {
                                ins.Parameters.AddWithValue("@id", _coursePlanId);
                                ins.Parameters.AddWithValue("@code", code);
                                ins.ExecuteNonQuery();
                            }
                        }

                        // 3) update totals
                        using (SqlCommand up = new SqlCommand(@"
UPDATE dbo.Course_Plans
SET TotalCredits = @t
WHERE CoursePlanID=@id;", con, tx))
                        {
                            up.Parameters.AddWithValue("@t", totalCredits);
                            up.Parameters.AddWithValue("@id", _coursePlanId);
                            up.ExecuteNonQuery();
                        }

                        tx.Commit();

                        // update UI
                        lblTotalCreditsValue.Text = $"{totalCredits} credits";
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
        private string GetPlanStatusFromDB(string planId)
        {
            string status = "Pending"; // default safety

            string sql = @"
        SELECT [Status]
        FROM Course_Plans
        WHERE CoursePlanID = @planId
    ";

            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@planId", planId);
                con.Open();

                object result = cmd.ExecuteScalar();
                if (result != null)
                    status = result.ToString();
            }

            return status;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BackRequested?.Invoke();

        }
        private void EligibleCheckboxChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;
            if (dgvEligibleCourses.Columns[e.ColumnIndex].Name != "Pick") return;

            RefreshSelectedPreview();
        }

        private void RefreshSelectedPreview()
        {
            var preview = new DataTable();
            preview.Columns.Add("CourseCode");
            preview.Columns.Add("CourseName");
            preview.Columns.Add("Credits", typeof(int));

            int total = 0;

            foreach (DataGridViewRow row in dgvEligibleCourses.Rows)
            {
                bool picked = row.Cells["Pick"].Value is true;
                if (!picked) continue;

                string code = row.Cells["CourseCode"].Value.ToString();
                string name = row.Cells["CourseName"].Value.ToString();
                int credits = Convert.ToInt32(row.Cells["Credits"].Value);

                preview.Rows.Add(code, name, credits);
                total += credits;
            }

            dgvSelectedCourses.DataSource = preview;
            lblTotalCreditsValue.Text = $"{total} credits";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_planStatus != "Pending") return;

            SaveSelectedCoursesFromCheckboxes();

            // Set status Revised + insert feedback ("Needs Revision")
            SaveDecision("Revised");

            _planStatus = "Revised";
            btnSave.Visible = false;

            // lock everything (Revised => no buttons)
            ApplyEditRules();

            // reload UI from DB (recommended)
            LoadSelectedCourses();
            LoadEligibleCoursesAutoValidated();

            MessageBox.Show("Saved successfully. Status changed to Revised.");
        }
        private void LoadRemarksForPlan()
        {
            string sql = @"
SELECT TOP 1 Remarks
FROM dbo.Adviser_Feedback
WHERE CoursePlanID = @CoursePlanID AND AdviserID = @AdviserID
ORDER BY FeedbackDate DESC;";

            DataTable dt = Db.Query(sql,
                new SqlParameter("@CoursePlanID", _coursePlanId),
                new SqlParameter("@AdviserID", CurrentAdviserId));

            txtAdviserRemarks.Text = (dt.Rows.Count > 0)
                ? dt.Rows[0]["Remarks"].ToString()
                : "";
        }
        private void MarkAlreadySelectedCheckboxes(List<CoursePlanItem> selected)
        {
            var selectedSet = selected.Select(x => x.CourseCode).ToHashSet();

            foreach (DataGridViewRow row in dgvEligibleCourses.Rows)
            {
                if (row.Cells["CourseCode"].Value == null) continue;

                string code = row.Cells["CourseCode"].Value.ToString();
                if (selectedSet.Contains(code))
                    row.Cells["Pick"].Value = true;
            }
        }
    }

}

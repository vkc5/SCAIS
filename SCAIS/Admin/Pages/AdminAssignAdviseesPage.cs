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

namespace SCAIS.Admin.Pages
{
    public partial class AdminAssignAdviseesPage : UserControl
    {
        private DataTable _studentsDt;

        public AdminAssignAdviseesPage()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void AdminAssignAdviseesPage_Load(object sender, EventArgs e)
        {
            LoadAdvisers();
            LoadStudents();
        }

        // ===================== UI SETUP =====================
        private void SetupGrid()
        {
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvStudents.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvStudents.IsCurrentCellDirty)
                    dgvStudents.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dgvStudents.CellValueChanged += dgvStudents_CellValueChanged;
        }

        // ===================== 1) LOAD ADVISERS =====================
        private void LoadAdvisers()
        {
            string sql = @"
SELECT
    a.AdviserID,
    u.FullName
FROM dbo.Advisers a
JOIN dbo.Users u ON u.UserID = a.UserID
WHERE u.Status = 'Active'
ORDER BY u.FullName;";

            DataTable dt = Db.Query(sql);

            cmbAdvisers.DisplayMember = "FullName";
            cmbAdvisers.ValueMember = "AdviserID";
            cmbAdvisers.DataSource = dt;
            cmbAdvisers.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
        }

        // ===================== 2) LOAD STUDENTS GRID =====================
        private void LoadStudents()
        {
            string sql = @"
SELECT
    s.StudentID,
    u.FullName AS StudentName,
    sp.SpecializationName,
    CASE WHEN aa.StudentID IS NULL THEN 0 ELSE 1 END AS HasAdviser,
    CASE WHEN aa.StudentID IS NULL THEN 'Available' ELSE 'Already Assigned' END AS StatusText
FROM dbo.Students s
JOIN dbo.Users u ON u.UserID = s.UserID
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
LEFT JOIN dbo.Advisee_Assignments aa 
    ON aa.StudentID = s.StudentID AND aa.IsActive = 1
ORDER BY s.StudentID;";

            _studentsDt = Db.Query(sql);

            // Add a hidden "WasAssigned" column to know original state
            if (!_studentsDt.Columns.Contains("WasAssigned"))
                _studentsDt.Columns.Add("WasAssigned", typeof(int));

            foreach (DataRow r in _studentsDt.Rows)
                r["WasAssigned"] = Convert.ToInt32(r["HasAdviser"]);

            dgvStudents.Columns.Clear();
            dgvStudents.DataSource = _studentsDt;

            // Replace HasAdviser with a checkbox column
            int hasIndex = dgvStudents.Columns["HasAdviser"].Index;
            dgvStudents.Columns.Remove("HasAdviser");

            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "HasAdviser",
                HeaderText = "Select",
                DataPropertyName = "HasAdviser",
                Width = 60
            };
            dgvStudents.Columns.Insert(0, chk);

            // Friendly column headers
            dgvStudents.Columns["StudentID"].HeaderText = "Student ID";
            dgvStudents.Columns["StudentName"].HeaderText = "Student Name";
            dgvStudents.Columns["SpecializationName"].HeaderText = "Specialization";
            dgvStudents.Columns["StatusText"].HeaderText = "Status";

            // Hide helper column
            dgvStudents.Columns["WasAssigned"].Visible = false;

            // Make text columns read-only
            dgvStudents.Columns["StudentID"].ReadOnly = true;
            dgvStudents.Columns["StudentName"].ReadOnly = true;
            dgvStudents.Columns["SpecializationName"].ReadOnly = true;
            dgvStudents.Columns["StatusText"].ReadOnly = true;

            StyleStatusCells();
        }

        // When admin checks/unchecks, update StatusText visually
        private void dgvStudents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvStudents.Columns[e.ColumnIndex].Name != "HasAdviser") return;

            var row = dgvStudents.Rows[e.RowIndex];
            bool nowChecked = IsChecked(row.Cells["HasAdviser"].Value);

            row.Cells["StatusText"].Value = nowChecked ? "Selected" : "Not Selected";

            // But if it was originally assigned, keep a clearer hint
            int wasAssigned = Convert.ToInt32(row.Cells["WasAssigned"].Value);
            if (wasAssigned == 1 && nowChecked)
                row.Cells["StatusText"].Value = "Already Assigned";
            else if (wasAssigned == 1 && !nowChecked)
                row.Cells["StatusText"].Value = "Will Be Removed";

            StyleStatusCells();
        }

        private void StyleStatusCells()
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                string status = row.Cells["StatusText"].Value?.ToString() ?? "";
                var cell = row.Cells["StatusText"];

                cell.Style.ForeColor = Color.Black;
                cell.Style.BackColor = Color.White;

                if (status == "Already Assigned")
                    cell.Style.BackColor = Color.FromArgb(235, 235, 235);
                else if (status == "Available")
                    cell.Style.BackColor = Color.FromArgb(220, 255, 230);
                else if (status == "Will Be Removed")
                    cell.Style.BackColor = Color.FromArgb(255, 230, 230);
                else if (status == "Selected")
                    cell.Style.BackColor = Color.FromArgb(220, 240, 255);
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            if (cmbAdvisers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an adviser first 🙂", "Select Adviser",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string adviserId = cmbAdvisers.SelectedValue.ToString();
            int successCount = 0;
            int skippedAssigned = 0;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                bool checkedNow = IsChecked(row.Cells["HasAdviser"].Value);
                if (!checkedNow) continue;

                int wasAssigned = Convert.ToInt32(row.Cells["WasAssigned"].Value);
                string studentId = row.Cells["StudentID"].Value.ToString();

                // Validation: cannot assign if already has adviser
                if (wasAssigned == 1)
                {
                    skippedAssigned++;
                    continue;
                }

                if (AssignStudentToAdviser(studentId, adviserId, out string msg))
                {
                    successCount++;
                }
                else
                {
                    MessageBox.Show($"Student {studentId}: {msg}", "Assign Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            LoadStudents();

            MessageBox.Show(
                $"Assigned: {successCount}\nSkipped (Already Assigned): {skippedAssigned}",
                "Done",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

        }

        private bool AssignStudentToAdviser(string studentId, string adviserId, out string message)
        {
            message = "";

            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_Assign_Student_To_Adviser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_StudentID", studentId);
                cmd.Parameters.AddWithValue("@p_AdviserID", adviserId);

                var pSuccess = new SqlParameter("@p_Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var pMsg = new SqlParameter("@p_Message", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pSuccess);
                cmd.Parameters.Add(pMsg);

                con.Open();
                cmd.ExecuteNonQuery();

                bool ok = Convert.ToBoolean(pSuccess.Value);
                message = pMsg.Value?.ToString() ?? "";

                return ok;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int removed = 0;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                bool checkedNow = IsChecked(row.Cells["HasAdviser"].Value);
                int wasAssigned = Convert.ToInt32(row.Cells["WasAssigned"].Value);

                // Remove rule:
                // was assigned (1) AND now unchecked => remove
                if (wasAssigned == 1 && !checkedNow)
                {
                    string studentId = row.Cells["StudentID"].Value.ToString();
                    if (RemoveStudentAdviser(studentId))
                        removed++;
                }
            }

            LoadStudents();

            MessageBox.Show(
                removed == 0
                    ? "No students were removed. Uncheck assigned students first, then click Remove 🙂"
                    : $"Removed adviser from {removed} student(s).",
                "Remove Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

        }

        private bool RemoveStudentAdviser(string studentId)
        {
            string sql = @"
UPDATE dbo.Advisee_Assignments
SET IsActive = 0,
    EndDate = CAST(GETDATE() AS date)
WHERE StudentID = @stu AND IsActive = 1;";

            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConnStr))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@stu", studentId);
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnHowItWorks_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
    "How Assigning Advisees Works\n\n" +
    "1️⃣ Select an Adviser\n" +
    "Choose an adviser from the drop-down list at the top.\n\n" +

    "2️⃣ Student List\n" +
    "• Each row represents one student.\n" +
    "• ✔ Checked = student currently has an adviser.\n" +
    "• ⬜ Unchecked = student has no adviser.\n\n" +

    "3️⃣ Assign Students\n" +
    "• Check students who DO NOT have an adviser.\n" +
    "• Click \"Assign\" to assign them to the selected adviser.\n\n" +

    "⚠️ Students who already have an adviser cannot be assigned again.\n\n" +

    "4️⃣ Remove Adviser\n" +
    "• Uncheck students who currently have an adviser.\n" +
    "• Click \"Remove\" to remove their adviser assignment.\n\n" +

    "5️⃣ Status\n" +
    "• Available → Student has no adviser\n" +
    "• Already Assigned → Student already has an adviser\n" +
    "• Will Be Removed → Adviser will be removed after clicking Remove\n\n" +

    "✔ Changes take effect immediately after clicking Assign or Remove.",
    "How It Works",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
            );

        }
        private bool IsChecked(object cellValue)
        {
            if (cellValue == null || cellValue == DBNull.Value) return false;

            if (cellValue is bool b) return b;

            // for 0/1 stored as int/string
            if (int.TryParse(cellValue.ToString(), out int i))
                return i == 1;

            // fallback
            if (bool.TryParse(cellValue.ToString(), out bool bb))
                return bb;

            return false;
        }

    }
}

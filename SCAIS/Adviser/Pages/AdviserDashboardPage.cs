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

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserDashboardPage : UserControl
    {
        // TODO: set this after login (logged-in adviser)
        public string CurrentAdviserId { get; set; } = "ADV001";
        public Action<string> ViewStudentRequested { get; set; }

        public AdviserDashboardPage()
        {
            InitializeComponent();
            SetupGrid();
            LoadSpecializations();
            LoadAdvisees(); // initial load

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void SetupGrid()
        {
            dgvAdvisees.AllowUserToAddRows = false;
            dgvAdvisees.ReadOnly = true;
            dgvAdvisees.RowHeadersVisible = false;
            dgvAdvisees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdvisees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvAdvisees.Columns.Clear();
            dgvAdvisees.Columns.Add("StudentID", "Student ID");
            dgvAdvisees.Columns.Add("FullName", "Name");
            dgvAdvisees.Columns.Add("SpecializationName", "Specialization");
            dgvAdvisees.Columns.Add("CurrentGPA", "GPA");
            dgvAdvisees.Columns.Add("Status", "Status");

            var btn = new DataGridViewButtonColumn();
            btn.Name = "View";
            btn.HeaderText = "Actions";
            btn.Text = "View";
            btn.UseColumnTextForButtonValue = true;
            dgvAdvisees.Columns.Add(btn);

            dgvAdvisees.CellContentClick += dgvAdvisees_CellContentClick;
        }

        private void LoadSpecializations()
        {
            cmbSpecialization.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpecialization.Items.Clear();
            cmbSpecialization.Items.Add("All Specializations");

            DataTable dt = Db.Query("SELECT SpecializationName FROM dbo.Specializations WHERE IsActive = 1 ORDER BY SpecializationName");
            foreach (DataRow row in dt.Rows)
                cmbSpecialization.Items.Add(row["SpecializationName"].ToString());

            cmbSpecialization.SelectedIndex = 0;
        }

        private void LoadAdvisees()
        {
            string search = txtSearch.Text.Trim();
            string spec = cmbSpecialization.SelectedItem == null ? "All Specializations" : cmbSpecialization.SelectedItem.ToString();

            string sql = @"
SELECT 
    s.StudentID,
    u.FullName,
    ISNULL(sp.SpecializationName, 'N/A') AS SpecializationName,
    s.CurrentGPA,
    CASE WHEN s.CurrentGPA >= 3.50 THEN 'On Track' ELSE 'Needs Review' END AS Status
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s ON aa.StudentID = s.StudentID
JOIN dbo.Users u ON s.UserID = u.UserID
LEFT JOIN dbo.Specializations sp ON s.SpecializationID = sp.SpecializationID
WHERE aa.AdviserID = @AdviserID
  AND aa.IsActive = 1
  AND (
        @Search = '' 
        OR s.StudentID LIKE '%' + @Search + '%'
        OR u.FullName LIKE '%' + @Search + '%'
      )
  AND (
        @Spec = 'All Specializations'
        OR sp.SpecializationName = @Spec
      )
ORDER BY u.FullName;
";

            DataTable dt = Db.Query(sql,
                new SqlParameter("@AdviserID", CurrentAdviserId),
                new SqlParameter("@Search", search),
                new SqlParameter("@Spec", spec)
            );

            dgvAdvisees.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                dgvAdvisees.Rows.Add(
                    r["StudentID"].ToString(),
                    r["FullName"].ToString(),
                    r["SpecializationName"].ToString(),
                    r["CurrentGPA"].ToString(),
                    r["Status"].ToString()
                );
            }

            UpdateSummaryCards();
        }

        private void UpdateSummaryCards()
        {
            string sql = @"
SELECT
    COUNT(*) AS TotalAdvisees,
    SUM(CASE WHEN s.CurrentGPA >= 3.50 THEN 1 ELSE 0 END) AS OnTrack,
    SUM(CASE WHEN s.CurrentGPA < 3.50 THEN 1 ELSE 0 END) AS PendingReviews
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s ON aa.StudentID = s.StudentID
WHERE aa.AdviserID = @AdviserID AND aa.IsActive = 1;
";

            DataTable dt = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));

            if (dt.Rows.Count > 0)
            {
                lblTotalValue.Text = dt.Rows[0]["TotalAdvisees"].ToString();
                lblOnTrackValue.Text = dt.Rows[0]["OnTrack"].ToString();
                lblPendingValue.Text = dt.Rows[0]["PendingReviews"].ToString();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadAdvisees();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Optional: auto filter while typing
            // LoadAdvisees();
        }

        private void dgvAdvisees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvAdvisees.Columns[e.ColumnIndex].Name == "View")
            {
                string studentId = dgvAdvisees.Rows[e.RowIndex].Cells["StudentID"].Value.ToString();

                if (ViewStudentRequested != null)
                    ViewStudentRequested(studentId);
            }
        }

    }

}


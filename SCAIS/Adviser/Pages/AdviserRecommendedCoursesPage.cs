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

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserRecommendedCoursesPage : UserControl
    {
        public string CurrentAdviserId { get; set; } = "ADV001"; // static for now

        // This is the navigation callback to open Page 2 later
        public Action<string> ViewPlanRequested { get; set; }  // sends CoursePlanID

        private DataTable _dt;

        public AdviserRecommendedCoursesPage()
        {
            InitializeComponent();
            SetupGrid();
            SetupFilters();
        }

        private void AdviserRecommendedCoursesPage_Load(object sender, EventArgs e)
        {
            LoadPlans();
        }
        private void AdviserCoursePlansPage_Load(object sender, EventArgs e)
        {
            LoadPlans();
        }

        public void LoadPlans()
        {
            string sql = @"
SELECT
    cp.CoursePlanID,
    cp.StudentID,
    u.FullName AS StudentName,
    sem.SemesterName,
    cp.SubmissionDate,
    cp.TotalCredits,
    cp.Status
FROM dbo.Course_Plans cp
JOIN dbo.Students s ON cp.StudentID = s.StudentID
JOIN dbo.Users u ON s.UserID = u.UserID
JOIN dbo.Semesters sem ON cp.SemesterID = sem.SemesterID
JOIN dbo.Advisee_Assignments aa ON aa.StudentID = cp.StudentID AND aa.IsActive = 1
WHERE aa.AdviserID = @AdviserID
ORDER BY cp.SubmissionDate DESC;";

            _dt = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));
            ApplyFilters();
        }

        private void SetupGrid()
        {
            dgvPlans.AllowUserToAddRows = false;
            dgvPlans.ReadOnly = true;
            dgvPlans.RowHeadersVisible = false;
            dgvPlans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvPlans.AutoGenerateColumns = true;

            // Add View button column
            var btn = new DataGridViewButtonColumn();
            btn.Name = "View";
            btn.HeaderText = "Action";
            btn.Text = "View";
            btn.UseColumnTextForButtonValue = true;
            dgvPlans.Columns.Add(btn);

            dgvPlans.CellContentClick += dgvPlans_CellContentClick;
        }

        private void SetupFilters()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("All");
            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Approved");
            cmbStatus.Items.Add("Rejected");
            cmbStatus.Items.Add("Revised");
            cmbStatus.SelectedIndex = 0;

            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_dt == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            string status = cmbStatus.SelectedItem?.ToString() ?? "All";

            string filter = "";

            if (!string.IsNullOrWhiteSpace(search))
            {
                filter += $"(StudentID LIKE '%{search}%' OR StudentName LIKE '%{search}%')";
            }

            if (status != "All")
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"Status = '{status}'";
            }

            DataView dv = _dt.DefaultView;
            dv.RowFilter = filter;
            dgvPlans.DataSource = dv;
        }
        private void dgvPlans_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvPlans.Columns[e.ColumnIndex].Name == "View")
            {
                string planId = dgvPlans.Rows[e.RowIndex].Cells["CoursePlanID"].Value.ToString();

                if (ViewPlanRequested != null)
                    ViewPlanRequested(planId);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPlans();
        }
    }
}

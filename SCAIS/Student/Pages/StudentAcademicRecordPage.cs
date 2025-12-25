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
using System.IO;
using SCAIS.Core.Database;

namespace SCAIS.Student.Pages
{
    public partial class StudentAcademicRecordPage : UserControl
    {
        // Set this from login/session
        public string CurrentStudentId { get; set; } = "STU001";

        private DataTable _dtHistory;   // full history (before filter)

        public StudentAcademicRecordPage()
        {
            InitializeComponent();
            SetupGrid();
            SetupFilters();
        }

        private void StudentAcademicRecordPage_Load(object sender, EventArgs e)
        {
            LoadHeaderSummary();
            LoadAcademicHistory();
            ApplyFilters();
        }
        private void LoadHeaderSummary()
        {
            string sql = @"
SELECT 
    StudentID,
    FullName,
    SpecializationName,
    CurrentGPA,
    TotalCreditsEarned
FROM dbo.vw_Student_Academic_Summary
WHERE StudentID = @stu;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

            if (dt.Rows.Count == 0)
            {
                // fallback if view returns nothing
                lblStudentIdValue.Text = CurrentStudentId;
                lblStudentNameValue.Text = "-";
                lblSpecializationValue.Text = "-";
                lblCurrentGpaValue.Text = "0.00";
                lblCreditsEarnedValue.Text = "0";
                lblCreditsRequiredValue.Text = "120";
                return;
            }

            var r = dt.Rows[0];

            lblStudentIdValue.Text = r["StudentID"].ToString();
            lblStudentNameValue.Text = r["FullName"].ToString();
            lblSpecializationValue.Text = r["SpecializationName"]?.ToString() ?? "-";
            lblCurrentGpaValue.Text = Convert.ToDecimal(r["CurrentGPA"]).ToString("0.00");
            lblCreditsEarnedValue.Text = r["TotalCreditsEarned"].ToString();

            // Credits required from Specializations.RequiredCredits
            string sqlReq = @"
SELECT ISNULL(sp.RequiredCredits, 120)
FROM dbo.Students s
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
WHERE s.StudentID = @stu;";

            DataTable reqDt = Db.Query(sqlReq, new SqlParameter("@stu", CurrentStudentId));
            lblCreditsRequiredValue.Text = (reqDt.Rows.Count > 0) ? reqDt.Rows[0][0].ToString() : "120";
        }

        // ===================== GRID =====================
        private void SetupGrid()
        {
            dgvAcademicHistory.AllowUserToAddRows = false;
            dgvAcademicHistory.ReadOnly = true;
            dgvAcademicHistory.RowHeadersVisible = false;
            dgvAcademicHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAcademicHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAcademicHistory.MultiSelect = false;
            dgvAcademicHistory.AutoGenerateColumns = true;
        }

        private void LoadAcademicHistory()
        {
            string sql = @"
SELECT 
    (c.CourseCode + ' - ' + c.CourseName) AS Course,
    sem.SemesterName AS Semester,
    sc.[Status] AS Status,
    ISNULL(sc.Grade, '-') AS Grade
FROM dbo.Student_Courses sc
JOIN dbo.Courses c ON c.CourseCode = sc.CourseCode
JOIN dbo.Semesters sem ON sem.SemesterID = sc.SemesterID
WHERE sc.StudentID = @stu
ORDER BY sem.[Year], 
         CASE sem.Term WHEN 'Spring' THEN 1 WHEN 'Summer' THEN 2 WHEN 'Fall' THEN 3 END,
         c.CourseCode;";

            _dtHistory = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

            dgvAcademicHistory.DataSource = _dtHistory;
            ColorStatusCells();
        }

        private void ColorStatusCells()
        {
            if (!dgvAcademicHistory.Columns.Contains("Status")) return;

            foreach (DataGridViewRow row in dgvAcademicHistory.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                // Soft coloring like your screenshot
                if (status == "Completed") row.Cells["Status"].Style.BackColor = System.Drawing.Color.Honeydew;
                else if (status == "In-Progress") row.Cells["Status"].Style.BackColor = System.Drawing.Color.LemonChiffon;
                else if (status == "Pending") row.Cells["Status"].Style.BackColor = System.Drawing.Color.MistyRose;
                else row.Cells["Status"].Style.BackColor = System.Drawing.Color.White;
            }
        }

        // ===================== FILTERS =====================
        private void SetupFilters()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("All");
            cmbStatus.Items.Add("Completed");
            cmbStatus.Items.Add("In-Progress");
            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Dropped");
            cmbStatus.Items.Add("Failed");
            cmbStatus.SelectedIndex = 0;

            btnFilter.Click += (s, e) => ApplyFilters();

            // Optional live filtering:
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_dtHistory == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            string status = cmbStatus.SelectedItem?.ToString() ?? "All";

            string filter = "";

            if (!string.IsNullOrWhiteSpace(search))
            {
                filter += $"(Course LIKE '%{search}%' OR Semester LIKE '%{search}%')";
            }

            if (status != "All")
            {
                if (filter.Length > 0) filter += " AND ";
                filter += $"Status = '{status}'";
            }

            DataView dv = _dtHistory.DefaultView;
            dv.RowFilter = filter;
            dgvAcademicHistory.DataSource = dv;

            ColorStatusCells();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvAcademicHistory.Rows.Count == 0)
            {
                MessageBox.Show("Nothing to export.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV file (*.csv)|*.csv";
                sfd.FileName = $"AcademicHistory_{CurrentStudentId}.csv";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    ExportGridToCsv(dgvAcademicHistory, sfd.FileName);
                    MessageBox.Show("Exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export failed: " + ex.Message);
                }
            }
        }
        private void ExportGridToCsv(DataGridView grid, string filePath)
        {
            var sb = new StringBuilder();

            // headers
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                sb.Append(EscapeCsv(grid.Columns[i].HeaderText));
                if (i < grid.Columns.Count - 1) sb.Append(",");
            }
            sb.AppendLine();

            // rows
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;

                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    string val = row.Cells[i].Value?.ToString() ?? "";
                    sb.Append(EscapeCsv(val));
                    if (i < grid.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private string EscapeCsv(string input)
        {
            if (input == null) return "";
            bool mustQuote = input.Contains(",") || input.Contains("\"") || input.Contains("\n");
            input = input.Replace("\"", "\"\"");
            return mustQuote ? $"\"{input}\"" : input;
        }


    }
}

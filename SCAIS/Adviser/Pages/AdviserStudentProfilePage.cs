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
using System.IO;
using System.Text;

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserStudentProfilePage : UserControl
    {
        public event Action<string> RecommendCoursesRequested;
        private string _studentId; // store current loaded student

        // Student to show
        public string CurrentStudentId { get; private set; }

        // Callback for Back (we call it from AdviserMainForm)
        public Action BackRequested { get; set; }

        public AdviserStudentProfilePage()
        {
            InitializeComponent();
            SetupGrid();

        }
        public void LoadStudent(string studentId)
        {
            CurrentStudentId = studentId;
            _studentId = studentId;

            LoadStudentHeader();
            LoadAcademicHistory();
        }

        private void SetupGrid()
        {
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add("CourseName", "Course Name");
            dgvHistory.Columns.Add("Semester", "Semester");
            dgvHistory.Columns.Add("Status", "Status");
            dgvHistory.Columns.Add("Grade", "Grade");

            dgvHistory.CellFormatting += dgvHistory_CellFormatting;
        }
        private void LoadStudentHeader()
        {
            string sql = @"
SELECT 
    s.StudentID,
    u.FullName,
    ISNULL(sp.SpecializationName, 'N/A') AS SpecializationName,
    s.CurrentGPA,
    s.TotalCreditsEarned,
    ISNULL(s.CurrentSemester, 'N/A') AS CurrentSemester
FROM dbo.Students s
JOIN dbo.Users u ON s.UserID = u.UserID
LEFT JOIN dbo.Specializations sp ON s.SpecializationID = sp.SpecializationID
WHERE s.StudentID = @StudentID;
";

            DataTable dt = Db.Query(sql, new SqlParameter("@StudentID", CurrentStudentId));
            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];

            lblStudentIdValue.Text = r["StudentID"].ToString();
            lblNameValue.Text = r["FullName"].ToString();
            lblSpecValue.Text = r["SpecializationName"].ToString();
            lblGpaValue.Text = Convert.ToDecimal(r["CurrentGPA"]).ToString("0.00");
            lblCreditsValue.Text = r["TotalCreditsEarned"].ToString();
            lblSemesterValue.Text = r["CurrentSemester"].ToString();
        }

        private void LoadAcademicHistory()
        {
            string sql = @"
SELECT
    c.CourseCode + ' - ' + c.CourseName AS CourseName,
    sem.SemesterName AS Semester,
    sc.[Status],
    ISNULL(sc.Grade, '-') AS Grade
FROM dbo.Student_Courses sc
JOIN dbo.Courses c ON sc.CourseCode = c.CourseCode
JOIN dbo.Semesters sem ON sc.SemesterID = sem.SemesterID
WHERE sc.StudentID = @StudentID
ORDER BY sem.[Year], sem.Term, c.CourseCode;
";

            DataTable dt = Db.Query(sql, new SqlParameter("@StudentID", CurrentStudentId));

            dgvHistory.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                dgvHistory.Rows.Add(
                    row["CourseName"].ToString(),
                    row["Semester"].ToString(),
                    row["Status"].ToString(),
                    row["Grade"].ToString()
                );
            }
        }
        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvHistory.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                var cell = dgvHistory.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (status == "Completed")
                {
                    cell.Style.BackColor = Color.FromArgb(220, 255, 230);
                    cell.Style.ForeColor = Color.FromArgb(0, 120, 50);
                }
                else if (status == "In-Progress")
                {
                    cell.Style.BackColor = Color.FromArgb(255, 245, 220);
                    cell.Style.ForeColor = Color.FromArgb(180, 120, 0);
                }
                else if (status == "Pending")
                {
                    cell.Style.BackColor = Color.FromArgb(255, 230, 230);
                    cell.Style.ForeColor = Color.FromArgb(170, 0, 0);
                }
            }
        }

        private void AdviserStudentProfilePage_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblPageTitle_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (BackRequested != null)
                BackRequested();
        }

        private void btnRecommendCourses_Click(object sender, EventArgs e)
        {
            RecommendCoursesRequested?.Invoke(_studentId);

        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentStudentId))
            {
                MessageBox.Show("No student selected.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Student Report";
                sfd.Filter = "Excel CSV (*.csv)|*.csv";
                sfd.FileName = $"StudentReport_{CurrentStudentId}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportStudentReportToCsv(sfd.FileName);
                        MessageBox.Show("Report saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving report: " + ex.Message);
                    }
                }
            }
        }

        private void ExportStudentReportToCsv(string filePath)
        {
            var sb = new StringBuilder();

            // ===== Header =====
            sb.AppendLine("Student Academic Profile Report");
            sb.AppendLine($"Generated On,{DateTime.Now}");
            sb.AppendLine();

            // ===== Student Info (from labels on screen) =====
            sb.AppendLine("Student Information");
            sb.AppendLine($"Student ID,{Escape(lblStudentIdValue.Text)}");
            sb.AppendLine($"Name,{Escape(lblNameValue.Text)}");
            sb.AppendLine($"Specialization,{Escape(lblSpecValue.Text)}");
            sb.AppendLine($"Current GPA,{Escape(lblGpaValue.Text)}");
            sb.AppendLine($"Total Credits,{Escape(lblCreditsValue.Text)}");
            sb.AppendLine($"Current Semester,{Escape(lblSemesterValue.Text)}");
            sb.AppendLine();

            // ===== Academic History Table =====
            sb.AppendLine("Academic History");
            sb.AppendLine("Course Name,Semester,Status,Grade");

            foreach (DataGridViewRow row in dgvHistory.Rows)
            {
                if (row.IsNewRow) continue;

                string course = row.Cells["CourseName"].Value?.ToString() ?? "";
                string semester = row.Cells["Semester"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string grade = row.Cells["Grade"].Value?.ToString() ?? "";

                sb.AppendLine($"{Escape(course)},{Escape(semester)},{Escape(status)},{Escape(grade)}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        // CSV escaping (handles commas, quotes, new lines)
        private string Escape(string value)
        {
            if (value == null) return "";
            value = value.Trim();

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

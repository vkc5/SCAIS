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
using System.IO;
using System.Text;

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserReportsPage : UserControl
    {
        public string CurrentAdviserId { get; set; }

        private DataTable _dtStudentProgress;
        private DataTable _dtCourseProgression;
        public AdviserReportsPage()
        {
            InitializeComponent();
            SetupGrids();
        }

        private void AdviserReportsPage_Load(object sender, EventArgs e)
        {

        }

        public void RefreshPage()
        {
            if (string.IsNullOrWhiteSpace(CurrentAdviserId))
                return;

            LoadStudentProgress();
            LoadCourseProgression();
            LoadSummary();
        }
        private void SetupGrids()
        {
            // Student Progress
            dgvStudentProgress.AllowUserToAddRows = false;
            dgvStudentProgress.ReadOnly = true;
            dgvStudentProgress.RowHeadersVisible = false;
            dgvStudentProgress.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudentProgress.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Course Progression
            dgvCourseProgression.AllowUserToAddRows = false;
            dgvCourseProgression.ReadOnly = true;
            dgvCourseProgression.RowHeadersVisible = false;
            dgvCourseProgression.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourseProgression.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadStudentProgress()
        {
            string sql = @"
SELECT
    u.FullName AS StudentName,
    SUM(CASE WHEN sc.[Status] = 'Completed'   THEN 1 ELSE 0 END) AS CompletedCount,
    SUM(CASE WHEN sc.[Status] = 'In-Progress' THEN 1 ELSE 0 END) AS InProgressCount,
    SUM(CASE WHEN sc.[Status] = 'Pending'     THEN 1 ELSE 0 END) AS PendingCount,
    CAST(s.CurrentGPA AS DECIMAL(3,2)) AS GPA
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s       ON aa.StudentID = s.StudentID
JOIN dbo.Users u          ON s.UserID = u.UserID
LEFT JOIN dbo.Student_Courses sc ON sc.StudentID = s.StudentID
WHERE aa.AdviserID = @AdviserID
  AND aa.IsActive = 1
GROUP BY u.FullName, s.CurrentGPA
ORDER BY u.FullName;";

            _dtStudentProgress = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));
            dgvStudentProgress.DataSource = _dtStudentProgress;
        }

        private void LoadCourseProgression()
        {
            string sql = @"
SELECT
    c.CourseCode + ' - ' + c.CourseName AS CourseName,
    COUNT(sc.EnrollmentID) AS Enrolled,
    SUM(CASE WHEN sc.[Status] = 'Completed' THEN 1 ELSE 0 END) AS Completed,
    CASE 
        WHEN COUNT(sc.EnrollmentID) = 0 THEN NULL
        ELSE CAST(100.0 * SUM(CASE WHEN sc.[Status] = 'Completed' THEN 1 ELSE 0 END) / COUNT(sc.EnrollmentID) AS DECIMAL(5,2))
    END AS PassingRate
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s ON aa.StudentID = s.StudentID
LEFT JOIN dbo.Student_Courses sc ON sc.StudentID = s.StudentID
LEFT JOIN dbo.Courses c ON c.CourseCode = sc.CourseCode
WHERE aa.AdviserID = @AdviserID
  AND aa.IsActive = 1
GROUP BY c.CourseCode, c.CourseName
ORDER BY c.CourseCode;";

            _dtCourseProgression = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));
            dgvCourseProgression.DataSource = _dtCourseProgression;
        }

        private void LoadSummary()
        {
            string sql = @"
SELECT
    COUNT(DISTINCT aa.StudentID) AS TotalAdvisees,
    CAST(AVG(CAST(s.CurrentGPA AS FLOAT)) AS DECIMAL(4,2)) AS AverageGPA,
    CAST(
        CASE 
            WHEN SUM(CASE WHEN sc.[Status] IN ('Completed','In-Progress','Pending') THEN 1 ELSE 0 END) = 0 THEN 0
            ELSE 100.0 * SUM(CASE WHEN sc.[Status] = 'Completed' THEN 1 ELSE 0 END)
                 / SUM(CASE WHEN sc.[Status] IN ('Completed','In-Progress','Pending') THEN 1 ELSE 0 END)
        END
    AS DECIMAL(5,2)) AS OverallProgressPct
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s ON aa.StudentID = s.StudentID
LEFT JOIN dbo.Student_Courses sc ON sc.StudentID = s.StudentID
WHERE aa.AdviserID = @AdviserID
  AND aa.IsActive = 1;";

            DataTable dt = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));
            if (dt.Rows.Count == 0) return;

            lblTotalAdviseesValue.Text = dt.Rows[0]["TotalAdvisees"].ToString();
            lblAverageGpaValue.Text = dt.Rows[0]["AverageGPA"].ToString();
            lblOverallProgressValue.Text = dt.Rows[0]["OverallProgressPct"].ToString() + "%";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_dtStudentProgress == null || _dtCourseProgression == null)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Export Adviser Reports";
                sfd.Filter = "Excel CSV (*.csv)|*.csv";
                sfd.FileName = $"AdviserReports_{CurrentAdviserId}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportBothTablesToSingleCsv(sfd.FileName);
                        MessageBox.Show("Export completed!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Export failed: " + ex.Message);
                    }
                }
            }
        }

        private void ExportBothTablesToSingleCsv(string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Adviser Reports");
            sb.AppendLine($"AdviserID,{CurrentAdviserId}");
            sb.AppendLine($"Generated On,{DateTime.Now}");
            sb.AppendLine();

            // Summary
            sb.AppendLine("Report Summary");
            sb.AppendLine($"Total Advisees,{Escape(lblTotalAdviseesValue.Text)}");
            sb.AppendLine($"Average GPA,{Escape(lblAverageGpaValue.Text)}");
            sb.AppendLine($"Overall Progress,{Escape(lblOverallProgressValue.Text)}");
            sb.AppendLine();

            // Student Progress
            sb.AppendLine("Student Progress Report");
            AppendDataTableCsv(sb, _dtStudentProgress);
            sb.AppendLine();

            // Course Progression
            sb.AppendLine("Course Progression Report");
            AppendDataTableCsv(sb, _dtCourseProgression);

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private void AppendDataTableCsv(StringBuilder sb, DataTable dt)
        {
            // headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(Escape(dt.Columns[i].ColumnName));
                if (i < dt.Columns.Count - 1) sb.Append(",");
            }
            sb.AppendLine();

            // rows
            foreach (DataRow r in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(Escape(r[i]?.ToString()));
                    if (i < dt.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
            }
        }

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

    }
}

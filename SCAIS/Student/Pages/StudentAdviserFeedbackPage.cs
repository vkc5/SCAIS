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
using System.Drawing.Printing;

namespace SCAIS.Student.Pages
{
    public partial class StudentAdviserFeedbackPage : UserControl
    {
        // set from login/session
        public string CurrentStudentId { get; set; } = "STU001";

        private PrintDocument _printDoc;
        private PrintPreviewDialog _preview;

        private string _printPlanId;
        private List<(string Code, string Name, int Credits)> _printCourses = new List<(string, string, int)>();
        private DataRow _printHeaderRow;

        private string _printStudentName;
        private string _printStudentSpec;
        private string _printAdviserName;
        private string _printDecision;
        private string _printRemarks;

        private int _printCourseIndex = 0; // for multi-page

        public StudentAdviserFeedbackPage()
        {
            InitializeComponent();
            SetupFlow();
            SetupPrinting();
        }
        private void SetupPrinting()
        {
            _printDoc = new PrintDocument();
            _printDoc.BeginPrint += (s, e) => { _printCourseIndex = 0; }; // ✅ ADD THIS
            _printDoc.PrintPage += PrintDoc_PrintPage;

            _preview = new PrintPreviewDialog();
            _preview.Document = _printDoc;
            _preview.Width = 1100;
            _preview.Height = 800;
        }

        private void StudentAdviserFeedbackPage_Load(object sender, EventArgs e)
        {
            LoadPage();
        }

        public void LoadPage()
        {
            LoadCounts();
            LoadAdvisorHelpBox();
            LoadPlanCards();
        }

        private void SetupFlow()
        {
            // if you didn’t set in designer
            flpPlans.AutoScroll = true;
            flpPlans.WrapContents = false;
            flpPlans.FlowDirection = FlowDirection.TopDown;
        }

        // ===================== 1) COUNTS =====================
        private void LoadCounts()
        {
            string sql = @"
SELECT
    SUM(CASE WHEN [Status] = 'Approved' THEN 1 ELSE 0 END) AS ApprovedCount,
    SUM(CASE WHEN [Status] = 'Pending'  THEN 1 ELSE 0 END) AS PendingCount,
    SUM(CASE WHEN [Status] = 'Rejected' THEN 1 ELSE 0 END) AS RejectedCount
FROM dbo.Course_Plans
WHERE StudentID = @stu;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

            int approved = 0, pending = 0, rejected = 0;
            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                approved = r["ApprovedCount"] == DBNull.Value ? 0 : Convert.ToInt32(r["ApprovedCount"]);
                pending = r["PendingCount"] == DBNull.Value ? 0 : Convert.ToInt32(r["PendingCount"]);
                rejected = r["RejectedCount"] == DBNull.Value ? 0 : Convert.ToInt32(r["RejectedCount"]);
            }

            lblApprovedCount.Text = approved.ToString();
            lblPendingCount.Text = pending.ToString();
            lblRejectedCount.Text = rejected.ToString();
        }

        // ===================== 2) NEED HELP BOX =====================
        private void LoadAdvisorHelpBox()
        {
            // Get active adviser for this student
            string sql = @"
SELECT TOP 1
    ua.FullName AS AdviserName,
    ua.Email AS AdviserEmail,
    a.OfficeHours
FROM dbo.Advisee_Assignments aa
JOIN dbo.Advisers a ON a.AdviserID = aa.AdviserID
JOIN dbo.Users ua ON ua.UserID = a.UserID
WHERE aa.StudentID = @stu AND aa.IsActive = 1
ORDER BY aa.AssignmentDate DESC;";

            DataTable dt = Db.Query(sql, new SqlParameter("@stu", CurrentStudentId));

            if (dt.Rows.Count == 0)
            {
                lblAdviserContactValue.Text = "No adviser assigned.";
                lblOfficeHoursValue.Text = "-";
                return;
            }

            var r = dt.Rows[0];
            string name = r["AdviserName"]?.ToString() ?? "-";
            string email = r["AdviserEmail"]?.ToString() ?? "-";
            string hours = r["OfficeHours"]?.ToString() ?? "-";

            lblAdviserContactValue.Text = $"{name} - {email}";
            lblOfficeHoursValue.Text = hours;
        }

        // ===================== 3) CARDS =====================
        private void LoadPlanCards()
        {
            flpPlans.Controls.Clear();

            // Plans for this student (newest first)
            string sqlPlans = @"
SELECT
    cp.CoursePlanID,
    cp.SemesterID,
    sem.SemesterName,
    cp.SubmissionDate,
    cp.[Status],
    cp.TotalCredits,
    aa.AdviserID,
    ua.FullName AS AdviserName
FROM dbo.Course_Plans cp
JOIN dbo.Semesters sem ON sem.SemesterID = cp.SemesterID
LEFT JOIN dbo.Advisee_Assignments aa ON aa.StudentID = cp.StudentID AND aa.IsActive = 1
LEFT JOIN dbo.Advisers adv ON adv.AdviserID = aa.AdviserID
LEFT JOIN dbo.Users ua ON ua.UserID = adv.UserID
WHERE cp.StudentID = @stu
ORDER BY cp.SubmissionDate DESC;";

            DataTable plans = Db.Query(sqlPlans, new SqlParameter("@stu", CurrentStudentId));

            if (plans.Rows.Count == 0)
            {
                flpPlans.Controls.Add(BuildEmptyStateCard());
                return;
            }

            foreach (DataRow p in plans.Rows)
            {
                string planId = p["CoursePlanID"].ToString();
                string semesterName = p["SemesterName"].ToString();
                string status = p["Status"].ToString();
                DateTime submitted = Convert.ToDateTime(p["SubmissionDate"]);
                int totalCredits = Convert.ToInt32(p["TotalCredits"]);
                string adviserName = p["AdviserName"]?.ToString() ?? "-";

                // Courses in this plan
                var courses = LoadPlanCourses(planId);

                // Adviser remarks (if exists)
                var remarks = LoadLatestFeedback(planId);

                Panel card = BuildPlanCard(
                    planId,
                    semesterName,
                    submitted,
                    status,
                    adviserName,
                    totalCredits,
                    courses,
                    remarks
                );

                flpPlans.Controls.Add(card);
            }
        }

        private DataTable LoadPlanCourses(string planId)
        {
            string sql = @"
SELECT
    pi.CourseCode,
    c.CourseName,
    c.Credits
FROM dbo.Course_Plan_Items pi
JOIN dbo.Courses c ON c.CourseCode = pi.CourseCode
WHERE pi.CoursePlanID = @pid
ORDER BY pi.CourseCode;";

            return Db.Query(sql, new SqlParameter("@pid", planId));
        }

        private (string decision, string remarks) LoadLatestFeedback(string planId)
        {
            string sql = @"
SELECT TOP 1
    Decision,
    Remarks
FROM dbo.Adviser_Feedback
WHERE CoursePlanID = @pid
ORDER BY FeedbackDate DESC;";

            DataTable dt = Db.Query(sql, new SqlParameter("@pid", planId));
            if (dt.Rows.Count == 0) return ("", "");

            return (dt.Rows[0]["Decision"]?.ToString() ?? "", dt.Rows[0]["Remarks"]?.ToString() ?? "");
        }

        // ===================== UI BUILDERS =====================

        private Panel BuildEmptyStateCard()
        {
            var card = new Panel
            {
                Width = flpPlans.ClientSize.Width - 25,
                Height = 120,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12)
            };

            var lbl = new Label
            {
                Text = "No course plans found yet.\nSubmit your first plan from 'Submit Course Plan'.",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            card.Controls.Add(lbl);
            return card;
        }

        private Panel BuildPlanCard(
            string planId,
            string semesterName,
            DateTime submitted,
            string status,
            string adviserName,
            int totalCredits,
            DataTable courses,
            (string decision, string remarks) feedback)
        {
            var card = new Panel
            {
                Width = flpPlans.ClientSize.Width - 25,
                Height = 260,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14)
            };

            // ---------- Header ----------
            var header = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = $"Course Plan - {semesterName}",
                AutoSize = false,
                Left = 14,
                Top = 10,
                Width = card.Width - 200,
                Height = 22,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            var lblSub = new Label
            {
                Text = $"Submitted: {submitted:MMM dd, yyyy}",
                AutoSize = false,
                Left = 14,
                Top = 32,
                Width = 250,
                Height = 18,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            var badge = new Label
            {
                Text = status,
                AutoSize = false,
                Width = 110,
                Height = 28,
                Top = 14,
                Left = card.Width - 130,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = GetStatusBackColor(status),
                ForeColor = GetStatusForeColor(status),
                BorderStyle = BorderStyle.FixedSingle
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSub);
            header.Controls.Add(badge);

            // ---------- Body container ----------
            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14, 10, 14, 10) };

            // Left box: Approved Courses list
            var leftBox = new Panel
            {
                Left = 10,
                Top = 0,
                Width = (card.Width / 2) - 22,
                Height = 160,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 250, 255)
            };
            var lblLeftTitle = new Label
            {
                Text = "Courses",
                Left = 10,
                Top = 8,
                Width = leftBox.Width - 20,
                Height = 18,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lst = new ListBox
            {
                Left = 10,
                Top = 32,
                Width = leftBox.Width - 20,
                Height = 95,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            foreach (DataRow r in courses.Rows)
            {
                string code = r["CourseCode"].ToString();
                // You can include name if you want:
                // string name = r["CourseName"].ToString();
                // lst.Items.Add($"{code} - {name}");
                lst.Items.Add(code);
            }

            var lblTotal = new Label
            {
                Text = $"Total Courses: {courses.Rows.Count}    Total Credits: {totalCredits}",
                Left = 10,
                Top = 132,
                Width = leftBox.Width - 20,
                Height = 20,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            leftBox.Controls.Add(lblLeftTitle);
            leftBox.Controls.Add(lst);
            leftBox.Controls.Add(lblTotal);

            // Right box: Adviser remarks
            var rightBox = new Panel
            {
                Left = leftBox.Right + 14,
                Top = 0,
                Width = (card.Width / 2) - 22,
                Height = 160,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 255, 245)
            };

            var lblRightTitle = new Label
            {
                Text = "Adviser Remarks",
                Left = 10,
                Top = 8,
                Width = rightBox.Width - 20,
                Height = 18,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var txtRemarks = new TextBox
            {
                Left = 10,
                Top = 32,
                Width = rightBox.Width - 20,
                Height = 115,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Text = BuildRemarksText(status, adviserName, feedback.decision, feedback.remarks)
            };

            rightBox.Controls.Add(lblRightTitle);
            rightBox.Controls.Add(txtRemarks);

            body.Controls.Add(leftBox);
            body.Controls.Add(rightBox);

            // ---------- Bottom buttons (optional) ----------
            // (You said ignore View Details, so you can remove these if you want)
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 45, BackColor = Color.White };
            var btnPrint = new Button
            {
                Text = "Print Approval",
                Width = 130,
                Height = 30,
                Left = card.Width - 150,
                Top = 7
            };
            btnPrint.Click += (s, e) =>
            {
                // Optional: only allow printing if Approved
                if (!status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "This plan is not approved yet.\nYou can print only approved plans ✅",
                        "Print Not Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                PrintPlan(planId);
            };
            bottom.Controls.Add(btnPrint);

            card.Controls.Add(body);
            card.Controls.Add(bottom);
            card.Controls.Add(header);

            return card;
        }

        private string BuildRemarksText(string status, string adviserName, string decision, string remarks)
        {
            // Friendly fallback message
            if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                return
                    $"Adviser: {adviserName}\r\n\r\n" +
                    "Your request is still under review ⏳\r\n" +
                    "Please check again later.";
            }

            // If there is feedback record, show it
            if (!string.IsNullOrWhiteSpace(remarks) || !string.IsNullOrWhiteSpace(decision))
            {
                return
                    $"Adviser: {adviserName}\r\n" +
                    $"Decision: {decision}\r\n\r\n" +
                    $"{remarks}";
            }

            // Otherwise show generic based on status
            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                return $"Adviser: {adviserName}\r\n\r\n✅ Approved.\r\nNo remarks were recorded.";

            if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                return $"Adviser: {adviserName}\r\n\r\n❌ Rejected.\r\nNo remarks were recorded.";

            if (status.Equals("Revised", StringComparison.OrdinalIgnoreCase))
                return $"Adviser: {adviserName}\r\n\r\n⚠️ Revised.\r\nNo remarks were recorded.";

            return $"Adviser: {adviserName}\r\n\r\nNo remarks available.";
        }

        private Color GetStatusBackColor(string status)
        {
            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(220, 255, 230); // green-ish

            if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(255, 250, 220); // yellow-ish

            if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(255, 230, 230); // red-ish

            if (status.Equals("Revised", StringComparison.OrdinalIgnoreCase))
                return Color.FromArgb(235, 235, 255); // blue-ish

            return Color.WhiteSmoke;
        }

        private Color GetStatusForeColor(string status)
        {
            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase)) return Color.DarkGreen;
            if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) return Color.DarkOrange;
            if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase)) return Color.DarkRed;
            if (status.Equals("Revised", StringComparison.OrdinalIgnoreCase)) return Color.Navy;
            return Color.Black;
        }
        private void PrintPlan(string planId)
        {
            _printPlanId = planId;

            // Header + student info + semester + status + credits
            string sqlHeader = @"
SELECT TOP 1
    cp.CoursePlanID,
    cp.StudentID,        
    cp.SubmissionDate,
    cp.Status,
    cp.TotalCredits,
    sem.SemesterName,
    u.FullName AS StudentName,
    sp.SpecializationName,
    ua.FullName AS AdviserName
FROM dbo.Course_Plans cp
JOIN dbo.Semesters sem ON sem.SemesterID = cp.SemesterID
JOIN dbo.Students s ON s.StudentID = cp.StudentID
JOIN dbo.Users u ON u.UserID = s.UserID
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
LEFT JOIN dbo.Advisee_Assignments aa ON aa.StudentID = s.StudentID AND aa.IsActive = 1
LEFT JOIN dbo.Advisers adv ON adv.AdviserID = aa.AdviserID
LEFT JOIN dbo.Users ua ON ua.UserID = adv.UserID
WHERE cp.CoursePlanID = @pid;";

            DataTable header = Db.Query(sqlHeader, new SqlParameter("@pid", planId));
            if (header.Rows.Count == 0)
            {
                MessageBox.Show("Plan not found.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _printHeaderRow = header.Rows[0];
            _printStudentName = _printHeaderRow["StudentName"]?.ToString() ?? "-";
            _printStudentSpec = _printHeaderRow["SpecializationName"]?.ToString() ?? "-";
            _printAdviserName = _printHeaderRow["AdviserName"]?.ToString() ?? "-";

            // Courses
            string sqlCourses = @"
SELECT pi.CourseCode, c.CourseName, c.Credits
FROM dbo.Course_Plan_Items pi
JOIN dbo.Courses c ON c.CourseCode = pi.CourseCode
WHERE pi.CoursePlanID = @pid
ORDER BY pi.CourseCode;";

            var dtCourses = Db.Query(sqlCourses, new SqlParameter("@pid", planId));
            _printCourses.Clear();

            foreach (DataRow r in dtCourses.Rows)
            {
                _printCourses.Add((
                    r["CourseCode"].ToString(),
                    r["CourseName"].ToString(),
                    Convert.ToInt32(r["Credits"])
                ));
            }

            // Feedback
            var fb = LoadLatestFeedback(planId);
            _printDecision = fb.decision ?? "";
            _printRemarks = fb.remarks ?? "";

            // reset pagination index
            _printCourseIndex = 0;

            // show preview
            _preview.ShowDialog();
        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int left = 60;
            int top = 60;
            int width = e.MarginBounds.Width;

            var titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
            var subFont = new Font("Segoe UI", 10, FontStyle.Regular);
            var boldFont = new Font("Segoe UI", 10, FontStyle.Bold);
            var normalFont = new Font("Segoe UI", 10, FontStyle.Regular);

            int y = top;

            // ===== Header =====
            g.DrawString("Smart Course Advising Information System", titleFont, Brushes.Black, left, y);
            y += 35;
            g.DrawString("Course Plan Approval Document", boldFont, Brushes.Black, left, y);
            y += 20;

            g.DrawLine(Pens.Black, left, y, left + width, y);
            y += 18;

            // ===== Plan/Student Info =====
            string semester = _printHeaderRow["SemesterName"]?.ToString() ?? "-";
            string status = _printHeaderRow["Status"]?.ToString() ?? "-";
            DateTime submitted = Convert.ToDateTime(_printHeaderRow["SubmissionDate"]);
            int totalCredits = Convert.ToInt32(_printHeaderRow["TotalCredits"]);

            string studentId = _printHeaderRow["StudentID"]?.ToString() ?? CurrentStudentId;
            g.DrawString($"Student ID: {studentId}", normalFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Student Name: {_printStudentName}", normalFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Specialization: {_printStudentSpec}", normalFont, Brushes.Black, left, y); y += 18;

            y += 8;

            g.DrawString($"Course Plan ID: {_printPlanId}", normalFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Semester: {semester}", normalFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Submitted: {submitted:MMM dd, yyyy}", normalFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Status: {status}", boldFont, Brushes.Black, left, y); y += 18;
            g.DrawString($"Total Credits: {totalCredits}", normalFont, Brushes.Black, left, y); y += 22;

            g.DrawLine(Pens.Gray, left, y, left + width, y);
            y += 18;

            // ===== Courses Section =====
            g.DrawString("Approved Courses", boldFont, Brushes.Black, left, y);
            y += 18;

            int lineHeight = 18;
            int maxY = e.MarginBounds.Bottom - 140; // leave space for remarks + signature

            while (_printCourseIndex < _printCourses.Count)
            {
                var c = _printCourses[_printCourseIndex];

                string line = $"{c.Code}  -  {c.Name}  ({c.Credits} credits)";
                g.DrawString(line, normalFont, Brushes.Black, left + 10, y);

                y += lineHeight;
                _printCourseIndex++;

                if (y > maxY)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            y += 10;
            g.DrawLine(Pens.Gray, left, y, left + width, y);
            y += 18;

            // ===== Remarks =====
            g.DrawString("Adviser Remarks", boldFont, Brushes.Black, left, y);
            y += 18;

            g.DrawString($"Adviser: {_printAdviserName}", normalFont, Brushes.Black, left, y);
            y += 18;

            if (!string.IsNullOrWhiteSpace(_printDecision))
            {
                g.DrawString($"Decision: {_printDecision}", normalFont, Brushes.Black, left, y);
                y += 18;
            }

            string remarksText = string.IsNullOrWhiteSpace(_printRemarks)
                ? "No remarks were recorded."
                : _printRemarks;

            // wrap remarks
            RectangleF remarksRect = new RectangleF(left, y, width, 80);
            g.DrawString(remarksText, subFont, Brushes.Black, remarksRect);
            y += 90;

            // ===== Signature =====
            g.DrawLine(Pens.Black, left, y, left + 260, y);
            y += 5;
            g.DrawString("Adviser Signature", subFont, Brushes.Black, left, y);
            y += 25;

            g.DrawString($"Printed on: {DateTime.Now:MMM dd, yyyy}", subFont, Brushes.DimGray, left, y);

            e.HasMorePages = false;
        }

    }
}

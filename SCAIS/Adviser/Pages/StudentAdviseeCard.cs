using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SCAIS.Adviser.Pages
{
    public partial class StudentAdviseeCard : UserControl
    {
        public StudentAdviseeCard()
        {
            InitializeComponent();
            BuildUI();

        }

        private void StudentAdviseeCard_Load(object sender, EventArgs e)
        {

        }
        // Events for navigation later
        public event Action<string> ViewProfileClicked;
        public event Action<string> RecommendCoursesClicked;

        private string _studentId;
        private string _email;

        private Label lblName, lblMeta, lblStatus;
        private Label lblSpec, lblGpa, lblCredits, lblProgress;
        private ProgressBar prg;
        private Label lblCompleted, lblInProgress, lblPendingPlans;
        private Label lblEmail, lblLastContact;

        private Button btnViewProfile, btnRecommend, btnEmail;

        private void BuildUI()
        {
            this.Height = 200;
            this.Width = 1000; // will be resized by parent
            this.Margin = new Padding(0, 0, 0, 12);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Header
            lblName = new Label { AutoSize = false, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(14, 10), Size = new Size(500, 24) };
            lblMeta = new Label { AutoSize = false, Font = new Font("Segoe UI", 9), Location = new Point(14, 36), Size = new Size(500, 18) };

            lblStatus = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(this.Width - 120, 12),
                Size = new Size(90, 26),
                BackColor = Color.LightGreen
            };
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Column blocks
            GroupBox gbAcademic = new GroupBox { Text = "Academic Information", Location = new Point(14, 62), Size = new Size(340, 88) };
            GroupBox gbCourse = new GroupBox { Text = "Course Status", Location = new Point(365, 62), Size = new Size(260, 88) };
            GroupBox gbContact = new GroupBox { Text = "Contact & Activity", Location = new Point(635, 62), Size = new Size(360, 88) };

            // Academic
            lblSpec = new Label { Location = new Point(12, 22), Size = new Size(300, 18) };
            lblGpa = new Label { Location = new Point(12, 40), Size = new Size(160, 18) };
            lblCredits = new Label { Location = new Point(175, 40), Size = new Size(150, 18) };
            lblProgress = new Label { Location = new Point(12, 58), Size = new Size(120, 18), Text = "Progress:" };
            prg = new ProgressBar { Location = new Point(80, 60), Size = new Size(240, 14) };

            gbAcademic.Controls.Add(lblSpec);
            gbAcademic.Controls.Add(lblGpa);
            gbAcademic.Controls.Add(lblCredits);
            gbAcademic.Controls.Add(lblProgress);
            gbAcademic.Controls.Add(prg);

            // Course status
            lblCompleted = new Label { Location = new Point(12, 25), Size = new Size(220, 18) };
            lblInProgress = new Label { Location = new Point(12, 45), Size = new Size(220, 18) };
            lblPendingPlans = new Label { Location = new Point(12, 65), Size = new Size(220, 18) };
            gbCourse.Controls.Add(lblCompleted);
            gbCourse.Controls.Add(lblInProgress);
            gbCourse.Controls.Add(lblPendingPlans);

            // Contact
            lblEmail = new Label { Location = new Point(12, 25), Size = new Size(300, 18) };
            lblLastContact = new Label { Location = new Point(12, 45), Size = new Size(300, 18) };
            gbContact.Controls.Add(lblEmail);
            gbContact.Controls.Add(lblLastContact);

            // Buttons
            btnRecommend = new Button { Text = "Recommend Courses", Location = new Point(this.Width - 220, 135), Size = new Size(140, 28) };
            btnEmail = new Button { Text = "Send Email", Location = new Point(this.Width - 70, 135), Size = new Size(80, 28) };

            btnRecommend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEmail.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            btnRecommend.Click += (s, e) =>
            {
                RecommendCoursesClicked?.Invoke(_studentId);
            };
            btnEmail.Click += (s, e) => OpenEmailClient();

            this.Controls.Add(lblName);
            this.Controls.Add(lblMeta);
            this.Controls.Add(lblStatus);
            this.Controls.Add(gbAcademic);
            this.Controls.Add(gbCourse);
            this.Controls.Add(gbContact);
            this.Controls.Add(btnViewProfile);
            this.Controls.Add(btnRecommend);
            this.Controls.Add(btnEmail);

            this.Resize += (s, e) =>
            {
                lblStatus.Location = new Point(this.Width - 120, 12);
                btnRecommend.Location = new Point(this.Width - 240, this.Height - 40);
                btnEmail.Location = new Point(this.Width - 90, this.Height - 40);
                gbContact.Width = this.Width - gbContact.Left - 14;
            };
        }

        public void Bind(
            string studentId, string fullName, string currentSemester,
            string specialization, decimal gpa, int earnedCredits, int requiredCredits,
            int completedCourses, int inProgressCourses, int pendingPlans,
            string email, DateTime? lastContact, string statusText)
        {
            _studentId = studentId;
            _email = email;

            lblName.Text = fullName;
            lblMeta.Text = "ID: " + studentId + "   •   Semester: " + (string.IsNullOrWhiteSpace(currentSemester) ? "-" : currentSemester);

            lblSpec.Text = "Specialization: " + (string.IsNullOrWhiteSpace(specialization) ? "-" : specialization);
            lblGpa.Text = "GPA: " + gpa.ToString("0.00");
            lblCredits.Text = "Credits: " + earnedCredits + " / " + requiredCredits;

            int pct = 0;
            if (requiredCredits > 0)
            {
                pct = (int)Math.Round((earnedCredits * 100.0) / requiredCredits);
                if (pct < 0) pct = 0;
                if (pct > 100) pct = 100;
            }
            prg.Value = pct;

            lblCompleted.Text = "Completed Courses: " + completedCourses;
            lblInProgress.Text = "In-Progress: " + inProgressCourses;
            lblPendingPlans.Text = "Pending Plans: " + pendingPlans;

            lblEmail.Text = "Email: " + (string.IsNullOrWhiteSpace(email) ? "-" : email);
            lblLastContact.Text = "Last Contact: " + (lastContact.HasValue ? lastContact.Value.ToString("MMM dd, yyyy") : "-");

            lblStatus.Text = statusText;
            lblStatus.BackColor = (statusText == "Needs Review") ? Color.MistyRose : Color.Honeydew;
        }

        private void OpenEmailClient()
        {
            if (string.IsNullOrWhiteSpace(_email))
            {
                MessageBox.Show("No email found for this student.");
                return;
            }

            string subject = Uri.EscapeDataString("SCAIS Advising");
            string body = Uri.EscapeDataString("Hello,\n\n");

            string mailto = "mailto:" + _email + "?subject=" + subject + "&body=" + body;

            Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
        }
    }
}

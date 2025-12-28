using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SCAIS.Adviser.Pages
{
    public partial class AdviserMainForm : Form
    {
        private readonly Dictionary<string, UserControl> _pages =
        new Dictionary<string, UserControl>();
        private readonly string _userId;

        public AdviserMainForm(string userId)
        {
            InitializeComponent();
            _userId = userId;

            RegisterPages();
            ShowPage("Dashboard");
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void RegisterPages()
        {
            _pages["Dashboard"] = new AdviserDashboardPage(_userId);
            _pages["MyAdvisees"] = new AdviserMyAdviseesPage(_userId);
            _pages["RecommendedCourses"] = new AdviserRecommendedCoursesPage(_userId);
            _pages["Reports"] = new AdviserReportsPage(_userId);
            _pages["StudentProfile"] = new AdviserStudentProfilePage();
            _pages["PlanReview"] = new AdviserCoursePlanReviewPage(_userId);

            foreach (UserControl page in _pages.Values)
            {
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                pnlContent.Controls.Add(page);
            }

            // ✅ Declare ONCE
            var dashboard = (AdviserDashboardPage)_pages["Dashboard"];
            var profile = (AdviserStudentProfilePage)_pages["StudentProfile"];
            var rec = (AdviserRecommendedCoursesPage)_pages["RecommendedCourses"];
            var p2 = (AdviserCoursePlanReviewPage)_pages["PlanReview"];

            // Dashboard → Student Profile
            dashboard.ViewStudentRequested += (studentId) =>
            {
                profile.LoadStudent(studentId);
                ShowPage("StudentProfile");
            };

            // Student Profile → Dashboard
            profile.BackRequested += () =>
            {
                ShowPage("Dashboard");
            };

            rec.ViewPlanRequested += (coursePlanId) =>
            {
                p2.CurrentAdviserId = _userId;          // ✅ use logged-in adviser id
                p2.LoadPlan(coursePlanId);
                ShowPage("PlanReview");
            };

            // Plan Review → Recommended Courses
            p2.BackRequested += () =>
            {
                ShowPage("RecommendedCourses");
            };

            // Student Profile → Recommended Courses (FOCUS STUDENT ✅)
            profile.RecommendCoursesRequested += (studentId) =>
            {
                rec.FocusStudent(studentId);
                ShowPage("RecommendedCourses");
            };

            var advisees = (AdviserMyAdviseesPage)_pages["MyAdvisees"];
            advisees.RecommendCoursesRequested += (studentId) =>
            {
                rec.FocusStudent(studentId);
                ShowPage("RecommendedCourses");
            };
        }


        private void ShowPage(string key)
        {
            foreach (UserControl p in _pages.Values)
                p.Visible = false;

            _pages[key].Visible = true;
            _pages[key].BringToFront();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowPage("Dashboard");

        }

        private void btnMyAdvisees_Click(object sender, EventArgs e)
        {
            ShowPage("MyAdvisees");

        }

        private void btnRecommendedCourses_Click(object sender, EventArgs e)
        {
            ShowPage("RecommendedCourses");

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var reports = (AdviserReportsPage)_pages["Reports"];
            reports.CurrentAdviserId = _userId;        // ✅ use logged-in adviser id
            reports.RefreshPage();
            ShowPage("Reports");

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            this.Close(); // ✅ this will trigger Login.Show()
        }

        private void AdviserMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

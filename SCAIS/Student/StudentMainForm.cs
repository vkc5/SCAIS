using SCAIS.Student.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAIS.Student
{
    public partial class StudentMainForm : Form
    {
        private readonly Dictionary<string, UserControl> _pages =
            new Dictionary<string, UserControl>();
        public StudentMainForm()
        {
            InitializeComponent();
            RegisterPages();
            ShowPage("Dashboard");
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void RegisterPages()
        {
            // Create pages once (fast switching)
            _pages["Dashboard"] = new StudentDashboardPage();
            _pages["AcademicRecord"] = new StudentAcademicRecordPage();
            _pages["EligibleCourses"] = new StudentEligibleCoursesPage();
            _pages["SubmitPlan"] = new StudentSubmitCoursePlanPage();
            _pages["Feedback"] = new StudentAdviserFeedbackPage();

            // Add to pnlContent and hide
            foreach (var page in _pages.Values)
            {
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                pnlContent.Controls.Add(page);
            }
        }

        private void ShowPage(string key)
        {
            foreach (var p in _pages.Values) p.Visible = false;

            _pages[key].Visible = true;
            _pages[key].BringToFront();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StudentMainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowPage("Dashboard");
        }

        private void btnAcademicRecord_Click(object sender, EventArgs e)
        {
            ShowPage("AcademicRecord");

        }

        private void btnAvailableCourses_Click(object sender, EventArgs e)
        {
            ShowPage("EligibleCourses");

        }

        private void btnSubmitCoursePlan_Click(object sender, EventArgs e)
        {
            ShowPage("SubmitPlan");

        }

        private void btnAdviserFeedback_Click(object sender, EventArgs e)
        {
            ShowPage("Feedback");

        }
    }
}

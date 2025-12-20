using SCAIS.Admin.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAIS.Admin
{
    public partial class AdminMainForm : Form
    {
        private readonly Dictionary<string, UserControl> _pages =
        new Dictionary<string, UserControl>();
        public AdminMainForm()
        {
            InitializeComponent();
            RegisterPages();
            ShowPage("Dashboard");
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void RegisterPages()
        {
            _pages["Dashboard"] = new AdminDashboardPage();
            _pages["ManageUsers"] = new AdminManageUsersPage();
            _pages["ManageCourses"] = new AdminManageCoursesPage();
            _pages["AssignAdvisees"] = new AdminAssignAdviseesPage();
            _pages["Curriculum"] = new AdminCurriculumPage();

            foreach (UserControl page in _pages.Values)
            {
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                pnlContent.Controls.Add(page);
            }
        }

        private void ShowPage(string key)
        {
            foreach (UserControl p in _pages.Values)
                p.Visible = false;

            _pages[key].Visible = true;
            _pages[key].BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowPage("Dashboard");

        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            ShowPage("ManageUsers");

        }

        private void btnManageCourses_Click(object sender, EventArgs e)
        {
            ShowPage("ManageCourses");

        }

        private void btnAssignAdvisees_Click(object sender, EventArgs e)
        {
            ShowPage("AssignAdvisees");

        }

        private void btnCurriculum_Click(object sender, EventArgs e)
        {
            ShowPage("Curriculum");

        }
    }
}

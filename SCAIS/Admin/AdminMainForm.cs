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
            var dashboard = new AdminDashboardPage();
            dashboard.NavigateRequested += (key) => ShowPage(key);

            _pages["Dashboard"] = dashboard;

            var manageUsers = new AdminManageUsersPage();
            manageUsers.EditUserRequested += OpenEditUserPage;  // ✅ hook event
            manageUsers.AddUserRequested += OpenAddUserPage;    // ✅ NEW
            _pages["ManageUsers"] = manageUsers;

            var manageCourses = new AdminManageCoursesPage();
            manageCourses.EditCourseRequested += OpenEditCoursePage;   // ✅ hook event
            manageCourses.AddCourseRequested += OpenAddCoursePage;    // ✅ hook add (we’ll add event below)
            _pages["ManageCourses"] = manageCourses; _pages["AssignAdvisees"] = new AdminAssignAdviseesPage();

            _pages["Curriculum"] = new AdminCurriculumPage();

            foreach (UserControl page in _pages.Values)
            {
                page.Dock = DockStyle.Fill;
                page.Visible = false;
                pnlContent.Controls.Add(page);
            }
        }

        public void ShowPage(string key)
        {
            foreach (UserControl p in _pages.Values)
                p.Visible = false;

            if (!_pages.ContainsKey(key)) return;

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

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OpenEditUserPage(string userId)
        {
            // create a unique key for this edit page
            string key = $"EditUser:{userId}";

            // if page not created yet, create and register it
            if (!_pages.ContainsKey(key))
            {
                var editPage = new AdminEditUsersPage(userId);

                editPage.UserChanged += () =>
                {
                    // go back to ManageUsers
                    ShowPage("ManageUsers");

                    // refresh manage users grid
                    if (_pages["ManageUsers"] is AdminManageUsersPage mu)
                        mu.ReloadUsers();   // you must have this method
                };

                editPage.BackRequested += () =>
                {
                    ShowPage("ManageUsers");
                };

                editPage.Dock = DockStyle.Fill;
                editPage.Visible = false;

                _pages[key] = editPage;
                pnlContent.Controls.Add(editPage);
            }

            ShowPage(key);
        }
        private void OpenAddUserPage()
        {
            string key = "AddUser";

            if (!_pages.ContainsKey(key))
            {
                var addPage = new AdminAddUsersPage();

                addPage.UserCreated += () =>
                {
                    // go back to ManageUsers
                    ShowPage("ManageUsers");

                    // refresh manage users grid
                    if (_pages["ManageUsers"] is AdminManageUsersPage mu)
                        mu.ReloadUsers();
                };

                addPage.BackRequested += () =>
                {
                    ShowPage("ManageUsers");
                };

                addPage.Dock = DockStyle.Fill;
                addPage.Visible = false;

                _pages[key] = addPage;
                pnlContent.Controls.Add(addPage);
            }

            ShowPage(key);
        }
        private void OpenEditCoursePage(string courseCode)
        {
            string key = $"EditCourse:{courseCode}";

            if (!_pages.ContainsKey(key))
            {
                var editPage = new AdminEditCoursesPage(courseCode);

                editPage.CourseChanged += () =>
                {
                    ShowPage("ManageCourses");

                    if (_pages["ManageCourses"] is AdminManageCoursesPage mc)
                        mc.ReloadCourses();
                };

                editPage.BackRequested += () =>
                {
                    ShowPage("ManageCourses");
                };

                editPage.Dock = DockStyle.Fill;
                editPage.Visible = false;

                _pages[key] = editPage;
                pnlContent.Controls.Add(editPage);
            }

            ShowPage(key);
        }
        private void OpenAddCoursePage()
        {
            string key = "AddCourse";

            if (!_pages.ContainsKey(key))
            {
                var addPage = new AdminAddCoursePage();

                addPage.CourseCreated += () =>
                {
                    ShowPage("ManageCourses");
                    if (_pages["ManageCourses"] is AdminManageCoursesPage mc)
                        mc.ReloadCourses();
                };

                addPage.BackRequested += () =>
                {
                    ShowPage("ManageCourses");
                };

                addPage.Dock = DockStyle.Fill;
                addPage.Visible = false;

                _pages[key] = addPage;
                pnlContent.Controls.Add(addPage);
            }

            ShowPage(key);
        }


    }
}

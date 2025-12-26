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

namespace SCAIS.Admin.Pages
{
    public partial class AdminDashboardPage : UserControl
    {
        // ✅ This event lets AdminMainForm navigate when a quick action is clicked
        public event Action<string> NavigateRequested;

        public AdminDashboardPage()
        {
            InitializeComponent();
            WireQuickActions();
        }

        private void AdminDashboardPage_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }
        public void LoadDashboard()
        {
            LoadCounts();
            LoadSystemInfo();
        }

        // ===================== COUNTS =====================
        private void LoadCounts()
        {
            // Adjust table names if yours differ
            string sql = @"
SELECT
    (SELECT COUNT(*) FROM dbo.Students)            AS TotalStudents,
    (SELECT COUNT(*) FROM dbo.Advisers)            AS TotalAdvisers,
    (SELECT COUNT(*) FROM dbo.Courses WHERE IsActive = 1) AS TotalCourses,
    (SELECT COUNT(*) FROM dbo.Specializations)     AS TotalSpecs;";

            DataTable dt = Db.Query(sql);
            if (dt.Rows.Count == 0) return;

            var r = dt.Rows[0];

            lblTotalStudentsValue.Text = r["TotalStudents"].ToString();
            lblTotalAdvisersValue.Text = r["TotalAdvisers"].ToString();
            lblTotalCoursesValue.Text = r["TotalCourses"].ToString();
            lblTotalSpecializationsValue.Text = r["TotalSpecs"].ToString();
        }

        // ===================== SYSTEM INFO =====================
        private void LoadSystemInfo()
        {
            // ✅ Active Users (example: users with Status='Active')
            string sqlActiveUsers = @"SELECT COUNT(*) AS ActiveUsers FROM dbo.Users WHERE Status = 'Active';";
            DataTable dtUsers = Db.Query(sqlActiveUsers);
            lblActiveUsersValue.Text = (dtUsers.Rows.Count > 0) ? dtUsers.Rows[0]["ActiveUsers"].ToString() : "0";

            // ✅ Current Semester (recommended: a flag IsCurrent=1)
            // If you DON'T have IsCurrent, I will give you an alternative query below.
            string sqlSemester = @"
SELECT TOP 1 SemesterName
FROM dbo.Semesters
WHERE IsCurrentSemester = 1;";

            DataTable dtSem = Db.Query(sqlSemester);
            lblCurrentSemesterValue.Text = (dtSem.Rows.Count > 0) ? dtSem.Rows[0]["SemesterName"].ToString() : "-";
        }

        // ===================== QUICK ACTIONS (PANELS) =====================
        private void WireQuickActions()
        {
            // When you click the panel => request navigation
            MakePanelClickable(pnlQuickManageUsers, () => NavigateRequested?.Invoke("ManageUsers"));
            MakePanelClickable(pnlQuickManageCourses, () => NavigateRequested?.Invoke("ManageCourses"));
            MakePanelClickable(pnlQuickAssignAdvisees, () => NavigateRequested?.Invoke("AssignAdvisees"));
        }

        // ✅ Makes the panel AND all controls inside it clickable
        private void MakePanelClickable(Control container, Action onClick)
        {
            container.Cursor = Cursors.Hand;
            container.Click += (s, e) => onClick();

            foreach (Control child in container.Controls)
            {
                child.Cursor = Cursors.Hand;
                child.Click += (s, e) => onClick();
            }
        }

    }
}

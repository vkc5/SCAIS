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

        public AdviserMainForm()
        {
            InitializeComponent();
            RegisterPages();
            ShowPage("Dashboard");
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void RegisterPages()
        {
            _pages["Dashboard"] = new AdviserDashboardPage();
            _pages["MyAdvisees"] = new AdviserMyAdviseesPage();
            _pages["RecommendedCourses"] = new AdviserRecommendedCoursesPage();
            _pages["Reports"] = new AdviserReportsPage();

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
            ShowPage("Reports");

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AdviserMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

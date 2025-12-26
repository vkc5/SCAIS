using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCAIS.Adviser.Pages;
using System.Data.SqlClient;
using SCAIS.Core.Database;

namespace SCAIS.Adviser.Pages
{
    public partial class AdviserMyAdviseesPage : UserControl
    {

        public string CurrentAdviserId { get; set; } = "ADV001";

        // later navigation
        public Action<string> ViewProfileRequested { get; set; }
        public Action<string> RecommendCoursesRequested { get; set; }

        private DataTable _dtAdvisees;

        public AdviserMyAdviseesPage()
        {
            InitializeComponent();

            // IMPORTANT properties
            flpCards.WrapContents = false;
            flpCards.FlowDirection = FlowDirection.TopDown;
            flpCards.AutoScroll = true;

            SetupFilters();
        }

        private void AdviserMyAdviseesPage_Load(object sender, EventArgs e)
        {
            LoadAdvisees();
        }
        private void SetupFilters()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("All Status");
            cmbStatus.Items.Add("On Track");
            cmbStatus.Items.Add("Needs Review");
            cmbStatus.SelectedIndex = 0;

            cmbSpecialization.Items.Clear();
            cmbSpecialization.Items.Add("All Specializations");
            cmbSpecialization.SelectedIndex = 0;

            txtSearch.TextChanged += (s, e) => ApplyFiltersAndRender();
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFiltersAndRender();
            cmbSpecialization.SelectedIndexChanged += (s, e) => ApplyFiltersAndRender();
        }

        private void LoadAdvisees()
        {
            string sql = @"
SELECT
    s.StudentID,
    u.FullName,
    u.Email,
    s.CurrentGPA,
    s.TotalCreditsEarned,
    s.CurrentSemester,
    sp.SpecializationName,
    ISNULL(sp.RequiredCredits, 120) AS RequiredCredits,

    SUM(CASE WHEN sc.[Status] = 'Completed' THEN 1 ELSE 0 END) AS CompletedCourses,
    SUM(CASE WHEN sc.[Status] = 'In-Progress' THEN 1 ELSE 0 END) AS InProgressCourses,

    (SELECT COUNT(*) FROM dbo.Course_Plans cp
     WHERE cp.StudentID = s.StudentID AND cp.[Status] = 'Pending') AS PendingPlans,

    (SELECT MAX(af.FeedbackDate)
     FROM dbo.Adviser_Feedback af
     JOIN dbo.Course_Plans cp2 ON cp2.CoursePlanID = af.CoursePlanID
     WHERE cp2.StudentID = s.StudentID AND af.AdviserID = @AdviserID) AS LastContact,

    CASE
        WHEN (SELECT COUNT(*) FROM dbo.Course_Plans cp
              WHERE cp.StudentID = s.StudentID AND cp.[Status] = 'Pending') > 0
        THEN 'Needs Review'
        ELSE 'On Track'
    END AS AdviseeStatus
FROM dbo.Advisee_Assignments aa
JOIN dbo.Students s ON s.StudentID = aa.StudentID
JOIN dbo.Users u ON u.UserID = s.UserID
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = s.SpecializationID
LEFT JOIN dbo.Student_Courses sc ON sc.StudentID = s.StudentID
WHERE aa.AdviserID = @AdviserID AND aa.IsActive = 1
GROUP BY
    s.StudentID, u.FullName, u.Email, s.CurrentGPA, s.TotalCreditsEarned, s.CurrentSemester,
    sp.SpecializationName, sp.RequiredCredits
ORDER BY u.FullName;";

            _dtAdvisees = Db.Query(sql, new SqlParameter("@AdviserID", CurrentAdviserId));

            // Fill specialization combobox
            var specs = _dtAdvisees.AsEnumerable()
                .Select(r => r["SpecializationName"] == DBNull.Value ? "" : r["SpecializationName"].ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            cmbSpecialization.Items.Clear();
            cmbSpecialization.Items.Add("All Specializations");
            foreach (var sp in specs) cmbSpecialization.Items.Add(sp);
            cmbSpecialization.SelectedIndex = 0;

            UpdateTopStats();
            ApplyFiltersAndRender();
        }

        private void UpdateTopStats()
        {
            if (_dtAdvisees == null) return;

            int total = _dtAdvisees.Rows.Count;
            int onTrack = _dtAdvisees.AsEnumerable().Count(r => r["AdviseeStatus"].ToString() == "On Track");
            int needsReview = _dtAdvisees.AsEnumerable().Count(r => r["AdviseeStatus"].ToString() == "Needs Review");
            int pendingPlansTotal = _dtAdvisees.AsEnumerable().Sum(r => Convert.ToInt32(r["PendingPlans"]));

            lblTotalValue.Text = total.ToString();
            lblOnTrackValue.Text = onTrack.ToString();
            lblNeedsReviewValue.Text = needsReview.ToString();
            lblPendingPlansValue.Text = pendingPlansTotal.ToString();
        }

        private void ApplyFiltersAndRender()
        {
            if (_dtAdvisees == null) return;

            string search = (txtSearch.Text ?? "").Trim().ToLower();
            string status = cmbStatus.SelectedItem == null ? "All Status" : cmbStatus.SelectedItem.ToString();
            string spec = cmbSpecialization.SelectedItem == null ? "All Specializations" : cmbSpecialization.SelectedItem.ToString();

            var rows = _dtAdvisees.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                rows = rows.Where(r =>
                    r["StudentID"].ToString().ToLower().Contains(search) ||
                    r["FullName"].ToString().ToLower().Contains(search));
            }

            if (status != "All Status")
                rows = rows.Where(r => r["AdviseeStatus"].ToString() == status);

            if (spec != "All Specializations")
                rows = rows.Where(r => (r["SpecializationName"] == DBNull.Value ? "" : r["SpecializationName"].ToString()) == spec);

            RenderCards(rows.ToList());
        }

        private void RenderCards(System.Collections.Generic.List<DataRow> rows)
        {
            flpCards.SuspendLayout();
            flpCards.Controls.Clear();

            foreach (var r in rows)
            {
                var card = new StudentAdviseeCard();
                card.Width = flpCards.ClientSize.Width - 20; // keep margin scroll

                string studentId = r["StudentID"].ToString();
                string name = r["FullName"].ToString();
                string semester = r["CurrentSemester"] == DBNull.Value ? "" : r["CurrentSemester"].ToString();
                string specialization = r["SpecializationName"] == DBNull.Value ? "" : r["SpecializationName"].ToString();

                decimal gpa = Convert.ToDecimal(r["CurrentGPA"]);
                int earnedCredits = Convert.ToInt32(r["TotalCreditsEarned"]);
                int requiredCredits = Convert.ToInt32(r["RequiredCredits"]);

                int completed = Convert.ToInt32(r["CompletedCourses"]);
                int inProg = Convert.ToInt32(r["InProgressCourses"]);
                int pendingPlans = Convert.ToInt32(r["PendingPlans"]);

                string email = r["Email"] == DBNull.Value ? "" : r["Email"].ToString();

                DateTime? lastContact = null;
                if (r["LastContact"] != DBNull.Value) lastContact = Convert.ToDateTime(r["LastContact"]);

                string adviseeStatus = r["AdviseeStatus"].ToString();

                card.Bind(studentId, name, semester,
                    specialization, gpa, earnedCredits, requiredCredits,
                    completed, inProg, pendingPlans,
                    email, lastContact, adviseeStatus);

                // wire events for later
                card.ViewProfileClicked += (sid) => ViewProfileRequested?.Invoke(sid);
                card.RecommendCoursesClicked += (sid) => RecommendCoursesRequested?.Invoke(sid);

                flpCards.Controls.Add(card);
            }

            flpCards.ResumeLayout();
        }

    }
}

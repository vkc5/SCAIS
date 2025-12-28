using SCAIS.Core.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCAIS.Admin.Pages
{
    public partial class AdminManageCoursesPage : UserControl
    {
        private DataTable _coursesTable;

        public event Action<string> EditCourseRequested; // sends CourseCode
        public event Action AddCourseRequested;

        public AdminManageCoursesPage()
        {
            InitializeComponent();
            SetupUi();
            SetupGrid();
            // events
            this.Load += AdminManageCoursesPage_Load;
            btnRefresh.Click += btnRefresh_Click;
            txtSearch.KeyDown += txtSearch_KeyDown;
            dgvCourses.CellFormatting += dgvCourses_CellFormatting;
            dgvCourses.CellContentClick += dgvCourses_CellContentClick;
            btnAddCourse.Click += (s, e) => AddCourseRequested?.Invoke();
        }

        private void AdminManageCoursesPage_Load(object sender, EventArgs e)
        {
            LoadSpecializations();
            LoadCourses();
        }

        // ---------- UI ----------
        private void SetupUi()
        {
            cmbSpecialization.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadSpecializations()
        {
            cmbSpecialization.Items.Clear();
            cmbSpecialization.Items.Add("All");
            cmbSpecialization.Items.Add("Core"); // show core courses

            var dt = Db.Query(@"
                SELECT SpecializationName
                FROM dbo.Specializations
                WHERE IsActive = 1
                ORDER BY SpecializationName;
            ");

            foreach (DataRow r in dt.Rows)
                cmbSpecialization.Items.Add(r["SpecializationName"].ToString());

            cmbSpecialization.SelectedIndex = 0;
        }

        // ---------- GRID ----------
        private void SetupGrid()
        {
            dgvCourses.AllowUserToAddRows = false;
            dgvCourses.AllowUserToDeleteRows = false;
            dgvCourses.ReadOnly = true;
            dgvCourses.RowHeadersVisible = false;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.MultiSelect = false;
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCourses.AutoGenerateColumns = false;
            dgvCourses.Columns.Clear();

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCourseCode",
                HeaderText = "Course Code",
                DataPropertyName = "CourseCode",
                FillWeight = 12
            });

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCourseName",
                HeaderText = "Course Name",
                DataPropertyName = "CourseName",
                FillWeight = 28
            });

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSpecialization",
                HeaderText = "Specialization",
                DataPropertyName = "Specialization",
                FillWeight = 15
            });

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrereq",
                HeaderText = "Prerequisite",
                DataPropertyName = "Prerequisite",
                FillWeight = 15
            });

            dgvCourses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCoreq",
                HeaderText = "Corequisite",
                DataPropertyName = "Corequisite",
                FillWeight = 15
            });

            var editBtn = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 8
            };
            dgvCourses.Columns.Add(editBtn);
        }

        private void dgvCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCourses.Columns[e.ColumnIndex].Name != "colEdit") return;

            // IMPORTANT: use the column NAME you created: colCourseCode
            string code = dgvCourses.Rows[e.RowIndex].Cells["colCourseCode"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(code)) return;

            EditCourseRequested?.Invoke(code);
        }

        // styling (optional like your screenshot)
        private void dgvCourses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCourses.Columns[e.ColumnIndex].Name == "colSpecialization" && e.Value != null)
            {
                string spec = e.Value.ToString();
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // simple coloring (adjust as you want)
                if (spec.Equals("Core", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Gainsboro;
                }
                else if (spec.Equals("Programming", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.AliceBlue;
                }
                else if (spec.Equals("Networking", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Honeydew;
                }
                else if (spec.Equals("Cybersecurity", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.MistyRose;
                }
                else if (spec.Equals("Database", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Lavender;
                }
            }

            if (dgvCourses.Columns[e.ColumnIndex].Name == "colEdit")
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ---------- LOAD DATA ----------
        private void LoadCourses()
        {
            string spec = cmbSpecialization.SelectedItem?.ToString() ?? "All";
            string search = (txtSearch.Text ?? "").Trim();

            // NOTE:
            // - specialization shown as "Core" when SpecializationID is NULL
            // - prereq/coreq shown as single value (TOP 1) else "None"
            string sql = @"
SELECT
    c.CourseCode,
    c.CourseName,
    CASE 
        WHEN c.SpecializationID IS NULL THEN 'Core'
        ELSE sp.SpecializationName
    END AS Specialization,
    ISNULL(pr.PrerequisiteCourseCode, 'None') AS Prerequisite,
    ISNULL(cr.CorequisiteCourseCode, 'None') AS Corequisite
FROM dbo.Courses c
LEFT JOIN dbo.Specializations sp ON sp.SpecializationID = c.SpecializationID

OUTER APPLY (
    SELECT TOP 1 p.PrerequisiteCourseCode
    FROM dbo.Prerequisites p
    WHERE p.CourseCode = c.CourseCode
    ORDER BY p.PrerequisiteCourseCode
) pr

OUTER APPLY (
    SELECT TOP 1 x.CorequisiteCourseCode
    FROM dbo.Corequisites x
    WHERE x.CourseCode = c.CourseCode
    ORDER BY x.CorequisiteCourseCode
) cr

WHERE
(
    @spec = 'All'
    OR (@spec = 'Core' AND c.SpecializationID IS NULL)
    OR (sp.SpecializationName = @spec)
)
AND (
    @search = '' OR
    c.CourseCode LIKE '%' + @search + '%' OR
    c.CourseName LIKE '%' + @search + '%'
)
ORDER BY c.CourseCode;
";

            _coursesTable = Db.Query(sql,
                new SqlParameter("@spec", spec),
                new SqlParameter("@search", search)
            );

            dgvCourses.DataSource = _coursesTable;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadCourses();
            }
        }

        public void ReloadCourses()
        {
            LoadCourses();
        }







    }
}

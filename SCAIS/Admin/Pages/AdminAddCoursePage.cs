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
    public partial class AdminAddCoursePage : UserControl
    {
        public event Action BackRequested;
        public event Action CourseCreated;

        public AdminAddCoursePage()
        {
            InitializeComponent();
            this.Load += AdminAddCoursePage_Load;
            btnBack.Click += (s, e) => BackRequested?.Invoke();
            btnCreate.Click += btnCreate_Click;
        }

        private void AdminAddCoursePage_Load(object sender, EventArgs e)
        {
            SetupCombos();
        }

        // ---------- COMBOS ----------
        private void SetupCombos()
        {
            // CourseType
            cmbCourseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseType.Items.Clear();
            cmbCourseType.Items.Add("Core");
            cmbCourseType.Items.Add("Specialized");
            cmbCourseType.Items.Add("Elective");
            cmbCourseType.SelectedIndex = 0;

            // Specialization
            cmbSpecializationID.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpecializationID.Items.Clear();
            cmbSpecializationID.Items.Add(new ComboItem { Text = "None (Core)", Value = DBNull.Value });

            var specs = Db.Query(@"
                SELECT SpecializationID, SpecializationName
                FROM dbo.Specializations
                WHERE IsActive = 1
                ORDER BY SpecializationName;
            ");

            foreach (DataRow r in specs.Rows)
            {
                cmbSpecializationID.Items.Add(new ComboItem
                {
                    Text = r["SpecializationName"].ToString(),
                    Value = r["SpecializationID"]
                });
            }

            cmbSpecializationID.SelectedIndex = 0;

            // prereq/coreq lists (courses)
            LoadCourseListIntoCombo(cmbPrerequisite);
            LoadCourseListIntoCombo(cmbCorequisite);
        }

        private void LoadCourseListIntoCombo(ComboBox cmb)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Items.Clear();
            cmb.Items.Add(new ComboItem { Text = "None", Value = DBNull.Value });

            var dt = Db.Query(@"
                SELECT CourseCode, CourseName
                FROM dbo.Courses
                ORDER BY CourseCode;
            ");

            foreach (DataRow r in dt.Rows)
            {
                cmb.Items.Add(new ComboItem
                {
                    Text = $"{r["CourseCode"]} - {r["CourseName"]}",
                    Value = r["CourseCode"]
                });
            }

            cmb.SelectedIndex = 0;
        }

        // ---------- VALIDATION ----------
        private bool ValidateInputs(out int credits, out string error)
        {
            error = "";
            credits = 0;

            if (string.IsNullOrWhiteSpace(txtCourseCode.Text))
            {
                error = "Course Code is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                error = "Course Name is required.";
                return false;
            }

            if (!int.TryParse(txtCredits.Text.Trim(), out credits) || credits <= 0)
            {
                error = "Credits must be a number > 0.";
                return false;
            }

            string code = txtCourseCode.Text.Trim();

            // prereq/coreq selections
            string prereqCode = GetSelectedCourseCode(cmbPrerequisite);
            string coreqCode = GetSelectedCourseCode(cmbCorequisite);

            // cannot be itself
            if (!string.IsNullOrEmpty(prereqCode) && prereqCode == code)
            {
                error = "A course cannot be its own prerequisite.";
                return false;
            }

            if (!string.IsNullOrEmpty(coreqCode) && coreqCode == code)
            {
                error = "A course cannot be its own corequisite.";
                return false;
            }

            // prereq cannot equal coreq
            if (!string.IsNullOrEmpty(prereqCode) && !string.IsNullOrEmpty(coreqCode) && prereqCode == coreqCode)
            {
                error = "Prerequisite and Corequisite cannot be the same course.";
                return false;
            }

            return true;
        }

        private string GetSelectedCourseCode(ComboBox cmb)
        {
            if (cmb.SelectedItem is ComboItem item)
            {
                if (item.Value == DBNull.Value) return null;
                return item.Value.ToString();
            }
            return null;
        }

        private object GetSelectedValue(ComboBox cmb)
        {
            if (cmb.SelectedItem is ComboItem item)
                return item.Value;
            return DBNull.Value;
        }

        // ---------- CREATE ----------
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out int credits, out string err))
            {
                MessageBox.Show(err, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string code = txtCourseCode.Text.Trim();
            string name = txtCourseName.Text.Trim();
            string desc = txtDescription.Text ?? "";
            string courseType = cmbCourseType.SelectedItem?.ToString() ?? "Core";
            object specId = GetSelectedValue(cmbSpecializationID);

            // If specialization is None -> treat as Core
            if (specId == DBNull.Value)
                courseType = "Core";

            string prereqCode = GetSelectedCourseCode(cmbPrerequisite);
            string coreqCode = GetSelectedCourseCode(cmbCorequisite);

            string sql = @"
BEGIN TRY
    BEGIN TRAN;

    -- avoid duplicates
    IF EXISTS (SELECT 1 FROM dbo.Courses WHERE CourseCode = @code)
        THROW 50001, 'Course Code already exists.', 1;

    INSERT INTO dbo.Courses (CourseCode, CourseName, [Description], Credits, SpecializationID, CourseType)
    VALUES (@code, @name, @desc, @credits, @specId, @type);

    IF (@pr IS NOT NULL)
        INSERT INTO dbo.Prerequisites (CourseCode, PrerequisiteCourseCode) VALUES (@code, @pr);

    IF (@cr IS NOT NULL)
        INSERT INTO dbo.Corequisites (CourseCode, CorequisiteCourseCode) VALUES (@code, @cr);

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    THROW;
END CATCH;";

            try
            {
                Db.Execute(sql,
                    new SqlParameter("@code", code),
                    new SqlParameter("@name", name),
                    new SqlParameter("@desc", (object)desc ?? ""),
                    new SqlParameter("@credits", credits),
                    new SqlParameter("@specId", specId),
                    new SqlParameter("@type", courseType),
                    new SqlParameter("@pr", (object)prereqCode ?? DBNull.Value),
                    new SqlParameter("@cr", (object)coreqCode ?? DBNull.Value)
                );

                MessageBox.Show("Course created ✅", "Add Course",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CourseCreated?.Invoke();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Create failed: " + ex.Message, "Add Course",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ComboItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }


    }
}

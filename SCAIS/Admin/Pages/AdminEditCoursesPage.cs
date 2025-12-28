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
    public partial class AdminEditCoursesPage : UserControl
    {
        public event Action BackRequested;
        public event Action CourseChanged;

        private readonly string _courseCode;

        public AdminEditCoursesPage(string courseCode)
        {
            InitializeComponent();
            _courseCode = courseCode;

            this.Load += AdminEditCoursesPage_Load;
        }

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void AdminEditCoursesPage_Load(object sender, EventArgs e)
        {
            SetupCombos();
            LoadCourse(_courseCode);
        }
        private void SetupCombos()
        {
            // CourseType
            cmbCourseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseType.Items.Clear();
            cmbCourseType.Items.Add("Core");
            cmbCourseType.Items.Add("Specialized");
            cmbCourseType.Items.Add("Elective");

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

            // Prereq/Coreq lists (courses)
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

        private void LoadCourse(string courseCode)
        {
            // course info
            var dt = Db.Query(@"
SELECT CourseCode, CourseName, [Description], Credits, CourseType, SpecializationID
FROM dbo.Courses
WHERE CourseCode = @code;", new SqlParameter("@code", courseCode));

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Course not found.", "Edit Course",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = dt.Rows[0];

            txtCourseCode.Text = r["CourseCode"].ToString();
            txtCourseCode.ReadOnly = true; // recommended (changing code breaks FK tables)

            txtCourseName.Text = r["CourseName"].ToString();
            txtDescription.Text = r["Description"]?.ToString() ?? "";
            txtCredits.Text = r["Credits"].ToString();

            SelectComboText(cmbCourseType, r["CourseType"].ToString());
            SelectComboByValue(cmbSpecializationID, r["SpecializationID"]);

            // prereq (take TOP 1)
            var prereq = Db.Query(@"
SELECT TOP 1 PrerequisiteCourseCode
FROM dbo.Prerequisites
WHERE CourseCode = @code
ORDER BY PrerequisiteCourseCode;",
                new SqlParameter("@code", courseCode));

            if (prereq.Rows.Count > 0)
                SelectComboByValue(cmbPrerequisite, prereq.Rows[0][0]);

            // coreq (take TOP 1)
            var coreq = Db.Query(@"
SELECT TOP 1 CorequisiteCourseCode
FROM dbo.Corequisites
WHERE CourseCode = @code
ORDER BY CorequisiteCourseCode;",
                new SqlParameter("@code", courseCode));

            if (coreq.Rows.Count > 0)
                SelectComboByValue(cmbCorequisite, coreq.Rows[0][0]);
        }

        private void SelectComboText(ComboBox cmb, string text)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (string.Equals(cmb.Items[i].ToString(), text, StringComparison.OrdinalIgnoreCase))
                {
                    cmb.SelectedIndex = i;
                    return;
                }
            }
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0; // default
        }

        private void SelectComboByValue(ComboBox cmb, object value)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i] is ComboItem item)
                {
                    if ((value == null || value == DBNull.Value) && item.Value == DBNull.Value)
                    {
                        cmb.SelectedIndex = i;
                        return;
                    }

                    if (value != null && value != DBNull.Value && item.Value != DBNull.Value &&
                        item.Value.ToString() == value.ToString())
                    {
                        cmb.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        // ---------- VALIDATION ----------
        private bool ValidateInputs(out int credits, out string error)
        {
            error = "";
            credits = 0;

            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                error = "Course Name is required.";
                return false;
            }

            if (!int.TryParse(txtCredits.Text.Trim(), out credits) || credits <= 0)
            {
                error = "Credits must be a number greater than 0.";
                return false;
            }

            // get selected prereq/coreq course codes
            string prereqCode = GetSelectedCourseCode(cmbPrerequisite);
            string coreqCode = GetSelectedCourseCode(cmbCorequisite);

            // course code itself
            string thisCode = txtCourseCode.Text.Trim();

            // cannot be itself
            if (!string.IsNullOrEmpty(prereqCode) && prereqCode == thisCode)
            {
                error = "A course cannot be its own prerequisite.";
                return false;
            }

            if (!string.IsNullOrEmpty(coreqCode) && coreqCode == thisCode)
            {
                error = "A course cannot be its own corequisite.";
                return false;
            }

            // IMPORTANT RULE:
            // prereq cannot be coreq at same time
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out int credits, out string err))
            {
                MessageBox.Show(err, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string code = txtCourseCode.Text.Trim();
            string prereqCode = GetSelectedCourseCode(cmbPrerequisite);
            string coreqCode = GetSelectedCourseCode(cmbCorequisite);

            object specId = GetSelectedValue(cmbSpecializationID);
            string courseType = cmbCourseType.SelectedItem?.ToString() ?? "Core";

            // If specialization is "None", force Core type (optional but logical)
            if (specId == DBNull.Value)
                courseType = "Core";

            string sql = @"
BEGIN TRY
    BEGIN TRAN;

    UPDATE dbo.Courses
    SET CourseName = @name,
        [Description] = @desc,
        Credits = @credits,
        CourseType = @type,
        SpecializationID = @specId
    WHERE CourseCode = @code;

    -- Reset prereq/coreq (simple single-value approach)
    DELETE FROM dbo.Prerequisites WHERE CourseCode = @code;
    DELETE FROM dbo.Corequisites  WHERE CourseCode = @code;

    IF (@pr IS NOT NULL)
        INSERT INTO dbo.Prerequisites (CourseCode, PrerequisiteCourseCode) VALUES (@code, @pr);

    IF (@cr IS NOT NULL)
        INSERT INTO dbo.Corequisites (CourseCode, CorequisiteCourseCode) VALUES (@code, @cr);

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    THROW;
END CATCH;
";

            try
            {
                Db.Execute(sql,
                    new SqlParameter("@code", code),
                    new SqlParameter("@name", txtCourseName.Text.Trim()),
                    new SqlParameter("@desc", (object)(txtDescription.Text ?? "") ?? DBNull.Value),
                    new SqlParameter("@credits", credits),
                    new SqlParameter("@type", courseType),
                    new SqlParameter("@specId", specId),
                    new SqlParameter("@pr", (object)prereqCode ?? DBNull.Value),
                    new SqlParameter("@cr", (object)coreqCode ?? DBNull.Value)
                );

                MessageBox.Show("Saved ✅", "Edit Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CourseChanged?.Invoke();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Edit Course",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BackRequested?.Invoke();
        }
        private class ComboItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }

    }
}

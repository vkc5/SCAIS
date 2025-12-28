using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SCAIS.Core.Database;

namespace SCAIS.Admin.Pages
{
    public partial class AdminCurriculumStructurePage : UserControl
    {
        public event Action BackRequested;
        public event Action StructureSaved;

        private DataTable _table;

        public AdminCurriculumStructurePage()
        {
            InitializeComponent();
            // wire events
            this.Load += AdminCurriculumStructurePage_Load;
            cmbSpecialization.SelectedIndexChanged += cmbSpecialization_SelectedIndexChanged;
            btnSave.Click += btnSave_Click;
            btnBack.Click += (s, e) => BackRequested?.Invoke();

            SetupUi();
            SetupGrid();
        }

        private void AdminCurriculumStructurePage_Load(object sender, EventArgs e)
        {
            LoadSpecializations();
        }

        // ---------------- UI ----------------
        private void SetupUi()
        {
            cmbSpecialization.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetupGrid()
        {
            dgvStructure.AllowUserToAddRows = false;
            dgvStructure.AllowUserToDeleteRows = false;
            dgvStructure.RowHeadersVisible = false;
            dgvStructure.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStructure.MultiSelect = false;
            dgvStructure.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStructure.AutoGenerateColumns = false;
            dgvStructure.Columns.Clear();

            dgvStructure.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCourseCode",
                HeaderText = "Course Code",
                DataPropertyName = "CourseCode",
                ReadOnly = true,
                FillWeight = 12
            });

            dgvStructure.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCourseName",
                HeaderText = "Course Name",
                DataPropertyName = "CourseName",
                ReadOnly = true,
                FillWeight = 35
            });

            dgvStructure.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCredits",
                HeaderText = "Credits",
                DataPropertyName = "Credits",
                ReadOnly = true,
                FillWeight = 8
            });

            dgvStructure.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colType",
                HeaderText = "Type",
                DataPropertyName = "CourseType",
                ReadOnly = true,
                FillWeight = 12
            });

            // Term dropdown column (Fall/Spring order)
            var termCol = new DataGridViewComboBoxColumn
            {
                Name = "colTermIndex",
                HeaderText = "Recommended Term",
                DataPropertyName = "TermIndex", // int in datatable
                FillWeight = 20,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            };

            // Add choices (1..8 typical, change to 10/12 if your program longer)
            termCol.Items.Add(DBNull.Value); // "not set"
            for (int i = 1; i <= 8; i++)
                termCol.Items.Add(i);

            dgvStructure.Columns.Add(termCol);

            dgvStructure.CellFormatting += dgvStructure_CellFormatting;
        }

        // Show “Fall 1 / Spring 1 ...” instead of number
        private void dgvStructure_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvStructure.Columns[e.ColumnIndex].Name == "colTermIndex" && e.Value != null && e.Value != DBNull.Value)
            {
                if (int.TryParse(e.Value.ToString(), out int termIndex))
                {
                    e.Value = TermIndexToText(termIndex);
                    e.FormattingApplied = true;
                }
            }
        }

        private string TermIndexToText(int termIndex)
        {
            // 1=Fall1, 2=Spring1, 3=Fall2, 4=Spring2...
            int year = (termIndex + 1) / 2;
            bool isFall = termIndex % 2 == 1;
            return isFall ? $"Fall {year}" : $"Spring {year}";
        }

        // ---------------- LOAD ----------------
        private void LoadSpecializations()
        {
            cmbSpecialization.Items.Clear();

            var dt = Db.Query(@"
                SELECT SpecializationID, SpecializationName
                FROM dbo.Specializations
                WHERE IsActive = 1
                ORDER BY SpecializationName;
            ");

            foreach (DataRow r in dt.Rows)
            {
                cmbSpecialization.Items.Add(new ComboItem
                {
                    Text = r["SpecializationName"].ToString(),
                    Value = r["SpecializationID"]
                });
            }

            if (cmbSpecialization.Items.Count > 0)
                cmbSpecialization.SelectedIndex = 0;
        }

        private void cmbSpecialization_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStructure();
        }

        private int GetSelectedSpecializationId()
        {
            if (cmbSpecialization.SelectedItem is ComboItem item)
                return Convert.ToInt32(item.Value);
            return 0;
        }

        private void LoadStructure()
        {
            int specId = GetSelectedSpecializationId();
            if (specId == 0) return;

            // courses in specialization + current saved TermIndex (if any)
            string sql = @"
SELECT
    c.CourseCode,
    c.CourseName,
    c.Credits,
    c.CourseType,
    cs.TermIndex
FROM dbo.Specialization_Courses sc
JOIN dbo.Courses c ON c.CourseCode = sc.CourseCode
LEFT JOIN dbo.Curriculum_Structure cs
    ON cs.SpecializationID = sc.SpecializationID
   AND cs.CourseCode = sc.CourseCode
WHERE sc.SpecializationID = @specId
ORDER BY c.CourseCode;";

            _table = Db.Query(sql, new SqlParameter("@specId", specId));
            dgvStructure.DataSource = _table;

            // Convert display for combobox column:
            // It stores int, but formatting shows Fall/Spring text automatically.
        }

        // ---------------- SAVE ----------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            int specId = GetSelectedSpecializationId();
            if (specId == 0)
            {
                MessageBox.Show("Select a specialization first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Commit edits from grid
            dgvStructure.EndEdit();

            try
            {
                // Save each row (UPSERT)
                foreach (DataGridViewRow row in dgvStructure.Rows)
                {
                    string courseCode = row.Cells["colCourseCode"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(courseCode)) continue;

                    object termVal = row.Cells["colTermIndex"].Value;

                    // If user didn't set term => remove from structure table (optional behavior)
                    if (termVal == null || termVal == DBNull.Value || termVal.ToString() == "")
                    {
                        Db.Execute(@"
DELETE FROM dbo.Curriculum_Structure
WHERE SpecializationID=@sid AND CourseCode=@code;",
                            new SqlParameter("@sid", specId),
                            new SqlParameter("@code", courseCode));
                        continue;
                    }

                    // termVal might be string “Fall 1” because of formatting,
                    // so we force it back to int safely:
                    int termIndex = ExtractTermIndex(termVal);

                    if (termIndex < 1 || termIndex > 12)
                    {
                        MessageBox.Show($"Invalid term for course {courseCode}.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Db.Execute(@"
MERGE dbo.Curriculum_Structure AS t
USING (SELECT @sid AS SpecializationID, @code AS CourseCode) AS s
ON (t.SpecializationID = s.SpecializationID AND t.CourseCode = s.CourseCode)
WHEN MATCHED THEN
    UPDATE SET TermIndex = @term
WHEN NOT MATCHED THEN
    INSERT (SpecializationID, CourseCode, TermIndex)
    VALUES (@sid, @code, @term);",
                        new SqlParameter("@sid", specId),
                        new SqlParameter("@code", courseCode),
                        new SqlParameter("@term", termIndex)
                    );
                }

                MessageBox.Show("Curriculum structure saved ✅", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                StructureSaved?.Invoke();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ExtractTermIndex(object termVal)
        {
            if (termVal == null || termVal == DBNull.Value) return 0;

            // If it's already int
            if (int.TryParse(termVal.ToString(), out int num))
                return num;

            // If it is "Fall 2" / "Spring 3"
            string s = termVal.ToString().Trim();
            if (s.StartsWith("Fall", StringComparison.OrdinalIgnoreCase))
            {
                // Fall y => termIndex = 2*y - 1
                var parts = s.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[1], out int y))
                    return (2 * y) - 1;
            }
            if (s.StartsWith("Spring", StringComparison.OrdinalIgnoreCase))
            {
                // Spring y => termIndex = 2*y
                var parts = s.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[1], out int y))
                    return (2 * y);
            }

            return 0;
        }

        private class ComboItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }


    }
}

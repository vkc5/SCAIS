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
using System.Drawing.Printing;

namespace SCAIS.Admin.Pages
{
    public partial class AdminCurriculumPage : UserControl
    {
        private PrintDocument _printDoc;
        private PrintPreviewDialog _preview;

        // One combined table for all specs
        private DataTable _printAllRows;
        private int _printRowIndex = 0;

        // Keep loaded tables so we can save changes later
        private readonly Dictionary<int, DataTable> _specTables = new Dictionary<int, DataTable>();

        // Map specialization id -> grid
        private Dictionary<int, DataGridView> _specGrids;

        public AdminCurriculumPage()
        {
            InitializeComponent();
            SetupGrids();

            // Your sample data uses:
            // 1 Programming, 2 Networking, 3 Cybersecurity, 4 Database
            _specGrids = new Dictionary<int, DataGridView>
            {
                { 1, dgvProgramming },
                { 2, dgvNetworking },
                { 3, dgvCybersecurity },
                { 4, dgvDatabase }
            };

            SetupPrinting();
        }

        private void AdminCurriculumPage_Load(object sender, EventArgs e)
        {
            LoadAllCurriculums();

        }
        private void SetupPrinting()
        {
            _printDoc = new PrintDocument();

            // Reset before ANY print job (preview OR real print)
            _printDoc.BeginPrint += (s, e) =>
            {
                _printRowIndex = 0;
            };

            _printDoc.PrintPage += PrintDoc_PrintPage_All;

            _preview = new PrintPreviewDialog();
            _preview.Document = _printDoc;
            _preview.Width = 1100;
            _preview.Height = 800;
        }

        private void SetupGrids()
        {
            foreach (var dgv in new[] { dgvProgramming, dgvNetworking, dgvCybersecurity, dgvDatabase })
            {
                dgv.AllowUserToAddRows = false;
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgv.CurrentCellDirtyStateChanged += (s, e) =>
                {
                    if (dgv.IsCurrentCellDirty)
                        dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
            }
        }

        // ===================== LOAD =====================
        private void LoadAllCurriculums()
        {
            _specTables.Clear();

            foreach (var kv in _specGrids)
            {
                int specId = kv.Key;
                DataGridView grid = kv.Value;

                DataTable dt = LoadCurriculumForSpecialization(specId);
                _specTables[specId] = dt;

                BindGrid(grid, dt);
            }
        }

        private DataTable LoadCurriculumForSpecialization(int specId)
        {
            string sql = @"
SELECT
    c.CourseCode,
    c.CourseName,
    c.Credits,
    CASE 
        WHEN c.CourseType = 'Core' THEN 'Core'
        ELSE 'Specialized'
    END AS [Type],
    CAST(CASE WHEN sc.CourseCode IS NULL THEN 0 ELSE 1 END AS bit) AS InCurriculum
FROM dbo.Courses c
LEFT JOIN dbo.Specialization_Courses sc
    ON sc.SpecializationID = @specId
   AND sc.CourseCode = c.CourseCode
WHERE c.IsActive = 1
  AND (c.CourseType = 'Core' OR c.SpecializationID = @specId)
ORDER BY 
    CASE WHEN c.CourseType = 'Core' THEN 0 ELSE 1 END,
    c.CourseCode;";

            return Db.Query(sql, new SqlParameter("@specId", specId));
        }


        private void BindGrid(DataGridView dgv, DataTable dt)
        {
            dgv.Columns.Clear();
            dgv.DataSource = dt;

            // Replace InCurriculum with checkbox column at first position
            int idx = dgv.Columns["InCurriculum"].Index;
            dgv.Columns.Remove("InCurriculum");

            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "InCurriculum",
                HeaderText = "In Curriculum",
                DataPropertyName = "InCurriculum",
                TrueValue = true,
                FalseValue = false,
                Width = 90
            };
            dgv.Columns.Insert(0, chk);

            dgv.Columns["CourseCode"].HeaderText = "Course Code";
            dgv.Columns["CourseName"].HeaderText = "Course Name";
            dgv.Columns["Credits"].HeaderText = "Credits";
            dgv.Columns["Type"].HeaderText = "Type";

            // Make text columns read-only
            dgv.Columns["CourseCode"].ReadOnly = true;
            dgv.Columns["CourseName"].ReadOnly = true;
            dgv.Columns["Credits"].ReadOnly = true;
            dgv.Columns["Type"].ReadOnly = true;

            // Only checkbox editable
            dgv.Columns["InCurriculum"].ReadOnly = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int totalInserted = 0, totalDeleted = 0;

            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var kv in _specTables)
                        {
                            int specId = kv.Key;
                            DataTable dt = kv.Value;

                            foreach (DataRow r in dt.Rows)
                            {
                                string code = r["CourseCode"].ToString();
                                bool inCurr = false;
                                object val = r["InCurriculum"];

                                if (val is bool b) inCurr = b;
                                else if (val != DBNull.Value) inCurr = Convert.ToInt32(val) == 1;
                                bool exists = ExistsInCurriculum(con, tx, specId, code);

                                if (inCurr && !exists)
                                {
                                    InsertCurriculum(con, tx, specId, code, r["Type"]?.ToString() ?? "Core");
                                    totalInserted++;
                                }
                                else if (!inCurr && exists)
                                {
                                    DeleteCurriculum(con, tx, specId, code);
                                    totalDeleted++;
                                }
                            }
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        MessageBox.Show("Save failed: " + ex.Message, "Curriculum",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            MessageBox.Show(
                $"Saved ✅\nAdded: {totalInserted}\nRemoved: {totalDeleted}",
                "Curriculum",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Reload to reflect DB truth
            LoadAllCurriculums();

        }

        private bool ExistsInCurriculum(SqlConnection con, SqlTransaction tx, int specId, string courseCode)
        {
            using (SqlCommand cmd = new SqlCommand(@"
SELECT 1
FROM dbo.Specialization_Courses
WHERE SpecializationID = @sid AND CourseCode = @code;", con, tx))
            {
                cmd.Parameters.AddWithValue("@sid", specId);
                cmd.Parameters.AddWithValue("@code", courseCode);
                return cmd.ExecuteScalar() != null;
            }
        }

        private void InsertCurriculum(SqlConnection con, SqlTransaction tx, int specId, string courseCode, string type)
        {
            string category = type.Equals("Core", StringComparison.OrdinalIgnoreCase)
                ? "Core"
                : "Specialized";

            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Specialization_Courses (SpecializationID, CourseCode, CourseCategory, IsRequired)
VALUES (@sid, @code, @cat, 1);", con, tx))
            {
                cmd.Parameters.AddWithValue("@sid", specId);
                cmd.Parameters.AddWithValue("@code", courseCode);
                cmd.Parameters.AddWithValue("@cat", category);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteCurriculum(SqlConnection con, SqlTransaction tx, int specId, string courseCode)
        {
            using (SqlCommand cmd = new SqlCommand(@"
DELETE FROM dbo.Specialization_Courses
WHERE SpecializationID = @sid AND CourseCode = @code;", con, tx))
            {
                cmd.Parameters.AddWithValue("@sid", specId);
                cmd.Parameters.AddWithValue("@code", courseCode);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Commit any pending checkbox edit
            this.Validate();
            dgvProgramming.EndEdit();
            dgvNetworking.EndEdit();
            dgvCybersecurity.EndEdit();
            dgvDatabase.EndEdit();

            _printAllRows = BuildAllPrintableRows();

            if (_printAllRows.Rows.Count == 0)
            {
                MessageBox.Show("No courses are selected in the curriculum.", "Print",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printAllRows.DefaultView.Sort = "Spec ASC, CourseCode ASC";
            _printAllRows = _printAllRows.DefaultView.ToTable();

            _printRowIndex = 0;
            _preview.ShowDialog();
        }

        private DataTable BuildAllPrintableRows()
        {
            var dt = new DataTable();
            dt.Columns.Add("Spec");       // Programming / Networking / ...
            dt.Columns.Add("CourseCode");
            dt.Columns.Add("CourseName");
            dt.Columns.Add("Credits");
            dt.Columns.Add("Type");

            AddCheckedFromGrid(dt, dgvProgramming, "Programming");
            AddCheckedFromGrid(dt, dgvNetworking, "Networking");
            AddCheckedFromGrid(dt, dgvCybersecurity, "Cybersecurity");
            AddCheckedFromGrid(dt, dgvDatabase, "Database");

            return dt;
        }

        private void AddCheckedFromGrid(DataTable dt, DataGridView grid, string specName)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;

                bool inCurr = row.Cells["InCurriculum"].Value is bool b && b;
                if (!inCurr) continue;

                dt.Rows.Add(
                    specName,
                    row.Cells["CourseCode"].Value?.ToString() ?? "",
                    row.Cells["CourseName"].Value?.ToString() ?? "",
                    row.Cells["Credits"].Value?.ToString() ?? "",
                    row.Cells["Type"].Value?.ToString() ?? ""
                );
            }
        }
        private void PrintDoc_PrintPage_All(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int left = e.MarginBounds.Left;
            int top = e.MarginBounds.Top;
            int width = e.MarginBounds.Width;

            var titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
            var sectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
            var headerFont = new Font("Segoe UI", 10, FontStyle.Bold);
            var normalFont = new Font("Segoe UI", 10, FontStyle.Regular);

            int y = top;

            // Title (only on first page)
            if (_printRowIndex == 0)
            {
                g.DrawString("SCAIS - Curriculum Report (All Specializations)", titleFont, Brushes.Black, left, y);
                y += 35;
                g.DrawString($"Printed on: {DateTime.Now:MMM dd, yyyy}", normalFont, Brushes.Black, left, y);
                y += 20;
                g.DrawLine(Pens.Black, left, y, left + width, y);
                y += 20;
            }

            int colSpec = left;
            int colCode = left + 140;
            int colCredits = left + 250;
            int colType = left + 320;
            int colName = left + 430;

            int lineHeight = 20;
            int maxY = e.MarginBounds.Bottom - 40;

            string currentSpec = "";
            if (_printRowIndex > 0 && _printRowIndex < _printAllRows.Rows.Count)
                currentSpec = _printAllRows.Rows[_printRowIndex - 1]["Spec"].ToString();

            while (_printRowIndex < _printAllRows.Rows.Count)
            {
                var r = _printAllRows.Rows[_printRowIndex];
                string spec = r["Spec"].ToString();

                // If specialization changed, print a section header
                if (!spec.Equals(currentSpec, StringComparison.OrdinalIgnoreCase))
                {
                    // Page break before new section if needed
                    if (y + 60 > maxY)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    currentSpec = spec;

                    g.DrawString(spec, sectionFont, Brushes.Black, left, y);
                    y += 25;

                    // Table header
                    g.DrawString("Course Code", headerFont, Brushes.Black, colCode, y);
                    g.DrawString("Credits", headerFont, Brushes.Black, colCredits, y);
                    g.DrawString("Type", headerFont, Brushes.Black, colType, y);
                    g.DrawString("Course Name", headerFont, Brushes.Black, colName, y);
                    y += 18;

                    g.DrawLine(Pens.Gray, left, y, left + width, y);
                    y += 10;
                }

                // Page break if needed
                if (y + lineHeight > maxY)
                {
                    e.HasMorePages = true;
                    return;
                }

                string code = r["CourseCode"].ToString();
                string credits = r["Credits"].ToString();
                string type = r["Type"].ToString();
                string name = r["CourseName"].ToString();

                if (name.Length > 45) name = name.Substring(0, 45) + "...";

                g.DrawString(code, normalFont, Brushes.Black, colCode, y);
                g.DrawString(credits, normalFont, Brushes.Black, colCredits, y);
                g.DrawString(type, normalFont, Brushes.Black, colType, y);
                g.DrawString(name, normalFont, Brushes.Black, colName, y);

                y += lineHeight;
                _printRowIndex++;
            }

            e.HasMorePages = false;
        }



    }
}

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
    public partial class AdminManageUsersPage : UserControl
    {
        private DataTable _usersTable;
        public event Action<string> EditUserRequested; // sends UserID
        public event Action AddUserRequested;

        public AdminManageUsersPage()
        {
            InitializeComponent();
            SetupUi();
            SetupGrid();
            btnAddUser.Click += (s, e) => AddUserRequested?.Invoke();

        }

        private void AdminManageUsersPage_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadUsers();
        }
        private void SetupUi()
        {
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadRoles()
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("All");
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Adviser");
            cmbRole.Items.Add("Student");
            cmbRole.SelectedIndex = 0;
        }

        // ---------- GRID ----------
        private void SetupGrid()
        {
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colUserID",
                HeaderText = "User ID",
                DataPropertyName = "UserID",
                FillWeight = 12
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFullName",
                HeaderText = "Full Name",
                DataPropertyName = "FullName",
                FillWeight = 25
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRole",
                HeaderText = "Role",
                DataPropertyName = "Role",
                FillWeight = 12
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colUsername",
                HeaderText = "Username",
                DataPropertyName = "Username",
                FillWeight = 15
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                DataPropertyName = "Status",
                FillWeight = 12
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

            dgvUsers.Columns.Add(editBtn);

            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            dgvUsers.CellContentClick += dgvUsers_CellContentClick;

        }

        private void AddEditButtonColumn()
        {
            if (dgvUsers.Columns.Contains("colEdit")) return;

            var editBtn = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
            };

            dgvUsers.Columns.Add(editBtn);
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUsers.Columns[e.ColumnIndex].Name != "colEdit") return;

            string userId = dgvUsers.Rows[e.RowIndex].Cells["colUserID"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(userId)) return;

            EditUserRequested?.Invoke(userId);
        }



        // Color role/status like your screenshot (simple style)
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns[e.ColumnIndex].Name == "colRole" && e.Value != null)
            {
                string role = e.Value.ToString();
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (role.Equals("Adviser", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Honeydew;
                    e.CellStyle.ForeColor = Color.DarkGreen;
                    e.CellStyle.SelectionBackColor = Color.Honeydew;
                    e.CellStyle.SelectionForeColor = Color.DarkGreen;
                }
                else if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.AliceBlue;
                    e.CellStyle.ForeColor = Color.RoyalBlue;
                    e.CellStyle.SelectionBackColor = Color.AliceBlue;
                    e.CellStyle.SelectionForeColor = Color.RoyalBlue;
                }
            }

            if (dgvUsers.Columns[e.ColumnIndex].Name == "colStatus" && e.Value != null)
            {
                string status = e.Value.ToString();
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Honeydew;
                    e.CellStyle.ForeColor = Color.DarkGreen;
                    e.CellStyle.SelectionBackColor = Color.Honeydew;
                    e.CellStyle.SelectionForeColor = Color.DarkGreen;
                }
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.MistyRose;
                    e.CellStyle.ForeColor = Color.DarkRed;
                    e.CellStyle.SelectionBackColor = Color.MistyRose;
                    e.CellStyle.SelectionForeColor = Color.DarkRed;
                }
            }

            // Center edit icon
            if (dgvUsers.Columns[e.ColumnIndex].Name == "colEdit")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.Padding = new Padding(4);
            }
        }

        // ---------- LOAD DATA ----------
        private void LoadUsers()
        {
            string role = cmbRole.SelectedItem?.ToString() ?? "All";
            string search = (txtSearch.Text ?? "").Trim();

            string sql = @"
SELECT 
    UserID,
    FullName,
    Role,
    Username,
    [Status]
FROM dbo.Users
WHERE
    (@role = 'All' OR Role = @role)
AND (
    @search = '' OR
    UserID   LIKE '%' + @search + '%' OR
    FullName LIKE '%' + @search + '%' OR
    Username LIKE '%' + @search + '%' OR
    Email    LIKE '%' + @search + '%'
)
ORDER BY UserID;";

            _usersTable = Db.Query(sql,
                new SqlParameter("@role", role),
                new SqlParameter("@search", search)
            );

            dgvUsers.DataSource = _usersTable;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

                private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadUsers();
            }
        }

        public void ReloadUsers()
        {
            LoadUsers(); // your existing method that fills dgv
        }

    }
}

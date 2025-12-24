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
using SCAIS.Model;


namespace SCAIS.Admin.Pages
{
    public partial class AdminManageUsersPage : UserControl
    {

        private List<User> allusers = new List<User>(); // this all users 
        private List<User> users = new List<User>(); //this filltered users

        public AdminManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();

            // Wire up events
            txtSearch.TextChanged += TxtSearch_TextChanged;
            roleFilter.SelectedIndexChanged += RoleFilter_SelectedIndexChanged;

        }

        private void AdminManageUsersPage_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void userGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadUsers()
        {
            string sql = @"
                SELECT 
                    UserID,
                    Username,
                    FullName,
                    Role,
                    [Status]
                FROM dbo.Users
                ORDER BY FullName;
            ";

            DataTable dt = Db.Query(sql);

            allusers.Clear();
            users.Clear();

            foreach (DataRow r in dt.Rows)
            {
                User user = new User
                {
                    UserID = r["UserID"].ToString(),
                    Username = r["Username"].ToString(),
                    FullName = r["FullName"].ToString(),
                    Role = r["Role"].ToString(),
                    Status = r["Status"].ToString()
                };

                allusers.Add(user);
            }

            users = new List<User>(allusers);
            FillGrid(users);

            if (roleFilter.Items.Count > 0)
                roleFilter.SelectedIndex = 0; 
        }

        private void FillGrid(List<User> list)
        {
            userGridView.Rows.Clear();

            foreach (var u in list)
            {
                userGridView.Rows.Add(
                    u.UserID,
                    u.FullName,
                    u.Role,
                    u.Username,
                    u.Status
                );
            }
        }

    

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void RoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // Apply both username search and role filter
        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string role = roleFilter.SelectedItem?.ToString() ?? "All User";

            var filtered = allusers.Where(u =>
                (role == "All User" || u.Role.Equals(role, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(search) || u.Username.ToLower().Contains(search))
            ).ToList();

            users = filtered;
            FillGrid(users);
        }

    }
}
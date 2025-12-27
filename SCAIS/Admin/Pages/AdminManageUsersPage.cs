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
        private int currentPage = 1;
        private int pageSize = 20; // 10 users per page
        private int totalPages = 1;



        public AdminManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();

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
            // Ignore header clicks
            if (e.RowIndex < 0) return;

            var userId = userGridView.Rows[e.RowIndex].Cells["UserID"].Value.ToString();

            if (userGridView.Columns[e.ColumnIndex].Name == "Edit")
            {
                string fullName = userGridView.Rows[e.RowIndex].Cells["FullName"].Value?.ToString() ?? "";
                string role = userGridView.Rows[e.RowIndex].Cells["Role"].Value?.ToString() ?? "";
                string username = userGridView.Rows[e.RowIndex].Cells["Username"].Value?.ToString() ?? "";
                string status = userGridView.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? "";

                // update database
                string sql = @"
                    UPDATE dbo.Users
                    SET FullName = @FullName,
                        Role = @Role,
                        Username = @Username,
                        [Status] = @Status
                    WHERE UserID = @UserID
                ";

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Status", status),
                    new SqlParameter("@UserID", userId)
                };

                Db.Query(sql, param);

                // Update local list
                var user = allusers.FirstOrDefault(u => u.UserID == userId);
                if (user != null)
                {
                    user.FullName = fullName;
                    user.Role = role;
                    user.Username = username;
                    user.Status = status;
                }

                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK);

                // Refresh grid
                FillGrid(users);
            }

            if (userGridView.Columns[e.ColumnIndex].Name == "Delete")
            {
                DeleteUser(userId);
            }
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


            if (list == null || list.Count == 0)
            {
                userNumLab.Text = "0 of 0 users";
                return;
            }

            totalPages = (int)Math.Ceiling(list.Count / (double)pageSize);

            //  currentPage
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            //for pagination
            var pagedUsers = list
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var u in pagedUsers)
            {
                userGridView.Rows.Add(
                    u.UserID,
                    u.FullName,
                    u.Role,
                    u.Username,
                    u.Status,
                    "Edit",
                    "Delete"
                );
            }

            // update label
            int start = (currentPage - 1) * pageSize + 1;
            int end = start + pagedUsers.Count - 1;
            userNumLab.Text = $"{start}-{end} of {list.Count} users";

            // Enable/disable buttons
            backBtn.Enabled = currentPage > 1;
            NextBtn.Enabled = currentPage < totalPages;
        }




        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void RoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //  both username search and role filter
        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string role = roleFilter.SelectedItem?.ToString() ?? "All User";

            var filtered = allusers.Where(u =>
                (role == "All User" || u.Role.Equals(role, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(search) || u.Username.ToLower().Contains(search))
            ).ToList();

            users = filtered;

            currentPage = 1;
            FillGrid(users);
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                FillGrid(users);
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                FillGrid(users);
            }
        }

        private void userNumLab_Click(object sender, EventArgs e)
        {

        }

        // Delete user method
        private void DeleteUser(string userId)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Delete from database
                string sql = "DELETE FROM dbo.Users WHERE UserID = @UserID";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@UserID", userId)
                };
                Db.Query(sql, param);

                // Remove from local lists
                var user = allusers.FirstOrDefault(u => u.UserID == userId);
                if (user != null)
                    allusers.Remove(user);

                users.RemoveAll(u => u.UserID == userId);

                // Refresh grid
                FillGrid(users);
            }
        }

        private void AddUserBtn_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            AdminAddUserPage addUserPage = new AdminAddUserPage();
            addUserPage.Dock = DockStyle.Fill; // fill the form
            form.Controls.Add(addUserPage);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(600, 400); // adjust size as needed
            form.Show(); // show the form
        }

    }
}

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
    public partial class AdminEditUsersPage : UserControl
    {
        public event Action BackRequested;
        private readonly string _userId;
        private string _currentStatus = "Active";

        public event Action UserChanged;
        public event Action UserUpdated;

        public AdminEditUsersPage(string userId)
        {
            InitializeComponent();
            _userId = userId;

            this.Load += AdminEditUsersPage_Load;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AdminEditUsersPage_Load(object sender, EventArgs e)
        {
            LoadUser(_userId);

        }
        private void LoadUser(string userId)
        {
            string sql = @"
SELECT UserID, Username, [Password], FullName, Email, Role, [Status]
FROM dbo.Users
WHERE UserID = @id;";

            var dt = Db.Query(sql, new SqlParameter("@id", userId));

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("User not found.", "Edit User",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = dt.Rows[0];

            lblUserID.Text = r["UserID"].ToString();
            lblRole.Text = r["Role"].ToString();

            txtUsername.Text = r["Username"].ToString();
            txtPassword.Text = r["Password"].ToString();
            txtFullName.Text = r["FullName"].ToString();
            txtEmail.Text = r["Email"].ToString();

            // ✅ ADD THIS
            _currentStatus = r["Status"]?.ToString() ?? "Active";
            UpdateActionButtons();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
    string.IsNullOrWhiteSpace(txtFullName.Text) ||
    string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill Username, Full Name, and Email.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sql = @"
UPDATE dbo.Users
SET Username = @u,
    [Password] = @p,
    FullName = @fn,
    Email = @em
WHERE UserID = @id;";

            try
            {
                Db.Execute(sql,
                    new SqlParameter("@u", txtUsername.Text.Trim()),
                    new SqlParameter("@p", txtPassword.Text),         // (hash if your system uses hashing)
                    new SqlParameter("@fn", txtFullName.Text.Trim()),
                    new SqlParameter("@em", txtEmail.Text.Trim()),
                    new SqlParameter("@id", _userId)
                );

                MessageBox.Show("Saved ✅", "Edit User",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserChanged?.Invoke();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Edit User",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // assuming you show UserID in a label like lblUserID
            string userId = lblUserID.Text?.Trim();

            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("UserID not found.", "Deactivate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to deactivate this user?\n\nUserID: {userId}",
                "Confirm Deactivate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                string sql = @"
UPDATE dbo.Users
SET [Status] = 'Inactive'
WHERE UserID = @id;";

                int rows = Db.Execute(sql, new SqlParameter("@id", userId));

                if (rows > 0)
                {
                    MessageBox.Show("User deactivated ✅", "Deactivate",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _currentStatus = "Inactive";      // ✅ ADD
                    UpdateActionButtons();            // ✅ ADD

                    UserUpdated?.Invoke();
                    BackRequested?.Invoke();
                }
                else
                {
                    MessageBox.Show("No user was updated. Check the UserID.", "Deactivate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deactivate failed: " + ex.Message, "Deactivate",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BackRequested?.Invoke();

        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            string userId = lblUserID.Text?.Trim();

            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("UserID not found.", "Activate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Activate this user?\n\nUserID: {userId}",
                "Confirm Activate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                string sql = @"
UPDATE dbo.Users
SET [Status] = 'Active'
WHERE UserID = @id;";

                int rows = Db.Execute(sql, new SqlParameter("@id", userId));

                if (rows > 0)
                {
                    MessageBox.Show("User activated ✅", "Activate",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _currentStatus = "Active";
                    UpdateActionButtons();

                    UserUpdated?.Invoke();
                    BackRequested?.Invoke(); // remove if you want to stay on page
                }
                else
                {
                    MessageBox.Show("No user was updated. Check the UserID.", "Activate",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Activate failed: " + ex.Message, "Activate",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateActionButtons()
        {
            bool isActive = _currentStatus.Equals("Active", StringComparison.OrdinalIgnoreCase);

            // Active => show Deactivate button (btnDelete), hide Activate
            btnDelete.Visible = isActive;
            btnActivate.Visible = !isActive;
        }

    }
}

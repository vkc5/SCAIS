using SCAIS.Core.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SCAIS.Admin.Pages
{
    public partial class AdminAddUserPage : UserControl
    {
        public AdminAddUserPage()
        {
            InitializeComponent();
        }

        private void AdminAddUserPage_Load(object sender, EventArgs e)
        {
            // Page load
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool isValid = true;

            // validation
            if (string.IsNullOrWhiteSpace(Usernametext.Text))
            {
                errorProvider1.SetError(Usernametext, "Username is required");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Passtext.Text))
            {
                errorProvider1.SetError(Passtext, "Password is required");
                isValid = false;
            }

            if (Passtext.Text.Length < 8)
            {
                errorProvider1.SetError(Passtext, "Password must be at least 8 characters");
                MessageBox.Show("Password must be at least 8 characters");
                isValid = false;
            }

            if (Passtext.Text != ConPasstext.Text)
            {
                errorProvider1.SetError(ConPasstext, "Passwords do not match");
                MessageBox.Show("Passwords do not match");
                isValid = false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(Emailtext.Text))
            {
                errorProvider1.SetError(Emailtext, "Email is required");
                isValid = false;
            }
            else if (!Emailtext.Text.Contains("@") || !Emailtext.Text.EndsWith(".com", StringComparison.OrdinalIgnoreCase))
            {
                errorProvider1.SetError(Emailtext, "Email must be valid and end with .com");
                MessageBox.Show("Email must be valid and end with .com");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Fullnametext.Text))
            {
                errorProvider1.SetError(Fullnametext, "Full name is required");
                isValid = false;
            }

            if (RoleCombo.SelectedIndex < 0)
            {
                errorProvider1.SetError(RoleCombo, "Role must be selected");
                isValid = false;
            }

            if (StatusCombo.SelectedIndex < 0)
            {
                errorProvider1.SetError(StatusCombo, "Status must be selected");
                isValid = false;
            }

            if (!isValid) return;

            // Hash password
            string hashedPassword = null;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(Passtext.Text);
                var hash = sha256.ComputeHash(bytes);
                hashedPassword = Convert.ToBase64String(hash);
            }

            // Ensure unique username
            string originalUsername = Usernametext.Text.Trim();
            string finalUsername = originalUsername;
            int usernameCounter = 1;

            while (true)
            {
                string checkSql = "SELECT COUNT(*) FROM dbo.Users WHERE Username = @Username";
                DataTable dtCheck = Db.Query(checkSql, new SqlParameter("@Username", finalUsername));

                if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
                {
                    finalUsername = originalUsername + usernameCounter.ToString();
                    usernameCounter++;
                }
                else
                {
                    break;
                }
            }

            // Ensure email is unique
            string emailCheckSql = "SELECT COUNT(*) FROM dbo.Users WHERE Email = @Email";
            DataTable dtEmail = Db.Query(emailCheckSql, new SqlParameter("@Email", Emailtext.Text.Trim()));
            if (dtEmail.Rows.Count > 0 && Convert.ToInt32(dtEmail.Rows[0][0]) > 0)
            {
                errorProvider1.SetError(Emailtext, "Email is already used");
                MessageBox.Show("This email is already used. Please use a different email.");
                return; // stop insertion
            }

            // INSERT
            try
            {
                string role = RoleCombo.SelectedItem.ToString();
                string userId = GenerateUserID(role);

                string sql = @"
INSERT INTO dbo.Users
(UserID, Username, [Password], FullName, Email, Role, [Status], DateCreated, LastLogin)
VALUES
(@UserID, @Username, @Password, @FullName, @Email, @Role, @Status, SYSDATETIME(), NULL);
";

                SqlParameter[] param =
                {
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@Username", finalUsername),
                    new SqlParameter("@Password", hashedPassword),
                    new SqlParameter("@FullName", Fullnametext.Text.Trim()),
                    new SqlParameter("@Email", Emailtext.Text.Trim()),
                    new SqlParameter("@Role", role),
                    new SqlParameter("@Status", StatusCombo.SelectedItem.ToString())
                };

                int rowsAffected = Db.Execute(sql, param);
                MessageBox.Show("Rows affected: " + rowsAffected);

                if (rowsAffected == 1)
                {
                    MessageBox.Show(
                        $"User added successfully!\nUserID: {userId}\nUsername: {finalUsername}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    ClearForm();
                }
                else
                {
                    MessageBox.Show(
                        "Insert failed. No rows affected.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    $"SQL ERROR {ex.Number}\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string GenerateUserID(string role)
        {
            string prefix =
                role == "Adviser" ? "ADV" :
                role == "Student" ? "STU" :
                "USR";

            string sql = @"
SELECT COUNT(*) AS Total
FROM dbo.Users
WHERE UserID LIKE @Prefix + '%'";

            DataTable dt = Db.Query(sql, new SqlParameter("@Prefix", prefix));
            int nextNumber = 1;
            if (dt.Rows.Count > 0)
            {
                nextNumber = Convert.ToInt32(dt.Rows[0]["Total"]) + 1;
            }

            return $"{prefix}{nextNumber:D3}";
        }

        private void ClearForm()
        {
            Usernametext.Clear();
            Passtext.Clear();
            ConPasstext.Clear();
            Fullnametext.Clear();
            Emailtext.Clear();
            RoleCombo.SelectedIndex = -1;
            StatusCombo.SelectedIndex = -1;
        }
    }
}

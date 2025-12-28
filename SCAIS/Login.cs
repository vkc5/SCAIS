using SCAIS.Admin;
using SCAIS.Adviser.Pages;
using SCAIS.Student;
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
using SCAIS.Adviser.Pages;
using SCAIS.Admin;
using SCAIS.Student;

namespace SCAIS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtPassword.UseSystemPasswordChar = true;
            btnLogin.Click += btnLogin_Click;
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnLogin.PerformClick(); };

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = (txtUserId.Text ?? "").Trim();
            string pass = txtPassword.Text ?? "";

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Please enter User ID and Password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 1) get user
                var dt = Db.Query(@"
SELECT UserID, [Password], Role, [Status]
FROM dbo.Users
WHERE UserID = @id;",
                    new SqlParameter("@id", id)
                );

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid ID or Password.", "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var r = dt.Rows[0];
                string dbPass = r["Password"].ToString();
                string role = r["Role"].ToString();
                string status = r["Status"].ToString();

                // 2) active check
                if (!status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Your account is inactive. Please contact the administrator.", "Login Blocked",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3) password check (plain)
                if (!pass.Equals(dbPass))
                {
                    MessageBox.Show("Invalid ID or Password.", "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4) optional: update last login
                Db.Execute(@"UPDATE dbo.Users SET LastLogin = SYSDATETIME() WHERE UserID = @id;",
                    new SqlParameter("@id", id));

                // 5) open role form
                OpenRoleForm(role, id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message, "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenRoleForm(string role, string userId)
        {
            Form next;

            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                next = new SCAIS.Admin.AdminMainForm();
            else if (role.Equals("Adviser", StringComparison.OrdinalIgnoreCase))
                next = new SCAIS.Adviser.Pages.AdviserMainForm(userId); // if you have it
            else if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                next = new SCAIS.Student.StudentMainForm(userId); // if you have it
            else
            {
                MessageBox.Show("Unknown role: " + role, "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // show next, hide login
            next.FormClosed += (s, e) => this.Show(); // when user closes, show login again
            this.Hide();
            next.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

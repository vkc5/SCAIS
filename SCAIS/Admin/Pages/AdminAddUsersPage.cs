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
    public partial class AdminAddUsersPage : UserControl
    {
        public event Action BackRequested;
        public event Action UserCreated; // to refresh ManageUsers grid after creating

        public AdminAddUsersPage()
        {
            InitializeComponent();


            // UI setup
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpecialization.DropDownStyle = ComboBoxStyle.DropDownList;

            pnlAdviser.Visible = false;
            pnlStudent.Visible = false;

            this.Load += AdminAddUsersPage_Load;
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
            btnCreate.Click += btnCreate_Click;
            // btnBack.Click += (s,e)=> BackRequested?.Invoke();
        }

        private void AdminAddUsersPage_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadSpecializations();

            cmbRole.SelectedIndex = 0; // triggers role logic + ID generation
        }
        private void LoadRoles()
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Adviser");
            cmbRole.Items.Add("Student");
        }

        private void LoadSpecializations()
        {
            string sql = @"
SELECT SpecializationID, SpecializationName
FROM dbo.Specializations
WHERE IsActive = 1
ORDER BY SpecializationName;";

            DataTable dt = Db.Query(sql);

            cmbSpecialization.DataSource = dt;
            cmbSpecialization.DisplayMember = "SpecializationName";
            cmbSpecialization.ValueMember = "SpecializationID";
            cmbSpecialization.SelectedIndex = dt.Rows.Count > 0 ? 0 : -1;
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = cmbRole.SelectedItem?.ToString() ?? "Admin";

            // Show/Hide panels
            pnlAdviser.Visible = role.Equals("Adviser", StringComparison.OrdinalIgnoreCase);
            pnlStudent.Visible = role.Equals("Student", StringComparison.OrdinalIgnoreCase);

            // Generate ID preview immediately
            string prefix = GetPrefixForRole(role);
            lblUserID.Text = GenerateNextUserId(prefix);
        }

        private string GetPrefixForRole(string role)
        {
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return "ADM";
            if (role.Equals("Adviser", StringComparison.OrdinalIgnoreCase)) return "ADV";
            return "STU";
        }

        // Generates next ID like ADV001, ADV002... based on existing max in Users table
        private string GenerateNextUserId(string prefix)
        {
            // NOTE: this is "preview". Real uniqueness is enforced again during INSERT (transaction).
            string sql = @"
SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(UserID, 4, 10) AS INT)), 0)
FROM dbo.Users
WHERE UserID LIKE @p + '%';";

            object result = Db.Scalar(sql, new SqlParameter("@p", prefix));
            int maxNum = Convert.ToInt32(result);
            int nextNum = maxNum + 1;

            return $"{prefix}{nextNum:000}";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string role = cmbRole.SelectedItem?.ToString() ?? "Admin";

            // Basic validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill Username, Password, Full Name, and Email.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Extra validation based on role
            if (role.Equals("Adviser", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(txtDepartment.Text))
                {
                    MessageBox.Show("Please enter Department for Adviser.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (cmbSpecialization.SelectedValue == null)
                {
                    MessageBox.Show("Please select Specialization for Student.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            string prefix = GetPrefixForRole(role);

            // IMPORTANT: do the final create inside a SERIALIZABLE transaction to keep IDs unique (no duplicates)
            using (SqlConnection con = new SqlConnection(Db.ConnStr))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        // 1) Generate next id WITH LOCK
                        string id = GenerateNextUserId_Locked(con, tx, prefix);
                        lblUserID.Text = id;

                        // 2) Insert into Users
                        using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Users(UserID, Username, [Password], FullName, Email, Role, [Status])
VALUES(@id, @un, @pw, @fn, @em, @role, 'Active');", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@un", txtUsername.Text.Trim());
                            cmd.Parameters.AddWithValue("@pw", txtPassword.Text); // hash later if you want
                            cmd.Parameters.AddWithValue("@fn", txtFullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@em", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@role", role);
                            cmd.ExecuteNonQuery();
                        }

                        // 3) Insert into Advisers / Students if needed
                        if (role.Equals("Adviser", StringComparison.OrdinalIgnoreCase))
                        {
                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Advisers(AdviserID, UserID, Department, OfficeLocation, OfficeHours, MaxAdvisees)
VALUES(@aid, @uid, @dep, @loc, @hrs, 30);", con, tx))
                            {
                                cmd.Parameters.AddWithValue("@aid", id);
                                cmd.Parameters.AddWithValue("@uid", id);
                                cmd.Parameters.AddWithValue("@dep", (txtDepartment.Text ?? "").Trim());
                                cmd.Parameters.AddWithValue("@loc", (txtOfficeLocation.Text ?? "").Trim());
                                cmd.Parameters.AddWithValue("@hrs", (txtOfficeHours.Text ?? "").Trim());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                        {
                            int specId = Convert.ToInt32(cmbSpecialization.SelectedValue);

                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.Students(StudentID, UserID, SpecializationID, EnrollmentDate, CurrentSemester)
VALUES(@sid, @uid, @spec, CAST(GETDATE() AS date), 'Fall 2025');", con, tx))
                            {
                                cmd.Parameters.AddWithValue("@sid", id);
                                cmd.Parameters.AddWithValue("@uid", id);
                                cmd.Parameters.AddWithValue("@spec", specId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();

                        MessageBox.Show($"User created ✅\nUserID: {id}", "Add User",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UserCreated?.Invoke();   // refresh ManageUsers
                        // BackRequested?.Invoke(); // optional: auto back to list

                        ClearFormButKeepRole();  // optional
                        lblUserID.Text = GenerateNextUserId(prefix); // next preview
                    }
                    catch (SqlException ex)
                    {
                        tx.Rollback();
                        MessageBox.Show("Create failed: " + ex.Message, "Add User",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        // Locked ID generation (prevents 2 users getting same next number)
        private string GenerateNextUserId_Locked(SqlConnection con, SqlTransaction tx, string prefix)
        {
            // UPDLOCK + HOLDLOCK ensures safe "max+1" under Serializable transaction
            using (SqlCommand cmd = new SqlCommand(@"
SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(UserID, 4, 10) AS INT)), 0)
FROM dbo.Users WITH (UPDLOCK, HOLDLOCK)
WHERE UserID LIKE @p + '%';", con, tx))
            {
                cmd.Parameters.AddWithValue("@p", prefix);
                int maxNum = Convert.ToInt32(cmd.ExecuteScalar());
                return $"{prefix}{(maxNum + 1):000}";
            }
        }

        private void ClearFormButKeepRole()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtEmail.Clear();

            txtDepartment.Clear();
            txtOfficeLocation.Clear();
            txtOfficeHours.Clear();

            if (cmbSpecialization.Items.Count > 0) cmbSpecialization.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BackRequested?.Invoke();
        }
    }
}

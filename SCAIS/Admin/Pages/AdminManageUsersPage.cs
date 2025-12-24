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
        public AdminManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();

        }

        private void AdminManageUsersPage_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }

        private void userGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadUsers()
        {
            string sql = @"
                        SELECT 
                            UserID,
                            FullName,
                            Role,
                            Username,
                            [Status]
                        FROM dbo.Users
                        ORDER BY FullName;
                        ";

            DataTable dt = Db.Query(sql);

            userGridView.Rows.Clear();

            foreach (DataRow r in dt.Rows)
            {
                userGridView.Rows.Add(
                    r["UserID"].ToString(),
                    r["FullName"].ToString(),
                    r["Role"].ToString(),
                    r["Username"].ToString(),
                    r["Status"].ToString()
                );
            }
        }

    }
}
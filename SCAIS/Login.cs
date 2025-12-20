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

namespace SCAIS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new StudentMainForm().Show();
            this.Hide(); // hide login

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AdviserMainForm().Show();
            this.Hide(); // hide login
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AdminMainForm().Show();
            this.Hide(); // hide login
        }
    }
}

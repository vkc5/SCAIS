namespace SCAIS.Admin.Pages
{
    partial class AdminAddUserPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Addpnl = new System.Windows.Forms.Panel();
            this.CreateBtn = new System.Windows.Forms.Button();
            this.StatusCombo = new System.Windows.Forms.ComboBox();
            this.RoleCombo = new System.Windows.Forms.ComboBox();
            this.Emailtext = new System.Windows.Forms.TextBox();
            this.Fullnametext = new System.Windows.Forms.TextBox();
            this.ConPasstext = new System.Windows.Forms.TextBox();
            this.Passtext = new System.Windows.Forms.TextBox();
            this.Usernametext = new System.Windows.Forms.TextBox();
            this.StatusLab = new System.Windows.Forms.Label();
            this.RoleLab = new System.Windows.Forms.Label();
            this.EmailLab = new System.Windows.Forms.Label();
            this.FullNameLab = new System.Windows.Forms.Label();
            this.ConPassLab = new System.Windows.Forms.Label();
            this.PassLab = new System.Windows.Forms.Label();
            this.UsernameLab = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.Addpnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(39, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(644, 45);
            this.label3.TabIndex = 2;
            this.label3.Text = "Smart Course Advising Information System";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(42, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Add User";
            // 
            // Addpnl
            // 
            this.Addpnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Addpnl.Controls.Add(this.CreateBtn);
            this.Addpnl.Controls.Add(this.StatusCombo);
            this.Addpnl.Controls.Add(this.RoleCombo);
            this.Addpnl.Controls.Add(this.Emailtext);
            this.Addpnl.Controls.Add(this.Fullnametext);
            this.Addpnl.Controls.Add(this.ConPasstext);
            this.Addpnl.Controls.Add(this.Passtext);
            this.Addpnl.Controls.Add(this.Usernametext);
            this.Addpnl.Controls.Add(this.StatusLab);
            this.Addpnl.Controls.Add(this.RoleLab);
            this.Addpnl.Controls.Add(this.EmailLab);
            this.Addpnl.Controls.Add(this.FullNameLab);
            this.Addpnl.Controls.Add(this.ConPassLab);
            this.Addpnl.Controls.Add(this.PassLab);
            this.Addpnl.Controls.Add(this.UsernameLab);
            this.Addpnl.Location = new System.Drawing.Point(177, 142);
            this.Addpnl.Name = "Addpnl";
            this.Addpnl.Size = new System.Drawing.Size(506, 542);
            this.Addpnl.TabIndex = 5;
            // 
            // CreateBtn
            // 
            this.CreateBtn.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CreateBtn.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.CreateBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CreateBtn.Location = new System.Drawing.Point(167, 479);
            this.CreateBtn.Name = "CreateBtn";
            this.CreateBtn.Size = new System.Drawing.Size(160, 45);
            this.CreateBtn.TabIndex = 20;
            this.CreateBtn.Text = "Create";
            this.CreateBtn.UseVisualStyleBackColor = false;
            this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
            // 
            // StatusCombo
            // 
            this.StatusCombo.FormattingEnabled = true;
            this.StatusCombo.Items.AddRange(new object[] {
            "Active",
            "In-Active"});
            this.StatusCombo.Location = new System.Drawing.Point(240, 419);
            this.StatusCombo.Name = "StatusCombo";
            this.StatusCombo.Size = new System.Drawing.Size(233, 21);
            this.StatusCombo.TabIndex = 19;
            // 
            // RoleCombo
            // 
            this.RoleCombo.FormattingEnabled = true;
            this.RoleCombo.Items.AddRange(new object[] {
            "Advisor",
            "Student"});
            this.RoleCombo.Location = new System.Drawing.Point(240, 360);
            this.RoleCombo.Name = "RoleCombo";
            this.RoleCombo.Size = new System.Drawing.Size(233, 21);
            this.RoleCombo.TabIndex = 18;
            // 
            // Emailtext
            // 
            this.Emailtext.Location = new System.Drawing.Point(240, 286);
            this.Emailtext.Name = "Emailtext";
            this.Emailtext.Size = new System.Drawing.Size(233, 20);
            this.Emailtext.TabIndex = 17;
            // 
            // Fullnametext
            // 
            this.Fullnametext.Location = new System.Drawing.Point(240, 224);
            this.Fullnametext.Name = "Fullnametext";
            this.Fullnametext.Size = new System.Drawing.Size(233, 20);
            this.Fullnametext.TabIndex = 16;
            // 
            // ConPasstext
            // 
            this.ConPasstext.Location = new System.Drawing.Point(240, 159);
            this.ConPasstext.Name = "ConPasstext";
            this.ConPasstext.Size = new System.Drawing.Size(233, 20);
            this.ConPasstext.TabIndex = 15;
            this.ConPasstext.UseSystemPasswordChar = true;
            // 
            // Passtext
            // 
            this.Passtext.Location = new System.Drawing.Point(240, 99);
            this.Passtext.Name = "Passtext";
            this.Passtext.Size = new System.Drawing.Size(233, 20);
            this.Passtext.TabIndex = 14;
            this.Passtext.UseSystemPasswordChar = true;
            // 
            // Usernametext
            // 
            this.Usernametext.Location = new System.Drawing.Point(240, 31);
            this.Usernametext.Name = "Usernametext";
            this.Usernametext.Size = new System.Drawing.Size(233, 20);
            this.Usernametext.TabIndex = 13;
            // 
            // StatusLab
            // 
            this.StatusLab.AutoSize = true;
            this.StatusLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.StatusLab.Location = new System.Drawing.Point(48, 416);
            this.StatusLab.Name = "StatusLab";
            this.StatusLab.Size = new System.Drawing.Size(70, 25);
            this.StatusLab.TabIndex = 12;
            this.StatusLab.Text = "Status:";
            // 
            // RoleLab
            // 
            this.RoleLab.AutoSize = true;
            this.RoleLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.RoleLab.Location = new System.Drawing.Point(48, 360);
            this.RoleLab.Name = "RoleLab";
            this.RoleLab.Size = new System.Drawing.Size(55, 25);
            this.RoleLab.TabIndex = 11;
            this.RoleLab.Text = "Role:";
            // 
            // EmailLab
            // 
            this.EmailLab.AutoSize = true;
            this.EmailLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.EmailLab.Location = new System.Drawing.Point(48, 286);
            this.EmailLab.Name = "EmailLab";
            this.EmailLab.Size = new System.Drawing.Size(64, 25);
            this.EmailLab.TabIndex = 10;
            this.EmailLab.Text = "Email:";
            // 
            // FullNameLab
            // 
            this.FullNameLab.AutoSize = true;
            this.FullNameLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.FullNameLab.Location = new System.Drawing.Point(39, 224);
            this.FullNameLab.Name = "FullNameLab";
            this.FullNameLab.Size = new System.Drawing.Size(95, 25);
            this.FullNameLab.TabIndex = 9;
            this.FullNameLab.Text = "Fullname:";
            // 
            // ConPassLab
            // 
            this.ConPassLab.AutoSize = true;
            this.ConPassLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.ConPassLab.Location = new System.Drawing.Point(39, 154);
            this.ConPassLab.Name = "ConPassLab";
            this.ConPassLab.Size = new System.Drawing.Size(171, 25);
            this.ConPassLab.TabIndex = 8;
            this.ConPassLab.Text = "Confirm Password:";
            // 
            // PassLab
            // 
            this.PassLab.AutoSize = true;
            this.PassLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.PassLab.Location = new System.Drawing.Point(39, 94);
            this.PassLab.Name = "PassLab";
            this.PassLab.Size = new System.Drawing.Size(97, 25);
            this.PassLab.TabIndex = 7;
            this.PassLab.Text = "Password:";
            // 
            // UsernameLab
            // 
            this.UsernameLab.AutoSize = true;
            this.UsernameLab.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.UsernameLab.Location = new System.Drawing.Point(39, 26);
            this.UsernameLab.Name = "UsernameLab";
            this.UsernameLab.Size = new System.Drawing.Size(103, 25);
            this.UsernameLab.TabIndex = 6;
            this.UsernameLab.Text = "Username:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AdminAddUserPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Addpnl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "AdminAddUserPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdminAddUserPage_Load);
            this.Addpnl.ResumeLayout(false);
            this.Addpnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel Addpnl;
        private System.Windows.Forms.Label StatusLab;
        private System.Windows.Forms.Label RoleLab;
        private System.Windows.Forms.Label EmailLab;
        private System.Windows.Forms.Label FullNameLab;
        private System.Windows.Forms.Label ConPassLab;
        private System.Windows.Forms.Label PassLab;
        private System.Windows.Forms.Label UsernameLab;
        private System.Windows.Forms.ComboBox StatusCombo;
        private System.Windows.Forms.ComboBox RoleCombo;
        private System.Windows.Forms.TextBox Emailtext;
        private System.Windows.Forms.TextBox Fullnametext;
        private System.Windows.Forms.TextBox ConPasstext;
        private System.Windows.Forms.TextBox Passtext;
        private System.Windows.Forms.TextBox Usernametext;
        private System.Windows.Forms.Button CreateBtn;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}

namespace SCAIS.Admin.Pages
{
    partial class AdminManageUsersPage
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
            this.label3 = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(644, 45);
            this.label3.TabIndex = 1;
            this.label3.Text = "Smart Course Advising Information System";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPageTitle.Location = new System.Drawing.Point(27, 105);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(133, 25);
            this.lblPageTitle.TabIndex = 26;
            this.lblPageTitle.Text = "Manage Users";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(839, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────────────";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.cmbRole);
            this.panel3.Controls.Add(this.btnFilter);
            this.panel3.Controls.Add(this.txtSearch);
            this.panel3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.panel3.Location = new System.Drawing.Point(32, 143);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(857, 73);
            this.panel3.TabIndex = 27;
            // 
            // cmbRole
            // 
            this.cmbRole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbRole.Font = new System.Drawing.Font("Yu Gothic UI", 18.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(470, 16);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(175, 43);
            this.cmbRole.TabIndex = 2;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Location = new System.Drawing.Point(670, 16);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(160, 38);
            this.btnFilter.TabIndex = 1;
            this.btnFilter.Text = "Refresh";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Yu Gothic UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txtSearch.Location = new System.Drawing.Point(15, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(431, 43);
            this.txtSearch.TabIndex = 0;
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(32, 235);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(857, 511);
            this.dgvUsers.TabIndex = 28;
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackColor = System.Drawing.Color.Green;
            this.btnAddUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(735, 761);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(154, 43);
            this.btnAddUser.TabIndex = 31;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = false;
            // 
            // AdminManageUsersPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblPageTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "AdminManageUsersPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdminManageUsersPage_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnAddUser;
    }
}

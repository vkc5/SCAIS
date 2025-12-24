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
            this.label1 = new System.Windows.Forms.Label();
            this.AddUserBtn = new System.Windows.Forms.Button();
            this.searchPnl = new System.Windows.Forms.Panel();
            this.roleFilter = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.userGridView = new System.Windows.Forms.DataGridView();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pagenationPnl = new System.Windows.Forms.Panel();
            this.NextBtn = new System.Windows.Forms.Button();
            this.backBtn = new System.Windows.Forms.Button();
            this.userNumLab = new System.Windows.Forms.Label();
            this.searchPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGridView)).BeginInit();
            this.pagenationPnl.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(67, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Manage Users";
            // 
            // AddUserBtn
            // 
            this.AddUserBtn.BackColor = System.Drawing.Color.ForestGreen;
            this.AddUserBtn.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.AddUserBtn.ForeColor = System.Drawing.Color.White;
            this.AddUserBtn.Location = new System.Drawing.Point(733, 88);
            this.AddUserBtn.Name = "AddUserBtn";
            this.AddUserBtn.Size = new System.Drawing.Size(154, 52);
            this.AddUserBtn.TabIndex = 4;
            this.AddUserBtn.Text = "+ Add User";
            this.AddUserBtn.UseVisualStyleBackColor = false;
            // 
            // searchPnl
            // 
            this.searchPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPnl.Controls.Add(this.roleFilter);
            this.searchPnl.Controls.Add(this.btnFilter);
            this.searchPnl.Controls.Add(this.txtSearch);
            this.searchPnl.Location = new System.Drawing.Point(42, 163);
            this.searchPnl.Name = "searchPnl";
            this.searchPnl.Size = new System.Drawing.Size(845, 75);
            this.searchPnl.TabIndex = 5;
            // 
            // roleFilter
            // 
            this.roleFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.roleFilter.Font = new System.Drawing.Font("Yu Gothic UI", 18.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roleFilter.FormattingEnabled = true;
            this.roleFilter.Items.AddRange(new object[] {
            "All User",
            "Adviser",
            "Student"});
            this.roleFilter.Location = new System.Drawing.Point(492, 16);
            this.roleFilter.Name = "roleFilter";
            this.roleFilter.Size = new System.Drawing.Size(193, 43);
            this.roleFilter.TabIndex = 8;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Location = new System.Drawing.Point(691, 16);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(142, 43);
            this.btnFilter.TabIndex = 7;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Yu Gothic UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txtSearch.Location = new System.Drawing.Point(12, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(471, 43);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // userGridView
            // 
            this.userGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.userGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserID,
            this.FullName,
            this.Role,
            this.Username,
            this.Status,
            this.Action});
            this.userGridView.Location = new System.Drawing.Point(42, 262);
            this.userGridView.Name = "userGridView";
            this.userGridView.Size = new System.Drawing.Size(845, 451);
            this.userGridView.TabIndex = 6;
            this.userGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.userGridView_CellContentClick);
            // 
            // UserID
            // 
            this.UserID.HeaderText = "User ID";
            this.UserID.Name = "UserID";
            // 
            // FullName
            // 
            this.FullName.HeaderText = "Full Name";
            this.FullName.Name = "FullName";
            // 
            // Role
            // 
            this.Role.HeaderText = "Role";
            this.Role.Name = "Role";
            // 
            // Username
            // 
            this.Username.HeaderText = "Username";
            this.Username.Name = "Username";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            // 
            // pagenationPnl
            // 
            this.pagenationPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pagenationPnl.Controls.Add(this.userNumLab);
            this.pagenationPnl.Controls.Add(this.backBtn);
            this.pagenationPnl.Controls.Add(this.NextBtn);
            this.pagenationPnl.Location = new System.Drawing.Point(42, 734);
            this.pagenationPnl.Name = "pagenationPnl";
            this.pagenationPnl.Size = new System.Drawing.Size(845, 74);
            this.pagenationPnl.TabIndex = 7;
            // 
            // NextBtn
            // 
            this.NextBtn.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.NextBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NextBtn.FlatAppearance.BorderSize = 0;
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextBtn.Location = new System.Drawing.Point(728, 15);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(96, 42);
            this.NextBtn.TabIndex = 8;
            this.NextBtn.Text = "Next";
            this.NextBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.NextBtn.UseVisualStyleBackColor = false;
            // 
            // backBtn
            // 
            this.backBtn.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.backBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backBtn.FlatAppearance.BorderSize = 0;
            this.backBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backBtn.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backBtn.Location = new System.Drawing.Point(626, 15);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(96, 42);
            this.backBtn.TabIndex = 9;
            this.backBtn.Text = "Back";
            this.backBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.backBtn.UseVisualStyleBackColor = false;
            // 
            // userNumLab
            // 
            this.userNumLab.AutoSize = true;
            this.userNumLab.Location = new System.Drawing.Point(44, 28);
            this.userNumLab.Name = "userNumLab";
            this.userNumLab.Size = new System.Drawing.Size(79, 13);
            this.userNumLab.TabIndex = 10;
            this.userNumLab.Text = "Number of user";
            // 
            // AdminManageUsersPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pagenationPnl);
            this.Controls.Add(this.userGridView);
            this.Controls.Add(this.searchPnl);
            this.Controls.Add(this.AddUserBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "AdminManageUsersPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdminManageUsersPage_Load);
            this.searchPnl.ResumeLayout(false);
            this.searchPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGridView)).EndInit();
            this.pagenationPnl.ResumeLayout(false);
            this.pagenationPnl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddUserBtn;
        private System.Windows.Forms.Panel searchPnl;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox roleFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.DataGridView userGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.Panel pagenationPnl;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Label userNumLab;
    }
}

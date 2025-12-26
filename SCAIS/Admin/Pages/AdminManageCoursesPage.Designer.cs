namespace SCAIS.Admin.Pages
{
    partial class AdminManageCoursesPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.pagenationPnl = new System.Windows.Forms.Panel();
            this.courseNumLab = new System.Windows.Forms.Label();
            this.backBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.courseGridView = new System.Windows.Forms.DataGridView();
            this.searchPnl = new System.Windows.Forms.Panel();
            this.specialFilter = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.AddUserBtn = new System.Windows.Forms.Button();
            this.MangeCourselabel = new System.Windows.Forms.Label();
            this.CourseCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specialization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prerequisite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Corequisite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pagenationPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.courseGridView)).BeginInit();
            this.searchPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // pagenationPnl
            // 
            this.pagenationPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pagenationPnl.Controls.Add(this.courseNumLab);
            this.pagenationPnl.Controls.Add(this.backBtn);
            this.pagenationPnl.Controls.Add(this.NextBtn);
            this.pagenationPnl.Location = new System.Drawing.Point(44, 747);
            this.pagenationPnl.Name = "pagenationPnl";
            this.pagenationPnl.Size = new System.Drawing.Size(845, 74);
            this.pagenationPnl.TabIndex = 12;
            // 
            // courseNumLab
            // 
            this.courseNumLab.AutoSize = true;
            this.courseNumLab.Location = new System.Drawing.Point(44, 28);
            this.courseNumLab.Name = "courseNumLab";
            this.courseNumLab.Size = new System.Drawing.Size(79, 13);
            this.courseNumLab.TabIndex = 10;
            this.courseNumLab.Text = "Number of user";
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
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click_1);
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
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click_1);
            // 
            // courseGridView
            // 
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            this.courseGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.courseGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.courseGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.courseGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CourseCode,
            this.CourseName,
            this.Specialization,
            this.Prerequisite,
            this.Corequisite,
            this.Edit,
            this.Delete});
            this.courseGridView.Location = new System.Drawing.Point(44, 275);
            this.courseGridView.Name = "courseGridView";
            this.courseGridView.Size = new System.Drawing.Size(845, 451);
            this.courseGridView.TabIndex = 11;
            // 
            // searchPnl
            // 
            this.searchPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPnl.Controls.Add(this.specialFilter);
            this.searchPnl.Controls.Add(this.btnFilter);
            this.searchPnl.Controls.Add(this.txtSearch);
            this.searchPnl.Location = new System.Drawing.Point(44, 176);
            this.searchPnl.Name = "searchPnl";
            this.searchPnl.Size = new System.Drawing.Size(845, 75);
            this.searchPnl.TabIndex = 10;
            // 
            // specialFilter
            // 
            this.specialFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.specialFilter.Font = new System.Drawing.Font("Yu Gothic UI", 18.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.specialFilter.FormattingEnabled = true;
            this.specialFilter.Items.AddRange(new object[] {
            "All Specializations",
            "Core",
            "Cybersecurity",
            "Networking",
            "Database",
            "Programming"});
            this.specialFilter.Location = new System.Drawing.Point(492, 16);
            this.specialFilter.Name = "specialFilter";
            this.specialFilter.Size = new System.Drawing.Size(193, 43);
            this.specialFilter.TabIndex = 8;
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
            // 
            // AddUserBtn
            // 
            this.AddUserBtn.BackColor = System.Drawing.Color.ForestGreen;
            this.AddUserBtn.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.AddUserBtn.ForeColor = System.Drawing.Color.White;
            this.AddUserBtn.Location = new System.Drawing.Point(735, 101);
            this.AddUserBtn.Name = "AddUserBtn";
            this.AddUserBtn.Size = new System.Drawing.Size(154, 52);
            this.AddUserBtn.TabIndex = 9;
            this.AddUserBtn.Text = "+ Add Course";
            this.AddUserBtn.UseVisualStyleBackColor = false;
            // 
            // MangeCourselabel
            // 
            this.MangeCourselabel.AutoSize = true;
            this.MangeCourselabel.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.MangeCourselabel.Location = new System.Drawing.Point(69, 114);
            this.MangeCourselabel.Name = "MangeCourselabel";
            this.MangeCourselabel.Size = new System.Drawing.Size(154, 25);
            this.MangeCourselabel.TabIndex = 8;
            this.MangeCourselabel.Text = "Manage Courses";
            // 
            // CourseCode
            // 
            this.CourseCode.HeaderText = "Course Code";
            this.CourseCode.Name = "CourseCode";
            this.CourseCode.ReadOnly = true;
            // 
            // CourseName
            // 
            this.CourseName.HeaderText = "Course Name";
            this.CourseName.Name = "CourseName";
            // 
            // Specialization
            // 
            this.Specialization.HeaderText = "Specialization";
            this.Specialization.Name = "Specialization";
            // 
            // Prerequisite
            // 
            this.Prerequisite.HeaderText = "Prerequisite";
            this.Prerequisite.Name = "Prerequisite";
            // 
            // Corequisite
            // 
            this.Corequisite.HeaderText = "Corequisite";
            this.Corequisite.Name = "Corequisite";
            // 
            // Edit
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(2);
            this.Edit.DefaultCellStyle = dataGridViewCellStyle11;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Text = "Edit";
            // 
            // Delete
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(2);
            this.Delete.DefaultCellStyle = dataGridViewCellStyle12;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Delete.Text = "Delete";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AdminManageCoursesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pagenationPnl);
            this.Controls.Add(this.courseGridView);
            this.Controls.Add(this.searchPnl);
            this.Controls.Add(this.AddUserBtn);
            this.Controls.Add(this.MangeCourselabel);
            this.Controls.Add(this.label3);
            this.Name = "AdminManageCoursesPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdminManageCoursesPage_Load);
            this.pagenationPnl.ResumeLayout(false);
            this.pagenationPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.courseGridView)).EndInit();
            this.searchPnl.ResumeLayout(false);
            this.searchPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pagenationPnl;
        private System.Windows.Forms.Label courseNumLab;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.DataGridView courseGridView;
        private System.Windows.Forms.Panel searchPnl;
        private System.Windows.Forms.ComboBox specialFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button AddUserBtn;
        private System.Windows.Forms.Label MangeCourselabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourseCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourseName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Specialization;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prerequisite;
        private System.Windows.Forms.DataGridViewTextBoxColumn Corequisite;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}

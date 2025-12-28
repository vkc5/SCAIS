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
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbSpecialization = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvCourses = new System.Windows.Forms.DataGridView();
            this.btnAddCourse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(839, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────────────";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPageTitle.Location = new System.Drawing.Point(27, 103);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(154, 25);
            this.lblPageTitle.TabIndex = 27;
            this.lblPageTitle.Text = "Manage Courses";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.cmbSpecialization);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.txtSearch);
            this.panel3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.panel3.Location = new System.Drawing.Point(32, 140);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(857, 73);
            this.panel3.TabIndex = 28;
            // 
            // cmbSpecialization
            // 
            this.cmbSpecialization.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbSpecialization.Font = new System.Drawing.Font("Yu Gothic UI", 18.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSpecialization.FormattingEnabled = true;
            this.cmbSpecialization.Location = new System.Drawing.Point(470, 16);
            this.cmbSpecialization.Name = "cmbSpecialization";
            this.cmbSpecialization.Size = new System.Drawing.Size(175, 43);
            this.cmbSpecialization.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(670, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(160, 38);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
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
            // dgvCourses
            // 
            this.dgvCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCourses.Location = new System.Drawing.Point(32, 230);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.Size = new System.Drawing.Size(857, 511);
            this.dgvCourses.TabIndex = 29;
            // 
            // btnAddCourse
            // 
            this.btnAddCourse.BackColor = System.Drawing.Color.Green;
            this.btnAddCourse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCourse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCourse.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCourse.ForeColor = System.Drawing.Color.White;
            this.btnAddCourse.Location = new System.Drawing.Point(735, 758);
            this.btnAddCourse.Name = "btnAddCourse";
            this.btnAddCourse.Size = new System.Drawing.Size(154, 43);
            this.btnAddCourse.TabIndex = 32;
            this.btnAddCourse.Text = "Add Course";
            this.btnAddCourse.UseVisualStyleBackColor = false;
            // 
            // AdminManageCoursesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddCourse);
            this.Controls.Add(this.dgvCourses);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblPageTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "AdminManageCoursesPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdminManageCoursesPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbSpecialization;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvCourses;
        private System.Windows.Forms.Button btnAddCourse;
    }
}

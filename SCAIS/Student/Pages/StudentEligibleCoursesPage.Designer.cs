namespace SCAIS.Student.Pages
{
    partial class StudentEligibleCoursesPage
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
            this.label13 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblEligibleCountValue = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBlockedCountValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvEligibleCourses = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvBlockedCourses = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEligibleCourses)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlockedCourses)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(644, 45);
            this.label3.TabIndex = 3;
            this.label3.Text = "Smart Course Advising Information System";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(29, 89);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(839, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────────────";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblEligibleCountValue);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Location = new System.Drawing.Point(32, 153);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 121);
            this.panel4.TabIndex = 19;
            // 
            // lblEligibleCountValue
            // 
            this.lblEligibleCountValue.AutoSize = true;
            this.lblEligibleCountValue.Font = new System.Drawing.Font("Yu Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEligibleCountValue.Location = new System.Drawing.Point(10, 59);
            this.lblEligibleCountValue.Name = "lblEligibleCountValue";
            this.lblEligibleCountValue.Size = new System.Drawing.Size(28, 31);
            this.lblEligibleCountValue.TabIndex = 1;
            this.lblEligibleCountValue.Text = "6";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label15.Location = new System.Drawing.Point(11, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(162, 30);
            this.label15.TabIndex = 0;
            this.label15.Text = "Eligible Courses";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblBlockedCountValue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(474, 153);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 121);
            this.panel1.TabIndex = 20;
            // 
            // lblBlockedCountValue
            // 
            this.lblBlockedCountValue.AutoSize = true;
            this.lblBlockedCountValue.Font = new System.Drawing.Font("Yu Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlockedCountValue.Location = new System.Drawing.Point(10, 59);
            this.lblBlockedCountValue.Name = "lblBlockedCountValue";
            this.lblBlockedCountValue.Size = new System.Drawing.Size(28, 31);
            this.lblBlockedCountValue.TabIndex = 1;
            this.lblBlockedCountValue.Text = "6";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(11, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Blocked Courses";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPageTitle.Location = new System.Drawing.Point(29, 113);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(192, 25);
            this.lblPageTitle.TabIndex = 21;
            this.lblPageTitle.Text = "View Eligible Courses";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dgvEligibleCourses);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(32, 299);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 258);
            this.panel2.TabIndex = 22;
            // 
            // dgvEligibleCourses
            // 
            this.dgvEligibleCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEligibleCourses.Location = new System.Drawing.Point(25, 52);
            this.dgvEligibleCourses.Name = "dgvEligibleCourses";
            this.dgvEligibleCourses.Size = new System.Drawing.Size(784, 175);
            this.dgvEligibleCourses.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(20, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 25);
            this.label7.TabIndex = 18;
            this.label7.Text = "Eligible Courses (Auto-Validated)";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dgvBlockedCourses);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(32, 573);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(836, 258);
            this.panel3.TabIndex = 23;
            // 
            // dgvBlockedCourses
            // 
            this.dgvBlockedCourses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlockedCourses.Location = new System.Drawing.Point(25, 52);
            this.dgvBlockedCourses.Name = "dgvBlockedCourses";
            this.dgvBlockedCourses.Size = new System.Drawing.Size(784, 175);
            this.dgvBlockedCourses.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "Blocked Courses (Prerequisites Not Met)";
            // 
            // StudentEligibleCoursesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblPageTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label3);
            this.Name = "StudentEligibleCoursesPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.StudentEligibleCoursesPage_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEligibleCourses)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlockedCourses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblEligibleCountValue;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBlockedCountValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvEligibleCourses;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvBlockedCourses;
        private System.Windows.Forms.Label label1;
    }
}

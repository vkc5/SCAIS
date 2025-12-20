namespace SCAIS.Student
{
    partial class StudentMainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdviserFeedback = new System.Windows.Forms.Button();
            this.btnSubmitCoursePlan = new System.Windows.Forms.Button();
            this.btnAvailableCourses = new System.Windows.Forms.Button();
            this.btnAcademicRecord = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlSidebar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 855);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(43)))), ((int)(((byte)(57)))));
            this.pnlSidebar.Controls.Add(this.button6);
            this.pnlSidebar.Controls.Add(this.panel4);
            this.pnlSidebar.Controls.Add(this.tableLayoutPanel1);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(319, 855);
            this.pnlSidebar.TabIndex = 1;
            this.pnlSidebar.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(48)))));
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(15, 765);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(283, 63);
            this.button6.TabIndex = 2;
            this.button6.Text = "Exit";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(26)))), ((int)(((byte)(35)))));
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(0, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(319, 120);
            this.panel4.TabIndex = 1;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(15, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Student Panel";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "SCAIS";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdviserFeedback, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSubmitCoursePlan, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnAvailableCourses, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAcademicRecord, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDashboard, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 151);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(289, 348);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // btnAdviserFeedback
            // 
            this.btnAdviserFeedback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnAdviserFeedback.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdviserFeedback.FlatAppearance.BorderSize = 0;
            this.btnAdviserFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdviserFeedback.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdviserFeedback.ForeColor = System.Drawing.SystemColors.Window;
            this.btnAdviserFeedback.Location = new System.Drawing.Point(3, 279);
            this.btnAdviserFeedback.Name = "btnAdviserFeedback";
            this.btnAdviserFeedback.Size = new System.Drawing.Size(283, 63);
            this.btnAdviserFeedback.TabIndex = 4;
            this.btnAdviserFeedback.Text = "Adviser Feedback";
            this.btnAdviserFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdviserFeedback.UseVisualStyleBackColor = false;
            this.btnAdviserFeedback.Click += new System.EventHandler(this.btnAdviserFeedback_Click);
            // 
            // btnSubmitCoursePlan
            // 
            this.btnSubmitCoursePlan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnSubmitCoursePlan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmitCoursePlan.FlatAppearance.BorderSize = 0;
            this.btnSubmitCoursePlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitCoursePlan.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitCoursePlan.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSubmitCoursePlan.Location = new System.Drawing.Point(3, 210);
            this.btnSubmitCoursePlan.Name = "btnSubmitCoursePlan";
            this.btnSubmitCoursePlan.Size = new System.Drawing.Size(283, 63);
            this.btnSubmitCoursePlan.TabIndex = 3;
            this.btnSubmitCoursePlan.Text = "Submit Course Plan";
            this.btnSubmitCoursePlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmitCoursePlan.UseVisualStyleBackColor = false;
            this.btnSubmitCoursePlan.Click += new System.EventHandler(this.btnSubmitCoursePlan_Click);
            // 
            // btnAvailableCourses
            // 
            this.btnAvailableCourses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnAvailableCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAvailableCourses.FlatAppearance.BorderSize = 0;
            this.btnAvailableCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvailableCourses.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvailableCourses.ForeColor = System.Drawing.SystemColors.Window;
            this.btnAvailableCourses.Location = new System.Drawing.Point(3, 141);
            this.btnAvailableCourses.Name = "btnAvailableCourses";
            this.btnAvailableCourses.Size = new System.Drawing.Size(283, 63);
            this.btnAvailableCourses.TabIndex = 2;
            this.btnAvailableCourses.Text = "Eligible Courses";
            this.btnAvailableCourses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAvailableCourses.UseVisualStyleBackColor = false;
            this.btnAvailableCourses.Click += new System.EventHandler(this.btnAvailableCourses_Click);
            // 
            // btnAcademicRecord
            // 
            this.btnAcademicRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnAcademicRecord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAcademicRecord.FlatAppearance.BorderSize = 0;
            this.btnAcademicRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcademicRecord.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcademicRecord.ForeColor = System.Drawing.SystemColors.Window;
            this.btnAcademicRecord.Location = new System.Drawing.Point(3, 72);
            this.btnAcademicRecord.Name = "btnAcademicRecord";
            this.btnAcademicRecord.Size = new System.Drawing.Size(283, 63);
            this.btnAcademicRecord.TabIndex = 1;
            this.btnAcademicRecord.Text = "Academic Record";
            this.btnAcademicRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAcademicRecord.UseVisualStyleBackColor = false;
            this.btnAcademicRecord.Click += new System.EventHandler(this.btnAcademicRecord_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.SystemColors.Window;
            this.btnDashboard.Location = new System.Drawing.Point(3, 3);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(283, 63);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(319, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(932, 855);
            this.pnlContent.TabIndex = 1;
            this.pnlContent.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // StudentMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 855);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StudentMainForm";
            this.Text = "StudentMainForm";
            this.Load += new System.EventHandler(this.StudentMainForm_Load);
            this.panel1.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdviserFeedback;
        private System.Windows.Forms.Button btnSubmitCoursePlan;
        private System.Windows.Forms.Button btnAvailableCourses;
        private System.Windows.Forms.Button btnAcademicRecord;
        private System.Windows.Forms.Button button6;
    }
}
namespace SCAIS.Adviser.Pages
{
    partial class AdviserStudentProfilePage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnRecommendCourses = new System.Windows.Forms.Button();
            this.pnlHistory = new System.Windows.Forms.Panel();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.pnlStudentInfo = new System.Windows.Forms.Panel();
            this.lblStudentIdValue = new System.Windows.Forms.Label();
            this.lblNameValue = new System.Windows.Forms.Label();
            this.lblSpecValue = new System.Windows.Forms.Label();
            this.lblSemesterValue = new System.Windows.Forms.Label();
            this.lblCreditsValue = new System.Windows.Forms.Label();
            this.lblGpaValue = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnlHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.pnlStudentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGenerateReport);
            this.panel1.Controls.Add(this.btnRecommendCourses);
            this.panel1.Controls.Add(this.pnlHistory);
            this.panel1.Controls.Add(this.pnlStudentInfo);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblPageTitle);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(932, 855);
            this.panel1.TabIndex = 9;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.Location = new System.Drawing.Point(738, 797);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(154, 43);
            this.btnGenerateReport.TabIndex = 16;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnRecommendCourses
            // 
            this.btnRecommendCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecommendCourses.Location = new System.Drawing.Point(564, 797);
            this.btnRecommendCourses.Name = "btnRecommendCourses";
            this.btnRecommendCourses.Size = new System.Drawing.Size(154, 43);
            this.btnRecommendCourses.TabIndex = 15;
            this.btnRecommendCourses.Text = "Recommend Courses";
            this.btnRecommendCourses.UseVisualStyleBackColor = true;
            this.btnRecommendCourses.Click += new System.EventHandler(this.btnRecommendCourses_Click);
            // 
            // pnlHistory
            // 
            this.pnlHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHistory.Controls.Add(this.dgvHistory);
            this.pnlHistory.Controls.Add(this.label13);
            this.pnlHistory.Controls.Add(this.label14);
            this.pnlHistory.Location = new System.Drawing.Point(37, 349);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(855, 442);
            this.pnlHistory.TabIndex = 14;
            // 
            // dgvHistory
            // 
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(24, 65);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.Size = new System.Drawing.Size(807, 363);
            this.dgvHistory.TabIndex = 17;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label13.Location = new System.Drawing.Point(19, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(175, 25);
            this.label13.TabIndex = 15;
            this.label13.Text = "Student Information";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(815, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "────────────────────";
            // 
            // pnlStudentInfo
            // 
            this.pnlStudentInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStudentInfo.Controls.Add(this.lblStudentIdValue);
            this.pnlStudentInfo.Controls.Add(this.lblNameValue);
            this.pnlStudentInfo.Controls.Add(this.lblSpecValue);
            this.pnlStudentInfo.Controls.Add(this.lblSemesterValue);
            this.pnlStudentInfo.Controls.Add(this.lblCreditsValue);
            this.pnlStudentInfo.Controls.Add(this.lblGpaValue);
            this.pnlStudentInfo.Controls.Add(this.label12);
            this.pnlStudentInfo.Controls.Add(this.label11);
            this.pnlStudentInfo.Controls.Add(this.label10);
            this.pnlStudentInfo.Controls.Add(this.label9);
            this.pnlStudentInfo.Controls.Add(this.label8);
            this.pnlStudentInfo.Controls.Add(this.label7);
            this.pnlStudentInfo.Controls.Add(this.label6);
            this.pnlStudentInfo.Controls.Add(this.label5);
            this.pnlStudentInfo.Controls.Add(this.label2);
            this.pnlStudentInfo.Controls.Add(this.label1);
            this.pnlStudentInfo.Location = new System.Drawing.Point(37, 153);
            this.pnlStudentInfo.Name = "pnlStudentInfo";
            this.pnlStudentInfo.Size = new System.Drawing.Size(855, 190);
            this.pnlStudentInfo.TabIndex = 13;
            // 
            // lblStudentIdValue
            // 
            this.lblStudentIdValue.AutoSize = true;
            this.lblStudentIdValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentIdValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStudentIdValue.Location = new System.Drawing.Point(116, 64);
            this.lblStudentIdValue.Name = "lblStudentIdValue";
            this.lblStudentIdValue.Size = new System.Drawing.Size(115, 25);
            this.lblStudentIdValue.TabIndex = 27;
            this.lblStudentIdValue.Text = "Current GPA:";
            // 
            // lblNameValue
            // 
            this.lblNameValue.AutoSize = true;
            this.lblNameValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblNameValue.Location = new System.Drawing.Point(81, 99);
            this.lblNameValue.Name = "lblNameValue";
            this.lblNameValue.Size = new System.Drawing.Size(115, 25);
            this.lblNameValue.TabIndex = 27;
            this.lblNameValue.Text = "Current GPA:";
            // 
            // lblSpecValue
            // 
            this.lblSpecValue.AutoSize = true;
            this.lblSpecValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSpecValue.Location = new System.Drawing.Point(146, 137);
            this.lblSpecValue.Name = "lblSpecValue";
            this.lblSpecValue.Size = new System.Drawing.Size(115, 25);
            this.lblSpecValue.TabIndex = 26;
            this.lblSpecValue.Text = "Current GPA:";
            // 
            // lblSemesterValue
            // 
            this.lblSemesterValue.AutoSize = true;
            this.lblSemesterValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemesterValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSemesterValue.Location = new System.Drawing.Point(565, 137);
            this.lblSemesterValue.Name = "lblSemesterValue";
            this.lblSemesterValue.Size = new System.Drawing.Size(115, 25);
            this.lblSemesterValue.TabIndex = 25;
            this.lblSemesterValue.Text = "Current GPA:";
            // 
            // lblCreditsValue
            // 
            this.lblCreditsValue.AutoSize = true;
            this.lblCreditsValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditsValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCreditsValue.Location = new System.Drawing.Point(531, 102);
            this.lblCreditsValue.Name = "lblCreditsValue";
            this.lblCreditsValue.Size = new System.Drawing.Size(115, 25);
            this.lblCreditsValue.TabIndex = 24;
            this.lblCreditsValue.Text = "Current GPA:";
            // 
            // lblGpaValue
            // 
            this.lblGpaValue.AutoSize = true;
            this.lblGpaValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGpaValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblGpaValue.Location = new System.Drawing.Point(531, 64);
            this.lblGpaValue.Name = "lblGpaValue";
            this.lblGpaValue.Size = new System.Drawing.Size(115, 25);
            this.lblGpaValue.TabIndex = 23;
            this.lblGpaValue.Text = "Current GPA:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label12.Location = new System.Drawing.Point(410, 137);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(157, 25);
            this.label12.TabIndex = 22;
            this.label12.Text = "Current Semester:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(410, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 25);
            this.label11.TabIndex = 21;
            this.label11.Text = "Total Credits:";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label10.Location = new System.Drawing.Point(410, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 25);
            this.label10.TabIndex = 20;
            this.label10.Text = "Current GPA:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Silver;
            this.label9.Location = new System.Drawing.Point(20, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(775, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Silver;
            this.label8.Location = new System.Drawing.Point(20, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(775, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(18, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 25);
            this.label7.TabIndex = 17;
            this.label7.Text = "Specialization:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Location = new System.Drawing.Point(18, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 25);
            this.label6.TabIndex = 16;
            this.label6.Text = "Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label5.Location = new System.Drawing.Point(18, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Student ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(18, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "Student Information";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(775, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::SCAIS.Properties.Resources.back1;
            this.pictureBox1.Location = new System.Drawing.Point(37, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(839, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────────────";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPageTitle.Location = new System.Drawing.Point(67, 113);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(228, 25);
            this.lblPageTitle.TabIndex = 10;
            this.lblPageTitle.Text = "Student Academic Profile";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(644, 45);
            this.label3.TabIndex = 9;
            this.label3.Text = "Smart Course Advising Information System";
            // 
            // AdviserStudentProfilePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "AdviserStudentProfilePage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.AdviserStudentProfilePage_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlHistory.ResumeLayout(false);
            this.pnlHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.pnlStudentInfo.ResumeLayout(false);
            this.pnlStudentInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlStudentInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblStudentIdValue;
        private System.Windows.Forms.Label lblNameValue;
        private System.Windows.Forms.Label lblSpecValue;
        private System.Windows.Forms.Label lblSemesterValue;
        private System.Windows.Forms.Label lblCreditsValue;
        private System.Windows.Forms.Label lblGpaValue;
        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnRecommendCourses;
        private System.Windows.Forms.DataGridView dgvHistory;
    }
}

namespace SCAIS.Student.Pages
{
    partial class StudentAcademicRecordPage
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
            this.label4 = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.pnlHistory = new System.Windows.Forms.Panel();
            this.dgvAcademicHistory = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlStudentInfo = new System.Windows.Forms.Panel();
            this.lblStudentIdValue = new System.Windows.Forms.Label();
            this.lblStudentNameValue = new System.Windows.Forms.Label();
            this.lblSpecializationValue = new System.Windows.Forms.Label();
            this.lblCreditsRequiredValue = new System.Windows.Forms.Label();
            this.lblCreditsEarnedValue = new System.Windows.Forms.Label();
            this.lblCurrentGpaValue = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAcademicHistory)).BeginInit();
            this.pnlStudentInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(644, 45);
            this.label3.TabIndex = 2;
            this.label3.Text = "Smart Course Advising Information System";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(839, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────────────";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPageTitle.Location = new System.Drawing.Point(33, 106);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(194, 25);
            this.lblPageTitle.TabIndex = 20;
            this.lblPageTitle.Text = "My Academic Record";
            // 
            // pnlHistory
            // 
            this.pnlHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHistory.Controls.Add(this.btnExport);
            this.pnlHistory.Controls.Add(this.cmbStatus);
            this.pnlHistory.Controls.Add(this.btnFilter);
            this.pnlHistory.Controls.Add(this.txtSearch);
            this.pnlHistory.Controls.Add(this.dgvAcademicHistory);
            this.pnlHistory.Controls.Add(this.label13);
            this.pnlHistory.Controls.Add(this.label1);
            this.pnlHistory.Location = new System.Drawing.Point(31, 335);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(858, 505);
            this.pnlHistory.TabIndex = 24;
            // 
            // dgvAcademicHistory
            // 
            this.dgvAcademicHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAcademicHistory.Location = new System.Drawing.Point(24, 123);
            this.dgvAcademicHistory.Name = "dgvAcademicHistory";
            this.dgvAcademicHistory.Size = new System.Drawing.Size(812, 316);
            this.dgvAcademicHistory.TabIndex = 17;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label13.Location = new System.Drawing.Point(19, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(155, 25);
            this.label13.TabIndex = 15;
            this.label13.Text = "Academic History";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(815, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "────────────────────";
            // 
            // pnlStudentInfo
            // 
            this.pnlStudentInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStudentInfo.Controls.Add(this.lblStudentIdValue);
            this.pnlStudentInfo.Controls.Add(this.lblStudentNameValue);
            this.pnlStudentInfo.Controls.Add(this.lblSpecializationValue);
            this.pnlStudentInfo.Controls.Add(this.lblCreditsRequiredValue);
            this.pnlStudentInfo.Controls.Add(this.lblCreditsEarnedValue);
            this.pnlStudentInfo.Controls.Add(this.lblCurrentGpaValue);
            this.pnlStudentInfo.Controls.Add(this.label12);
            this.pnlStudentInfo.Controls.Add(this.label11);
            this.pnlStudentInfo.Controls.Add(this.label10);
            this.pnlStudentInfo.Controls.Add(this.label9);
            this.pnlStudentInfo.Controls.Add(this.label8);
            this.pnlStudentInfo.Controls.Add(this.label7);
            this.pnlStudentInfo.Controls.Add(this.label6);
            this.pnlStudentInfo.Controls.Add(this.label5);
            this.pnlStudentInfo.Controls.Add(this.label2);
            this.pnlStudentInfo.Controls.Add(this.label14);
            this.pnlStudentInfo.Location = new System.Drawing.Point(32, 139);
            this.pnlStudentInfo.Name = "pnlStudentInfo";
            this.pnlStudentInfo.Size = new System.Drawing.Size(857, 190);
            this.pnlStudentInfo.TabIndex = 25;
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
            // lblStudentNameValue
            // 
            this.lblStudentNameValue.AutoSize = true;
            this.lblStudentNameValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentNameValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStudentNameValue.Location = new System.Drawing.Point(81, 99);
            this.lblStudentNameValue.Name = "lblStudentNameValue";
            this.lblStudentNameValue.Size = new System.Drawing.Size(115, 25);
            this.lblStudentNameValue.TabIndex = 27;
            this.lblStudentNameValue.Text = "Current GPA:";
            // 
            // lblSpecializationValue
            // 
            this.lblSpecializationValue.AutoSize = true;
            this.lblSpecializationValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecializationValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSpecializationValue.Location = new System.Drawing.Point(146, 137);
            this.lblSpecializationValue.Name = "lblSpecializationValue";
            this.lblSpecializationValue.Size = new System.Drawing.Size(115, 25);
            this.lblSpecializationValue.TabIndex = 26;
            this.lblSpecializationValue.Text = "Current GPA:";
            // 
            // lblCreditsRequiredValue
            // 
            this.lblCreditsRequiredValue.AutoSize = true;
            this.lblCreditsRequiredValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditsRequiredValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCreditsRequiredValue.Location = new System.Drawing.Point(561, 137);
            this.lblCreditsRequiredValue.Name = "lblCreditsRequiredValue";
            this.lblCreditsRequiredValue.Size = new System.Drawing.Size(115, 25);
            this.lblCreditsRequiredValue.TabIndex = 25;
            this.lblCreditsRequiredValue.Text = "Current GPA:";
            // 
            // lblCreditsEarnedValue
            // 
            this.lblCreditsEarnedValue.AutoSize = true;
            this.lblCreditsEarnedValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditsEarnedValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCreditsEarnedValue.Location = new System.Drawing.Point(545, 99);
            this.lblCreditsEarnedValue.Name = "lblCreditsEarnedValue";
            this.lblCreditsEarnedValue.Size = new System.Drawing.Size(115, 25);
            this.lblCreditsEarnedValue.TabIndex = 24;
            this.lblCreditsEarnedValue.Text = "Current GPA:";
            // 
            // lblCurrentGpaValue
            // 
            this.lblCurrentGpaValue.AutoSize = true;
            this.lblCurrentGpaValue.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentGpaValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCurrentGpaValue.Location = new System.Drawing.Point(526, 64);
            this.lblCurrentGpaValue.Name = "lblCurrentGpaValue";
            this.lblCurrentGpaValue.Size = new System.Drawing.Size(115, 25);
            this.lblCurrentGpaValue.TabIndex = 23;
            this.lblCurrentGpaValue.Text = "Current GPA:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label12.Location = new System.Drawing.Point(410, 137);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(151, 25);
            this.label12.TabIndex = 22;
            this.label12.Text = "Credits Required:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label11.Location = new System.Drawing.Point(410, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 25);
            this.label11.TabIndex = 21;
            this.label11.Text = "Credits Earned:";
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
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(775, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "─────────────────────────────────────────────────────────────────────────────────" +
    "───────────────";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbStatus.Font = new System.Drawing.Font("Yu Gothic UI", 18.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(477, 62);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(175, 43);
            this.cmbStatus.TabIndex = 20;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Location = new System.Drawing.Point(676, 65);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(160, 38);
            this.btnFilter.TabIndex = 19;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Yu Gothic UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txtSearch.Location = new System.Drawing.Point(24, 63);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(431, 43);
            this.txtSearch.TabIndex = 18;
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(650, 450);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(186, 43);
            this.btnExport.TabIndex = 21;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // StudentAcademicRecordPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStudentInfo);
            this.Controls.Add(this.pnlHistory);
            this.Controls.Add(this.lblPageTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "StudentAcademicRecordPage";
            this.Size = new System.Drawing.Size(932, 855);
            this.Load += new System.EventHandler(this.StudentAcademicRecordPage_Load);
            this.pnlHistory.ResumeLayout(false);
            this.pnlHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAcademicHistory)).EndInit();
            this.pnlStudentInfo.ResumeLayout(false);
            this.pnlStudentInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.DataGridView dgvAcademicHistory;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlStudentInfo;
        private System.Windows.Forms.Label lblStudentIdValue;
        private System.Windows.Forms.Label lblStudentNameValue;
        private System.Windows.Forms.Label lblSpecializationValue;
        private System.Windows.Forms.Label lblCreditsRequiredValue;
        private System.Windows.Forms.Label lblCreditsEarnedValue;
        private System.Windows.Forms.Label lblCurrentGpaValue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnExport;
    }
}

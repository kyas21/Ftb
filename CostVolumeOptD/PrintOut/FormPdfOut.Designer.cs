namespace PrintOut
{
    partial class FormPdfOut
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbStart = new System.Windows.Forms.ComboBox();
            this.cmbEnd = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optRange = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.optSelect = new System.Windows.Forms.RadioButton();
            this.dgvOutPut = new System.Windows.Forms.DataGridView();
            this.LY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutPut)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbStart
            // 
            this.cmbStart.FormattingEnabled = true;
            this.cmbStart.Location = new System.Drawing.Point(33, 98);
            this.cmbStart.Name = "cmbStart";
            this.cmbStart.Size = new System.Drawing.Size(150, 20);
            this.cmbStart.TabIndex = 3;
            // 
            // cmbEnd
            // 
            this.cmbEnd.FormattingEnabled = true;
            this.cmbEnd.Location = new System.Drawing.Point(218, 98);
            this.cmbEnd.Name = "cmbEnd";
            this.cmbEnd.Size = new System.Drawing.Size(150, 20);
            this.cmbEnd.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "から";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "業務番号";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(16, 205);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 40);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "印刷";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(289, 205);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(100, 40);
            this.btnEnd.TabIndex = 6;
            this.btnEnd.Text = "終了";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(89, 164);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(234, 27);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "しばらくお待ちください";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optRange);
            this.groupBox1.Controls.Add(this.optAll);
            this.groupBox1.Controls.Add(this.optSelect);
            this.groupBox1.Controls.Add(this.cmbStart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbEnd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 129);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "印刷設定";
            // 
            // optRange
            // 
            this.optRange.AutoSize = true;
            this.optRange.Location = new System.Drawing.Point(14, 62);
            this.optRange.Name = "optRange";
            this.optRange.Size = new System.Drawing.Size(119, 16);
            this.optRange.TabIndex = 2;
            this.optRange.TabStop = true;
            this.optRange.Text = "業務番号範囲指定";
            this.optRange.UseVisualStyleBackColor = true;
            this.optRange.CheckedChanged += new System.EventHandler(this.optRange_CheckedChanged);
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(14, 40);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(52, 16);
            this.optAll.TabIndex = 1;
            this.optAll.TabStop = true;
            this.optAll.Text = "すべて";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.CheckedChanged += new System.EventHandler(this.optAll_CheckedChanged);
            // 
            // optSelect
            // 
            this.optSelect.AutoSize = true;
            this.optSelect.Location = new System.Drawing.Point(14, 18);
            this.optSelect.Name = "optSelect";
            this.optSelect.Size = new System.Drawing.Size(177, 16);
            this.optSelect.TabIndex = 0;
            this.optSelect.TabStop = true;
            this.optSelect.Text = "業務番号(前画面で指定された)";
            this.optSelect.UseVisualStyleBackColor = true;
            this.optSelect.CheckedChanged += new System.EventHandler(this.optSelect_CheckedChanged);
            // 
            // dgvOutPut
            // 
            this.dgvOutPut.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOutPut.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOutPut.ColumnHeadersHeight = 21;
            this.dgvOutPut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOutPut.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LY,
            this.M7,
            this.M8,
            this.M9,
            this.M10,
            this.M11,
            this.M12,
            this.M1,
            this.M2,
            this.M3,
            this.M4,
            this.M5,
            this.M6});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOutPut.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOutPut.Location = new System.Drawing.Point(4, 3);
            this.dgvOutPut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvOutPut.Name = "dgvOutPut";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOutPut.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOutPut.RowHeadersVisible = false;
            this.dgvOutPut.RowTemplate.Height = 21;
            this.dgvOutPut.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOutPut.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvOutPut.Size = new System.Drawing.Size(407, 15);
            this.dgvOutPut.TabIndex = 51;
            this.dgvOutPut.TabStop = false;
            this.dgvOutPut.Visible = false;
            // 
            // LY
            // 
            this.LY.HeaderText = "前年度";
            this.LY.MaxInputLength = 80;
            this.LY.Name = "LY";
            this.LY.Width = 90;
            // 
            // M7
            // 
            this.M7.HeaderText = "7月";
            this.M7.MaxInputLength = 80;
            this.M7.Name = "M7";
            this.M7.Width = 80;
            // 
            // M8
            // 
            this.M8.HeaderText = "8月";
            this.M8.MaxInputLength = 80;
            this.M8.Name = "M8";
            this.M8.Width = 80;
            // 
            // M9
            // 
            this.M9.HeaderText = "9月";
            this.M9.MaxInputLength = 80;
            this.M9.Name = "M9";
            this.M9.Width = 80;
            // 
            // M10
            // 
            this.M10.HeaderText = "10月";
            this.M10.MaxInputLength = 80;
            this.M10.Name = "M10";
            this.M10.Width = 80;
            // 
            // M11
            // 
            this.M11.HeaderText = "11月";
            this.M11.MaxInputLength = 80;
            this.M11.Name = "M11";
            this.M11.Width = 80;
            // 
            // M12
            // 
            this.M12.HeaderText = "12月";
            this.M12.MaxInputLength = 80;
            this.M12.Name = "M12";
            this.M12.Width = 80;
            // 
            // M1
            // 
            this.M1.HeaderText = "1月";
            this.M1.MaxInputLength = 80;
            this.M1.Name = "M1";
            this.M1.Width = 80;
            // 
            // M2
            // 
            this.M2.HeaderText = "2月";
            this.M2.MaxInputLength = 80;
            this.M2.Name = "M2";
            this.M2.Width = 80;
            // 
            // M3
            // 
            this.M3.HeaderText = "3月";
            this.M3.MaxInputLength = 80;
            this.M3.Name = "M3";
            this.M3.Width = 80;
            // 
            // M4
            // 
            this.M4.HeaderText = "4月";
            this.M4.MaxInputLength = 80;
            this.M4.Name = "M4";
            this.M4.Width = 80;
            // 
            // M5
            // 
            this.M5.HeaderText = "5月";
            this.M5.Name = "M5";
            this.M5.Width = 80;
            // 
            // M6
            // 
            this.M6.HeaderText = "6月";
            this.M6.MaxInputLength = 80;
            this.M6.Name = "M6";
            this.M6.Width = 80;
            // 
            // FormPdfOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 264);
            this.Controls.Add(this.dgvOutPut);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnPrint);
            this.Name = "FormPdfOut";
            this.Text = "PDF印刷";
            this.Load += new System.EventHandler(this.FormPdfOut_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutPut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbStart;
        private System.Windows.Forms.ComboBox cmbEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optRange;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.RadioButton optSelect;
        private System.Windows.Forms.DataGridView dgvOutPut;
        private System.Windows.Forms.DataGridViewTextBoxColumn LY;
        private System.Windows.Forms.DataGridViewTextBoxColumn M7;
        private System.Windows.Forms.DataGridViewTextBoxColumn M8;
        private System.Windows.Forms.DataGridViewTextBoxColumn M9;
        private System.Windows.Forms.DataGridViewTextBoxColumn M10;
        private System.Windows.Forms.DataGridViewTextBoxColumn M11;
        private System.Windows.Forms.DataGridViewTextBoxColumn M12;
        private System.Windows.Forms.DataGridViewTextBoxColumn M1;
        private System.Windows.Forms.DataGridViewTextBoxColumn M2;
        private System.Windows.Forms.DataGridViewTextBoxColumn M3;
        private System.Windows.Forms.DataGridViewTextBoxColumn M4;
        private System.Windows.Forms.DataGridViewTextBoxColumn M5;
        private System.Windows.Forms.DataGridViewTextBoxColumn M6;
    }
}
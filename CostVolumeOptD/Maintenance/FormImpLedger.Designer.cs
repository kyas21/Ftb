namespace Maintenance
{
    partial class FormImpLedger
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
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.comboBoxOfficeCode = new System.Windows.Forms.ComboBox();
            this.comboBoxDepartment = new System.Windows.Forms.ComboBox();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.HideSelection = false;
            this.textBoxMsg.Location = new System.Drawing.Point(12, 103);
            this.textBoxMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(680, 265);
            this.textBoxMsg.TabIndex = 8;
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(592, 54);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 40);
            this.buttonEnd.TabIndex = 7;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(431, 54);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(307, 54);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 40);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "取込開始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(201, 54);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(100, 40);
            this.buttonOpen.TabIndex = 6;
            this.buttonOpen.Text = "ファイル指定";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.button_Click);
            // 
            // comboBoxOfficeCode
            // 
            this.comboBoxOfficeCode.FormattingEnabled = true;
            this.comboBoxOfficeCode.Location = new System.Drawing.Point(203, 16);
            this.comboBoxOfficeCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxOfficeCode.Name = "comboBoxOfficeCode";
            this.comboBoxOfficeCode.Size = new System.Drawing.Size(76, 23);
            this.comboBoxOfficeCode.TabIndex = 2;
            this.comboBoxOfficeCode.SelectedIndexChanged += new System.EventHandler(this.comboBoxOfficeCode_SelectedIndexChanged);
            // 
            // comboBoxDepartment
            // 
            this.comboBoxDepartment.FormattingEnabled = true;
            this.comboBoxDepartment.Location = new System.Drawing.Point(285, 16);
            this.comboBoxDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxDepartment.Name = "comboBoxDepartment";
            this.comboBoxDepartment.Size = new System.Drawing.Size(76, 23);
            this.comboBoxDepartment.TabIndex = 3;
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(63, 16);
            this.textBoxYear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(67, 23);
            this.textBoxYear.TabIndex = 1;
            this.textBoxYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 62;
            this.label1.Text = "年度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 63;
            this.label2.Text = "部署：";
            // 
            // FormImpLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 381);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.comboBoxDepartment);
            this.Controls.Add(this.comboBoxOfficeCode);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonOpen);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormImpLedger";
            this.Text = "出来高取込";
            this.Load += new System.EventHandler(this.FormImpSurvey_Load);
            this.Shown += new System.EventHandler(this.FormImpSurvey_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.ComboBox comboBoxOfficeCode;
        private System.Windows.Forms.ComboBox comboBoxDepartment;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
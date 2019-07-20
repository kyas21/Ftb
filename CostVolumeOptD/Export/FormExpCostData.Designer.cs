namespace Maintenance
{
    partial class FormExpCostData
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
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerDateTO = new System.Windows.Forms.DateTimePicker();
            this.labelDateTL = new System.Windows.Forms.Label();
            this.dateTimePickerDateFR = new System.Windows.Forms.DateTimePicker();
            this.labelDate = new System.Windows.Forms.Label();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 45;
            this.label3.Text = "事業所：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(82, 18);
            this.comboBoxOffice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(125, 23);
            this.comboBoxOffice.TabIndex = 44;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(24, 388);
            this.buttonCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(100, 40);
            this.buttonCheck.TabIndex = 43;
            this.buttonCheck.Text = "確認";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(355, 388);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 40;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(170, 388);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 40);
            this.buttonOK.TabIndex = 39;
            this.buttonOK.Text = "開始";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 38;
            this.label2.Text = "状態：";
            // 
            // dateTimePickerDateTO
            // 
            this.dateTimePickerDateTO.Location = new System.Drawing.Point(315, 59);
            this.dateTimePickerDateTO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerDateTO.Name = "dateTimePickerDateTO";
            this.dateTimePickerDateTO.Size = new System.Drawing.Size(140, 23);
            this.dateTimePickerDateTO.TabIndex = 37;
            this.dateTimePickerDateTO.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // labelDateTL
            // 
            this.labelDateTL.AutoSize = true;
            this.labelDateTL.Location = new System.Drawing.Point(290, 65);
            this.labelDateTL.Name = "labelDateTL";
            this.labelDateTL.Size = new System.Drawing.Size(19, 15);
            this.labelDateTL.TabIndex = 36;
            this.labelDateTL.Text = "～";
            // 
            // dateTimePickerDateFR
            // 
            this.dateTimePickerDateFR.Location = new System.Drawing.Point(144, 59);
            this.dateTimePickerDateFR.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerDateFR.Name = "dateTimePickerDateFR";
            this.dateTimePickerDateFR.Size = new System.Drawing.Size(140, 23);
            this.dateTimePickerDateFR.TabIndex = 35;
            this.dateTimePickerDateFR.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(71, 61);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(67, 15);
            this.labelDate.TabIndex = 27;
            this.labelDate.Text = "期間指定：";
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(602, 3);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 46;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.HideSelection = false;
            this.textBoxMsg.Location = new System.Drawing.Point(24, 109);
            this.textBoxMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(668, 271);
            this.textBoxMsg.TabIndex = 47;
            this.textBoxMsg.Click += new System.EventHandler(this.textBox_Click);
            // 
            // FormExpCostData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerDateTO);
            this.Controls.Add(this.labelDateTL);
            this.Controls.Add(this.dateTimePickerDateFR);
            this.Controls.Add(this.labelDate);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormExpCostData";
            this.Text = "原価データ→（売上明細）→商魂 移行データ作成";
            this.Load += new System.EventHandler(this.FormExpCostData_Load);
            this.Shown += new System.EventHandler(this.FormExpCostData_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateTO;
        private System.Windows.Forms.Label labelDateTL;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateFR;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.TextBox textBoxMsg;
    }
}
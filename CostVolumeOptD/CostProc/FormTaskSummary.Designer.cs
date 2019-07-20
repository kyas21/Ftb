namespace CostProc
{
    partial class FormTaskSummary
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBoxTaskCode = new System.Windows.Forms.TextBox();
            this.labelTaskName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPartnerName = new System.Windows.Forms.Label();
            this.labelTerm = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerFR = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTO = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.labelOffice = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ReportDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.comboBoxDepartment = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxTaskCode
            // 
            this.textBoxTaskCode.Location = new System.Drawing.Point(84, 38);
            this.textBoxTaskCode.Name = "textBoxTaskCode";
            this.textBoxTaskCode.Size = new System.Drawing.Size(80, 23);
            this.textBoxTaskCode.TabIndex = 0;
            this.textBoxTaskCode.TextChanged += new System.EventHandler(this.textBoxTaskCode_TextChanged);
            this.textBoxTaskCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // labelTaskName
            // 
            this.labelTaskName.AutoSize = true;
            this.labelTaskName.Location = new System.Drawing.Point(201, 41);
            this.labelTaskName.Name = "labelTaskName";
            this.labelTaskName.Size = new System.Drawing.Size(67, 15);
            this.labelTaskName.TabIndex = 1;
            this.labelTaskName.Text = "業務名表示";
            this.labelTaskName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "業務番号：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPartnerName
            // 
            this.labelPartnerName.AutoSize = true;
            this.labelPartnerName.Location = new System.Drawing.Point(201, 61);
            this.labelPartnerName.Name = "labelPartnerName";
            this.labelPartnerName.Size = new System.Drawing.Size(79, 15);
            this.labelPartnerName.TabIndex = 3;
            this.labelPartnerName.Text = "取引先会社名";
            this.labelPartnerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTerm
            // 
            this.labelTerm.AutoSize = true;
            this.labelTerm.Location = new System.Drawing.Point(551, 61);
            this.labelTerm.Name = "labelTerm";
            this.labelTerm.Size = new System.Drawing.Size(195, 15);
            this.labelTerm.TabIndex = 4;
            this.labelTerm.Text = "工期：YYYYMMDD～YYYYMMDD";
            this.labelTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "表示期間：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerFR
            // 
            this.dateTimePickerFR.Location = new System.Drawing.Point(324, 8);
            this.dateTimePickerFR.Name = "dateTimePickerFR";
            this.dateTimePickerFR.Size = new System.Drawing.Size(132, 23);
            this.dateTimePickerFR.TabIndex = 6;
            this.dateTimePickerFR.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dateTimePickerTO
            // 
            this.dateTimePickerTO.Location = new System.Drawing.Point(491, 8);
            this.dateTimePickerTO.Name = "dateTimePickerTO";
            this.dateTimePickerTO.Size = new System.Drawing.Size(132, 23);
            this.dateTimePickerTO.TabIndex = 7;
            this.dateTimePickerTO.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(462, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "～";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(84, 8);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(50, 23);
            this.comboBoxOffice.TabIndex = 9;
            this.comboBoxOffice.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // labelOffice
            // 
            this.labelOffice.AutoSize = true;
            this.labelOffice.Location = new System.Drawing.Point(11, 11);
            this.labelOffice.Name = "labelOffice";
            this.labelOffice.Size = new System.Drawing.Size(43, 15);
            this.labelOffice.TabIndex = 10;
            this.labelOffice.Text = "部署：";
            this.labelOffice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReportDate,
            this.SlipNo,
            this.ItemCode,
            this.Item,
            this.Quantity,
            this.UnitPrice,
            this.Cost,
            this.Balance});
            this.dataGridView1.Location = new System.Drawing.Point(11, 116);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1058, 503);
            this.dataGridView1.TabIndex = 11;
            // 
            // ReportDate
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ReportDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.ReportDate.HeaderText = "月日";
            this.ReportDate.Name = "ReportDate";
            this.ReportDate.ReadOnly = true;
            this.ReportDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReportDate.Width = 80;
            // 
            // SlipNo
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SlipNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.SlipNo.HeaderText = "伝票No.";
            this.SlipNo.Name = "SlipNo";
            this.SlipNo.ReadOnly = true;
            this.SlipNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SlipNo.Width = 80;
            // 
            // ItemCode
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ItemCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.ItemCode.HeaderText = "コード";
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.ReadOnly = true;
            this.ItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemCode.Width = 80;
            // 
            // Item
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Item.DefaultCellStyle = dataGridViewCellStyle4;
            this.Item.HeaderText = "名称";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Item.Width = 400;
            // 
            // Quantity
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle5;
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UnitPrice
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.UnitPrice.DefaultCellStyle = dataGridViewCellStyle6;
            this.UnitPrice.HeaderText = "単価";
            this.UnitPrice.Name = "UnitPrice";
            this.UnitPrice.ReadOnly = true;
            this.UnitPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Cost
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Cost.DefaultCellStyle = dataGridViewCellStyle7;
            this.Cost.HeaderText = "金額";
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            this.Cost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Balance
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Balance.DefaultCellStyle = dataGridViewCellStyle8;
            this.Balance.HeaderText = "累計";
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            this.Balance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(982, 6);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 25;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(11, 628);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 26;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // comboBoxDepartment
            // 
            this.comboBoxDepartment.FormattingEnabled = true;
            this.comboBoxDepartment.Location = new System.Drawing.Point(140, 8);
            this.comboBoxDepartment.Name = "comboBoxDepartment";
            this.comboBoxDepartment.Size = new System.Drawing.Size(80, 23);
            this.comboBoxDepartment.TabIndex = 27;
            this.comboBoxDepartment.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonOK.Location = new System.Drawing.Point(11, 76);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(140, 30);
            this.buttonOK.TabIndex = 28;
            this.buttonOK.Text = "表示";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.button_Click);
            // 
            // FormTaskSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 681);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxDepartment);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelOffice);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerTO);
            this.Controls.Add(this.dateTimePickerFR);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTerm);
            this.Controls.Add(this.labelPartnerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTaskName);
            this.Controls.Add(this.textBoxTaskCode);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTaskSummary";
            this.Text = "業務元帳（得意先元帳）";
            this.Load += new System.EventHandler(this.FormTaskSummary_Load);
            this.Shown += new System.EventHandler(this.FormTaskSummary_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxTaskCode;
        private System.Windows.Forms.Label labelTaskName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPartnerName;
        private System.Windows.Forms.Label labelTerm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerFR;
        private System.Windows.Forms.DateTimePicker dateTimePickerTO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label labelOffice;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.ComboBox comboBoxDepartment;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
    }
}
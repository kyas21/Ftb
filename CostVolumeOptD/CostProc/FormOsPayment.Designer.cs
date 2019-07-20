namespace CostProc
{
    partial class FormOsPayment
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
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelMsg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPCheckDate = new System.Windows.Forms.Label();
            this.labelACheckDate = new System.Windows.Forms.Label();
            this.labelDCheckDate = new System.Windows.Forms.Label();
            this.checkBoxAdmin = new System.Windows.Forms.CheckBox();
            this.checkBoxDirector = new System.Windows.Forms.CheckBox();
            this.checkBoxPresident = new System.Windows.Forms.CheckBox();
            this.buttonCost = new System.Windows.Forms.Button();
            this.dateTimePickerEx1 = new DateTimePickerEx.DateTimePickerEx();
            this.dataGridView1 = new DataGridViewPlus.DataGridViewPlus();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaderMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostReportID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPrint = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(146, 48);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(87, 23);
            this.comboBoxDepart.TabIndex = 3;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 47;
            this.label3.Text = "部門：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(82, 48);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(58, 23);
            this.comboBoxOffice.TabIndex = 2;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(1239, 6);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 25;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(146, 611);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 21;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(21, 611);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 40);
            this.buttonSave.TabIndex = 20;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(396, 611);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 40);
            this.buttonDelete.TabIndex = 22;
            this.buttonDelete.Text = "削除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Location = new System.Drawing.Point(284, 21);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(30, 15);
            this.labelMsg.TabIndex = 85;
            this.labelMsg.Text = "Msg";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 84;
            this.label2.Text = "対象月：";
            // 
            // labelPCheckDate
            // 
            this.labelPCheckDate.AutoSize = true;
            this.labelPCheckDate.Location = new System.Drawing.Point(831, 72);
            this.labelPCheckDate.Name = "labelPCheckDate";
            this.labelPCheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelPCheckDate.TabIndex = 106;
            this.labelPCheckDate.Text = "2016年12月31日";
            // 
            // labelACheckDate
            // 
            this.labelACheckDate.AutoSize = true;
            this.labelACheckDate.Location = new System.Drawing.Point(1041, 72);
            this.labelACheckDate.Name = "labelACheckDate";
            this.labelACheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelACheckDate.TabIndex = 105;
            this.labelACheckDate.Text = "2016年12月31日";
            // 
            // labelDCheckDate
            // 
            this.labelDCheckDate.AutoSize = true;
            this.labelDCheckDate.Location = new System.Drawing.Point(936, 72);
            this.labelDCheckDate.Name = "labelDCheckDate";
            this.labelDCheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelDCheckDate.TabIndex = 104;
            this.labelDCheckDate.Text = "2016年12月31日";
            // 
            // checkBoxAdmin
            // 
            this.checkBoxAdmin.AutoSize = true;
            this.checkBoxAdmin.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxAdmin.Location = new System.Drawing.Point(1057, 12);
            this.checkBoxAdmin.Name = "checkBoxAdmin";
            this.checkBoxAdmin.Size = new System.Drawing.Size(71, 48);
            this.checkBoxAdmin.TabIndex = 10;
            this.checkBoxAdmin.Text = "確認\r\n業務責任者";
            this.checkBoxAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxAdmin.UseVisualStyleBackColor = true;
            this.checkBoxAdmin.CheckedChanged += new System.EventHandler(this.checkBoxAdmin_CheckedChanged);
            // 
            // checkBoxDirector
            // 
            this.checkBoxDirector.AutoSize = true;
            this.checkBoxDirector.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDirector.Location = new System.Drawing.Point(952, 12);
            this.checkBoxDirector.Name = "checkBoxDirector";
            this.checkBoxDirector.Size = new System.Drawing.Size(71, 48);
            this.checkBoxDirector.TabIndex = 11;
            this.checkBoxDirector.Text = "照査\r\n担当取締役";
            this.checkBoxDirector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDirector.UseVisualStyleBackColor = true;
            this.checkBoxDirector.CheckedChanged += new System.EventHandler(this.checkBoxDirector_CheckedChanged);
            // 
            // checkBoxPresident
            // 
            this.checkBoxPresident.AutoSize = true;
            this.checkBoxPresident.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxPresident.Location = new System.Drawing.Point(865, 12);
            this.checkBoxPresident.Name = "checkBoxPresident";
            this.checkBoxPresident.Size = new System.Drawing.Size(35, 48);
            this.checkBoxPresident.TabIndex = 12;
            this.checkBoxPresident.Text = "承認\r\n社長";
            this.checkBoxPresident.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPresident.UseVisualStyleBackColor = true;
            this.checkBoxPresident.CheckedChanged += new System.EventHandler(this.checkBoxPresident_CheckedChanged);
            // 
            // buttonCost
            // 
            this.buttonCost.Location = new System.Drawing.Point(526, 611);
            this.buttonCost.Name = "buttonCost";
            this.buttonCost.Size = new System.Drawing.Size(100, 40);
            this.buttonCost.TabIndex = 23;
            this.buttonCost.Text = "原価データ作成";
            this.buttonCost.UseVisualStyleBackColor = true;
            this.buttonCost.Click += new System.EventHandler(this.button_Click);
            // 
            // dateTimePickerEx1
            // 
            this.dateTimePickerEx1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateTimePickerEx1.CustomFormat = "yyyy年MM月";
            this.dateTimePickerEx1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEx1.Location = new System.Drawing.Point(82, 15);
            this.dateTimePickerEx1.Name = "dateTimePickerEx1";
            this.dateTimePickerEx1.Size = new System.Drawing.Size(119, 23);
            this.dateTimePickerEx1.TabIndex = 1;
            this.dateTimePickerEx1.Value = new System.DateTime(2016, 10, 1, 10, 34, 31, 984);
            this.dateTimePickerEx1.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.SeqNo,
            this.ItemCode,
            this.Item,
            this.OrderNo,
            this.TaskCode,
            this.TaskName,
            this.OAmount,
            this.SAmount,
            this.Amount,
            this.ROAmount,
            this.SlipNo,
            this.LeaderMCode,
            this.SalesMCode,
            this.CostReportID,
            this.PaymentID,
            this.PartnerCode});
            this.dataGridView1.Location = new System.Drawing.Point(21, 101);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1308, 503);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // Check
            // 
            this.Check.HeaderText = "削除";
            this.Check.Name = "Check";
            this.Check.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Check.Width = 40;
            // 
            // SeqNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SeqNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.SeqNo.HeaderText = "No";
            this.SeqNo.Name = "SeqNo";
            this.SeqNo.ReadOnly = true;
            this.SeqNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SeqNo.Width = 30;
            // 
            // ItemCode
            // 
            this.ItemCode.HeaderText = "原価コード";
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemCode.Width = 80;
            // 
            // Item
            // 
            this.Item.HeaderText = "原価内容";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Item.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Item.Width = 200;
            // 
            // OrderNo
            // 
            this.OrderNo.HeaderText = "注文番号";
            this.OrderNo.Name = "OrderNo";
            this.OrderNo.ReadOnly = true;
            this.OrderNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.OrderNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OrderNo.Width = 80;
            // 
            // TaskCode
            // 
            this.TaskCode.HeaderText = "業務番号";
            this.TaskCode.Name = "TaskCode";
            this.TaskCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TaskCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskCode.Width = 80;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "業務名";
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            this.TaskName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TaskName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskName.Width = 300;
            // 
            // OAmount
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.OAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.OAmount.HeaderText = "発注金額";
            this.OAmount.Name = "OAmount";
            this.OAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.OAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SAmount
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.SAmount.HeaderText = "前月累計出来高";
            this.SAmount.Name = "SAmount";
            this.SAmount.ReadOnly = true;
            this.SAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Amount
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle4;
            this.Amount.HeaderText = "今月出来高";
            this.Amount.Name = "Amount";
            this.Amount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ROAmount
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ROAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.ROAmount.HeaderText = "発注残額";
            this.ROAmount.Name = "ROAmount";
            this.ROAmount.ReadOnly = true;
            this.ROAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ROAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SlipNo
            // 
            this.SlipNo.HeaderText = "伝票番号";
            this.SlipNo.Name = "SlipNo";
            this.SlipNo.ReadOnly = true;
            this.SlipNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SlipNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SlipNo.Width = 80;
            // 
            // LeaderMCode
            // 
            this.LeaderMCode.HeaderText = "業務担当者";
            this.LeaderMCode.Name = "LeaderMCode";
            this.LeaderMCode.ReadOnly = true;
            this.LeaderMCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LeaderMCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LeaderMCode.Visible = false;
            // 
            // SalesMCode
            // 
            this.SalesMCode.HeaderText = "営業担当者";
            this.SalesMCode.Name = "SalesMCode";
            this.SalesMCode.ReadOnly = true;
            this.SalesMCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SalesMCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SalesMCode.Visible = false;
            // 
            // CostReportID
            // 
            this.CostReportID.HeaderText = "原価実績データID";
            this.CostReportID.Name = "CostReportID";
            this.CostReportID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CostReportID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CostReportID.Visible = false;
            // 
            // PaymentID
            // 
            this.PaymentID.HeaderText = "出来高データID";
            this.PaymentID.Name = "PaymentID";
            this.PaymentID.Visible = false;
            // 
            // PartnerCode
            // 
            this.PartnerCode.HeaderText = "顧客コード";
            this.PartnerCode.Name = "PartnerCode";
            this.PartnerCode.Visible = false;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(846, 611);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 24;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // FormOsPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1350, 681);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonCost);
            this.Controls.Add(this.labelPCheckDate);
            this.Controls.Add(this.labelACheckDate);
            this.Controls.Add(this.labelDCheckDate);
            this.Controls.Add(this.checkBoxAdmin);
            this.Controls.Add(this.checkBoxDirector);
            this.Controls.Add(this.checkBoxPresident);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.dateTimePickerEx1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "FormOsPayment";
            this.Text = "外注出来高調書一覧表";
            this.Load += new System.EventHandler(this.FormOsPayment_Load);
            this.Shown += new System.EventHandler(this.FormOsPayment_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelMsg;
        private DateTimePickerEx.DateTimePickerEx dateTimePickerEx1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPCheckDate;
        private System.Windows.Forms.Label labelACheckDate;
        private System.Windows.Forms.Label labelDCheckDate;
        private System.Windows.Forms.CheckBox checkBoxAdmin;
        private System.Windows.Forms.CheckBox checkBoxDirector;
        private System.Windows.Forms.CheckBox checkBoxPresident;
        private System.Windows.Forms.Button buttonCost;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostReportID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerCode;
    }
}
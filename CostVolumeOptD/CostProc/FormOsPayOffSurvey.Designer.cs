namespace CostProc
{
    partial class FormOsPayOffSurvey
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
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.labelPCheckDate = new System.Windows.Forms.Label();
            this.labelACheckDate = new System.Windows.Forms.Label();
            this.labelDCheckDate = new System.Windows.Forms.Label();
            this.checkBoxAdmin = new System.Windows.Forms.CheckBox();
            this.checkBoxDirector = new System.Windows.Forms.CheckBox();
            this.checkBoxPresident = new System.Windows.Forms.CheckBox();
            this.buttonCost = new System.Windows.Forms.Button();
            this.labelMsg = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.textBoxCostCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAccountCode = new System.Windows.Forms.TextBox();
            this.textBoxPartnerCode = new System.Windows.Forms.TextBox();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelItem = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.labelOsPayOffNoteID = new System.Windows.Forms.Label();
            this.dateTimePickerEx1 = new DateTimePickerEx.DateTimePickerEx();
            this.dataGridView1 = new DataGridViewPlus.DataGridViewPlus();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContractForm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportCheck = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaderMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CloseDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaderMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PayOffID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostReportID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.Location = new System.Drawing.Point(131, 112);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(100, 30);
            this.buttonNext.TabIndex = 118;
            this.buttonNext.Text = "次  >";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Enabled = false;
            this.buttonPrev.Location = new System.Drawing.Point(21, 112);
            this.buttonPrev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(100, 30);
            this.buttonPrev.TabIndex = 117;
            this.buttonPrev.Text = "<  前";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(627, 632);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 123;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // labelPCheckDate
            // 
            this.labelPCheckDate.AutoSize = true;
            this.labelPCheckDate.Location = new System.Drawing.Point(919, 126);
            this.labelPCheckDate.Name = "labelPCheckDate";
            this.labelPCheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelPCheckDate.TabIndex = 129;
            this.labelPCheckDate.Text = "2016年12月31日";
            // 
            // labelACheckDate
            // 
            this.labelACheckDate.AutoSize = true;
            this.labelACheckDate.Location = new System.Drawing.Point(1129, 126);
            this.labelACheckDate.Name = "labelACheckDate";
            this.labelACheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelACheckDate.TabIndex = 128;
            this.labelACheckDate.Text = "2016年12月31日";
            // 
            // labelDCheckDate
            // 
            this.labelDCheckDate.AutoSize = true;
            this.labelDCheckDate.Location = new System.Drawing.Point(1024, 126);
            this.labelDCheckDate.Name = "labelDCheckDate";
            this.labelDCheckDate.Size = new System.Drawing.Size(99, 15);
            this.labelDCheckDate.TabIndex = 127;
            this.labelDCheckDate.Text = "2016年12月31日";
            // 
            // checkBoxAdmin
            // 
            this.checkBoxAdmin.AutoSize = true;
            this.checkBoxAdmin.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxAdmin.Location = new System.Drawing.Point(1145, 66);
            this.checkBoxAdmin.Name = "checkBoxAdmin";
            this.checkBoxAdmin.Size = new System.Drawing.Size(71, 48);
            this.checkBoxAdmin.TabIndex = 114;
            this.checkBoxAdmin.Text = "確認\r\n業務責任者";
            this.checkBoxAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxAdmin.UseVisualStyleBackColor = true;
            this.checkBoxAdmin.CheckedChanged += new System.EventHandler(this.checkBoxAdmin_CheckedChanged);
            // 
            // checkBoxDirector
            // 
            this.checkBoxDirector.AutoSize = true;
            this.checkBoxDirector.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDirector.Location = new System.Drawing.Point(1040, 66);
            this.checkBoxDirector.Name = "checkBoxDirector";
            this.checkBoxDirector.Size = new System.Drawing.Size(71, 48);
            this.checkBoxDirector.TabIndex = 115;
            this.checkBoxDirector.Text = "照査\r\n担当取締役";
            this.checkBoxDirector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDirector.UseVisualStyleBackColor = true;
            this.checkBoxDirector.CheckedChanged += new System.EventHandler(this.checkBoxDirector_CheckedChanged);
            // 
            // checkBoxPresident
            // 
            this.checkBoxPresident.AutoSize = true;
            this.checkBoxPresident.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxPresident.Location = new System.Drawing.Point(953, 66);
            this.checkBoxPresident.Name = "checkBoxPresident";
            this.checkBoxPresident.Size = new System.Drawing.Size(35, 48);
            this.checkBoxPresident.TabIndex = 116;
            this.checkBoxPresident.Text = "承認\r\n社長";
            this.checkBoxPresident.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPresident.UseVisualStyleBackColor = true;
            this.checkBoxPresident.CheckedChanged += new System.EventHandler(this.checkBoxPresident_CheckedChanged);
            // 
            // buttonCost
            // 
            this.buttonCost.Location = new System.Drawing.Point(447, 632);
            this.buttonCost.Name = "buttonCost";
            this.buttonCost.Size = new System.Drawing.Size(100, 40);
            this.buttonCost.TabIndex = 122;
            this.buttonCost.Text = "原価データ作成";
            this.buttonCost.UseVisualStyleBackColor = true;
            this.buttonCost.Click += new System.EventHandler(this.button_Click);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMsg.ForeColor = System.Drawing.Color.Blue;
            this.labelMsg.Location = new System.Drawing.Point(317, 15);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(40, 20);
            this.labelMsg.TabIndex = 126;
            this.labelMsg.Text = "Msg";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(317, 632);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 40);
            this.buttonDelete.TabIndex = 121;
            this.buttonDelete.Text = "削除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(1138, 6);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 124;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // textBoxUnit
            // 
            this.textBoxUnit.Location = new System.Drawing.Point(1151, 649);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.Size = new System.Drawing.Size(71, 23);
            this.textBoxUnit.TabIndex = 132;
            this.textBoxUnit.TabStop = false;
            this.textBoxUnit.Visible = false;
            // 
            // textBoxCostCode
            // 
            this.textBoxCostCode.Location = new System.Drawing.Point(94, 79);
            this.textBoxCostCode.Name = "textBoxCostCode";
            this.textBoxCostCode.Size = new System.Drawing.Size(89, 23);
            this.textBoxCostCode.TabIndex = 109;
            this.textBoxCostCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 125;
            this.label5.Text = "原価コード：";
            // 
            // textBoxAccountCode
            // 
            this.textBoxAccountCode.Location = new System.Drawing.Point(1061, 649);
            this.textBoxAccountCode.Name = "textBoxAccountCode";
            this.textBoxAccountCode.Size = new System.Drawing.Size(78, 23);
            this.textBoxAccountCode.TabIndex = 131;
            this.textBoxAccountCode.TabStop = false;
            this.textBoxAccountCode.Visible = false;
            // 
            // textBoxPartnerCode
            // 
            this.textBoxPartnerCode.Location = new System.Drawing.Point(971, 649);
            this.textBoxPartnerCode.Name = "textBoxPartnerCode";
            this.textBoxPartnerCode.Size = new System.Drawing.Size(78, 23);
            this.textBoxPartnerCode.TabIndex = 130;
            this.textBoxPartnerCode.TabStop = false;
            this.textBoxPartnerCode.Visible = false;
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(147, 48);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(75, 23);
            this.comboBoxDepart.TabIndex = 108;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(147, 632);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 120;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(21, 632);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 40);
            this.buttonSave.TabIndex = 119;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button_Click);
            // 
            // labelTotal
            // 
            this.labelTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotal.Location = new System.Drawing.Point(971, 585);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(101, 40);
            this.labelTotal.TabIndex = 113;
            this.labelTotal.Text = "合計金額";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(871, 585);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 40);
            this.label4.TabIndex = 112;
            this.label4.Text = "合計";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 111;
            this.label3.Text = "起案部署：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(94, 48);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(50, 23);
            this.comboBoxOffice.TabIndex = 106;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 107;
            this.label2.Text = "対象月：";
            // 
            // labelItem
            // 
            this.labelItem.AutoSize = true;
            this.labelItem.Location = new System.Drawing.Point(208, 82);
            this.labelItem.Name = "labelItem";
            this.labelItem.Size = new System.Drawing.Size(81, 15);
            this.labelItem.TabIndex = 105;
            this.labelItem.Text = "原価コード名称";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(21, 585);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 40);
            this.label1.TabIndex = 133;
            this.label1.Text = "備考";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxNote
            // 
            this.textBoxNote.Location = new System.Drawing.Point(91, 585);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(781, 40);
            this.textBoxNote.TabIndex = 134;
            // 
            // labelOsPayOffNoteID
            // 
            this.labelOsPayOffNoteID.AutoSize = true;
            this.labelOsPayOffNoteID.Location = new System.Drawing.Point(1126, 598);
            this.labelOsPayOffNoteID.Name = "labelOsPayOffNoteID";
            this.labelOsPayOffNoteID.Size = new System.Drawing.Size(102, 15);
            this.labelOsPayOffNoteID.TabIndex = 135;
            this.labelOsPayOffNoteID.Text = "OsPayOffNoteID";
            this.labelOsPayOffNoteID.Visible = false;
            // 
            // dateTimePickerEx1
            // 
            this.dateTimePickerEx1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateTimePickerEx1.CustomFormat = "yyyy年MM月";
            this.dateTimePickerEx1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEx1.Location = new System.Drawing.Point(94, 15);
            this.dateTimePickerEx1.Name = "dateTimePickerEx1";
            this.dateTimePickerEx1.Size = new System.Drawing.Size(103, 23);
            this.dateTimePickerEx1.TabIndex = 104;
            this.dateTimePickerEx1.Value = new System.DateTime(2016, 10, 1, 10, 34, 31, 984);
            this.dateTimePickerEx1.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.SeqNo,
            this.TaskCode,
            this.TaskName,
            this.PartnerName,
            this.ContractForm,
            this.ReportCheck,
            this.LeaderMName,
            this.Amount,
            this.CloseDT,
            this.LeaderMCode,
            this.SlipNo,
            this.SalesMCode,
            this.PartnerCode,
            this.PayOffID,
            this.CostReportID});
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(21, 151);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.Size = new System.Drawing.Size(1208, 435);
            this.dataGridView1.TabIndex = 110;
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
            this.SeqNo.HeaderText = "No.";
            this.SeqNo.Name = "SeqNo";
            this.SeqNo.ReadOnly = true;
            this.SeqNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SeqNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SeqNo.Width = 30;
            // 
            // TaskCode
            // 
            this.TaskCode.HeaderText = "業務番号";
            this.TaskCode.Name = "TaskCode";
            this.TaskCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaskCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskCode.Width = 80;
            // 
            // TaskName
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TaskName.DefaultCellStyle = dataGridViewCellStyle2;
            this.TaskName.HeaderText = "業務名";
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            this.TaskName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskName.Width = 240;
            // 
            // PartnerName
            // 
            this.PartnerName.HeaderText = "発注会社名";
            this.PartnerName.Name = "PartnerName";
            this.PartnerName.ReadOnly = true;
            this.PartnerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PartnerName.Width = 160;
            // 
            // ContractForm
            // 
            this.ContractForm.HeaderText = "請負常傭";
            this.ContractForm.Name = "ContractForm";
            this.ContractForm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContractForm.Width = 40;
            // 
            // ReportCheck
            // 
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ReportCheck.DefaultCellStyle = dataGridViewCellStyle3;
            this.ReportCheck.HeaderText = "業務内訳書提出確認";
            this.ReportCheck.Name = "ReportCheck";
            this.ReportCheck.ReadOnly = true;
            this.ReportCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReportCheck.Width = 260;
            // 
            // LeaderMName
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.LeaderMName.DefaultCellStyle = dataGridViewCellStyle4;
            this.LeaderMName.HeaderText = "業務担当者";
            this.LeaderMName.Name = "LeaderMName";
            this.LeaderMName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LeaderMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Amount
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle5;
            this.Amount.HeaderText = "金額";
            this.Amount.Name = "Amount";
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CloseDT
            // 
            this.CloseDT.HeaderText = "締切日";
            this.CloseDT.Name = "CloseDT";
            this.CloseDT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CloseDT.Width = 60;
            // 
            // LeaderMCode
            // 
            this.LeaderMCode.HeaderText = "業務担当者コード";
            this.LeaderMCode.Name = "LeaderMCode";
            this.LeaderMCode.ReadOnly = true;
            this.LeaderMCode.Visible = false;
            // 
            // SlipNo
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SlipNo.DefaultCellStyle = dataGridViewCellStyle6;
            this.SlipNo.HeaderText = "伝票番号";
            this.SlipNo.Name = "SlipNo";
            this.SlipNo.ReadOnly = true;
            this.SlipNo.Width = 80;
            // 
            // SalesMCode
            // 
            this.SalesMCode.HeaderText = "営業担当者";
            this.SalesMCode.Name = "SalesMCode";
            this.SalesMCode.ReadOnly = true;
            this.SalesMCode.Visible = false;
            // 
            // PartnerCode
            // 
            this.PartnerCode.HeaderText = "発注社コード";
            this.PartnerCode.Name = "PartnerCode";
            this.PartnerCode.ReadOnly = true;
            this.PartnerCode.Visible = false;
            // 
            // PayOffID
            // 
            this.PayOffID.HeaderText = "外注精算データID";
            this.PayOffID.Name = "PayOffID";
            this.PayOffID.ReadOnly = true;
            this.PayOffID.Visible = false;
            // 
            // CostReportID
            // 
            this.CostReportID.HeaderText = "原価実績データID";
            this.CostReportID.Name = "CostReportID";
            this.CostReportID.ReadOnly = true;
            this.CostReportID.Visible = false;
            // 
            // FormOsPayOffSurvey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.labelOsPayOffNoteID);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.labelPCheckDate);
            this.Controls.Add(this.labelACheckDate);
            this.Controls.Add(this.labelDCheckDate);
            this.Controls.Add(this.checkBoxAdmin);
            this.Controls.Add(this.checkBoxDirector);
            this.Controls.Add(this.checkBoxPresident);
            this.Controls.Add(this.buttonCost);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.textBoxUnit);
            this.Controls.Add(this.textBoxCostCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxAccountCode);
            this.Controls.Add(this.textBoxPartnerCode);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.dateTimePickerEx1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelItem);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormOsPayOffSurvey";
            this.Text = "外注清算書（起案）測量";
            this.Load += new System.EventHandler(this.FormOsPayOffSurvey_Load);
            this.Shown += new System.EventHandler(this.FormOsPayOffSurvey_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Label labelPCheckDate;
        private System.Windows.Forms.Label labelACheckDate;
        private System.Windows.Forms.Label labelDCheckDate;
        private System.Windows.Forms.CheckBox checkBoxAdmin;
        private System.Windows.Forms.CheckBox checkBoxDirector;
        private System.Windows.Forms.CheckBox checkBoxPresident;
        private System.Windows.Forms.Button buttonCost;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.TextBox textBoxCostCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAccountCode;
        private System.Windows.Forms.TextBox textBoxPartnerCode;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private DateTimePickerEx.DateTimePickerEx dateTimePickerEx1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelItem;
        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Label labelOsPayOffNoteID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CloseDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PayOffID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostReportID;
    }
}
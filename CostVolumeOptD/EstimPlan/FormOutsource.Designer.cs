namespace EstimPlan
{
    partial class FormOutsource
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerOrderDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerOStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerOEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPartner = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelTask = new System.Windows.Forms.Label();
            this.labelTerm = new System.Windows.Forms.Label();
            this.textBoxDeliveryPoint = new System.Windows.Forms.TextBox();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelSum = new System.Windows.Forms.Label();
            this.comboBoxPayRoule = new System.Windows.Forms.ComboBox();
            this.labelTaskCode = new System.Windows.Forms.Label();
            this.buttonPrevData = new System.Windows.Forms.Button();
            this.buttonNextData = new System.Windows.Forms.Button();
            this.buttonFromPlan = new System.Windows.Forms.Button();
            this.labelPageNo = new System.Windows.Forms.Label();
            this.buttonCopyAndNext = new System.Windows.Forms.Button();
            this.buttonOverWrite = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonReCalc = new System.Windows.Forms.Button();
            this.buttonPrintOrder = new System.Windows.Forms.Button();
            this.buttonPrintContent = new System.Windows.Forms.Button();
            this.buttonPrintConfirm = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.labelOrderNo = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.dateTimePickerInspectDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerReceiptDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new DataGridViewPlus.DataGridViewPlus();
            this.Button = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxWork = new System.Windows.Forms.ComboBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxPublish = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(311, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "注文書・注文請書発行日付：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerOrderDate
            // 
            this.dateTimePickerOrderDate.Location = new System.Drawing.Point(474, 18);
            this.dateTimePickerOrderDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerOrderDate.Name = "dateTimePickerOrderDate";
            this.dateTimePickerOrderDate.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerOrderDate.TabIndex = 1;
            this.dateTimePickerOrderDate.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dateTimePickerOStart
            // 
            this.dateTimePickerOStart.Location = new System.Drawing.Point(81, 71);
            this.dateTimePickerOStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerOStart.Name = "dateTimePickerOStart";
            this.dateTimePickerOStart.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerOStart.TabIndex = 2;
            this.dateTimePickerOStart.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "注文工期：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerOEnd
            // 
            this.dateTimePickerOEnd.Location = new System.Drawing.Point(255, 71);
            this.dateTimePickerOEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerOEnd.Name = "dateTimePickerOEnd";
            this.dateTimePickerOEnd.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerOEnd.TabIndex = 4;
            this.dateTimePickerOEnd.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(217, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "～";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "業者名：";
            // 
            // comboBoxPartner
            // 
            this.comboBoxPartner.FormattingEnabled = true;
            this.comboBoxPartner.Location = new System.Drawing.Point(81, 101);
            this.comboBoxPartner.Name = "comboBoxPartner";
            this.comboBoxPartner.Size = new System.Drawing.Size(572, 23);
            this.comboBoxPartner.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "業務：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(794, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "工期：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "納入場所：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 12;
            this.label8.Text = "特記事項：";
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.Location = new System.Drawing.Point(167, 51);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(137, 15);
            this.labelTask.TabIndex = 13;
            this.labelTask.Text = "原価管理区分+業務名称";
            // 
            // labelTerm
            // 
            this.labelTerm.AutoSize = true;
            this.labelTerm.Location = new System.Drawing.Point(843, 51);
            this.labelTerm.Name = "labelTerm";
            this.labelTerm.Size = new System.Drawing.Size(91, 15);
            this.labelTerm.TabIndex = 14;
            this.labelTerm.Text = "開始日～終了日";
            // 
            // textBoxDeliveryPoint
            // 
            this.textBoxDeliveryPoint.Location = new System.Drawing.Point(81, 126);
            this.textBoxDeliveryPoint.Name = "textBoxDeliveryPoint";
            this.textBoxDeliveryPoint.Size = new System.Drawing.Size(572, 23);
            this.textBoxDeliveryPoint.TabIndex = 15;
            // 
            // textBoxNote
            // 
            this.textBoxNote.Location = new System.Drawing.Point(81, 151);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(572, 23);
            this.textBoxNote.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(356, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 15);
            this.label10.TabIndex = 18;
            this.label10.Text = "金額（税抜）：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(765, 129);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 19;
            this.label11.Text = "支払基準：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(753, 154);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 15);
            this.label12.TabIndex = 20;
            this.label12.Text = "注文書番号：";
            // 
            // labelSum
            // 
            this.labelSum.AutoSize = true;
            this.labelSum.Location = new System.Drawing.Point(453, 182);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(78, 15);
            this.labelSum.TabIndex = 21;
            this.labelSum.Text = "999,999,999";
            // 
            // comboBoxPayRoule
            // 
            this.comboBoxPayRoule.FormattingEnabled = true;
            this.comboBoxPayRoule.Location = new System.Drawing.Point(838, 126);
            this.comboBoxPayRoule.Name = "comboBoxPayRoule";
            this.comboBoxPayRoule.Size = new System.Drawing.Size(121, 23);
            this.comboBoxPayRoule.TabIndex = 22;
            // 
            // labelTaskCode
            // 
            this.labelTaskCode.AutoSize = true;
            this.labelTaskCode.Location = new System.Drawing.Point(82, 51);
            this.labelTaskCode.Name = "labelTaskCode";
            this.labelTaskCode.Size = new System.Drawing.Size(79, 15);
            this.labelTaskCode.TabIndex = 24;
            this.labelTaskCode.Text = "業務番号表示";
            // 
            // buttonPrevData
            // 
            this.buttonPrevData.Location = new System.Drawing.Point(1035, 100);
            this.buttonPrevData.Name = "buttonPrevData";
            this.buttonPrevData.Size = new System.Drawing.Size(75, 30);
            this.buttonPrevData.TabIndex = 25;
            this.buttonPrevData.Text = "前社";
            this.buttonPrevData.UseVisualStyleBackColor = true;
            this.buttonPrevData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonNextData
            // 
            this.buttonNextData.Location = new System.Drawing.Point(1116, 100);
            this.buttonNextData.Name = "buttonNextData";
            this.buttonNextData.Size = new System.Drawing.Size(75, 30);
            this.buttonNextData.TabIndex = 26;
            this.buttonNextData.Text = "次社";
            this.buttonNextData.UseVisualStyleBackColor = true;
            this.buttonNextData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonFromPlan
            // 
            this.buttonFromPlan.BackColor = System.Drawing.Color.OldLace;
            this.buttonFromPlan.Location = new System.Drawing.Point(826, 6);
            this.buttonFromPlan.Name = "buttonFromPlan";
            this.buttonFromPlan.Size = new System.Drawing.Size(156, 40);
            this.buttonFromPlan.TabIndex = 27;
            this.buttonFromPlan.Text = "最新予算データの取込";
            this.buttonFromPlan.UseVisualStyleBackColor = false;
            this.buttonFromPlan.Click += new System.EventHandler(this.button_Click);
            // 
            // labelPageNo
            // 
            this.labelPageNo.BackColor = System.Drawing.Color.Honeydew;
            this.labelPageNo.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPageNo.Location = new System.Drawing.Point(1075, 68);
            this.labelPageNo.Name = "labelPageNo";
            this.labelPageNo.Size = new System.Drawing.Size(75, 27);
            this.labelPageNo.TabIndex = 28;
            this.labelPageNo.Text = "0/0";
            this.labelPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCopyAndNext
            // 
            this.buttonCopyAndNext.Location = new System.Drawing.Point(1035, 131);
            this.buttonCopyAndNext.Name = "buttonCopyAndNext";
            this.buttonCopyAndNext.Size = new System.Drawing.Size(156, 30);
            this.buttonCopyAndNext.TabIndex = 30;
            this.buttonCopyAndNext.Text = "データを残して次社";
            this.buttonCopyAndNext.UseVisualStyleBackColor = true;
            this.buttonCopyAndNext.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOverWrite
            // 
            this.buttonOverWrite.Location = new System.Drawing.Point(11, 629);
            this.buttonOverWrite.Name = "buttonOverWrite";
            this.buttonOverWrite.Size = new System.Drawing.Size(100, 40);
            this.buttonOverWrite.TabIndex = 31;
            this.buttonOverWrite.Text = "上書保存";
            this.buttonOverWrite.UseVisualStyleBackColor = true;
            this.buttonOverWrite.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(421, 629);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 43;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(301, 629);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 40);
            this.buttonDelete.TabIndex = 42;
            this.buttonDelete.Text = "削除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonReCalc
            // 
            this.buttonReCalc.BackColor = System.Drawing.Color.AntiqueWhite;
            this.buttonReCalc.Location = new System.Drawing.Point(1035, 165);
            this.buttonReCalc.Name = "buttonReCalc";
            this.buttonReCalc.Size = new System.Drawing.Size(156, 30);
            this.buttonReCalc.TabIndex = 77;
            this.buttonReCalc.Text = "再計算";
            this.buttonReCalc.UseVisualStyleBackColor = false;
            this.buttonReCalc.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrintOrder
            // 
            this.buttonPrintOrder.Location = new System.Drawing.Point(761, 629);
            this.buttonPrintOrder.Name = "buttonPrintOrder";
            this.buttonPrintOrder.Size = new System.Drawing.Size(140, 40);
            this.buttonPrintOrder.TabIndex = 78;
            this.buttonPrintOrder.Text = "注文書印刷";
            this.buttonPrintOrder.UseVisualStyleBackColor = true;
            this.buttonPrintOrder.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrintContent
            // 
            this.buttonPrintContent.Location = new System.Drawing.Point(601, 629);
            this.buttonPrintContent.Name = "buttonPrintContent";
            this.buttonPrintContent.Size = new System.Drawing.Size(140, 40);
            this.buttonPrintContent.TabIndex = 79;
            this.buttonPrintContent.Text = "内訳書印刷";
            this.buttonPrintContent.UseVisualStyleBackColor = true;
            this.buttonPrintContent.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrintConfirm
            // 
            this.buttonPrintConfirm.Location = new System.Drawing.Point(921, 629);
            this.buttonPrintConfirm.Name = "buttonPrintConfirm";
            this.buttonPrintConfirm.Size = new System.Drawing.Size(140, 40);
            this.buttonPrintConfirm.TabIndex = 80;
            this.buttonPrintConfirm.Text = "注文請書印刷";
            this.buttonPrintConfirm.UseVisualStyleBackColor = true;
            this.buttonPrintConfirm.Click += new System.EventHandler(this.button_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 15);
            this.label15.TabIndex = 81;
            this.label15.Text = "発行部署：";
            // 
            // labelOrderNo
            // 
            this.labelOrderNo.AutoSize = true;
            this.labelOrderNo.Location = new System.Drawing.Point(843, 154);
            this.labelOrderNo.Name = "labelOrderNo";
            this.labelOrderNo.Size = new System.Drawing.Size(56, 15);
            this.labelOrderNo.TabIndex = 83;
            this.labelOrderNo.Text = "OrderNo";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(455, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 15);
            this.label13.TabIndex = 84;
            this.label13.Text = "検査完了年月日：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(758, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 15);
            this.label16.TabIndex = 85;
            this.label16.Text = "受領年月日：";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerInspectDate
            // 
            this.dateTimePickerInspectDate.Location = new System.Drawing.Point(564, 69);
            this.dateTimePickerInspectDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerInspectDate.Name = "dateTimePickerInspectDate";
            this.dateTimePickerInspectDate.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerInspectDate.TabIndex = 86;
            // 
            // dateTimePickerReceiptDate
            // 
            this.dateTimePickerReceiptDate.Location = new System.Drawing.Point(838, 71);
            this.dateTimePickerReceiptDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePickerReceiptDate.Name = "dateTimePickerReceiptDate";
            this.dateTimePickerReceiptDate.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerReceiptDate.TabIndex = 87;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Button,
            this.ItemCode,
            this.Item,
            this.ItemDetail,
            this.Quantity,
            this.Unit,
            this.Cost,
            this.Amount,
            this.Note});
            this.dataGridView1.Location = new System.Drawing.Point(11, 201);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1180, 420);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // Button
            // 
            this.Button.HeaderText = "No.";
            this.Button.Name = "Button";
            this.Button.Width = 40;
            // 
            // ItemCode
            // 
            this.ItemCode.HeaderText = "コード";
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.Visible = false;
            // 
            // Item
            // 
            this.Item.HeaderText = "名称";
            this.Item.Name = "Item";
            this.Item.Width = 300;
            // 
            // ItemDetail
            // 
            this.ItemDetail.HeaderText = "細別";
            this.ItemDetail.Name = "ItemDetail";
            this.ItemDetail.Width = 300;
            // 
            // Quantity
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            // 
            // Unit
            // 
            this.Unit.HeaderText = "単位";
            this.Unit.Name = "Unit";
            // 
            // Cost
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Cost.DefaultCellStyle = dataGridViewCellStyle2;
            this.Cost.HeaderText = "単価";
            this.Cost.Name = "Cost";
            // 
            // Amount
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle3;
            this.Amount.HeaderText = "金額";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // Note
            // 
            this.Note.HeaderText = "備考";
            this.Note.Name = "Note";
            this.Note.Width = 120;
            // 
            // comboBoxWork
            // 
            this.comboBoxWork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWork.Enabled = false;
            this.comboBoxWork.FormattingEnabled = true;
            this.comboBoxWork.Location = new System.Drawing.Point(170, 18);
            this.comboBoxWork.Name = "comboBoxWork";
            this.comboBoxWork.Size = new System.Drawing.Size(80, 23);
            this.comboBoxWork.TabIndex = 89;
            this.comboBoxWork.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.Enabled = false;
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Items.AddRange(new object[] {
            "H",
            "K",
            "S",
            "T"});
            this.comboBoxOffice.Location = new System.Drawing.Point(84, 18);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(80, 23);
            this.comboBoxOffice.TabIndex = 88;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(131, 629);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(100, 40);
            this.buttonNew.TabIndex = 90;
            this.buttonNew.Text = "新規保存";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1131, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 91;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // checkBoxPublish
            // 
            this.checkBoxPublish.AutoSize = true;
            this.checkBoxPublish.Location = new System.Drawing.Point(1069, 641);
            this.checkBoxPublish.Name = "checkBoxPublish";
            this.checkBoxPublish.Size = new System.Drawing.Size(122, 19);
            this.checkBoxPublish.TabIndex = 143;
            this.checkBoxPublish.Text = "発行元を本社とする";
            this.checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // FormOutsource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1224, 681);
            this.Controls.Add(this.checkBoxPublish);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.comboBoxWork);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.dateTimePickerReceiptDate);
            this.Controls.Add(this.dateTimePickerInspectDate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.labelOrderNo);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.buttonPrintConfirm);
            this.Controls.Add(this.buttonPrintContent);
            this.Controls.Add(this.buttonPrintOrder);
            this.Controls.Add(this.buttonReCalc);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonOverWrite);
            this.Controls.Add(this.buttonCopyAndNext);
            this.Controls.Add(this.labelPageNo);
            this.Controls.Add(this.buttonFromPlan);
            this.Controls.Add(this.buttonNextData);
            this.Controls.Add(this.buttonPrevData);
            this.Controls.Add(this.labelTaskCode);
            this.Controls.Add(this.comboBoxPayRoule);
            this.Controls.Add(this.labelSum);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.textBoxDeliveryPoint);
            this.Controls.Add(this.labelTerm);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxPartner);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerOEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerOStart);
            this.Controls.Add(this.dateTimePickerOrderDate);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormOutsource";
            this.Text = "外注内訳書";
            this.Load += new System.EventHandler(this.FormOutsource_Load);
            this.Shown += new System.EventHandler(this.FormOutsource_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerOrderDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerOStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerOEnd;
        private System.Windows.Forms.Label label3;
        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPartner;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelTask;
        private System.Windows.Forms.Label labelTerm;
        private System.Windows.Forms.TextBox textBoxDeliveryPoint;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.ComboBox comboBoxPayRoule;
        private System.Windows.Forms.Label labelTaskCode;
        private System.Windows.Forms.Button buttonPrevData;
        private System.Windows.Forms.Button buttonNextData;
        private System.Windows.Forms.Button buttonFromPlan;
        private System.Windows.Forms.Label labelPageNo;
        private System.Windows.Forms.Button buttonCopyAndNext;
        private System.Windows.Forms.Button buttonOverWrite;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonReCalc;
        private System.Windows.Forms.Button buttonPrintOrder;
        private System.Windows.Forms.Button buttonPrintContent;
        private System.Windows.Forms.Button buttonPrintConfirm;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelOrderNo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DateTimePicker dateTimePickerInspectDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerReceiptDate;
        private System.Windows.Forms.DataGridViewButtonColumn Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.ComboBox comboBoxWork;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.CheckBox checkBoxPublish;
    }
}


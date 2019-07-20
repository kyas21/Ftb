namespace EstimPlan
{
    partial class FormEstimate
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
            this.comboBoxVersion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTask = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPartner = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelWorkingPlace = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxTitle = new System.Windows.Forms.ComboBox();
            this.buttonFromPlan = new System.Windows.Forms.Button();
            this.labelTax = new System.Windows.Forms.Label();
            this.labelAmount = new System.Windows.Forms.Label();
            this.labelTotalAmount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.panelCarbon = new System.Windows.Forms.Panel();
            this.textBoxContract = new System.Windows.Forms.TextBox();
            this.textBoxMinBid = new System.Windows.Forms.TextBox();
            this.textBoxBudgets = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonNWrite = new System.Windows.Forms.Button();
            this.buttonOWrite = new System.Windows.Forms.Button();
            this.labelTaskCode = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxPVersion = new System.Windows.Forms.ComboBox();
            this.labelMsg = new System.Windows.Forms.Label();
            this.labelCostType = new System.Windows.Forms.Label();
            this.buttonReCalc = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.textBoxExpenses = new System.Windows.Forms.TextBox();
            this.textBoxTaxRate = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
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
            this.HAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxPublish = new System.Windows.Forms.CheckBox();
            this.panelCarbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxVersion
            // 
            this.comboBoxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVersion.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxVersion.FormattingEnabled = true;
            this.comboBoxVersion.Location = new System.Drawing.Point(134, 16);
            this.comboBoxVersion.Name = "comboBoxVersion";
            this.comboBoxVersion.Size = new System.Drawing.Size(52, 28);
            this.comboBoxVersion.TabIndex = 3;
            this.comboBoxVersion.SelectedIndexChanged += new System.EventHandler(this.comboBoxVersion_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "業務名：";
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.Location = new System.Drawing.Point(200, 61);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(114, 15);
            this.labelTask.TabIndex = 6;
            this.labelTask.Text = "TaskXXXXXXXXXX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(564, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "発注者：";
            // 
            // labelPartner
            // 
            this.labelPartner.AutoSize = true;
            this.labelPartner.Location = new System.Drawing.Point(613, 81);
            this.labelPartner.Name = "labelPartner";
            this.labelPartner.Size = new System.Drawing.Size(130, 15);
            this.labelPartner.TabIndex = 8;
            this.labelPartner.Text = "PartnerXXXXXXXXXX";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(188, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "版";
            // 
            // labelWorkingPlace
            // 
            this.labelWorkingPlace.AutoSize = true;
            this.labelWorkingPlace.Location = new System.Drawing.Point(72, 81);
            this.labelWorkingPlace.Name = "labelWorkingPlace";
            this.labelWorkingPlace.Size = new System.Drawing.Size(89, 15);
            this.labelWorkingPlace.TabIndex = 13;
            this.labelWorkingPlace.Text = "業務場所の表示";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(11, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "業務場所：";
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitle.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(6, 16);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(122, 28);
            this.comboBoxTitle.TabIndex = 20;
            this.comboBoxTitle.SelectedIndexChanged += new System.EventHandler(this.comboBoxTitle_SelectedIndexChanged);
            // 
            // buttonFromPlan
            // 
            this.buttonFromPlan.BackColor = System.Drawing.Color.OldLace;
            this.buttonFromPlan.Location = new System.Drawing.Point(421, 11);
            this.buttonFromPlan.Name = "buttonFromPlan";
            this.buttonFromPlan.Size = new System.Drawing.Size(130, 40);
            this.buttonFromPlan.TabIndex = 21;
            this.buttonFromPlan.Text = "予算書から作成";
            this.buttonFromPlan.UseVisualStyleBackColor = false;
            this.buttonFromPlan.Click += new System.EventHandler(this.button_Click);
            this.buttonFromPlan.MouseHover += new System.EventHandler(this.buttonFromPlan_MouseHover);
            // 
            // labelTax
            // 
            this.labelTax.Location = new System.Drawing.Point(84, 146);
            this.labelTax.Name = "labelTax";
            this.labelTax.Size = new System.Drawing.Size(86, 15);
            this.labelTax.TabIndex = 30;
            this.labelTax.Text = "消費税";
            this.labelTax.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(81, 126);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(89, 15);
            this.labelAmount.TabIndex = 29;
            this.labelAmount.Text = "見積内訳合計";
            this.labelAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTotalAmount
            // 
            this.labelTotalAmount.Location = new System.Drawing.Point(74, 106);
            this.labelTotalAmount.Name = "labelTotalAmount";
            this.labelTotalAmount.Size = new System.Drawing.Size(96, 15);
            this.labelTotalAmount.TabIndex = 28;
            this.labelTotalAmount.Text = "下記合計";
            this.labelTotalAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(21, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 27;
            this.label7.Text = "消費税額：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(21, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 26;
            this.label5.Text = "業務代金：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(11, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 25;
            this.label6.Text = "合計金額：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(311, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 15);
            this.label8.TabIndex = 31;
            this.label8.Text = "その他：";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Location = new System.Drawing.Point(367, 143);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(556, 23);
            this.textBoxNote.TabIndex = 32;
            // 
            // panelCarbon
            // 
            this.panelCarbon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCarbon.Controls.Add(this.textBoxContract);
            this.panelCarbon.Controls.Add(this.textBoxMinBid);
            this.panelCarbon.Controls.Add(this.textBoxBudgets);
            this.panelCarbon.Controls.Add(this.label11);
            this.panelCarbon.Controls.Add(this.label10);
            this.panelCarbon.Controls.Add(this.label12);
            this.panelCarbon.Controls.Add(this.label9);
            this.panelCarbon.Location = new System.Drawing.Point(1032, 46);
            this.panelCarbon.Name = "panelCarbon";
            this.panelCarbon.Size = new System.Drawing.Size(218, 105);
            this.panelCarbon.TabIndex = 33;
            // 
            // textBoxContract
            // 
            this.textBoxContract.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxContract.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxContract.Location = new System.Drawing.Point(111, 76);
            this.textBoxContract.Name = "textBoxContract";
            this.textBoxContract.Size = new System.Drawing.Size(100, 23);
            this.textBoxContract.TabIndex = 34;
            this.textBoxContract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxContract.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBoxMinBid
            // 
            this.textBoxMinBid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMinBid.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxMinBid.Location = new System.Drawing.Point(111, 49);
            this.textBoxMinBid.Name = "textBoxMinBid";
            this.textBoxMinBid.Size = new System.Drawing.Size(100, 23);
            this.textBoxMinBid.TabIndex = 33;
            this.textBoxMinBid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMinBid.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBoxBudgets
            // 
            this.textBoxBudgets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBudgets.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxBudgets.Location = new System.Drawing.Point(111, 22);
            this.textBoxBudgets.MaxLength = 0;
            this.textBoxBudgets.Name = "textBoxBudgets";
            this.textBoxBudgets.Size = new System.Drawing.Size(100, 23);
            this.textBoxBudgets.TabIndex = 32;
            this.textBoxBudgets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxBudgets.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(31, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 31;
            this.label11.Text = "決定金額：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(31, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 15);
            this.label10.TabIndex = 30;
            this.label10.Text = "最低応札額：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(31, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 15);
            this.label12.TabIndex = 29;
            this.label12.Text = "予算額：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "見積控項目";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(400, 629);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 37;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(270, 629);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 40);
            this.buttonDelete.TabIndex = 36;
            this.buttonDelete.Text = "削除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonNWrite
            // 
            this.buttonNWrite.Location = new System.Drawing.Point(136, 629);
            this.buttonNWrite.Name = "buttonNWrite";
            this.buttonNWrite.Size = new System.Drawing.Size(100, 40);
            this.buttonNWrite.TabIndex = 35;
            this.buttonNWrite.Text = "新版で保存";
            this.buttonNWrite.UseVisualStyleBackColor = true;
            this.buttonNWrite.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOWrite
            // 
            this.buttonOWrite.Location = new System.Drawing.Point(11, 629);
            this.buttonOWrite.Name = "buttonOWrite";
            this.buttonOWrite.Size = new System.Drawing.Size(100, 40);
            this.buttonOWrite.TabIndex = 34;
            this.buttonOWrite.Text = "上書保存";
            this.buttonOWrite.UseVisualStyleBackColor = true;
            this.buttonOWrite.Click += new System.EventHandler(this.button_Click);
            // 
            // labelTaskCode
            // 
            this.labelTaskCode.AutoSize = true;
            this.labelTaskCode.Location = new System.Drawing.Point(72, 61);
            this.labelTaskCode.Name = "labelTaskCode";
            this.labelTaskCode.Size = new System.Drawing.Size(55, 15);
            this.labelTaskCode.TabIndex = 39;
            this.labelTaskCode.Text = "業務番号";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 15);
            this.label13.TabIndex = 38;
            this.label13.Text = "業務番号：";
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Location = new System.Drawing.Point(1007, 16);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(91, 15);
            this.labelPublisher.TabIndex = 41;
            this.labelPublisher.Text = "発行部署名表示";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(947, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 15);
            this.label14.TabIndex = 40;
            this.label14.Text = "発行部署：";
            // 
            // comboBoxPVersion
            // 
            this.comboBoxPVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPVersion.FormattingEnabled = true;
            this.comboBoxPVersion.Location = new System.Drawing.Point(557, 21);
            this.comboBoxPVersion.Name = "comboBoxPVersion";
            this.comboBoxPVersion.Size = new System.Drawing.Size(44, 23);
            this.comboBoxPVersion.TabIndex = 42;
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMsg.ForeColor = System.Drawing.Color.Blue;
            this.labelMsg.Location = new System.Drawing.Point(607, 19);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(125, 20);
            this.labelMsg.TabIndex = 43;
            this.labelMsg.Text = "Assist Message";
            // 
            // labelCostType
            // 
            this.labelCostType.AutoSize = true;
            this.labelCostType.Location = new System.Drawing.Point(249, 24);
            this.labelCostType.Name = "labelCostType";
            this.labelCostType.Size = new System.Drawing.Size(42, 15);
            this.labelCostType.TabIndex = 44;
            this.labelCostType.Text = "CostG";
            this.labelCostType.Visible = false;
            // 
            // buttonReCalc
            // 
            this.buttonReCalc.BackColor = System.Drawing.Color.AntiqueWhite;
            this.buttonReCalc.Location = new System.Drawing.Point(1102, 158);
            this.buttonReCalc.Name = "buttonReCalc";
            this.buttonReCalc.Size = new System.Drawing.Size(150, 30);
            this.buttonReCalc.TabIndex = 45;
            this.buttonReCalc.Text = "再計算";
            this.buttonReCalc.UseVisualStyleBackColor = false;
            this.buttonReCalc.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(570, 629);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(140, 40);
            this.buttonPrint.TabIndex = 46;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // textBoxExpenses
            // 
            this.textBoxExpenses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExpenses.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxExpenses.Location = new System.Drawing.Point(252, 171);
            this.textBoxExpenses.Name = "textBoxExpenses";
            this.textBoxExpenses.Size = new System.Drawing.Size(60, 23);
            this.textBoxExpenses.TabIndex = 140;
            this.textBoxExpenses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxExpenses.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // textBoxTaxRate
            // 
            this.textBoxTaxRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTaxRate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxTaxRate.Location = new System.Drawing.Point(91, 171);
            this.textBoxTaxRate.Name = "textBoxTaxRate";
            this.textBoxTaxRate.Size = new System.Drawing.Size(60, 23);
            this.textBoxTaxRate.TabIndex = 139;
            this.textBoxTaxRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxTaxRate.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label38
            // 
            this.label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label38.Location = new System.Drawing.Point(171, 171);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(82, 23);
            this.label38.TabIndex = 138;
            this.label38.Text = "諸経費（％）";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label39.Location = new System.Drawing.Point(11, 171);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(82, 23);
            this.label39.TabIndex = 137;
            this.label39.Text = "消費税（％）";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1170, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 141;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
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
            this.Note,
            this.HAmount});
            this.dataGridView1.Location = new System.Drawing.Point(11, 196);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1240, 420);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellButtonClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
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
            this.ItemCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemCode.Width = 40;
            // 
            // Item
            // 
            this.Item.HeaderText = "名称";
            this.Item.Name = "Item";
            this.Item.Width = 320;
            // 
            // ItemDetail
            // 
            this.ItemDetail.HeaderText = "細別";
            this.ItemDetail.Name = "ItemDetail";
            this.ItemDetail.Width = 320;
            // 
            // Quantity
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = null;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            this.Quantity.Width = 70;
            // 
            // Unit
            // 
            this.Unit.HeaderText = "単位";
            this.Unit.Name = "Unit";
            this.Unit.Width = 70;
            // 
            // Cost
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = null;
            this.Cost.DefaultCellStyle = dataGridViewCellStyle2;
            this.Cost.HeaderText = "単価";
            this.Cost.Name = "Cost";
            this.Cost.Width = 90;
            // 
            // Amount
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.NullValue = null;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle3;
            this.Amount.HeaderText = "金額";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 90;
            // 
            // Note
            // 
            this.Note.HeaderText = "備考";
            this.Note.Name = "Note";
            this.Note.Width = 180;
            // 
            // HAmount
            // 
            this.HAmount.HeaderText = "入力金額";
            this.HAmount.Name = "HAmount";
            this.HAmount.Visible = false;
            this.HAmount.Width = 90;
            // 
            // checkBoxPublish
            // 
            this.checkBoxPublish.AutoSize = true;
            this.checkBoxPublish.Location = new System.Drawing.Point(716, 641);
            this.checkBoxPublish.Name = "checkBoxPublish";
            this.checkBoxPublish.Size = new System.Drawing.Size(122, 19);
            this.checkBoxPublish.TabIndex = 142;
            this.checkBoxPublish.Text = "発行元を本社とする";
            this.checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // FormEstimate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.checkBoxPublish);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxExpenses);
            this.Controls.Add(this.textBoxTaxRate);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonReCalc);
            this.Controls.Add(this.labelCostType);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.comboBoxPVersion);
            this.Controls.Add(this.labelPublisher);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.labelTaskCode);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonNWrite);
            this.Controls.Add(this.buttonOWrite);
            this.Controls.Add(this.panelCarbon);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelTax);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.labelTotalAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonFromPlan);
            this.Controls.Add(this.comboBoxTitle);
            this.Controls.Add(this.labelWorkingPlace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelPartner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxVersion);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormEstimate";
            this.Text = "見積内訳入力";
            this.Load += new System.EventHandler(this.FormEstimate_Load);
            this.Shown += new System.EventHandler(this.FormEstimate_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEstimate_KeyDown);
            this.panelCarbon.ResumeLayout(false);
            this.panelCarbon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxVersion;
        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPartner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelWorkingPlace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxTitle;
        private System.Windows.Forms.Button buttonFromPlan;
        private System.Windows.Forms.Label labelTax;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label labelTotalAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Panel panelCarbon;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxContract;
        private System.Windows.Forms.TextBox textBoxMinBid;
        private System.Windows.Forms.TextBox textBoxBudgets;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonNWrite;
        private System.Windows.Forms.Button buttonOWrite;
        private System.Windows.Forms.Label labelTaskCode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxPVersion;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Label labelCostType;
        private System.Windows.Forms.Button buttonReCalc;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.TextBox textBoxExpenses;
        private System.Windows.Forms.TextBox textBoxTaxRate;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridViewButtonColumn Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.DataGridViewTextBoxColumn HAmount;
        private System.Windows.Forms.CheckBox checkBoxPublish;
    }
}


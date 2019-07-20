namespace Accounts
{
    partial class FormInvoice
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonCopyAndNext = new System.Windows.Forms.Button();
            this.labelPageNo = new System.Windows.Forms.Label();
            this.buttonNextData = new System.Windows.Forms.Button();
            this.buttonPrevData = new System.Windows.Forms.Button();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.dateTimePickerRecordedDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTask = new System.Windows.Forms.Label();
            this.buttonFromEstimate = new System.Windows.Forms.Button();
            this.buttonReCalc = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.textBoxExpenses = new System.Windows.Forms.TextBox();
            this.textBoxTaxRate = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonOverWrite = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.labelMsg = new System.Windows.Forms.Label();
            this.labelPartnerCode = new System.Windows.Forms.Label();
            this.dataGridView1 = new DataGridViewPlus.DataGridViewPlus();
            this.Month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelPartnerName = new System.Windows.Forms.Label();
            this.checkBoxPublish = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCopyAndNext
            // 
            this.buttonCopyAndNext.Location = new System.Drawing.Point(986, 118);
            this.buttonCopyAndNext.Name = "buttonCopyAndNext";
            this.buttonCopyAndNext.Size = new System.Drawing.Size(156, 30);
            this.buttonCopyAndNext.TabIndex = 128;
            this.buttonCopyAndNext.Text = "データを残して次回";
            this.buttonCopyAndNext.UseVisualStyleBackColor = true;
            this.buttonCopyAndNext.Click += new System.EventHandler(this.button_Click);
            // 
            // labelPageNo
            // 
            this.labelPageNo.BackColor = System.Drawing.Color.Honeydew;
            this.labelPageNo.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPageNo.Location = new System.Drawing.Point(1027, 53);
            this.labelPageNo.Name = "labelPageNo";
            this.labelPageNo.Size = new System.Drawing.Size(75, 27);
            this.labelPageNo.TabIndex = 127;
            this.labelPageNo.Text = "0/0";
            this.labelPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonNextData
            // 
            this.buttonNextData.Location = new System.Drawing.Point(1067, 86);
            this.buttonNextData.Name = "buttonNextData";
            this.buttonNextData.Size = new System.Drawing.Size(75, 30);
            this.buttonNextData.TabIndex = 126;
            this.buttonNextData.Text = "次回";
            this.buttonNextData.UseVisualStyleBackColor = true;
            this.buttonNextData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrevData
            // 
            this.buttonPrevData.Location = new System.Drawing.Point(986, 86);
            this.buttonPrevData.Name = "buttonPrevData";
            this.buttonPrevData.Size = new System.Drawing.Size(75, 30);
            this.buttonPrevData.TabIndex = 125;
            this.buttonPrevData.Text = "前回";
            this.buttonPrevData.UseVisualStyleBackColor = true;
            this.buttonPrevData.Click += new System.EventHandler(this.button_Click);
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Location = new System.Drawing.Point(939, 11);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(115, 15);
            this.labelPublisher.TabIndex = 124;
            this.labelPublisher.Text = "部署名が表示されます";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(866, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 15);
            this.label15.TabIndex = 123;
            this.label15.Text = "発行部署：";
            // 
            // dateTimePickerRecordedDate
            // 
            this.dateTimePickerRecordedDate.Location = new System.Drawing.Point(120, 70);
            this.dateTimePickerRecordedDate.Name = "dateTimePickerRecordedDate";
            this.dateTimePickerRecordedDate.Size = new System.Drawing.Size(130, 23);
            this.dateTimePickerRecordedDate.TabIndex = 122;
            this.dateTimePickerRecordedDate.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 121;
            this.label1.Text = "業務名：";
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.Location = new System.Drawing.Point(72, 46);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(105, 15);
            this.labelTask.TabIndex = 120;
            this.labelTask.Text = "業務名が表示される";
            // 
            // buttonFromEstimate
            // 
            this.buttonFromEstimate.BackColor = System.Drawing.Color.OldLace;
            this.buttonFromEstimate.Location = new System.Drawing.Point(696, 6);
            this.buttonFromEstimate.Name = "buttonFromEstimate";
            this.buttonFromEstimate.Size = new System.Drawing.Size(156, 40);
            this.buttonFromEstimate.TabIndex = 129;
            this.buttonFromEstimate.Text = "見積書データの取込";
            this.buttonFromEstimate.UseVisualStyleBackColor = false;
            this.buttonFromEstimate.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonReCalc
            // 
            this.buttonReCalc.BackColor = System.Drawing.Color.AntiqueWhite;
            this.buttonReCalc.Location = new System.Drawing.Point(696, 114);
            this.buttonReCalc.Name = "buttonReCalc";
            this.buttonReCalc.Size = new System.Drawing.Size(156, 30);
            this.buttonReCalc.TabIndex = 130;
            this.buttonReCalc.Text = "再計算";
            this.buttonReCalc.UseVisualStyleBackColor = false;
            this.buttonReCalc.Click += new System.EventHandler(this.button_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(11, 76);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(103, 15);
            this.label25.TabIndex = 131;
            this.label25.Text = "請求書発行日付：";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxExpenses
            // 
            this.textBoxExpenses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExpenses.Location = new System.Drawing.Point(310, 122);
            this.textBoxExpenses.Name = "textBoxExpenses";
            this.textBoxExpenses.Size = new System.Drawing.Size(82, 23);
            this.textBoxExpenses.TabIndex = 136;
            this.textBoxExpenses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTaxRate
            // 
            this.textBoxTaxRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTaxRate.Location = new System.Drawing.Point(108, 122);
            this.textBoxTaxRate.Name = "textBoxTaxRate";
            this.textBoxTaxRate.Size = new System.Drawing.Size(82, 23);
            this.textBoxTaxRate.TabIndex = 135;
            this.textBoxTaxRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(213, 127);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(91, 15);
            this.label38.TabIndex = 133;
            this.label38.Text = "諸経費（％）：";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(10, 127);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(91, 15);
            this.label39.TabIndex = 132;
            this.label39.Text = "消費税（％）：";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(141, 629);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(100, 40);
            this.buttonNew.TabIndex = 140;
            this.buttonNew.Text = "新規保存";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(441, 629);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 139;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(311, 629);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 40);
            this.buttonDelete.TabIndex = 138;
            this.buttonDelete.Text = "削除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOverWrite
            // 
            this.buttonOverWrite.Location = new System.Drawing.Point(11, 629);
            this.buttonOverWrite.Name = "buttonOverWrite";
            this.buttonOverWrite.Size = new System.Drawing.Size(100, 40);
            this.buttonOverWrite.TabIndex = 137;
            this.buttonOverWrite.Text = "上書保存";
            this.buttonOverWrite.UseVisualStyleBackColor = true;
            this.buttonOverWrite.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(621, 629);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(140, 40);
            this.buttonPrint.TabIndex = 141;
            this.buttonPrint.Text = "請求書印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1072, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 142;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(417, 14);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 20);
            this.label18.TabIndex = 143;
            this.label18.Text = "御中";
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMsg.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelMsg.Location = new System.Drawing.Point(514, 14);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(76, 20);
            this.labelMsg.TabIndex = 144;
            this.labelMsg.Text = "labelMsg";
            // 
            // labelPartnerCode
            // 
            this.labelPartnerCode.AutoSize = true;
            this.labelPartnerCode.Location = new System.Drawing.Point(321, 76);
            this.labelPartnerCode.Name = "labelPartnerCode";
            this.labelPartnerCode.Size = new System.Drawing.Size(106, 15);
            this.labelPartnerCode.TabIndex = 145;
            this.labelPartnerCode.Text = "labelPartnerCode";
            this.labelPartnerCode.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Month,
            this.Day,
            this.ItemCode,
            this.Item,
            this.ItemDetail,
            this.Quantity,
            this.Unit,
            this.CCost,
            this.Amount,
            this.HAmount,
            this.CQuantity,
            this.HContract,
            this.SQuantity,
            this.SAmount});
            this.dataGridView1.Location = new System.Drawing.Point(11, 151);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1130, 461);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // Month
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Month.DefaultCellStyle = dataGridViewCellStyle7;
            this.Month.HeaderText = "月";
            this.Month.Name = "Month";
            this.Month.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Month.Width = 30;
            // 
            // Day
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Day.DefaultCellStyle = dataGridViewCellStyle8;
            this.Day.HeaderText = "日";
            this.Day.Name = "Day";
            this.Day.Width = 30;
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
            this.ItemDetail.Width = 250;
            // 
            // Quantity
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle9;
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            // 
            // Unit
            // 
            this.Unit.HeaderText = "単位";
            this.Unit.Name = "Unit";
            // 
            // CCost
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.CCost.DefaultCellStyle = dataGridViewCellStyle10;
            this.CCost.HeaderText = "単価";
            this.CCost.Name = "CCost";
            // 
            // Amount
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle11;
            this.Amount.HeaderText = "金額";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // HAmount
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.HAmount.DefaultCellStyle = dataGridViewCellStyle12;
            this.HAmount.HeaderText = "入力金額";
            this.HAmount.Name = "HAmount";
            // 
            // CQuantity
            // 
            this.CQuantity.HeaderText = "契約数量（Dummy）";
            this.CQuantity.Name = "CQuantity";
            this.CQuantity.Visible = false;
            // 
            // HContract
            // 
            this.HContract.HeaderText = "入力契約金額（Dummy）";
            this.HContract.Name = "HContract";
            this.HContract.Visible = false;
            // 
            // SQuantity
            // 
            this.SQuantity.HeaderText = "前回までの数量（Dummy）";
            this.SQuantity.Name = "SQuantity";
            this.SQuantity.Visible = false;
            // 
            // SAmount
            // 
            this.SAmount.HeaderText = "前回までの金額（Dummy）";
            this.SAmount.Name = "SAmount";
            this.SAmount.Visible = false;
            // 
            // labelPartnerName
            // 
            this.labelPartnerName.AutoSize = true;
            this.labelPartnerName.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPartnerName.Location = new System.Drawing.Point(12, 14);
            this.labelPartnerName.Name = "labelPartnerName";
            this.labelPartnerName.Size = new System.Drawing.Size(57, 20);
            this.labelPartnerName.TabIndex = 146;
            this.labelPartnerName.Text = "発注者";
            // 
            // checkBoxPublish
            // 
            this.checkBoxPublish.AutoSize = true;
            this.checkBoxPublish.Location = new System.Drawing.Point(767, 641);
            this.checkBoxPublish.Name = "checkBoxPublish";
            this.checkBoxPublish.Size = new System.Drawing.Size(122, 19);
            this.checkBoxPublish.TabIndex = 147;
            this.checkBoxPublish.Text = "発行元を本社とする";
            this.checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // FormInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1164, 681);
            this.Controls.Add(this.checkBoxPublish);
            this.Controls.Add(this.labelPartnerName);
            this.Controls.Add(this.labelPartnerCode);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonOverWrite);
            this.Controls.Add(this.textBoxExpenses);
            this.Controls.Add(this.textBoxTaxRate);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.buttonReCalc);
            this.Controls.Add(this.buttonFromEstimate);
            this.Controls.Add(this.buttonCopyAndNext);
            this.Controls.Add(this.labelPageNo);
            this.Controls.Add(this.buttonNextData);
            this.Controls.Add(this.buttonPrevData);
            this.Controls.Add(this.labelPublisher);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.dateTimePickerRecordedDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormInvoice";
            this.Text = "請求書";
            this.Load += new System.EventHandler(this.FormInvoice_Load);
            this.Shown += new System.EventHandler(this.FormInvoice_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormInvoice_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.Button buttonCopyAndNext;
        private System.Windows.Forms.Label labelPageNo;
        private System.Windows.Forms.Button buttonNextData;
        private System.Windows.Forms.Button buttonPrevData;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dateTimePickerRecordedDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTask;
        private System.Windows.Forms.Button buttonFromEstimate;
        private System.Windows.Forms.Button buttonReCalc;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBoxExpenses;
        private System.Windows.Forms.TextBox textBoxTaxRate;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonOverWrite;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Label labelPartnerCode;
        private System.Windows.Forms.Label labelPartnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Month;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn CCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn HAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn HContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn SQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAmount;
        private System.Windows.Forms.CheckBox checkBoxPublish;
    }
}
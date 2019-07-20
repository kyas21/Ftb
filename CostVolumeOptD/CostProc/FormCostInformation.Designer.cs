namespace CostProc
{
    partial class FormCostInformation
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
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.labelIssueDepartment = new System.Windows.Forms.Label();
            this.dateTimePickerEntryDate = new System.Windows.Forms.DateTimePicker();
            this.labelEntryDate = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.buttonPWeek = new System.Windows.Forms.Button();
            this.buttonNWeek = new System.Windows.Forms.Button();
            this.dateTimePickerMonth = new DateTimePickerEx.DateTimePickerEx();
            this.dataGridView1 = new DataGridViewPlus.DataGridViewPlus();
            this.MemberCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MemberName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonEnd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(192, 11);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(60, 30);
            this.buttonNext.TabIndex = 46;
            this.buttonNext.Text = "次  >";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPrev.Location = new System.Drawing.Point(126, 11);
            this.buttonPrev.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(60, 30);
            this.buttonPrev.TabIndex = 45;
            this.buttonPrev.Text = "<  前";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.button_Click);
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepart.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(488, 57);
            this.comboBoxDepart.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(87, 23);
            this.comboBoxDepart.TabIndex = 49;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Items.AddRange(new object[] {
            "H",
            "K",
            "S",
            "T"});
            this.comboBoxOffice.Location = new System.Drawing.Point(413, 57);
            this.comboBoxOffice.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(67, 23);
            this.comboBoxOffice.TabIndex = 48;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // labelIssueDepartment
            // 
            this.labelIssueDepartment.AutoSize = true;
            this.labelIssueDepartment.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.labelIssueDepartment.Location = new System.Drawing.Point(356, 61);
            this.labelIssueDepartment.Name = "labelIssueDepartment";
            this.labelIssueDepartment.Size = new System.Drawing.Size(43, 15);
            this.labelIssueDepartment.TabIndex = 51;
            this.labelIssueDepartment.Text = "部署：";
            // 
            // dateTimePickerEntryDate
            // 
            this.dateTimePickerEntryDate.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.dateTimePickerEntryDate.Location = new System.Drawing.Point(1065, 58);
            this.dateTimePickerEntryDate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dateTimePickerEntryDate.Name = "dateTimePickerEntryDate";
            this.dateTimePickerEntryDate.Size = new System.Drawing.Size(136, 23);
            this.dateTimePickerEntryDate.TabIndex = 52;
            this.dateTimePickerEntryDate.Visible = false;
            this.dateTimePickerEntryDate.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // labelEntryDate
            // 
            this.labelEntryDate.AutoSize = true;
            this.labelEntryDate.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.labelEntryDate.Location = new System.Drawing.Point(1016, 61);
            this.labelEntryDate.Name = "labelEntryDate";
            this.labelEntryDate.Size = new System.Drawing.Size(43, 15);
            this.labelEntryDate.TabIndex = 53;
            this.labelEntryDate.Text = "日付：";
            this.labelEntryDate.Visible = false;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMessage.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labelMessage.Location = new System.Drawing.Point(417, 17);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(63, 17);
            this.labelMessage.TabIndex = 72;
            this.labelMessage.Text = "Message";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(611, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 73;
            this.label1.Text = "月：";
            // 
            // comboBoxMonth
            // 
            this.comboBoxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonth.FormattingEnabled = true;
            this.comboBoxMonth.Location = new System.Drawing.Point(889, 58);
            this.comboBoxMonth.Name = "comboBoxMonth";
            this.comboBoxMonth.Size = new System.Drawing.Size(52, 23);
            this.comboBoxMonth.TabIndex = 74;
            this.comboBoxMonth.Visible = false;
            this.comboBoxMonth.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonPWeek
            // 
            this.buttonPWeek.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPWeek.Location = new System.Drawing.Point(31, 11);
            this.buttonPWeek.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonPWeek.Name = "buttonPWeek";
            this.buttonPWeek.Size = new System.Drawing.Size(80, 30);
            this.buttonPWeek.TabIndex = 75;
            this.buttonPWeek.Text = "<<  前週";
            this.buttonPWeek.UseVisualStyleBackColor = true;
            this.buttonPWeek.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonNWeek
            // 
            this.buttonNWeek.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonNWeek.Location = new System.Drawing.Point(267, 11);
            this.buttonNWeek.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonNWeek.Name = "buttonNWeek";
            this.buttonNWeek.Size = new System.Drawing.Size(80, 30);
            this.buttonNWeek.TabIndex = 76;
            this.buttonNWeek.Text = "次週  >>";
            this.buttonNWeek.UseVisualStyleBackColor = true;
            this.buttonNWeek.Click += new System.EventHandler(this.button_Click);
            // 
            // dateTimePickerMonth
            // 
            this.dateTimePickerMonth.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateTimePickerMonth.CustomFormat = "yyyy年MM月";
            this.dateTimePickerMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMonth.Location = new System.Drawing.Point(648, 58);
            this.dateTimePickerMonth.Name = "dateTimePickerMonth";
            this.dateTimePickerMonth.Size = new System.Drawing.Size(103, 23);
            this.dateTimePickerMonth.TabIndex = 77;
            this.dateTimePickerMonth.Value = new System.DateTime(2016, 10, 1, 10, 34, 31, 984);
            this.dateTimePickerMonth.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MemberCode,
            this.MemberName,
            this.Date0,
            this.Date1,
            this.Date2,
            this.Date3,
            this.Date4,
            this.Date5,
            this.Date6});
            this.dataGridView1.Location = new System.Drawing.Point(31, 130);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1212, 444);
            this.dataGridView1.TabIndex = 54;
            // 
            // MemberCode
            // 
            this.MemberCode.HeaderText = "番号";
            this.MemberCode.Name = "MemberCode";
            this.MemberCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MemberCode.Width = 40;
            // 
            // MemberName
            // 
            this.MemberName.HeaderText = "社員名";
            this.MemberName.Name = "MemberName";
            this.MemberName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Date0
            // 
            this.Date0.HeaderText = "date0";
            this.Date0.Name = "Date0";
            this.Date0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date0.Width = 150;
            // 
            // Date1
            // 
            this.Date1.HeaderText = "date1";
            this.Date1.Name = "Date1";
            this.Date1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date1.Width = 150;
            // 
            // Date2
            // 
            this.Date2.HeaderText = "date2";
            this.Date2.Name = "Date2";
            this.Date2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date2.Width = 150;
            // 
            // Date3
            // 
            this.Date3.HeaderText = "date3";
            this.Date3.Name = "Date3";
            this.Date3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date3.Width = 150;
            // 
            // Date4
            // 
            this.Date4.HeaderText = "date4";
            this.Date4.Name = "Date4";
            this.Date4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date4.Width = 150;
            // 
            // Date5
            // 
            this.Date5.HeaderText = "date5";
            this.Date5.Name = "Date5";
            this.Date5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date5.Width = 150;
            // 
            // Date6
            // 
            this.Date6.HeaderText = "date6";
            this.Date6.Name = "Date6";
            this.Date6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Date6.Width = 150;
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(1164, 6);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 78;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // FormCostInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 601);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.dateTimePickerMonth);
            this.Controls.Add(this.buttonNWeek);
            this.Controls.Add(this.buttonPWeek);
            this.Controls.Add(this.comboBoxMonth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dateTimePickerEntryDate);
            this.Controls.Add(this.labelEntryDate);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.labelIssueDepartment);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormCostInformation";
            this.Text = "作業内訳書入力状況";
            this.Load += new System.EventHandler(this.FormCostInformation_Load);
            this.Shown += new System.EventHandler(this.FormCostInformation_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label labelIssueDepartment;
        private System.Windows.Forms.DateTimePicker dateTimePickerEntryDate;
        private System.Windows.Forms.Label labelEntryDate;
        private DataGridViewPlus.DataGridViewPlus dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MemberCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn MemberName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date6;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.Button buttonPWeek;
        private System.Windows.Forms.Button buttonNWeek;
        private DateTimePickerEx.DateTimePickerEx dateTimePickerMonth;
        private System.Windows.Forms.Button buttonEnd;
    }
}
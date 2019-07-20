namespace EstimPlan
{
    partial class FormPlanningNoConfList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
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
            this.labelMsg = new System.Windows.Forms.Label();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreeningMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreeningDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApOfficerMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApOfficerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApPresidentMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApPresidentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMsg.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelMsg.Location = new System.Drawing.Point(221, 20);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(55, 20);
            this.labelMsg.TabIndex = 162;
            this.labelMsg.Text = "label1";
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(12, 630);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 161;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1163, 10);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 160;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(159, 20);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(56, 23);
            this.comboBoxDepart.TabIndex = 159;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(97, 20);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(56, 23);
            this.comboBoxOffice.TabIndex = 158;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(12, 23);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(79, 15);
            this.label31.TabIndex = 157;
            this.label31.Text = "営業事業所：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqNo,
            this.TaskCode,
            this.TaskName,
            this.VersionNo,
            this.CreateMName,
            this.CreateDate,
            this.ConfirmMName,
            this.ConfirmDate,
            this.ScreeningMName,
            this.ScreeningDate,
            this.ApOfficerMName,
            this.ApOfficerDate,
            this.ApPresidentMName,
            this.ApPresidentDate});
            this.dataGridView1.Location = new System.Drawing.Point(12, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1240, 541);
            this.dataGridView1.TabIndex = 156;
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
            // TaskCode
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TaskCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.TaskCode.HeaderText = "業務番号";
            this.TaskCode.Name = "TaskCode";
            this.TaskCode.ReadOnly = true;
            this.TaskCode.Width = 80;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "業務名";
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            this.TaskName.Width = 280;
            // 
            // VersionNo
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.VersionNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.VersionNo.HeaderText = "版";
            this.VersionNo.Name = "VersionNo";
            this.VersionNo.ReadOnly = true;
            this.VersionNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VersionNo.Width = 30;
            // 
            // CreateMName
            // 
            this.CreateMName.HeaderText = "作成";
            this.CreateMName.Name = "CreateMName";
            this.CreateMName.ReadOnly = true;
            this.CreateMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreateMName.Width = 80;
            // 
            // CreateDate
            // 
            this.CreateDate.HeaderText = "作成日";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreateDate.Width = 80;
            // 
            // ConfirmMName
            // 
            this.ConfirmMName.HeaderText = "確認";
            this.ConfirmMName.Name = "ConfirmMName";
            this.ConfirmMName.ReadOnly = true;
            this.ConfirmMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ConfirmMName.Width = 80;
            // 
            // ConfirmDate
            // 
            this.ConfirmDate.HeaderText = "確認日";
            this.ConfirmDate.Name = "ConfirmDate";
            this.ConfirmDate.ReadOnly = true;
            this.ConfirmDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ConfirmDate.Width = 80;
            // 
            // ScreeningMName
            // 
            this.ScreeningMName.HeaderText = "審査";
            this.ScreeningMName.Name = "ScreeningMName";
            this.ScreeningMName.ReadOnly = true;
            this.ScreeningMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ScreeningMName.Width = 80;
            // 
            // ScreeningDate
            // 
            this.ScreeningDate.HeaderText = "審査日";
            this.ScreeningDate.Name = "ScreeningDate";
            this.ScreeningDate.ReadOnly = true;
            this.ScreeningDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ScreeningDate.Width = 80;
            // 
            // ApOfficerMName
            // 
            this.ApOfficerMName.HeaderText = "担当役員";
            this.ApOfficerMName.Name = "ApOfficerMName";
            this.ApOfficerMName.ReadOnly = true;
            this.ApOfficerMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ApOfficerMName.Width = 80;
            // 
            // ApOfficerDate
            // 
            this.ApOfficerDate.HeaderText = "役員承認日";
            this.ApOfficerDate.Name = "ApOfficerDate";
            this.ApOfficerDate.ReadOnly = true;
            this.ApOfficerDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ApOfficerDate.Width = 80;
            // 
            // ApPresidentMName
            // 
            this.ApPresidentMName.HeaderText = "承認";
            this.ApPresidentMName.Name = "ApPresidentMName";
            this.ApPresidentMName.ReadOnly = true;
            this.ApPresidentMName.Width = 80;
            // 
            // ApPresidentDate
            // 
            this.ApPresidentDate.HeaderText = "承認日";
            this.ApPresidentDate.Name = "ApPresidentDate";
            this.ApPresidentDate.ReadOnly = true;
            this.ApPresidentDate.Width = 80;
            // 
            // FormPlanningNoConfList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormPlanningNoConfList";
            this.Text = "実行予算書承認未完了一覧表";
            this.Load += new System.EventHandler(this.FormPlanningNoConfList_Load);
            this.Shown += new System.EventHandler(this.FormPlanningNoConfList_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreeningMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreeningDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApOfficerMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApOfficerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApPresidentMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApPresidentDate;
    }
}
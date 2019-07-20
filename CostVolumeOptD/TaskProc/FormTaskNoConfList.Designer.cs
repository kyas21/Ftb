namespace TaskProc
{
    partial class FormTaskNoConfList
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.labelMsg = new System.Windows.Forms.Label();
            this.SeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesMinputDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Approval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApprovalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MakeOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MakeOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmAdm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            this.IssueDate,
            this.SalesMName,
            this.SalesMinputDate,
            this.Approval,
            this.ApprovalDate,
            this.MakeOrder,
            this.MakeOrderDate,
            this.ConfirmAdm,
            this.ConfirmDate});
            this.dataGridView1.Location = new System.Drawing.Point(11, 61);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1240, 541);
            this.dataGridView1.TabIndex = 0;
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(158, 18);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(56, 23);
            this.comboBoxDepart.TabIndex = 152;
            this.comboBoxDepart.Visible = false;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(96, 18);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(56, 23);
            this.comboBoxOffice.TabIndex = 151;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(11, 21);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(79, 15);
            this.label31.TabIndex = 150;
            this.label31.Text = "営業事業所：";
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1162, 8);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 153;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(11, 628);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 154;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMsg.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelMsg.Location = new System.Drawing.Point(220, 18);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(55, 20);
            this.labelMsg.TabIndex = 155;
            this.labelMsg.Text = "label1";
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
            this.TaskName.Width = 360;
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
            // IssueDate
            // 
            this.IssueDate.HeaderText = "発行日";
            this.IssueDate.Name = "IssueDate";
            this.IssueDate.ReadOnly = true;
            this.IssueDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IssueDate.Width = 80;
            // 
            // SalesMName
            // 
            this.SalesMName.HeaderText = "作成";
            this.SalesMName.Name = "SalesMName";
            this.SalesMName.ReadOnly = true;
            this.SalesMName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SalesMName.Width = 80;
            // 
            // SalesMinputDate
            // 
            this.SalesMinputDate.HeaderText = "作成日";
            this.SalesMinputDate.Name = "SalesMinputDate";
            this.SalesMinputDate.ReadOnly = true;
            this.SalesMinputDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SalesMinputDate.Width = 80;
            // 
            // Approval
            // 
            this.Approval.HeaderText = "着工承認";
            this.Approval.Name = "Approval";
            this.Approval.ReadOnly = true;
            this.Approval.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Approval.Width = 80;
            // 
            // ApprovalDate
            // 
            this.ApprovalDate.HeaderText = "着工承認日";
            this.ApprovalDate.Name = "ApprovalDate";
            this.ApprovalDate.ReadOnly = true;
            this.ApprovalDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ApprovalDate.Width = 80;
            // 
            // MakeOrder
            // 
            this.MakeOrder.HeaderText = "作成指示";
            this.MakeOrder.Name = "MakeOrder";
            this.MakeOrder.ReadOnly = true;
            this.MakeOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MakeOrder.Width = 80;
            // 
            // MakeOrderDate
            // 
            this.MakeOrderDate.HeaderText = "作成日";
            this.MakeOrderDate.Name = "MakeOrderDate";
            this.MakeOrderDate.ReadOnly = true;
            this.MakeOrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MakeOrderDate.Width = 80;
            // 
            // ConfirmAdm
            // 
            this.ConfirmAdm.HeaderText = "確認";
            this.ConfirmAdm.Name = "ConfirmAdm";
            this.ConfirmAdm.ReadOnly = true;
            this.ConfirmAdm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ConfirmAdm.Width = 80;
            // 
            // ConfirmDate
            // 
            this.ConfirmDate.HeaderText = "確認日";
            this.ConfirmDate.Name = "ConfirmDate";
            this.ConfirmDate.ReadOnly = true;
            this.ConfirmDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ConfirmDate.Width = 80;
            // 
            // FormTaskNoConfList
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
            this.Name = "FormTaskNoConfList";
            this.Text = "業務引継書 承認未完了一覧";
            this.Load += new System.EventHandler(this.FormTaskNoConfList_Load);
            this.Shown += new System.EventHandler(this.FormTaskNoConfList_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IssueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesMinputDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Approval;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApprovalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MakeOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn MakeOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmAdm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmDate;
    }
}
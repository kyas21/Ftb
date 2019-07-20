namespace Maintenance
{
    partial class FormImpAuthority
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaderMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeaderMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalesMCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PayOffID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostReportID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.dgvSetting = new DataGridViewPlus.DataGridViewPlus();
            this.AuthID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetting)).BeginInit();
            this.SuspendLayout();
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
            this.TaskCode.Width = 80;
            // 
            // TaskName
            // 
            this.TaskName.HeaderText = "業務名";
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            this.TaskName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskName.Width = 420;
            // 
            // Amount
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.Amount.HeaderText = "金額";
            this.Amount.Name = "Amount";
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LeaderMName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.LeaderMName.DefaultCellStyle = dataGridViewCellStyle3;
            this.LeaderMName.HeaderText = "業務担当者";
            this.LeaderMName.Name = "LeaderMName";
            this.LeaderMName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // LeaderMCode
            // 
            this.LeaderMCode.HeaderText = "業務担当者コード";
            this.LeaderMCode.Name = "LeaderMCode";
            this.LeaderMCode.Visible = false;
            // 
            // SlipNo
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SlipNo.DefaultCellStyle = dataGridViewCellStyle4;
            this.SlipNo.HeaderText = "伝票番号";
            this.SlipNo.Name = "SlipNo";
            this.SlipNo.Width = 80;
            // 
            // SalesMCode
            // 
            this.SalesMCode.HeaderText = "営業担当者";
            this.SalesMCode.Name = "SalesMCode";
            this.SalesMCode.Visible = false;
            // 
            // PartnerCode
            // 
            this.PartnerCode.HeaderText = "発注社コード";
            this.PartnerCode.Name = "PartnerCode";
            this.PartnerCode.Visible = false;
            // 
            // PayOffID
            // 
            this.PayOffID.HeaderText = "外注精算データID";
            this.PayOffID.Name = "PayOffID";
            this.PayOffID.Visible = false;
            // 
            // CostReportID
            // 
            this.CostReportID.HeaderText = "原価実績データID";
            this.CostReportID.Name = "CostReportID";
            this.CostReportID.Visible = false;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "削除";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn1.HeaderText = "No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 30;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "業務番号";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "業務名";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 420;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn4.HeaderText = "金額";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn5.HeaderText = "業務担当者";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "業務担当者コード";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn7.HeaderText = "伝票番号";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 80;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "営業担当者";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "発注社コード";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "外注精算データID";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "原価実績データID";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 388);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.IndianRed;
            this.btnEnd.Location = new System.Drawing.Point(204, 388);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(100, 40);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "終了";
            this.btnEnd.UseVisualStyleBackColor = false;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // dgvSetting
            // 
            this.dgvSetting.AllowUserToAddRows = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetting.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSetting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AuthID,
            this.ModuleLabel,
            this.ModuleName});
            this.dgvSetting.Location = new System.Drawing.Point(14, 15);
            this.dgvSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvSetting.MultiSelect = false;
            this.dgvSetting.Name = "dgvSetting";
            this.dgvSetting.RowHeadersVisible = false;
            this.dgvSetting.RowTemplate.Height = 21;
            this.dgvSetting.Size = new System.Drawing.Size(1238, 365);
            this.dgvSetting.TabIndex = 2;
            this.dgvSetting.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSetting_CellClick);
            this.dgvSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSetting_KeyDown);
            // 
            // AuthID
            // 
            this.AuthID.Frozen = true;
            this.AuthID.HeaderText = "ID";
            this.AuthID.Name = "AuthID";
            this.AuthID.Visible = false;
            // 
            // ModuleLabel
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ModuleLabel.DefaultCellStyle = dataGridViewCellStyle10;
            this.ModuleLabel.Frozen = true;
            this.ModuleLabel.HeaderText = "ラベル";
            this.ModuleLabel.Name = "ModuleLabel";
            this.ModuleLabel.Width = 200;
            // 
            // ModuleName
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ModuleName.DefaultCellStyle = dataGridViewCellStyle11;
            this.ModuleName.Frozen = true;
            this.ModuleName.HeaderText = "プログラム";
            this.ModuleName.Name = "ModuleName";
            // 
            // FormImpAuthority
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 441);
            this.Controls.Add(this.dgvSetting);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormImpAuthority";
            this.Text = "権限設定";
            this.Load += new System.EventHandler(this.FormImpAuthority_Load);
            this.Shown += new System.EventHandler(this.FormImpAuthority_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlipNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalesMCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PayOffID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostReportID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEnd;
        private DataGridViewPlus.DataGridViewPlus dgvSetting;
        private System.Windows.Forms.DataGridViewTextBoxColumn AuthID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
    }
}
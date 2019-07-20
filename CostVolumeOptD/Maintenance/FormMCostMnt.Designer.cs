namespace Maintenance
{
    partial class FormMCostMnt
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOfficeCode = new System.Windows.Forms.ComboBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelOfficeCode = new System.Windows.Forms.Label();
            this.dataGridViewList = new DataGridViewPlus.DataGridViewPlus();
            this.CostID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostCodeH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MemberCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "部署：";
            // 
            // comboBoxOfficeCode
            // 
            this.comboBoxOfficeCode.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxOfficeCode.FormattingEnabled = true;
            this.comboBoxOfficeCode.Location = new System.Drawing.Point(62, 13);
            this.comboBoxOfficeCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxOfficeCode.Name = "comboBoxOfficeCode";
            this.comboBoxOfficeCode.Size = new System.Drawing.Size(81, 26);
            this.comboBoxOfficeCode.TabIndex = 1;
            this.comboBoxOfficeCode.SelectedIndexChanged += new System.EventHandler(this.comboBoxOfficeCode_SelectedIndexChanged);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonEnd.Location = new System.Drawing.Point(555, 662);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 40);
            this.buttonEnd.TabIndex = 11;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonSave.Location = new System.Drawing.Point(12, 662);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 40);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "確定";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button_Click);
            // 
            // labelOfficeCode
            // 
            this.labelOfficeCode.AutoSize = true;
            this.labelOfficeCode.Location = new System.Drawing.Point(149, 19);
            this.labelOfficeCode.Name = "labelOfficeCode";
            this.labelOfficeCode.Size = new System.Drawing.Size(56, 12);
            this.labelOfficeCode.TabIndex = 7;
            this.labelOfficeCode.Text = "部署コード";
            this.labelOfficeCode.Visible = false;
            // 
            // dataGridViewList
            // 
            this.dataGridViewList.AllowUserToAddRows = false;
            this.dataGridViewList.AllowUserToDeleteRows = false;
            this.dataGridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CostID,
            this.CostCodeH,
            this.CostCode,
            this.Items,
            this.ItemDetail,
            this.Unit,
            this.Cost,
            this.MemberCode,
            this.UpdateDate,
            this.UpdateFlag,
            this.ErrorMsg});
            this.dataGridViewList.Location = new System.Drawing.Point(12, 51);
            this.dataGridViewList.Name = "dataGridViewList";
            this.dataGridViewList.RowHeadersVisible = false;
            this.dataGridViewList.RowTemplate.Height = 21;
            this.dataGridViewList.Size = new System.Drawing.Size(643, 604);
            this.dataGridViewList.TabIndex = 2;
            this.dataGridViewList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewList_CellEndEdit);
            this.dataGridViewList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewList_RowEnter);
            this.dataGridViewList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewList_KeyDown);
            // 
            // CostID
            // 
            this.CostID.Frozen = true;
            this.CostID.HeaderText = "原価ID";
            this.CostID.Name = "CostID";
            this.CostID.Visible = false;
            // 
            // CostCodeH
            // 
            this.CostCodeH.Frozen = true;
            this.CostCodeH.HeaderText = "原価コードヘッダー";
            this.CostCodeH.Name = "CostCodeH";
            this.CostCodeH.Visible = false;
            // 
            // CostCode
            // 
            this.CostCode.Frozen = true;
            this.CostCode.HeaderText = "原価コード";
            this.CostCode.Name = "CostCode";
            // 
            // Items
            // 
            this.Items.Frozen = true;
            this.Items.HeaderText = "原価名称";
            this.Items.Name = "Items";
            this.Items.Width = 250;
            // 
            // ItemDetail
            // 
            this.ItemDetail.Frozen = true;
            this.ItemDetail.HeaderText = "細別";
            this.ItemDetail.Name = "ItemDetail";
            // 
            // Unit
            // 
            this.Unit.Frozen = true;
            this.Unit.HeaderText = "単位";
            this.Unit.Name = "Unit";
            this.Unit.Width = 70;
            // 
            // Cost
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Cost.DefaultCellStyle = dataGridViewCellStyle1;
            this.Cost.Frozen = true;
            this.Cost.HeaderText = "原価";
            this.Cost.Name = "Cost";
            // 
            // MemberCode
            // 
            this.MemberCode.Frozen = true;
            this.MemberCode.HeaderText = "社員番号";
            this.MemberCode.Name = "MemberCode";
            this.MemberCode.Visible = false;
            // 
            // UpdateDate
            // 
            this.UpdateDate.Frozen = true;
            this.UpdateDate.HeaderText = "更新日";
            this.UpdateDate.Name = "UpdateDate";
            this.UpdateDate.Visible = false;
            // 
            // UpdateFlag
            // 
            this.UpdateFlag.HeaderText = "更新フラグ";
            this.UpdateFlag.Name = "UpdateFlag";
            this.UpdateFlag.Visible = false;
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.HeaderText = "エラーメッセージ";
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.Visible = false;
            // 
            // FormMCostMnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 715);
            this.Controls.Add(this.labelOfficeCode);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOfficeCode);
            this.Controls.Add(this.dataGridViewList);
            this.Name = "FormMCostMnt";
            this.Text = "原価マスタ変更";
            this.Load += new System.EventHandler(this.FormMCostMentL_Load);
            this.Shown += new System.EventHandler(this.FormMCostMentL_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridViewPlus.DataGridViewPlus dataGridViewList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOfficeCode;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelOfficeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostCodeH;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Items;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn MemberCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorMsg;
    }
}
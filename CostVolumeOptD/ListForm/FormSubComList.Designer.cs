namespace ListForm
{
    partial class FormSubComList
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AccountCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerNeme = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FaxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AccountCode,
            this.PartnerNeme,
            this.PostCode,
            this.Address,
            this.TelNo,
            this.FaxNo,
            this.PartnerCode,
            this.PartnerID});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1060, 673);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // AccountCode
            // 
            this.AccountCode.HeaderText = "コード";
            this.AccountCode.Name = "AccountCode";
            this.AccountCode.ReadOnly = true;
            this.AccountCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AccountCode.Width = 50;
            // 
            // PartnerNeme
            // 
            this.PartnerNeme.HeaderText = "取引先名";
            this.PartnerNeme.Name = "PartnerNeme";
            this.PartnerNeme.Width = 250;
            // 
            // PostCode
            // 
            this.PostCode.HeaderText = "〒";
            this.PostCode.Name = "PostCode";
            // 
            // Address
            // 
            this.Address.HeaderText = "住所";
            this.Address.Name = "Address";
            this.Address.Width = 400;
            // 
            // TelNo
            // 
            this.TelNo.HeaderText = "電話";
            this.TelNo.Name = "TelNo";
            this.TelNo.Width = 120;
            // 
            // FaxNo
            // 
            this.FaxNo.HeaderText = "Fax";
            this.FaxNo.Name = "FaxNo";
            this.FaxNo.Width = 120;
            // 
            // PartnerCode
            // 
            this.PartnerCode.HeaderText = "PartnerCode";
            this.PartnerCode.Name = "PartnerCode";
            this.PartnerCode.Visible = false;
            // 
            // PartnerID
            // 
            this.PartnerID.HeaderText = "PartnerID";
            this.PartnerID.Name = "PartnerID";
            this.PartnerID.Visible = false;
            // 
            // FormSubComList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 673);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSubComList";
            this.Text = "協力会社選択リスト";
            this.Load += new System.EventHandler(this.FormSubComList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerNeme;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn TelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FaxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerID;
    }
}


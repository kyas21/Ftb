namespace Menu
{
    partial class FormMenuEstPlan
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
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCostType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelPartner = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelTaskPlace = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTaskCode = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEstimate = new System.Windows.Forms.Button();
            this.buttonPlanning = new System.Windows.Forms.Button();
            this.buttonOsDetail = new System.Windows.Forms.Button();
            this.buttonVolumeInvoice = new System.Windows.Forms.Button();
            this.buttonInvoice = new System.Windows.Forms.Button();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.comboBoxDepartment = new System.Windows.Forms.ComboBox();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.labelContractDate = new System.Windows.Forms.Label();
            this.labelSalesMName = new System.Windows.Forms.Label();
            this.labelLeaderName = new System.Windows.Forms.Label();
            this.buttonDetail = new System.Windows.Forms.Button();
            this.buttonContract = new System.Windows.Forms.Button();
            this.buttonRegular = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonStoreMWorkItems = new System.Windows.Forms.Button();
            this.buttonTask = new System.Windows.Forms.Button();
            this.labelTaskName = new System.Windows.Forms.Label();
            this.textBoxTaskName = new System.Windows.Forms.TextBox();
            this.labelTaskEntryID = new System.Windows.Forms.Label();
            this.labelTtlLeader = new System.Windows.Forms.Label();
            this.labelTtlSales = new System.Windows.Forms.Label();
            this.labelTtlTName = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelPartnerCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "見積・計画 業務名：";
            // 
            // comboBoxCostType
            // 
            this.comboBoxCostType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCostType.FormattingEnabled = true;
            this.comboBoxCostType.Location = new System.Drawing.Point(491, 86);
            this.comboBoxCostType.Name = "comboBoxCostType";
            this.comboBoxCostType.Size = new System.Drawing.Size(80, 23);
            this.comboBoxCostType.TabIndex = 3;
            this.comboBoxCostType.Visible = false;
            this.comboBoxCostType.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "発注者：";
            // 
            // labelPartner
            // 
            this.labelPartner.AutoSize = true;
            this.labelPartner.Location = new System.Drawing.Point(94, 86);
            this.labelPartner.Name = "labelPartner";
            this.labelPartner.Size = new System.Drawing.Size(43, 15);
            this.labelPartner.TabIndex = 6;
            this.labelPartner.Text = "注文主";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "業務場所：";
            // 
            // labelTaskPlace
            // 
            this.labelTaskPlace.AutoSize = true;
            this.labelTaskPlace.Location = new System.Drawing.Point(94, 111);
            this.labelTaskPlace.Name = "labelTaskPlace";
            this.labelTaskPlace.Size = new System.Drawing.Size(79, 15);
            this.labelTaskPlace.TabIndex = 8;
            this.labelTaskPlace.Text = "作業実施場所";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "業務番号：";
            // 
            // labelTaskCode
            // 
            this.labelTaskCode.AutoSize = true;
            this.labelTaskCode.Location = new System.Drawing.Point(94, 136);
            this.labelTaskCode.Name = "labelTaskCode";
            this.labelTaskCode.Size = new System.Drawing.Size(116, 15);
            this.labelTaskCode.TabIndex = 10;
            this.labelTaskCode.Text = "採番されているときのみ";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(11, 221);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(100, 40);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "見積・計画業務登録";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEstimate
            // 
            this.buttonEstimate.BackColor = System.Drawing.Color.LemonChiffon;
            this.buttonEstimate.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonEstimate.Location = new System.Drawing.Point(11, 291);
            this.buttonEstimate.Name = "buttonEstimate";
            this.buttonEstimate.Size = new System.Drawing.Size(120, 120);
            this.buttonEstimate.TabIndex = 7;
            this.buttonEstimate.Text = "見積書";
            this.buttonEstimate.UseVisualStyleBackColor = false;
            this.buttonEstimate.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPlanning
            // 
            this.buttonPlanning.BackColor = System.Drawing.Color.LemonChiffon;
            this.buttonPlanning.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPlanning.Location = new System.Drawing.Point(141, 291);
            this.buttonPlanning.Name = "buttonPlanning";
            this.buttonPlanning.Size = new System.Drawing.Size(120, 120);
            this.buttonPlanning.TabIndex = 8;
            this.buttonPlanning.Text = "予算書";
            this.buttonPlanning.UseVisualStyleBackColor = false;
            this.buttonPlanning.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOsDetail
            // 
            this.buttonOsDetail.BackColor = System.Drawing.Color.LemonChiffon;
            this.buttonOsDetail.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonOsDetail.Location = new System.Drawing.Point(271, 291);
            this.buttonOsDetail.Name = "buttonOsDetail";
            this.buttonOsDetail.Size = new System.Drawing.Size(120, 120);
            this.buttonOsDetail.TabIndex = 9;
            this.buttonOsDetail.Text = "外注内訳書";
            this.buttonOsDetail.UseVisualStyleBackColor = false;
            this.buttonOsDetail.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonVolumeInvoice
            // 
            this.buttonVolumeInvoice.BackColor = System.Drawing.Color.Gold;
            this.buttonVolumeInvoice.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonVolumeInvoice.Location = new System.Drawing.Point(271, 421);
            this.buttonVolumeInvoice.Name = "buttonVolumeInvoice";
            this.buttonVolumeInvoice.Size = new System.Drawing.Size(120, 120);
            this.buttonVolumeInvoice.TabIndex = 12;
            this.buttonVolumeInvoice.Text = "出来高請求書";
            this.buttonVolumeInvoice.UseVisualStyleBackColor = false;
            this.buttonVolumeInvoice.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonInvoice
            // 
            this.buttonInvoice.BackColor = System.Drawing.Color.Gold;
            this.buttonInvoice.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonInvoice.Location = new System.Drawing.Point(401, 421);
            this.buttonInvoice.Name = "buttonInvoice";
            this.buttonInvoice.Size = new System.Drawing.Size(120, 120);
            this.buttonInvoice.TabIndex = 13;
            this.buttonInvoice.Text = "請求書";
            this.buttonInvoice.UseVisualStyleBackColor = false;
            this.buttonInvoice.Click += new System.EventHandler(this.button_Click);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Items.AddRange(new object[] {
            "H",
            "K",
            "S",
            "T"});
            this.comboBoxOffice.Location = new System.Drawing.Point(72, 18);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(80, 23);
            this.comboBoxOffice.TabIndex = 1;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxDepartment
            // 
            this.comboBoxDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepartment.FormattingEnabled = true;
            this.comboBoxDepartment.Location = new System.Drawing.Point(158, 18);
            this.comboBoxDepartment.Name = "comboBoxDepartment";
            this.comboBoxDepartment.Size = new System.Drawing.Size(80, 23);
            this.comboBoxDepartment.TabIndex = 2;
            this.comboBoxDepartment.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(407, 186);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(111, 15);
            this.labelEndDate.TabIndex = 30;
            this.labelEndDate.Text = "～   YYYY/MM/DD";
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(258, 186);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(143, 15);
            this.labelStartDate.TabIndex = 29;
            this.labelStartDate.Text = "工期：     YYYY/MM/DD";
            // 
            // labelContractDate
            // 
            this.labelContractDate.AutoSize = true;
            this.labelContractDate.Location = new System.Drawing.Point(94, 186);
            this.labelContractDate.Name = "labelContractDate";
            this.labelContractDate.Size = new System.Drawing.Size(143, 15);
            this.labelContractDate.TabIndex = 28;
            this.labelContractDate.Text = "契約日：  YYYY/MM/DD";
            // 
            // labelSalesMName
            // 
            this.labelSalesMName.AutoSize = true;
            this.labelSalesMName.Location = new System.Drawing.Point(486, 136);
            this.labelSalesMName.Name = "labelSalesMName";
            this.labelSalesMName.Size = new System.Drawing.Size(31, 15);
            this.labelSalesMName.TabIndex = 25;
            this.labelSalesMName.Text = "氏名";
            // 
            // labelLeaderName
            // 
            this.labelLeaderName.AutoSize = true;
            this.labelLeaderName.Location = new System.Drawing.Point(316, 136);
            this.labelLeaderName.Name = "labelLeaderName";
            this.labelLeaderName.Size = new System.Drawing.Size(31, 15);
            this.labelLeaderName.TabIndex = 24;
            this.labelLeaderName.Text = "氏名";
            // 
            // buttonDetail
            // 
            this.buttonDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.buttonDetail.Location = new System.Drawing.Point(471, 221);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Size = new System.Drawing.Size(100, 40);
            this.buttonDetail.TabIndex = 5;
            this.buttonDetail.Text = "詳細確認";
            this.buttonDetail.UseVisualStyleBackColor = false;
            this.buttonDetail.Visible = false;
            this.buttonDetail.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonContract
            // 
            this.buttonContract.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonContract.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonContract.Location = new System.Drawing.Point(11, 421);
            this.buttonContract.Name = "buttonContract";
            this.buttonContract.Size = new System.Drawing.Size(120, 120);
            this.buttonContract.TabIndex = 10;
            this.buttonContract.Text = "外注精算書\r\n（請負）";
            this.buttonContract.UseVisualStyleBackColor = false;
            this.buttonContract.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonRegular
            // 
            this.buttonRegular.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonRegular.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonRegular.Location = new System.Drawing.Point(141, 421);
            this.buttonRegular.Name = "buttonRegular";
            this.buttonRegular.Size = new System.Drawing.Size(120, 120);
            this.buttonRegular.TabIndex = 11;
            this.buttonRegular.Text = "外注精算書\r\n（常傭）";
            this.buttonRegular.UseVisualStyleBackColor = false;
            this.buttonRegular.Click += new System.EventHandler(this.button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 34;
            this.label5.Text = "事業所：";
            // 
            // buttonStoreMWorkItems
            // 
            this.buttonStoreMWorkItems.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMWorkItems.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMWorkItems.Location = new System.Drawing.Point(451, 291);
            this.buttonStoreMWorkItems.Name = "buttonStoreMWorkItems";
            this.buttonStoreMWorkItems.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMWorkItems.TabIndex = 35;
            this.buttonStoreMWorkItems.Text = "作業項目マスタ登録\r\n\r\n(見積予算\r\n商品マスタ）";
            this.buttonStoreMWorkItems.UseVisualStyleBackColor = false;
            this.buttonStoreMWorkItems.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTask
            // 
            this.buttonTask.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonTask.Location = new System.Drawing.Point(141, 221);
            this.buttonTask.Name = "buttonTask";
            this.buttonTask.Size = new System.Drawing.Size(100, 40);
            this.buttonTask.TabIndex = 36;
            this.buttonTask.Text = "本業務切替";
            this.buttonTask.UseVisualStyleBackColor = false;
            this.buttonTask.Click += new System.EventHandler(this.button_Click);
            // 
            // labelTaskName
            // 
            this.labelTaskName.AutoSize = true;
            this.labelTaskName.Location = new System.Drawing.Point(95, 161);
            this.labelTaskName.Name = "labelTaskName";
            this.labelTaskName.Size = new System.Drawing.Size(42, 15);
            this.labelTaskName.TabIndex = 38;
            this.labelTaskName.Text = "xxxxx";
            // 
            // textBoxTaskName
            // 
            this.textBoxTaskName.Location = new System.Drawing.Point(130, 53);
            this.textBoxTaskName.Name = "textBoxTaskName";
            this.textBoxTaskName.Size = new System.Drawing.Size(460, 23);
            this.textBoxTaskName.TabIndex = 39;
            this.textBoxTaskName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // labelTaskEntryID
            // 
            this.labelTaskEntryID.AutoSize = true;
            this.labelTaskEntryID.Location = new System.Drawing.Point(268, 7);
            this.labelTaskEntryID.Name = "labelTaskEntryID";
            this.labelTaskEntryID.Size = new System.Drawing.Size(135, 15);
            this.labelTaskEntryID.TabIndex = 40;
            this.labelTaskEntryID.Text = "TaskEntryID(invisible)";
            this.labelTaskEntryID.Visible = false;
            // 
            // labelTtlLeader
            // 
            this.labelTtlLeader.AutoSize = true;
            this.labelTtlLeader.Location = new System.Drawing.Point(236, 136);
            this.labelTtlLeader.Name = "labelTtlLeader";
            this.labelTtlLeader.Size = new System.Drawing.Size(79, 15);
            this.labelTtlLeader.TabIndex = 41;
            this.labelTtlLeader.Text = "業務担当者：";
            this.labelTtlLeader.Visible = false;
            // 
            // labelTtlSales
            // 
            this.labelTtlSales.AutoSize = true;
            this.labelTtlSales.Location = new System.Drawing.Point(406, 136);
            this.labelTtlSales.Name = "labelTtlSales";
            this.labelTtlSales.Size = new System.Drawing.Size(79, 15);
            this.labelTtlSales.TabIndex = 42;
            this.labelTtlSales.Text = "営業担当者：";
            this.labelTtlSales.Visible = false;
            // 
            // labelTtlTName
            // 
            this.labelTtlTName.AutoSize = true;
            this.labelTtlTName.Location = new System.Drawing.Point(33, 161);
            this.labelTtlTName.Name = "labelTtlTName";
            this.labelTtlTName.Size = new System.Drawing.Size(55, 15);
            this.labelTtlTName.TabIndex = 43;
            this.labelTtlTName.Text = "業務名：";
            this.labelTtlTName.Visible = false;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(505, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 44;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Visible = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // labelPartnerCode
            // 
            this.labelPartnerCode.AutoSize = true;
            this.labelPartnerCode.Location = new System.Drawing.Point(268, 26);
            this.labelPartnerCode.Name = "labelPartnerCode";
            this.labelPartnerCode.Size = new System.Drawing.Size(135, 15);
            this.labelPartnerCode.TabIndex = 45;
            this.labelPartnerCode.Text = "PartnerCode(invisible)";
            this.labelPartnerCode.Visible = false;
            // 
            // FormMenuEstPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(604, 551);
            this.Controls.Add(this.labelPartnerCode);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelTtlTName);
            this.Controls.Add(this.labelTtlSales);
            this.Controls.Add(this.labelTtlLeader);
            this.Controls.Add(this.labelTaskEntryID);
            this.Controls.Add(this.textBoxTaskName);
            this.Controls.Add(this.labelTaskName);
            this.Controls.Add(this.buttonTask);
            this.Controls.Add(this.buttonStoreMWorkItems);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonRegular);
            this.Controls.Add(this.buttonContract);
            this.Controls.Add(this.buttonDetail);
            this.Controls.Add(this.labelEndDate);
            this.Controls.Add(this.labelStartDate);
            this.Controls.Add(this.labelContractDate);
            this.Controls.Add(this.labelSalesMName);
            this.Controls.Add(this.labelLeaderName);
            this.Controls.Add(this.comboBoxDepartment);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.buttonInvoice);
            this.Controls.Add(this.buttonVolumeInvoice);
            this.Controls.Add(this.buttonOsDetail);
            this.Controls.Add(this.buttonPlanning);
            this.Controls.Add(this.buttonEstimate);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.labelTaskCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelTaskPlace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelPartner);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxCostType);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMenuEstPlan";
            this.Text = "出来高管理システム 見積・予算メニュー";
            this.Load += new System.EventHandler(this.FormMenuEstPlan_Load);
            this.Shown += new System.EventHandler(this.FormMenuEstPlan_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCostType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelPartner;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelTaskPlace;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTaskCode;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEstimate;
        private System.Windows.Forms.Button buttonPlanning;
        private System.Windows.Forms.Button buttonOsDetail;
        private System.Windows.Forms.Button buttonVolumeInvoice;
        private System.Windows.Forms.Button buttonInvoice;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.ComboBox comboBoxDepartment;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Label labelContractDate;
        private System.Windows.Forms.Label labelSalesMName;
        private System.Windows.Forms.Label labelLeaderName;
        private System.Windows.Forms.Button buttonDetail;
        private System.Windows.Forms.Button buttonContract;
        private System.Windows.Forms.Button buttonRegular;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonStoreMWorkItems;
        private System.Windows.Forms.Button buttonTask;
        private System.Windows.Forms.Label labelTaskName;
        private System.Windows.Forms.TextBox textBoxTaskName;
        private System.Windows.Forms.Label labelTaskEntryID;
        private System.Windows.Forms.Label labelTtlLeader;
        private System.Windows.Forms.Label labelTtlSales;
        private System.Windows.Forms.Label labelTtlTName;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelPartnerCode;
    }
}


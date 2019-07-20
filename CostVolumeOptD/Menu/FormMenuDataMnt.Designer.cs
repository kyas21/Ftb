namespace Menu
{
    partial class FormMenuDataMnt
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
            this.buttonExpCostData = new System.Windows.Forms.Button();
            this.buttonMntDBBackup = new System.Windows.Forms.Button();
            this.buttonClosing = new System.Windows.Forms.Button();
            this.buttonMntGenData = new System.Windows.Forms.Button();
            this.buttonStoreMWorkItems = new System.Windows.Forms.Button();
            this.buttonStoreMOffice = new System.Windows.Forms.Button();
            this.buttonStoreMCalendar = new System.Windows.Forms.Button();
            this.buttonStoreMCost = new System.Windows.Forms.Button();
            this.buttonStoreMCommon = new System.Windows.Forms.Button();
            this.buttonStoreMMembers = new System.Windows.Forms.Button();
            this.buttonStoreMPartners = new System.Windows.Forms.Button();
            this.buttonStoreTaskData = new System.Windows.Forms.Button();
            this.buttonAuth = new System.Windows.Forms.Button();
            this.buttonMntVolume = new System.Windows.Forms.Button();
            this.buttonTaskChange = new System.Windows.Forms.Button();
            this.buttonMCostMnt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonExpCostData
            // 
            this.buttonExpCostData.BackColor = System.Drawing.Color.Orange;
            this.buttonExpCostData.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExpCostData.Location = new System.Drawing.Point(11, 11);
            this.buttonExpCostData.Name = "buttonExpCostData";
            this.buttonExpCostData.Size = new System.Drawing.Size(120, 120);
            this.buttonExpCostData.TabIndex = 1;
            this.buttonExpCostData.Text = "作業内訳\r\n汎用データ\r\n作成";
            this.buttonExpCostData.UseVisualStyleBackColor = false;
            this.buttonExpCostData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMntDBBackup
            // 
            this.buttonMntDBBackup.BackColor = System.Drawing.Color.SkyBlue;
            this.buttonMntDBBackup.Location = new System.Drawing.Point(271, 11);
            this.buttonMntDBBackup.Name = "buttonMntDBBackup";
            this.buttonMntDBBackup.Size = new System.Drawing.Size(120, 120);
            this.buttonMntDBBackup.TabIndex = 4;
            this.buttonMntDBBackup.Text = "DBバックアップ";
            this.buttonMntDBBackup.UseVisualStyleBackColor = false;
            this.buttonMntDBBackup.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClosing
            // 
            this.buttonClosing.BackColor = System.Drawing.Color.YellowGreen;
            this.buttonClosing.Location = new System.Drawing.Point(141, 11);
            this.buttonClosing.Name = "buttonClosing";
            this.buttonClosing.Size = new System.Drawing.Size(120, 120);
            this.buttonClosing.TabIndex = 11;
            this.buttonClosing.Text = "締め処理";
            this.buttonClosing.UseVisualStyleBackColor = false;
            this.buttonClosing.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMntGenData
            // 
            this.buttonMntGenData.BackColor = System.Drawing.Color.Coral;
            this.buttonMntGenData.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonMntGenData.Location = new System.Drawing.Point(11, 401);
            this.buttonMntGenData.Name = "buttonMntGenData";
            this.buttonMntGenData.Size = new System.Drawing.Size(120, 120);
            this.buttonMntGenData.TabIndex = 12;
            this.buttonMntGenData.Text = "汎用データ\r\n取込処理";
            this.buttonMntGenData.UseVisualStyleBackColor = false;
            this.buttonMntGenData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMWorkItems
            // 
            this.buttonStoreMWorkItems.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMWorkItems.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMWorkItems.Location = new System.Drawing.Point(271, 271);
            this.buttonStoreMWorkItems.Name = "buttonStoreMWorkItems";
            this.buttonStoreMWorkItems.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMWorkItems.TabIndex = 6;
            this.buttonStoreMWorkItems.Text = "作業項目マスタ登録\r\n\r\n(見積予算\r\n商品マスタ）";
            this.buttonStoreMWorkItems.UseVisualStyleBackColor = false;
            this.buttonStoreMWorkItems.Visible = false;
            this.buttonStoreMWorkItems.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMOffice
            // 
            this.buttonStoreMOffice.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMOffice.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMOffice.Location = new System.Drawing.Point(11, 141);
            this.buttonStoreMOffice.Name = "buttonStoreMOffice";
            this.buttonStoreMOffice.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMOffice.TabIndex = 2;
            this.buttonStoreMOffice.Text = "事業所マスタ\r\nメンテナンス";
            this.buttonStoreMOffice.UseVisualStyleBackColor = false;
            this.buttonStoreMOffice.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMCalendar
            // 
            this.buttonStoreMCalendar.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMCalendar.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMCalendar.Location = new System.Drawing.Point(11, 271);
            this.buttonStoreMCalendar.Name = "buttonStoreMCalendar";
            this.buttonStoreMCalendar.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMCalendar.TabIndex = 7;
            this.buttonStoreMCalendar.Text = "カレンダマスタ\r\nメンテナンス";
            this.buttonStoreMCalendar.UseVisualStyleBackColor = false;
            this.buttonStoreMCalendar.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMCost
            // 
            this.buttonStoreMCost.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMCost.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMCost.Location = new System.Drawing.Point(401, 141);
            this.buttonStoreMCost.Name = "buttonStoreMCost";
            this.buttonStoreMCost.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMCost.TabIndex = 3;
            this.buttonStoreMCost.Text = "原価マスタ\r\nメンテナンス";
            this.buttonStoreMCost.UseVisualStyleBackColor = false;
            this.buttonStoreMCost.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMCommon
            // 
            this.buttonStoreMCommon.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMCommon.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMCommon.Location = new System.Drawing.Point(141, 271);
            this.buttonStoreMCommon.Name = "buttonStoreMCommon";
            this.buttonStoreMCommon.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMCommon.TabIndex = 8;
            this.buttonStoreMCommon.Text = "共通マスタ\r\nメンテナンス";
            this.buttonStoreMCommon.UseVisualStyleBackColor = false;
            this.buttonStoreMCommon.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMMembers
            // 
            this.buttonStoreMMembers.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMMembers.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMMembers.Location = new System.Drawing.Point(141, 141);
            this.buttonStoreMMembers.Name = "buttonStoreMMembers";
            this.buttonStoreMMembers.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMMembers.TabIndex = 3;
            this.buttonStoreMMembers.Text = "社員マスタ\r\nメンテナンス";
            this.buttonStoreMMembers.UseVisualStyleBackColor = false;
            this.buttonStoreMMembers.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreMPartners
            // 
            this.buttonStoreMPartners.BackColor = System.Drawing.Color.Khaki;
            this.buttonStoreMPartners.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreMPartners.Location = new System.Drawing.Point(271, 141);
            this.buttonStoreMPartners.Name = "buttonStoreMPartners";
            this.buttonStoreMPartners.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreMPartners.TabIndex = 4;
            this.buttonStoreMPartners.Text = "取引先マスタ\r\nメンテナンス";
            this.buttonStoreMPartners.UseVisualStyleBackColor = false;
            this.buttonStoreMPartners.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStoreTaskData
            // 
            this.buttonStoreTaskData.BackColor = System.Drawing.Color.Goldenrod;
            this.buttonStoreTaskData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonStoreTaskData.Location = new System.Drawing.Point(401, 271);
            this.buttonStoreTaskData.Name = "buttonStoreTaskData";
            this.buttonStoreTaskData.Size = new System.Drawing.Size(120, 120);
            this.buttonStoreTaskData.TabIndex = 2;
            this.buttonStoreTaskData.Text = "業務データ登録\r\n（最小限）";
            this.buttonStoreTaskData.UseVisualStyleBackColor = false;
            this.buttonStoreTaskData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonAuth
            // 
            this.buttonAuth.BackColor = System.Drawing.Color.Sienna;
            this.buttonAuth.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonAuth.Location = new System.Drawing.Point(271, 401);
            this.buttonAuth.Name = "buttonAuth";
            this.buttonAuth.Size = new System.Drawing.Size(120, 120);
            this.buttonAuth.TabIndex = 13;
            this.buttonAuth.Text = "アクセス権限設定";
            this.buttonAuth.UseVisualStyleBackColor = false;
            this.buttonAuth.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMntVolume
            // 
            this.buttonMntVolume.BackColor = System.Drawing.Color.LightCoral;
            this.buttonMntVolume.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonMntVolume.Location = new System.Drawing.Point(141, 401);
            this.buttonMntVolume.Name = "buttonMntVolume";
            this.buttonMntVolume.Size = new System.Drawing.Size(120, 120);
            this.buttonMntVolume.TabIndex = 14;
            this.buttonMntVolume.Text = "出来高データ\r\n取込処理";
            this.buttonMntVolume.UseVisualStyleBackColor = false;
            this.buttonMntVolume.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTaskChange
            // 
            this.buttonTaskChange.BackColor = System.Drawing.Color.DarkKhaki;
            this.buttonTaskChange.Location = new System.Drawing.Point(402, 11);
            this.buttonTaskChange.Name = "buttonTaskChange";
            this.buttonTaskChange.Size = new System.Drawing.Size(120, 120);
            this.buttonTaskChange.TabIndex = 15;
            this.buttonTaskChange.Text = "業務番号振替";
            this.buttonTaskChange.UseVisualStyleBackColor = false;
            this.buttonTaskChange.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMCostMnt
            // 
            this.buttonMCostMnt.BackColor = System.Drawing.Color.Khaki;
            this.buttonMCostMnt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonMCostMnt.Location = new System.Drawing.Point(401, 402);
            this.buttonMCostMnt.Name = "buttonMCostMnt";
            this.buttonMCostMnt.Size = new System.Drawing.Size(120, 120);
            this.buttonMCostMnt.TabIndex = 16;
            this.buttonMCostMnt.Text = "原価マスタ\r\nメンテナンス\r\n（画面編集）";
            this.buttonMCostMnt.UseVisualStyleBackColor = false;
            this.buttonMCostMnt.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuDataMnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 531);
            this.Controls.Add(this.buttonMCostMnt);
            this.Controls.Add(this.buttonTaskChange);
            this.Controls.Add(this.buttonMntVolume);
            this.Controls.Add(this.buttonAuth);
            this.Controls.Add(this.buttonStoreTaskData);
            this.Controls.Add(this.buttonStoreMPartners);
            this.Controls.Add(this.buttonStoreMMembers);
            this.Controls.Add(this.buttonStoreMCommon);
            this.Controls.Add(this.buttonStoreMCost);
            this.Controls.Add(this.buttonStoreMCalendar);
            this.Controls.Add(this.buttonStoreMOffice);
            this.Controls.Add(this.buttonStoreMWorkItems);
            this.Controls.Add(this.buttonMntGenData);
            this.Controls.Add(this.buttonClosing);
            this.Controls.Add(this.buttonMntDBBackup);
            this.Controls.Add(this.buttonExpCostData);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMenuDataMnt";
            this.Text = "出来高管理システム　データ管理メニュー";
            this.Load += new System.EventHandler(this.FormMenuDataMnt_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonExpCostData;
        private System.Windows.Forms.Button buttonMntDBBackup;
        private System.Windows.Forms.Button buttonClosing;
        private System.Windows.Forms.Button buttonMntGenData;
        private System.Windows.Forms.Button buttonStoreMWorkItems;
        private System.Windows.Forms.Button buttonStoreMOffice;
        private System.Windows.Forms.Button buttonStoreMCalendar;
        private System.Windows.Forms.Button buttonStoreMCost;
        private System.Windows.Forms.Button buttonStoreMCommon;
        private System.Windows.Forms.Button buttonStoreMMembers;
        private System.Windows.Forms.Button buttonStoreMPartners;
        private System.Windows.Forms.Button buttonStoreTaskData;
        private System.Windows.Forms.Button buttonAuth;
        private System.Windows.Forms.Button buttonMntVolume;
        private System.Windows.Forms.Button buttonTaskChange;
        private System.Windows.Forms.Button buttonMCostMnt;
    }
}
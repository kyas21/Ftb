namespace Menu
{
    partial class FormMenuA
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
            this.buttonPlan = new System.Windows.Forms.Button();
            this.buttonVol = new System.Windows.Forms.Button();
            this.buttonMnt = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonOs = new System.Windows.Forms.Button();
            this.buttonSyo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPlan
            // 
            this.buttonPlan.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonPlan.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPlan.Location = new System.Drawing.Point(141, 11);
            this.buttonPlan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPlan.Name = "buttonPlan";
            this.buttonPlan.Size = new System.Drawing.Size(120, 120);
            this.buttonPlan.TabIndex = 2;
            this.buttonPlan.Text = "見積・予算メニュー";
            this.buttonPlan.UseVisualStyleBackColor = false;
            this.buttonPlan.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonVol
            // 
            this.buttonVol.BackColor = System.Drawing.Color.LightSkyBlue;
            this.buttonVol.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonVol.Location = new System.Drawing.Point(11, 11);
            this.buttonVol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonVol.Name = "buttonVol";
            this.buttonVol.Size = new System.Drawing.Size(120, 120);
            this.buttonVol.TabIndex = 1;
            this.buttonVol.Text = "原価・出来高\r\nメニュー";
            this.buttonVol.UseVisualStyleBackColor = false;
            this.buttonVol.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMnt
            // 
            this.buttonMnt.BackColor = System.Drawing.Color.Ivory;
            this.buttonMnt.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonMnt.Location = new System.Drawing.Point(271, 11);
            this.buttonMnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonMnt.Name = "buttonMnt";
            this.buttonMnt.Size = new System.Drawing.Size(120, 120);
            this.buttonMnt.TabIndex = 3;
            this.buttonMnt.Text = "データ管理メニュー";
            this.buttonMnt.UseVisualStyleBackColor = false;
            this.buttonMnt.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.BackColor = System.Drawing.Color.LightGray;
            this.buttonInfo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonInfo.Location = new System.Drawing.Point(271, 141);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(120, 120);
            this.buttonInfo.TabIndex = 6;
            this.buttonInfo.Text = "情報表示";
            this.buttonInfo.UseVisualStyleBackColor = false;
            this.buttonInfo.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOs
            // 
            this.buttonOs.BackColor = System.Drawing.Color.Moccasin;
            this.buttonOs.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonOs.Location = new System.Drawing.Point(11, 141);
            this.buttonOs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOs.Name = "buttonOs";
            this.buttonOs.Size = new System.Drawing.Size(120, 120);
            this.buttonOs.TabIndex = 7;
            this.buttonOs.Text = "協力会社関連\r\nメニュー";
            this.buttonOs.UseVisualStyleBackColor = false;
            this.buttonOs.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSyo
            // 
            this.buttonSyo.BackColor = System.Drawing.Color.Tan;
            this.buttonSyo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonSyo.Location = new System.Drawing.Point(141, 141);
            this.buttonSyo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSyo.Name = "buttonSyo";
            this.buttonSyo.Size = new System.Drawing.Size(120, 120);
            this.buttonSyo.TabIndex = 8;
            this.buttonSyo.Text = "商魂データ作成\r\nメニュー";
            this.buttonSyo.UseVisualStyleBackColor = false;
            this.buttonSyo.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 271);
            this.Controls.Add(this.buttonSyo);
            this.Controls.Add(this.buttonOs);
            this.Controls.Add(this.buttonInfo);
            this.Controls.Add(this.buttonMnt);
            this.Controls.Add(this.buttonVol);
            this.Controls.Add(this.buttonPlan);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMenuA";
            this.Text = "出来高管理システム メインメニュー";
            this.Load += new System.EventHandler(this.FormMenuA_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonPlan;
        private System.Windows.Forms.Button buttonVol;
        private System.Windows.Forms.Button buttonMnt;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Button buttonOs;
        private System.Windows.Forms.Button buttonSyo;
    }
}
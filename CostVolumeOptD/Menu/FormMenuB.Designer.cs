namespace Menu
{
    partial class FormMenuB
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
            this.buttonOs = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonVol = new System.Windows.Forms.Button();
            this.buttonPlan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOs
            // 
            this.buttonOs.BackColor = System.Drawing.Color.Moccasin;
            this.buttonOs.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonOs.Location = new System.Drawing.Point(11, 141);
            this.buttonOs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOs.Name = "buttonOs";
            this.buttonOs.Size = new System.Drawing.Size(120, 120);
            this.buttonOs.TabIndex = 13;
            this.buttonOs.Text = "協力会社関連\r\nメニュー";
            this.buttonOs.UseVisualStyleBackColor = false;
            this.buttonOs.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.BackColor = System.Drawing.Color.LightGray;
            this.buttonInfo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonInfo.Location = new System.Drawing.Point(272, 11);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(120, 120);
            this.buttonInfo.TabIndex = 12;
            this.buttonInfo.Text = "情報表示";
            this.buttonInfo.UseVisualStyleBackColor = false;
            this.buttonInfo.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonVol
            // 
            this.buttonVol.BackColor = System.Drawing.Color.LightSkyBlue;
            this.buttonVol.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonVol.Location = new System.Drawing.Point(11, 11);
            this.buttonVol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonVol.Name = "buttonVol";
            this.buttonVol.Size = new System.Drawing.Size(120, 120);
            this.buttonVol.TabIndex = 9;
            this.buttonVol.Text = "原価・出来高\r\nメニュー";
            this.buttonVol.UseVisualStyleBackColor = false;
            this.buttonVol.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPlan
            // 
            this.buttonPlan.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonPlan.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPlan.Location = new System.Drawing.Point(141, 11);
            this.buttonPlan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPlan.Name = "buttonPlan";
            this.buttonPlan.Size = new System.Drawing.Size(120, 120);
            this.buttonPlan.TabIndex = 10;
            this.buttonPlan.Text = "見積・予算メニュー";
            this.buttonPlan.UseVisualStyleBackColor = false;
            this.buttonPlan.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 271);
            this.Controls.Add(this.buttonOs);
            this.Controls.Add(this.buttonInfo);
            this.Controls.Add(this.buttonVol);
            this.Controls.Add(this.buttonPlan);
            this.Name = "FormMenuB";
            this.Text = "出来高管理システムメニュー";
            this.Load += new System.EventHandler(this.FormMenuB_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOs;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Button buttonVol;
        private System.Windows.Forms.Button buttonPlan;
    }
}
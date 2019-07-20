namespace Maintenance
{
    partial class FormClosingProc
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
            this.labelHistory = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.labelNow = new System.Windows.Forms.Label();
            this.labelNowEx = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.labelCaution = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelHistory
            // 
            this.labelHistory.AutoSize = true;
            this.labelHistory.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelHistory.Location = new System.Drawing.Point(31, 81);
            this.labelHistory.Name = "labelHistory";
            this.labelHistory.Size = new System.Drawing.Size(423, 17);
            this.labelHistory.TabIndex = 0;
            this.labelHistory.Text = "XX月まで締め処理が済んでいます。同時に99年度の締め処理も完了しています";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(41, 221);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 40);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "締める";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(171, 221);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(100, 40);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "直近の締めを\r\n解除する";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(321, 221);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 40);
            this.buttonEnd.TabIndex = 3;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // labelNow
            // 
            this.labelNow.AutoSize = true;
            this.labelNow.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNow.Location = new System.Drawing.Point(31, 111);
            this.labelNow.Name = "labelNow";
            this.labelNow.Size = new System.Drawing.Size(166, 17);
            this.labelNow.TabIndex = 4;
            this.labelNow.Text = "XX月が締待ちになっています。";
            // 
            // labelNowEx
            // 
            this.labelNowEx.AutoSize = true;
            this.labelNowEx.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNowEx.Location = new System.Drawing.Point(51, 131);
            this.labelNowEx.Name = "labelNowEx";
            this.labelNowEx.Size = new System.Drawing.Size(291, 17);
            this.labelNowEx.TabIndex = 5;
            this.labelNowEx.Text = "当月の締処理は同時に、XX年度の締処理となります。";
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(31, 176);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(41, 15);
            this.labelMessage.TabIndex = 6;
            this.labelMessage.Text = "label4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "事業所：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(93, 18);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(78, 23);
            this.comboBoxOffice.TabIndex = 8;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBoxOffice_TextChanged);
            // 
            // labelCaution
            // 
            this.labelCaution.AutoSize = true;
            this.labelCaution.Location = new System.Drawing.Point(203, 21);
            this.labelCaution.Name = "labelCaution";
            this.labelCaution.Size = new System.Drawing.Size(31, 15);
            this.labelCaution.TabIndex = 9;
            this.labelCaution.Text = "注意";
            // 
            // FormClosingProc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 281);
            this.Controls.Add(this.labelCaution);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.labelNowEx);
            this.Controls.Add(this.labelNow);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelHistory);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormClosingProc";
            this.Text = "FormClosingProc";
            this.Load += new System.EventHandler(this.FormClosingProc_Load);
            this.Shown += new System.EventHandler(this.FormClosingProc_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHistory;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Label labelNow;
        private System.Windows.Forms.Label labelNowEx;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label labelCaution;
    }
}
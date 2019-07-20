namespace Menu
{
    partial class FormMenuS
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
            this.buttonPlan = new System.Windows.Forms.Button();
            this.buttonTaskNote = new System.Windows.Forms.Button();
            this.buttonTaskSummary = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPlan
            // 
            this.buttonPlan.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonPlan.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPlan.Location = new System.Drawing.Point(11, 139);
            this.buttonPlan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPlan.Name = "buttonPlan";
            this.buttonPlan.Size = new System.Drawing.Size(120, 120);
            this.buttonPlan.TabIndex = 3;
            this.buttonPlan.Text = "見積・予算メニュー";
            this.buttonPlan.UseVisualStyleBackColor = false;
            this.buttonPlan.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTaskNote
            // 
            this.buttonTaskNote.BackColor = System.Drawing.Color.LightSkyBlue;
            this.buttonTaskNote.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonTaskNote.Location = new System.Drawing.Point(11, 11);
            this.buttonTaskNote.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTaskNote.Name = "buttonTaskNote";
            this.buttonTaskNote.Size = new System.Drawing.Size(120, 120);
            this.buttonTaskNote.TabIndex = 4;
            this.buttonTaskNote.Text = "受注業務\r\n引継書\r\n";
            this.buttonTaskNote.UseVisualStyleBackColor = false;
            this.buttonTaskNote.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTaskSummary
            // 
            this.buttonTaskSummary.BackColor = System.Drawing.Color.Azure;
            this.buttonTaskSummary.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonTaskSummary.Location = new System.Drawing.Point(142, 13);
            this.buttonTaskSummary.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTaskSummary.Name = "buttonTaskSummary";
            this.buttonTaskSummary.Size = new System.Drawing.Size(120, 120);
            this.buttonTaskSummary.TabIndex = 8;
            this.buttonTaskSummary.Text = "業務元帳\r\n(得意先元帳）";
            this.buttonTaskSummary.UseVisualStyleBackColor = false;
            this.buttonTaskSummary.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 271);
            this.Controls.Add(this.buttonTaskSummary);
            this.Controls.Add(this.buttonTaskNote);
            this.Controls.Add(this.buttonPlan);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMenuS";
            this.Text = "出来高管理システムメニュー";
            this.Load += new System.EventHandler(this.FormMenuS_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPlan;
        private System.Windows.Forms.Button buttonTaskNote;
        private System.Windows.Forms.Button buttonTaskSummary;
    }
}
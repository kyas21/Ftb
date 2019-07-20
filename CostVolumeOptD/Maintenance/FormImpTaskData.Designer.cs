namespace Maintenance
{
    partial class FormImpTaskData
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.buttonDateSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(431, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(307, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 40);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "開始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(201, 12);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(100, 40);
            this.buttonOpen.TabIndex = 4;
            this.buttonOpen.Text = "ファイル指定";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(592, 12);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 40);
            this.buttonEnd.TabIndex = 50;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.HideSelection = false;
            this.textBoxMsg.Location = new System.Drawing.Point(12, 60);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(680, 309);
            this.textBoxMsg.TabIndex = 51;
            // 
            // buttonDateSet
            // 
            this.buttonDateSet.Location = new System.Drawing.Point(537, 12);
            this.buttonDateSet.Name = "buttonDateSet";
            this.buttonDateSet.Size = new System.Drawing.Size(50, 40);
            this.buttonDateSet.TabIndex = 52;
            this.buttonDateSet.Text = "日付設定";
            this.buttonDateSet.UseVisualStyleBackColor = true;
            this.buttonDateSet.Visible = false;
            this.buttonDateSet.Click += new System.EventHandler(this.button_Click);
            // 
            // FormImpTaskData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 381);
            this.Controls.Add(this.buttonDateSet);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonOpen);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormImpTaskData";
            this.Text = "業務データ更新";
            this.Load += new System.EventHandler(this.FormImpTaskData_Load);
            this.Shown += new System.EventHandler(this.FormImpTaskData_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Button buttonDateSet;
    }
}
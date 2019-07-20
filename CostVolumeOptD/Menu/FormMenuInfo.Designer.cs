namespace Menu
{
    partial class FormMenuInfo
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
            this.buttonCostInfo = new System.Windows.Forms.Button();
            this.buttonTaskList = new System.Windows.Forms.Button();
            this.buttonTaskNoConfList = new System.Windows.Forms.Button();
            this.buttonPlanNoConfList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCostInfo
            // 
            this.buttonCostInfo.BackColor = System.Drawing.Color.Orange;
            this.buttonCostInfo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonCostInfo.Location = new System.Drawing.Point(11, 11);
            this.buttonCostInfo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonCostInfo.Name = "buttonCostInfo";
            this.buttonCostInfo.Size = new System.Drawing.Size(120, 120);
            this.buttonCostInfo.TabIndex = 11;
            this.buttonCostInfo.Text = "作業内訳書\r\n入力状況";
            this.buttonCostInfo.UseVisualStyleBackColor = false;
            this.buttonCostInfo.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTaskList
            // 
            this.buttonTaskList.BackColor = System.Drawing.Color.Salmon;
            this.buttonTaskList.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonTaskList.Location = new System.Drawing.Point(141, 11);
            this.buttonTaskList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTaskList.Name = "buttonTaskList";
            this.buttonTaskList.Size = new System.Drawing.Size(120, 120);
            this.buttonTaskList.TabIndex = 12;
            this.buttonTaskList.Text = "業務一覧表\r\n表示";
            this.buttonTaskList.UseVisualStyleBackColor = false;
            this.buttonTaskList.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTaskNoConfList
            // 
            this.buttonTaskNoConfList.BackColor = System.Drawing.Color.Peru;
            this.buttonTaskNoConfList.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonTaskNoConfList.Location = new System.Drawing.Point(11, 141);
            this.buttonTaskNoConfList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonTaskNoConfList.Name = "buttonTaskNoConfList";
            this.buttonTaskNoConfList.Size = new System.Drawing.Size(120, 120);
            this.buttonTaskNoConfList.TabIndex = 13;
            this.buttonTaskNoConfList.Text = "業務引継書\r\n承認未完了リスト";
            this.buttonTaskNoConfList.UseVisualStyleBackColor = false;
            this.buttonTaskNoConfList.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPlanNoConfList
            // 
            this.buttonPlanNoConfList.BackColor = System.Drawing.Color.Khaki;
            this.buttonPlanNoConfList.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonPlanNoConfList.Location = new System.Drawing.Point(141, 141);
            this.buttonPlanNoConfList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonPlanNoConfList.Name = "buttonPlanNoConfList";
            this.buttonPlanNoConfList.Size = new System.Drawing.Size(120, 120);
            this.buttonPlanNoConfList.TabIndex = 14;
            this.buttonPlanNoConfList.Text = "実行書\r\n承認未完了リスト";
            this.buttonPlanNoConfList.UseVisualStyleBackColor = false;
            this.buttonPlanNoConfList.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 271);
            this.Controls.Add(this.buttonPlanNoConfList);
            this.Controls.Add(this.buttonTaskNoConfList);
            this.Controls.Add(this.buttonTaskList);
            this.Controls.Add(this.buttonCostInfo);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMenuInfo";
            this.Text = "情報表示メニュー";
            this.Load += new System.EventHandler(this.FormMenuInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCostInfo;
        private System.Windows.Forms.Button buttonTaskList;
        private System.Windows.Forms.Button buttonTaskNoConfList;
        private System.Windows.Forms.Button buttonPlanNoConfList;
    }
}
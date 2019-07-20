namespace Menu
{
    partial class FormMenuSYO
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
            this.buttonExportCostData = new System.Windows.Forms.Button();
            this.buttonExportCostMaster = new System.Windows.Forms.Button();
            this.buttonExportTask = new System.Windows.Forms.Button();
            this.buttonExportKMaster = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonExportCostData
            // 
            this.buttonExportCostData.BackColor = System.Drawing.Color.Orange;
            this.buttonExportCostData.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExportCostData.Location = new System.Drawing.Point(11, 11);
            this.buttonExportCostData.Name = "buttonExportCostData";
            this.buttonExportCostData.Size = new System.Drawing.Size(120, 120);
            this.buttonExportCostData.TabIndex = 2;
            this.buttonExportCostData.Text = "作業内訳\r\n汎用データ\r\n作成";
            this.buttonExportCostData.UseVisualStyleBackColor = false;
            this.buttonExportCostData.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonExportCostMaster
            // 
            this.buttonExportCostMaster.BackColor = System.Drawing.Color.DarkKhaki;
            this.buttonExportCostMaster.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExportCostMaster.Location = new System.Drawing.Point(142, 140);
            this.buttonExportCostMaster.Name = "buttonExportCostMaster";
            this.buttonExportCostMaster.Size = new System.Drawing.Size(120, 120);
            this.buttonExportCostMaster.TabIndex = 3;
            this.buttonExportCostMaster.Text = "商品マスタ\r\n";
            this.buttonExportCostMaster.UseVisualStyleBackColor = false;
            this.buttonExportCostMaster.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonExportTask
            // 
            this.buttonExportTask.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.buttonExportTask.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExportTask.Location = new System.Drawing.Point(11, 140);
            this.buttonExportTask.Name = "buttonExportTask";
            this.buttonExportTask.Size = new System.Drawing.Size(120, 120);
            this.buttonExportTask.TabIndex = 4;
            this.buttonExportTask.Text = "得意先マスタ";
            this.buttonExportTask.UseVisualStyleBackColor = false;
            this.buttonExportTask.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonExportKMaster
            // 
            this.buttonExportKMaster.BackColor = System.Drawing.Color.Olive;
            this.buttonExportKMaster.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonExportKMaster.Location = new System.Drawing.Point(272, 140);
            this.buttonExportKMaster.Name = "buttonExportKMaster";
            this.buttonExportKMaster.Size = new System.Drawing.Size(120, 120);
            this.buttonExportKMaster.TabIndex = 5;
            this.buttonExportKMaster.Text = "区分マスタ";
            this.buttonExportKMaster.UseVisualStyleBackColor = false;
            this.buttonExportKMaster.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMenuSYO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 271);
            this.Controls.Add(this.buttonExportKMaster);
            this.Controls.Add(this.buttonExportTask);
            this.Controls.Add(this.buttonExportCostMaster);
            this.Controls.Add(this.buttonExportCostData);
            this.Name = "FormMenuSYO";
            this.Text = "商魂データ作成メニュー";
            this.Load += new System.EventHandler(this.FormMenuSYO_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExportCostData;
        private System.Windows.Forms.Button buttonExportCostMaster;
        private System.Windows.Forms.Button buttonExportTask;
        private System.Windows.Forms.Button buttonExportKMaster;
    }
}
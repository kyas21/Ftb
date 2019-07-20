namespace Export
{
    partial class FormExportKMaster
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
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxKubun = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.HideSelection = false;
            this.textBoxMsg.Location = new System.Drawing.Point(17, 69);
            this.textBoxMsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(674, 307);
            this.textBoxMsg.TabIndex = 75;
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(601, 3);
            this.buttonEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 74;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 73;
            this.label3.Text = "事業所：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(75, 16);
            this.comboBoxOffice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(85, 23);
            this.comboBoxOffice.TabIndex = 72;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(328, 384);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 70;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(136, 384);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 40);
            this.buttonOK.TabIndex = 69;
            this.buttonOK.Text = "開始";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 68;
            this.label2.Text = "状態：";
            // 
            // comboBoxKubun
            // 
            this.comboBoxKubun.FormattingEnabled = true;
            this.comboBoxKubun.Location = new System.Drawing.Point(254, 16);
            this.comboBoxKubun.Name = "comboBoxKubun";
            this.comboBoxKubun.Size = new System.Drawing.Size(121, 23);
            this.comboBoxKubun.TabIndex = 76;
            this.comboBoxKubun.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 77;
            this.label1.Text = "区分名：";
            // 
            // FormExportKMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxKubun);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxOffice);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormExportKMaster";
            this.Text = "FormExportKMaster";
            this.Load += new System.EventHandler(this.FormExportKMaster_Load);
            this.Shown += new System.EventHandler(this.FormExportKMaster_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxKubun;
        private System.Windows.Forms.Label label1;
    }
}
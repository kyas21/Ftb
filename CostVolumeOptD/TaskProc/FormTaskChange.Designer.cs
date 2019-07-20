namespace TaskProc
{
    partial class FormTaskChange
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
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.dateTimePickerFr = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelItem = new System.Windows.Forms.Label();
            this.labelSTaskName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelDTaskName = new System.Windows.Forms.Label();
            this.textBoxItem = new System.Windows.Forms.TextBox();
            this.textBoxSTask = new System.Windows.Forms.TextBox();
            this.textBoxDTask = new System.Windows.Forms.TextBox();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.labelMes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(158, 38);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(73, 23);
            this.comboBoxDepart.TabIndex = 2;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 53;
            this.label3.Text = "部門：";
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(94, 38);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(58, 23);
            this.comboBoxOffice.TabIndex = 1;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(24, 309);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(100, 40);
            this.buttonChange.TabIndex = 21;
            this.buttonChange.Text = "変更";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEnd
            // 
            this.buttonEnd.BackColor = System.Drawing.Color.IndianRed;
            this.buttonEnd.Location = new System.Drawing.Point(681, 6);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(90, 40);
            this.buttonEnd.TabIndex = 22;
            this.buttonEnd.Text = "終了";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button_Click);
            // 
            // dateTimePickerFr
            // 
            this.dateTimePickerFr.Location = new System.Drawing.Point(94, 85);
            this.dateTimePickerFr.Name = "dateTimePickerFr";
            this.dateTimePickerFr.Size = new System.Drawing.Size(137, 23);
            this.dateTimePickerFr.TabIndex = 3;
            this.dateTimePickerFr.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 58;
            this.label1.Text = "日付：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 59;
            this.label2.Text = "項目：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 61;
            this.label4.Text = "元業務番号：";
            // 
            // labelItem
            // 
            this.labelItem.AutoSize = true;
            this.labelItem.Location = new System.Drawing.Point(150, 141);
            this.labelItem.Name = "labelItem";
            this.labelItem.Size = new System.Drawing.Size(31, 15);
            this.labelItem.TabIndex = 63;
            this.labelItem.Text = "名称";
            // 
            // labelSTaskName
            // 
            this.labelSTaskName.Location = new System.Drawing.Point(192, 209);
            this.labelSTaskName.Name = "labelSTaskName";
            this.labelSTaskName.Size = new System.Drawing.Size(539, 23);
            this.labelSTaskName.TabIndex = 64;
            this.labelSTaskName.Text = "業務名\r\n";
            this.labelSTaskName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 262);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 65;
            this.label6.Text = "新業務番号：";
            // 
            // labelDTaskName
            // 
            this.labelDTaskName.Location = new System.Drawing.Point(192, 259);
            this.labelDTaskName.Name = "labelDTaskName";
            this.labelDTaskName.Size = new System.Drawing.Size(527, 23);
            this.labelDTaskName.TabIndex = 67;
            this.labelDTaskName.Text = "業務名\r\n";
            this.labelDTaskName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxItem
            // 
            this.textBoxItem.Location = new System.Drawing.Point(94, 138);
            this.textBoxItem.Name = "textBoxItem";
            this.textBoxItem.Size = new System.Drawing.Size(50, 23);
            this.textBoxItem.TabIndex = 5;
            this.textBoxItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxItem.TextChanged += new System.EventHandler(this.textBoxItem_TextChanged);
            this.textBoxItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // textBoxSTask
            // 
            this.textBoxSTask.Location = new System.Drawing.Point(106, 209);
            this.textBoxSTask.Name = "textBoxSTask";
            this.textBoxSTask.Size = new System.Drawing.Size(80, 23);
            this.textBoxSTask.TabIndex = 6;
            this.textBoxSTask.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSTask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // textBoxDTask
            // 
            this.textBoxDTask.Location = new System.Drawing.Point(106, 259);
            this.textBoxDTask.Name = "textBoxDTask";
            this.textBoxDTask.Size = new System.Drawing.Size(80, 23);
            this.textBoxDTask.TabIndex = 7;
            this.textBoxDTask.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxDTask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(258, 85);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(137, 23);
            this.dateTimePickerTo.TabIndex = 4;
            this.dateTimePickerTo.Visible = false;
            this.dateTimePickerTo.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // labelMes
            // 
            this.labelMes.AutoSize = true;
            this.labelMes.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMes.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelMes.Location = new System.Drawing.Point(346, 305);
            this.labelMes.Name = "labelMes";
            this.labelMes.Size = new System.Drawing.Size(66, 24);
            this.labelMes.TabIndex = 75;
            this.labelMes.Text = "label5";
            // 
            // FormTaskChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.labelMes);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.textBoxDTask);
            this.Controls.Add(this.textBoxSTask);
            this.Controls.Add(this.textBoxItem);
            this.Controls.Add(this.labelDTaskName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelSTaskName);
            this.Controls.Add(this.labelItem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerFr);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxOffice);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTaskChange";
            this.Text = "業務番号振替処理";
            this.Load += new System.EventHandler(this.FormTaskChange_Load);
            this.Shown += new System.EventHandler(this.FormTaskChange_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerFr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelItem;
        private System.Windows.Forms.Label labelSTaskName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelDTaskName;
        private System.Windows.Forms.TextBox textBoxItem;
        private System.Windows.Forms.TextBox textBoxSTask;
        private System.Windows.Forms.TextBox textBoxDTask;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label labelMes;
    }
}
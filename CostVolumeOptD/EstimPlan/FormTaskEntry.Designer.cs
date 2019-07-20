namespace EstimPlan 
{
    partial class FormTaskEntry
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPartner = new System.Windows.Forms.ComboBox();
            this.textBoxTaskName = new System.Windows.Forms.TextBox();
            this.textBoxTaskPlace = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.comboBoxCostType = new System.Windows.Forms.ComboBox();
            this.labelTaskCode = new System.Windows.Forms.Label();
            this.textBoxPartner = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "業務名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "発注者名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "業務場所：";
            // 
            // comboBoxPartner
            // 
            this.comboBoxPartner.FormattingEnabled = true;
            this.comboBoxPartner.Location = new System.Drawing.Point(132, 132);
            this.comboBoxPartner.Name = "comboBoxPartner";
            this.comboBoxPartner.Size = new System.Drawing.Size(640, 23);
            this.comboBoxPartner.TabIndex = 3;
            this.comboBoxPartner.Visible = false;
            // 
            // textBoxTaskName
            // 
            this.textBoxTaskName.Location = new System.Drawing.Point(170, 61);
            this.textBoxTaskName.Name = "textBoxTaskName";
            this.textBoxTaskName.Size = new System.Drawing.Size(571, 23);
            this.textBoxTaskName.TabIndex = 4;
            this.textBoxTaskName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxTaskName_KeyDown);
            // 
            // textBoxTaskPlace
            // 
            this.textBoxTaskPlace.Location = new System.Drawing.Point(101, 161);
            this.textBoxTaskPlace.Name = "textBoxTaskPlace";
            this.textBoxTaskPlace.Size = new System.Drawing.Size(640, 23);
            this.textBoxTaskPlace.TabIndex = 5;
            this.textBoxTaskPlace.TextChanged += new System.EventHandler(this.textBoxTaskPlace_TextChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(31, 217);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(100, 40);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "登録";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(201, 217);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 40);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "取消・戻る";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "部署：";
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Location = new System.Drawing.Point(64, 11);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(169, 15);
            this.labelPublisher.TabIndex = 9;
            this.labelPublisher.Text = "発行部署名（例）本社測量など";
            // 
            // comboBoxCostType
            // 
            this.comboBoxCostType.FormattingEnabled = true;
            this.comboBoxCostType.Location = new System.Drawing.Point(101, 61);
            this.comboBoxCostType.Name = "comboBoxCostType";
            this.comboBoxCostType.Size = new System.Drawing.Size(63, 23);
            this.comboBoxCostType.TabIndex = 11;
            // 
            // labelTaskCode
            // 
            this.labelTaskCode.AutoSize = true;
            this.labelTaskCode.Location = new System.Drawing.Point(453, 11);
            this.labelTaskCode.Name = "labelTaskCode";
            this.labelTaskCode.Size = new System.Drawing.Size(63, 15);
            this.labelTaskCode.TabIndex = 12;
            this.labelTaskCode.Text = "TaskCode";
            this.labelTaskCode.Visible = false;
            // 
            // textBoxPartner
            // 
            this.textBoxPartner.Location = new System.Drawing.Point(101, 111);
            this.textBoxPartner.Name = "textBoxPartner";
            this.textBoxPartner.Size = new System.Drawing.Size(640, 23);
            this.textBoxPartner.TabIndex = 13;
            this.textBoxPartner.TextChanged += new System.EventHandler(this.textBoxPartner_TextChanged);
            this.textBoxPartner.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPartner_KeyDown);
            // 
            // FormTaskEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 281);
            this.Controls.Add(this.textBoxPartner);
            this.Controls.Add(this.labelTaskCode);
            this.Controls.Add(this.comboBoxCostType);
            this.Controls.Add(this.labelPublisher);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxTaskPlace);
            this.Controls.Add(this.textBoxTaskName);
            this.Controls.Add(this.comboBoxPartner);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTaskEntry";
            this.Text = "見積・計画業務登録";
            this.Load += new System.EventHandler(this.FormTaskEntry_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxPartner;
        private System.Windows.Forms.TextBox textBoxTaskName;
        private System.Windows.Forms.TextBox textBoxTaskPlace;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.ComboBox comboBoxCostType;
        private System.Windows.Forms.Label labelTaskCode;
        private System.Windows.Forms.TextBox textBoxPartner;
    }
}
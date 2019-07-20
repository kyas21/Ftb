namespace CostProc
{
    partial class FormContractCost
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.textBoxTaskCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTaskName = new System.Windows.Forms.Label();
            this.labelPartner = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work00 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost00 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Work11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFY = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(215, 8);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(56, 23);
            this.comboBoxOffice.TabIndex = 0;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(277, 8);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(56, 23);
            this.comboBoxDepart.TabIndex = 1;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(130, 11);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(79, 15);
            this.label31.TabIndex = 85;
            this.label31.Text = "営業事業所：";
            // 
            // textBoxTaskCode
            // 
            this.textBoxTaskCode.Location = new System.Drawing.Point(439, 8);
            this.textBoxTaskCode.Name = "textBoxTaskCode";
            this.textBoxTaskCode.Size = new System.Drawing.Size(70, 23);
            this.textBoxTaskCode.TabIndex = 86;
            this.textBoxTaskCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(366, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 87;
            this.label1.Text = "業務番号：";
            // 
            // labelTaskName
            // 
            this.labelTaskName.AutoSize = true;
            this.labelTaskName.Location = new System.Drawing.Point(515, 11);
            this.labelTaskName.Name = "labelTaskName";
            this.labelTaskName.Size = new System.Drawing.Size(44, 15);
            this.labelTaskName.TabIndex = 88;
            this.labelTaskName.Text = "タスク名";
            // 
            // labelPartner
            // 
            this.labelPartner.AutoSize = true;
            this.labelPartner.Location = new System.Drawing.Point(515, 37);
            this.labelPartner.Name = "labelPartner";
            this.labelPartner.Size = new System.Drawing.Size(55, 15);
            this.labelPartner.TabIndex = 89;
            this.labelPartner.Text = "発注元名";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MName,
            this.MCode,
            this.Unit,
            this.Price,
            this.WorkSum,
            this.CostSum,
            this.Work00,
            this.Cost00,
            this.Work01,
            this.Cost01,
            this.Work02,
            this.Cost02,
            this.Work03,
            this.Cost03,
            this.Work04,
            this.Cost04,
            this.Work05,
            this.Cost05,
            this.Work06,
            this.Cost06,
            this.Work07,
            this.Cost07,
            this.Work08,
            this.Cost08,
            this.Work09,
            this.Cost09,
            this.Work10,
            this.Cost10,
            this.Work11,
            this.Cost11});
            this.dataGridView1.Location = new System.Drawing.Point(12, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1241, 560);
            this.dataGridView1.TabIndex = 90;
            // 
            // MName
            // 
            this.MName.Frozen = true;
            this.MName.HeaderText = "名称";
            this.MName.Name = "MName";
            this.MName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MName.Width = 150;
            // 
            // MCode
            // 
            this.MCode.Frozen = true;
            this.MCode.HeaderText = "";
            this.MCode.Name = "MCode";
            this.MCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MCode.Width = 50;
            // 
            // Unit
            // 
            this.Unit.Frozen = true;
            this.Unit.HeaderText = "単位";
            this.Unit.Name = "Unit";
            this.Unit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Unit.Width = 70;
            // 
            // Price
            // 
            this.Price.Frozen = true;
            this.Price.HeaderText = "単価";
            this.Price.Name = "Price";
            this.Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Price.Width = 60;
            // 
            // WorkSum
            // 
            this.WorkSum.Frozen = true;
            this.WorkSum.HeaderText = "工数計";
            this.WorkSum.Name = "WorkSum";
            this.WorkSum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.WorkSum.Width = 60;
            // 
            // CostSum
            // 
            this.CostSum.Frozen = true;
            this.CostSum.HeaderText = "原価計";
            this.CostSum.Name = "CostSum";
            this.CostSum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CostSum.Width = 80;
            // 
            // Work00
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Work00.DefaultCellStyle = dataGridViewCellStyle1;
            this.Work00.HeaderText = "4月       工数";
            this.Work00.Name = "Work00";
            this.Work00.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work00.Width = 50;
            // 
            // Cost00
            // 
            this.Cost00.HeaderText = "               原価";
            this.Cost00.Name = "Cost00";
            this.Cost00.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost00.Width = 70;
            // 
            // Work01
            // 
            this.Work01.HeaderText = "5月       工数";
            this.Work01.Name = "Work01";
            this.Work01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work01.Width = 50;
            // 
            // Cost01
            // 
            this.Cost01.HeaderText = "               原価";
            this.Cost01.Name = "Cost01";
            this.Cost01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost01.Width = 70;
            // 
            // Work02
            // 
            this.Work02.HeaderText = "6月       工数";
            this.Work02.Name = "Work02";
            this.Work02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work02.Width = 50;
            // 
            // Cost02
            // 
            this.Cost02.HeaderText = "               原価";
            this.Cost02.Name = "Cost02";
            this.Cost02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost02.Width = 70;
            // 
            // Work03
            // 
            this.Work03.HeaderText = "7月       工数";
            this.Work03.Name = "Work03";
            this.Work03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work03.Width = 50;
            // 
            // Cost03
            // 
            this.Cost03.HeaderText = "               原価";
            this.Cost03.Name = "Cost03";
            this.Cost03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost03.Width = 70;
            // 
            // Work04
            // 
            this.Work04.HeaderText = "8月       工数";
            this.Work04.Name = "Work04";
            this.Work04.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work04.Width = 50;
            // 
            // Cost04
            // 
            this.Cost04.HeaderText = "               原価";
            this.Cost04.Name = "Cost04";
            this.Cost04.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost04.Width = 70;
            // 
            // Work05
            // 
            this.Work05.HeaderText = "9月       工数";
            this.Work05.Name = "Work05";
            this.Work05.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work05.Width = 50;
            // 
            // Cost05
            // 
            this.Cost05.HeaderText = "               原価";
            this.Cost05.Name = "Cost05";
            this.Cost05.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost05.Width = 70;
            // 
            // Work06
            // 
            this.Work06.HeaderText = "10月      工数";
            this.Work06.Name = "Work06";
            this.Work06.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work06.Width = 50;
            // 
            // Cost06
            // 
            this.Cost06.HeaderText = "               原価";
            this.Cost06.Name = "Cost06";
            this.Cost06.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost06.Width = 70;
            // 
            // Work07
            // 
            this.Work07.HeaderText = "11月      工数";
            this.Work07.Name = "Work07";
            this.Work07.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work07.Width = 50;
            // 
            // Cost07
            // 
            this.Cost07.HeaderText = "               原価";
            this.Cost07.Name = "Cost07";
            this.Cost07.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost07.Width = 70;
            // 
            // Work08
            // 
            this.Work08.HeaderText = "12月      工数";
            this.Work08.Name = "Work08";
            this.Work08.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work08.Width = 50;
            // 
            // Cost08
            // 
            this.Cost08.HeaderText = "               原価";
            this.Cost08.Name = "Cost08";
            this.Cost08.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost08.Width = 70;
            // 
            // Work09
            // 
            this.Work09.HeaderText = "1月       工数";
            this.Work09.Name = "Work09";
            this.Work09.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work09.Width = 50;
            // 
            // Cost09
            // 
            this.Cost09.HeaderText = "               原価";
            this.Cost09.Name = "Cost09";
            this.Cost09.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost09.Width = 70;
            // 
            // Work10
            // 
            this.Work10.HeaderText = "2月       工数";
            this.Work10.Name = "Work10";
            this.Work10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work10.Width = 50;
            // 
            // Cost10
            // 
            this.Cost10.HeaderText = "               原価";
            this.Cost10.Name = "Cost10";
            this.Cost10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost10.Width = 70;
            // 
            // Work11
            // 
            this.Work11.HeaderText = "3月       工数";
            this.Work11.Name = "Work11";
            this.Work11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Work11.Width = 50;
            // 
            // Cost11
            // 
            this.Cost11.HeaderText = "               原価";
            this.Cost11.Name = "Cost11";
            this.Cost11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost11.Width = 70;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(12, 629);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 91;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1163, 6);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 92;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 93;
            this.label2.Text = "年度";
            // 
            // comboBoxFY
            // 
            this.comboBoxFY.FormattingEnabled = true;
            this.comboBoxFY.Location = new System.Drawing.Point(11, 8);
            this.comboBoxFY.Name = "comboBoxFY";
            this.comboBoxFY.Size = new System.Drawing.Size(60, 23);
            this.comboBoxFY.TabIndex = 94;
            this.comboBoxFY.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // FormContractCost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.comboBoxFY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelPartner);
            this.Controls.Add(this.labelTaskName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTaskCode);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.comboBoxOffice);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormContractCost";
            this.Text = "労働保険資料 元請作業実績集計";
            this.Load += new System.EventHandler(this.FormContractCost_Load);
            this.Shown += new System.EventHandler(this.FormContractCost_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox textBoxTaskCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTaskName;
        private System.Windows.Forms.Label labelPartner;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxFY;
        private System.Windows.Forms.DataGridViewTextBoxColumn MName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work00;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost00;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work04;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost04;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work05;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost05;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work06;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost06;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work07;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost07;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work08;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost08;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work09;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost09;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Work11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost11;
    }
}
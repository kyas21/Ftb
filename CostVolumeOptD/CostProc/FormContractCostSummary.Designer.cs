namespace CostProc
{
    partial class FormContractCostSummary
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
            this.comboBoxFY = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.comboBoxDepart = new System.Windows.Forms.ComboBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost00 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDisplay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxFY
            // 
            this.comboBoxFY.FormattingEnabled = true;
            this.comboBoxFY.Location = new System.Drawing.Point(13, 22);
            this.comboBoxFY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxFY.Name = "comboBoxFY";
            this.comboBoxFY.Size = new System.Drawing.Size(69, 23);
            this.comboBoxFY.TabIndex = 99;
            this.comboBoxFY.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 98;
            this.label2.Text = "年度";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(152, 26);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 15);
            this.label31.TabIndex = 97;
            this.label31.Text = "事業所：";
            // 
            // comboBoxDepart
            // 
            this.comboBoxDepart.FormattingEnabled = true;
            this.comboBoxDepart.Location = new System.Drawing.Point(286, 22);
            this.comboBoxDepart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxDepart.Name = "comboBoxDepart";
            this.comboBoxDepart.Size = new System.Drawing.Size(65, 23);
            this.comboBoxDepart.TabIndex = 96;
            this.comboBoxDepart.Visible = false;
            this.comboBoxDepart.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Location = new System.Drawing.Point(213, 22);
            this.comboBoxOffice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(65, 23);
            this.comboBoxOffice.TabIndex = 95;
            this.comboBoxOffice.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaskCode,
            this.PartnerName,
            this.TaskName,
            this.Cost00,
            this.Cost01,
            this.Cost02,
            this.Cost03,
            this.Cost04,
            this.Cost05,
            this.Cost06,
            this.Cost07,
            this.Cost08,
            this.Cost09,
            this.Cost10,
            this.Cost11,
            this.CostSum});
            this.dataGridView1.Location = new System.Drawing.Point(11, 61);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1245, 562);
            this.dataGridView1.TabIndex = 100;
            // 
            // TaskCode
            // 
            this.TaskCode.Frozen = true;
            this.TaskCode.HeaderText = "業務番号";
            this.TaskCode.Name = "TaskCode";
            this.TaskCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskCode.Width = 65;
            // 
            // PartnerName
            // 
            this.PartnerName.Frozen = true;
            this.PartnerName.HeaderText = "発注者名";
            this.PartnerName.Name = "PartnerName";
            this.PartnerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PartnerName.Width = 110;
            // 
            // TaskName
            // 
            this.TaskName.Frozen = true;
            this.TaskName.HeaderText = "業務名";
            this.TaskName.Name = "TaskName";
            this.TaskName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TaskName.Width = 410;
            // 
            // Cost00
            // 
            this.Cost00.HeaderText = "4月";
            this.Cost00.Name = "Cost00";
            this.Cost00.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost00.Width = 80;
            // 
            // Cost01
            // 
            this.Cost01.HeaderText = "5月";
            this.Cost01.Name = "Cost01";
            this.Cost01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost01.Width = 80;
            // 
            // Cost02
            // 
            this.Cost02.HeaderText = "6月";
            this.Cost02.Name = "Cost02";
            this.Cost02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost02.Width = 80;
            // 
            // Cost03
            // 
            this.Cost03.HeaderText = "7月";
            this.Cost03.Name = "Cost03";
            this.Cost03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost03.Width = 80;
            // 
            // Cost04
            // 
            this.Cost04.HeaderText = "8月";
            this.Cost04.Name = "Cost04";
            this.Cost04.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost04.Width = 80;
            // 
            // Cost05
            // 
            this.Cost05.HeaderText = "9月";
            this.Cost05.Name = "Cost05";
            this.Cost05.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost05.Width = 80;
            // 
            // Cost06
            // 
            this.Cost06.HeaderText = "10月";
            this.Cost06.Name = "Cost06";
            this.Cost06.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost06.Width = 80;
            // 
            // Cost07
            // 
            this.Cost07.HeaderText = "11月";
            this.Cost07.Name = "Cost07";
            this.Cost07.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost07.Width = 80;
            // 
            // Cost08
            // 
            this.Cost08.HeaderText = "12月";
            this.Cost08.Name = "Cost08";
            this.Cost08.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost08.Width = 80;
            // 
            // Cost09
            // 
            this.Cost09.HeaderText = "1月";
            this.Cost09.Name = "Cost09";
            this.Cost09.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost09.Width = 80;
            // 
            // Cost10
            // 
            this.Cost10.HeaderText = "2月";
            this.Cost10.Name = "Cost10";
            this.Cost10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost10.Width = 80;
            // 
            // Cost11
            // 
            this.Cost11.HeaderText = "3月";
            this.Cost11.Name = "Cost11";
            this.Cost11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cost11.Width = 80;
            // 
            // CostSum
            // 
            this.CostSum.HeaderText = "合計";
            this.CostSum.Name = "CostSum";
            this.CostSum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CostSum.Width = 80;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(11, 631);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(100, 40);
            this.buttonPrint.TabIndex = 101;
            this.buttonPrint.Text = "印刷";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.IndianRed;
            this.buttonClose.Location = new System.Drawing.Point(1166, 6);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 40);
            this.buttonClose.TabIndex = 102;
            this.buttonClose.Text = "終了";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDisplay
            // 
            this.buttonDisplay.BackColor = System.Drawing.Color.LightYellow;
            this.buttonDisplay.Location = new System.Drawing.Point(357, 12);
            this.buttonDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDisplay.Name = "buttonDisplay";
            this.buttonDisplay.Size = new System.Drawing.Size(140, 40);
            this.buttonDisplay.TabIndex = 103;
            this.buttonDisplay.Text = "表示";
            this.buttonDisplay.UseVisualStyleBackColor = false;
            this.buttonDisplay.Click += new System.EventHandler(this.button_Click);
            // 
            // FormContractCostSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.buttonDisplay);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxFY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.comboBoxDepart);
            this.Controls.Add(this.comboBoxOffice);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormContractCostSummary";
            this.Text = "労働保険資料 工事原価総括表";
            this.Load += new System.EventHandler(this.FormContractCostSummary_Load);
            this.Shown += new System.EventHandler(this.FormContractCost_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox comboBoxDepart;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost00;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost04;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost05;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost06;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost07;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost08;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost09;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost11;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostSum;
    }
}
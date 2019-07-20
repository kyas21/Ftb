using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;

namespace ListForm
{
    public partial class FormVolumeCostDetailList : Form
    {

        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        CostReportData[] crd;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormVolumeCostDetailList()
        {
            InitializeComponent();
        }

        public FormVolumeCostDetailList(CostReportData[] crd)
        {
            InitializeComponent();
            this.crd = crd;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//


        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormVolumeCostDetailList_Load(object sender, EventArgs e)
        {
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();

            dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToDeleteRows = false;

            // 列幅設定、入力不可設定   
            for (int i = 0; i < dataGridView1.ColumnCount; i++) dataGridView1.Columns[i].ReadOnly = true;
            if (crd.GetLength(0) > 1) dataGridView1.Rows.Add(crd.GetLength(0) - 1);

            //年月日
            DateTime dtReportDateBefore = DateTime.MinValue;
            string strReportDateBefore = dtReportDateBefore.ToString("yyyy/MM/dd");

            //伝票番号
            string strSlipNoBefore = "";

            //累計（小計）
            decimal decCumulative = 0;
            //累計（合計）
            decimal decTotalCumulative = 0;
            int intGridCnt = 0;
            for (int i = 0; i < crd.GetLength(0); i++)
            {
                if (intGridCnt != 0)
                {
                    if (strSlipNoBefore != crd[i].SlipNo.ToString())
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value = "";                                               //年月日
                        dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value = "";                                                   //伝票No
                        dataGridView1.Rows[intGridCnt].Cells["CostCode"].Value = "";                                                 //原価コード
                        dataGridView1.Rows[intGridCnt].Cells["Item"].Value = "";                                                     //品名
                        dataGridView1.Rows[intGridCnt].Cells["Unit"].Value = "";                                                     //単位
                        dataGridView1.Rows[intGridCnt].Cells["UnitPrice"].Value = "";                                                //単価
                        dataGridView1.Rows[intGridCnt].Cells["Quantity"].Value = "伝票計";                                           //数量
                        dataGridView1.Rows[intGridCnt].Cells["Cost"].Value = DHandling.DecimaltoStr(decCumulative, "#,0");           //金額(小計)
                        decTotalCumulative = decTotalCumulative + decCumulative;
                        dataGridView1.Rows[intGridCnt].Cells["Cumulative"].Value = DHandling.DecimaltoStr(decTotalCumulative, "#,0");//累計
                        decCumulative = 0;
                        intGridCnt++;
                    }
                }

                if (i == crd.GetLength(0) - 1)//最後の場合、伝票計、合計の行を追加しておく
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows.Add();
                }
                dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value = "";
                if (crd[i].ReportDate != null)
                {
                    if (strReportDateBefore != crd[i].ReportDate.ToString("yyyy/MM/dd"))
                    {
                        dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value = crd[i].ReportDate.ToString("yyyy/MM/dd");             //年月日
                        strReportDateBefore = dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value.ToString();
                    }
                }

                dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value = "";
                if (crd[i].SlipNo.ToString() != null && crd[i].SlipNo.ToString() != "")
                {
                    if (strSlipNoBefore != crd[i].SlipNo.ToString())
                    {
                        dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value = crd[i].SlipNo.ToString();
                        strSlipNoBefore = dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value.ToString();
                    }
                }

                dataGridView1.Rows[intGridCnt].Cells["CostCode"].Value = crd[i].ItemCode;                                                                //原価コード
                dataGridView1.Rows[intGridCnt].Cells["Item"].Value = crd[i].Item;                                                                        //品名
                dataGridView1.Rows[intGridCnt].Cells["Unit"].Value = crd[i].Unit;                                                                        //単位
                dataGridView1.Rows[intGridCnt].Cells["UnitPrice"].Value = (crd[i].UnitPrice == 0) ? "" : DHandling.DecimaltoStr(crd[i].UnitPrice, "#,0");//単価
                dataGridView1.Rows[intGridCnt].Cells["Quantity"].Value = (crd[i].Quantity == 0) ? "" : DHandling.DecimaltoStr(crd[i].Quantity, "#,0.00");//数量
                dataGridView1.Rows[intGridCnt].Cells["Cost"].Value = (crd[i].Cost == 0) ? "0" : DHandling.DecimaltoStr(crd[i].Cost, "#,0");              //金額
                decCumulative = decCumulative + crd[i].Cost;
                dataGridView1.Rows[intGridCnt].Cells["Cumulative"].Value = "N";
                intGridCnt++;
            }

            if (crd.GetLength(0) > 0)
            {
                dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value = "";                                               //年月日
                dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value = "";                                                   //伝票No
                dataGridView1.Rows[intGridCnt].Cells["CostCode"].Value = "";                                                 //原価コード
                dataGridView1.Rows[intGridCnt].Cells["Item"].Value = "";                                                     //品名
                dataGridView1.Rows[intGridCnt].Cells["Unit"].Value = "";                                                     //単位
                dataGridView1.Rows[intGridCnt].Cells["UnitPrice"].Value = "";                                                //単価
                dataGridView1.Rows[intGridCnt].Cells["Quantity"].Value = "伝票計";                                           //数量
                dataGridView1.Rows[intGridCnt].Cells["Cost"].Value = DHandling.DecimaltoStr(decCumulative, "#,0");           //金額(小計)
                decTotalCumulative = decTotalCumulative + decCumulative;
                dataGridView1.Rows[intGridCnt].Cells["Cumulative"].Value = DHandling.DecimaltoStr(decTotalCumulative, "#,0");//累計
                decCumulative = 0;
                intGridCnt++;
                dataGridView1.Rows[intGridCnt].Cells["ReportDate"].Value = "";                                               //年月日
                dataGridView1.Rows[intGridCnt].Cells["SlipNo"].Value = "";                                                   //伝票No
                dataGridView1.Rows[intGridCnt].Cells["CostCode"].Value = "";                                                 //原価コード
                dataGridView1.Rows[intGridCnt].Cells["Item"].Value = "【合計】";                                             //品名
                dataGridView1.Rows[intGridCnt].Cells["Unit"].Value = "";                                                     //単位
                dataGridView1.Rows[intGridCnt].Cells["UnitPrice"].Value = "";                                                //単価
                dataGridView1.Rows[intGridCnt].Cells["Quantity"].Value = "";                                                 //数量
                dataGridView1.Rows[intGridCnt].Cells["Cost"].Value = DHandling.DecimaltoStr(decTotalCumulative, "#,0");      //金額(小計)
                dataGridView1.Rows[intGridCnt].Cells["Cumulative"].Value = DHandling.DecimaltoStr(decTotalCumulative, "#,0");//累計
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

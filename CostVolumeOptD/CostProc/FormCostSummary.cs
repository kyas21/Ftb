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
using PrintOut;

namespace CostProc
{
    public partial class FormCostSummary : Form
    {
        //-----------------------------------------------//
        //      Field
        //-----------------------------------------------//
        CostInfoIFOp cif;

        const string BookName = "原価集計表.xlsx";
        const string SheetName = "CostSummary";

        // Wakamatsu 20170316
        //string[] hTextArray1 = new string[] { "（業務番号）業務", "（コード）原価項目" };
        //string[] hTextArray2 = new string[] { "（コード）原価項目", "（業務番号）業務" };
        //string[] nTextArray1 = new string[] { "Task", "Item" };
        //string[] nTextArray2 = new string[] { "Item", "Task" };
        //string[] hTextItemName = new string[] { "【業務番号 計】", "【原価項目 計】", "【期間合計】" };
        string[] hTextItemName = new string[] { "", "", "", "【期間合計】" };
        int TotalColumns = 2;               // 合計表示列
        // Wakamatsu 20170316


        //-----------------------------------------------//
        //      Construction
        //-----------------------------------------------//
        public FormCostSummary()
        {
            InitializeComponent();
        }

        public FormCostSummary(CostInfoIFOp cif)
        {
            this.cif = cif;
            InitializeComponent();
        }

        //-----------------------------------------------//
        //      Property
        //-----------------------------------------------//

        //-----------------------------------------------//
        //      Method
        //-----------------------------------------------//
        private void FormCostSummary_Load(object sender, EventArgs e)
        {
            // DataGridViewの列タイトル設定
            UiHandling ui = new UiHandling(dataGridView1);
            // Wakamatsu 20170316
            string[] nTextArray = cif.EditColumnHeaderArraySummary(cif.Class0, cif.Class1, cif.Class2);
            //ui.DgvColumnName(0, cif.EditColumnNameArraySummary(cif.Class0, cif.Class1, cif.Class2));
            //ui.DgvColumnHeader(0, cif.EditColumnHeaderArraySummary(cif.Class0, cif.Class1, cif.Class2));
            ui.DgvColumnHeader(0, nTextArray);

            int VisibleCount = 0;

            for (int i = 0; i < nTextArray.Length; i++)
            {
                if (nTextArray[i] == "")
                    this.dataGridView1.Columns[i].Visible = false;
                else
                    VisibleCount++;
            }

            switch (VisibleCount)
            {
                case 0:
                    this.dataGridView1.Columns[0].Visible = true;
                    this.dataGridView1.Columns[0].Width = 980;
                    TotalColumns = 0;
                    break;
                case 1:
                    for (int i = 0; i < nTextArray.Length; i++)
                    {
                        if (this.dataGridView1.Columns[i].Visible == true)
                        {
                            this.dataGridView1.Columns[i].Width = 980;
                            TotalColumns = i;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < nTextArray.Length; i++)
                    {
                        if (this.dataGridView1.Columns[i].Visible == true)
                        {
                            this.dataGridView1.Columns[i].Width = 490;
                            TotalColumns = i;
                        }
                    }
                    break;
                case 3:
                    this.dataGridView1.Columns[0].Width = 340;
                    this.dataGridView1.Columns[1].Width = 320;
                    this.dataGridView1.Columns[2].Width = 320;
                    TotalColumns = 2;
                    break;
                default:
                    break;
            }
            // Wakamatsu 20170316

            //原価計上日
            labelFrom.Text = cif.DateSOP.ToString("yyyy/MM/dd");
            labelTo.Text = cif.DateEOP.ToString("yyyy/MM/dd");

            //明細表
            labelType.Text = cif.ClassificationItem;

            //出力範囲
            labelRange.Text = cif.OutputRange;

            labelOffice.Text = cif.Office;

            // データ読み込み
            displayDetailInformation(dataGridView1, cif.SqlStr);
        }

        // Wakamatsu 20170316
        private void FormCostSummary_Shown(object sender, EventArgs e)
        {
            this.dataGridView1.CurrentCell = null;
        }
        // Wakamatsu 20170316

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            // print 処理
            // Wakamatsu 20170301
            //Publish publ = new Publish(Folder.DefaultExcelTemplate("CostSummary.xlsx"));
            Publish publ = new Publish(Folder.DefaultExcelTemplate("原価集計表.xlsx"));
            // Wakamatsu 20170301
            PublishData pd = new PublishData();

            publ.ExcelFile("CostSummary", pd, dataGridView1);

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        // データ読み込み
        private bool displayDetailInformation(DataGridView dgv, string sqlStr)
        {
            DbAccess dba = new DbAccess();
            DataTable dt = dba.UsingSqlstring_Select(sqlStr);

            if (dt == null || dt.Rows.Count < 1) return false; // エラー処理

            // Wakamatsu 20170316
            //string[] valArray = new string[2];
            dgv.Rows.Add(dt.Rows.Count);
            DataRow dr;

            //DBから取得したデータを保持しておくための変数
            // Wakamatsu 20170316
            //string task = "";
            //string item = "";
            // Wakamatsu 20170316
            string quantity = "";
            string unitPrice = "";
            string cost = "";
            // Wakamatsu 20170316
            //string customer = "";
            //string leaderMName = "";
            //string salesMName = "";
            decimal WorkDecimal = 0;
            // Wakamatsu 20170316

            // Wakamatsu 20170316
            string[] GetClass = new string[3] { "", "", "" };                               // 比較対象データ格納用
            string[] TargetString = new string[3] { cif.Class0, cif.Class1, cif.Class2 };   // 設定情報格納用

            //string costCode = "";
            // Wakamatsu 20170316



            //前回の値
            // Wakamatsu 20170316
            //string preTask = "";
            //string preItem = "";
            string[] PreClass = new string[3] { "", "", "" };                               // 比較対象データ格納用(前行)
            // Wakamatsu 20170316

            //金額
            // Wakamatsu 20170316
            //decimal totalSectionCost = 0;   //totalCostよりさらに小さい区分け
            //decimal totalCost = 0;          //【作業項目 計】【業務番号 計】に表示する金額値
            decimal[] TotalCostClass = new decimal[3] { 0, 0, 0 };              // 表示条件画面○○毎金額合計
            // Wakamatsu 20170316
            decimal periodTotalCost = 0;    //【期間合計】に表示する金額値
            decimal costCalc = 0;           //金額

            //数量
            // Wakamatsu 20170316
            //decimal totalSectionQuantity = 0;
            //decimal totalQuantity = 0;
            decimal[] TotalQuantityClass = new decimal[3] { 0, 0, 0 };          // 表示条件画面○○毎数量合計
            // Wakamatsu 20170316
            decimal periodTotalQuantity = 0;
            decimal quantityCalc = 0;

            int intGridCnt = 0;             //グリッドの行数

            // Wakamatsu 20170316
            for (int i = 0; i < TargetString.Length; i++)
            {
                switch (TargetString[i])
                {
                    case "1":       // "業務番号"
                        hTextItemName[i] = "【業務番号 計】";
                        break;
                    case "2":       // "得意先"
                        hTextItemName[i] = "【得意先 計】";
                        break;
                    case "3":       // "原価項目"
                        hTextItemName[i] = "【原価項目 計】";
                        break;
                    case "4":       // "業務担当者"
                        hTextItemName[i] = "【業務担当者 計】";
                        break;
                    case "5":       // "営業担当者"
                        hTextItemName[i] = "【営業担当者 計】";
                        break;
                    default:        // "指定なし"
                        hTextItemName[i] = "";
                        break;
                }
            }
            // Wakamatsu 20170316

            //DBから取得したデータをグリッドへセット
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                //データを一時退避
                // Wakamatsu 20170316
                //valArray = editTaskInfo(Convert.ToString(dr["TaskCode"]));
                //task = valArray[0];
                //item = Convert.ToString(dr["Item"]);
                decimal.TryParse(Convert.ToString(dr["Quantity"]), out WorkDecimal);
                //quantity = decPointFormat(Convert.ToDecimal(dr["Quantity"]));
                quantity = decPointFormat(WorkDecimal);
                decimal.TryParse(Convert.ToString(dr["UnitPrice"]), out WorkDecimal);
                //unitPrice = decFormat(Convert.ToDecimal(dr["UnitPrice"]));
                unitPrice = decFormat(WorkDecimal);
                decimal.TryParse(Convert.ToString(dr["Cost"]), out WorkDecimal);
                //cost = decFormat(Convert.ToDecimal(dr["Cost"]));
                cost = decFormat(WorkDecimal);
                //customer = valArray[1];
                //leaderMName = editMemberName(Convert.ToString(dr["LeaderMCode"]));
                //salesMName = editMemberName(Convert.ToString(dr["SalesMCode"]));
                // Wakamatsu 20170316

                // Wakamatsu 20170316
                //costCode = Convert.ToString(dr["costCode"]);

                // 比較用データ格納
                GetClass[0] = Convert.ToString(dr["Class0"]);
                GetClass[1] = Convert.ToString(dr["Class1"]);
                GetClass[2] = Convert.ToString(dr["Class2"]);

                //for (int j = 0; j < GetClass.Length; j++)
                //{
                //    switch (TargetString[j])
                //    {
                //        case "1":       // "業務番号"
                //            GetClass[j] = task;
                //            break;
                //        case "2":       // "得意先"
                //            GetClass[j] = customer;
                //            break;
                //        case "3":       // "原価項目"
                //            GetClass[j] = "( " + costCode + " )  " + item;
                //            break;
                //        case "4":       // "業務担当者"
                //            GetClass[j] = leaderMName;
                //            break;
                //        case "5":       // "営業担当者"
                //            GetClass[j] = salesMName;
                //            break;
                //        default:        // "指定なし"
                //            GetClass[j] = "";
                //            break;
                //    }
                //}
                // Wakamatsu 20170316


                //if (cost.ToString().Trim() == "")
                //{
                //    costCalc =  0;
                //}
                //else
                //{
                //    costCalc = Convert.ToDecimal(cost);
                //}
                costCalc = string.IsNullOrEmpty((Convert.ToString(cost)).Trim()) ? 0 : Convert.ToDecimal(cost);
                //if (quantity.ToString().Trim() == "")
                //{
                //    quantityCalc = 0;
                //}
                //else
                //{
                //    quantityCalc = Convert.ToDecimal(quantity);
                //}
                quantityCalc = string.IsNullOrEmpty((Convert.ToString(quantity)).Trim()) ? 0 : Convert.ToDecimal(quantity);

                if (i > 0)//一回目かの判断（一回目の場合は前回との比較を行わない）
                {
                    // Wakamatsu 20170316
                    //if (cif.Class0 == "3" && cif.Class1 == "1" && cif.Class2 == "4")//"作業項目", "業務番号"
                    //{
                    //    if ((preItem != item) || (preTask != task))
                    //    {
                    //        if (preItem != item)//作業項目の変化チェック（作業項目, 業務番号を出力）
                    //        {
                    //            decTotalCostQuantitySet(dgv, 1, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt);
                    //            decTotalCostQuantitySet(dgv, 1, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt);

                    //            totalCost = 0;
                    //            totalSectionCost = 0;
                    //            totalQuantity = 0;
                    //            totalSectionQuantity = 0;
                    //        }
                    //        else if (preTask != task)//業務番号の変化チェック（業務番号を出力）
                    //        {
                    //            decTotalCostQuantitySet(dgv, 1, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt);
                    //            totalSectionCost = 0;
                    //            totalSectionQuantity = 0;
                    //        }
                    //    }
                    //}
                    if (PreClass[0] != GetClass[0])     // ○○毎キー
                    {
                        if (hTextItemName[2] != "")
                        {
                            // 行追加
                            decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[2], TotalCostClass[2], TotalQuantityClass[2], ref intGridCnt);

                            TotalCostClass[2] = 0;
                            TotalQuantityClass[2] = 0;
                        }

                        if (hTextItemName[1] != "")
                        {
                            // 行追加
                            decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[1], TotalCostClass[1], TotalQuantityClass[1], ref intGridCnt);

                            TotalCostClass[1] = 0;
                            TotalQuantityClass[1] = 0;
                        }

                        // 行追加
                        decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[0], TotalCostClass[0], TotalQuantityClass[0], ref intGridCnt);

                        TotalCostClass[0] = 0;
                        TotalQuantityClass[0] = 0;
                    }
                    else
                    {
                        if (PreClass[1] != GetClass[1])     // ○○別キー
                        {
                            if (hTextItemName[2] != "")
                            {
                                // 行追加
                                decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[2], TotalCostClass[2], TotalQuantityClass[2], ref intGridCnt);

                                TotalCostClass[2] = 0;
                                TotalQuantityClass[2] = 0;
                            }

                            // 行追加
                            decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[1], TotalCostClass[1], TotalQuantityClass[1], ref intGridCnt);

                            TotalCostClass[1] = 0;
                            TotalQuantityClass[1] = 0;
                        }
                        else
                        {
                            if (PreClass[2] != GetClass[2])     // ○○別キー
                            {
                                // 行追加
                                decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[2], TotalCostClass[2], TotalQuantityClass[2], ref intGridCnt);

                                TotalCostClass[2] = 0;
                                TotalQuantityClass[2] = 0;
                            }
                        }
                    }
                    // Wakamatsu 20170316
                }
                //グリッドへデータをセット
                // Wakamatsu 20170316
                //dgv.Rows[intGridCnt].Cells["Task"].Value = task;

                //dgv.Rows[intGridCnt].Cells["Item"].Value = item;
                //dgv.Rows[intGridCnt].Cells["Item"].Value = "( " + costCode + " )  " + item;
                dgv.Rows[intGridCnt].Cells["Class0"].Value = Convert.ToString(dr["Class0Disp"]);
                dgv.Rows[intGridCnt].Cells["Class1"].Value = Convert.ToString(dr["Class1Disp"]);
                dgv.Rows[intGridCnt].Cells["Class2"].Value = Convert.ToString(dr["Class2Disp"]);

                dgv.Rows[intGridCnt].Cells["Quantity"].Value = quantity;
                dgv.Rows[intGridCnt].Cells["UnitPrice"].Value = unitPrice;
                dgv.Rows[intGridCnt].Cells["Cost"].Value = cost;
                //dgv.Rows[intGridCnt].Cells["Customer"].Value = customer;
                //dgv.Rows[intGridCnt].Cells["LeaderMName"].Value = leaderMName;
                //dgv.Rows[intGridCnt].Cells["SalesMName"].Value = salesMName;
                // Wakamatsu 20170316

                //データの保持（次に取得したデータに異なるか比較する）
                // Wakamatsu 20170316
                //preTask = task;
                //preItem = item;
                PreClass[0] = GetClass[0];
                PreClass[1] = GetClass[1];
                PreClass[2] = GetClass[2];
                // Wakamatsu 20170316

                //作業項目計、業務番号計の計算
                //金額
                // Wakamatsu 20170316
                //totalCost = totalCost + costCalc;
                //totalSectionCost = totalSectionCost + costCalc; //作業項目, 業務番号などの組み合わせで使用
                TotalCostClass[0] += costCalc;
                TotalCostClass[1] += costCalc;
                TotalCostClass[2] += costCalc;
                // Wakamatsu 20170316
                periodTotalCost = periodTotalCost + costCalc;

                //数量
                // Wakamatsu 20170316
                //totalQuantity = totalQuantity + quantityCalc;
                //totalSectionQuantity = totalSectionQuantity + quantityCalc; //作業項目, 業務番号などの組み合わせで使用
                TotalQuantityClass[0] += quantityCalc;
                TotalQuantityClass[1] += quantityCalc;
                TotalQuantityClass[2] += quantityCalc;
                // Wakamatsu 20170316
                periodTotalQuantity = periodTotalQuantity + quantityCalc;
                intGridCnt++;
            }

            // Wakamatsu 20170316
            //if (cif.Class0 == "3" && cif.Class1 == "1" && cif.Class2 == "4")
            //{
            //    decTotalCostQuantitySet(dgv, 1, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt);
            //    decTotalCostQuantitySet(dgv, 1, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt);
            //}
            if (GetClass[2] != "")
                // 行追加(最終行)
                decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[2], TotalCostClass[2], TotalQuantityClass[2], ref intGridCnt);
            if (GetClass[1] != "")
                // 行追加(最終行)
                decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[1], TotalCostClass[1], TotalQuantityClass[1], ref intGridCnt);
            if (GetClass[0] != "")
                // 行追加(最終行)
                decTotalCostQuantitySet(dgv, TotalColumns, hTextItemName[0], TotalCostClass[0], TotalQuantityClass[0], ref intGridCnt);
            // Wakamatsu 20170316

            //期間合計の表示
            // Wakamatsu 20170316
            //dgv.Rows[intGridCnt].Cells[1].Value = hTextItemName[2];
            dgv.Rows[intGridCnt].Cells[TotalColumns].Value = hTextItemName[3];
            // Wakamatsu 20170316
            dgv.Rows[intGridCnt].Cells["Cost"].Value = decFormat(periodTotalCost);
            dgv.Rows[intGridCnt].Cells["Quantity"].Value = decPointFormat(periodTotalQuantity);
            return true;
        }

        // Wakamatsu 20170316
        //private string[] editTaskInfo(string taskCode)
        //{
        //    string debugstring = DHandling.NumberOfCharacters(taskCode, 1);
        //    string[] retval = new string[2];
        //    string sqlStr = "T.TaskName, P.PartnerName FROM D_Task AS T LEFT JOIN M_Partners AS P ON T.PartnerCode = P.PartnerCode WHERE T.TaskBaseCode = '" + DHandling.NumberOfCharacters(taskCode, 1) + "'";
        //    DbAccess dba = new DbAccess();
        //    DataTable dt = dba.UsingSqlstring_Select(sqlStr);
        //    if (dt == null || dt.Rows.Count < 1)
        //    {
        //        retval[0] = "（" + taskCode + "）";
        //        retval[1] = "";
        //    }
        //    else
        //    {
        //        DataRow dr = dt.Rows[0];
        //        retval[0] = "（" + taskCode + "）" + Convert.ToString(dr["TaskName"]);
        //        retval[1] = Convert.ToString(dr["PartnerName"]);
        //    }
        //    return retval;
        //}


        //private string editMemberName(string memberCode)
        //{
        //    if (String.IsNullOrEmpty(memberCode.Trim()))
        //        return "";

        //    memberCode = memberCode.Trim();
        //    if (memberCode.Length < 3)
        //    {
        //        memberCode = "00" + memberCode;
        //        memberCode = memberCode.Substring(memberCode.Length - 3, 3);
        //    }

        //    DbAccess dba = new DbAccess();
        //    DataTable dt = dba.UsingParamater_Select("M_Members", "WHERE MemberCode = '" + memberCode + "'");
        //    if (dt == null || dt.Rows.Count < 1)
        //    {
        //        return "";
        //    }
        //    DataRow dr = dt.Rows[0];
        //    return Convert.ToString(dr["Name"]);
        //}
        // Wakamatsu 20170316

        private static decimal toRegDecimal(string decStr)
        {
            return DHandling.ToRegDecimal(decStr);
        }

        private static string decFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "#,0");
        }

        private static string decPointFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "0.00");
        }

        private void buttonPrint_Click_1(object sender, EventArgs e)
        {
            // print 処理
            // Wakamatsu 20170301
            //PublishVolume publ = new PublishVolume(Folder.DefaultExcelTemplate("CostSummary.xlsx"));
            PublishVolume publ = new PublishVolume(Folder.DefaultExcelTemplate(BookName));
            // Wakamatsu 20170301
            PublishData pd = new PublishData();
            pd.CostOffice = labelOffice.Text;
            pd.CostReportDate = labelFrom.Text + "～" + labelTo.Text;
            pd.CostTypeData = labelType.Text;
            pd.CostRange = labelRange.Text;
            publ.ExcelFile(SheetName, pd, dataGridView1);
        }

        private void decTotalCostQuantitySet(DataGridView dgv, int cellNo, string itemName, decimal totalSectionCost, decimal totalSectionQuantity, ref int intGridCnt)
        {
            dgv.Rows.Add();
            dgv.Rows[intGridCnt].Cells[cellNo].Value = itemName;                                //項目名
            dgv.Rows[intGridCnt].Cells["Cost"].Value = decFormat(totalSectionCost);
            dgv.Rows[intGridCnt].Cells["Quantity"].Value = decPointFormat(totalSectionQuantity);
            intGridCnt++;
        }
    }
}

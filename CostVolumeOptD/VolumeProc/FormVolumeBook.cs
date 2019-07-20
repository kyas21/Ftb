//-----------------------------------------------------------------------------
//
// 業務集計台帳の表示と印刷
//
// Note:
//     20190206 asakawa 前年度受注額・前年度出来高・前年度原価の算出を
//              変更
//              出来高データの
//             「受注単月金額」
//             「『出来高未成業務金額』『出来高未請求金額』『出来高請求金額』の計」
//             「単月原価金額」のそれぞれの年間合算値を表示・印刷
//
//-----------------------------------------------------------------------------

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

namespace VolumeProc
{
    public partial class FormVolumeBook : Form
    {
        //----------------------------------------------------------------------
        //     Field
        //----------------------------------------------------------------------
        HumanProperty hp;
        private string[] OfficeArray;
        bool iniPro = true;

        //----------------------------------------------------------------------
        //     Contructor
        //----------------------------------------------------------------------
        public FormVolumeBook( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }

        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        /// <summary>
        /// フォームロード時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormVolumeBook_Load(object sender, EventArgs e)
        {
            UiHandling uih = new UiHandling(this.dataGridViewList);
            uih.DgvReadyNoRHeader();
            // 並び替えができないようにする
            //uih.NoSortable();
            // 部署コンボボックス設定
            CreateCbOffice();
            // 部門コンボボックス設定
            CreateCbDepartment();
            // 締め月コンボボックス設定
            CreateCbMonth("Dummy");
            // データグリッドビュー初期表示
            CreateDataGridView();
        }

        /// <summary>
        /// フォームロード後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormVolumeBook_Shown(object sender, EventArgs e)
        {
            iniPro = false;
            // Wakamatsu 20170313
            this.dataGridViewList.CurrentCell = null;
        }

        /// <summary>
        /// 部署コンボボックス変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOfficeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Wakamatsu 20170313
            if (iniPro) return;
            // 部門コンボボックス制御
            CreateCbDepartment();
        }

        /// <summary>
        /// 各ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonPrevew":
                    CreateDataGridView();
                    break;
                case "buttonPrint":
                    ExcelExport();
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }
        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        /// <summary>
        /// 部署コンボボックス設定
        /// </summary>
        private void CreateCbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(this.comboBoxOfficeCode);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName", "すべて", "Dummy", hp.AccessLevel);

            OfficeArray = new string[cbe.ValueItem.Length];
            Array.Copy(cbe.ValueItem, 0, OfficeArray, 0, OfficeArray.Length);
        }

        /// <summary>
        /// 部門コンボボックス項目設定
        /// </summary>
        private void CreateCbDepartment()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(this.comboBoxDepartment);
            cbe.DepartmentList((comboBoxOfficeCode.Text == Sign.HQOffice) ? "DEPH" : "DEPB", 1);

            if (this.comboBoxOfficeCode.Text == Sign.HQOffice)
                // 部門コンボボックス表示
                this.comboBoxDepartment.Visible = true;
            else
                // 部門コンボボックス非表示
                this.comboBoxDepartment.Visible = false;
        }

        /// <summary>
        /// 年月コンボボックス設定
        /// </summary>
        /// <param name="SetCode"></param>
        private void CreateCbMonth(string SetCode)
        {
            CommonData com = new CommonData();          // M_Commonアクセスクラス
            DateTime[] ClosingDate;                     // 締め年月格納用

            if (SetCode == "Dummy")
            {
                // すべてが選択されていた場合
                ClosingDate = new DateTime[this.comboBoxOfficeCode.Items.Count];
                for (int i = 0; i < this.comboBoxOfficeCode.Items.Count; i++)
                {
                    ClosingDate[i] = com.SelectCloseDate(Convert.ToString(((DataRowView)this.comboBoxOfficeCode.Items[i]).Row[0].ToString()));
                }
            }
            else
            {
                // すべて以外が選択されていた場合
                ClosingDate = new DateTime[1];
                ClosingDate[0] = com.SelectCloseDate(Convert.ToString(this.comboBoxOfficeCode.SelectedValue));
            }

            DateTime MaxDate = DateTime.MinValue;           // 最終締め年月格納用
            for (int i = 0; i < ClosingDate.Length; i++)
                // 最終締め月を取得
                if (MaxDate <= ClosingDate[i]) MaxDate = ClosingDate[i];

            // 最大1年度分の月情報を表示する
            int DefMonth = 0;               // 処理回数カウント用
            string AddYear = "";            // 年情報格納用
            string AddMonth = "";           // 月情報格納用
            do
            {
                //AddYear += MaxDate.AddMonths(-DefMonth).Year + MaxDate.AddMonths(-DefMonth).Month.ToString("00") + ",";
                AddYear += MaxDate.AddMonths(-DefMonth).ToString("yyyy/MM/dd") + ",";
                AddMonth += MaxDate.AddMonths(-DefMonth).Month + "月現在,";
                DefMonth++;
            } while (MaxDate.AddMonths(-DefMonth).Month != 6);

            // 末尾のカンマを外す
            AddYear = AddYear.Substring(0, AddYear.Length - 1);
            AddMonth = AddMonth.Substring(0, AddMonth.Length - 1);

            ComboBoxEdit cbe = new ComboBoxEdit(this.comboBoxMonth);        // コンボボックス制御クラス
            cbe.ValueItem = AddYear.Split(',');
            cbe.DisplayItem = AddMonth.Split(',');
            cbe.Basic();
        }

        /// <summary>
        /// データグリッドビュー表示
        /// </summary>
        private void CreateDataGridView()
        {
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            int RowCount = this.dataGridViewList.Rows.Count;
            if (RowCount > 0)
                // データグリッドビューすべての行を削除
                for (int i = 1; i <= RowCount; i++)
                    this.dataGridViewList.Rows.RemoveAt(0);

            SqlHandling sh = new SqlHandling();                         // SQL実行クラス
            DataTable dt = sh.SelectFullDescription(CreateSql());       // SQL実行

            if (dt == null) return;
            this.dataGridViewList.Rows.Add(dt.Rows.Count);

            DataRow dr = null;                                              // レコード格納用
            DataGridViewRow TargetRow = null;                               // 格納データグリッドビュー列格納用
            for (int i = 0; i < this.dataGridViewList.Rows.Count; i++)
            {
                dr = dt.Rows[i];

                // 20190206 asakawa ADD 1Line
                dr = GetDataLastYear(dr);
                // 20190206 asakawa ADD END

                TargetRow = this.dataGridViewList.Rows[i];

                TargetRow.Cells[0].Value = Convert.ToString(dr["OfficeName"]);                                      // 部署
                TargetRow.Cells[1].Value = Convert.ToString(dr["TaskCode"]);                                        // 業務番号
                TargetRow.Cells[2].Value = Convert.ToString(dr["TaskName"]);                                        // 業務名
                TargetRow.Cells[3].Value = Convert.ToString(dr["PartnerName"]);                                     // 業者名
                TargetRow.Cells[4].Value = CheckDatetTime(Convert.ToString(dr["StartDate"]), "yyyy/MM/dd");         // 工期開始
                TargetRow.Cells[5].Value = CheckDatetTime(Convert.ToString(dr["EndDate"]), "yyyy/MM/dd");           // 工期完了
                TargetRow.Cells[6].Value = Convert.ToString(dr["SalesName"]);                                       // 営業担当者
                TargetRow.Cells[7].Value = Convert.ToString(dr["LeaderName"]);                                      // 業務担当者
                TargetRow.Cells[8].Value = CheckDecimal(Convert.ToString(dr["LastOrder"]));                         // 前年度受注額          
                TargetRow.Cells[9].Value = CheckDecimal(Convert.ToString(dr["ThisOrder"]));                         // 本年度受注額

                // 受注額計
                if (Convert.ToString(TargetRow.Cells[8].Value) != "" ||
                    Convert.ToString(TargetRow.Cells[9].Value) != "")
                    TargetRow.Cells[10].Value = CheckDecimal(Convert.ToString(dr["TotalOrder"]));

                TargetRow.Cells[11].Value = CheckDecimal(Convert.ToString(dr["LastVolume"]));                       // 前年度出来高
                TargetRow.Cells[12].Value = CheckDecimal(Convert.ToString(dr["ThisVolume"]));                       // 本年度出来高

                // 出来高計
                if (Convert.ToString(TargetRow.Cells[11].Value) != "" ||
                    Convert.ToString(TargetRow.Cells[12].Value) != "")
                    TargetRow.Cells[13].Value = CheckDecimal(Convert.ToString(dr["TotalVolume"]));

                TargetRow.Cells[14].Value = CheckDecimal(Convert.ToString(dr["LastCost"]));                         // 前年度原価
                TargetRow.Cells[15].Value = CheckDecimal(Convert.ToString(dr["ThisCost"]));                         // 本年度原価

                // 原価計
                if (Convert.ToString(TargetRow.Cells[14].Value) != "" ||
                    Convert.ToString(TargetRow.Cells[15].Value) != "")
                    TargetRow.Cells[16].Value = CheckDecimal(Convert.ToString(dr["TotalCost"]));
                
                // 残業務高
                if (Convert.ToString(TargetRow.Cells[9].Value) != "" &&
                    Convert.ToString(TargetRow.Cells[12].Value) != "")
                    // Wakamatsu 20170309
                    //TargetRow.Cells[17].Value = DHandling.DecimaltoStr(Convert.ToDecimal(TargetRow.Cells[10].Value) -
                    //                            Convert.ToDecimal(TargetRow.Cells[13].Value), "#,0");
                    TargetRow.Cells[17].Value = MinusConvert(SignConvert(TargetRow.Cells[10].Value) -
                                                SignConvert(TargetRow.Cells[13].Value));
                // Wakamatsu 20170309

                TargetRow.Cells[18].Value = CheckDecimal(Convert.ToString(dr["CarryOverPlanned"]));　                // 繰越予定額        

                // 年度内完工高(本年度受注額 - 繰越予定額)
                if (Convert.ToString(TargetRow.Cells[9].Value) != "" &&
                    Convert.ToString(TargetRow.Cells[18].Value) != "")
                    // Wakamatsu 20170309
                    //TargetRow.Cells[19].Value = DHandling.DecimaltoStr(Convert.ToDecimal(TargetRow.Cells[9].Value) -
                    //                            Convert.ToDecimal(TargetRow.Cells[18].Value), "#,0");
                    TargetRow.Cells[19].Value = MinusConvert(SignConvert(TargetRow.Cells[9].Value) -
                                                SignConvert(TargetRow.Cells[18].Value));
                // Wakamatsu 20170309

                TargetRow.Cells[20].Value = CheckDecimal(Convert.ToString(dr["LastClaim"]));                        // 前年度請求額
                TargetRow.Cells[21].Value = CheckDecimal(Convert.ToString(dr["ThisClaim"]));                        // 本年度請求額

                // 請求額計
                if (Convert.ToString(TargetRow.Cells[20].Value) != "" ||
                    Convert.ToString(TargetRow.Cells[21].Value) != "")
                    TargetRow.Cells[22].Value = CheckDecimal(Convert.ToString(dr["TotalClaim"]));

                TargetRow.Cells[23].Value = CheckDecimal(Convert.ToString(dr["BalanceIncom"]));                     // 未収入金
                TargetRow.Cells[24].Value = CheckDecimal(Convert.ToString(dr["BalanceClaim"]));                     // 残請求額

                // Wakamatsu
                // 未成業務受入金額(請求単月-出来高)
                //if ( Convert.ToString(TargetRow.Cells[12].Value) != "" &&
                //    Convert.ToString(TargetRow.Cells[21].Value) != "")
                //{
                //    TargetRow.Cells[25].Value = (Convert.ToDecimal(TargetRow.Cells[22].Value) -
                //                                Convert.ToDecimal(TargetRow.Cells[13].Value)).ToString("#,0");
                //    if (Convert.ToDecimal(TargetRow.Cells[25].Value) <= 0)
                //        TargetRow.Cells[25].Value = "";
                //}

                // 未成業務受入金額(入金-出来高)
                // Wakamatsu 20170302
                //if (Convert.ToString(TargetRow.Cells[12].Value) != "" &&
                //    Convert.ToString(dr["ThisPaid"]) != "")
                //{
                //    decimal PaidWork = 0;
                //    if (decimal.TryParse(Convert.ToString(dr["TotalPaid"]), out PaidWork))
                //    {
                //        TargetRow.Cells[25].Value = PaidWork - Convert.ToDecimal(TargetRow.Cells[13].Value);
                //        if (Convert.ToDecimal(TargetRow.Cells[25].Value) <= 0)
                //            TargetRow.Cells[25].Value = "";
                //    }
                //}
                TargetRow.Cells[25].Value = CheckDecimal(Convert.ToString(dr["Deposit2"]));
                // Wakamatsu 20170302
                // Wakamatsu

                // 状況
                switch (Convert.ToString(dr["TaskStat"]))
                {
                    case "0":
                        TargetRow.Cells[26].Value = "稼働";
                        break;
                    case "1":
                        TargetRow.Cells[26].Value = "完了";
                        break;
                    case "2":
                        TargetRow.Cells[26].Value = "休止中";
                        break;
                    case "3":
                        TargetRow.Cells[26].Value = "完全完了";
                        break;
                    default:
                        break;
                }

                // 業務内容確認書
                // Wakamatsu
                //TargetRow.Cells[28].Value = CheckDecimal(Convert.ToString(dr["Sales"]));                            // 請負額
                //TargetRow.Cells[28].Value = CheckDecimal(Convert.ToString(dr["Contract"]));                            // 請負額
                //TargetRow.Cells[29].Value = CheckDecimal(Convert.ToString(dr["Budgets"]));                          // 予算額
                // Wakamatsu
                TargetRow.Dispose();
            }
            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
        }

        /// <summary>
        /// SQL作成
        /// </summary>
        /// <returns>作成SQL</returns>
        private string CreateSql()
        {
            string SetSQL1 = "";            // SQL格納用
            string SetSQL2 = "";            // SQL格納用
            string SetSQL3 = "";            // SQL格納用
            // Wakamatsu
            //string SetSQL4 = "";            // SQL格納用
            // Wakamatsu
            string SetSQL5 = "";            // SQL格納用
            string SetSQL6 = "";            // SQL格納用
            string SetSQL7 = "";            // SQL格納用
            // Wakamatsu
            //string MDepartMent = "";        // 部門コード格納用            

            try
            {
                // Wakamatsu
                //SqlHandling sh = new SqlHandling("M_Common");
                //DataTable dt = sh.SpecifiedData("ComSymbol", "ComData",
                //                                "WHERE Kind = 'DEPB' ORDER BY CommonID");
                //if (dt == null)
                //    MDepartMent = "NULL";
                //else
                //{
                //    DataRow dr = dt.Rows[0];                                              // レコード格納用
                //    MDepartMent = Convert.ToString(dr["ComSymbol"]);
                //}
                // Wakamatsu

                // ①・・・D_Volumeから対象業務番号取得
                SetSQL1 += "SELECT OfficeCode, ";
                // Wakamatsu
                //SetSQL1 += "IIF(OfficeCode = 'H', IIF(Department IS NULL, 'NULL', Department), ";
                //SetSQL1 += "'" + MDepartMent + "') AS M_Department, ";
                SetSQL1 += "IIF(Department IS NULL, 'NULL', Department) AS M_Department, ";
                // Wakamatsu
                SetSQL1 += "TaskCode, CarryOverPlanned, BalanceIncom, ";
                // Wakamatsu 20170302
                //SetSQL1 += "BalanceClaim, TaskStat ";
                SetSQL1 += "BalanceClaim, Deposit2, TaskStat ";
                // Wakamatsu 20170302
                SetSQL1 += "FROM D_Volume ";
                SetSQL1 += "WHERE ";
                if (Convert.ToString(this.comboBoxOfficeCode.SelectedValue) != "Dummy")
                {
                    SetSQL1 += "OfficeCode = '" + Convert.ToString(this.comboBoxOfficeCode.SelectedValue) + "' ";
                    SetSQL1 += "AND ";
                }
                if (comboBoxOfficeCode.Text == Sign.HQOffice)
                {
                    SetSQL1 += "Department = " + Convert.ToString(this.comboBoxDepartment.SelectedValue) + " ";
                    SetSQL1 += "AND ";
                }
                SetSQL1 += "YearMonth =  " + CheckDatetTime(Convert.ToString(this.comboBoxMonth.SelectedValue), "yyyyMM") + " ";
                SetSQL1 += "GROUP BY OfficeCode, Department, TaskCode, ";
                // Wakamatsu 20170302
                //SetSQL1 += "CarryOverPlanned, BalanceIncom, BalanceClaim, TaskStat";
                SetSQL1 += "CarryOverPlanned, BalanceIncom, BalanceClaim, Deposit2, TaskStat";
                // Wakamatsu 20170302

                // ②・・・D_Volumeから各年度の必要レコードを取得する
                SetSQL2 += "SELECT TaskCode, OfficeCode, ";
                // Wakamatsu
                //SetSQL2 += "IIF(OfficeCode = 'H', IIF(Department IS NULL, 'NULL', Department), ";
                //SetSQL2 += "'" + MDepartMent + "') AS M_Department, ";
                SetSQL2 += "IIF(Department IS NULL, 'NULL', Department) AS M_Department, ";
                // Wakamatsu
                SetSQL2 += "SUM(MonthlyVolume) AS S_Order, ";
                SetSQL2 += "SUM(IIF(VolUncomp IS NULL, 0, VolUncomp) + ";
                SetSQL2 += "IIF(VolClaimRem IS NULL, 0, VolClaimRem) + ";
                SetSQL2 += "IIF(VolClaim IS NULL, 0, VolClaim)) AS S_Volume, ";
                SetSQL2 += "SUM(MonthlyCost) AS S_Cost, ";
                SetSQL2 += "SUM(MonthlyClaim) AS S_Claim, ";
                SetSQL2 += "SUM(VolPaid) AS S_Paid ";
                SetSQL2 += "FROM D_Volume ";
                SetSQL2 += "WHERE YearMonth BETWEEN {0} AND {1} ";
                SetSQL2 += "GROUP BY TaskCode, OfficeCode, Department";

                // ③・・・原価を取得する
                SetSQL3 += "SELECT TaskCode, OfficeCode, ";
                SetSQL3 += "SUM(Cost) AS S_Cost ";
                SetSQL3 += "FROM D_CostReport ";
                SetSQL3 += "WHERE CONVERT(INT, FORMAT(ReportDate, 'yyyyMM')) ";
                SetSQL3 += "BETWEEN {0} AND {1} ";
                SetSQL3 += "GROUP BY TaskCode, OfficeCode";

                // ④・・・請負額、予算額を取得する
                // Wakamatsu
                //SetSQL4 += "SELECT TEY.TaskCode, PLG.Sales, PLG.Budgets ";
                //SetSQL4 += "FROM D_TaskEntry AS TEY ";
                //SetSQL4 += "LEFT JOIN D_Planning AS PLG ";
                //SetSQL4 += "ON TEY.TaskEntryID = PLG.TaskEntryID";
                // Wakamatsu

                // ⑤・・・各業務番号の最大バージョンを取得する
                SetSQL5 += "SELECT TaskBaseCode, MAX(VersionNo) AS M_No ";
                SetSQL5 += "FROM D_Task ";
                SetSQL5 += "GROUP BY TaskBaseCode";

                // ⑥・・・D_TaskIndから業務担当者コードを取得する
                // Wakamatsu
                //SetSQL6 += "SELECT TaskCode, OfficeCode, ";
                //SetSQL6 += "IIF(OfficeCode = 'H', IIF(Department IS NULL, 'NULL', Department), ";
                //SetSQL6 += "'" + MDepartMent + "') AS M_Department, LeaderMCode ";
                SetSQL6 += "SELECT TaskCode, OfficeCode, ";
                SetSQL6 += "IIF(Department IS NULL, 'NULL', Department) AS M_Department, ";
                SetSQL6 += "LeaderMCode, Contract ";
                // Wakamatsu
                SetSQL6 += "FROM D_TaskInd";

                // 年度取得
                int YearPeriod = Convert.ToDateTime(CheckDatetTime(Convert.ToString(this.comboBoxMonth.SelectedValue), "yyyy/MM/dd")).FisicalYear();

                // ⑥・・・表示データを作成する
                SetSQL7 += "OFI.OfficeName, MTC.TaskCode, TSK.TaskName, PTN.PartnerName, ";
                SetSQL7 += "TSK.StartDate, TSK.EndDate, MEM1.Name AS SalesName, MEM2.Name AS LeaderName, ";
                SetSQL7 += "LVL.S_Order AS LastOrder, TVL.S_Order AS ThisOrder, ";
                SetSQL7 += "(IIF(LVL.S_Order IS NULL, 0, LVL.S_Order) + ";
                SetSQL7 += "IIF(TVL.S_Order IS NULL, 0, TVL.S_Order)) AS TotalOrder, ";
                SetSQL7 += "LVL.S_Volume AS LastVolume, TVL.S_Volume AS ThisVolume, ";
                SetSQL7 += "(IIF(LVL.S_Volume IS NULL, 0, LVL.S_Volume) + ";
                SetSQL7 += "IIF(TVL.S_Volume IS NULL, 0, TVL.S_Volume)) AS TotalVolume, ";
                SetSQL7 += "LCP.S_Cost AS LastCost, TCP.S_Cost AS ThisCost, ";
                SetSQL7 += "(IIF(LCP.S_Cost IS NULL, 0, LCP.S_Cost) + ";
                SetSQL7 += "IIF(TCP.S_Cost IS NULL, 0, TCP.S_Cost)) AS TotalCost, ";
                SetSQL7 += "MTC.CarryOverPlanned, ";
                SetSQL7 += "LVL.S_Claim AS LastClaim, TVL.S_Claim AS ThisClaim, ";
                SetSQL7 += "(IIF(LVL.S_Claim IS NULL, 0, LVL.S_Claim) + ";
                SetSQL7 += "IIF(TVL.S_Claim IS NULL, 0, TVL.S_Claim)) AS TotalClaim, ";
                SetSQL7 += "LVL.S_Paid AS LastPaid, TVL.S_Paid AS ThisPaid, ";
                SetSQL7 += "(IIF(LVL.S_Paid IS NULL, 0, LVL.S_Paid) + ";
                SetSQL7 += "IIF(TVL.S_Paid IS NULL, 0, TVL.S_Paid)) AS TotalPaid, ";
                // Wakamatsu 20170302
                //SetSQL7 += "MTC.BalanceIncom, MTC.BalanceClaim, MTC.TaskStat, ";
                SetSQL7 += "MTC.BalanceIncom, MTC.BalanceClaim, MTC.Deposit2, MTC.TaskStat, ";
                // Wakamatsu 20170302
                // Wakamatsu
                //SetSQL7 += "TPL.Sales, TPL.Budgets, TSK.VersionNo ";
                //SetSQL7 += "FROM ((((((((((((" + SetSQL1 + ") AS MTC ";
                SetSQL7 += "TSK.VersionNo ";
                SetSQL7 += "FROM (((((((((((" + SetSQL1 + ") AS MTC ";
                // Wakamatsu
                SetSQL7 += "LEFT JOIN M_Office AS OFI ";
                SetSQL7 += "ON MTC.OfficeCode = OFI.OfficeCode) ";
                SetSQL7 += "LEFT JOIN D_Task AS TSK ";
                SetSQL7 += "ON RIGHT(MTC.TaskCode,LEN(MTC.TaskCode) - 1) = TSK.TaskBaseCode) ";
                SetSQL7 += "LEFT JOIN M_Partners AS PTN ";
                SetSQL7 += "ON TSK.PartnerCode = PTN.PartnerCode) ";
                SetSQL7 += "LEFT JOIN (" + SetSQL6 + ") AS TSI ";
                SetSQL7 += "ON MTC.TaskCode = TSI.TaskCode ";
                SetSQL7 += "AND MTC.OfficeCode = TSI.OfficeCode ";
                SetSQL7 += "AND MTC.M_Department = TSI.M_Department) ";
                SetSQL7 += "LEFT JOIN M_Members AS MEM1 ";
                SetSQL7 += "ON TSK.SalesMCode = MEM1.MemberCode) ";
                SetSQL7 += "LEFT JOIN M_Members AS MEM2 ";
                SetSQL7 += "ON TSI.LeaderMCode = MEM2.MemberCode) ";
                SetSQL7 += "LEFT JOIN (" + string.Format(SetSQL2,
                                        CheckDatetTime(Convert.ToString(((DataRowView)this.comboBoxMonth.Items[this.comboBoxMonth.Items.Count - 1]).Row[0].ToString()), "yyyyMM"),
                                        CheckDatetTime(Convert.ToString(this.comboBoxMonth.SelectedValue), "yyyyMM")) + ") AS TVL ";
                SetSQL7 += "ON MTC.TaskCode = TVL.TaskCode ";
                SetSQL7 += "AND MTC.OfficeCode = TVL.OfficeCode ";
                SetSQL7 += "AND MTC.M_Department = TVL.M_Department) ";
                SetSQL7 += "LEFT JOIN (" + string.Format(SetSQL2,
                                        Convert.ToString(YearPeriod - 1) + Conv.FisicalYearStartMonth.ToString("00"),
                                        Convert.ToString(YearPeriod) + (Conv.FisicalYearStartMonth - 1).ToString("00")) + ") AS LVL ";
                SetSQL7 += "ON MTC.TaskCode = LVL.TaskCode ";
                SetSQL7 += "AND MTC.OfficeCode = LVL.OfficeCode ";
                SetSQL7 += "AND MTC.M_Department = LVL.M_Department) ";
                // Wakamatsu
                //SetSQL7 += "LEFT JOIN (" + SetSQL4 + ") AS TPL ";
                //SetSQL7 += "ON MTC.TaskCode = TPL.TaskCode) ";
                // Wakamatsu
                SetSQL7 += "LEFT JOIN (" + string.Format(SetSQL3,
                                        CheckDatetTime(Convert.ToString(((DataRowView)this.comboBoxMonth.Items[this.comboBoxMonth.Items.Count - 1]).Row[0].ToString()), "yyyyMM"),
                                        CheckDatetTime(Convert.ToString(this.comboBoxMonth.SelectedValue), "yyyyMM")) + ") AS TCP ";
                SetSQL7 += "ON MTC.TaskCode = TCP.TaskCode ";
                SetSQL7 += "AND MTC.OfficeCode = TCP.OfficeCode) ";
                SetSQL7 += "LEFT JOIN (" + string.Format(SetSQL3,
                                        Convert.ToString(YearPeriod - 1) + Conv.FisicalYearStartMonth.ToString("00"),
                                        Convert.ToString(YearPeriod) + (Conv.FisicalYearStartMonth - 1).ToString("00")) + ") AS LCP ";
                SetSQL7 += "ON MTC.TaskCode = LCP.TaskCode ";
                SetSQL7 += "AND MTC.OfficeCode = LCP.OfficeCode) ";
                SetSQL7 += "LEFT JOIN (" + SetSQL5 + ") AS FLG ";
                SetSQL7 += "ON RIGHT(MTC.TaskCode,LEN(MTC.TaskCode) - 1) = FLG.TaskBaseCode ";
                SetSQL7 += "AND TSK.VersionNo = FLG.M_No ";
                SetSQL7 += "WHERE (NOT TSK.VersionNo IS NULL ";
                SetSQL7 += "AND NOT FLG.M_No IS NULL) ";
                SetSQL7 += "OR TSK.VersionNo IS NULL ";
                // Wakamatsu 20170301
                //SetSQL7 += "ORDER BY OFI.OfficeCode, MTC.TaskCode";
                SetSQL7 += "ORDER BY MTC.TaskStat, OFI.OfficeCode, MTC.TaskCode";
                // Wakamatsu 20170301

                return SetSQL7;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 20190131 asakawa Add
        /// 
        /// 出来高データから
        /// 「受注単月金額」、「出来高未成業務金額」、「出来高未請求金額」、「出来高請求金額」、「単月原価金額」の計を
        /// SELECT算出するためのSQL編集
        /// </summary>
        /// <param name="TaskCode">業務番号</param>
        /// <returns>
        /// string sql
        /// </returns>
        private DataRow GetDataLastYear(DataRow sdr)
        {

            decimal tmpd;

            string sql = "";
            // 現在の年度取得
            int YearPeriod = Convert.ToDateTime(CheckDatetTime(Convert.ToString(this.comboBoxMonth.SelectedValue), "yyyy/MM/dd")).FisicalYear();


            sql += "SUM(MonthlyVolume) AS L_MonthlyVolume, ";
            sql += "SUM(VolUncomp) AS L_VolUncomp, SUM(VolClaimRem) AS L_VolClaimRem, SUM(VolClaim) AS L_VolClaim, ";
            sql += "SUM(MonthlyCost) AS L_MonthlyCost FROM D_Volume ";
            sql += "WHERE (TaskCode = '" + Convert.ToString(sdr["TaskCode"]) + "') AND ";
            sql += "(YearMonth BETWEEN " + Convert.ToString((YearPeriod - 1) * 100 + 7) + " AND " + Convert.ToString(YearPeriod * 100 + 6) + ")";

            if (Convert.ToString(this.comboBoxOfficeCode.SelectedValue) != "Dummy")
            {
                sql += " AND (OfficeCode = '" + Convert.ToString(this.comboBoxOfficeCode.SelectedValue) + "')";
            }
            if (comboBoxOfficeCode.Text == Sign.HQOffice)
            {
                sql += " AND (Department = '" + Convert.ToString(this.comboBoxDepartment.SelectedValue) + "')";
            }

            SqlHandling sh = new SqlHandling();                 // SQL実行クラス
            DataTable dt = sh.SelectFullDescription(sql);       // SQL実行
            if (dt == null) return sdr;
           
            DataRow ddr = dt.Rows[0];                           // レコード格納用
            
            sdr["LastOrder"] = ddr["L_MonthlyVolume"];          // 前年度受注額


            //sdr["LastVolume"] = Convert.ToDecimal(ddr["L_VolUncomp"]) + Convert.ToDecimal(ddr["L_VolClaimRem"]) + Convert.ToDecimal(ddr["L_VolClaim"]);                       // 前年度出来高
            tmpd = 0;

            if (ddr["L_VolUncomp"] != DBNull.Value)
                tmpd += Convert.ToDecimal(ddr["L_VolUncomp"]);
            if (ddr["L_VolClaimRem"] != DBNull.Value)
                tmpd += Convert.ToDecimal(ddr["L_VolClaimRem"]);
            if (ddr["L_VolClaim"] != DBNull.Value)
                tmpd += Convert.ToDecimal(ddr["L_VolClaim"]);
            sdr["LastVolume"] = tmpd;                       // 前年度出来高


            sdr["LastCost"] = ddr["L_MonthlyCost"];              // 前年度原価

            return sdr;
        }


        /// <summary>
        /// DateTime型確認(フォーマット設定付き)
        /// </summary>                                         
        /// <param name="CheckTarget">確認対象文字列</param>
        /// <param name="SetFormat">設定フォーマット</param>
        /// <returns>フォーマット設定後文字列</returns>
        private string CheckDatetTime(string CheckTarget, string SetFormat)
        {
            DateTime CheckResult = DateTime.MinValue;

            if (DateTime.TryParse(CheckTarget, out CheckResult))
                return CheckResult.ToString(SetFormat);
            else
                return "";
        }

        /// <summary>
        /// Decimal型確認(フォーマット設定付き)
        /// </summary>
        /// <param name="CheckTarget">確認対象文字列</param>
        /// <returns>フォーマット設定後文字列</returns>
        private string CheckDecimal(string CheckTarget)
        {
            decimal CheckResult = 0;

            if (decimal.TryParse(CheckTarget, out CheckResult))
            {
                // Wakamatsu 20170309
                //return CheckResult.ToString("#,0");
                if (CheckResult < 0)
                    return "△" + (CheckResult * -1).ToString("#,0");
                else
                    return CheckResult.ToString("#,0");
                // Wakamatsu 20170309
            }
            else
                return "";
        }

        /// <summary>
        /// Excel出力
        /// </summary>
        private void ExcelExport()
        {
            // Wakamatsu 20170301
            //PrintOut.PublishVolume publ = new PrintOut.PublishVolume(Folder.DefaultExcelTemplate("LedgerAggregate.xlsx"));
            //publ.CreateExcelForPdf("LedgerAggregate", new PublishData(), this.dataGridViewList);
            PrintOut.PublishVolume publ = new PrintOut.PublishVolume(Folder.DefaultExcelTemplate("業務台帳.xlsx"));
            publ.ExcelFile("LedgerAggregate", new PublishData(), this.dataGridViewList);
            // Wakamatsu 20170301
        }

        // Wakamatsu 20170308
        private string MinusConvert(object TargetValue)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "-" → "△"コンバート
                Decimal.TryParse(WorkString, out WorkDecimal);
                if (WorkDecimal < 0)
                    return "△" + (WorkDecimal * -1).ToString("#,0");
                else
                    return WorkDecimal.ToString("#,0");
            }
            return WorkDecimal.ToString("#,0");
        }

        private decimal  SignConvert(object TargetValue)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "△" → "-"コンバート
                if (WorkString.Substring(0, 1) == "△")
                {
                    Decimal.TryParse(WorkString.Substring(1), out WorkDecimal);
                    return WorkDecimal * -1;
                }
                else
                {
                    Decimal.TryParse(WorkString, out WorkDecimal);
                    return WorkDecimal;
                }
            }
            return 0;
        }
        // Wakamatsu 20170308
    }
    }

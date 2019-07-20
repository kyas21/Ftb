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

namespace CostProc
{
    public partial class FormCostSummaryCond : Form
    {
        //--------------------------------------------------------------------//
        //      Field
        //--------------------------------------------------------------------//
        FormCostSummary formCostSummary = null;
        CostInfoIFOp cif;
        private bool iniPro = true;

        private ComboBox[] cbClass;
        private ComboBox[] cbItem;
        private ComboBox[] cbItemFR;
        private ComboBox[] cbItemTO;
        private Label[] lblTilde;
        private Label[] lblSubT;
        //--------------------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------------------//
        public FormCostSummaryCond()
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------//
        //      Property
        //--------------------------------------------------------------------//

        //--------------------------------------------------------------------//
        //      Method
        //--------------------------------------------------------------------//
        private void FormCostSummayCond_Load(object sender, EventArgs e)
        {
            cif = new CostInfoIFOp();
            createArray_Controls();

            create_cbOffice();

            dateTimePickerSOP.Value = System.DateTime.Today;
            dateTimePickerEOP.Value = System.DateTime.Today;
            create_cbClassification();
            create_cbRangeItem();
        }


        private void FormCostSummayCond_Shown(object sender, EventArgs e)
        {
            iniPro = false;
            setPreData();
        }


        // 表示画面へ遷移
        private void buttonOK_Click(object sender, EventArgs e)
        {

            //日付チェック
            if (!check_DateConsistency(dateTimePickerEOP, dateTimePickerSOP))
                return;

            //分類項目のチェック
            if (!check_ClassificationItem())
                return;

            if (!check_RangeComboBox())
                return;

            if (!check_FromToComboBox())
                return;

            if (formCostSummary == null || formCostSummary.IsDisposed)
            {
                labelGetData.Visible = true;
                labelGetData.Location = new Point(347, 132);
                labelGetData.Size = new Size(400, 200);
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

                cif.DateSOP = dateTimePickerSOP.Value;
                cif.DateEOP = dateTimePickerEOP.Value;
                cif.WherePhraseDate = "Where ( ReportDate >= '" + cif.DateSOP + "' AND ReportDate <= '" + cif.DateEOP + "')";
                // Wakamatsu 20170316
                //cif.WherePhraseDate = cif.WherePhraseDate + " AND D_CR.OfficeCode = '" + comboBoxOffice.SelectedValue + "'";
                cif.WherePhraseDate = cif.WherePhraseDate + " AND OfficeCode = '" + comboBoxOffice.SelectedValue + "'";
                // Wakamatsu 20170316
                cif.Class0 = Convert.ToString(cbClass[0].SelectedValue);
                cif.Class1 = Convert.ToString(cbClass[1].SelectedValue);
                cif.Class2 = Convert.ToString(cbClass[2].SelectedValue);
                string[] item = new string[] { cif.Item0, cif.Item1, cif.Item2 };
                string[] itemFR = new string[] { cif.ItemFR0, cif.ItemFR1, cif.ItemFR2 };
                string[] itemTO = new string[] { cif.ItemTO0, cif.ItemTO1, cif.ItemTO2 };
                DateTime[] dateFR = new DateTime[] { cif.DateFR0, cif.DateFR1, cif.DateFR2 };
                DateTime[] dateTO = new DateTime[] { cif.DateTO0, cif.DateTO1, cif.DateTO2 };

                // Wakamatsu 20170316
                //string LeaderMCode = " CASE WHEN LEN(ISNULL(D_CR.LeaderMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.LeaderMCode, '')), 3) ELSE ISNULL(D_CR.LeaderMCode, '') END ";
                //string SalesMCode = " CASE WHEN LEN(ISNULL(D_CR.SalesMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.SalesMCode, '')), 3) ELSE ISNULL(D_CR.SalesMCode, '') END ";
                string LeaderMCode = " CASE WHEN LEN(ISNULL(D_C.LeaderMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_C.LeaderMCode, '')), 3) ELSE RTRIM(ISNULL(D_C.LeaderMCode, '')) END ";
                string SalesMCode = " CASE WHEN LEN(ISNULL(D_C.SalesMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_C.SalesMCode, '')), 3) ELSE RTRIM(ISNULL(D_C.SalesMCode, '')) END ";
                // Wakamatsu 20170316

                // Wakamatsu 20170316
                string SetSQL1 = "";
                string SetSQL2 = "";
                string SetSQL3 = "";
                // Wakamatsu 20170316

                for (int i = 0; i < item.Length; i++)
                {
                    item[i] = Convert.ToString(cbItem[i].SelectedValue);

                    itemFR[i] = Convert.ToString(cbItemFR[i].SelectedValue);
                    itemTO[i] = Convert.ToString(cbItemTO[i].SelectedValue);

                    switch (item[i])
                    {
                        case "0"://指定なし
                            break;
                        case "1"://業務番号
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (TaskCode  >= '" + itemFR[i].Trim() + "' AND TaskCode <= '" + itemTO[i].Trim() + "')";
                            break;
                        case "2"://顧客
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (D_T.PartnerCode  >= '" + itemFR[i].Trim() + "' AND D_T.PartnerCode <= '" + itemTO[i].Trim() + "')";
                            break;
                        case "3"://作業項目
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (ItemCode  >= '" + itemFR[i].Trim() + "' AND ItemCode <= '" + itemTO[i].Trim() + "')";
                            break;
                        case "4"://業務担当者
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND ("+ LeaderMCode + " >= '" + itemFR[i].Trim() + "' AND " + LeaderMCode + " <= '" + itemTO[i].Trim() + "')";
                            break;
                        case "5"://営業担当者
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND ("+ SalesMCode + " >= '" + itemFR[i].Trim() + "' AND "+ SalesMCode + " <= '" + itemTO[i].Trim() + "')";
                            break;
                        default:
                            break;
                    }
                }

                //分類項目
                // Wakamatsu 20170316
                //if (cif.Class0 == "3" && cif.Class1 == "1" && cif.Class2 == "4")//"作業項目", "業務番号", "業務担当者"
                //{
                //    cif.SqlStr = " D_CR.TaskCode AS TaskCode, D_CR.ItemCode AS ItemCode, D_CR.UnitPrice AS UnitPrice, SUM(D_CR.Quantity) AS Quantity, SUM(D_CR.Cost) AS Cost, ";
                //    cif.SqlStr = cif.SqlStr + " SUBSTRING(D_CR.TaskCode, 2, 6) AS TaskBaseCode, D_T.PartnerCode AS CustoCode, M_C.CostCode AS CostCode, M_C.Item AS Item ,";
                //    cif.SqlStr = cif.SqlStr + LeaderMCode + " AS LeaderMCode, ";
                //    cif.SqlStr = cif.SqlStr + SalesMCode + " AS SalesMCode ";
                //    cif.SqlStr = cif.SqlStr + " FROM D_CostReport AS D_CR ";
                //    cif.SqlStr = cif.SqlStr + " LEFT JOIN M_Cost AS M_C ON D_CR.ItemCode = M_C.CostCode AND D_CR.OfficeCode = M_C.OfficeCode ";
                //    cif.SqlStr = cif.SqlStr + " LEFT JOIN D_Task AS D_T ON D_T.TaskBaseCode = SUBSTRING(D_CR.TaskCode, 2, 6) ";
                //    cif.SqlStr = cif.SqlStr + cif.WherePhraseDate + " GROUP BY TaskCode,ItemCode,M_C.Item,UnitPrice,LeaderMCode,D_CR.SalesMCode,M_C.CostCode,D_CR.OfficeCode,Department,D_T.PartnerCode ";
                //    cif.SqlStr = cif.SqlStr + " ORDER BY LeaderMCode,ItemCode,TaskCode,D_CR.OfficeCode,Department";
                //}
                // D_CostReportから対象データを取得する
                SetSQL1 += "SELECT RTRIM(D_C.TaskCode) AS TaskCode, RTRIM(D_T.TaskName) AS TaskName, ";
                SetSQL1 += "RTRIM(D_C.ItemCode) AS ItemCode, D_C.UnitPrice, D_C.Quantity, D_C.Cost, ";
                SetSQL1 += "RTRIM(D_C.OfficeCode) AS OfficeCode, " + LeaderMCode + " AS LeaderMCode, ";
                SetSQL1 += SalesMCode + " AS SalesMCode, RTRIM(D_T.PartnerCode) AS PartnerCode, ";
                SetSQL1 += "RTRIM(M_P.PartnerName) AS PartnerName ";
                SetSQL1 += "FROM (D_CostReport AS D_C ";
                SetSQL1 += "LEFT JOIN D_Task AS D_T ";
                SetSQL1 += "ON SUBSTRING(D_C.TaskCode, 2, 6) = D_T.TaskBaseCode) ";
                SetSQL1 += "LEFT JOIN M_Partners AS M_P ";
                SetSQL1 += "ON D_T.PartnerCode = M_P.PartnerCode ";
                SetSQL1 += cif.WherePhraseDate;

                // 対象データに対して必要情報を付与する
                SetSQL2 += "SELECT '(' + D_CR.TaskCode + ')' + D_CR.TaskName AS DispTask, ";
                SetSQL2 += "D_CR.PartnerName, '(' + D_CR.PartnerCode + ')' + D_CR.PartnerName AS KeyPartner, ";
                SetSQL2 += "'(' + D_CR.ItemCode + ')' + M_C.Item AS DispItem, ";
                SetSQL2 += "M_M1.Name AS DispLeader, '(' + D_CR.LeaderMCode + ')' + M_M1.Name AS KeyLeader, ";
                SetSQL2 += "M_M2.Name AS DispSales, '(' + D_CR.SalesMCode + ')' + M_M2.Name AS KeySales, ";
                SetSQL2 += "D_CR.UnitPrice, D_CR.Quantity, D_CR.Cost ";
                SetSQL2 += "FROM (((" + SetSQL1 + ") AS D_CR ";
                SetSQL2 += "LEFT JOIN M_Cost AS M_C ";
                SetSQL2 += "ON D_CR.ItemCode = M_C.CostCode ";
                SetSQL2 += "AND D_CR.OfficeCode = M_C.OfficeCode) ";
                SetSQL2 += "LEFT JOIN M_Members AS M_M1 ";
                SetSQL2 += "ON D_CR.LeaderMCode = M_M1.MemberCode) ";
                SetSQL2 += "LEFT JOIN M_Members AS M_M2 ";
                SetSQL2 += "ON D_CR.SalesMCode = M_M2.MemberCode";

                string[] TargetString = new string[3] { cif.Class0, cif.Class1, cif.Class2 };
                bool UnitFlag = false;

                SetSQL1 = "";
                SetSQL3 = "";
                for (int i = 0; i < TargetString.Length; i++)
                {
                    switch (TargetString[i])
                    {
                        case "1":       // "業務番号"
                            SetSQL1 += "DispTask AS Class" + i + ", ";
                            SetSQL1 += "DispTask AS Class" + i + "Disp, ";
                            SetSQL3 += "DispTask, ";
                            break;
                        case "2":       // "得意先"
                            SetSQL1 += "KeyPartner AS Class" + i + ", ";
                            SetSQL1 += "PartnerName AS Class" + i + "Disp, ";
                            SetSQL3 += "PartnerName, KeyPartner, ";
                            break;
                        case "3":       // "原価項目"
                            SetSQL1 += "DispItem AS Class" + i + ", ";
                            SetSQL1 += "DispItem AS Class" + i + "Disp, ";
                            SetSQL3 += "DispItem, ";
                            UnitFlag = true;
                            break;
                        case "4":       // "業務担当者"
                            SetSQL1 += "KeyLeader AS Class" + i + ", ";
                            SetSQL1 += "DispLeader AS Class" + i + "Disp, ";
                            SetSQL3 += "DispLeader, KeyLeader, ";
                            break;
                        case "5":       // "営業担当者"
                            SetSQL1 += "KeySales AS Class" + i + ", ";
                            SetSQL1 += "DispSales AS Class" + i + "Disp, ";
                            SetSQL3 += "DispSales, KeySales, ";
                            break;
                        default:        // "指定なし"
                            SetSQL1 += "'' AS Class" + i + ", ";
                            SetSQL1 += "'' AS Class" + i + "Disp, ";
                            break;
                    }
                }
                // 末尾のカンマ削除
                if (SetSQL3.Length > 2)
                    if (SetSQL3.Substring(SetSQL3.Length - 2) == ", ")
                        SetSQL3 = SetSQL3.Substring(0, SetSQL3.Length - 2);
                // 原価項目が対象の時のみ単価を取得
                if (UnitFlag == true)
                    SetSQL1 += "UnitPrice, ";
                else
                    SetSQL1 += "0 AS UnitPrice, ";

                SetSQL1 += "SUM(Quantity) AS Quantity, SUM(Cost) AS Cost ";
                cif.SqlStr = " " + SetSQL1 + "FROM (" + SetSQL2 + ") AS MS ";
                if (SetSQL3 != "")
                    cif.SqlStr += "GROUP BY " + SetSQL3;
                if (UnitFlag == true)
                    cif.SqlStr += ", UnitPrice ";
                else
                    cif.SqlStr += " ";
                if (SetSQL3 != "")
                    cif.SqlStr += "ORDER BY " + SetSQL3;
                // Wakamatsu 20170316

                cif.Office = comboBoxOffice.Text;

                //分類項目
                cif.ClassificationItem = "【" + comboBoxClass0.Text + "】" + "毎" + "【" + comboBoxClass1.Text + "】" + "別" + "【" + comboBoxClass2.Text + "】" + "別集計";

                //出力範囲
                cif.OutputRange = "項目1：【" + comboBoxItem0.Text + "】";
                //cif.OutputRange = "【項目1：" + comboBoxItem0.Text + "】";
                if (comboBoxItem0.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom0.Text + "  ～  " + comboBoxTo0.Text;
                }
                cif.OutputRange += System.Environment.NewLine;

                cif.OutputRange = cif.OutputRange + "項目2：【" + comboBoxItem1.Text + "】";
                //cif.OutputRange = cif.OutputRange + "【項目2：" + comboBoxItem1.Text + "】";
                if (comboBoxItem1.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom1.Text + "  ～  " + comboBoxTo1.Text;
                }
                cif.OutputRange += System.Environment.NewLine;

                cif.OutputRange = cif.OutputRange + "項目3：【" + comboBoxItem2.Text + "】";
                //cif.OutputRange = cif.OutputRange + "【項目3：" + comboBoxItem2.Text + "】";
                if (comboBoxItem2.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom2.Text + "  ～  " + comboBoxTo2.Text;
                }

                formCostSummary = new FormCostSummary(cif);
                formCostSummary.Show();
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                labelGetData.Visible = false;
            }
            else
            {
                MessageBox.Show("すでにこのプログラムは開始されています。");
            }
        }

        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }

        // カレンダの日付選択がされたときの処理
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            DateTimePicker dtp = (DateTimePicker)sender;

            if (dtp.Name == "dateTimePickerEOP")
            {
                check_DateConsistency(dtp, dateTimePickerSOP);
                return;
            }
        }

        // ComboBoxが操作された時の処理
        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;

            ComboBox cbb = (ComboBox)sender;
            switch (cbb.Name)
            {
                // Wakamatsu 20170316
                case "comboBoxClass0":
                    if (check_ClassificationItem() == false)
                        this.comboBoxClass0.SelectedIndex = 0;
                    break;
                case "comboBoxClass1":
                    if (check_ClassificationItem() == false)
                        this.comboBoxClass1.SelectedIndex = 0;
                    break;
                // Wakamatsu 20170316
                case "comboBoxClass2":
                    // Wakamatsu 20170316
                    //check_ClassificationItem();
                    if (check_ClassificationItem() == false)
                        this.comboBoxClass2.SelectedIndex = 0;
                    // Wakamatsu 20170316
                    break;
                case "comboBoxItem0":
                    // Wakamatsu 20170316
                    //edit_RangeComboBox(0);
                    if (edit_RangeComboBox(0) == false)
                        this.comboBoxItem0.SelectedIndex = 0;
                    // Wakamatsu 20170316
                    break;
                case "comboBoxItem1":
                    // Wakamatsu 20170316
                    //edit_RangeComboBox(1);
                    if (edit_RangeComboBox(1) == false)
                        this.comboBoxItem1.SelectedIndex = 0;
                    // Wakamatsu 20170316
                    break;
                case "comboBoxItem2":
                    // Wakamatsu 20170316
                    //edit_RangeComboBox(2);
                    if (edit_RangeComboBox(2) == false)
                        this.comboBoxItem2.SelectedIndex = 0;
                    // Wakamatsu 20170316
                    break;
                case "comboBoxTo0":
                    check_FromToComboBox();
                    break;
                case "comboBoxTo1":
                    check_FromToComboBox();
                    break;
                case "comboBoxTo2":
                    check_FromToComboBox();
                    break;
                case "comboBoxOffice":
                    comboBoxItem0.SelectedIndex = 0;
                    comboBoxItem1.SelectedIndex = 0;
                    comboBoxItem2.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        //--------------------------------------------------------------------//
        //     SubRoutine
        //--------------------------------------------------------------------//
        // コントロールをプログラミングの効率化を目的として配列化する
        private void createArray_Controls()
        {
            this.cbClass = new ComboBox[] { this.comboBoxClass0, this.comboBoxClass1,this.comboBoxClass2 };
            this.cbItem = new ComboBox[] { this.comboBoxItem0, this.comboBoxItem1, this.comboBoxItem2 };
            this.cbItemFR = new ComboBox[] { this.comboBoxFrom0, this.comboBoxFrom1, this.comboBoxFrom2 };
            this.cbItemTO = new ComboBox[] { this.comboBoxTo0, this.comboBoxTo1, this.comboBoxTo2 };
            this.lblTilde = new Label[] { this.labelTilde0, this.labelTilde1, this.labelTilde2 };
            this.lblSubT = new Label[] { this.labelSub0, this.labelSub1, this.labelSub2 };

            for (int i = 0; i < cbItemFR.Length; i++) invisible_AllRangeArea(i);
        }

        // ComboBox作成
        // 作成方法の2項目を編集
        private void create_cbClassification()
        {
            ComboBoxEdit cbe;

            for (int i = 0; i < cbClass.Length; i++)
            {
                cbe = new ComboBoxEdit(cbClass[i]);
                cbe.ValueItem = cif.VItemArray2;
                cbe.DisplayItem = cif.DItemArray2;
                cbe.Basic();
            }
        }


        // 出力範囲の項目1から項目3を編集
        private void create_cbRangeItem()
        {
            ComboBoxEdit cbe;
            for (int i = 0; i < cbItem.Length; i++)
            {
                cbe = new ComboBoxEdit(cbItem[i]);
                cbe.ValueItem = cif.VItemArray2;
                cbe.DisplayItem = cif.DItemArray2;
                cbe.Basic();
            }
        }


        // 出力範囲、範囲指定欄のComboBox,dateTimePicker（カレンダ）を編集し表示する
        // Wakamatsu 20170316
        //private void edit_RangeComboBox(int idx)
        private bool edit_RangeComboBox(int idx)
        // Wakamatsu 20170316
        {
            invisible_AllRangeArea(idx);
            //if (Convert.ToInt32(cbItem[idx].SelectedValue) == 0) return;        // 指定なし
            if (Convert.ToInt32(cbItem[idx].SelectedValue) == 0) return true;        // 指定なし

            int selectValue = Convert.ToInt32(cbItem[idx].SelectedValue);
            string msg = "「出力範囲」";
            switch (idx)
            {
                case 0:
                    if ((selectValue == Convert.ToInt32(cbItem[1].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目２や項目３と同一項目の選択はできません。");
                        // Wakamatsu 20170316
                        //return;
                        return false;
                    }
                    break;
                case 1:
                    if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目１や項目３と同一項目の選択はできません。");
                        // Wakamatsu 20170316
                        //return;
                        return false;
                    }
                    break;
                case 2:
                    if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[1].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目１や項目２と同一項目の選択はできません。");
                        // Wakamatsu 20170316
                        //return;
                        return false;
                    }
                    break;
                default:
                    break;
            }

            lblSubT[idx].Visible = true;
            lblTilde[idx].Visible = true;
            cbItemTO[idx].Visible = true;
            cbItemFR[idx].Visible = true;

            // "指定なし", "原価計上日", "業務番号", "顧客", "作業項目", "業務担当者", "営業担当者"
            int tiNo = Convert.ToInt32(cbItem[idx].SelectedValue);
            SqlHandling sh = new SqlHandling("D_CostReport");

            string strSql = "";
            DataTable dt;

            if (tiNo == 4)
            {
                strSql = "DISTINCT CASE WHEN LEN( ISNULL( " + cif.TItemArray[tiNo] + ", '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(" + cif.TItemArray[tiNo] + ", '')), 3) ELSE ISNULL (" + cif.TItemArray[tiNo] + ", '') END AS LeaderMCode";
            }
            else if (tiNo == 5)
            {
                strSql = "DISTINCT CASE WHEN LEN( ISNULL( " + cif.TItemArray[tiNo] + ", '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(" + cif.TItemArray[tiNo] + ", '')), 3) ELSE ISNULL (" + cif.TItemArray[tiNo] + ", '') END AS SalesMCode";
            }
            else
            {
                strSql = "DISTINCT ISNULL ( " + cif.TItemArray[tiNo] + ", '') AS " + cif.TItemArray[tiNo];
            }

            dt = sh.SelectFullDescription(strSql
                + " FROM D_CostReport WHERE " + cif.TItemArray[tiNo] + " <> '' AND OfficeCode = '" + comboBoxOffice.SelectedValue.ToString() + "'"
                + " ORDER BY " + cif.TItemArray[tiNo]);

            if (dt == null)
            {
                MessageBox.Show("選択対象となるべきデータがありません。");
                cbItem[idx].SelectedValue = "0";
                invisible_RangeLabel(idx);
                // Wakamatsu 20170316
                //return;
                return false;
            }

            ComboBoxEdit cbe = new ComboBoxEdit(cbItemFR[idx]);
            ComboBoxEdit cbe1 = new ComboBoxEdit(cbItemTO[idx]);
            cbe.ValueItem = new string[dt.Rows.Count];
            cbe.DisplayItem = new string[dt.Rows.Count];
            cbe1.ValueItem = new string[dt.Rows.Count];
            cbe1.DisplayItem = new string[dt.Rows.Count];


            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                cbe.ValueItem[i] = Convert.ToString(dr[cif.TItemArray[tiNo]]);
                cbe1.ValueItem[i] = cbe.ValueItem[i];
            }

            if (tiNo == 1) //業務番号
            {
                sh = new SqlHandling();
                string sqlStr = "DISTINCT T.TaskName, T.CostType FROM D_Task AS T INNER JOIN D_TaskInd AS I "
                        + "ON T.TaskID = I.TaskID WHERE I.OfficeCode = '" + comboBoxOffice.SelectedValue.ToString() + "' AND "
                        + " I.TaskCode = '";
                for (int i = 0; i < cbe.ValueItem.Length; i++)
                {
                    dt = sh.SelectFullDescription(sqlStr + cbe.ValueItem[i] + "'");
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        dr = dt.Rows[0];
                        cbe.DisplayItem[i] = "(" + cbe.ValueItem[i] + ") "
                                            + "(" + Convert.ToString(dr["CostType"]) + ") "
                                            + Convert.ToString(dr["TaskName"]);
                        cbe1.DisplayItem[i] = cbe.DisplayItem[i];
                    }
                    // Wakamatsu 20170316
                    else
                    {
                        cbe.DisplayItem[i] = "(" + cbe.ValueItem[i] + ") ";
                        cbe1.DisplayItem[i] = cbe.DisplayItem[i];
                    }
                    // Wakamatsu 20170316
                }
            }
            else
            {
                cbe.DisplayItem = edit_ComboBoxValueItem(tiNo, cbe.ValueItem);
                cbe1.DisplayItem = cbe.DisplayItem;
            }
            cbe.Basic();
            cbe1.Basic();

            // Wakamatsu 20170316
            return true;
        }


        private string[] edit_ComboBoxValueItem(int vidx, string[] vItem)
        {
            string[] dItem = new string[vItem.Length];
            SqlHandling sh = new SqlHandling(cif.DITableArray[vidx]);
            string sqlStr = " WHERE " + cif.DItmKeyArray[vidx] + " = '";
            string sqlStrOfficeCode = "";
            if (vidx == 1 || vidx == 3)//業務コードと原価項目の場合、部署コードを条件に追加
                sqlStrOfficeCode = " AND OfficeCode = '" + comboBoxOffice.SelectedValue.ToString() + "'";

            DataTable dt;
            DataRow dr;
            string Item = "";
            for (int i = 0; i < vItem.Length; i++)
            {
                Item = vItem[i].Trim();
                dt = sh.SelectAllData(sqlStr + Item + "'" + sqlStrOfficeCode);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    dr = dt.Rows[0];
                    dItem[i] = Convert.ToString(dr[cif.DItmNamArray[vidx]]);
                }
                // Wakamatsu 20170316
                else
                    dItem[i] = "(" + Item + ")";
                // Wakamatsu 20170316
            }
            return dItem;
        }


        // 出力範囲の範囲指定をすべて非表示に変更する
        private void invisible_AllRangeArea(int idx)
        {
            cbItemFR[idx].Visible = false;
            cbItemTO[idx].Visible = false;
            invisible_RangeLabel(idx);
        }


        private void invisible_RangeLabel(int idx)
        {
            lblTilde[idx].Visible = false;
            lblSubT[idx].Visible = false;
        }


        // 日付の整合性検査
        // 日付はFROM-TOで指定するのでFROM>TOは不整合とする
        private bool check_DateConsistency(DateTimePicker dtpEnd, DateTimePicker dtpSta)
        {
            if (dtpEnd.Value < dtpSta.Value)
            {
                MessageBox.Show("「作成条件」左の日付より以前の日付は指定できません。");
                return false;
            }
            return true;
        }


        private bool check_ClassificationItem()
        {
            string msg = "「作成方法」の組み合わせが不適切です。選択しなおしてください";
            // Wakamatsu 20170316
            int[] TargetSel = new int[3] { Convert.ToInt32(comboBoxClass0.SelectedValue),
                                            Convert.ToInt32(comboBoxClass1.SelectedValue),
                                            Convert.ToInt32(comboBoxClass2.SelectedValue) };
            int ZeroCount = 0;          // "指定なし"カウント用

            for (int i = 0; i < TargetSel.Length; i++)
                if (TargetSel[i] == 0)
                    ZeroCount++;
            if (ZeroCount <= 1)
            {
                //if ((Convert.ToInt32(comboBoxClass0.SelectedValue) == Convert.ToInt32(comboBoxClass1.SelectedValue)) ||
                //    (Convert.ToInt32(comboBoxClass0.SelectedValue) == Convert.ToInt32(comboBoxClass2.SelectedValue)) ||
                //    (Convert.ToInt32(comboBoxClass1.SelectedValue) == Convert.ToInt32(comboBoxClass2.SelectedValue)))//同じ項目同士の組み合わせ
                if (TargetSel[0] == TargetSel[1] || TargetSel[0] == TargetSel[2] || TargetSel[1] == TargetSel[2])//同じ項目同士の組み合わせ
                {
                    MessageBox.Show(msg);
                    return false;
                }
            }
            // Wakamatsu 20170316
            return true;
        }


        private bool check_RangeComboBox()
        {
            int selectValue = Convert.ToInt32(cbItem[0].SelectedValue);
            string msg = "「出力範囲」";
            if (selectValue != 0)
            {
                if ((selectValue == Convert.ToInt32(cbItem[1].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                {
                    MessageBox.Show(msg + "項目２や項目３と同一項目の選択はできません。");
                    return false;
                }
            }

            selectValue = Convert.ToInt32(cbItem[1].SelectedValue);
            if (selectValue != 0)
            {
                if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                {
                    MessageBox.Show(msg + "項目１や項目３と同一項目の選択はできません。");
                    return false;
                }
            }

            selectValue = Convert.ToInt32(cbItem[2].SelectedValue);
            if (selectValue != 0)
            {
                if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[1].SelectedValue)))
                {
                    MessageBox.Show(msg + "項目１や項目２と同一項目の選択はできません。");
                    return false;
                }
            }
            return true;
        }

        private bool check_FromToComboBox()
        {
            string msg = "範囲の大小関係が正しくありません。";
            string msg1 = "「出力範囲」";


            if (comboBoxFrom0.Visible)
            {
                if ((comboBoxFrom0.SelectedIndex == -1) || (comboBoxTo0.SelectedIndex == -1))
                    return false;
                if (comboBoxFrom0.SelectedIndex > comboBoxTo0.SelectedIndex)
                {
                    msg = msg1 + "「項目1」の" + msg;
                    MessageBox.Show(msg);
                    return false;
                }
            }

            if (comboBoxFrom1.Visible)
            {
                if ((comboBoxFrom1.SelectedIndex == -1) || (comboBoxTo1.SelectedIndex == -1))
                    return false;
                if (comboBoxFrom1.SelectedIndex > comboBoxTo1.SelectedIndex)
                {
                    msg = msg1 + "「項目2」の" + msg;
                    MessageBox.Show(msg);
                    return false;
                }
            }

            if (comboBoxFrom2.Visible)
            {
                if ((comboBoxFrom2.SelectedIndex == -1) || (comboBoxTo2.SelectedIndex == -1))
                    return false;
                if (comboBoxFrom2.SelectedIndex > comboBoxTo2.SelectedIndex)
                {
                    msg = msg1 + "「項目3」の" + msg;
                    MessageBox.Show(msg);
                    return false;
                }
            }
            return true;
        }

        private void buttonCancel_Click( object sender, EventArgs e )
        {
            if ( comboBoxOffice != null ) CostSummary.Default.Office = Convert.ToString( comboBoxOffice.SelectedValue );

            CostSummary.Default.SOP = dateTimePickerSOP.Value.ToString( "yyyy,MM,dd" );
            CostSummary.Default.EOP = dateTimePickerEOP.Value.ToString( "yyyy,MM,dd" );

            if ( comboBoxClass0 != null ) CostSummary.Default.Class0 = Convert.ToString( comboBoxClass0.SelectedValue );
            if ( comboBoxClass1 != null ) CostSummary.Default.Class1 = Convert.ToString( comboBoxClass1.SelectedValue );
            if ( comboBoxClass2 != null ) CostSummary.Default.Class2 = Convert.ToString( comboBoxClass2.SelectedValue );

            if ( comboBoxItem0 != null )
            {
                CostSummary.Default.Item0 = Convert.ToString( comboBoxItem0.SelectedValue );
                CostSummary.Default.From0 = Convert.ToString( comboBoxFrom0.SelectedValue );
                CostSummary.Default.To0 = Convert.ToString( comboBoxTo0.SelectedValue );
            }

            if ( comboBoxItem1 != null )
            {
                CostSummary.Default.Item1 = Convert.ToString( comboBoxItem1.SelectedValue );
                CostSummary.Default.From1 = Convert.ToString( comboBoxFrom1.SelectedValue );
                CostSummary.Default.To1 = Convert.ToString( comboBoxTo1.SelectedValue );
            }

            if ( comboBoxItem2 != null )
            {
                CostSummary.Default.Item2 = Convert.ToString( comboBoxItem2.SelectedValue );
                CostSummary.Default.From2 = Convert.ToString( comboBoxFrom2.SelectedValue );
                CostSummary.Default.To2 = Convert.ToString( comboBoxTo2.SelectedValue );
            }

            CostSummary.Default.Save();
            this.Close();
        }


        private void setPreData()
        {
            //CostDetail.Default.Reset();

            string[] dArray = new string[3];
            if ( !string.IsNullOrEmpty( CostSummary.Default.Office ) )
                comboBoxOffice.SelectedValue = CostSummary.Default.Office;

            if ( !string.IsNullOrEmpty( Convert.ToString( CostSummary.Default.SOP ) ) )
            {
                dArray = ( CostSummary.Default.SOP ).Split( ',' );
                dateTimePickerSOP.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if ( !string.IsNullOrEmpty( Convert.ToString( CostSummary.Default.EOP ) ) )
            {
                dArray = ( CostSummary.Default.EOP ).Split( ',' );
                dateTimePickerEOP.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if ( !string.IsNullOrEmpty( CostSummary.Default.Class0 ) )
                comboBoxClass0.SelectedValue = CostSummary.Default.Class0;

            if ( !string.IsNullOrEmpty( CostSummary.Default.Class1 ) )
                comboBoxClass1.SelectedValue = CostSummary.Default.Class1;

            if ( !string.IsNullOrEmpty( CostSummary.Default.Class2 ) )
                comboBoxClass2.SelectedValue = CostSummary.Default.Class2;

            if ( !string.IsNullOrEmpty( CostSummary.Default.Item0 ) )
            {
                comboBoxItem0.SelectedValue = CostSummary.Default.Item0;

                if ( !string.IsNullOrEmpty( CostSummary.Default.From0 ) )
                    comboBoxFrom0.SelectedValue = CostSummary.Default.From0;

                if ( !string.IsNullOrEmpty( CostSummary.Default.To0 ) )
                    comboBoxTo0.SelectedValue = CostSummary.Default.To0;
            }

            if ( !string.IsNullOrEmpty( CostSummary.Default.Item1 ) )
            {
                comboBoxItem1.SelectedValue = CostSummary.Default.Item1;

                if ( !string.IsNullOrEmpty( CostSummary.Default.From1 ) )
                    comboBoxFrom1.SelectedValue = CostSummary.Default.From1;

                if ( !string.IsNullOrEmpty( CostSummary.Default.To1 ) )
                    comboBoxTo1.SelectedValue = CostSummary.Default.To1;
            }

            if ( !string.IsNullOrEmpty( CostSummary.Default.Item2 ) )
            {
                comboBoxItem2.SelectedValue = CostSummary.Default.Item2;

                if ( !string.IsNullOrEmpty( CostSummary.Default.From2 ) )
                    comboBoxFrom2.SelectedValue = CostSummary.Default.From2;

                if ( !string.IsNullOrEmpty( CostSummary.Default.To2 ) )
                    comboBoxTo2.SelectedValue = CostSummary.Default.To2;
            }



        }
       





    }
}

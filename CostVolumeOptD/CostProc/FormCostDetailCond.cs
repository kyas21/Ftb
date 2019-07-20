/*
 * Change history
 * 
 * 20190519 anonymous 出力範囲に部門追加
 * 
 * 20190527 anonymous 初期値設定追加
 * 
 * 20190622 anonymous 部コードの判定対象とソート対象をCostReportにする
 * Department → D_CR.Department
 * 
 */

using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostProc
{
    public partial class FormCostDetailCond : Form
    {
        //--------------------------------------------------------------------//
        //      Field
        //--------------------------------------------------------------------//
        FormCostDetail formCostDetail = null;
        CostInfoIFOp cif;
        private bool iniPro = true;

        private ComboBox[] cbClass;
        private ComboBox[] cbItem;
        private ComboBox[] cbItemFR;
        private ComboBox[] cbItemTO;
        private Label[] lblTilde;
        private Label[] lblSubT;

        const string HQOffice = "本社";
        //--------------------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------------------//
        public FormCostDetailCond()
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------//
        //      Property
        //--------------------------------------------------------------------//

        //--------------------------------------------------------------------//
        //      Method
        //--------------------------------------------------------------------//
        private void FormCostDetailCond_Load(object sender, EventArgs e)
        {
            cif = new CostInfoIFOp();

            createArray_Controls();

            create_cbOffice();

            dateTimePickerSOP.Value = System.DateTime.Today;
            dateTimePickerEOP.Value = System.DateTime.Today;
            create_cbClassification();
            create_cbRangeItem();
        }


        private void FormCostDetailCond_Shown(object sender, EventArgs e)
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

            if (formCostDetail == null || formCostDetail.IsDisposed)
            {
                labelGetData.Visible = true;
                labelGetData.Location = new Point(347, 132);
                labelGetData.Size = new Size(400, 200);
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

                cif.DateSOP = dateTimePickerSOP.Value;
                cif.DateEOP = dateTimePickerEOP.Value;
                cif.WherePhraseDate = "Where ( ReportDate >= '" + cif.DateSOP + "' AND ReportDate <= '" + cif.DateEOP + "')";
                cif.WherePhraseDate = cif.WherePhraseDate + " AND D_CR.OfficeCode = '" + comboBoxOffice.SelectedValue + "'";
                cif.Class0 = Convert.ToString(cbClass[0].SelectedValue);
                cif.Class1 = Convert.ToString(cbClass[1].SelectedValue);
                string[] item = new string[] { cif.Item0, cif.Item1, cif.Item2 };
                string[] itemFR = new string[] { cif.ItemFR0, cif.ItemFR1, cif.ItemFR2 };
                string[] itemTO = new string[] { cif.ItemTO0, cif.ItemTO1, cif.ItemTO2 };
                DateTime[] dateFR = new DateTime[] { cif.DateFR0, cif.DateFR1, cif.DateFR2 };
                DateTime[] dateTO = new DateTime[] { cif.DateTO0, cif.DateTO1, cif.DateTO2 };

                string LeaderMCode = " CASE WHEN LEN(ISNULL(D_CR.LeaderMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.LeaderMCode, '')), 3) ELSE ISNULL(D_CR.LeaderMCode, '') END ";
                string SalesMCode = " CASE WHEN LEN(ISNULL(D_CR.SalesMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.SalesMCode, '')), 3) ELSE ISNULL(D_CR.SalesMCode, '') END ";

                for (int i = 0;i < item.Length; i++)
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
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (" + LeaderMCode + "  >= '" + itemFR[i].Trim() + "' AND " + LeaderMCode + " <= '" + itemTO[i].Trim() + "')";
                            break;
                        case "5"://営業担当者
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (" + SalesMCode + " >= '" + itemFR[i].Trim() + "' AND " + SalesMCode + " <= '" + itemTO[i].Trim() + "')";
                            break;
                        // 20190518 anonymous Add
                        case "6"://部門
                            // 20190622 anonymous Change
                            //cif.WherePhraseDate = cif.WherePhraseDate + " AND (Department >= '" + itemFR[i].Trim() + "' AND Department <= '" + itemTO[i].Trim() + "')";
                            cif.WherePhraseDate = cif.WherePhraseDate + " AND (D_CR.Department >= '" + itemFR[i].Trim() + "' AND D_CR.Department <= '" + itemTO[i].Trim() + "')";
                            break;
                        default:
                            break;
                    }
                }

                // 20190622 anonymous Change "Department" → "D_CR.Department"
                //分類項目指定なし
                if (cif.Class0 == "0" && cif.Class1 == "0")//"指定なし", "指定なし"
                {
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ReportDate";
                }
                else if (cif.Class0 == "0" && cif.Class1 == "2") //"指定なし", "業務番号""
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY TaskCode,D_CR.OfficeCode,Department,ReportDate";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY TaskCode,D_CR.OfficeCode,D_CR.Department,ReportDate";
                }
                else if (cif.Class0 == "0" && cif.Class1 == "4")//"指定なし", "作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ItemCode,D_CR.OfficeCode,Department,ReportDate";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ItemCode,D_CR.OfficeCode,D_CR.Department,ReportDate";
                }
                else if (cif.Class0 == "1" && cif.Class1 == "2")//"原価計上日", "業務番号"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ReportDate,TaskCode,D_CR.OfficeCode,Department";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ReportDate,TaskCode,D_CR.OfficeCode,D_CR.Department";
                }
                else if (cif.Class0 == "1" && cif.Class1 == "4")//"原価計上日", "作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ReportDate,ItemCode,D_CR.OfficeCode,Department";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ReportDate,ItemCode,D_CR.OfficeCode,D_CR.Department";
                }
                else if (cif.Class0 == "2" && cif.Class1 == "4")//"業務番号","作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY TaskCode,ItemCode,D_CR.OfficeCode,Department,ReportDate";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY TaskCode,ItemCode,D_CR.OfficeCode,D_CR.Department,ReportDate";
                }
                else if (cif.Class0 == "3" && cif.Class1 == "2")//"顧客", "業務番号"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY CustoCode,TaskCode,D_CR.OfficeCode,Department,ReportDate,ItemCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY CustoCode,TaskCode,D_CR.OfficeCode,D_CR.Department,ReportDate,ItemCode";
                }
                else if (cif.Class0 == "3" && cif.Class1 == "4")//"顧客", "作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY CustoCode,ItemCode,D_CR.OfficeCode,Department,ReportDate,TaskCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY CustoCode,ItemCode,D_CR.OfficeCode,D_CR.Department,ReportDate,TaskCode";
                }
                else if (cif.Class0 == "4" && cif.Class1 == "2")//"作業項目", "業務番号"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ItemCode,TaskCode,D_CR.OfficeCode,Department,ReportDate";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY ItemCode,TaskCode,D_CR.OfficeCode,D_CR.Department,ReportDate";
                }
                else if (cif.Class0 == "5" && cif.Class1 == "2")//"業務担当者", "業務番号"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY LeaderMCode,TaskCode,D_CR.OfficeCode,Department,ReportDate,ItemCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY LeaderMCode,TaskCode,D_CR.OfficeCode,D_CR.Department,ReportDate,ItemCode";
                }
                else if (cif.Class0 == "5" && cif.Class1 == "4")//"業務担当者", "作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY LeaderMCode,ItemCode,D_CR.OfficeCode,Department,ReportDate,TaskCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY LeaderMCode,ItemCode,D_CR.OfficeCode,D_CR.Department,ReportDate,TaskCode";
                }
                else if (cif.Class0 == "6" && cif.Class1 == "2")//"営業担当者","業務番号"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY SalesMCode,TaskCode,D_CR.OfficeCode,Department,ReportDate,ItemCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY SalesMCode,TaskCode,D_CR.OfficeCode,D_CR.Department,ReportDate,ItemCode";
                }
                else if (cif.Class0 == "6" && cif.Class1 == "4")//"営業担当者","作業項目"
                {
                    //cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY SalesMCode,ItemCode,D_CR.OfficeCode,Department,ReportDate,TaskCode";
                    cif.WherePhraseDate = cif.WherePhraseDate + " ORDER BY SalesMCode,ItemCode,D_CR.OfficeCode,D_CR.Department,ReportDate,TaskCode";
                }

                cif.Office = comboBoxOffice.Text;

                //分類項目
                cif.ClassificationItem = "【" + comboBoxClass0.Text + "】" + "毎" + "【" + comboBoxClass1.Text + "】" + "別明細";

                //出力範囲
                cif.OutputRange = "項目1：【" + comboBoxItem0.Text + "】";
                if (comboBoxItem0.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom0.Text + "  ～  " + comboBoxTo0.Text;
                }
                cif.OutputRange += System.Environment.NewLine;

                cif.OutputRange = cif.OutputRange + "項目2：【" + comboBoxItem1.Text + "】";
                if (comboBoxItem1.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom1.Text + "  ～  " + comboBoxTo1.Text;
                }
                cif.OutputRange += System.Environment.NewLine;

                cif.OutputRange = cif.OutputRange + "項目3：【" + comboBoxItem2.Text + "】";
                if (comboBoxItem2.SelectedIndex != 0)
                {
                    cif.OutputRange = cif.OutputRange + comboBoxFrom2.Text + "  ～  " + comboBoxTo2.Text;
                }

                formCostDetail = new FormCostDetail(cif);
                formCostDetail.Show();
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

            // 20190527 anonymous add
            cif.RangeArraySet(comboBoxOffice.SelectedValue.ToString());
            create_cbRangeItem();
            // 20190527 anonymous add end
        }

        // カレンダの日付選択がされたときの処理
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            DateTimePicker dtp = (DateTimePicker)sender;

            // 2018.01 asakawa 操作性を向上させるための改善のため「return」で終了、それ以降は未到達処理のためコメントアウト
            return;
            //if (dtp.Name == "dateTimePickerEOP")
            //{
            //    check_DateConsistency(dtp, dateTimePickerSOP);
            //    return;
            //}
            // 2018.01 asakawa //
        }

        // ComboBoxが操作された時の処理
        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            // 2018.01 asakawa 出力範囲を編集する都度警告画面が出て操作しにくいため改善

            if (iniPro) return;

            ComboBox cbb = (ComboBox)sender;
            switch (cbb.Name)
            {
                case "comboBoxClass1":
                    check_ClassificationItem();
                    break;
                case "comboBoxItem0":
                    edit_RangeComboBox(0);
                    break;
                case "comboBoxItem1":
                    edit_RangeComboBox(1);
                    break;
                case "comboBoxItem2":
                    edit_RangeComboBox(2);
                    break;
                case "comboBoxTo0":
                    // 2018.01 asakawa 以下１行削除
                    // check_FromToComboBox();
                    break;
                case "comboBoxTo1":
                    // 2018.01 asakawa 以下１行削除
                    // check_FromToComboBox();
                    break;
                case "comboBoxTo2":
                    // 2018.01 asakawa 以下１行削除
                    // check_FromToComboBox();
                    break;
                case "comboBoxOffice":
                    // 20190519 anonymous add
                    cif.RangeArraySet(comboBoxOffice.SelectedValue.ToString());
                    create_cbRangeItem();
                    // 20190519 anonymous add end

                    comboBoxItem0.SelectedIndex = 0;
                    comboBoxItem1.SelectedIndex = 0;
                    comboBoxItem2.SelectedIndex = 0;
                    break;

                //2018.01 asakawa 操作性向上のために以下３つのcaseを追加
                case "comboBoxFrom0":
                    comboBoxTo0.SelectedValue = comboBoxFrom0.SelectedValue;
                    break;
                case "comboBoxFrom1":
                    comboBoxTo1.SelectedValue = comboBoxFrom1.SelectedValue;
                    break;
                case "comboBoxFrom2":
                    comboBoxTo2.SelectedValue = comboBoxFrom2.SelectedValue;
                    break;
                // 2018.01 asakawa 追加はここまで

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
            this.cbClass = new ComboBox[] { this.comboBoxClass0, this.comboBoxClass1 };
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
            ComboBoxEdit cbe = new ComboBoxEdit(cbClass[0]);
            cbe.ValueItem = cif.VItemArray0;
            cbe.DisplayItem = cif.DItemArray0;
            cbe.Basic();

            cbe = new ComboBoxEdit(cbClass[1]);
            cbe.ValueItem = cif.VItemArray1;
            cbe.DisplayItem = cif.DItemArray1;
            cbe.Basic();
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
        private void edit_RangeComboBox(int idx)
        {
            invisible_AllRangeArea(idx);
            if (Convert.ToInt32(cbItem[idx].SelectedValue) == 0) return;        // 指定なし
            string msg = "「出力範囲」";
            int selectValue = Convert.ToInt32(cbItem[idx].SelectedValue);
            switch (idx)
            {
                case 0:
                    if ((selectValue == Convert.ToInt32(cbItem[1].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目２や項目３と同一項目の選択はできません。");
                        return;
                    }
                    break;
                case 1:
                    if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[2].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目１や項目３と同一項目の選択はできません。");
                        return;
                    }
                    break;
                case 2:
                    if ((selectValue == Convert.ToInt32(cbItem[0].SelectedValue)) || (selectValue == Convert.ToInt32(cbItem[1].SelectedValue)))
                    {
                        MessageBox.Show(msg + "項目１や項目２と同一項目の選択はできません。");
                        return;
                    }
                    break;
                default:
                    break;
            }

            lblSubT[idx].Visible = true;
            lblTilde[idx].Visible = true;
            cbItemTO[idx].Visible = true;
            cbItemFR[idx].Visible = true;

            // "指定なし", "原価計上日", "業務番号", "顧客", "原価項目", "業務担当者", "営業担当者"
            int tiNo = Convert.ToInt32(cbItem[idx].SelectedValue);
            SqlHandling sh = new SqlHandling("D_CostReport");

            string strSql = "";
            DataTable dt;
            if (tiNo == 4)      //業務担当者
            {
                strSql = "DISTINCT CASE WHEN LEN( ISNULL( " + cif.TItemArray[tiNo] + ", '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(" + cif.TItemArray[tiNo] + ", '')), 3) ELSE ISNULL (" + cif.TItemArray[tiNo] + ", '') END AS LeaderMCode";
            }
            else if (tiNo == 5) //営業担当者
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
                return;
            }

            ComboBoxEdit cbe = new ComboBoxEdit(cbItemFR[idx]);
            ComboBoxEdit cbe1 = new ComboBoxEdit(cbItemTO[idx]);
            cbe.ValueItem = new string[dt.Rows.Count];
            cbe.DisplayItem = new string[dt.Rows.Count];
            cbe1.ValueItem = new string[dt.Rows.Count];
            cbe1.DisplayItem = new string[dt.Rows.Count];


            DataRow dr;
            for (int i = 0;i < dt.Rows.Count; i++)
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
                }
            }
            else
            {
                // 20190519 anonymous add
                if (tiNo == 6)
                {
                    for (int i = 0; i < cbe.ValueItem.Length; i++)
                    {                      
                        cbe.DisplayItem[i] = Conv.DepartName(comboBoxOffice.SelectedValue.ToString(), cbe.ValueItem[i]);
                    }
                }
                else
                {
                    cbe.DisplayItem = edit_ComboBoxValueItem(tiNo, cbe.ValueItem);
                }
                //cbe.DisplayItem = edit_ComboBoxValueItem(tiNo, cbe.ValueItem);
                
                // 20190519 anonymous add end
                cbe1.DisplayItem = cbe.DisplayItem;
            }
            cbe.Basic();
            cbe1.Basic();
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
            if (Convert.ToInt32(comboBoxClass0.SelectedValue) == Convert.ToInt32(comboBoxClass1.SelectedValue))//同じ項目同士の組み合わせ
            {
                if ((Convert.ToInt32(comboBoxClass0.SelectedValue) != 0) && (Convert.ToInt32(comboBoxClass1.SelectedValue) != 0))
                {
                    MessageBox.Show(msg);
                    return false;
                }
            }
            else
            {
                if ((Convert.ToInt32(comboBoxClass0.SelectedValue) != 0) && (Convert.ToInt32(comboBoxClass1.SelectedValue) == 0))//第二項目が指定なしだった時の組み合わせ
                {
                    MessageBox.Show(msg);
                    return false;
                }
            }
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {   
            if(comboBoxOffice != null) CostDetail.Default.Office = Convert.ToString(comboBoxOffice.SelectedValue);

            CostDetail.Default.SOP = dateTimePickerSOP.Value.ToString("yyyy,MM,dd");
            CostDetail.Default.EOP = dateTimePickerEOP.Value.ToString("yyyy,MM,dd");

            if(comboBoxClass0 != null) CostDetail.Default.Class0 = Convert.ToString(comboBoxClass0.SelectedValue);
            if(comboBoxClass1 != null) CostDetail.Default.Class1 = Convert.ToString(comboBoxClass1.SelectedValue);

            if(comboBoxItem0 != null )
            {
                CostDetail.Default.Item0 = Convert.ToString(comboBoxItem0.SelectedValue);
                CostDetail.Default.From0 = Convert.ToString(comboBoxFrom0.SelectedValue);
                CostDetail.Default.To0 = Convert.ToString(comboBoxTo0.SelectedValue);
            }

            if( comboBoxItem1 != null )
            {
                CostDetail.Default.Item1 = Convert.ToString( comboBoxItem1.SelectedValue );
                CostDetail.Default.From1 = Convert.ToString( comboBoxFrom1.SelectedValue );
                CostDetail.Default.To1 = Convert.ToString( comboBoxTo1.SelectedValue );
            }

            if( comboBoxItem2 != null )
            {
                CostDetail.Default.Item2 = Convert.ToString( comboBoxItem2.SelectedValue );
                CostDetail.Default.From2 = Convert.ToString( comboBoxFrom2.SelectedValue );
                CostDetail.Default.To2 = Convert.ToString( comboBoxTo2.SelectedValue );
            }

            CostDetail.Default.Save();
            this.Close();
        }


        private void setPreData()
        {
            //CostDetail.Default.Reset();

            string[] dArray = new string[3];
            if( !string.IsNullOrEmpty( CostDetail.Default.Office) )
                comboBoxOffice.SelectedValue = CostDetail.Default.Office;

            if( !string.IsNullOrEmpty( Convert.ToString( CostDetail.Default.SOP ) ) )
            {
                dArray = ( CostDetail.Default.SOP ).Split( ',' );
                dateTimePickerSOP.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if( !string.IsNullOrEmpty( Convert.ToString( CostDetail.Default.EOP ) ) )
            {
                dArray = ( CostDetail.Default.EOP ).Split( ',' );
                dateTimePickerEOP.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if( !string.IsNullOrEmpty( CostDetail.Default.Class0) )
                comboBoxClass0.SelectedValue = CostDetail.Default.Class0;

            if( !string.IsNullOrEmpty( CostDetail.Default.Class1) )
                comboBoxClass1.SelectedValue = CostDetail.Default.Class1;

            if( !string.IsNullOrEmpty( CostDetail.Default.Item0) )
            {
                comboBoxItem0.SelectedValue = CostDetail.Default.Item0;

                if( !string.IsNullOrEmpty( CostDetail.Default.From0 ) )
                    comboBoxFrom0.SelectedValue = CostDetail.Default.From0;

                if( !string.IsNullOrEmpty( CostDetail.Default.To0 ) )
                    comboBoxTo0.SelectedValue = CostDetail.Default.To0;
            }

            if( !string.IsNullOrEmpty( CostDetail.Default.Item1) )
            {
                comboBoxItem1.SelectedValue = CostDetail.Default.Item1;

                if( !string.IsNullOrEmpty( CostDetail.Default.From1) )
                    comboBoxFrom1.SelectedValue = CostDetail.Default.From1;

                if( !string.IsNullOrEmpty( CostDetail.Default.To1) )
                    comboBoxTo1.SelectedValue = CostDetail.Default.To1;
            }

            if( !string.IsNullOrEmpty( CostDetail.Default.Item2) )
            {
                comboBoxItem2.SelectedValue = CostDetail.Default.Item2;

                if( !string.IsNullOrEmpty( CostDetail.Default.From2) )
                    comboBoxFrom2.SelectedValue = CostDetail.Default.From2;

                if( !string.IsNullOrEmpty( CostDetail.Default.To2) )
                    comboBoxTo2.SelectedValue = CostDetail.Default.To2;
           }

            

        }
    }
}

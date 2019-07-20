using ClassLibrary;
using PrintOut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FormContract : Form
    {

        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        TaskEntryData ted;

        OsResultsData[] ord;
        OutsourceData [] osd;
        private bool iniPro = true;
        private int iniRCnt = 29;
        private int cpg = 0;            // Current Page Number
        private string[] itemsArray;
        private string[] vPartnerAry;
        const string BookName = "外注清算書(請負).xlsx";
        const string SheetName = "OsAContract";
        private DateTime nowDate;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormContract()
        {
            InitializeComponent();
        }


        public FormContract(TaskEntryData ted)
        {
            InitializeComponent();
            this.ted = ted;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormContract_Load(object sender, EventArgs e)
        {
            UiHandling uih = new UiHandling(dataGridView1);
            uih.DgvReadyNoRHeader();
            uih.NoSortable();
            dataGridView1.Rows.Add(iniRCnt);

            create_cbOffice();                      // 事業所comboBox
            comboBoxOffice.Text = ted.OfficeName;
            create_cbWork();                        // 部門ComboBox
            comboBoxWork.Text = ted.DepartName;

            create_dtPicker();

            edit_Labels();

            initialViewSetting( 0 );

            nowMonth();

            // 指定月の実績値を外注作業実績データ(D_OsWkReport)から実績値を得る
        }


        private void FormContract_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }


        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            if (ord == null) return;
            DateTimePicker dtp = (DateTimePicker)sender;
            switch (dtp.Name)
            {
                case "dateTimePickerRecordedDate":
                    dateTimePickerRecordedDate.Value = (dateTimePickerRecordedDate.Value).EndOfMonth();
                    break;
                default:
                    break;
            }

        }


        private void button_Click(object sender, EventArgs e)
        {
            nowMonth();
            if (iniPro) return;

            Button btn = (Button)sender;
            BillingOp blo = new BillingOp();

            switch (btn.Name)
            {
                case "buttonFromOutsource":
                    Func<DialogResult> dialogNewLoad = DMessage.DialogNewLoad;
                    if (dialogNewLoad() == DialogResult.No) return;
                    loadOutsourceData(cpg);
                    buttonFromOutsource.Enabled = false;
                    labelMsg.Text = "外注内訳データを取り込みました。";
                    break;

                case "buttonPrevData":
                    if (cpg > 0)
                    {
                        cpg--;
                        viewData(cpg);
                    }
                    break;

                case "buttonNextData":
                    if (cpg != (ord.Length - 1))
                    {
                        cpg++;
                        viewData(cpg);
                    }
                    break;

                case "buttonCopyAndNext":
                    clearOsResultsContAmount(dataGridView1);
                    OsResultsData[] newOrd = new OsResultsData[ord.Length + 1];
                    Array.Copy(ord, newOrd, Math.Min(ord.Length, newOrd.Length));
                    newOrd[newOrd.Length - 1] = (OsResultsData)ord[cpg].Clone();
                    newOrd[newOrd.Length - 1].OsResultsID = 0;
                    DateTime wkdt = newOrd[newOrd.Length - 1].RecordedDate.AddMonths(1);
                    newOrd[newOrd.Length - 1].RecordedDate = wkdt;

                    ord = newOrd;

                    dateTimePickerRecordedDate.Value = wkdt.EndOfMonth();
                    nowMonth( wkdt );

                    buttonGrpDisabled();
                    buttonPrevData.Enabled = true;
                    buttonNew.Enabled = true;

                    cpg++;
                    break;

                case "buttonClose":
                    this.Close();
                    break;

                case "buttonReCalc":
                    reCalculateAll(dataGridView1);
                    break;

                case "buttonOverWrite":
                    Func<DialogResult> dialogOverWrite = DMessage.DialogOverWrite;
                    if (dialogOverWrite() == DialogResult.No) return;

                    storeDateToOsResultsData(ord[cpg]);
                    blo.UpdateOsResults(ord[cpg]);
                    blo.DeleteAndInsertOsResultsCont(dataGridView1, ord[cpg]);
                    labelMsg.Text = "上書しました。";
                    break;

                case "buttonNew":
                    if (ord == null || ord[cpg].OsResultsID < 1)
                    {
                        if (ord == null)
                        {
                            ord = new OsResultsData[1];
                            cpg = 0;
                        }

                        editOsResultsData( cpg );
                        ord[cpg].OsResultsID = blo.InsertOsResults(ord[cpg]);
                        if( ord[cpg].OsResultsID < 1 )
                        {
                            labelMsg.Text = "新規書込みできませんでした。";
                            return;
                        }
                    }
                    storeDateToOsResultsData(ord[cpg]);
                    blo.InsertOsResultsCont(dataGridView1, ord[cpg]);
                    initialViewSetting( cpg );
                    labelMsg.Text = "新規書込みしました。";
                    break;

                case "buttonDelete":
                    Func<DialogResult> dialogDelete = DMessage.DialogDelete;
                    if (dialogDelete() == DialogResult.No) return;
                    blo.DeleteOsResultsCont(ord[cpg].OsResultsID);
                    blo.DeleteOsResults(ord[cpg].OsResultsID);
                    cpg--;
                    initialViewSetting( cpg );
                    labelMsg.Text = "削除しました。";
                    break;

                case "buttonCancel":
                    Func<DialogResult> dialogCancel = DMessage.DialogCancel;
                    if (dialogCancel() == DialogResult.No) return;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(iniRCnt);
                    break;

                case "buttonPrint":
                    createExcelFile("外注清算書(請負).xlsx", "OsAContract");
                    break;

                default:
                    break;
            }
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // DataGridView [ItemCode]コード列が非表示になっている←注意
            DataGridView dgv = (DataGridView)sender;
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.Tab:
                    if (dgv.CurrentCellAddress.X == 0) SendKeys.Send("{RIGHT}");
                    if (dgv.CurrentCellAddress.X == 5)
                    {
                        for (int i = 0;i<3;i++) SendKeys.Send("{RIGHT}");
                    }
                    if (dgv.CurrentCellAddress.X == 9) SendKeys.Send("{LEFT}");
                    break;
                case Keys.Left:
                    if (dgv.CurrentCellAddress.X == 2) SendKeys.Send("{RIGHT}");
                    if (dgv.CurrentCellAddress.X == 9)
                    {
                        for(int i = 0;i<3;i++) SendKeys.Send("{LEFT}");
                    }
                    break;
                default:
                    break;
            }

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            switch (e.KeyCode)
            {
                case Keys.A:
                    break;
                case Keys.C:
                    Clipboard.SetDataObject(dgv.GetClipboardContent());
                    break;
                case Keys.I:
                case Keys.D:
                    lineNumbering(dgv);
                    break;
                case Keys.R:
                    //reCalculateAll();
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;   // 初期化中

            DataGridView dgv = (DataGridView)sender;
            
            switch (e.ColumnIndex)
            {
                case 7:     // 「数量」列
                case 9:     // 「数量」列
                case 11:     // 「数量」列
                case 13:     // 「数量」列
                    reCalculateAll(dgv);
                    break;
                default:
                    break;
            }
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;   // 初期化中

            ComboBox cbx = (ComboBox)sender;

            if (cbx.Name == "comboBoxOffice") create_cbWork();
            switch (cbx.Name)
            {
                case "comboBoxOffice":
                case "comboBoxWork":
                    if (loadExistingOsResultsData())
                    {
                        cpg = 0;
                        loadOsResultsContData(ord[cpg]);
                        edit_lblPageNo();
                    }
                    break;
                default:
                    break;

            }
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // Label編集
        private void edit_lblPageNo()
        {
            if( ord == null ) return;
            labelPageNo.Text =  ( cpg + 1 ).ToString() + " / " + ord.Length.ToString();
        }


        private void edit_Labels()
        {
            labelTaskCode.Text = (ted.TaskCode == "") ? "" : ted.TaskCode;
            labelTask.Text = ted.TaskName;
            if (ted.PartnerName == "")
            {
                PartnersData pd = new PartnersData();
                labelOPartner.Text = pd.SelectPartnerName(ted.PartnerCode);

            }
            else
            {
                labelOPartner.Text = ted.PartnerName;
            }
            MembersData md = new MembersData();
            labelSalesM.Text = md.SelectMemberName(ted.SalesMCode);
            labelLeader.Text = md.SelectMemberName(ted.LeaderMCode);
            labelPartner.Text = "";
            labelOrderNo.Text = "";
        }


        private void edit_ResultsLabels(int idx)
        {
            if (ord == null) return;
            PartnersData pd = new PartnersData();
            labelPartner.Text = pd.SelectPartnerName(ord[idx].PartnerCode);
            labelOrderNo.Text = ord[idx].OrderNo;
            labelPartnerCode.Text = ord[idx].PartnerCode;
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }


        // 部門
        private void create_cbWork()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxWork);
            cbe.DepartmentList((comboBoxOffice.Text == Sign.HQOffice) ? "DEPH" : "DEPB");
        }


        // 業者名(下請け業者のみ）
        private void create_cbPartner()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxPartner);
            cbe.TableData("M_Partners", "PartnerCode", "PartnerName", " WHERE RelSubco = '1'");
            vPartnerAry = new string[cbe.ValueItem.Length];
            Array.Copy(cbe.ValueItem, 0, vPartnerAry, 0, vPartnerAry.Length);
        }


        private void create_dtPicker()
        {
            dateTimePickerRecordedDate.Value = (DateTime.Today).EndOfMonth();
            nowDate = dateTimePickerRecordedDate.Value;
        }


        private void nowMonth()
        {
            labelMsg.Text = Convert.ToString( nowDate.Month ) + "月分を処理します";
        }

        private void nowMonth(DateTime ymd)
        {
            labelMsg.Text = Convert.ToString( ymd.Month ) + "月分を処理します";
        }


        private void buttonGrpDisabled()
        {
            buttonOverWrite.Enabled = false;
            buttonPrevData.Enabled = false;
            buttonNextData.Enabled = false;
            buttonCopyAndNext.Enabled = false;
        }


        private void buttonGrpEnabled()
        {
            buttonOverWrite.Enabled = true;
            buttonPrevData.Enabled = (ord.Length > 1) ? true : false;
            buttonNextData.Enabled = (ord.Length > 1) ? true : false;
            buttonCopyAndNext.Enabled = true;
        }


        private void initialViewSetting( int idx )
        {
            if( loadExistingOsResultsData() )
            {
                if( idx < 0 ) idx = 0;
                if( idx > ord.Length ) idx = 0;

                loadOsResultsContData( ord[idx] );
                viewOsResultsData( idx );
                edit_lblPageNo();
                buttonGrpEnabled();
                buttonNew.Enabled = false;
            }
            else
            {
                buttonGrpDisabled();
                buttonNew.Enabled = true;
            }
        }

        // 既存データの読取表示
        private bool loadExistingOsResultsData()
        {
            if (ted.TaskCode == null) return false;
            // 選択された業務でその部署のデータすべてを処理対象とする。
            BillingOp blo = new BillingOp();
            //string wParam = " WHERE TaskCode = '" + ted.TaskCode +"'"
            //                + " AND OfficeCode = '" + ted.OfficeCode + "' AND Department = '" + ted.Department + "'"
            //                + " AND ContractForm = 0";
            string wParam = " WHERE TaskEntryID = " + ted.TaskEntryID  + " AND ContractForm = 0";
            DataTable dt = blo.UsingParamater_Select("D_OsResults", wParam);
            if (dt == null) return false;
            if (!(dt.Rows.Count > 0)) return false;
            ord = new OsResultsData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) ord[i] = new OsResultsData(dt.Rows[i]);
            return true;
        }


        private bool loadOsResultsContData(OsResultsData ord)
        {
            if (ord.OsResultsID == 0) return false;
            //対象となる「項目」（Item）を抽出
            BillingOp blo = new BillingOp();
            //string sqlStr = "DISTINCT Item,* FROM D_OsResultsCont WHERE OsResultsID = " + Convert.ToString(ord.OsResultsID) ;
            string sqlStr = "* FROM D_OsResultsCont WHERE OsResultsID = " + Convert.ToString(ord.OsResultsID) 
                          + " AND RecordedDate = '" + ord.RecordedDate + "' AND Subject = '*'";
            DataTable dt = blo.UsingSqlstring_Select(sqlStr);
            if (dt == null) return false;
            if (dt.Rows.Count > iniRCnt) dataGridView1.Rows.Add(dt.Rows.Count - iniRCnt);    // GridView行不足分追加

            DataRow dr;
            itemsArray = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                //viewOsResultsContToDgv(ord.OsResultsID, Convert.ToString(dr["Item"]), dataGridView1.Rows[i]);
                viewOsResultsContToDgv(ord, Convert.ToString( dr["Item"] ), dataGridView1.Rows[i]);
            }
            return true;
        }


        private void loadOutsourceData(int idx)
        {
            // 下請け業者コードがある状態では、その外注内訳書データのみ得る。
            //                   ない状態では、そのタスクエントリ、部署の外注内訳書データ全件を得る。
            string wParam = " WHERE TaskEntryID = " + ted.TaskEntryID 
                            + " AND OfficeCode = '" + Convert.ToString(comboBoxOffice.SelectedValue) + "'" 
                            + " AND Department = '" + Convert.ToString(comboBoxWork.SelectedValue) + "'";
            BillingOp blo = new BillingOp();
            DataTable dt;

            if (ord != null && ord[idx].PartnerCode != null)
            {
                wParam += " AND PartnerCode = '" + ord[idx].PartnerCode + "'";
            }
            if ((dt = blo.UsingParamater_Select("D_Outsource", wParam)) == null) return;
            osd = new OutsourceData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) osd[i] = new OutsourceData(dt.Rows[i]);

            if (ord == null)
            {
                ord = new OsResultsData[osd.Length];
                for (int i = 0; i < osd.Length; i++)
                {
                    ord[i] = new OsResultsData();
                    storeOutsourceToOsResultsData(osd[i], i);
                }
            }

            blo = new BillingOp();
            wParam = " WHERE OutsourceID = " + osd[0].OutsourceID;
            dt = blo.UsingParamater_Select("D_OutsourceCont", wParam);

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(iniRCnt);

            if (dt.Rows.Count > iniRCnt) dataGridView1.Rows.Add(dt.Rows.Count - iniRCnt);
            if (!viewOutsourceContToDgv(dt, dataGridView1, osd[0].PartnerCode)) return;

            reCalculateAll(dataGridView1);

            return;
        }


        private void storeOutsourceToOsResultsData(OutsourceData osd,int idx)
        {
            ord[idx].TaskCode = ted.TaskCode;
            ord[idx].OrderNo = osd.OrderNo;
            labelOrderNo.Text = osd.OrderNo;
            ord[idx].PartnerCode = osd.PartnerCode;
            labelPartnerCode.Text = osd.PartnerCode;
            ord[idx].PayRoule = osd.PayRoule;
            ord[idx].Amount = osd.Amount;
            ord[idx].StartDate = osd.StartDate;
            ord[idx].EndDate = osd.EndDate;
            ord[idx].InspectDate = osd.InspectDate;
            ord[idx].ReceiptDate = osd.ReceiptDate;
            ord[idx].Place = osd.Place;
            ord[idx].Note = osd.Note;
            ord[idx].OfficeCode = osd.OfficeCode;
            ord[idx].Department = osd.Department;
            ord[idx].Publisher = osd.OfficeCode + osd.Department;
            ord[idx].ContractForm = 0;
            ord[idx].RecordedDate = (dateTimePickerRecordedDate.Value).EndOfMonth();
            ord[idx].TaskEntryID = ted.TaskEntryID;
            //storeDateToOsResultsData(ord[idx]);
        }


        private void editOsResultsData( int idx )
        {
            ord[idx].TaskCode = ted.TaskCode;
            ord[idx].OrderNo = labelOrderNo.Text;
            ord[idx].PartnerCode = labelPartnerCode.Text;
            ord[idx].PayRoule = 0;
            //ord[idx].Amount = 0;
            ord[idx].StartDate = ted.StartDate;
            ord[idx].EndDate = ted.EndDate;
            //ord[idx].InspectDate = osd.InspectDate;
            //ord[idx].ReceiptDate = osd.ReceiptDate;
            //ord[idx].Place = osd.Place;
            //ord[idx].Note = osd.Note;
            ord[idx].OfficeCode = Convert.ToString(comboBoxOffice.SelectedValue);
            ord[idx].Department = Convert.ToString(comboBoxWork.SelectedValue);
            ord[idx].Publisher = ord[idx].OfficeCode + ord[idx].Department;
            ord[idx].ContractForm = 0;
            ord[idx].RecordedDate = ( dateTimePickerRecordedDate.Value ).EndOfMonth();
            ord[idx].TaskEntryID = ted.TaskEntryID;
            //storeDateToOsResultsData(ord[idx]);
        }


        private void storeDateToOsResultsData(OsResultsData ord)
        {
            ord.PublishDate = (DateTime.Today).StripTime();                                           // 請負契約では使用されない
            ord.RecordedDate = (dateTimePickerRecordedDate.Value).EndOfMonth();
        }


        //private void viewOsResultsContToDgv(int osResultsID, string item, DataGridViewRow dgvRow)
        //{
        //    viewLastMonthOsResultsContToDgv(osResultsID, item, dgvRow);
        //    viewThisMonthOsResultsContToDgv(osResultsID, item, dgvRow);
        //}


        private void viewOsResultsContToDgv( OsResultsData ord, string item, DataGridViewRow dgvRow )
        {
            viewLastMonthOsResultsContToDgv( ord, item, dgvRow );
            viewThisMonthOsResultsContToDgv( ord, item, dgvRow );
        }


        //private void viewLastMonthOsResultsContToDgv(int osResultsID, string item, DataGridViewRow dgvRow)
        //{
        //    // 過去データ
        //    string wParam = " WHERE OsResultsID = " + Convert.ToString(osResultsID)
        //        + " AND RecordedDate < '" + Convert.ToDateTime((dateTimePickerRecordedDate.Value).EndOfMonth())
        //        + "' AND Item = '" + item + "'";
        //    BillingOp blo = new BillingOp();
        //    DataTable dt = blo.UsingParamater_Select("D_OsResultsCont", wParam);
        //    if (dt == null) return;
        //    DataRow dr;
        //    decimal sum = 0;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dr = dt.Rows[i];
        //        sum += Convert.ToDecimal(dr["Quantity"]);   // 各月の数量の和 = 前月累計
        //    }
        //    dr = dt.Rows[0];
        //    viewOsResultsContItemToDgv(dgvRow, dr); 
        //    dgvRow.Cells["LQuantity"].Value = DHandling.DecimaltoStr(sum, "0.00");
        //    dgvRow.Cells["LAmount"].Value = DHandling.DecimaltoStr(sum * Convert.ToDecimal(dr["Cost"]), "0.00");
        //    dgvRow.Cells["ContID"].Value = 0;
        //}

        private void viewLastMonthOsResultsContToDgv( OsResultsData ord, string item, DataGridViewRow dgvRow )
        {
            // 過去データ
            string wParam = " WHERE TaskEntryID = " + ord.TaskEntryID + " AND PartnerCode = '" + ord.PartnerCode + "' "
                          + "AND RecordedDate < '" + ord.RecordedDate + "' AND Item = '" + item + "' AND Subject = '*'" ; 
            BillingOp blo = new BillingOp();
            DataTable dt = blo.UsingParamater_Select( "D_OsResultsCont", wParam );
            if( dt == null ) return;
            DataRow dr;
            decimal sum = 0;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                sum += Convert.ToDecimal( dr["Quantity"] );   // 各月の数量の和 = 前月累計
            }
            dr = dt.Rows[0];
            viewOsResultsContItemToDgv( dgvRow, dr );
            dgvRow.Cells["LQuantity"].Value = DHandling.DecimaltoStr( sum, "0.00" );
            dgvRow.Cells["LAmount"].Value = DHandling.DecimaltoStr( sum * Convert.ToDecimal( dr["Cost"] ), "#,0" );
            dgvRow.Cells["ContID"].Value = 0;
        }

        //private void viewThisMonthOsResultsContToDgv(int osResultsID, string item, DataGridViewRow dgvRow)
        //{
        //    // 当月データ
        //    string wParam = " WHERE OsResultsID = " + Convert.ToString(osResultsID)
        //        + " AND RecordedDate = '" + Convert.ToDateTime((dateTimePickerRecordedDate.Value).EndOfMonth())
        //        + "' AND Item = '" + item + "'";
        //    BillingOp blo = new BillingOp();
        //    DataTable dt = blo.UsingParamater_Select("D_OsResultsCont", wParam);
        //    if (dt == null)
        //    {
        //        dgvRow.Cells["Quantity"].Value = "0.00";
        //        dgvRow.Cells["ContID"].Value = 0;
        //    }
        //    else
        //    {
        //        DataRow dr = dt.Rows[0];
        //        viewOsResultsContItemToDgv(dgvRow, dr); 
        //        dgvRow.Cells["Quantity"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Quantity"]), "0.00");
        //        dgvRow.Cells["ContID"].Value = Convert.ToInt32(dr["OsResultsContID"]);
        //    }
        //}


        private void viewThisMonthOsResultsContToDgv( OsResultsData ord, string item, DataGridViewRow dgvRow )
        {
            // 当月データ
            string wParam = " WHERE TaskEntryID = " + ord.TaskEntryID + " AND PartnerCode = '" + ord.PartnerCode + "' "
                          + "AND RecordedDate = '" + ord.RecordedDate + "' AND Item = '" + item + "' AND Subject = '*'" ; 
            BillingOp blo = new BillingOp();
            DataTable dt = blo.UsingParamater_Select( "D_OsResultsCont", wParam );
            if( dt == null )
            {
                dgvRow.Cells["Quantity"].Value = "0.00";
                dgvRow.Cells["ContID"].Value = 0;
            }
            else
            {
                DataRow dr = dt.Rows[0];
                viewOsResultsContItemToDgv( dgvRow, dr );
                dgvRow.Cells["Quantity"].Value = DHandling.DecimaltoStr( Convert.ToDecimal( dr["Quantity"] ), "0.00" );
                dgvRow.Cells["ContID"].Value = Convert.ToInt32( dr["OsResultsContID"] );
            }
        }


        private void viewOsResultsContItemToDgv(DataGridViewRow dgvRow, DataRow dr)
        {
            dgvRow.Cells["ItemCode"].Value = Convert.ToString(dr["ItemCode"]);
            dgvRow.Cells["Item"].Value = Convert.ToString(dr["Item"]);
            dgvRow.Cells["PQuantity"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["PQuantity"]), "0.00");
            dgvRow.Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
            dgvRow.Cells["Cost"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Cost"]), "#,0");
            dgvRow.Cells["PAmount"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["PQuantity"]) * Convert.ToDecimal(dr["Cost"]), "#,0");
        }


        private bool viewOutsourceContToDgv(DataTable dt, DataGridView dgv, string partnerCode)
        {
            PartnersData pd = new PartnersData();
            labelPartner.Text = pd.SelectPartnerName(partnerCode);

            DataRow dr;
            decimal sum = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                sum += Convert.ToDecimal(dr["Quantity"]);   // 各月の数量の和 = 前月累計
                dgv.Rows[i].Cells["ItemCode"].Value = Convert.ToString(dr["ItemCode"]);
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString(dr["Item"]);
                if (Convert.ToString(dr["Item"]) == null || Convert.ToString(dr["Item"]) == "")
                {
                    dgv.Rows[i].Cells["Item"].Value = Convert.ToString(dr["ItemDetail"]);
                }
                dgv.Rows[i].Cells["PQuantity"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Quantity"]), "0.00");
                dgv.Rows[i].Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
                dgv.Rows[i].Cells["Cost"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Cost"]), "#,0");
                dgv.Rows[i].Cells["PAmount"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Quantity"]) * Convert.ToDecimal(dr["Cost"]), "#,0");
                dgv.Rows[i].Cells["ContID"].Value = 0;
            }
            return true;
        }


        private void viewData(int idx)
        {
            viewOsResultsData(idx);
            viewOsResultsContData(idx, dataGridView1);
        }


        private void viewOsResultsData(int idx)
        {
            edit_lblPageNo();
            edit_ResultsLabels(idx);
            dateTimePickerRecordedDate.Value = ord[idx].RecordedDate;
            nowMonth( ord[idx].RecordedDate );
        }

       
        private void viewOsResultsContData(int idx,DataGridView dgv)
        {
            dgv.Rows.Clear();
            dgv.Rows.Add(iniRCnt);
            loadOsResultsContData(ord[idx]);
            lineNumbering(dgv);
            reCalculateAll(dgv);
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private bool reCalculateAll(DataGridView dgv)
        {
            if (!checkInputData(dgv)) return false;

            decimal cost;
            decimal[] qty = new decimal[5];
            decimal[] amt = new decimal[5];
            decimal[] vSum = new decimal[5];
            int lastLine = 0;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (dgv.Rows[i].Cells["Cost"].Value != null)
                {

                    cost = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Cost"].Value));
                    // 計画
                    qty[0] = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["PQuantity"].Value));
                    amt[0] = cost * qty[0];
                    dgv.Rows[i].Cells["PAmount"].Value = DHandling.DecimaltoStr(amt[0], "#,0");

                    // 前月累計
                    qty[1] = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["LQuantity"].Value));
                    amt[1] = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["LAmount"].Value));
                    // 当月出来高
                    qty[2] = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value));
                    amt[2] = cost * qty[2];
                    dgv.Rows[i].Cells["Amount"].Value = DHandling.DecimaltoStr(amt[2], "#,0");
                    // 当月累計
                    qty[3] = qty[1] + qty[2];
                    amt[3] = amt[1] + amt[2];
                    dgv.Rows[i].Cells["SQuantity"].Value = DHandling.DecimaltoStr(qty[3], "#,0.00");
                    dgv.Rows[i].Cells["SAmount"].Value = DHandling.DecimaltoStr(amt[3], "#,0");
                    // 残高
                    qty[4] = qty[0] - qty[3];
                    amt[4] = amt[0] - amt[3];
                    dataGridView1.Rows[i].Cells["RQuantity"].Value = DHandling.DecimaltoStr(qty[4], "#,0.00");
                    dataGridView1.Rows[i].Cells["RAmount"].Value = DHandling.DecimaltoStr(amt[4], "#,0");

                    for (int j = 0; j < vSum.Length; j++) vSum[j] += amt[j];
                    lastLine = i;
                }
            }

            if ((vSum[0] + vSum[1] + vSum[2] + vSum[3] + vSum[4]) == 0) return true;
            
            if ((lastLine+=2) > dgv.RowCount) dgv.Rows.Add(2);

            string[] itmAry = new string[] { "PAmount", "LAmount", "Amount", "SAmount", "RAmount" };
            dgv.Rows[lastLine].Cells["Item"].Value = Sign.SumTotal;
            for (int i = 0; i < itmAry.Length; i++)
            {
                dgv.Rows[lastLine].Cells[itmAry[i]].Value = DHandling.DecimaltoStr(vSum[i], "#,0");
            }
            return true;
        }
        

        private void clearOsResultsContAmount(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells["Cost"].Value = "";
                dgv.Rows[i].Cells["Amount"].Value = "";
            }
        }


        private void lineNumbering(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["Item"].Value) != "")
                    dgv.Rows[i].Cells["LNo"].Value = (i + 1).ToString();
            }
        }


        private bool checkInputData(DataGridView dgv)
        {
            string checkStr;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (dgv.Rows[i].Cells["Cost"].Value != null)
                {
                    for (int j = 3; j < dgv.ColumnCount; j++)
                    {
                        if (j != 4)
                        {
                            checkStr = Convert.ToString(dgv.Rows[i].Cells[j].Value);
                            if (checkStr == null || checkStr == "")
                            {

                            }
                            else
                            {
                                if (!DHandling.IsDecimal(checkStr))
                                {
                                    DMessage.ValueErrMsg();
                                    //dgv[j, i].Style.BackColor = Color.Red;
                                    dgv.CurrentCell = dgv[j, i];
                                    return false;
                                }
                            }
                            
                        }
                    }
                }
            }
            return true;
        }


        private void createExcelFile(string templateFileName, string sheetName)
        {
            Publish publ = new Publish(Folder.DefaultExcelTemplate(templateFileName));
            PublishData pd = new PublishData();

            pd.TaskCode = ted.TaskCode;
            pd.TaskName = ted.TaskName;
            pd.CostType = ted.CostType;
            pd.TaskPlace = ted.TaskPlace;
            pd.StartDate = ted.StartDate;
            pd.EndDate = ted.EndDate;
            pd.TaxRate = ted.TaxRate;
            pd.PartnerName = ted.PartnerName;
            pd.LeaderName = labelLeader.Text;
            pd.SalesMName = labelSalesM.Text;
            pd.OrderPartner = labelPartner.Text;
            pd.RecordedDate = dateTimePickerRecordedDate.Value;

            publ.ExcelFile(sheetName, pd, dataGridView1);
            return;
        }



        private void summaryLastMonth(DataGridView dgv )
        {
            
        }
    }
}

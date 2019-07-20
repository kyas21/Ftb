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
using ListForm;

namespace EstimPlan
{
    public partial class FormEstimate : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        TaskEntryData td;
        private bool iniPro = true;
        WorkItemsData[] wid;
        EstimateData estd;
        private int maxVer = 0;
        private int iniRCnt = 29;
        const string estim = "見積書";
        const string estimCC = "見積書（控有）";
        private string bookName = estim + ".xlsx";
        private string bookNameC = estimCC + ".xlsx";
        const string sheetNameT = "EstimateTop";
        const string sheetNameC = "EstimateCopy";

        private bool grdSet = false;
        private string AmountReg = "";

        private string msgTaxGuide = "消費税は、《計》を先にしていないと計算されません。";
        private string msgNoPlan = "取りこめる「予算書」はありません。";
        private string msgCopyPlan = "「予算書」データを取りこみました。";
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormEstimate()
        {
            InitializeComponent();
        }

        public FormEstimate(TaskEntryData td)
        {
            InitializeComponent();
            this.td = td;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormEstimate_Load(object sender, EventArgs e)
        {
            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling uih = new UiHandling(dataGridView1);
            uih.DgvReadyNoRHeader();
            //並び替えができないようにする
            uih.NoSortable();

            dataGridView1.Rows.Add(iniRCnt);
            buttonNumbering(dataGridView1);
            labelMsg.Text = "";

            create_cbTitle();       // ComboBoxTitle作成
            create_cbVersion();     // ComboBox見積書Virsion作成
            create_cbPVersion();    // ComboBox予算書Virsion作成

            labelPublisher.Text = td.OfficeName + td.DepartName;        // 部署名
            labelTaskCode.Text = td.TaskCode;                           // 業務番号表示
            //labelCostType.Text = td.CostType;                          // 原価目標区分表示
            //labelTask.Text = td.TaskName;                              // 業務名表示
            //labelTask.Text = td.CostType + td.TaskName;                // 業務名表示
            labelTask.Text = td.TaskName;                                // 業務名表示
            labelWorkingPlace.Text = td.TaskPlace;                  // 業務場所表示
            labelPartner.Text = td.PartnerName;                     // 発注者表示
            labelTotalAmount.Text = "0";                            // 合計金額
            labelAmount.Text = "0";                                 // 業務金額
            labelTax.Text = "0";                                   // 消費税額

            textBoxTaxRate.Text = Convert.ToString(td.TaxRate * 100);
            textBoxExpenses.Text = Convert.ToString(td.Expenses * 100);

            // 見積データの読込み、表示
            dgvSetting();

            // 作業項目マスタの一覧作成
            EstPlanOp ep = new EstPlanOp();
            wid = ep.StoreWorkItemsData(td.MemberCode);
        }


        private void FormEstimate_Shown(object sender, EventArgs e)
        {
            iniPro = false;       // 初期化処理終了
        }


        // タイトル「見積書」「見積書（控）」選択による表示項目変更
        private void comboBoxTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelCarbon.Visible = (comboBoxTitle.SelectedIndex == 0) ? false : true;
        }


        // Short-Cut Key
        // 前提：コントロールはどこにあっても良い
        private void FormEstimate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) reCalculateAll(dataGridView1);

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            switch (e.KeyCode)
            {
                case Keys.R:
                    reCalculateAll(dataGridView1);
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_CellButtonClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //"Button"列ならば、ボタンがクリックされた 
            if (dgv.Columns[e.ColumnIndex].Name == "Button") chooseItemData(dgv);
        }


        // [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        // 前提：コントロールがDataGridViewにある時
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.CurrentCellAddress.X == 7)
            {
                if (e.KeyData == Keys.Delete || e.KeyData == Keys.Back)
                {
                    if (dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].ReadOnly == true)
                    {
                        grdSet = true;
                        dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = AmountReg;
                        grdSet = false;
                    }
                }
            }

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            switch (e.KeyCode)
            {
                case Keys.A:
                    chooseItemData(dgv);
                    dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                    break;
                case Keys.C:
                    Clipboard.SetDataObject(dgv.GetClipboardContent());
                    break;
                case Keys.I:
                case Keys.D:
                    buttonNumbering(dgv);
                    break;
                case Keys.R:
                    reCalculateAll(dgv);
                    break;
                default:
                    break;
            }
        }


        // Cellの内容に変化があったとき
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;   // 初期化中
            if (grdSet == true) return;

            labelMsg.Text = "";
            DataGridView dgv = (DataGridView)sender;
            Calculation calc = new Calculation();

            switch (e.ColumnIndex)
            {
                case 1:     // 「コード」列
                    EstPlanOp epo = new EstPlanOp();      // Code入力時作業項目マスタ読込
                    WorkItemsData wids = epo.LoadWorkItemsData(wid, Convert.ToString(dgv.Rows[e.RowIndex].Cells["ItemCode"].Value));
                    if (wids != null) viewItemDataToDgv(dgv.Rows[e.RowIndex], wids);
                    calc = new Calculation();
                    if (calc.ExtractCalcWord(Convert.ToString(dgv.Rows[e.RowIndex].Cells["Item"].Value)) == Sign.Tax)
                    {
                        labelMsg.Text = msgTaxGuide;
                    }
                    else
                    {
                        verticalCalc(dgv);
                    }
                    dgv.Refresh();
                    break;
                case 4:     // 「数量」列
                    //editDGVRowCell( dgv.Rows[e.RowIndex], e.ColumnIndex );
                    //break;

                case 6:     // 「単価」列
                    editDGVRowCell( dgv.Rows[e.RowIndex], e.ColumnIndex );
                    break;

                case 7:     // 「金額」列
                    editDGVRowCell( dgv.Rows[e.RowIndex], e.ColumnIndex );
                    if (Convert.ToString(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != "")
                        AmountReg = Convert.ToString(dgv.Rows[e.RowIndex].Cells["Amount"].Value); // 計
                    break;

                default:
                    break;
            }
        }


        private void comboBoxVersion_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            buttonOWrite.Enabled = loadExistingEstimateData( Convert.ToInt32( comboBoxVersion.Text ), dataGridView1 ); 
        }


        private void buttonFromPlan_MouseHover(object sender, EventArgs e)
        {
            //labelAssistMsg.Text = "予算書データから見積書を作成するためには予算書の版数を指定してください。";
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;
            EstPlanOp epo = new EstPlanOp();
            labelMsg.Text = "";

            switch (btn.Name)
            {
                case "buttonReCalc":
                    reCalculateAll(dataGridView1);
                    break;

                case "buttonFromPlan":
                    if( loadPlanningData( dataGridView1 ) )
                    {
                        buttonFromPlan.Enabled = false;
                        labelMsg.Text = msgCopyPlan;
                    }
                    else
                    {
                        labelMsg.Text = msgNoPlan;
                    }
                    buttonNWrite.Visible = true;
                    break;

                case "buttonOWrite":
                    reCalculateAll(this.dataGridView1);                                                 // 再計算
                    storeEstimateData();
                    if (!epo.Estimate_Update(estd)) return;
                    if (!epo.EstimateCont_Delete(estd.EstimateID)) return;
                    if (!epo.EstimateCont_Insert(dataGridView1, estd.EstimateID)) return;
                    // kusano TRY 20170425
                    buttonOWrite.Text = "上書保存";
                    buttonNWrite.Visible = true;
                    labelMsg.Text = "保存しました。";
                    // TRY end
                    break;

                case "buttonNWrite":
                    reCalculateAll(this.dataGridView1);                                                 // 再計算
                    storeEstimateData();
                    estd.VersionNo = maxVer + 1;
                    estd.EstimateID = epo.Estimate_Insert(estd);
                    if (estd.EstimateID < 0) return;
                    if (!epo.EstimateCont_Insert(dataGridView1, estd.EstimateID)) return;
                    buttonOWrite.Enabled = true;
                    buttonOWrite.Text = "上書保存";
                    maxVer = estd.VersionNo;
                    iniPro = true;
                    create_cbVersion();
                    iniPro = false;
                    comboBoxVersion.SelectedIndex = comboBoxVersion.Items.Count - 1;
                    labelMsg.Text = "新規保存しました。";
                    break;

                case "buttonDelete":
                    if (!epo.Estimate_Delete(estd.EstimateID)) return;
                    if (!epo.EstimateCont_Delete(estd.EstimateID)) return;
                    iniPro = true;
                    create_cbVersion();
                    iniPro = false;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(iniRCnt);
                    buttonNumbering(dataGridView1);
                    labelTotalAmount.Text = "0";                            // 合計金額
                    labelAmount.Text = "0";                                 // 業務金額
                    labelTax.Text = "0";                                    // 消費税額

                    textBoxTaxRate.Text = Convert.ToString(td.TaxRate * 100);
                    textBoxExpenses.Text = Convert.ToString(td.Expenses * 100);
                    dgvSetting();
                    buttonNWrite.Enabled = true;
                    labelMsg.Text = "削除しました。";
                    break;

                case "buttonCancel":
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(iniRCnt);
                    buttonNumbering(dataGridView1);
                    dgvSetting();
                    break;

                case "buttonClose":
                    this.Close();
                    break;

                case "buttonPrint":
                    reCalculateAll(this.dataGridView1);                                                 // 再計算
                    Publish publ;
                    PublishData pd;
                    string sheetName;
                    if (comboBoxTitle.Text == estim)
                    {
                        publ = new Publish(Folder.DefaultExcelTemplate(bookName));
                        pd = new PublishData();
                        sheetName = sheetNameT;
                    }
                    else
                    {
                        publ = new Publish(Folder.DefaultExcelTemplate(bookNameC));
                        pd = new PublishData();
                        sheetName = sheetNameC;
                        pd.Budgets = DHandling.ToRegDecimal(textBoxBudgets.Text);
                        pd.MinBid = DHandling.ToRegDecimal(textBoxMinBid.Text);
                        pd.Contract = DHandling.ToRegDecimal(textBoxContract.Text);
                    }

                    pd.TotalAmount = DHandling.ToRegDecimal(labelTotalAmount.Text);
                    pd.Amount = DHandling.ToRegDecimal(labelAmount.Text);
                    pd.Tax = DHandling.ToRegDecimal(labelTax.Text);
                    pd.TaskName = td.TaskName;
                    pd.TaskPlace = td.TaskPlace;
                    pd.Note = textBoxNote.Text;
                    pd.PartnerName = td.PartnerName;
                    pd.OfficeCode = td.OfficeCode;
                    pd.OfficeName = td.OfficeName;

                    pd.PublishOffice = ( checkBoxPublish.Checked ) ? 1 : 0;

                    publ.ExcelFile(sheetName, pd, dataGridView1);
                    break;

                default:
                    break;
            }
        }


        private void textBox_TextChanged(object sender, EventArgs e)
        {
            //if (iniPro) return;

            //TextBox tb = (TextBox)sender;

            //decimal WorkDwcimal = 0;
            //decimal.TryParse(Convert.ToString(tb.Text), out WorkDwcimal);
            //tb.Text = decFormat(WorkDwcimal);
        }


        private void textBox_Leave(object sender, EventArgs e)
        {
            if (iniPro) return;

            TextBox tb = (TextBox)sender;

            decimal WorkDwcimal = 0;
            decimal.TryParse(Convert.ToString(tb.Text), out WorkDwcimal);

            switch( tb.Name )
            {
                case "textBoxBudgets":
                case "textBoxMinBid":
                case "textBoxContract":
                    tb.Text = decFormat(WorkDwcimal);
                    break;

                default:
                    tb.Text = decPointFormat(WorkDwcimal);
                    reCalculateAll(this.dataGridView1);             // 再計算
                    break;
            }
        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (iniPro) return;   // 初期化中

            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex == 7)
            {
                if (Convert.ToString(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != "")
                {
                    grdSet = true;
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = signConvert(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    grdSet = false;
                }
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;   // 初期化中

            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex == 7)
            {
                if (Convert.ToString(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != "")
                {
                    grdSet = true;
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = minusConvert(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, "#,0");
                    grdSet = false;
                }
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (iniPro) return;   // 初期化中

            DataGridView dgv = (DataGridView)sender;
            AmountReg = Convert.ToString(dgv.Rows[e.RowIndex].Cells["Amount"].Value);
        }

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // comboBox作成
        // Title
        private void create_cbTitle()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxTitle);
            cbe.ValueItem = new string[] { "0", "1" };
            cbe.DisplayItem = new string[] { "見積書", "見積書（控）" };
            cbe.Basic();
        }


        // 見積書Virsion
        private void create_cbVersion()
        {
            ComboBoxEdit cb = new ComboBoxEdit(comboBoxVersion);
            if (!cb.Version("D_Estimate", "WHERE TaskEntryID = " + td.TaskEntryID + " ORDER BY VersionNo ASC"))
            {
                comboBoxVersion.Text = "-";
            }
        }


        // 予算書Version
        private void create_cbPVersion()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxPVersion);
            if( cbe.Version( "D_Planning", "WHERE TaskEntryID = " + td.TaskEntryID + " ORDER BY VersionNo ASC" ) )
            {
                buttonFromPlan.Enabled = ( string.IsNullOrEmpty( cbe.ValueItem[0] ) ) ? false : true;
                if( buttonFromPlan.Enabled )
                {
                    comboBoxPVersion.Visible = true;
                    comboBoxPVersion.Enabled = true;
                }
                else
                {
                    labelMsg.Text = msgNoPlan;
                    comboBoxPVersion.Visible = false;
                    comboBoxPVersion.Enabled = false;
                }
            }
            else
            { 
                comboBoxPVersion.Text = "-";
            }
        }


        // 見積データの読み込み 
        private bool loadExistingEstimateData(int verNo, DataGridView dgv)
        {
            int TaskEntryIDReg = estd.TaskEntryID;              // TaskEntryID保持

            EstPlanOp epo = new EstPlanOp("D_Estimate");
            DataTable dt = epo.EstPlan_Select(td.TaskEntryID, verNo);
            if (dt == null) return false;
            DataRow dr = dt.Rows[0];
            estd = new EstimateData(dr);
            estd.TaskEntryID = TaskEntryIDReg;

            textBoxBudgets.Text = decFormat(estd.Budgets);
            textBoxMinBid.Text = decFormat(estd.MinimalBid);
            textBoxContract.Text = decFormat(estd.Contract);
            textBoxNote.Text = estd.Note;
            dt = epo.EstimateCont_Select(estd.EstimateID);
            //if (dt == null) return false;
            if (dt == null) return true;

            dgv.Rows.Clear();
            dgv.Rows.Add(iniRCnt);

            if (dt.Rows.Count > iniRCnt) dgv.Rows.Add(dt.Rows.Count - iniRCnt);
            if (!viewEstimateContToDgv(dt, dgv, "EstimateCont")) return false;                 // DgvへのTable内容セット

            reCalculateAll(dgv);                                                 // 再計算
            buttonNumbering(dgv);

            return true;
        }


        // 予算データの読み込み 
        private bool loadPlanningData(DataGridView dgv)
        {
            EstPlanOp epo = new EstPlanOp();
            int verNo = ( string.IsNullOrEmpty(comboBoxPVersion.Text)) ? 0 : Convert.ToInt32(comboBoxPVersion.Text);
            DataTable dt = epo.Planning_Select(td.TaskEntryID, verNo);
            if (dt == null) return false;
            DataRow dr = dt.Rows[0];
            PlanningData plnd = new PlanningData(dr);

            dt = epo.PlanningCont_Select(plnd.PlanningID);
            if (dt == null) return false;

            dgv.Rows.Clear();
            dgv.Rows.Add(iniRCnt);

            if (dt.Rows.Count > iniRCnt) dgv.Rows.Add(dt.Rows.Count - iniRCnt);
            if (!viewEstimateContToDgv(dt, dgv, "PlanningCont")) return false;

            reCalculateAll(dgv);                                                 // 再計算



            // kusano TRY 20170425
            buttonOWrite.Text = "保存";
            buttonNWrite.Visible = false;
            // TRY End



            return true;
        }


        private bool viewEstimateContToDgv(DataTable dt, DataGridView dgv, string tblName)
        {
            DataRow dr;
            Calculation calc = new Calculation();

            decimal wkCost;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                dgv.Rows[i].Cells["ItemCode"].Value = Convert.ToString(dr["ItemCode"]);
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString(dr["Item"]);
                dgv.Rows[i].Cells["ItemDetail"].Value = Convert.ToString(dr["ItemDetail"]);

                wkCost = 0;
                if(tblName == "EstimateCont" )
                {
                    wkCost = Convert.ToDecimal( dr["Cost"] );
                }
                else
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        if( Convert.ToDecimal( dr["Cost" + j.ToString()] ) > 0 )
                        {
                            wkCost = Convert.ToDecimal( dr["Cost" + j.ToString()] );
                        }
                    }
                }

                if (Convert.ToString(dr["Unit"]) != "")
                {
                    if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) == Sign.Expenses )
                    {
                        // TRY kusano 20170425
                        //dgv.Rows[i].Cells["Quantity"].ReadOnly = true;
                        //dgv.Rows[i].Cells["Quantity"].Value = null;
                        dgv.Rows[i].Cells["Quantity"].Value = 1;
                        dgv.Rows[i].Cells["Unit"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost"].Value = null;
                    }
                    else
                    {
                        // データグリッドビューの編集ロック制御
                        // TRY kusano 20170425
                        //dgv.Rows[i].Cells["Quantity"].ReadOnly = false;
                        dgv.Rows[i].Cells["Unit"].ReadOnly = false;
                        dgv.Rows[i].Cells["Cost"].ReadOnly = false;
                        dgv.Rows[i].Cells["Quantity"].Value = decPointFormat(Convert.ToDecimal(dr["Quantity"]));
                        dgv.Rows[i].Cells["Cost"].Value = decFormat( wkCost );
                    }
                    // TRY kusano 20170425
                    dgv.Rows[i].Cells["Quantity"].ReadOnly = false;
                    // TRY End kusano 20170425
                    dgv.Rows[i].Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
                }
                else
                {
                    // データグリッドビューの編集ロック制御
                    dgv.Rows[i].Cells["Quantity"].ReadOnly = true;
                    dgv.Rows[i].Cells["Quantity"].Value = null;
                    dgv.Rows[i].Cells["Unit"].ReadOnly = true;
                    dgv.Rows[i].Cells["Cost"].ReadOnly = true;
                }

                if (calc.ExtractCalcWord(Convert.ToString(dgv.Rows[i].Cells["Item"].Value)) == Sign.Discount)
                {
                    dgv.Rows[i].Cells["Amount"].ReadOnly = false;
                    dgv.Rows[i].Cells["Amount"].Value = minusConvert(wkCost, "#,0");
                }
                else
                {
                    dgv.Rows[i].Cells["Amount"].ReadOnly = true;
                }
                if(tblName == "EstimateCont") dgv.Rows[i].Cells["Note"].Value = Convert.ToString(dr["Note"]);
            }
            return true;
        }


        private void storeEstimateData()
        {
            decimal billings = DHandling.ToRegDecimal(labelAmount.Text);

            estd.Total = DHandling.ToRegDecimal(labelAmount.Text);
            estd.Budgets = DHandling.ToRegDecimal(textBoxBudgets.Text);
            estd.MinimalBid = DHandling.ToRegDecimal(textBoxMinBid.Text);
            estd.Contract = DHandling.ToRegDecimal(textBoxContract.Text);
            estd.Note = textBoxNote.Text;
            estd.OfficeCode = td.OfficeCode;
            estd.Department = td.Department;
        }


        private void clearEstimateData()
        {
            estd = new EstimateData();
            estd.TaskEntryID = td.TaskEntryID;
            estd.Publisher = td.Department;
        }


        // 作業項目マスタデータをItemList画面から得る
        private void chooseItemData(DataGridView dgv)
        {
            WorkItemsData wids = FormItemList.ReceiveItems(wid);
            if (wids == null) return;

            viewItemDataToDgv(dgv.Rows[dgv.CurrentCellAddress.Y], wids);
            if (wids.Unit != "") dgv.CurrentCell = dgv.Rows[dgv.CurrentCellAddress.Y].Cells["Quantity"];

            Calculation calc = new Calculation();
            if (calc.ExtractCalcWord(Convert.ToString(dgv.Rows[dgv.CurrentCellAddress.Y].Cells["Item"].Value)) != null) verticalCalc(dgv);
        }


        // DataGridViewに選択された作業項目マスタデータの内容セット
        private void viewItemDataToDgv(DataGridViewRow dgvRow, WorkItemsData wids)
        {
            Calculation calc = new Calculation();

            // データグリッドビューの編集ロック制御
            if (wids.Unit == "")
            {
                dgvRow.Cells["Quantity"].ReadOnly = true;
                dgvRow.Cells["Quantity"].Value = null;
                dgvRow.Cells["Unit"].ReadOnly = true;
                dgvRow.Cells["Cost"].ReadOnly = true;
                dgvRow.Cells["Amount"].Value = null;
            }
            else
            {
                if (calc.ExtractCalcWord(wids.UItem) == Sign.Expenses)
                {
                    //dgvRow.Cells["Quantity"].ReadOnly = true;
                    dgvRow.Cells["Quantity"].ReadOnly = false;
                    //dgvRow.Cells["Quantity"].Value = null;
                    dgvRow.Cells["Quantity"].Value = 1;
                    dgvRow.Cells["Unit"].ReadOnly = true;
                    dgvRow.Cells["Cost"].ReadOnly = true;
                    dgvRow.Cells["Cost"].Value = null;
                }
                else
                {
                    dgvRow.Cells["Quantity"].ReadOnly = false;
                    dgvRow.Cells["Unit"].ReadOnly = false;
                    dgvRow.Cells["Cost"].ReadOnly = false;
                }
            }

            dgvRow.Cells["Amount"].ReadOnly = ( calc.ExtractCalcWord( wids.UItem ) == Sign.Discount )?false:true;

            dgvRow.Cells["ItemCode"].Value = wids.ItemCode;
            if (wids.UItem != "")
            {
                dgvRow.Cells["Item"].Value = wids.UItem;
                dgvRow.Cells["Cost"].Value = null;
            }
            else
            {
                dgvRow.Cells["Item"].Value = wids.Item;
                dgvRow.Cells["Cost"].Value = (wids.StdCost == 0) ? null : decFormat(wids.StdCost);
            }
            dgvRow.Cells["ItemDetail"].Value = wids.ItemDetail;
            dgvRow.Cells["Unit"].Value = wids.Unit;
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private void reCalculateAll(DataGridView dgv)
        {
            td.TaxRate = DHandling.ToRegDecimal(textBoxTaxRate.Text) / 100;
            td.Expenses = DHandling.ToRegDecimal(textBoxExpenses.Text) / 100;
            horizontalCalc(dgv);
            verticalCalc(dgv);
        }


        private void horizontalCalc(DataGridView dgv)
        {
            Calculation calc = new Calculation(td);
            for (int i = 0; i < dgv.RowCount; i++)
            {
                if (dgv.Rows[i].Cells["Item"].Value != null || dgv.Rows[i].Cells["ItemDetail"].Value != null)
                {
                    calc.HCalcEstimateRow(dgv.Rows[i]);
                }
            }
        }


        // DataGridViewの縦計算
        private void verticalCalc(DataGridView dgv)
        {
            Calculation calc = new Calculation(td);
            calc.VCalcEstimate(dgv);
            labelAmount.Text = decFormat(calc.Sum);
            labelTax.Text = decFormat(calc.Tax);
            labelTotalAmount.Text = decFormat(calc.GSum);
        }


        // DataGridViewButtonの番号を再採番
        private void buttonNumbering(DataGridView dgv)
        {
            int startNo = 1;
            for (int i = 0; i < dgv.RowCount; i++)
                dgv.Rows[i].Cells[0].Value = (startNo + i).ToString();
        }


        private static string decFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "#,0");
        }


        private static string decPointFormat(decimal decNum)
        {
            return DHandling.DecimaltoStr(decNum, "0.00");
        }

        
        /// <summary>
        /// 見積データの読込み、表示
        /// </summary>
        private void dgvSetting()
        {
            clearEstimateData();                     // 見積データの読込み、表示

            buttonOWrite.Enabled = false;           // 読込がなければ上書Click不可
            if (comboBoxVersion.Text != "")
            {
                comboBoxVersion.SelectedIndex = comboBoxVersion.Items.Count - 1;
                maxVer = (comboBoxVersion.Text == "") ? 0 : Convert.ToInt32(comboBoxVersion.Text);
                //if (loadExistingEstimateData(maxVer, dataGridView1)) buttonOWrite.Enabled = true;
                buttonOWrite.Enabled = loadExistingEstimateData( maxVer, dataGridView1 );
            }
            else
            {
                maxVer = 0;
            }
        }


        /// <summary>
        /// "-" → "△"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <param name="FormatSet">表示フォーマット</param>
        /// <returns>変換結果</returns>
        private string minusConvert(object TargetValue, string FormatSet)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "-" → "△"コンバート
                Decimal.TryParse(WorkString, out WorkDecimal);
                if (WorkDecimal < 0)
                    return "△" + (WorkDecimal * -1).ToString(FormatSet);
                else
                    return WorkDecimal.ToString(FormatSet);
            }
            return "";
        }

        /// <summary>
        /// "△" → "-"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private decimal signConvert(object TargetValue)
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


        /// <summary>
        /// DataGridViewのセルに0が入力された場合はブランクに置き換える
        /// </summary>
        /// <param name="dgvRow"></param>
        /// <param name="cellIndex"></param>
        private void editDGVRowCell( DataGridViewRow dgvRow, int cellIndex )
        {
            Calculation calc = new Calculation();
            decimal WorkDecimal = 0;
            if( dgvRow.Cells[cellIndex].ReadOnly == false )
            {
                decimal.TryParse( Convert.ToString( dgvRow.Cells[cellIndex].Value ), out WorkDecimal );
                if( WorkDecimal == 0 )
                {
                    dgvRow.Cells[cellIndex].Value = "";
                }
                else
                {
                    dgvRow.Cells[cellIndex].Value = ( cellIndex == 4 ) ? decPointFormat( WorkDecimal ) : decFormat( WorkDecimal );
                }
                calc = new Calculation();
                calc.HCalcEstimateRow( dgvRow );
            }
        }

    }
}

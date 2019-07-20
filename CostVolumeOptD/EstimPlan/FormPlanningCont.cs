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
    public partial class FormPlanningCont :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        PlanningData[] plnd;
        TaskEntryData ted;
        private int idx;
        private int idxBackup;
        private bool iniPro = true;
        WorkItemsData[] wid;
        private int iniRCnt = 29;
        string[] labelArray = new string[] { "原予算内訳書", "変更１回内訳書", "変更２回内訳書" };
        const string bookName = "原予算内訳書.xlsx";
        const string sheetName = "PlanningCont";

        private string msgNoEstimate = "取りこめる「見積書」はありません。";
        private string msgCopyEstimate = "「見積書」を取り込みました。";

        private bool grdSet = false;
        private string[] AmountReg = new string[4] { "", "", "", "" };
        private int endPoint;
        private bool updateStat = false;
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormPlanningCont()
        {
            InitializeComponent();
        }


        public FormPlanningCont( TaskEntryData ted, PlanningData[] plnd, int idx )
        {
            InitializeComponent();
            this.ted = ted;
            this.plnd = plnd;
            this.idx = idx;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//

        static public PlanningData[] SummaryData( TaskEntryData ted, PlanningData[] plnd, int idx )
        {
            FormPlanningCont formPlanningCont = new FormPlanningCont( ted, plnd, idx );
            formPlanningCont.ShowDialog();
            formPlanningCont.Dispose();

            return plnd;
        }


        private void FormPlanningCont_Load( object sender, EventArgs e )
        {
            this.defaultCellStyle = new DataGridViewCellStyle();

            UiHandling uih = new UiHandling( dataGridView1 );

            //並び替えができないようにする
            foreach( DataGridViewColumn c in dataGridView1.Columns )
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            // Assist Message クリア
            labelMsg.Text = "";

            dataGridView1.Rows.Add( iniRCnt );
            buttonNumbering();

            initialDisplayPlannignContData();

            create_cbEVersion();

            // 作業項目マスタの取込み
            EstPlanOp ep = new EstPlanOp();
            wid = ep.StoreWorkItemsData( ted.MemberCode );

            // I/Fデータ(plnd)の既存データ数確認と利用可能ボタンの設定
            setButtonEnabled();

            // 既存内訳データ読込
            if( endPoint > -1 && plnd[idx].PlanningID > 0 ) loadPlanningContData( dataGridView1 );

            updateStat = false;
        }


        private void FormPlanningCont_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        //------------------------------//
        // Event
        //------------------------------//
        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            EstPlanOp epo = new EstPlanOp();

            switch( btn.Name )
            {
                // 予算書からデータ取得
                case "buttonFromEstimate":
                    if( MessageBox.Show( "見積書のデータを予算書に取り込みます。\r\n" +
                                        "現在表示中のデータはすべて削除されます。", "", MessageBoxButtons.YesNo ) == DialogResult.No )
                                        //"予算書内のデータはすべて削除されますので注意してください。", "", MessageBoxButtons.YesNo ) == DialogResult.No )
                        return;
                    if( loadEstimateData() )
                    {
                        reCalculateAll( dataGridView1 );
                        labelMsg.Text = msgCopyEstimate;
                    }
                    else
                    {
                        labelMsg.Text = msgNoEstimate;
                    }
                    break;

                // 実行予算書へ戻る
                case "buttonPrev":
                    if( updateStat )
                    {
                        Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                        if( dialogRemining() == DialogResult.No ) return;
                    }
                    this.Close();
                    break;

                // 再計算
                case "buttonReCalc":
                    reCalculateAll( dataGridView1 );
                    break;

                // 上書、但しデータが全くない場合は新規保存
                case "buttonOWrite":
                    reCalculateAll( dataGridView1 );
                    if( plnd[idx].PlanningID < 1 )
                    {
                        storePlanningAndContents( dataGridView1 );
                    }
                    else
                    {
                        updatePlanningAndContents( dataGridView1 );
                    }
                    updateStat = false;
                    break;

                // 新版で保存
                case "buttonNWrite":
                    reCalculateAll( dataGridView1 );
                    restructPlanningData();
                    storePlanningAndContents( dataGridView1 );
                    updateStat = false;
                    break;

                // 削除
                case "buttonDelete":
                    deletePlanningAndContents();
                    updateStat = false;
                    break;

                case "buttonCancel":
                    initialDisplayPlannignContData();
                    loadPlanningContData( dataGridView1 );
                    updateStat = false;
                    break;

                case "buttonPrint":
                    reCalculateAll( dataGridView1 );
                    Publish publ = new Publish( Folder.DefaultExcelTemplate( bookName ) );
                    PublishData pd = new PublishData();
                    pd.Version = plnd[idx].VersionNo;
                    pd.PublishIndex = idx;
                    publ.ExcelFile( sheetName, pd, dataGridView1 );
                    break;

                default:
                    break;
            }
        }


        // Short-Cut Key
        // 前提：コントロールはどこにあっても良い
        private void FormPlanningCont_KeyDown( object sender, KeyEventArgs e )
        {
            switch( e.KeyCode )
            {
                case Keys.F5:
                    reCalculateAll( dataGridView1 );
                    break;

                default:
                    break;
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.R:
                    reCalculateAll( dataGridView1 );
                    break;

                default:
                    break;
            }
        }


        private void dataGridView1_CellButtonClick( object sender, DataGridViewCellEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            //"Button"列ならば、ボタンがクリックされた 
            if( dgv.Columns[e.ColumnIndex].Name == "Button" ) chooseItemData( dataGridView1 );
            updateStat = true;
        }


        // [Tab],矢印キーの挙動
        // [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        // 前提：コントロールがDataGridViewにある時
        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            switch( e.KeyCode )
            {
                case Keys.Right:
                case Keys.Tab:
                    if( dgv.CurrentCellAddress.X != 12 )
                    {
                        if( dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X + 1].ReadOnly == true )
                        {
                            if( dgv.CurrentCellAddress.X == 6 ) SendKeys.Send( "{RIGHT}" );
                            if( dgv.CurrentCellAddress.X == 8 ) SendKeys.Send( "{RIGHT}" );
                            if( dgv.CurrentCellAddress.X == 10 ) SendKeys.Send( "{LEFT}" );
                        }
                    }
                    break;

                case Keys.Left:
                    if( dgv.CurrentCellAddress.X != 0 )
                    {
                        if( dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X - 1].ReadOnly == true )
                        {
                            if( dgv.CurrentCellAddress.X == 12 ) SendKeys.Send( "{LEFT}" );
                            if( dgv.CurrentCellAddress.X == 10 ) SendKeys.Send( "{LEFT}" );
                            if( dgv.CurrentCellAddress.X == 8 ) SendKeys.Send( "{LEFT}" );
                        }
                    }
                    break;

                case Keys.Delete:
                case Keys.Back:
                    if( dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].ReadOnly == true )
                    {
                        grdSet = true;
                        switch( dgv.CurrentCellAddress.X )
                        {
                            case 7:
                                dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = AmountReg[0];
                                break;

                            case 9:
                                dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = AmountReg[1];
                                break;

                            case 11:
                                dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = AmountReg[2];
                                break;

                            case 12:
                                dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = AmountReg[3];
                                break;

                            default:
                                break;
                        }
                        grdSet = false;
                    }
                    break;

                default:
                    break;
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    chooseItemData( dataGridView1 );
                    dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                    break;

                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;

                case Keys.I:
                case Keys.D:
                    buttonNumbering();
                    reCalculateAll( dataGridView1 );
                    break;

                case Keys.R:
                    reCalculateAll( dataGridView1 );
                    break;

                default:
                    break;
            }

            updateStat = true;
        }


        // Cell 内容に変化があった時
        private void dataGridView1_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;   // 初期化中
            if( grdSet == true ) return;

            DataGridView dgv = ( DataGridView )sender;
            Calculation calc = new Calculation();

            switch( e.ColumnIndex )
            {
                case 1:     // 「コード」列
                    EstPlanOp epo = new EstPlanOp();      // Code入力時作業項目マスタ読込
                    WorkItemsData wids = epo.LoadWorkItemsData( wid, Convert.ToString( dgv.Rows[e.RowIndex].Cells["Code"].Value ) );
                    if( wids != null ) viewItemDataToDgv( dgv.Rows[e.RowIndex], wids );
                    dgv.Refresh();
                    break;

                case 4:     // 「数量」列
                    if( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false )
                    {
                        editDGVRowCell( dgv.Rows[e.RowIndex], e.ColumnIndex );
                        reCalculateAll( dataGridView1 );
                    }
                    break;

                case 6:     // 単価0
                case 8:     // 単価1
                case 10:    // 単価2
                    if( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false )
                    {
                        if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                        {
                            editDGVRowCell( dgv.Rows[e.RowIndex], e.ColumnIndex );
                        }
                        reCalculateAll( dataGridView1 );
                    }
                    break;

                case 7:
                    if( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false )
                        reCalculateAll( dataGridView1 );
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                        // 直営費金額
                        AmountReg[0] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount0"].Value );
                    break;

                case 9:
                    if( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false )
                        reCalculateAll( dataGridView1 );
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                        // 下請外注費金額
                        AmountReg[1] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount1"].Value );
                    break;

                case 11:
                    if( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly == false )
                        reCalculateAll( dataGridView1 );
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                        // 資材費金額
                        AmountReg[2] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount2"].Value );
                    break;

                case 12:
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                        // 計
                        AmountReg[3] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Sum"].Value );
                    break;

                default:
                    break;
            }

            updateStat = true;
        }


        private void dataGridView1_RowEnter( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;   // 初期化中

            DataGridView dgv = ( DataGridView )sender;

            // 直営費金額
            AmountReg[0] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount0"].Value );
            // 下請外注費金額
            AmountReg[1] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount1"].Value );
            // 資材費金額
            AmountReg[2] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount2"].Value );
            // 計
            AmountReg[3] = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Sum"].Value );
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // 対応する見積データの版数
        private void create_cbEVersion()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxEVersion );
            if( cbe.Version( "D_Estimate", "WHERE TaskEntryID = " + Convert.ToString( ted.TaskEntryID ) + " ORDER BY VersionNo ASC" ) )
            {
                buttonFromEstimate.Enabled = ( string.IsNullOrEmpty( cbe.ValueItem[0] ) ) ? false : true;
                if( buttonFromEstimate.Enabled )
                {
                    comboBoxEVersion.Visible = true;
                    comboBoxEVersion.Enabled = true;
                }
                else
                {
                    labelMsg.Text = msgNoEstimate;
                    comboBoxEVersion.Visible = false;
                    comboBoxEVersion.Enabled = false;
                }
            }
            else
            {
                comboBoxEVersion.Text = "-";
            }
            
        }


        private void setButtonEnabled()
        {
            // I/Fデータ(plnd)の既存データ数確認
            endPoint = -1;
            for( int i = 0; i < plnd.Length; i++ ) if( plnd[i].PlanningID > 0 ) endPoint++;

            if( plnd[idx].PlanningID > 0 )
            {
                buttonOWrite.Enabled = true;
                buttonNWrite.Enabled = true;
                buttonDelete.Enabled = true;
                //buttonFromEstimate.Enabled = false;
            }
            else
            {
                buttonOWrite.Text = "保存";
                buttonOWrite.Enabled = true;
                buttonNWrite.Enabled = false;
                buttonDelete.Enabled = false;
                //buttonFromEstimate.Enabled = true;
            }
        }


        private void clearLabelText()
        {
            labelSales.Text = "";
            labelBudgets.Text = "";
            labelCostR.Text = "";
            labelTax.Text = "";
            labelDirect.Text = "";
            labelOutS.Text = "";
            labelMatel.Text = "";
            labelSum.Text = "";
            labelOther.Text = "";
            labelAdmCost.Text = "";
            labelTotal.Text = "";
        }


        // 内訳データ表示初期設定
        private void initialDisplayPlannignContData()
        {
            labelPublisher.Text = ted.OfficeName + ted.DepartName;    // 発行部署
            labelTaskCode.Text = ted.TaskCode;                         // 業務番号
            labelTask.Text = ted.CostType + "  " + ted.TaskName;      // 原価区分+業務名
            labelWorkingPlace.Text = ted.TaskPlace;                    // 業務場所
            labelPartner.Text = ted.PartnerName;                       // 発注者
            labelSales.Text = decFormat( plnd[idx].Sales );                              // 請負金額
            labelTax.Text = decFormat( plnd[idx].Sales * ted.TaxRate ) + " (消費税）";   // 消費税額
            labelBudgets.Text = decFormat( plnd[idx].Budgets );                          // 実行予算額
            labelCostR.Text = ( plnd[idx].Budgets == 0 || plnd[idx].Sales == 0 ) ? "" : decPointFormat( ( plnd[idx].Budgets / plnd[idx].Sales * 100 ) );     // 原価率

            labelOther.Text = decFormat( plnd[idx].Other );                            // その他
            labelAdmCost.Text = decFormat( plnd[idx].AdmCost );                        // 一般管理費

            labelTitle.Text = labelArray[idx];
            buttonPrint.Text = labelArray[idx] + "印刷";
        }


        // 内訳データ読込GridView表示
        private bool loadPlanningContData( DataGridView dgv )
        {
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );
            EstPlanOp epo = new EstPlanOp();
            if( plnd[idx].PlanningID > 0 )
            {
                DataTable dt = epo.PlanningCont_Select( plnd[idx].PlanningID );
                if( dt == null ) return false;
                if( dt.Rows.Count > iniRCnt ) dgv.Rows.Add( dt.Rows.Count - iniRCnt );
                if( !viewPlanningContToDgv( dt, dgv ) ) return false;
            }
            buttonNumbering();
            reCalculateAll( dgv );
            return true;
        }


        private bool viewPlanningContToDgv( DataTable dt, DataGridView dgv )
        {
            DataRow dr;
            decimal qty;
            Calculation calc = new Calculation();

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                dgv.Rows[i].Cells["Code"].Value = Convert.ToString( dr["ItemCode"] );
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString( dr["Item"] );
                dgv.Rows[i].Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );
                qty = Convert.ToDecimal( dr["Quantity"] );

                if( Convert.ToString( dr["Unit"] ) == "" )
                {
                    dgv.Rows[i].Cells["Quantity"].ReadOnly = true;
                    dgv.Rows[i].Cells["Quantity"].Value = null;
                    dgv.Rows[i].Cells["Unit"].ReadOnly = true;
                    dgv.Rows[i].Cells["Cost0"].ReadOnly = true;
                    dgv.Rows[i].Cells["Cost1"].ReadOnly = true;
                    dgv.Rows[i].Cells["Cost2"].ReadOnly = true;
                }
                else
                {
                    if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) == Sign.Expenses )
                    {
                        dgv.Rows[i].Cells["Quantity"].ReadOnly = false;
                        dgv.Rows[i].Cells["Unit"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost0"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost0"].Value = null;
                        dgv.Rows[i].Cells["Cost1"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost1"].Value = null;
                        dgv.Rows[i].Cells["Cost2"].ReadOnly = true;
                        dgv.Rows[i].Cells["Cost2"].Value = null;
                    }
                    else
                    {
                        dgv.Rows[i].Cells["Quantity"].ReadOnly = false;
                        dgv.Rows[i].Cells["Unit"].ReadOnly = false;
                        dgv.Rows[i].Cells["Cost0"].ReadOnly = false;
                        dgv.Rows[i].Cells["Cost1"].ReadOnly = false;
                        dgv.Rows[i].Cells["Cost2"].ReadOnly = false;
                    }

                    dgv.Rows[i].Cells["Quantity"].Value = qty;
                    dgv.Rows[i].Cells["Unit"].Value = Convert.ToString( dr["Unit"] );
                    if( qty != 0 )
                    {
                        for( int j = 0; j < 3; j++ )
                        {
                            if( Convert.ToDecimal( dr["Cost" + j.ToString()] ) != 0 )
                            {
                                dgv.Rows[i].Cells["Cost" + j.ToString()].Value = decFormat( Convert.ToDecimal( dr["Cost" + j.ToString()] ) );
                                dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat( qty * Convert.ToDecimal( dr["Cost" + j.ToString()] ) );
                            }
                        }

                    }
                }

                if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) == Sign.Discount )
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        dgv.Rows[i].Cells["Amount" + j.ToString()].ReadOnly = false;
                        dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat( Convert.ToDecimal( dr["Cost" + j.ToString()] ) );
                    }
                }
                else
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        dgv.Rows[i].Cells["Amount" + j.ToString()].ReadOnly = true;
                    }

                }
            }
            return true;
        }


        private bool viewEstimateContToDgv( DataTable dt, DataGridView dgv )
        {
            string wkCode;
            string arrayNo;
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                wkCode = Convert.ToString( dr["ItemCode"] );
                dgv.Rows[i].Cells["Code"].Value = wkCode;
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString( dr["Item"] );
                dgv.Rows[i].Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );
                if( Convert.ToString( dr["Unit"] ) != "" )
                {
                    dgv.Rows[i].Cells["Quantity"].Value = decPointFormat( Convert.ToDecimal( dr["Quantity"] ) );
                    dgv.Rows[i].Cells["Unit"].Value = string.IsNullOrEmpty( Convert.ToString( dr["Unit"] ) )? null : Convert.ToString(dr["Unit"]);

                    arrayNo = "0";
                    if( !string.IsNullOrEmpty( wkCode ) )
                    {
                        if( DHandling.IsNumeric( wkCode ) )
                        {
                            if( Convert.ToInt32( wkCode ) > 799 ) arrayNo = "2";
                        }
                        else
                        {
                            if( wkCode[0] == 'G' ) arrayNo = "2";
                        }
                    }
                    
                    dgv.Rows[i].Cells["Cost" + arrayNo].Value = decFormat( Convert.ToDecimal( dr["Cost"] ) );
                }
            }
            return true;
        }


        // 作業項目マスタデータをItemList画面から得る
        private void chooseItemData( DataGridView dgv )
        {
            WorkItemsData wids = FormItemList.ReceiveItems( wid );
            if( wids == null ) return;

            viewItemDataToDgv( dgv.Rows[dgv.CurrentCellAddress.Y], wids );

            if( wids.Unit != "" ) dgv.CurrentCell = dgv.Rows[dgv.CurrentCellAddress.Y].Cells["Quantity"];

            Calculation calc = new Calculation();
            if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[dgv.CurrentCellAddress.Y].Cells["Item"].Value ) ) != null )
                verticalCalc( dgv );
        }



        // DataGridViewに選択された作業項目マスタデータの内容セット
        private void viewItemDataToDgv( DataGridViewRow dgvRow, WorkItemsData wids )
        {
            Calculation calc = new Calculation();

            // データグリッドビューの編集ロック制御
            if( wids.Unit == "" )
            {
                dgvRow.Cells["Quantity"].ReadOnly = true;
                dgvRow.Cells["Quantity"].Value = null;
                dgvRow.Cells["Unit"].ReadOnly = true;
                dgvRow.Cells["Cost0"].ReadOnly = true;
                dgvRow.Cells["Cost1"].ReadOnly = true;
                dgvRow.Cells["Cost2"].ReadOnly = true;
                dgvRow.Cells["Amount0"].Value = null;
                dgvRow.Cells["Amount1"].Value = null;
                dgvRow.Cells["Amount2"].Value = null;
            }
            else
            {
                if( calc.ExtractCalcWord( wids.UItem ) == Sign.Expenses )
                {
                    dgvRow.Cells["Quantity"].ReadOnly = true;
                    dgvRow.Cells["Quantity"].Value = null;
                    dgvRow.Cells["Unit"].ReadOnly = true;
                    dgvRow.Cells["Cost0"].ReadOnly = true;
                    dgvRow.Cells["Cost1"].ReadOnly = true;
                    dgvRow.Cells["Cost2"].ReadOnly = true;
                }
                else
                {
                    dgvRow.Cells["Quantity"].ReadOnly = false;
                    dgvRow.Cells["Unit"].ReadOnly = false;
                    dgvRow.Cells["Cost0"].ReadOnly = false;
                    dgvRow.Cells["Cost1"].ReadOnly = false;
                    dgvRow.Cells["Cost2"].ReadOnly = false;
                }
            }

            if( calc.ExtractCalcWord( wids.UItem ) == Sign.Discount )
            {
                dgvRow.Cells["Amount0"].ReadOnly = false;
                dgvRow.Cells["Amount1"].ReadOnly = false;
                dgvRow.Cells["Amount2"].ReadOnly = false;
            }
            else
            {
                dgvRow.Cells["Amount0"].ReadOnly = true;
                dgvRow.Cells["Amount1"].ReadOnly = true;
                dgvRow.Cells["Amount2"].ReadOnly = true;
            }

            dgvRow.Cells["Code"].Value = wids.ItemCode;
            dgvRow.Cells["Item"].Value = wids.UItem == "" ? wids.Item : wids.UItem;
            dgvRow.Cells["ItemDetail"].Value = wids.ItemDetail;
            dgvRow.Cells["Unit"].Value = wids.Unit;
            string arrayNo = "0";
            if( DHandling.IsNumeric( wids.ItemCode ) )
            {
                if( Convert.ToInt32( wids.ItemCode ) > 799 ) arrayNo = "2";
            }
            else
            {
                if( wids.ItemCode[0] == 'G' ) arrayNo = "2";
            }

            if( wids.Unit != "" )
                if( calc.ExtractCalcWord( wids.UItem ) != Sign.Expenses )
                    dgvRow.Cells["Cost" + arrayNo].Value = decFormat( wids.StdCost );
        }


        private void restructPlanningData()
        {
            PlanningData plndTemp = new PlanningData();
            plndTemp = ( PlanningData )plnd[idx].Clone();       // 基データを一時退避
            idxBackup = idx;
            if( endPoint == 0 ) idx = 1;
            if( endPoint == 1 ) idx = 2;
            if( endPoint > 1 )
            {
                if( idx > 0 )
                {
                    plnd[1] = ( PlanningData )plnd[2].Clone();
                }
                idx = 2;
            }

            plnd[idx] = ( PlanningData )plndTemp.Clone();
            plnd[idx].PlanningID = -1;

            labelTitle.Text = labelArray[idx];
            buttonPrint.Text = labelArray[idx] + "印刷";
        }


        private void storePlanningAndContents( DataGridView dgv )
        {
            EstPlanOp epo = new EstPlanOp();
            plnd[idx].VersionNo = plnd[idx].MaxVersion + 1;
            plnd[idx].TaskEntryID = ted.TaskEntryID;
            plnd[idx].OfficeCode = ted.OfficeCode;
            plnd[idx].Department = ted.Department;
            plnd[idx].Publisher = ted.OfficeCode + ted.Department;
            plnd[idx].PlanningID = epo.Planning_Insert( plnd[idx] );
            if( plnd[idx].PlanningID < 0 ) return;
            if( !epo.PlanningCont_Insert( dgv, plnd[idx].PlanningID ) ) return;
            buttonOWrite.Enabled = true;
            buttonNWrite.Enabled = false;       // 新規保存利用不可
            labelMsg.Text = "保存しました。";
        }


        private void updatePlanningAndContents( DataGridView dgv )
        {
            EstPlanOp epo = new EstPlanOp();
            if( !epo.Planning_Update( plnd[idx] ) ) return;
            if( !epo.PlanningCont_Delete( plnd[idx].PlanningID ) ) return;
            if( !epo.PlanningCont_Insert( dgv, plnd[idx].PlanningID ) ) return;
            labelMsg.Text = "更新しました。";
        }


        private void deletePlanningAndContents()
        {
            EstPlanOp epo = new EstPlanOp();
            if( !epo.Planning_Delete( plnd[idx].PlanningID ) ) return;
            if( !epo.PlanningCont_Delete( plnd[idx].PlanningID ) ) return;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            buttonNumbering();
            reCalculateAll( dataGridView1 );

            plnd[idx].PlanningID = 0;
            plnd[idx].Sales = 0;
            plnd[idx].Budgets = 0;
            plnd[idx].Discussion = "";
            plnd[idx].OfficeCode = "";
            plnd[idx].Department = "";
            plnd[idx].Publisher = "";

            clearLabelText();
            labelMsg.Text = "削除しました。";
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private void reCalculateAll( DataGridView dgv )
        {
            horizontalCalc( dgv );
            verticalCalc( dgv );
        }


        private void horizontalCalc( DataGridView dgv )
        {
            Calculation calc = new Calculation( ted );
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                if( dgv.Rows[i].Cells["Item"].Value != null || dgv.Rows[i].Cells["ItemDetail"].Value != null )
                    calc.HCalcPlanningContRow( dgv.Rows[i] );
            }
        }


        // DataGridViewの縦計算
        private void verticalCalc( DataGridView dgv )
        {
            Calculation calc = new Calculation( ted );
            calc.VCalcPlanningCont( dgv );
            labelDirect.Text = decFormat( calc.SumArray[0] );
            plnd[idx].Direct = calc.SumArray[0];
            labelOutS.Text = decFormat( calc.SumArray[1] );
            plnd[idx].OutS = calc.SumArray[1];
            labelMatel.Text = decFormat( calc.SumArray[2] );
            plnd[idx].Matel = calc.SumArray[2];
            decimal sum = 0;
            for( int i = 0; i < 3; i++ ) sum += calc.SumArray[i];
            labelSum.Text = decFormat( sum );
            plnd[idx].Sum = sum;
            labelTotal.Text = decFormat( sum + ( sum * ted.OthersCostRate ) + ( sum * ted.AdminCostRate ) );
            labelBudgets.Text = decFormat( plnd[idx].Direct + plnd[idx].OutS + plnd[idx].Matel );
            plnd[idx].Budgets = plnd[idx].Direct + plnd[idx].OutS + plnd[idx].Matel;
            labelCostR.Text = ( plnd[idx].Budgets == 0 || plnd[idx].Sales == 0 ) ? "" : decPointFormat( ( plnd[idx].Budgets / plnd[idx].Sales * 100 ) );        // 原価率

            sum = 0;
            decimal WorkDecimal = 0;

            for( int i = 0; i < this.dataGridView1.Rows.Count; i++ )
            {
                if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) ) != null )
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        decimal.TryParse( Convert.ToString( dgv.Rows[i].Cells["Amount" + j.ToString()].Value ), out WorkDecimal );
                        sum += WorkDecimal;
                    }
                    dgv.Rows[i].Cells["Sum"].Value = decFormat( sum );
                }
                sum = 0;
            }
        }


        private void formatNumericValue( DataGridView dgv )
        {
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Code"].Value ) ) )
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        if( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Cost" + j.ToString()].Value ) ) )
                        {
                            dgv.Rows[i].Cells["Amount" + j.ToString()].Value = "";
                        }
                        else
                        {
                            dgv.Rows[i].Cells["Cost" + j.ToString()].Value = decFormat( Convert.ToDecimal( dgv.Rows[i].Cells["Cost" + j.ToString()].Value ) );
                            dgv.Rows[i].Cells["Amount" + j.ToString()].Value = decFormat( Convert.ToDecimal( dgv.Rows[i].Cells["Amount" + j.ToString()].Value ) );
                        }
                    }
                }
            }
        }


        // DataGridViewButtonの番号を再採番
        private void buttonNumbering()
        {
            int startNo = 1;
            for( int i = 0; i < dataGridView1.RowCount; i++ )
            {
                dataGridView1.Rows[i].Cells[0].Value = ( startNo + i ).ToString();
            }
        }


        private bool loadEstimateData()
        {
            EstPlanOp epo = new EstPlanOp();
            int verNo = ( String.IsNullOrEmpty( comboBoxEVersion.Text ) ) ? 0 : Convert.ToInt32( comboBoxEVersion.Text );
            DataTable dt = epo.Estimate_Select( ted.TaskEntryID, verNo );
            if( dt == null ) return false;
            DataRow dr = dt.Rows[0];
            EstimateData estd = new EstimateData( dr );

            dt = epo.EstimateCont_Select( estd.EstimateID );
            if( dt == null ) return false;
            viewEstimateContToDgv( dt, dataGridView1 );
            viewEstimateData( estd );

            viewPlanningContSummary();
            return true;
        }


        private void viewEstimateData( EstimateData estd )
        {
            plnd[idx] = new PlanningData();

            plnd[idx].Sales = estd.Total;
            plnd[idx].EstimateID = estd.EstimateID;
            plnd[idx].EstimateVer = estd.VersionNo;
        }


        private void viewPlanningContSummary()
        {
            decimal qty = 0;
            decimal direct = 0;
            decimal outs = 0;
            decimal matel = 0;
            SqlHandling sql = new SqlHandling( "D_PlanningCont" );
            DataTable dt;
            DataRow dr;

            for( int i = 0; i < plnd.Length; i++ )
            {
                if( plnd[i] == null ) break;
                if( plnd[i].PlanningID == 0 ) break;
                dt = sql.SelectAllData( "WHERE PlanningID = " + plnd[i].PlanningID );
                if( dt == null ) return;

                direct = 0;
                outs = 0;
                matel = 0;
                for( int j = 0; j < dt.Rows.Count; j++ )
                {
                    dr = dt.Rows[j];
                    qty = Convert.ToDecimal( dr["Quantity"] );
                    direct += Convert.ToDecimal( dr["Cost0"] ) * qty;
                    outs += Convert.ToDecimal( dr["Cost1"] ) * qty;
                    matel += Convert.ToDecimal( dr["Cost2"] ) * qty;
                }
            }
            return;
        }


        private void editDGVRowCell( DataGridViewRow dgvRow, int cellIndex )
        {
            decimal WorkDecimal = 0;

            decimal.TryParse( Convert.ToString( dgvRow.Cells[cellIndex].Value ), out WorkDecimal );
            if( WorkDecimal == 0 )
            {
                dgvRow.Cells[cellIndex].Value = "";
            }
            else
            {
                dgvRow.Cells[cellIndex].Value = ( cellIndex == 4 ) ? decPointFormat( WorkDecimal ) : decFormat( WorkDecimal );
            }
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private static string decPointFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "0.00" );
        }
    }
}

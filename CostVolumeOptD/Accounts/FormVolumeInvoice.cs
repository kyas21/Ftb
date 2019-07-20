﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;

namespace Accounts
{
    public partial class FormVolumeInvoice :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        WorkItemsData[] wid;
        TaskEntryData ted;
        AccountData[] acnd;
        private bool iniPro = true;
        private int iniRCnt = 29;
        private int cpg = 0;            // Current Page Number
        private DateTime nowDate;
        private decimal acntAmount;
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormVolumeInvoice()
        {
            InitializeComponent();
        }

        public FormVolumeInvoice( TaskEntryData ted )
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


        private void FormVolumeInvoice_Load( object sender, EventArgs e )
        {
            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();

            DateTime dtNow = DateTime.Now;
            iniRCnt = Convert.ToInt32( ( dtNow.EndOfMonth() ).Day ) - 1;
            dataGridView1.Rows.Add( iniRCnt );

            edit_Labels();
            create_dtPicker();

            // 作業項目マスタの一覧作成
            EstPlanOp ep = new EstPlanOp();
            wid = ep.StoreWorkItemsData( ted.MemberCode );

            initialProcessing( 0 );

            nowMonth();
        }


        private void FormVolumeInvoice_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            
            nowMonth( dateTimePickerRecordedDate.Value );
            if( iniPro ) return;

            Button btn = ( Button )sender;
            BillingOp blo = new BillingOp();

            switch( btn.Name )
            {

                case "buttonFromEstimate":
                    Func<DialogResult> dialogNewLoad = DMessage.DialogNewLoad;
                    if( dialogNewLoad() == DialogResult.No ) return;
                    loadEstimateData();
                    reCalculateAll();
                    labelMsg.Text = "見積書データを取り込みました。";
                    break;
                case "buttonPrevData":
                    if( cpg != 0 )
                    {
                        cpg--;
                        viewData( cpg );
                    }
                    break;
                case "buttonNextData":
                    if( cpg != ( acnd.Length - 1 ) )
                    {
                        cpg++;
                        viewData( cpg );
                    }
                    break;
                case "buttonCopyAndNext":
                    clearAccountContDataView();
                    AccountData[] newAcnd = new AccountData[acnd.Length + 1];
                    Array.Copy( acnd, newAcnd, Math.Min( acnd.Length, newAcnd.Length ) );
                    newAcnd[newAcnd.Length - 1] = ( AccountData )acnd[cpg].Clone();
                    //newAcnd[newAcnd.Length - 1].AccountID = 0;
                    acnd = newAcnd;

                    buttonGrpDisabled();
                    buttonPrevData.Enabled = true;
                    buttonNew.Enabled = true;

                    cpg++;

                    editViewNextData( dataGridView1 );

                    acnd[cpg].RecordedDate = dateTimePickerRecordedDate.Value.AddMonths( 1 );
                    dateTimePickerRecordedDate.Value = acnd[cpg].RecordedDate;
                    break;
                case "buttonReCalc":
                    reCalculateAll();
                    labelMsg.Text = "再計算しました。";
                    break;
                case "buttonOverWrite":
                    Func<DialogResult> dialogOverWrite = DMessage.DialogOverWrite;
                    if( dialogOverWrite() == DialogResult.No ) return;

                    editAccountData( acnd[cpg], cpg );
                    blo.Account_Update( acnd[cpg] );
                    blo.AccountCont_Update( dataGridView1, acnd[cpg] );

                    labelMsg.Text = "上書しました。";
                    break;
                case "buttonNew":
                    if( acnd == null )
                    {
                        acnd = new AccountData[1];
                        cpg = 0;
                        acnd[cpg] = new AccountData();
                    }
                    editAccountData( acnd[cpg], cpg );
                    acnd[cpg].AccountID = blo.Account_Insert( acnd[cpg] );
                    if( acnd[cpg].AccountID < 1 ) return;
                    if( !blo.AccountCont_Insert( dataGridView1, acnd[cpg] ) ) return;

                    initialProcessing( cpg );
                    labelMsg.Text = "新規に書込みました。";
                    break;
                case "buttonDelete":
                    Func<DialogResult> dialogDelete = DMessage.DialogDelete;
                    if( dialogDelete() == DialogResult.No ) return;
                    //blo.AccountCont_Delete( acnd[cpg] );
                    blo.AccountCont_Delete( acnd[cpg].AccountID );
                    blo.Account_Delete(acnd[cpg].AccountID);
                    if( acnd.Length > 1 )
                    {
                        if( cpg == ( acnd.Length - 1 ) ) cpg--;
                    }

                    initialProcessing( cpg );
                    labelMsg.Text = "削除しました。";
                    break;
                case "buttonCancel":
                    Func<DialogResult> dialogCancel = DMessage.DialogCancel;
                    if( dialogCancel() == DialogResult.No ) return;
                    viewData( cpg );
                    break;
                case "buttonPrint":
                    FormInvoice formInvoice = new Accounts.FormInvoice();
                    formInvoice.CreateExcelFile( ted, dataGridView1, "出来高請求書.xlsx", "VolumeInvoice" );
                    break;
                case "buttonClose":
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        // Short-Cut Key
        // 前提：コントロールはどこにあっても良い
        private void FormVolumeInvoice_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;

            switch( e.KeyCode )
            {
                case Keys.F5:
                    reCalculateAll();
                    break;
                default:
                    break;
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.R:
                    reCalculateAll();
                    break;
                default:
                    break;
            }
        }


        // Cellの内容に変化があったとき
        private void dataGridView1_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;   // 初期化中

            DataGridView dgv = ( DataGridView )sender;
            Calculation calc = new Calculation();
            switch( e.ColumnIndex )
            {
                case 5:     // 「数量」列
                    calc = new Calculation();
                    if( calc.ExtractCalcWord( Convert.ToString( dgv.Rows[e.RowIndex].Cells["Item"].Value ) ) != null ) verticalCalc();
                    //calc.HCalcEstimateRow( dgv.Rows[e.RowIndex] );
                    calc.HCalcInvoiceRow( dgv.Rows[e.RowIndex] );
                    break;

                case 9:     // 「入力金額」列
                    reCalculateAll();
                    break;

                default:
                    break;
            }
        }


        // [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        // 前提：コントロールがDataGridViewにある時
        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;   // 初期化中

            DataGridView dgv = ( DataGridView )sender;
            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    chooseItemData();
                    dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                    break;
                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;
                case Keys.I:
                    break;
                case Keys.R:
                    reCalculateAll();
                    break;
                default:
                    break;
            }
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( acnd == null ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            switch( dtp.Name )
            {
                case "dateTimePickerRecDate":
                    nowMonth( dateTimePickerRecordedDate.Value );
                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // Label編集
        private void edit_lblPageNo( int idx )
        {
            if( acnd == null ) return;
            labelPageNo.Text = ( idx + 1 ).ToString() + " / " + acnd.Length.ToString();
        }


        private void edit_Labels()
        {
            labelPublisher.Text = ted.OfficeName + ted.DepartName;    // 発行部署
            labelTask.Text = ted.TaskCode + " " + ted.TaskName;
            labelPartnerName.Text = ted.PartnerName;
            labelPartnerCode.Text = ted.PartnerCode;
        }


 
        private void create_dtPicker()
        {
            dateTimePickerRecordedDate.Value = DateTime.Today;
            nowDate = dateTimePickerRecordedDate.Value;
        }


        private void nowMonth()
        {
            labelMsg.Text = Convert.ToString( nowDate.Month ) + "月分を処理します";
        }

        private void nowMonth( DateTime ymd )
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
            buttonPrevData.Enabled = ( acnd.Length > 1 ) ? true : false;
            buttonNextData.Enabled = ( acnd.Length > 1 ) ? true : false;
            buttonCopyAndNext.Enabled = true;
        }


        private void initialProcessing( int idx )
        {
            readyDataGrieView();
            if( loadExistingAccountData() )
            {
                viewData( idx );
                buttonGrpEnabled();
                buttonNew.Enabled = false;
            }
            else
            {
                DMessage.DataNotExistence( "空の状態で表示します。" );
                buttonGrpDisabled();
                buttonNew.Enabled = true;
            }
        }


        private void readyDataGrieView()
        {
            dataGridView1.Rows.Clear();
            DateTime dtNow = DateTime.Now;
            iniRCnt = Convert.ToInt32( ( dtNow.EndOfMonth() ).Day ) - 1;
            dataGridView1.Rows.Add( iniRCnt );
        }


        private void viewData( int idx )
        {
            edit_Labels();

            dateTimePickerRecordedDate.Value = acnd[idx].RecordedDate.StripTime();
            nowMonth( dateTimePickerRecordedDate.Value );

            loadAccountContData( acnd[idx] );
            reCalculateAll();
            edit_lblPageNo( idx );
        }


        // Load Existing Data 既存データの読取表示
        private bool loadExistingAccountData()
        {
            string wParam = " WHERE ";
            wParam += ( string.IsNullOrEmpty( ted.TaskCode.Trim() ) ) ? " TaskEntryID = " + ted.TaskEntryID : " TaskCode = '" + ted.TaskCode + "'";
            wParam += " AND InvoiceType = 0";

            BillingOp blo = new BillingOp();
            DataTable dt = blo.UsingParamater_Select( "D_Account", wParam );
            if( dt == null ) return false;
            acnd = new AccountData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) acnd[i] = new AccountData( dt.Rows[i] );
            return true;
        }


        private bool loadAccountContData( AccountData acnd )
        {
            if( acnd.AccountID == 0 ) return false;

            BillingOp blo = new BillingOp();
            string wParam = " WHERE AccountID = " + acnd.AccountID + " ORDER BY LNo";
            DataTable dt = blo.UsingParamater_Select( "D_AccountCont", wParam );
            if( dt == null ) return false;

            readyDgv( dt.Rows.Count );

            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                viewAccountContToDgv( dr, dataGridView1.Rows[i] );
                //viewAccountContSumToDgv( dr, dataGridView1.Rows[i] );
            }
            return true;
        }


        // View  Data
        private void viewAccountContToDgv( DataRow dr, DataGridViewRow dgvRow )
        {
            DateTime pdt = DHandling.StripTime( Convert.ToDateTime( dr["WorkDate"] ) );
            if( pdt == DateTime.MinValue )
            {
                dgvRow.Cells["Month"].Value = "";
                dgvRow.Cells["Day"].Value = "";
            }
            else
            {
                dgvRow.Cells["Month"].Value = Convert.ToString( pdt.Month );
                dgvRow.Cells["Day"].Value = Convert.ToString( pdt.Day );
            }

            dgvRow.Cells["ItemCode"].Value = Convert.ToString( dr["ItemCode"] );
            dgvRow.Cells["Item"].Value = Convert.ToString( dr["Item"] );
            dgvRow.Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );

            if( Convert.ToString( dr["Unit"] ) != "" )
            {
                //契約項目
                decimal qty = Convert.ToDecimal( dr["CQuantity"] );
                decimal cost = Convert.ToDecimal( dr["CCost"] );
                dgvRow.Cells["CQuantity"].Value = decPointFormat( qty );
                dgvRow.Cells["Unit"].Value = Convert.ToString( dr["Unit"] );
                dgvRow.Cells["CCost"].Value = decFormat( cost );
                dgvRow.Cells["CAmount"].Value = decFormat( qty * cost );

                //前月までの請求
                dgvRow.Cells["SQuantity"].Value = ( string.IsNullOrEmpty( Convert.ToString( dr["SQuantity"] ) ) ) ? "0" : decPointFormat( Convert.ToDecimal( dr["SQuantity"] ) );
                dgvRow.Cells["SAmount"].Value = ( string.IsNullOrEmpty( Convert.ToString( dr["SAmount"] ) ) ) ? "0" : decFormat( Convert.ToDecimal( dr["SAmount"] ) );

                //当月数量、金額
                dgvRow.Cells["Quantity"].Value = decPointFormat( Convert.ToDecimal( dr["Quantity"] ) );

                if(cost != 0 && Convert.ToDecimal(dr["Amount"]) == 0 )
                {
                    dgvRow.Cells["Amount"].Value = decFormat( cost * Convert.ToDecimal( dr["Quantity"] ) );
                }
                else
                {
                    dgvRow.Cells["Amount"].Value = decFormat( Convert.ToDecimal( dr["Amount"] ) );
                }

                //マニュアル入力金額
                dgvRow.Cells["HAmount"].Value = ( Convert.ToDecimal( dr["HAmount"] ) > 0 ) ? decFormat( Convert.ToDecimal( dr["HAmount"] ) ) : "";

                //マニュアル契約金額
                dgvRow.Cells["HContract"].Value = ( Convert.ToDecimal( dr["HContract"] ) > 0 ) ? decFormat( Convert.ToDecimal( dr["HContract"] ) ) : "";
            }
        }


        private void viewAccountContSumToDgv( DataRow idr, DataGridViewRow dgvRow )
        {
            string sqlStr = " SUM( ISNULL(Quantity,0) ) AS SQty,SUM( ISNULL(Amount,0) ) AS SAmt FROM D_AccountCont WHERE "
                + " AccountID = " + Convert.ToString( idr["AccountID"] )
                + " AND ItemCode = '" + Convert.ToString( idr["ItemCode"] ) + "'"
                + " AND Item = '" + Convert.ToString( idr["Item"] ) + "'"
                + " AND ItemDetail = '" + Convert.ToString( idr["ItemDetail"] ) + "'"
                + " AND RecordedDate < '" + Convert.ToDateTime( idr["RecordedDate"] ) + "'";

            BillingOp blo = new BillingOp();
            DataTable dt = blo.UsingSqlstring_Select( sqlStr );
            if( dt == null ) return;
            DataRow dr = dt.Rows[0];
            dgvRow.Cells["SQuantity"].Value = string.IsNullOrEmpty( Convert.ToString( dr["SQty"] ) ) ? "0" : decPointFormat( Convert.ToDecimal( dr["SQty"] ) );
            dgvRow.Cells["SAmount"].Value = string.IsNullOrEmpty( Convert.ToString( dr["SAmt"] ) ) ? "0" : decPointFormat( Convert.ToDecimal( dr["SAmt"] ) );
        }


        private void editAccountData( AccountData ad, int idx )
        {
            ad.AccountID = 0;
            ad.PartnerCode = ted.PartnerCode;
            ad.TaskCode = ( string.IsNullOrEmpty( ted.TaskCode ) ) ? "" : ted.TaskCode;
            ad.RecordedDate = dateTimePickerRecordedDate.Value.StripTime();
            ad.Amount = acntAmount;
            ad.InvoiceType = 0;
            ad.TaskEntryID = ted.TaskEntryID;
            ad.OfficeCode = ted.OfficeCode;
            ad.Department = ted.Department;

            PlanningData pld = new PlanningData();
            pld = pld.LatestPlanningData( ted.TaskEntryID );
            ad.CAmount = ( pld == null ) ? 0 : pld.Sales;

            if( idx == 0 )
            {
                ad.SAmount = ad.CAmount;
            }
            else
            {
                AccountData acndp = new AccountData();
                acndp = acndp.SelectAccountData( acnd[idx - 1].AccountID );
                ad.SAmount = ( acndp == null ) ? ad.CAmount : ad.CAmount + acndp.SAmount;
            }
        }


        // Other
        private void readyDgv( int rowCount )
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            if( rowCount > iniRCnt ) dataGridView1.Rows.Add( rowCount - iniRCnt );
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private void reCalculateAll()
        {
            // HorizontalCalculation
            decimal cost = 0;
            decimal qty = 0;
            decimal amt = 0;
            acntAmount = 0;
            for( int i = 0; i < dataGridView1.RowCount; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dataGridView1.Rows[i].Cells["HContract"].Value ) )
                    || !string.IsNullOrWhiteSpace( Convert.ToString( dataGridView1.Rows[i].Cells["HAmount"].Value ) ) )
                {
                    // 契約金額の変更、コストを空白にする
                    if( string.IsNullOrEmpty( Convert.ToString( dataGridView1.Rows[i].Cells["HContract"].Value ) )
                        || Convert.ToDecimal( dataGridView1.Rows[i].Cells["HContract"].Value ) == 0 )
                    {
                        dataGridView1.Rows[i].Cells["HContract"].Value = null;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["CCost"].Value = null;
                        dataGridView1.Rows[i].Cells["HContract"].Value = decFormat( Convert.ToDecimal( dataGridView1.Rows[i].Cells["HContract"].Value ) );
                        dataGridView1.Rows[i].Cells["CAmount"].Value = dataGridView1.Rows[i].Cells["HContract"].Value;
                    }
                    // 今回金額の変更、今回の数量を空白にする
                    if( string.IsNullOrEmpty( Convert.ToString( dataGridView1.Rows[i].Cells["HAmount"].Value ) )
                        || Convert.ToDecimal( dataGridView1.Rows[i].Cells["HAmount"].Value ) == 0 )
                    {
                        dataGridView1.Rows[i].Cells["HAmount"].Value = null;
                    }
                    else
                    {
                        //dataGridView1.Rows[i].Cells["Quantity"].Value = null;
                        dataGridView1.Rows[i].Cells["HAmount"].Value = decFormat( Convert.ToDecimal( dataGridView1.Rows[i].Cells["HAmount"].Value ) );
                        dataGridView1.Rows[i].Cells["Amount"].Value = dataGridView1.Rows[i].Cells["HAmount"].Value;

                        acntAmount += Convert.ToDecimal( dataGridView1.Rows[i].Cells["HAmount"].Value );
                    }
                }
                else
                {
                    // 契約時の金額計算
                    cost = DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["CCost"].Value ) );
                    qty = DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["CQuantity"].Value ) );
                    dataGridView1.Rows[i].Cells["CAmount"].Value = decFormat( qty * cost );
                }

                // 累積計算
                if( Convert.ToString( dataGridView1.Rows[i].Cells["Unit"].Value ) != "" )
                {
                    amt = DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["SAmount"].Value ) );
                    amt += DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["Amount"].Value ) );

                    dataGridView1.Rows[i].Cells["RAmount"].Value = decFormat( ( qty * cost ) - amt );
                }

                // ZEROを空欄にする
                if( DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["CAmount"].Value ) ) == 0 )
                    dataGridView1.Rows[i].Cells["CAmount"].Value = null;
                if( DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["Amount"].Value ) ) == 0 )
                    dataGridView1.Rows[i].Cells["Amount"].Value = null;
                if( DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["RAmount"].Value ) ) == 0 )
                {
                    if( Convert.ToString( dataGridView1.Rows[i].Cells["CAmount"].Value ) == "" &&
                        Convert.ToString( dataGridView1.Rows[i].Cells["SAmount"].Value ) == "" &&
                        Convert.ToString( dataGridView1.Rows[i].Cells["Amount"].Value ) == "" )
                        dataGridView1.Rows[i].Cells["RAmount"].Value = null;
                }


                if( DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["SAmount"].Value ) ) == 0 )
                    dataGridView1.Rows[i].Cells["SAmount"].Value = null;
                if( DHandling.ToRegDecimal( Convert.ToString( dataGridView1.Rows[i].Cells["SQuantity"].Value ) ) == 0 )
                    dataGridView1.Rows[i].Cells["SQuantity"].Value = null;
            }

            verticalCalc();
        }


        private void verticalCalc()
        {
            Calculation calc = new Calculation( ted );
            calc.VCalcEstimate( dataGridView1 );
        }


        private void clearAccountContDataView()
        {
            for( int i = 0; i < dataGridView1.RowCount; i++ )
            {
                if( dataGridView1.Rows[i].Cells["Unit"].Value != null )
                    dataGridView1.Rows[i].Cells["HAmount"].Value = null;
            }

            reCalculateAll();
        }


        // 作業項目マスタデータをItemList画面から得る
        private void chooseItemData()
        {
            WorkItemsData wids = FormItemList.ReceiveItems( wid );
            if( wids == null ) return;

            viewItemDataToDgv( dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y], wids );

            Calculation calc = new Calculation();
            if( calc.ExtractCalcWord( Convert.ToString( dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells["Item"].Value ) ) != null ) verticalCalc();
        }


        // DataGridViewに選択された作業項目マスタデータの内容セット
        private void viewItemDataToDgv( DataGridViewRow dgvRow, WorkItemsData wids )
        {
            dgvRow.Cells["ItemCode"].Value = wids.ItemCode;
            if( wids.UItem != "" )
            {
                dgvRow.Cells["Item"].Value = wids.UItem;
                dgvRow.Cells["CCost"].Value = null;
            }
            else
            {
                dgvRow.Cells["Item"].Value = wids.Item;
                dgvRow.Cells["CCost"].Value = ( wids.StdCost == 0 ) ? null : decFormat( wids.StdCost );
            }
            dgvRow.Cells["ItemDetail"].Value = wids.ItemDetail;
            dgvRow.Cells["Unit"].Value = wids.Unit;
        }


        private void loadEstimateData()
        {
            BillingOp blo = new BillingOp();
            string wParam = "WHERE TaskEntryID = " + Convert.ToString( ted.TaskEntryID ) + " ORDER BY VersionNo DESC";
            DataTable dt = blo.UsingParamater_Select( "D_Estimate", wParam );
            if( dt == null )
            {
                DMessage.SelectInvalid();
                return;
            }
            DataRow dr = dt.Rows[0];
            EstimateData estd = new EstimateData( dr );

            blo = new BillingOp();
            wParam = " WHERE EstimateID = " + Convert.ToString( estd.EstimateID );
            dt = blo.UsingParamater_Select( "D_EstimateCont", wParam );
            if( dt == null )
            {
                DMessage.SelectInvalid();
                return;
            }

            edit_Labels();
            readyDgv( dt.Rows.Count );

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                dataGridView1.Rows[i].Cells["ItemCode"].Value = Convert.ToString( dr["ItemCode"] );
                dataGridView1.Rows[i].Cells["Item"].Value = Convert.ToString( dr["Item"] );
                dataGridView1.Rows[i].Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );
                dataGridView1.Rows[i].Cells["Unit"].Value = Convert.ToString( dr["Unit"] );
                if( Convert.ToString( dr["Unit"] ) != "" )
                {
                    dataGridView1.Rows[i].Cells["CQuantity"].Value = decPointFormat( Convert.ToDecimal( dr["Quantity"] ) );
                    dataGridView1.Rows[i].Cells["CCost"].Value = decFormat( Convert.ToDecimal( dr["Cost"] ) );
                    dataGridView1.Rows[i].Cells["CAmount"].Value = decFormat( Convert.ToDecimal( dr["Cost"] ) * Convert.ToDecimal( dr["Quantity"] ) );

                    dataGridView1.Rows[i].Cells["Quantity"].Value = dataGridView1.Rows[i].Cells["CQuantity"].Value;
                    dataGridView1.Rows[i].Cells["Amount"].Value = dataGridView1.Rows[i].Cells["CAmount"].Value;
                }
            }

            reCalculateAll();
            return;
        }


        private void editViewNextData(DataGridView dgv )
        {
            for(int i = 0;i<dgv.Rows.Count;i++ )
            {
                if( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["CAmount"].Value ) ) ) continue;
                decimal sqty = ( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["SQuantity"].Value ) ) ) ? 0 
                    : DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["SQuantity"].Value ) );
                decimal samt = ( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["SAmount"].Value ) ) ) ? 0 
                    : DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["SAmount"].Value ) );
                decimal qty = ( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) ) ) ? 0 
                    : DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) );
                decimal amt = ( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) ) ) ? 0 
                    : DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) );
                dgv.Rows[i].Cells["SQuantity"].Value = decPointFormat( sqty + qty );
                dgv.Rows[i].Cells["SAmount"].Value = decFormat( samt + amt );


                decimal cqty = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["CQuantity"].Value ) );
                decimal camt = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["SAmount"].Value ) );
                qty = cqty - ( sqty + qty );
                amt = camt - ( samt + amt );
                dgv.Rows[i].Cells["Quantity"].Value = decPointFormat( qty );
                dgv.Rows[i].Cells["Amount"].Value = decFormat( amt );
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

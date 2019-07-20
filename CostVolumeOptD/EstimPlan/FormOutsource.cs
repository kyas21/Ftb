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

namespace EstimPlan
{
    public partial class FormOutsource :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        TaskEntryData ted;
        PlanningData plnd;
        OutsourceData[] osd;
        private bool iniPro = true;
        private int iniRCnt = 29;
        private int cpg;               // Current Index
        private string[] vPartnerAry;
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormOutsource( TaskEntryData ted )
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
        private void FormOutsource_Load( object sender, EventArgs e )
        {
            this.defaultCellStyle = new DataGridViewCellStyle();

            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();
            dataGridView1.Rows.Add( iniRCnt );
            buttonNumbering( dataGridView1 );

            create_cbOffice();                  // 事業所comboBox
            create_cbWork();                    // 部門ComboBox
            create_cbPartner();                 // 業者名comboBox
            labelTask.Text = ted.TaskName;      // 業務名Label
            create_lblTerm();                   // 工期
            labelTaskCode.Text = ted.TaskCode;  // 業務番号
            create_cbPayRoule();                // 支払基準comboBox
            edit_tbDeliveryPoint();             // 納品場所初期値

            initialSetbuttonFromPlan();
            initialViewSetting(0);
        }


        private void FormOutsource_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( osd == null ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            switch( dtp.Name )
            {
                case "dateTimePickerOderDate":
                    osd[cpg].OrderDate = dtp.Value;
                    break;
                case "dateTimePickerOStart":
                    osd[cpg].StartDate = dtp.Value;
                    break;
                case "dateTimePickerOEnd":
                    osd[cpg].EndDate = dtp.Value;
                    break;
                case "dateTimePickerInspectDate":
                    osd[cpg].InspectDate = dtp.Value;
                    break;
                case "dateTimePickerReceiptDate":
                    osd[cpg].ReceiptDate = dtp.Value;
                    break;
                default:
                    break;
            }
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            EstPlanOp epo = new EstPlanOp();
            switch( btn.Name )
            {
                case "buttonFromPlan":
                    Func<DialogResult> dialogNewLoad = DMessage.DialogNewLoad;
                    if( dialogNewLoad() == DialogResult.No ) return;

                    loadPlanningData( dataGridView1 );
                    buttonNumbering( dataGridView1 );
                    buttonFromPlan.Enabled = false;
                    break;
                case "buttonPrevData":
                    if( cpg > 0 )
                    {
                        cpg--;
                        viewData( cpg );
                    }
                    break;
                case "buttonNextData":
                    if( cpg < osd.Length - 1 )
                    {
                        cpg++;
                        viewData( cpg );
                    }
                    break;
                case "buttonCopyAndNext":
                    clearOutsourceContAmount( dataGridView1 );
                    OutsourceData[] newOsd = new OutsourceData[osd.Length + 1];
                    Array.Copy( osd, newOsd, Math.Min( osd.Length, newOsd.Length ) );
                    newOsd[newOsd.Length - 1] = ( OutsourceData )osd[cpg].Clone();
                    newOsd[newOsd.Length - 1].OutsourceID = 0;
                    osd = newOsd;

                    buttonGrpDisabled();
                    buttonPrevData.Enabled = true;
                    buttonNew.Enabled = true;

                    cpg++;
                    break;
                case "buttonClose":
                    this.Close();
                    break;
                case "buttonReCalc":
                    reCalculateAll( dataGridView1 );
                    break;
                case "buttonOverWrite":
                    Func<DialogResult> dialogOverWrite = DMessage.DialogOverWrite;
                    if( dialogOverWrite() == DialogResult.No ) return;

                    if( checkAndStoreLatestOutsourceData( cpg ) )
                    {
                        viewOutsourceDataToForm( cpg, dataGridView1 );
                    }
                    else
                    {
                        epo.UpdateOutsource( osd[cpg] );
                        epo.UpdateOutsourceCont( dataGridView1, osd[cpg].OutsourceID );
                    }
                    break;
                case "buttonNew":
                    storeLatestOutsourceData( cpg );
                    viewOutsourceDataToForm( cpg, dataGridView1 );
                    initialViewSetting(cpg);
                    //buttonGrpEnabled();
                    break;
                case "buttonDelete":
                    Func<DialogResult> dialogDelete = DMessage.DialogDelete;
                    if( dialogDelete() == DialogResult.No ) return;

                    epo.DeleteOutsourceCont( osd[cpg].OutsourceID );
                    epo.DeleteOutsource( osd[cpg].OutsourceID );
                    cpg--;
                    initialViewSetting(cpg);
                    break;
                case "buttonCancel":
                    Func<DialogResult> dialogCancel = DMessage.DialogCancel;
                    if( dialogCancel() == DialogResult.No ) return;

                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add( iniRCnt );
                    break;

                case "buttonPrintContent":
                    createExcelFile( "外注内訳書.xlsx", "OsContent" );
                    break;
                case "buttonPrintOrder":
                    createExcelFile( "注文書.xlsx", "OsOrder" );
                    break;
                case "buttonPrintConfirm":
                    createExcelFile( "注文請書.xlsx", "OsConfirm" );
                    break;
                default:
                    break;
            }
            edit_lblPageNo();
        }


        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            // DataGridView [ItemCode]コード列が非表示になっている←注意
            DataGridView dgv = ( DataGridView )sender;
            switch( e.KeyCode )
            {
                case Keys.Right:
                case Keys.Tab:
                    if( dgv.CurrentCellAddress.X == 4 ) SendKeys.Send( "{RIGHT}" );
                    if( dgv.CurrentCellAddress.X == 6 ) SendKeys.Send( "{RIGHT}" );
                    break;
                case Keys.Left:
                    if( dgv.CurrentCellAddress.X == 2 ) SendKeys.Send( "{RIGHT}" );
                    if( dgv.CurrentCellAddress.X == 6 ) SendKeys.Send( "{LEFT}" );
                    if( dgv.CurrentCellAddress.X == 8 ) SendKeys.Send( "{LEFT}" );
                    break;
                default:
                    break;
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                    break;
                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;
                case Keys.I:
                case Keys.D:
                    buttonNumbering( dgv );
                    break;
                case Keys.R:
                    reCalculateAll( dgv );
                    break;
                default:
                    break;
            }

        }


        private void dataGridView1_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;   // 初期化中

            DataGridView dgv = ( DataGridView )sender;
            switch( e.ColumnIndex )
            {
                case 4:     // 「数量」列
                case 6:     // 「単価」列
                    horizontalCalc( dgv );
                    break;
                default:
                    break;
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            ComboBox cbx = ( ComboBox )sender;
            switch( cbx.Name )
            {
                case "comboBoxOffice":
                case "comboBoxWork":
                    if( loadExistingOutsourceData() )
                    {
                        cpg = 0;
                        loadOutsourceContData( osd[cpg].OutsourceID, dataGridView1 );
                        viewOutsourceData( 0 );
                        buttonFromPlan.Enabled = false;     // 予算からの取込み禁止
                        edit_lblPageNo();
                    }
                    break;
                case "comboBoxPartner":
                    break;
                default:
                    break;
            }
            if( cbx.Name == "comboBoxOffice" ) create_cbWork();


        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // Label編集
        // 工期
        private void create_lblTerm()
        {
            if( DHandling.CheckDate( ted.StartDate ) )
            {
                labelTerm.Text = DHandling.PickUpTopCharacters( Convert.ToString( ted.StartDate ), 10 ) + "  ～  ";
                if( DHandling.CheckDate( ted.EndDate ) )
                    labelTerm.Text += DHandling.PickUpTopCharacters( Convert.ToString( ted.EndDate ), 10 );
            }
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
            comboBoxOffice.Text = ted.OfficeName;                        // 初期値
            comboBoxOffice.SelectedValue = ted.OfficeCode;
        }


        // 部門
        private void create_cbWork()
        {
            if( comboBoxOffice.Text != Sign.HQOffice ) comboBoxWork.Visible = false;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxWork );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            comboBoxWork.SelectedValue = ted.Department;

            if( comboBoxOffice.Text == Sign.HQOffice )
            {
                if( ted.Department == "0" ) comboBoxWork.SelectedValue = "2";
            }
            else
            {
                comboBoxWork.SelectedValue = "8";
            }
        }


        // 業者名(下請け業者のみ）
        private void create_cbPartner()
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxPartner );
            cb.TableData( "M_Partners", "PartnerCode", "PartnerName", " WHERE RelSubco = '1'" );

            vPartnerAry = new string[cb.ValueItem.Length];
            Array.Copy( cb.ValueItem, 0, vPartnerAry, 0, vPartnerAry.Length );
        }


        // 支払い基準
        private void create_cbPayRoule()
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxPayRoule );

            cb.ValueItem = new string[] { "0", "1" };
            cb.DisplayItem = new string[] { "出来高払い", "完成払い" };
            cb.Basic();
        }


        // TextBox納品場所の初期値設定
        private void edit_tbDeliveryPoint()
        {
            textBoxDeliveryPoint.Text = "フタバコンサルタント株式会社　" + ted.OfficeName;
            textBoxDeliveryPoint.Text += ( ted.OfficeCode == "H" ) ? ted.DepartName : "技術";
        }


        private void edit_lblPageNo()
        {
            if( osd == null ) return;
            labelPageNo.Text = ( cpg + 1 ).ToString() + " / " + osd.Length.ToString();
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
            buttonPrevData.Enabled = true;
            buttonNextData.Enabled = true;
            buttonCopyAndNext.Enabled = true;
        }


        // DataGridViewButtonの番号を再採番
        private void buttonNumbering( DataGridView dgv )
        {
            int startNo = 1;
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                dgv.Rows[i].Cells["Button"].Value = ( startNo + i ).ToString();
            }
        }


        private void initialViewSetting(int idx)
        {
            if( loadExistingOutsourceData() )
            {
                if( idx < 0 ) idx = 0;
                if( idx > osd.Length ) idx = 0;
                loadOutsourceContData( osd[idx].OutsourceID, dataGridView1 );
                viewOutsourceData( idx );
                //buttonFromPlan.Enabled = false; // 予算からの取込み禁止
                edit_lblPageNo();
                buttonNew.Enabled = false;
                buttonGrpEnabled();
            }
            else
            {
                ///buttonOverWrite.Enabled = false;
                buttonGrpDisabled();
            }
        }
        

        private void initialSetbuttonFromPlan()
        {

            EstPlanOp epo = new EstPlanOp();
            DataTable dt;
            if( ( dt = epo.Planning_Select_Latest( ted.TaskEntryID ) ) == null )
            {
                buttonFromPlan.Enabled = false;
            }
            else
            {
                buttonFromPlan.Enabled = true;
            }
        }

        // 既存データの読取表示
        // TaskEntryIDと発行部署でSELECT
        private bool loadExistingOutsourceData()
        {
            OutsourceData outsd = new OutsourceData();
            DataTable dt = outsd.SelectOutsource( ted.TaskEntryID, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxWork.SelectedValue ) );
            if( dt == null ) return false;
            osd = new OutsourceData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) osd[i] = new OutsourceData( dt.Rows[i] );
            return true;
        }


        private bool loadOutsourceContData( int outsourceID, DataGridView dgv )
        {
            if( outsourceID == 0 ) return false;
            OutsourceData outsd = new OutsourceData();
            DataTable dt = outsd.SelectOutsourceCont( outsourceID );
            viewOutsourceContToDgv( dt, dgv );

            return true;
        }


        private void loadPlanningData( DataGridView dgv )
        {
            EstPlanOp epo = new EstPlanOp();
            DataTable dt;
            if( ( dt = epo.Planning_Select_Latest( ted.TaskEntryID ) ) == null ) return;
            plnd = new PlanningData( dt.Rows[dt.Rows.Count - 1] );

            string condition = "Cost1 > 0";
            if( ( dt = epo.PlanningCont_Select( plnd.PlanningID, condition ) ) == null ) return;

            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );

            if( dt.Rows.Count > iniRCnt ) dgv.Rows.Add( dt.Rows.Count - iniRCnt );
            if( !viewOutsourceContToDgv( dt, dgv ) ) return;

            reCalculateAll( dgv );                                                 // 再計算

            osd = new OutsourceData[1];
            osd[0] = new OutsourceData();
            osd[0].TaskEntryID = plnd.TaskEntryID;
            osd[0].PlanningID = plnd.PlanningID;
            osd[0].OfficeCode = plnd.OfficeCode;
            osd[0].Department = plnd.Department;
            osd[0].Publisher = plnd.OfficeCode + plnd.Department;

            plnd = new PlanningData();          // 使用済み初期化    
            return;
        }


        private bool viewOutsourceContToDgv( DataTable dt, DataGridView dgv )
        {
            if( dt == null ) return false;
            DataRow dr;
            decimal qty = 0;
            decimal cost = 0;

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                dgv.Rows[i].Cells["ItemCode"].Value = Convert.ToString( dr["ItemCode"] );
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString( dr["Item"] );
                dgv.Rows[i].Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );

                if( plnd == null || plnd.PlanningID == 0 )
                {
                    if( Convert.ToDecimal( dr["Cost"] ) == 0 ) qty = 0;
                    cost = Convert.ToDecimal( dr["Cost"] );
                }
                else
                {
                    if( Convert.ToDecimal( dr["Cost1"] ) == 0 ) qty = 0;
                    cost = Convert.ToDecimal( dr["Cost1"] );
                }
                if( cost != 0 ) qty = Convert.ToDecimal( dr["Quantity"] );

                if( Convert.ToString( dr["Unit"] ) != "" )
                {
                    dgv.Rows[i].Cells["Quantity"].Value = qty;
                    dgv.Rows[i].Cells["Unit"].Value = Convert.ToString( dr["Unit"] );
                    dgv.Rows[i].Cells["Cost"].Value = DHandling.DecimaltoStr( cost, "#,0" );
                    dgv.Rows[i].Cells["Amount"].Value = DHandling.DecimaltoStr( qty * cost, "#,0" );
                }
            }
            return true;
        }


        private void viewOutsourceDataToForm( int idx, DataGridView dgv )
        {
            EstPlanOp epo = new EstPlanOp();
            osd[idx].OrderNo = epo.NumberingOrder( osd[idx].OfficeCode, osd[idx].Department );
            labelOrderNo.Text = osd[idx].OrderNo;
            osd[idx].TaskEntryID = ted.TaskEntryID;
            if( ( osd[idx].OutsourceID = epo.InsertOutsource( osd[idx] ) ) > 0 )
                epo.InsertOutsourceCont( dataGridView1, osd[idx].OutsourceID );
            loadExistingOutsourceData();
        }


        /// <summary>
        /// 上書可能か否かのチェック。取引先コードが読み取った時点のとことなれば上書は不可とする
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>true 異なっている：上書不可、false 同じもの：上書可能</returns>
        private bool checkAndStoreLatestOutsourceData( int idx )
        {
            bool insProc = false;
            if( osd[idx].PartnerCode != Convert.ToString( comboBoxPartner.SelectedValue ) ) insProc = true;
            storeLatestOutsourceData( idx );
            return insProc;
        }


        private void storeLatestOutsourceData( int idx )
        {
            //osd[idx].OrderNo = labelOrderNo.Text;
            osd[idx].PartnerCode = Convert.ToString( comboBoxPartner.SelectedValue );
            osd[idx].PayRoule = Convert.ToInt32( comboBoxPayRoule.SelectedValue );
            osd[idx].Amount = DHandling.ToRegDecimal( labelSum.Text );
            osd[idx].OrderDate = dateTimePickerOrderDate.Value;
            osd[idx].StartDate = dateTimePickerOStart.Value;
            osd[idx].EndDate = dateTimePickerOEnd.Value;
            osd[idx].InspectDate = dateTimePickerInspectDate.Value;
            osd[idx].ReceiptDate = dateTimePickerReceiptDate.Value;
            osd[idx].OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            osd[idx].Department = Convert.ToString( comboBoxWork.SelectedValue );
            osd[idx].Publisher = osd[idx].OfficeCode + osd[idx].Department;
            osd[idx].Place = textBoxDeliveryPoint.Text;
            osd[idx].Note = textBoxNote.Text;
        }


        private void viewData( int idx )
        {
            viewOutsourceData( idx );
            viewOutsourceContData( idx, dataGridView1 );
        }


        private void viewOutsourceData( int idx )
        {
            comboBoxOffice.SelectedValue = osd[idx].OfficeCode;
            comboBoxWork.SelectedValue = osd[idx].Department;
            labelOrderNo.Text = osd[idx].OrderNo;
            comboBoxPartner.SelectedValue = osd[idx].PartnerCode;
            comboBoxPayRoule.SelectedValue = osd[idx].PayRoule;
            labelSum.Text = DHandling.DecimaltoStr( osd[idx].Amount, "#,0" );
            if( DHandling.CheckDate( osd[idx].OrderDate ) ) dateTimePickerOrderDate.Value = osd[idx].OrderDate;
            if( DHandling.CheckDate( osd[idx].StartDate ) ) dateTimePickerOStart.Value = osd[idx].StartDate;
            if( DHandling.CheckDate( osd[idx].EndDate ) ) dateTimePickerOEnd.Value = osd[idx].EndDate;
            if( DHandling.CheckDate( osd[idx].InspectDate ) ) dateTimePickerInspectDate.Value = osd[idx].InspectDate;
            if( DHandling.CheckDate( osd[idx].ReceiptDate ) ) dateTimePickerReceiptDate.Value = osd[idx].ReceiptDate;
            textBoxDeliveryPoint.Text = osd[idx].Place;
            textBoxNote.Text = osd[idx].Note;
        }


        private void viewOutsourceContData( int idx, DataGridView dgv )
        {
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );
            loadOutsourceContData( osd[idx].OutsourceID, dgv );
            buttonNumbering( dgv );
            reCalculateAll( dgv );
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private void reCalculateAll( DataGridView dgv )
        {
            horizontalCalc( dgv );
            verticalCalc( dgv );
        }


        private void verticalCalc( DataGridView dgv )
        {
            Calculation calc = new Calculation( ted );
            calc.VCalcOutsource( dgv );
            labelSum.Text = DHandling.DecimaltoStr( calc.Sum, "#,0" );
        }


        private void horizontalCalc( DataGridView dgv )
        {
            Calculation calc = new Calculation( ted );
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                if( dgv.Rows[i].Cells["Item"].Value != null || dgv.Rows[i].Cells["ItemDetail"].Value != null )
                {
                    calc.HCalcOutsourceRow( dgv.Rows[i] );
                }
            }
        }


        private void clearOutsourceContAmount( DataGridView dgv )
        {
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                dgv.Rows[i].Cells["Cost"].Value = "";
                dgv.Rows[i].Cells["Amount"].Value = "";
            }
            dateTimePickerOrderDate.Value = DateTime.Now;
            dateTimePickerOStart.Value = DateTime.Now;
            dateTimePickerOEnd.Value = DateTime.Now;
            dateTimePickerInspectDate.Value = DateTime.Now;
            dateTimePickerReceiptDate.Value = DateTime.Now;
        }


        private void createExcelFile( string templateFileName, string sheetName )
        {

            Publish publ = new Publish( Folder.DefaultExcelTemplate( templateFileName ) );
            PublishData pd = new PublishData();

            pd.TaskCode = ted.TaskCode;
            pd.TaskName = ted.TaskName;
            pd.CostType = ted.CostType;
            pd.TaskPlace = ted.TaskPlace;
            pd.StartDate = ted.StartDate;
            pd.EndDate = ted.EndDate;
            pd.TaxRate = ted.TaxRate;

            pd.Publisher = Convert.ToString( comboBoxOffice.SelectedValue ) + Convert.ToString( comboBoxWork.SelectedValue );
            pd.OrderDate = dateTimePickerOrderDate.Value;
            pd.OrderStartDate = dateTimePickerOStart.Value;
            pd.OrderEndDate = dateTimePickerOEnd.Value;
            pd.InspectDate = dateTimePickerInspectDate.Value;
            pd.ReceiptDate = dateTimePickerReceiptDate.Value;
            pd.OrderPartner = comboBoxPartner.Text;
            pd.Place = textBoxDeliveryPoint.Text;
            pd.Note = textBoxNote.Text;
            pd.PayRoule = Convert.ToInt32( comboBoxPayRoule.SelectedValue );
            pd.OrderNo = labelOrderNo.Text;
            pd.Amount = DHandling.ToRegDecimal( labelSum.Text );
            pd.Tax = pd.Amount * pd.TaxRate;

            pd.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            pd.OfficeName = comboBoxOffice.Text;
            pd.PublishOffice = ( checkBoxPublish.Checked ) ? 1 : 0;


            publ.ExcelFile( sheetName, pd, dataGridView1 );
            return;
        }


    }
}

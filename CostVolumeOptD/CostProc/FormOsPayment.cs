using ClassLibrary;
using ListForm;
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

namespace CostProc
{
    public partial class FormOsPayment :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;
        PartnersScData[] psd;
        TaskCodeNameData[] tcd;
        //MembersScData[] msd;
        CostData[] cmd;
        OsPaymentData opd = new OsPaymentData();

        private bool iniPro = true;
        private int iniRCnt = 29;

        const string costIns = "原価実績データを作成しました。";
        const string costUpd = "原価実績データを修正しました。";
        const string costSave = "原価実績データを保存しました。";
        const string dataSave = "外注出来高調書データを保存しました。";

        const string HQOffice = "本社";

        //private string prePartnerCode = null;
        private DateTime preReportDate;

        private DateTime[] clsArray = new DateTime[4];
        private CheckBox[] ckArray;
        private Label[] lbArray;
        private DateTime[] dtArray = new DateTime[3];

        private bool readyPro = true;
        private bool updateStat = false;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormOsPayment()
        {
            InitializeComponent();
        }

        public FormOsPayment( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }

        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormOsPayment_Load( object sender, EventArgs e )
        {
            create_cbOffice();
            comboBoxOffice.Text = hp.OfficeCode;        // 初期値
            create_cbDepart();
            comboBoxDepart.Text = hp.Department;        // 初期値
            this.clsArray = new DateTime[] { hp.CloseHDate, hp.CloseKDate, hp.CloseSDate, hp.CloseTDate };  // 現在の締日リスト

            dataGridView1.Rows.Add( iniRCnt );
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvReadyNoRHeader();

            readyDateTimePicker();
            dateTimePickerEx1.Value = clsArray[Conv.oList.IndexOf( hp.OfficeCode )].AddMonths( 1 );     // 初期表示開始月（締月の翌月）
            preReportDate = dateTimePickerEx1.Value.EndOfMonth();

            labelACheckDate.Text = "";
            labelDCheckDate.Text = "";
            labelPCheckDate.Text = "";
            labelMsg.Text = "";

            readyCheckBox();
            buttonEnabled();
            buttonCost.Enabled = false;

            // 取引先マスタより外注先一覧作成
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            ListFormDataOp lo = new ListFormDataOp();
            psd = lo.SelectPartnersScData();
            tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode );
            cmd = lo.SelectCostDataInitialF( Conv.OfficeCode );

            selectPaymentData( dataGridView1, dateTimePickerEx1.Value.EndOfMonth(), Conv.OfficeCode, Conv.DepartCode );

        }


        private void FormOsPayment_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            dataGridView1.CurrentCell = null;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonSave":
                    if( !savePaymentData( dataGridView1 ) ) return;
                    if( opd.SlipNo > 0 )
                    {
                        CostReportData crp = new CostReportData();
                        if( !crp.UpdateCostReport( opd ) ) return;      // 変更内容を原価実績データにも反映
                    }
                    labelMsg.Text = dataSave;
                    updateStat = false;
                    break;

                case "buttonDelete":
                    if( !deletePaymentData( dataGridView1 ) ) return;
                    break;

                case "buttonCost":
                    if( !saveCostReportData( dataGridView1 ) ) return;
                    if( !savePaymentData( dataGridView1 ) ) return;
                    labelMsg.Text = costSave;
                    break;

                case "buttonCancel":
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add( iniRCnt );
                    seqNoReNumbering( dataGridView1 );
                    break;

                case "buttonEnd":
                    if( !unsavedCheck( dataGridView1 ) ) return;
                    this.Close();
                    break;

                case "buttonPrint":
                    PublishOsCost poc = new PublishOsCost( Folder.DefaultExcelTemplate( "外注出来高調書.xlsx" ), collectPublishData( dataGridView1 ) );
                    poc.ExcelFile( "OsPayment" );
                    break;

                default:
                    break;
            }
            if( btn.Name == "buttonEnd" || btn.Name == "buttonPrint" ) return;
            selectPaymentData( dataGridView1, dateTimePickerEx1.Value.EndOfMonth(), Conv.OfficeCode, Conv.DepartCode );

        }



        // [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        // 前提：コントロールがDataGridViewにある時
        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;
            if( !this.dataGridView1.Focused ) return;

            DataGridView dgv = ( DataGridView )sender;
            if( dgv.CurrentCellAddress.Y < 0 ) return;

            if( ckArray[0].Checked ) return;        // 確認済なら入力不可とする

            switch( e.KeyCode )
            {
                case Keys.Right:
                case Keys.Tab:
                    break;

                case Keys.Left:
                    break;

                default:
                    break;
            }


            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    if( dgv.CurrentCellAddress.X == 2 || dgv.CurrentCellAddress.X == 3 )
                    {
                        chooseCostData( dgv.Rows[dgv.CurrentCellAddress.Y] );
                        dgv.CurrentCell = dgv[5, dgv.CurrentCellAddress.Y];
                        return;
                    }
                    if( dgv.CurrentCellAddress.X == 5 || dgv.CurrentCellAddress.X == 6 )
                    {
                        chooseTaskCodeNameData( dgv.Rows[dgv.CurrentCellAddress.Y] );

                        dgv.Rows[dgv.CurrentCellAddress.Y].Cells["OAmount"].Value =
                            opd.SelectOAmountPayment( Convert.ToString( dgv.Rows[dgv.CurrentCellAddress.Y].Cells["ItemCode"].Value ),
                                                        Convert.ToString( dgv.Rows[dgv.CurrentCellAddress.Y].Cells["TaskCode"].Value ),
                                                        Conv.OfficeCode, Conv.DepartCode, dateTimePickerEx1.Value );

                        dgv.Rows[dgv.CurrentCellAddress.Y].Cells["SAmount"].Value =
                            opd.SelectSumAmountPayment( Convert.ToString( dgv.Rows[dgv.CurrentCellAddress.Y].Cells["ItemCode"].Value ),
                                                        Convert.ToString( dgv.Rows[dgv.CurrentCellAddress.Y].Cells["TaskCode"].Value ),
                                                        Conv.OfficeCode, Conv.DepartCode, dateTimePickerEx1.Value );

                        dgv.CurrentCell = dgv[9, dgv.CurrentCellAddress.Y];
                        return;
                    }
                    break;

                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;

                case Keys.I:
                case Keys.D:
                    seqNoReNumbering( dgv );
                    break;

                case Keys.R:
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

            string WorkString = "";

            switch( e.ColumnIndex )
            {
                case 0:     // 「削除」チェックボックス列
                    break;
                case 1:     // 「No.」列
                    break;
                case 2:     // 「原価コード」列
                    break;
                case 3:     // 「注文番号」列
                    break;
                case 4:     // 「原価内容」列
                    break;
                case 5:     // 「業務番号」列
                    break;
                case 6:     // 「業務名」列
                    break;
                case 7:     // 「発注金額」列
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells["OAmount"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["OAmount"].Value );
                    if( !( DHandling.IsDecimal( WorkString ) ) ) WorkString = "0";
                    dgv.Rows[e.RowIndex].Cells["OAmount"].Value = decFormat( Convert.ToDecimal( WorkString ) );
                    calculateInvolvedItems( dgv.Rows[e.RowIndex] );
                    break;

                case 8:     // 「前月累計出来高」列
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells["SAmount"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["SAmount"].Value );
                    if( !( DHandling.IsDecimal( WorkString ) ) ) WorkString = "0";
                    dgv.Rows[e.RowIndex].Cells["SAmount"].Value = decFormat( Convert.ToDecimal( WorkString ) );
                    break;

                case 9:     // 「今月出来高」列
                    if( Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value );
                    if( !( DHandling.IsDecimal( WorkString ) ) ) WorkString = "0";
                    dgv.Rows[e.RowIndex].Cells["Amount"].Value = decFormat( Convert.ToDecimal( WorkString ) );
                    calculateInvolvedItems( dgv.Rows[e.RowIndex] );
                    updateStat = true;
                    break;

                case 10:     // 「発注残額」列
                    break;
                case 11:     // 「伝票番号」列
                    if( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value == null || Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value );
                    if( !( DHandling.IsNumeric( WorkString ) ) ) WorkString = "";
                    dgv.Rows[e.RowIndex].Cells["SlipNo"].Value = WorkString;
                    break;

                default:
                    break;
            }
            //updateStat = true;
        }



        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            ComboBox cbx = ( ComboBox )sender;
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            switch( cbx.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    ListFormDataOp lo = new ListFormDataOp();
                    cmd = lo.SelectCostDataInitialF( Conv.OfficeCode );
                    tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode );
                    break;

                case "comboBoxDepart":
                    break;

                default:
                    break;
            }
            selectPaymentData( dataGridView1, dateTimePickerEx1.Value.EndOfMonth(), Conv.OfficeCode, Conv.DepartCode );
        }


        /// <summary>
        /// チェックボックス確認
        /// ckArray = new CheckBox[] { checkBoxAdmin, checkBoxDirector, checkBoxPresident };
        /// lbArray = new Label[] { labelACheckDate, labelDCheckDate, labelPCheckDate };
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAdmin_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.AccessLevel != 3 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[0].Checked )
            {
                if( dtArray[0] == DateTime.MinValue ) dtArray[0] = DateTime.Today.StripTime();
                lbArray[0].Text = dtArray[0].ToLongDateString();
                if( checkEffectiveData( dataGridView1 ) )
                {
                    // 変更不可となるメッセージを表示し
                    // 保存済か確認
                    //ステータス更新
                    dataGridView1.ReadOnly = true;
                    updateCheckStatus( dataGridView1, 0, 1, dtArray[0] );
                }
                else
                {
                    ckArray[0].Checked = false;
                    lbArray[0].Text = "";
                    return;
                }
            }
            else
            {
                if( ckArray[1].Checked )
                {
                    chb.Checked = true;
                    return;
                }
                //ステータス更新
                updateCheckStatus( dataGridView1, 0, 0, DateTime.Today.StripTime() );
                lbArray[0].Text = "";
            }
        }


        private void checkBoxDirector_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.MemberType != 7 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[1].Checked )
            {
                if( dtArray[1] == DateTime.MinValue ) dtArray[1] = DateTime.Today.StripTime();
                lbArray[1].Text = dtArray[1].ToLongDateString();

                if( ckArray[0].Checked )
                {
                    buttonDisEnabled();
                    buttonCost.Enabled = true;
                }
                else
                {
                    ckArray[1].Checked = false;
                    buttonEnabled();
                    buttonCost.Enabled = false;
                    lbArray[1].Text = "";
                    return;
                }
                updateCheckStatus( dataGridView1, 1, 1, dtArray[1] );
                //ステータス更新
            }
            else
            {
                if( ckArray[2].Checked )
                {
                    ckArray[1].Checked = true;
                    buttonDisEnabled();
                    //buttonCost.Enabled = false; 
                    return;
                }
                else
                {
                    buttonEnabled();
                    buttonCost.Enabled = false;
                }
                lbArray[1].Text = "";
                updateCheckStatus( dataGridView1, 1, 0, dtArray[1] );
            }
        }


        private void checkBoxPresident_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.MemberType != 7 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[2].Checked )
            {
                if( dtArray[2] == DateTime.MinValue ) dtArray[2] = DateTime.Today.StripTime();
                lbArray[2].Text = dtArray[2].ToLongDateString();

                if( ckArray[1].Checked )
                {
                    buttonDisEnabled();
                }
                else
                {
                    ckArray[2].Checked = false;
                    lbArray[2].Text = "";
                    return;
                }
                updateCheckStatus( dataGridView1, 2, 1, dtArray[2] );
            }
            else
            {
                ckArray[2].Checked = false;
                lbArray[2].Text = "";
                if( ckArray[1].Checked )
                {
                    buttonDisEnabled();
                    buttonCost.Enabled = true;
                }
                else
                {
                    buttonEnabled();
                    buttonCost.Enabled = false;
                }
                updateCheckStatus( dataGridView1, 2, 0, dtArray[2] );
            }

        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            buttonEnabled();

            if( DHandling.CheckPastTheDeadline( dtp.Value, clsArray, Conv.OfficeCode ) ) buttonDisEnabled();

            if( preReportDate != dtp.Value.StripTime() )
            {
                Func<DialogResult> dialogOverLoad = DMessage.DialogOverLoad;
                if( dialogOverLoad() == DialogResult.No )
                {
                    dtp.Value = preReportDate;
                    return;
                }
            }
            selectPaymentData( dataGridView1, dateTimePickerEx1.Value.EndOfMonth(), Conv.OfficeCode, Conv.DepartCode );
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        private void buttonDisEnabled()
        {
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = false;
            buttonCost.Enabled = false;
        }

        private void buttonEnabled()
        {
            buttonSave.Enabled = true;
            buttonDelete.Enabled = true;
            buttonCancel.Enabled = true;
            buttonCost.Enabled = true;
        }


        private void readyCheckBox()
        {
            ckArray = new CheckBox[] { checkBoxAdmin, checkBoxDirector, checkBoxPresident };
            lbArray = new Label[] { labelACheckDate, labelDCheckDate, labelPCheckDate };
            for( int i = 0; i < dtArray.Length; i++ ) dtArray[i] = DateTime.MinValue;
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName", hp.AccessLevel );
            comboBoxOffice.SelectedValue = hp.OfficeCode;        // 初期値
        }


        // 部門
        private void create_cbDepart()
        {
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            if( comboBoxOffice.Text == Sign.HQOffice )
            {
                if( hp.Department == "0" ) comboBoxDepart.SelectedValue = "2";
            }
            else
            {
                comboBoxDepart.SelectedValue = "8";
            }
        }


        private void readyDateTimePicker()
        {
            dateTimePickerEx1.CustomFormat = "yyyy年MM月";
            dateTimePickerEx1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            //dateTimePickerEx1.Value = clsArray[Conv.oList.IndexOf(Convert.ToString(comboBoxOffice.SelectedValue))];
        }


        // DataGridViewButtonの番号を再採番
        private void seqNoReNumbering( DataGridView dgv )
        {
            for( int startNo = 1, i = 0; i < dgv.Rows.Count; i++ )
                dgv.Rows[i].Cells["SeqNo"].Value = ( startNo + i ).ToString();
        }


        // 取引先マスタ外注データをFormSubComList画面から得る
        private void choosePartnersScData( DataGridViewRow dgvRow )
        {
            PartnersScData psds = FormSubComList.ReceiveItems( psd );
            if( psds == null ) return;
            dgvRow.Cells["CoCompany"].Value = psds.PartnerName;
            ListFormDataOp lo = new ListFormDataOp();

            string editPartnerName = psds.PartnerName;
            cmd = lo.SelectCostData( Conv.OfficeCode, "CostCode", "F", editPartnerName.RemoveCorpForm() );
        }


        // 業務番号と業務名をFormTaskCodeNameList画面から得る
        private void chooseTaskCodeNameData( DataGridViewRow dgvRow )
        {
            if( tcd == null ) return;

            TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
            if( tcds == null ) return;
            dgvRow.Cells["TaskCode"].Value = tcds.TaskCode;
            dgvRow.Cells["TaskName"].Value = tcds.TaskName;
            TaskIndData tid = new TaskIndData();
            tid = tid.SelectTaskIndData( tcds.TaskCode );
            if( tid == null ) return;
            dgvRow.Cells["LeaderMCode"].Value = "0" + tid.LeaderMCode;
        }


        private void chooseCostData( DataGridViewRow dgvRow )
        {
            if( cmd == null ) return;
            CostData cmds = FormCostList.ReceiveItems( cmd );
            if( cmds == null ) return;

            // cmdsからtcdを作り直す場合は次の2Lineのコメントを外す。
            //ListFormDataOp lo = new ListFormDataOp();
            //tcd = lo.SelectTaskCodeNameFromOsWkReport( cmds.CostCode, dateTimePickerEx1.Value );

            dispCostData( cmds, dgvRow );
        }


        //private bool selectCostMaster( string costCode )
        //{
        //    CostData cdp = new CostData();
        //    cdp = cdp.SelectCostMaster( costCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
        //    if( cdp == null ) return false;
        //    return true;
        //}


        private void dispCostData( CostData cmds, DataGridViewRow dgvRow )
        {
            dgvRow.Cells["ItemCode"].Value = cmds.CostCode;
            dgvRow.Cells["Item"].Value = cmds.Item.Replace( "（支払い）", "" ); ;
        }


        /// <summary>
        /// 現在保存している外注出来高データをDBから読取り表示する
        /// </summary>
        /// <param name="reportDate"></param>
        /// <param name="costCode"></param>
        /// <param name="officeCode"></param>
        /// <param name="department"></param>
        private void selectPaymentData( DataGridView dgv, DateTime reportDate, string officeCode, string department )
        {
            readyPro = true;
            dgv.Enabled = true;
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );

            opd = new OsPaymentData();

            DataTable dt = opd.SelectPayment( reportDate.EndOfMonth(), officeCode, department );
            if( dt == null || dt.Rows.Count < 1 )
            {
                labelMsg.Text = "表示できるデータはありません。";
                seqNoReNumbering( dgv );
                this.dataGridView1.CurrentCell = null;
                readyPro = false;
                return;
            }
            labelMsg.Text = "";
            if( dt.Rows.Count > iniRCnt ) dgv.Rows.Add( dt.Rows.Count - iniRCnt + 5 );

            TaskData tdp = new TaskData();
            ListFormDataOp lo = new ListFormDataOp();
            DataRow dr;
            string taskCode;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                if( i == 0 )
                {
                    string[] ckItems = new string[] { "AdminCheck", "DirectorCheck", "PresidentCheck" };
                    string[] dtItems = new string[] { "ACheckDate", "DCheckDate", "PCheckDate" };
                    for( int j = 0; j < ckItems.Length; j++ )
                    {
                        int checkStat = Convert.ToInt32( dr[ckItems[j]] );
                        ckArray[j].Checked = ( checkStat == 1 ) ? true : false;
                        dtArray[j] = ( checkStat == 1 ) ? Convert.ToDateTime( dr[dtItems[j]] ) : DateTime.MinValue;
                        lbArray[j].Text = ( checkStat == 1 ) ? dtArray[j].ToLongDateString() : "";
                    }

                    if( ckArray[2].Checked )
                    {
                        buttonDisEnabled();
                    }
                    else if( ckArray[1].Checked )
                    {
                        buttonDisEnabled();
                        buttonCost.Enabled = true;
                    }
                    else if( ckArray[0].Checked )
                    {
                        buttonEnabled();
                        buttonCost.Enabled = false;
                    }
                }

                dgv.Rows[i].Cells["ItemCode"].Value = Convert.ToString( dr["ItemCode"] );
                dgv.Rows[i].Cells["Item"].Value = Convert.ToString( dr["Item"] );
                dgv.Rows[i].Cells["OrderNo"].Value = Convert.ToString( dr["OrderNo"] );
                taskCode = Convert.ToString( dr["TaskCode"] );
                dgv.Rows[i].Cells["TaskCode"].Value = taskCode;
                if( Convert.ToString( dr["OrderAmount"] ) == "" || Convert.ToString( dr["OrderAmount"] ) == "0.00" )
                {
                    dgv.Rows[i].Cells["OAmount"].Value =
                        opd.SelectOAmountPayment( Convert.ToString( dr["ItemCode"] ), taskCode, Conv.OfficeCode , Conv.DepartCode, dateTimePickerEx1.Value );
                }
                else
                {
                    dgv.Rows[i].Cells["OAmount"].Value = Convert.ToDecimal( dr["OrderAmount"] );
                }
                dgv.Rows[i].Cells["SAmount"].Value =
                    opd.SelectSumAmountPayment( Convert.ToString( dr["ItemCode"] ), taskCode, Conv.OfficeCode, Conv.DepartCode, dateTimePickerEx1.Value );
                dgv.Rows[i].Cells["Amount"].Value = Convert.ToDecimal( dr["Amount"] );
                calculateInvolvedItems( dgv.Rows[i] );

                dgv.Rows[i].Cells["LeaderMCode"].Value = Convert.ToString( dr["LeaderMCode"] );
                dgv.Rows[i].Cells["SlipNo"].Value = Convert.ToInt32( dr["SlipNo"] ) == 0 ? "" : Convert.ToString( dr["SlipNo"] );
                dgv.Rows[i].Cells["CostReportID"].Value = Convert.ToInt32( dr["CostReportID"] ) == 0 ? "" : Convert.ToString( dr["CostReportID"] );

                dgv.Rows[i].Cells["PaymentID"].Value = Convert.ToString( dr["OsPaymentID"] );
                dgv.Rows[i].Cells["Check"].Value = false;


                TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( taskCode, Conv.OfficeCode );
                if( tcnd != null )
                {
                    dgv.Rows[i].Cells["TaskName"].Value = tcnd.TaskName;

                    tdp = tdp.SelectTaskData( taskCode );
                    dgv.Rows[i].Cells["SalesMCode"].Value = "0" + tdp.SalesMCode;
                    dgv.Rows[i].Cells["PartnerCode"].Value = tdp.PartnerCode;
                }
            }
            seqNoReNumbering( dgv );
            this.dataGridView1.CurrentCell = null;

            readyPro = false;
        }


        private bool savePaymentData( DataGridView dgv )
        {
            int procCnt = 0;
            opd = new OsPaymentData();

            opd = moveCommonData();
            CostReportData crp = new CostReportData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value ) == "" ) continue;

                opd = moveIndividualData( dataGridView1.Rows[i] );
                if( Convert.ToString( dgv.Rows[i].Cells["PaymentID"].Value ) == "" )
                {
                    if( !opd.InsertPayment() ) return false;
                    dgv.Rows[i].Cells["PaymentID"].Value = Convert.ToString( opd.OsPaymentID );
                }
                else
                {
                    opd.OsPaymentID = Convert.ToInt32( dgv.Rows[i].Cells["PaymentID"].Value );
                    if( !opd.UpdatePayment() ) return false;
                }
                procCnt++;
            }

            if( procCnt == 0 )
            {
                MessageBox.Show( "処理対象のデータはありませんでした！" );
                return false;
            }
            return true;
        }


        private OsPaymentData moveCommonData()
        {
            opd.OfficeCode = Conv.OfficeCode;
            opd.Department = Conv.DepartCode;
            opd.ReportDate = dateTimePickerEx1.Value.EndOfMonth();

            opd.AdminCode = hp.MemberCode;
            opd.AdminCheck = ( ckArray[0].Checked ) ? 1 : 0;
            opd.DirectorCheck = ( ckArray[1].Checked ) ? 1 : 0;
            opd.PresidentCheck = ( ckArray[2].Checked ) ? 1 : 0;

            opd.ACheckDate = dtArray[0].StripTime();
            opd.DCheckDate = dtArray[1].StripTime();
            opd.PCheckDate = dtArray[2].StripTime();

            return opd;
        }


        private OsPaymentData moveIndividualData( DataGridViewRow dgvRow )
        {
            opd.ItemCode = Convert.ToString( dgvRow.Cells["ItemCode"].Value );
            opd.Item = Convert.ToString( dgvRow.Cells["Item"].Value );
            opd.OrderNo = Convert.ToString( dgvRow.Cells["OrderNo"].Value );
            opd.TaskCode = Convert.ToString( dgvRow.Cells["TaskCode"].Value );
            opd.OrderAmount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["OAmount"].Value ) );
            opd.SAmount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["SAmount"].Value ) );
            opd.Amount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["Amount"].Value ) );
            opd.LeaderMCode = Convert.ToString( dgvRow.Cells["LeaderMCode"].Value );
            opd.SlipNo = ( Convert.ToString( dgvRow.Cells["SlipNo"].Value ) == "" ) ? 0 : Convert.ToInt32( dgvRow.Cells["SlipNo"].Value );
            opd.CostReportID = ( Convert.ToString( dgvRow.Cells["CostReportID"].Value ) == "" ) ? 0 : Convert.ToInt32( dgvRow.Cells["CostReportID"].Value );
            opd.TaskName = Convert.ToString( dgvRow.Cells["TaskName"].Value );
            opd.Unit = "式";
            return opd;
        }


        /// <summary>
        /// 原価実績データを登録する（「原価データ作成」ボタン押下時）
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private bool saveCostReportData( DataGridView dgv )
        {
            int procCnt = 0;
            CostReportData crd = new CostReportData();
            crd.OfficeCode = Conv.OfficeCode;
            crd.Department = Conv.DepartCode;
            crd.ReportDate = dateTimePickerEx1.Value.EndOfMonth();
            crd.UnitPrice = 0;
            crd.Quantity = 1;
            crd.Unit = "式";
            //crd.SubCoCode = "";
            crd.MemberCode = hp.MemberCode;
            //crd.AccountCode = "";
            crd.Note = "";

            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value ) == "" ) continue;
                crd.ItemCode = Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value );
                crd.Item = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                crd.TaskCode = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                crd.Cost = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) );
                crd.LeaderMCode = Convert.ToString( dgv.Rows[i].Cells["LeaderMCode"].Value );
                crd.SalesMCode = Convert.ToString( dgv.Rows[i].Cells["SalesMCode"].Value );
                crd.CustoCode = Convert.ToString( dgv.Rows[i].Cells["PartnerCode"].Value );
                crd.Subject = Convert.ToString( crd.ItemCode[0] );
                //crd.Unit = "式";
                crd.SubCoCode = crd.ItemCode;

                crd.AccountCode = "OSPM";
                crd.CoTaskCode = "";

                if( Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value ) == "" )
                {
                    if( !crd.InsertCostReportAndGetID() ) return false;
                    dgv.Rows[i].Cells["SlipNo"].Value = Convert.ToString( crd.SlipNo );
                    dgv.Rows[i].Cells["CostReportID"].Value = Convert.ToString( crd.CostReportID );
                    labelMsg.Text = costIns;
                }
                else
                {
                    crd.SlipNo = Convert.ToInt32( dgv.Rows[i].Cells["SlipNo"].Value );
                    if( !crd.UpdateCostReport() ) return false;
                    labelMsg.Text = costUpd;
                }
                procCnt++;
            }

            if( procCnt == 0 )
            {
                MessageBox.Show( "処理対象のデータはありませんでした！" );
                return false;
            }
            return true;
        }


        /// <summary>
        /// 削除マークがついたもの、一度保存されたものを対象に外注精算データを削除する
        /// 対応する原画実績データがある場合はそれも削除する。
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private bool deletePaymentData( DataGridView dgv )
        {
            opd = new OsPaymentData();
            CostReportData crd = new CostReportData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["PaymentID"].Value ) == "" ) continue;
                if( !Convert.ToBoolean( dgv.Rows[i].Cells["Check"].Value ) ) continue;

                if( !opd.DeletePayment( "@pMID", Convert.ToInt32( dgv.Rows[i].Cells["PaymentID"].Value ) ) ) return false;     // 出来高データの削除
                if( Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value ) != "" )                                              // 原価データの削除
                {
                    if( !crd.DeleteCostReport( "@slip", Convert.ToInt32( dgv.Rows[i].Cells["SlipNo"].Value ) ) ) return false;
                }
            }
            return true;
        }


        private bool unsavedCheck( DataGridView dgv )
        {
            int usCount = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
                if( Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value ) != "" && Convert.ToString( dgv.Rows[i].Cells["PaymentID"].Value ) == "" ) usCount++;

            if( usCount > 0 )
            {
                Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                if( dialogRemining() == DialogResult.No ) return false;
            }
            else
            {
                unsavedCheck();
            }
            return true;
        }


        private bool unsavedCheck()
        {
            if( updateStat )
            {
                Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                if( dialogRemining() == DialogResult.No ) return false;
            }
            updateStat = false;
            return true;
        }


        private OsPaymentData[] collectPublishData( DataGridView dgv )
        {
            int rcnt = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value ) != "" ) rcnt++;
            }
            if( rcnt == 0 ) return null;
            OsPaymentData[] opda = new OsPaymentData[rcnt];
            opd = new OsPaymentData();
            opd = moveCommonData();
            for( int i = 0, j = 0; i < dataGridView1.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["ItemCode"].Value ) == "" ) continue;
                if( j < rcnt )
                {
                    opda[j] = ( OsPaymentData )( moveIndividualData( dgv.Rows[i] ) ).Clone();
                    j++;
                }
            }

            return opda;
        }


        private bool checkEffectiveData( DataGridView dgv )
        {
            int effectiveCount = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value ) )
                    && !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["PaymentID"].Value ) ) ) effectiveCount++;
            }
            if( effectiveCount == 0 ) return false;
            return true;
        }


        private void calculateInvolvedItems( DataGridViewRow dgvRow )
        {
            decimal oAmount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["OAmount"].Value ) );
            decimal sAmount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["SAmount"].Value ) );
            decimal amount = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["Amount"].Value ) );

            dgvRow.Cells["ROAmount"].Value = decFormat( oAmount - sAmount - amount );
            dgvRow.Cells["OAmount"].Value = decFormat( oAmount );
            dgvRow.Cells["SAmount"].Value = decFormat( sAmount );
            dgvRow.Cells["Amount"].Value = decFormat( amount );
        }


        private bool updateCheckStatus( DataGridView dgv, int ckNo, int stat, DateTime ckDt )
        {
            OsPaymentData pmdPro = new OsPaymentData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["PaymentID"].Value ) ) )
                {
                    if( !pmdPro.UpdatePaymentStatus( Convert.ToInt32( dgv.Rows[i].Cells["PaymentID"].Value ), ckNo, stat, ckDt ) ) return false;
                }
            }
            return true;
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }

    }
}

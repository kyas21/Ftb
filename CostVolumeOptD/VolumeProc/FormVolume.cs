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
using ListForm;
using System.IO;
using PrintOut;

namespace VolumeProc
{
    public partial class FormVolume :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;
        private DataGridViewCellStyle defaultCellStyle;
        CostReportData[] crdM07;
        CostReportData[] crdM08;
        CostReportData[] crdM09;
        CostReportData[] crdM10;
        CostReportData[] crdM11;
        CostReportData[] crdM12;
        CostReportData[] crdM01;
        CostReportData[] crdM02;
        CostReportData[] crdM03;
        CostReportData[] crdM04;
        CostReportData[] crdM05;
        CostReportData[] crdM06;

        private decimal[] cumulativeAry = new decimal[13];         //受注単月
        private decimal[] totalCumulativeAry = new decimal[13];    //受注累計
        private decimal[] volUncompAry = new decimal[13];          //出来高 単月 未成業務
        private decimal[] volClaimRemAry = new decimal[13];        //出来高 単月 未請求
        private decimal[] volClaimAry = new decimal[13];           //出来高 単月 請求
        private decimal[] monthlyTotalAry = new decimal[13];       //出来高 単月 月計
        private decimal[] totalTradingVolumeAry = new decimal[13]; //出来高 累計

        private decimal[] OverTime = new decimal[13];              //残業務高OverTime

        private decimal[] cumulativeMAry = new decimal[13];        //請求 単月
        private decimal[] totalCumulativeMAry = new decimal[13];   //請求 累計
        private string[] claimDateAry = new string[13];            //請求 請求日

        private decimal[] cumulativeVAry = new decimal[13];        //入金 単月
        private decimal[] totalCumulativeVAry = new decimal[13];   //入金 累計
        private string[] paidDateAry = new string[13];             //入金 入金日

        private decimal[] cumulativeMCAry = new decimal[13];       //原価 単月
        private decimal[] totalCumulativeMCAry = new decimal[13];  //原価 累計
        private decimal[] setCostRateAry = new decimal[13];        //原価率

        private bool iniPro = true;
        private bool grdSet = false;
        private int iniRCnt = 22;
        private int[] readOnlyRows = new int[] { 1, 5, 6, 7, 9, 12, 14, 15, 16, 17, 18, 19, 20 };
        private int[] readOnlyColumns = new int[] { 0 };
        private int[] readOnlyAllRows = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };
        private int[] readOnlyAllColumns = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private int[] rowsBackColorColumns = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


        VolumeData[] volumedata;

        private string[] OfficeArray;

        private int ClosingDate = 7;                    // 最終締め月格納用(初期値は年度初め7月とする)

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormVolume()
        {
            InitializeComponent();
        }

        public FormVolume( HumanProperty hp )
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
        private void FormVolume_Load( object sender, EventArgs e )
        {
            // 201901 Asakawa レイアウト修正

            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling.FormSizeSTD( this );

            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            //並び替えができないようにする
            uih.NoSortable();

            uih.DgvColumnsWidth( 80 );
            uih.DgvColumnsWidth( 0, 90 );

            dataGridView1.Rows.Add( iniRCnt );
            uih.DgvRowsHeight( 22 );
            uih.DgvRowsReadOnly( readOnlyRows );
            uih.DgvColumnsReadOnly( readOnlyColumns );

            uih.DgvRowsColor( readOnlyRows, Color.PaleGreen );
            uih.DgvColumnsColor( readOnlyColumns, Color.PaleGreen );

            // 201901 asakawa
            // 20190216 asakawa
            // uih.DgvRowsHeight( iniRCnt - 1, 43 );
            // uih.DgvRowsHeight(iniRCnt - 1, 86);
            uih.DgvRowsHeight(iniRCnt - 1, 83);
            uih.DgvColumnsWrapModeON( iniRCnt - 1, DataGridViewContentAlignment.TopLeft );

            dataGridView1.AllowUserToAddRows = false;

            //出来高テーブル関連（今年）
            CreateCbYear();

            //ドロップダウンリスト作成
            //期間
            CreateCbPeriod();

            //業務状態
            CreateCbTaskState();

            //部署
            CreateCbOffice();

            //部門
            CreateCbDepartment();

            /***** ComboBox 「業務コード」作成 *****/
            //全て
            createTaskCodeCB( comboBoxTaskCode );

            VolumeDataInit();

            // 最終締め月を取得する
            CommonData com = new CommonData();          // M_Commonアクセスクラス
            //ClosingDate = Convert.ToInt32( com.SelectCloseDate( Convert.ToString( this.comboBoxOfficeCode.SelectedValue ) ).ToString( "MM" ) );
            ClosingDate = closingMonth( Convert.ToString( comboBoxOfficeCode.SelectedValue ) );
            int[] monthArray = new int[] { 0, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );
            int[] ReadOnlySet = new int[DisplyLimit];
            for( int i = 0; i < DisplyLimit; i++ ) ReadOnlySet[i] = i;
            uih.DgvColumnsReadOnly( ReadOnlySet );
        }


        private void FormVolume_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            ScreenDisplay();
            dataGridView1.CurrentCell = null;
        }


        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        //-----------------------------//
        // User Control                //
        //-----------------------------//

        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            EstPlanOp epo = new EstPlanOp();

            switch( btn.Name )
            {
                case "buttonSave":
                    VolumeSave( dataGridView1 );
                    break;
                case "buttonCancel":
                    releaseExclusive();
                    this.Close();
                    break;
                case "buttonNextTask":
                    NextTaskCode();
                    break;
                case "buttonBeforeTask":
                    BeforeTaskCode();
                    break;
                case "buttonSaveNextTask":
                    if( VolumeSave( dataGridView1 ) == true )
                        NextTaskCode();
                    break;
                case "buttonSaveBeforeTask":
                    if( VolumeSave( dataGridView1 ) == true )
                        BeforeTaskCode();
                    break;
                case "buttonTaskSort":
                    TaskCodeSort();
                    break;
                case "buttonFormReview":
                    HumanProperty ihp = new HumanProperty();
                    ihp.OfficeCode = Convert.ToString( comboBoxOfficeCode.SelectedValue );
                    ihp.Department = Convert.ToString( comboBoxDepartment.SelectedValue );
                    ihp.CloseHDate = hp.CloseHDate;
                    ihp.CloseKDate = hp.CloseKDate;
                    ihp.CloseSDate = hp.CloseSDate;
                    ihp.CloseTDate = hp.CloseTDate;
                    FormReview formReview = null;
                    formReview = new FormReview( ihp, this.comboBoxYear.Text );
                    formReview.Show();
                    break;
                case "buttonGetCost":
                    SetOriginalCost();
                    break;
                case "buttonExcelPrint":
                    VolumeExcelPrint();
                    break;
                case "buttonPdfPrint":
                    FormPdfOut frmPdf = new FormPdfOut();
                    frmPdf.TaskCode = this.comboBoxTaskCode;
                    frmPdf.OfficeCode = this.comboBoxOfficeCode;
                    frmPdf.Department = this.comboBoxDepartment;
                    frmPdf.SetGridVew = this.dataGridView1;
                    frmPdf.TaskState = this.comboBoxTaskState;
                    frmPdf.SetYear = this.comboBoxYear.Text;
                    frmPdf.SetCarryOver = this.textBoxCarryOverPlanned.Text;
                    frmPdf.SetNote = this.textBoxNote.Text;
                    frmPdf.ClosingDate = ClosingDate;

                    frmPdf.Show();
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;
            switch( e.KeyCode )
            {
                case Keys.Delete:
                case Keys.Back:
                    if( dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].ReadOnly == false )
                        dgv.Rows[dgv.CurrentCellAddress.Y].Cells[dgv.CurrentCellAddress.X].Value = "";
                    break;
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
                    if( ( dgv.CurrentCellAddress.Y >= 16 ) && ( dgv.CurrentCellAddress.Y <= 18 ) )
                    {
                        if( dgv.CurrentCellAddress.X >= 1 )
                        {
                            chooseCostDetailData( dgv.CurrentCellAddress.X );
                            dgv.Rows[dgv.CurrentCellAddress.Y].Cells[1].Style = this.defaultCellStyle;
                        }
                    }
                    break;
                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;

            if( grdSet ) return;

            DataGridView dgv = ( DataGridView )sender;

            if( ( e.RowIndex == 10 ) || ( e.RowIndex == 13 ) ) //請求日または入金日
            {
                if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) ) )
                    && ( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value ) )
                {
                    string strDateTime = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    DateTime dt;
                    if( DateTime.TryParse( strDateTime, out dt ) )
                    {
                        //変換出来たら、dtにその値が入る
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dt.ToString( "yyyy/MM/dd" );
                    }
                    else
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        return;
                    }
                }
            }
            else if( ( e.RowIndex == 0 ) || //受注単月
               ( e.RowIndex == 2 ) || //出来高 未成業務
               ( e.RowIndex == 3 ) || //出来高 未請求
               ( e.RowIndex == 4 ) || //出来高 請求
               ( e.RowIndex == 8 ) || //請求 単月
               ( e.RowIndex == 11 ) ) //入金 単月
            {
                if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) ) )
                    && ( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value ) )
                {
                    if( !( DHandling.IsDecimal( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() ) ) ) )
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        return;
                    }
                }
            }

            if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) ) )
                && ( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != DBNull.Value ) )
            {
                if( e.RowIndex == 0 || e.RowIndex == 2 || e.RowIndex == 3 || e.RowIndex == 4 || e.RowIndex == 8 || e.RowIndex == 11 )
                {
                    decimal decSelectData = Convert.ToDecimal( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim() );
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DHandling.DecimaltoStr( decSelectData, "#,0" );
                }
            }

            switch( e.RowIndex )
            {
                case 0://受注単月
                case 2://出来高未成業務
                case 3://出来高未請求
                case 4://出来高請求
                case 8://請求単月
                case 11://入金単月
                case 16://原価単月
                    for( int i = 0; i <= 12; i++ )
                    {
                        SetCumulative( dgv, i );          //受注累計
                        SetMonthlyTotal( dgv, i );        //出来高月計
                        SetTotalTradingVolume( dgv, i );  //出来高累計
                        SetOverTime( dgv, i );            //残業務高
                        SetCumulativeM( dgv, i );         //請求累計
                        SetCumulativeV( dgv, i );         //入金累計
                        SetResidualClaimHigh( dgv, i );   //残請求高
                        SetUncompBusAccept( dgv, i );     //未成業務受入金
                        SetCumulativeMC( dgv, i );        //原価累計
                        SetCostRate( dgv, i );            //原価率
                        SetAccountsReceivable( dgv, i );  //未収入金
                        SetUncompBusAcceptM( dgv, i );    //未成業務受入金
                    }
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_CellBeginEdit( object sender, DataGridViewCellCancelEventArgs e )
        {
            if( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;
            if( e.RowIndex != 10 && e.RowIndex != 13 && e.RowIndex != 21 )
            {
                grdSet = true;
                if( Convert.ToString( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ) != "" )
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SignConvert( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value );
                grdSet = false;
            }
        }


        private void dataGridView1_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;

            if( e.RowIndex != 10 && e.RowIndex != 13 && e.RowIndex != 21 )
            {
                grdSet = true;
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = MinusConvert( dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value );
                grdSet = false;
            }
        }


        private void CarryOverPlanned_TextChanged( object sender, EventArgs e )
        {
            //年度内完工高
            //繰越予定額
            decimal carryOverPlanned = 0;
            decimal yearCompHigh = 0;

            // 20190227 asakawa
            // 年度内完工高の計算方法を変更 その１
            // if ( textBoxCarryOverPlanned.Text.Trim() == "" ) return;
            if (textBoxCarryOverPlanned.Text.Trim() == "")
            {
                labelYearCompletionHigh.Text = "";
                return;
            }
            // その１ end

            if ( textBoxCarryOverPlanned.Text.Trim() == "-" ) return;
            if( DHandling.IsDecimal( textBoxCarryOverPlanned.Text ) == true )
                carryOverPlanned = Convert.ToDecimal( textBoxCarryOverPlanned.Text );
            else
            {
                carryOverPlanned = SignConvert( textBoxCarryOverPlanned.Text );
                if( carryOverPlanned == 0 )
                {
                    textBoxCarryOverPlanned.Text = "";
                    return;
                }
            }

            if( textBoxYearCompHigh.Text.ToString().Trim() != "" )
                yearCompHigh = Convert.ToDecimal( textBoxYearCompHigh.Text );

            decimal decYearCompletionHigh = yearCompHigh - carryOverPlanned;

            labelYearCompletionHigh.Text = "";

            for( int i = 1; i < 13; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( this.dataGridView1.Rows[0].Cells[i].Value ) ) )
                {
                    if( decYearCompletionHigh != 0 )
                    {
                        if( decYearCompletionHigh > 0 )
                        {
                            labelYearCompletionHigh.Text = DHandling.DecimaltoStr( Convert.ToDecimal( decYearCompletionHigh ), "#,0" );
                        }
                        else
                        {
                            string strYearCompletionHigh = DHandling.DecimaltoStr( Convert.ToDecimal( decYearCompletionHigh ), "#,0" );
                            labelYearCompletionHigh.Text = strYearCompletionHigh.Replace( "-", "△" );
                        }
                    }
                }
            }

            // 20190227 asakawa
            // 年度内完工高の計算方法を変更 その２
            // add start
            decYearCompletionHigh = yearCompHigh - carryOverPlanned;
            if (textBoxCarryOverPlanned.Text.ToString().Trim() == "")
            {
                labelYearCompletionHigh.Text = "";
            }
            else
            {
                labelYearCompletionHigh.Text = MinusConvert(decYearCompletionHigh);
            }
            // add end

        }


        private void comboBoxYear_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            //今年度以外は編集不可にする。
            DateTime dtNow = DateTime.Now;

            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadOnlyColorClear();
            if( dtNow.FiscalYear() == Convert.ToInt32( comboBoxYear.Text ) )
            {
                uih.DgvRowsReadOnly( readOnlyRows );
                uih.DgvColumnsReadOnly( readOnlyColumns );
                uih.DgvRowsBackColorSet( readOnlyRows, rowsBackColorColumns, Color.PaleGreen );
                uih.DgvColumnsColor( readOnlyColumns, Color.PaleGreen );
                textBoxCarryOverPlanned.Enabled = true;
                textBoxNote.Enabled = true;
                buttonGetCost.Enabled = true;
                buttonSaveNextTask.Enabled = true;
                buttonSaveBeforeTask.Enabled = true;
                buttonTaskSort.Enabled = true;
                buttonSave.Enabled = true;
                comboBoxTaskState.Enabled = true;
                CommonData com = new CommonData();          // M_Commonアクセスクラス
                //ClosingDate = Convert.ToInt32( com.SelectCloseDate( Convert.ToString( this.comboBoxOfficeCode.SelectedValue ) ).ToString( "MM" ) );
                ClosingDate = closingMonth( Convert.ToString( comboBoxOfficeCode.SelectedValue ) );
                int[] monthArray = new int[] { 0, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
                int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );
                int[] ReadOnlySet = new int[DisplyLimit];
                for( int i = 0; i < DisplyLimit; i++ ) ReadOnlySet[i] = i;
                uih.DgvColumnsReadOnly( ReadOnlySet );
            }
            else
            {
                uih.DgvRowsReadOnly( readOnlyAllRows );
                uih.DgvColumnsReadOnly( readOnlyAllColumns );
                uih.DgvRowsColor( readOnlyAllRows, Color.PaleGreen );
                uih.DgvColumnsColor( readOnlyAllColumns, Color.PaleGreen );
                textBoxCarryOverPlanned.Enabled = false;
                textBoxNote.Enabled = false;
                buttonGetCost.Enabled = false;
                buttonSaveNextTask.Enabled = false;
                buttonSaveBeforeTask.Enabled = false;
                buttonTaskSort.Enabled = false;
                buttonSave.Enabled = false;
                comboBoxTaskState.Enabled = false;
                ClosingDate = 6;
            }
            ScreenDisplayUpdate();
            TaskCodeSort();
        }


        private void comboOfficeCode_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            // TRY kusano 20170426 
            //comboBoxDepartment.Visible = false;
            CreateCbDepartment();
            //if( comboBoxOfficeCode.SelectedIndex == 0 ) comboBoxDepartment.Visible = true;
            // TRY End
            createTaskCodeCB( comboBoxTaskCode );
            DateTime dtNow = DateTime.Now;
            // 最終締め月を取得する
            if( dtNow.FiscalYear() == Convert.ToInt32( comboBoxYear.Text ) )
            {
                CommonData com = new CommonData();          // M_Commonアクセスクラス
                //ClosingDate = Convert.ToInt32( com.SelectCloseDate( Convert.ToString( this.comboBoxOfficeCode.SelectedValue ) ).ToString( "MM" ) );
                ClosingDate = closingMonth( Convert.ToString( comboBoxOfficeCode.SelectedValue ) );
            }
            else
                ClosingDate = 6;
            ScreenDisplayUpdate();
        }


        private void textBoxCarryOverPlanned_Enter( object sender, EventArgs e )
        {
            if( textBoxCarryOverPlanned.Text.Trim() != "" )
                textBoxCarryOverPlanned.Text = SignConvert( textBoxCarryOverPlanned.Text ).ToString( "#,0" );
        }


        private void textBoxCarryOverPlanned_Leave( object sender, EventArgs e )
        {
            if( textBoxCarryOverPlanned.Text.ToString().Trim() != "" )
            {
                if( DHandling.IsDecimal( textBoxCarryOverPlanned.Text ) == true )
                    textBoxCarryOverPlanned.Text = MinusConvert( textBoxCarryOverPlanned.Text );
                else
                    textBoxCarryOverPlanned.Text = "";
            }
        }


        private void dataGridView1_UserDeletingRow( object sender, DataGridViewRowCancelEventArgs e )
        {
            if( dataGridView1.SelectedCells.IsReadOnly == true )
                e.Cancel = true;
        }


        private void comboBoxDepartment_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            createTaskCodeCB( comboBoxTaskCode );
            ScreenDisplayUpdate();
        }


        private void comboBoxTaskCode_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            releaseExclusive();
            ScreenDisplayUpdate();
            checkExclusive();
        }
        //-----------------------------//
        // comboBox作成                //
        //-----------------------------//
        private void CreateCbYear()
        {
            DateTime dtNow = DateTime.Now;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxYear );
            cbe.ValueItem = new string[5];
            cbe.DisplayItem = new string[5];
            for( int i = 0; i < cbe.ValueItem.Length; i++ )
            {
                cbe.ValueItem[i] = i.ToString();
                cbe.DisplayItem[i] = ( dtNow.FiscalYear() - i ).ToString();
            }

            cbe.Basic();
        }


        private void CreateCbPeriod()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxPeriod );
            cbe.ValueItem = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            cbe.DisplayItem = new string[] { "全ての月", "7月まで", "8月まで", "9月まで", "10月まで", "11月まで", "12月まで", "1月まで", "2月まで", "3月まで", "4月まで", "5月まで", "6月まで" };
            cbe.Basic();
        }


        private void CreateCbTaskState()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxTaskState );
            cbe.ValueItem = new string[] { "0", "1", "2", "3" };
            cbe.DisplayItem = new string[] { "稼働", "完了", "休止中", "完全完了" };
            cbe.Basic();
        }


        // 事業所
        private void CreateCbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOfficeCode );
            //cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
            cbe.TableData("M_Office", "OfficeCode", "OfficeName", hp.AccessLevel);

            OfficeArray = new string[cbe.ValueItem.Length];
            Array.Copy( cbe.ValueItem, 0, OfficeArray, 0, OfficeArray.Length );
            comboBoxOfficeCode.SelectedValue = hp.OfficeCode;
        }


        // 部門
        private void CreateCbDepartment()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepartment );
            cbe.DepartmentList( ( comboBoxOfficeCode.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            if(comboBoxOfficeCode.Text == Sign.HQOffice )
            {
                comboBoxDepartment.SelectedValue = ( hp.Department == "0" || hp.Department == "8" || hp.Department == "9"  ) ? "1" : hp.Department;
                comboBoxDepartment.Visible = true;
            }
            else
            {
                comboBoxDepartment.SelectedValue = "8";
                comboBoxDepartment.Visible = false;
            }
        }


        private void createTaskCodeCB( ComboBox cbTaskCode )
        {
            ComboBoxEdit cbe = new ComboBoxEdit( cbTaskCode );
            string volDepartmentParam = "";
            string tiDepartmentParam = "";
            if( comboBoxOfficeCode.Text == Sign.HQOffice )
            {
                if( string.IsNullOrEmpty( Convert.ToString( comboBoxDepartment.SelectedValue ) ) ) comboBoxDepartment.SelectedValue = "1";
                volDepartmentParam = " AND D_V.Department = " + "'" + comboBoxDepartment.SelectedValue.ToString() + "' ";
                tiDepartmentParam = " AND D_TI.Department = " + "'" + comboBoxDepartment.SelectedValue.ToString() + "' ";
            }
            else
            {
                if( string.IsNullOrEmpty( Convert.ToString( comboBoxDepartment.SelectedValue ) ) ) comboBoxDepartment.SelectedValue = "8";
                volDepartmentParam = " AND D_V.Department = '8' ";
                tiDepartmentParam = " AND D_TI.Department = '8' ";
            }
            string yearMonthParam = " YearMonth = " + comboBoxYear.Text + Conv.FisicalYearStartMonth.ToString( "00" ) + " OR YearMonth IS NULL ";
            string volOfficeCodeParam = " D_V.OfficeCode = '" + comboBoxOfficeCode.SelectedValue.ToString() + "' ";
            string tiOfficeCodeParam = " D_TI.OfficeCode = '" + comboBoxOfficeCode.SelectedValue.ToString() + "' ";

            // 20171025 Asakawa
            //   出来高台帳の年度選択欄で選択した事業年度内に施工期間を有する業務のみを業務番号の選択欄に表示するように改修
            // 　以下およびSQL文部分の下から２行目を追加

            // 20180717 asakawa 削除済のものを表示させないための修正
            // + " AND (D_T.OldVerMark = 0 AND D_TI.OldVerMark = 0)" を追加

            string strEndDate = comboBoxYear.Text + "-07-01";
            string strStartDate = Convert.ToString(int.Parse(comboBoxYear.Text) + 1) + "-06-30";
            // 　ここまで追加

            string strTable = "DISTINCT D_T.TaskID AS TaskID, D_TI.TaskCode AS TaskCode, ISNULL(D_V.TaskStat, 0) AS TaskStat, D_V.YearMonth AS YearMonth, LEFT(D_TI.TaskCode,1) AS TaskDep "
                            + " FROM D_Task AS D_T "
                            + " INNER JOIN D_TaskInd AS D_TI ON D_TI.TaskID = D_T.TaskID "
                            + " LEFT JOIN D_Volume AS D_V ON D_V.TaskCode = D_TI.TaskCode "
                            + " AND (" + yearMonthParam + ") AND" + volOfficeCodeParam + volDepartmentParam
                            + " WHERE " + tiOfficeCodeParam
                            + tiDepartmentParam
                            + " AND ( NOT (D_T.EndDate < '" + strEndDate + "' OR D_T.StartDate > '" + strStartDate + "'))"
                            + " AND (D_T.OldVerMark = 0 AND D_TI.OldVerMark = 0)"
                            + " ORDER BY TaskStat,TaskCode ASC ";
            if( cbe.TableDataForCostData( "TaskCode", strTable ) == false )
            {
                cbTaskCode.DataSource = null;
                cbTaskCode.Items.Add( new object() );
                cbTaskCode.Items.Clear();
            }




            //20170419kusano


            checkExclusive();




        }


        private void VolumeDataInit()
        {
            comboBoxPeriod.SelectedIndex = 0;       //期間
            comboBoxTaskCode.SelectedIndex = 0;     //業務番号
            labelTaskName.Text = "";                //業務名
            labelSupplierName.Text = "";            //業者名
            comboBoxTaskState.SelectedIndex = 0;    //業務状態
            labelStartDate.Text = "";               //工期開始
            labelEndDate.Text = "";                 //工期終了
            labelOrdersForm.Text = "";              //受注形態
            textBoxCarryOverPlanned.Text = "";      //繰越予定額
            labelYearCompletionHigh.Text = "";      //年度内完工高
            labelContact.Text = "";                 //担当者
            labelClaimform.Text = "";               //請求形態
            labelPayNote.Text = "";                 //支払条件
            comboBoxYear.Text = "";

            for( int i = 0; i <= 12; i++ ) ClrVolumeInf( i );
        }


        // 原価内訳データを得る
        private void chooseCostDetailData( int lNo )
        {
            CostReportData[] crd = null;
            switch( lNo )
            {
                case 1:
                    crd = crdM07;
                    break;
                case 2:
                    crd = crdM08;
                    break;
                case 3:
                    crd = crdM09;
                    break;
                case 4:
                    crd = crdM10;
                    break;
                case 5:
                    crd = crdM11;
                    break;
                case 6:
                    crd = crdM12;
                    break;
                case 7:
                    crd = crdM01;
                    break;
                case 8:
                    crd = crdM02;
                    break;
                case 9:
                    crd = crdM03;
                    break;
                case 10:
                    crd = crdM04;
                    break;
                case 11:
                    crd = crdM05;
                    break;
                case 12:
                    crd = crdM06;
                    break;
                default:
                    break;
            }

            FormVolumeCostDetailList formCostList = new FormVolumeCostDetailList( crd );
            formCostList.ShowDialog();
        }


        private bool dispPreYearVolumeData( string taskCode, int yearData, int preYearData, string officeCode, string department )
        {
            VolumeData vd = new VolumeData();
            volumedata = vd.SelectVolumeData( officeCode, department, taskCode, yearData, preYearData );
            if( volumedata == null ) return false;
            loadPreYearVolumeData( volumedata, dataGridView1 );
            return true;
        }


        private void loadPreYearVolumeData( VolumeData[] volumedata, DataGridView dgv )
        {
            if( volumedata.Count() < 1 ) return;
            dgv.Rows[0].Cells["LY"].Value = "";
            dgv.Rows[2].Cells["LY"].Value = "";
            dgv.Rows[3].Cells["LY"].Value = "";
            dgv.Rows[4].Cells["LY"].Value = "";
            dgv.Rows[8].Cells["LY"].Value = "";
            dgv.Rows[10].Cells["LY"].Value = "";
            dgv.Rows[11].Cells["LY"].Value = "";
            dgv.Rows[13].Cells["LY"].Value = "";
            dgv.Rows[16].Cells["LY"].Value = "";
            dgv.Rows[21].Cells["LY"].Value = "";

            if( volumedata[0].MonthlyVolume != null && volumedata[0].MonthlyVolume != 0 )
                dgv.Rows[0].Cells["LY"].Value = MinusConvert( volumedata[0].MonthlyVolume );   //受注単月

            if( volumedata[0].VolUncomp != null && volumedata[0].VolUncomp != 0 )
                dgv.Rows[2].Cells["LY"].Value = MinusConvert( volumedata[0].VolUncomp );        //出来高未成業務
                                                                                                // Wakamatsu 20170308
            if( volumedata[0].VolClaimRem != null && volumedata[0].VolClaimRem != 0 )
                dgv.Rows[3].Cells["LY"].Value = MinusConvert( volumedata[0].VolClaimRem );     //出来高未請求
                                                                                               // Wakamatsu 20170308
            if( volumedata[0].VolClaim != null && volumedata[0].VolClaim != 0 )
                dgv.Rows[4].Cells["LY"].Value = MinusConvert( volumedata[0].VolClaim );        //出来高請求

            if( volumedata[0].MonthlyClaim != null && volumedata[0].MonthlyClaim != 0 )
                dgv.Rows[8].Cells["LY"].Value = MinusConvert( volumedata[0].MonthlyClaim );     //請求単月

            if( volumedata[0].VolPaid != null && volumedata[0].VolPaid != 0 )
                dgv.Rows[11].Cells["LY"].Value = MinusConvert( volumedata[0].VolPaid );         //入金単月

            if( volumedata[0].ClaimDate != null && volumedata[0].ClaimDate != DateTime.MinValue )
                dgv.Rows[10].Cells["LY"].Value = Convert.ToDateTime( volumedata[0].ClaimDate ).ToString( "yyyy/MM/dd" );     //請求日

            if( volumedata[0].PaidDate != null && volumedata[0].PaidDate != DateTime.MinValue )
                dgv.Rows[13].Cells["LY"].Value = Convert.ToDateTime( volumedata[0].PaidDate ).ToString( "yyyy/MM/dd" );     //入金日

            if( volumedata[0].MonthlyCost != null && volumedata[0].MonthlyCost != 0 )
                dgv.Rows[16].Cells["LY"].Value = MinusConvert( volumedata[0].MonthlyCost );     //原価単月
        }


        private bool dispVolumeData( int colName, string taskCode, int yearMonth, string officeCode, string department, int colCnt )
        {
            VolumeData vd = new VolumeData();
            volumedata = vd.SelectVolumeData( taskCode, yearMonth, officeCode, department );

            if( volumedata == null ) return false;
            loadVolumeData( colName, volumedata, dataGridView1, colCnt );

            return true;
        }


        private void loadVolumeData( int intColName, VolumeData[] volumedata, DataGridView dgv, int intColCnt )
        {
            if( volumedata.Count() < 1 ) return;

            string strM = "M";
            strM = strM + intColName.ToString();
            intColCnt = 0;
            dgv.Rows[0].Cells[strM].Value = "";
            dgv.Rows[2].Cells[strM].Value = "";
            dgv.Rows[3].Cells[strM].Value = "";
            dgv.Rows[4].Cells[strM].Value = "";
            dgv.Rows[8].Cells[strM].Value = "";
            dgv.Rows[10].Cells[strM].Value = "";
            dgv.Rows[11].Cells[strM].Value = "";
            dgv.Rows[13].Cells[strM].Value = "";
            dgv.Rows[16].Cells[strM].Value = "";
            dgv.Rows[21].Cells[strM].Value = "";

            if( volumedata[intColCnt].MonthlyVolume != null )
                dgv.Rows[0].Cells[strM].Value = MinusConvert( volumedata[intColCnt].MonthlyVolume );        //受注単月
                                                                                                            // Wakamatsu 20170308
            if( volumedata[intColCnt].VolUncomp != null )
                dgv.Rows[2].Cells[strM].Value = MinusConvert( volumedata[intColCnt].VolUncomp );            //出来高未成業務
                                                                                                            // Wakamatsu 20170308
            if( volumedata[intColCnt].VolClaimRem != null )
                dgv.Rows[3].Cells[strM].Value = MinusConvert( volumedata[intColCnt].VolClaimRem );          //出来高未請求
                                                                                                            // Wakamatsu 20170308
            if( volumedata[intColCnt].VolClaim != null )
                dgv.Rows[4].Cells[strM].Value = MinusConvert( volumedata[intColCnt].VolClaim );             //出来高請求
                                                                                                            // Wakamatsu 20170308
            if( volumedata[intColCnt].MonthlyClaim != null )
                dgv.Rows[8].Cells[strM].Value = MinusConvert( volumedata[intColCnt].MonthlyClaim );         //請求単月
                                                                                                            // Wakamatsu 20170308
            if( volumedata[intColCnt].ClaimDate != null )
            {
                DateTime dtClaimDate = Convert.ToDateTime( volumedata[intColCnt].ClaimDate );
                dgv.Rows[10].Cells[strM].Value = dtClaimDate.ToString( "yyyy/MM/dd" );   //請求日
            }

            if( volumedata[intColCnt].VolPaid != null )
                dgv.Rows[11].Cells[strM].Value = MinusConvert( volumedata[intColCnt].VolPaid );             //入金単月
                                                                                                            // Wakamatsu 20170308

            if( volumedata[intColCnt].PaidDate != null )
            {
                DateTime dtPaidDate = Convert.ToDateTime( volumedata[intColCnt].PaidDate );
                dgv.Rows[13].Cells[strM].Value = dtPaidDate.ToString( "yyyy/MM/dd" );  //入金日
            }

            if( volumedata[intColCnt].MonthlyCost != null )
                dgv.Rows[16].Cells[strM].Value = MinusConvert( volumedata[intColCnt].MonthlyCost );     //原価単月
                                                                                                        // Wakamatsu 20170308
            if( volumedata[intColCnt].Comment != null )
                dgv.Rows[21].Cells[strM].Value = volumedata[intColCnt].Comment;    //コメント
            if( volumedata[intColCnt].CarryOverPlanned != null )
                textBoxCarryOverPlanned.Text = MinusConvert( volumedata[intColCnt].CarryOverPlanned );

            if( intColCnt == 0 )
            {
                if( volumedata[intColCnt].Note != null )
                    textBoxNote.Text = volumedata[intColCnt].Note;    //備考
            }
            string strTaskState = Convert.ToString( volumedata[intColCnt].TaskStat );
        }


        private void ScreenDisplay()
        {
            if( iniPro ) return;
            grdSet = true;

            initGridData();

            for( int i = 0; i <= 12; i++ )
            {
                ClrVolumeInf( i );
            }

            comboBoxTaskState.SelectedIndex = 0;
            textBoxCarryOverPlanned.Text = "";
            labelYearCompletionHigh.Text = "";
            textBoxNote.Text = "";

            string officeCode = Convert.ToString( comboBoxOfficeCode.SelectedValue );
            string department = ( officeCode == "H" ) ? Convert.ToString( comboBoxDepartment.SelectedValue ) : "";

            //業務テーブル関連
            SetTaskInfContents( comboBoxTaskCode.Text, officeCode, department );

            //出来高データ取得（前年度）
            dispPreYearVolumeData( comboBoxTaskCode.Text, Convert.ToInt32( comboBoxYear.Text ), Convert.ToInt32( comboBoxYear.Text ) - 1,
                                    officeCode, department );

            //出来高データ取得（今年度）
            int curYear = Convert.ToInt32( comboBoxYear.Text );
            int curMonth;

            int[] monthArray = new int[] { 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );
            this.SuspendLayout();
            for( int i = 0; i < 12; i++ )
            {
                curMonth = i + 7;
                if( curMonth > 12 ) curMonth = curMonth - 12;
                if( curMonth == 1 ) curYear++;
                if( i <= DisplyLimit )
                    dispVolumeData( curMonth, comboBoxTaskCode.Text, curYear * 100 + curMonth, officeCode, department, i );
            }
            //原価
            SetOriginalCost();
            grdSet = false;

            //自動計算処理
            AutoCalc( dataGridView1 );
            this.ResumeLayout();
        }


        private void AutoCalc( DataGridView dgv )
        {
            int[] monthArray = new int[] { 0, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );

            for( int i = 0; i < monthArray.Length; i++ )
            {
                if( i <= DisplyLimit )
                {
                    SetCumulative( dgv, i );        //受注累計
                    SetMonthlyTotal( dgv, i );      //月計
                    SetTotalTradingVolume( dgv, i );//累計
                    SetOverTime( dgv, i );          //残業務高
                    SetResidualClaimHigh( dgv, i ); //残請求高
                    SetCumulativeM( dgv, i );       //請求累計
                    SetUncompBusAccept( dgv, i );   //未成業務受入金
                    SetCumulativeV( dgv, i );       //入金累計
                    SetAccountsReceivable( dgv, i );//未収入金
                    SetUncompBusAcceptM( dgv, i );  //未成業務受入金
                    SetCumulativeMC( dgv, i );      //原価累計
                    SetCostRate( dgv, i );          //原価率
                }
            }
        }


        private void SetTaskInfContents( string taskCode, string officeCode, string department )
        {
            if( taskCode.Trim() == "" ) return;
            SqlHandling sh = new SqlHandling();
            string departmentSql = "";
            if( department != "" )
                departmentSql = " AND D_V.Department = " + "'" + department + "'";

            //string sqlStr = "DISTINCT D_T.TaskName AS TaskName, D_T.StartDate AS StartDate, D_T.EndDate AS EndDate, D_T.OrdersForm AS OrdersForm, D_T.TaskLeader AS TaskLeader, D_T.ClaimForm AS ClaimForm, D_T.PayNote AS PayNote,"
            // 2019.01.21 Asakawa
            // 2019.01.29 asakawa 再度修正
            // string sqlStr = "DISTINCT D_T.TaskName AS TaskName, D_T.StartDate AS StartDate, D_T.EndDate AS EndDate, D_T.OrdersForm AS OrdersForm, D_T.TaskLeader AS TaskLeader, D_T.ClaimForm AS ClaimForm, D_T.PayNote AS PayNote, D_T.SalesMCode AS SalesMCode,"
            string sqlStr = "DISTINCT D_T.TaskName AS TaskName, D_T.StartDate AS StartDate, D_T.EndDate AS EndDate, D_T.OrdersForm AS OrdersForm, D_T.TaskLeader AS TaskLeader, D_T.ClaimForm AS ClaimForm, D_T.PayNote AS PayNote, D_T.SalesMCode AS SalesMCode, D_TI.LeaderMCode AS LeaderMCode,"
                    + " D_TI.TaskCode AS TaskCode, M_P.PartnerName AS PartnerName, ISNULL(D_V.TaskStat, 0) AS TaskStat FROM D_Task D_T "
                    + " INNER JOIN D_TaskInd D_TI ON D_T.TaskID = D_TI.TaskID "
                    + " LEFT JOIN M_Partners M_P ON D_T.PartnerCode = M_P.PartnerCode "
                    + " LEFT JOIN D_Volume D_V ON D_TI.TaskCode = D_V.TaskCode "
                    + " AND D_TI.OfficeCode = D_V.OfficeCode AND D_V.YearMonth = " + "'" + comboBoxYear.Text + "07" + "'"
                    + departmentSql
                    + " WHERE D_TI.TaskCode = " + "'" + taskCode + "'"
                    + " AND D_TI.OfficeCode =" + "'" + officeCode + "'";

            System.Data.DataTable dt = sh.SelectFullDescription( sqlStr );

            labelTaskName.Text = "";
            labelSupplierName.Text = "";
            labelStartDate.Text = "";
            labelEndDate.Text = "";
            labelOrdersForm.Text = "";
            labelContact.Text = "";
            labelClaimform.Text = "";
            labelPayNote.Text = "";

            if( ( dt != null ) && ( dt.Rows.Count > 0 ) )
            {
                DataRow dr = dt.Rows[0];
                //業務名
                if( dr["TaskName"] != null && dr["TaskName"] != DBNull.Value ) labelTaskName.Text = Convert.ToString( dr["TaskName"] );
                //取引先名
                if( dr["PartnerName"] != null && dr["PartnerName"] != DBNull.Value ) labelSupplierName.Text = Convert.ToString( dr["PartnerName"] );
                //工期開始日
                if( dr["StartDate"] != null && dr["StartDate"] != DBNull.Value )
                {
                    DateTime dateStartDate = Convert.ToDateTime( dr["StartDate"] );
                    labelStartDate.Text = dateStartDate.ToString( "yyyy/MM/dd" );
                }
                //工期終了日
                if( dr["EndDate"] != null && dr["EndDate"] != DBNull.Value )
                {
                    DateTime dateEndDate = Convert.ToDateTime( dr["EndDate"] );
                    labelEndDate.Text = dateEndDate.ToString( "yyyy/MM/dd" );
                }
                //受注形態
                if( dr["OrdersForm"] != null && dr["OrdersForm"] != DBNull.Value )
                {
                    labelOrdersForm.Text = ( Convert.ToInt32( dr["OrdersForm"] ) == 0 ) ? "請負" : "常傭";
                }
                //請求形態
                if( dr["ClaimForm"] != null && dr["ClaimForm"] != DBNull.Value )
                {
                    labelClaimform.Text = ( Convert.ToInt32( dr["ClaimForm"] ) == 0 ) ? "月次" : "完成";
                }

                //担当者
                //************************* 20170530 kusano
                // comment out
                //if( dr["TaskLeader"] != null && dr["TaskLeader"] != DBNull.Value )
                //    labelContact.Text = Convert.ToString( dr["TaskLeader"] );
                // add
                //************************* 20190121 Asakawa
                // 営業担当＋業務担当へ変更
                // comment out
                //MembersData md = new MembersData();
                //if( dr["SalesMCode"] != null && dr["SalesMCode"] != DBNull.Value )
                //    labelContact.Text = md.SelectMemberName(Convert.ToString( dr["SalesMCode"] ));
                //************************* 20170530 kusano end
                // add
                // 2019.01.29 asakawa 再度修正
                MembersData md = new MembersData();
                MembersData md2 = new MembersData();
                labelContact.Text = "";
                if (dr["SalesMCode"] != null && dr["SalesMCode"] != DBNull.Value)
                    labelContact.Text += md.SelectMemberName(Convert.ToString(dr["SalesMCode"]));
                labelContact.Text += ":";
                if (dr["LeaderMCode"] != null && dr["LeaderMCode"] != DBNull.Value)
                    labelContact.Text += md2.SelectMemberName(Convert.ToString(dr["LeaderMCode"]));

                //************************* 20190121 Asakawa end

                //支払条件
                if ( dr["PayNote"] != null && dr["PayNote"] != DBNull.Value ) labelPayNote.Text = Convert.ToString( dr["PayNote"] );

                ////支払条件
                //if (dr["PayNote"] != null && dr["PayNote"] != DBNull.Value)
                //    textBoxPayNote.Text = Convert.ToString(dr["PayNote"]);

                //業務状態
                if( dr["TaskStat"] != null && dr["TaskStat"] != DBNull.Value ) comboBoxTaskState.SelectedIndex = Convert.ToInt32( dr["TaskStat"] );
            }
            else
            {
                comboBoxTaskState.SelectedIndex = 0;
            }
        }


        private void ClrVolumeInf( int i )
        {
            foreach( var row in dataGridView1.Rows.Cast<DataGridViewRow>() )
            {
                row.Cells[i].Value = "";
            }
        }


        //受注累計
        private void SetCumulative( DataGridView dgv, int i )
        {
            if( i == 0 )
            {
                if( !checkCellValue( dgv, i ) )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[0].Cells[i].Value ) ) )
                    {
                        dgv.Rows[1].Cells[i].Value = dgv.Rows[0].Cells[i].Value;
                        totalCumulativeAry[i] = SignConvert( dgv.Rows[0].Cells[i].Value );
                    }
                    else
                    {
                        dgv.Rows[1].Cells[i].Value = 0;
                        totalCumulativeAry[i] = 0;
                    }
                }
                return;
            }

            dgv.Rows[1].Cells[i].Value = "";//受注累計


            Calculation calc = new Calculation();
            // 受注累計算出
            dgv.Rows[1].Cells[i].Value = calc.Cumulative( dgv, i, 0, totalCumulativeAry[i - 1], checkCellValue( dgv, i ), "#,0", out totalCumulativeAry[i] );

            //年度内完工高更新
            //繰越予定額
            decimal decCarryOverPlanned = 0;
            if( textBoxCarryOverPlanned.Text.ToString().Trim() != "" )
                decCarryOverPlanned = SignConvert( textBoxCarryOverPlanned.Text );
            decimal decYearCompletionHigh = totalCumulativeAry[i] - totalCumulativeAry[0] - decCarryOverPlanned;
            textBoxYearCompHigh.Text = ( totalCumulativeAry[i] - totalCumulativeAry[0] ).ToString();

            labelYearCompletionHigh.Text = "";
            for( int j = 1; j <= i; j++ )
            // Wakamatsu
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[0].Cells[j].Value ) ) )
                {
                    if( decYearCompletionHigh != 0 )
                    {
                        labelYearCompletionHigh.Text = MinusConvert( decYearCompletionHigh );
                    }
                }
            }

            // 20190227 asakawa
            // 年度内完工高の計算方法を変更　その３
            // add start
            decYearCompletionHigh = totalCumulativeAry[i] - totalTradingVolumeAry[0] - decCarryOverPlanned;
            textBoxYearCompHigh.Text = (totalCumulativeAry[i] - totalTradingVolumeAry[0]).ToString();
            if (textBoxCarryOverPlanned.Text.ToString().Trim() == "")
            {
                labelYearCompletionHigh.Text = "";
            }
            else
            {
                labelYearCompletionHigh.Text = MinusConvert(decYearCompletionHigh);
            }
            // add end
        }


        //月計
        private void SetMonthlyTotal( DataGridView dgv, int i )
        {
            dgv.Rows[5].Cells[i].Value = "";

            decimal monthlyTotal = 0;

            if( !checkCellValue( dgv, i ) )
            {
                Calculation calc = new Calculation();
                // 月計算出
                monthlyTotal = calc.MonthlyTotal( dgv, i, 2, 3, 4 );
                dgv.Rows[5].Cells[i].Value = MinusConvert( monthlyTotal );     //出来高 単月 月計
            }
        }


        //出来高累計
        private void SetTotalTradingVolume( DataGridView dgv, int i )
        {
            if( i == 0 )
            {
                if( !checkCellValue( dgv, i ) )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[5].Cells[i].Value ) ) )
                    {
                        dgv.Rows[6].Cells[i].Value = dgv.Rows[5].Cells[i].Value;
                        totalTradingVolumeAry[i] = SignConvert( dgv.Rows[5].Cells[i].Value );
                    }
                    else
                    {
                        dgv.Rows[6].Cells[i].Value = 0;
                        totalTradingVolumeAry[i] = 0;
                    }
                }
                return;
            }

            dgv.Rows[6].Cells[i].Value = "";//受注累計

            Calculation calc = new Calculation();
            // 出来高累計算出
            dgv.Rows[6].Cells[i].Value = calc.TotalTradingVolume( dgv, i, 5, 2, 3, 4, totalTradingVolumeAry[i - 1], "#,0", out totalTradingVolumeAry[i] );
        }

        //請求累計
        private void SetCumulativeM( DataGridView dgv, int i )
        {
            if( i == 0 )
            {
                if( !checkCellValue( dgv, i ) )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[8].Cells[i].Value ) ) )
                    {
                        dgv.Rows[9].Cells[i].Value = dgv.Rows[8].Cells[i].Value;
                        totalCumulativeMAry[i] = SignConvert( dgv.Rows[8].Cells[i].Value );
                    }
                    else
                    {
                        dgv.Rows[9].Cells[i].Value = 0;
                        totalCumulativeMAry[i] = 0;
                    }
                }
                return;
            }

            dgv.Rows[9].Cells[i].Value = "";//受注累計

            Calculation calc = new Calculation();
            // 請求累計算出
            dgv.Rows[9].Cells[i].Value = calc.Cumulative( dgv, i, 8, totalCumulativeMAry[i - 1], checkCellValue( dgv, i ), "#,0", out totalCumulativeMAry[i] );
        }


        //入金累計
        private void SetCumulativeV( DataGridView dgv, int i )
        {
            if( i == 0 )
            {
                if( !checkCellValue( dgv, i ) )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[11].Cells[i].Value ) ) )
                    {
                        dgv.Rows[12].Cells[i].Value = dgv.Rows[11].Cells[i].Value;
                        totalCumulativeVAry[i] = SignConvert( dgv.Rows[11].Cells[i].Value );
                    }
                    else
                    {
                        dgv.Rows[12].Cells[i].Value = 0;
                        totalCumulativeVAry[i] = 0;
                    }
                }
                return;
            }

            dgv.Rows[12].Cells[i].Value = "";//入金累計

            Calculation calc = new Calculation();
            // 入金累計算出
            dgv.Rows[12].Cells[i].Value = calc.Cumulative( dgv, i, 11, totalCumulativeVAry[i - 1], checkCellValue( dgv, i ), "#,0", out totalCumulativeVAry[i] );
        }


        //原価累計
        private void SetCumulativeMC( DataGridView dgv, int i )
        {
            if( i == 0 )
            {
                if( !checkCellValue( dgv, i ) )
                {
                    if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[16].Cells[i].Value ) ) )
                    {
                        dgv.Rows[17].Cells[i].Value = dgv.Rows[16].Cells[i].Value;
                        totalCumulativeMCAry[i] = SignConvert( dgv.Rows[16].Cells[i].Value );
                    }
                    else
                    {
                        dgv.Rows[17].Cells[i].Value = 0;
                        totalCumulativeMCAry[i] = 0;
                    }
                }
                return;
            }

            dgv.Rows[17].Cells[i].Value = "";//原価累計

            Calculation calc = new Calculation();
            // 原価累計算出
            dgv.Rows[17].Cells[i].Value = calc.Cumulative( dgv, i, 16, totalCumulativeMCAry[i - 1], checkCellValue( dgv, i ), "#,0", out totalCumulativeMCAry[i] );
        }


        //残業務高
        private void SetOverTime( DataGridView dgv, int i )
        {
            dgv.Rows[7].Cells[i].Value = "";//残業務高

            if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[1].Cells[i].Value ) ) ) ||
                ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[6].Cells[i].Value ) ) ) )
            {
                Calculation calc = new Calculation();
                // 未成業務受入金算出
                dgv.Rows[7].Cells[i].Value = MinusConvert( calc.SubtrahendVol( dgv, i, 0, 2, 3, 4 ) );
            }
        }


        //残請求高
        private void SetResidualClaimHigh( DataGridView dgv, int i )
        {
            dgv.Rows[14].Cells[i].Value = "";//残請求高
            dgv[i, 14].Style.BackColor = Color.PaleGreen;

            decimal decResidualClaimHigh = 0;
            if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[6].Cells[i].Value ) ) ) ||
                ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[9].Cells[i].Value ) ) ) )
            {
                Calculation calc = new Calculation();
                // 残請求高算出
                decResidualClaimHigh = calc.MinuendVol( dgv, i, 2, 3, 4, 8 );

                if( decResidualClaimHigh > 0 )
                {
                    dgv.Rows[14].Cells[i].Value = MinusConvert( decResidualClaimHigh );
                    if( buttonSave.Enabled == true )
                        dgv[i, 14].Style.BackColor = Color.Pink;
                }
            }
        }


        //未成業務受入金
        private void SetUncompBusAccept( DataGridView dgv, int i )
        {
            dgv.Rows[15].Cells[i].Value = "";//未成業務受入金

            decimal uncompBusAccept = 0;

            if( ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[6].Cells[i].Value ) ) ) ||
                ( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[9].Cells[i].Value ) ) ) )
            {
                Calculation calc = new Calculation();
                // 未成業務受入金算出
                uncompBusAccept = calc.SubtrahendVol( dgv, i, 8, 2, 3, 4 );
                if( uncompBusAccept > 0 )
                    dgv.Rows[15].Cells[i].Value = MinusConvert( uncompBusAccept );
            }
        }


        //原価率
        private void SetCostRate( DataGridView dgv, int i )
        {
            Calculation calc = new Calculation();
            // 原価算出
            dgv.Rows[18].Cells[i].Value = calc.CostRate( dgv, i, 6, 17 );
        }


        //未収入金
        private void SetAccountsReceivable( DataGridView dgv, int i )
        {
            dgv.Rows[19].Cells[i].Value = "";             //未収入金
            dgv[i, 19].Style.BackColor = Color.PaleGreen;
            decimal accountsReceivable = 0;                      //未収入金

            dgv.Rows[19].Cells[i].Value = "";

            if( !checkCellValue( dgv, i ) )
            {
                Calculation calc = new Calculation();
                // 未収入金算出
                accountsReceivable = calc.AccountsReceivable( dgv, i, 2, 3, 4, 8, 11 );
                if( accountsReceivable >= 0 )
                {
                    dgv.Rows[19].Cells[i].Value = MinusConvert( accountsReceivable );
                    if( ( accountsReceivable == 0 ) && ( buttonSave.Enabled == true ) )
                        dgv[i, 19].Style.BackColor = Color.Pink;
                }
            }
        }


        //未成業務受入金
        private void SetUncompBusAcceptM( DataGridView dgv, int i )
        {
            dgv.Rows[20].Cells[i].Value = "";//未成業務受入金
            decimal uncompBusAcceptM = 0;//未成業務受入金

            if( !checkCellValue( dgv, i ) )
            {
                Calculation calc = new Calculation();
                uncompBusAcceptM = calc.SubtrahendVol( dgv, i, 11, 2, 3, 4 );

                if( uncompBusAcceptM > 0 )
                    dgv.Rows[20].Cells[i].Value = MinusConvert( uncompBusAcceptM );
            }
        }


        private bool VolumeSave( DataGridView dgv )
        {
            if( !checkReserved() ) return false;

            int[] monthArray = new int[] { 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );

            //業務が未選択の場合処理を抜ける
            if( comboBoxTaskCode.SelectedItem.ToString().Trim() == "" )
                return false;

            //年が未設定の場合処理を抜ける
            if( comboBoxYear.Text.Trim() == "" )
                return false;

            // 未成業務受入金が発生しているのに備考2が空欄の場合処理を抜ける
            // 経理総務のメンバー以外（hp.Department != "0")
            if(hp.Department != "0" )
            {
                if( !checkDepositData( dgv ) ) return false;
            }

            //for( int i = 0; i < monthArray.Length; i++ )
            //{
            //    if( i >= DisplyLimit )
            //    {
            //        decimal CheckDeposit = 0;

            //        if( decimal.TryParse( Convert.ToString( dgv.Rows[14].Cells[i + 1].Value ), out CheckDeposit ) )
            //        {
            //            if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
            //            {
            //                MessageBox.Show( monthArray[i] + "月に残請求高が発生しています。\r\n" +
            //                                "コメントを入力してください。" );
            //                return false;
            //            }
            //        }

            //        // 最終締め月の未成業務受入金を確認する
            //        // 未成業務受入金(請求 - 出来高)
            //        if( decimal.TryParse( Convert.ToString( dgv.Rows[15].Cells[i + 1].Value ), out CheckDeposit ) )
            //        {
            //            if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
            //            {
            //                MessageBox.Show( monthArray[i] + "月に未成業務受入金が発生しています。\r\n" +
            //                                "コメントを入力してください。" );
            //                return false;
            //            }
            //        }

            //        // 未成業務受入金(入金 - 出来高)
            //        if( decimal.TryParse( Convert.ToString( dgv.Rows[20].Cells[i + 1].Value ), out CheckDeposit ) )
            //        {
            //            if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
            //            {
            //                MessageBox.Show( monthArray[i] + "月に未成業務受入金が発生しています。\r\n" +
            //                                "コメントを入力してください。" );
            //                return false;
            //            }
            //        }
            //    }
            //}

            bool ret = false;
            string yearMonth = comboBoxYear.Text;
            for( int i = 0; i < monthArray.Length; i++ )
            {
                decimal? WorkDecimal = null;
                Calculation calc = new Calculation();

                ClassLibrary.VolumeData volume = new ClassLibrary.VolumeData();

                //業務番号
                volume.TaskCode = comboBoxTaskCode.Text;

                //年月
                volume.YearMonth = ( i < 6 ) ? Convert.ToInt32( comboBoxYear.Text ) * 100 + monthArray[i] : ( Convert.ToInt32( comboBoxYear.Text ) + 1 ) * 100 + monthArray[i];

                //受注単月
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[0].Cells[i + 1].Value ) ) )
                    volume.MonthlyVolume = SignConvert( dgv.Rows[0].Cells[i + 1].Value );

                //出来高未成業務
                volume.VolUncomp = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[2].Cells[i + 1].Value ) ) )
                    volume.VolUncomp = SignConvert( dgv.Rows[2].Cells[i + 1].Value );

                //出来高未請求
                volume.VolClaimRem = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[3].Cells[i + 1].Value ) ) )
                    volume.VolClaimRem = SignConvert( dgv.Rows[3].Cells[i + 1].Value );

                //出来高請求
                volume.VolClaim = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[4].Cells[i + 1].Value ) ) )
                    volume.VolClaim = SignConvert( dgv.Rows[4].Cells[i + 1].Value );

                //入金額
                volume.VolPaid = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[11].Cells[i + 1].Value ) ) )
                    volume.VolPaid = SignConvert( dgv.Rows[11].Cells[i + 1].Value );

                //請求単月
                volume.MonthlyClaim = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[8].Cells[i + 1].Value ) ) )
                    volume.MonthlyClaim = SignConvert( dgv.Rows[8].Cells[i + 1].Value );

                //請求日
                volume.ClaimDate = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[10].Cells[i + 1].Value ) ) )
                    volume.ClaimDate = Convert.ToDateTime( dgv.Rows[10].Cells[i + 1].Value );

                //入金日
                volume.PaidDate = null;
                if( ( dgv.Rows[13].Cells[i + 1].Value != null ) && ( dgv.Rows[13].Cells[i + 1].Value.ToString().Trim() != "" ) )
                    volume.PaidDate = Convert.ToDateTime( dgv.Rows[13].Cells[i + 1].Value );

                // 未成業務受入金(請求 - 出来高)
                volume.Deposit1 = null;
                WorkDecimal = calc.SubtrahendVol( dgv, i + 1, 8, 2, 3, 4 );
                if( WorkDecimal > 0 ) volume.Deposit1 = WorkDecimal;

                //原価単月
                volume.MonthlyCost = null;
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[16].Cells[i + 1].Value ) ) )
                    volume.MonthlyCost = SignConvert( dgv.Rows[16].Cells[i + 1].Value );

                //残請求高金額
                volume.BalanceClaim = null;
                WorkDecimal = calc.MinuendVol( dgv, i + 1, 2, 3, 4, 8 );
                if( WorkDecimal > 0 ) volume.BalanceClaim = WorkDecimal;

                //未収入金額
                volume.BalanceIncom = null;
                WorkDecimal = calc.AccountsReceivable( dgv, i + 1, 2, 3, 4, 8, 11 );
                if( WorkDecimal > 0 ) volume.BalanceIncom = WorkDecimal;
                // 未成業務受入金(請求 - 出来高)
                volume.Deposit2 = null;
                WorkDecimal = calc.SubtrahendVol( dgv, i + 1, 11, 2, 3, 4 );
                if( WorkDecimal > 0 ) volume.Deposit2 = WorkDecimal;

                //コメント
                volume.Comment = "";
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) ) )
                    volume.Comment = Convert.ToString( dgv.Rows[21].Cells[i + 1].Value );

                //備考
                volume.Note = "";
                if( !String.IsNullOrEmpty( Convert.ToString( textBoxNote.Text ) ) ) volume.Note = textBoxNote.Text.ToString();

                //備考2
                //volume.Note2 = "";
                //if (!String.IsNullOrEmpty(Convert.ToString(textBoxNote2.Text)))
                //    volume.Note2 = textBoxNote2.Text.ToString();

                //業務状況
                volume.TaskStat = comboBoxTaskState.SelectedIndex;

                //繰越予定額
                volume.CarryOverPlanned = null;
                if( !String.IsNullOrEmpty( Convert.ToString( textBoxCarryOverPlanned.Text ) ) )
                    volume.CarryOverPlanned = SignConvert( textBoxCarryOverPlanned.Text );

                //事業所コード
                volume.OfficeCode = comboBoxOfficeCode.SelectedValue.ToString();

                //部門コード
                volume.Department = ( volume.OfficeCode == "H" ) ? comboBoxDepartment.SelectedValue.ToString() : "8";

                if( volume.ExistenceTaskCodeYearMonth( "D_Volume" ) )
                {
                    ret = volume.UpdateVolume( volume );
                }
                else
                {
                    ret = volume.InsertVolume( volume );
                }
                if( !ret ) break;
            }

            // 締め月累計格納
            ret = YearVolumeSave( DisplyLimit + 1 );

            string strMassege = ( ret ) ? "保存しました。" : "保存に失敗しました。";
            MessageBox.Show( this, strMassege );




            // 20170419kusano



            //releaseExclusive();

            return ret;
        }


        private void VolumeExcelPrint()
        {
            // print 処理
            PublishVolume publ = new PublishVolume( Folder.DefaultExcelTemplate( "出来高台帳.xlsx" ) );
            publ.ExcelFile( "Volume", editPublishData(), dataGridView1 );
        }


        private PublishData editPublishData()
        {
            PublishData pd = new PublishData();

            pd.vTaskCode = comboBoxTaskCode.Text;                   //業務番号
            pd.vTaskName = labelTaskName.Text;                      //業務名
            pd.vSupplierName = labelSupplierName.Text;              //業者名
            pd.vStartDate = labelStartDate.Text;                    //工期開始
            pd.vEndDate = labelEndDate.Text;                        //工期終了
            pd.vOrdersForm = labelOrdersForm.Text;                  //受注形態
            pd.vCarryOverPlanned = textBoxCarryOverPlanned.Text;    //繰越予定額
            pd.vYearCompletionHigh = labelYearCompletionHigh.Text;  //年内完工高
            pd.vContact = labelContact.Text;                        //担当者
            pd.vClaimform = labelClaimform.Text;                    //請求形態
            pd.vPayNote = labelPayNote.Text;                        //支払条件
            pd.vYear = comboBoxYear.Text + "年度" + "－" + comboBoxOfficeCode.Text; //年
            if( comboBoxDepartment.Visible ) pd.vYear = pd.vYear + "－" + comboBoxDepartment.Text;
            pd.vTaskStat = comboBoxTaskState.Text;                  //業務状態
            pd.vNote = textBoxNote.Text;

            return pd;
        }


        private void NextTaskCode()
        {
            releaseExclusive();

            if( comboBoxTaskCode.SelectedIndex < comboBoxTaskCode.Items.Count - 1 ) comboBoxTaskCode.SelectedIndex++;
        }


        private void BeforeTaskCode()
        {
            releaseExclusive();

            if( comboBoxTaskCode.SelectedIndex > 0 ) comboBoxTaskCode.SelectedIndex--;
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private void SetOriginalCost()
        {
            string taskCode = comboBoxTaskCode.Text;
            string officeCode = comboBoxOfficeCode.SelectedValue.ToString();
            string yearMonth = "";
            string month = "";
            crdM07 = null;
            crdM08 = null;
            crdM09 = null;
            crdM10 = null;
            crdM11 = null;
            crdM12 = null;
            crdM01 = null;
            crdM02 = null;
            crdM03 = null;
            crdM04 = null;
            crdM05 = null;
            crdM06 = null;

            if( taskCode.Trim() == "" ) return;
            string strSql = "";

            int[] monthArray = new int[] { 0, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );

            for( int i = 1; i < monthArray.Length; i++ )
            {
                yearMonth = comboBoxYear.Text;
                int intMonth = i + 6;
                if( intMonth > 12 )
                {
                    intMonth = intMonth - 12;
                    yearMonth = Convert.ToString( Convert.ToInt32( comboBoxYear.Text ) + 1 );
                }
                month = "0" + intMonth.ToString();
                if( month.Length > 2 ) month = month.Substring( 1, 2 );

                yearMonth = yearMonth + "-" + month;
                strSql = " AND ReportDate LIKE " + "'" + yearMonth + "%'";

                if( i <= DisplyLimit )
                {
                    SqlHandling sh = new SqlHandling();

                    string sqlStr = " SUM(Cost) AS CostReportTotalMony FROM D_CostReport "
                                    + " WHERE TaskCode = " + "'" + taskCode + "'"
                                    + " AND OfficeCode = " + "'" + officeCode + "'"
                                    + strSql;
                    System.Data.DataTable dt = sh.SelectFullDescription( sqlStr );

                    dataGridView1.Rows[16].Cells[i].Value = "";
                    if( ( dt != null ) && ( dt.Rows.Count > 0 ) )
                    {
                        DataRow dr = dt.Rows[0];
                        if( dr["CostReportTotalMony"] != null && dr["CostReportTotalMony"] != DBNull.Value )
                            dataGridView1.Rows[16].Cells[i].Value = DHandling.DecimaltoStr( Convert.ToDecimal( dr["CostReportTotalMony"] ), "#,0" );
                        //注文明細を設定
                        sqlStr = " ReportDate, SlipNo, ItemCode, Item, Unit, UnitPrice, Quantity, Cost, SUM(Cost) AS CostReportTotalMony FROM D_CostReport "
                                    + " WHERE TaskCode = " + "'" + taskCode + "'"
                                    + " AND OfficeCode = " + "'" + officeCode + "'"
                                    + strSql
                                    + " GROUP BY ReportDate, SlipNo, ItemCode, Item, Unit, UnitPrice, Quantity, Cost "
                                    + " ORDER BY ReportDate ASC ";
                        dt = sh.SelectFullDescription( sqlStr );

                        if( ( dt != null ) && ( dt.Rows.Count > 0 ) )
                        {
                            CostReportData[] crd = new CostReportData[dt.Rows.Count];
                            for( int j = 0; j < dt.Rows.Count; j++ )
                            {
                                CostReportData crdata = new CostReportData();
                                crd[j] = crdata;
                                dr = dt.Rows[j];
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["ReportDate"] ) ) )
                                    crd[j].ReportDate = Convert.ToDateTime( dr["ReportDate"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["SlipNo"] ) ) )
                                    crd[j].SlipNo = Convert.ToInt32( dr["SlipNo"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["ItemCode"] ) ) )
                                    crd[j].ItemCode = Convert.ToString( dr["ItemCode"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["Item"] ) ) )
                                    crd[j].Item = Convert.ToString( dr["Item"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["Unit"] ) ) )
                                    crd[j].Unit = Convert.ToString( dr["Unit"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["UnitPrice"] ) ) )
                                    crd[j].UnitPrice = Convert.ToDecimal( dr["UnitPrice"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["Quantity"] ) ) )
                                    crd[j].Quantity = Convert.ToDecimal( dr["Quantity"] );
                                if( !String.IsNullOrEmpty( Convert.ToString( dr["Cost"] ) ) )
                                    crd[j].Cost = Convert.ToDecimal( dr["Cost"] );
                            }

                            switch( month )
                            {
                                case "07":
                                    crdM07 = crd;
                                    break;
                                case "08":
                                    crdM08 = crd;
                                    break;
                                case "09":
                                    crdM09 = crd;
                                    break;
                                case "10":
                                    crdM10 = crd;
                                    break;
                                case "11":
                                    crdM11 = crd;
                                    break;
                                case "12":
                                    crdM12 = crd;
                                    break;
                                case "01":
                                    crdM01 = crd;
                                    break;
                                case "02":
                                    crdM02 = crd;
                                    break;
                                case "03":
                                    crdM03 = crd;
                                    break;
                                case "04":
                                    crdM04 = crd;
                                    break;
                                case "05":
                                    crdM05 = crd;
                                    break;
                                case "06":
                                    crdM06 = crd;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }


        private void ScreenDisplayUpdate()
        {
            if( iniPro ) return;

            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvRowsBackColorSet( readOnlyRows, rowsBackColorColumns, Color.PaleGreen );

            //業務テーブル関連
            SetTaskInfContents( comboBoxTaskCode.Text, Convert.ToString( comboBoxOfficeCode.SelectedValue ),
                                Convert.ToString( comboBoxOfficeCode.SelectedValue ) == "H" ? Convert.ToString( comboBoxDepartment.SelectedValue ) : "" );
            ScreenDisplay();
        }


        private void TaskCodeSort()
        {
            string selectValue = comboBoxTaskCode.Text.Trim();
            createTaskCodeCB( comboBoxTaskCode );

            int index = comboBoxTaskCode.FindStringExact( selectValue );
            comboBoxTaskCode.SelectedIndex = index;
        }


        private void initGridData()
        {
            for( int i = 0; i < 13; i++ )
            {
                cumulativeAry[i] = 0;
                totalCumulativeAry[i] = 0;
                volUncompAry[i] = 0;
                volClaimRemAry[i] = 0;
                volClaimAry[i] = 0;
                monthlyTotalAry[i] = 0;
                totalTradingVolumeAry[i] = 0;
                OverTime[i] = 0;
                cumulativeMAry[i] = 0;
                totalCumulativeMAry[i] = 0;
                claimDateAry[i] = "";
                cumulativeVAry[i] = 0;
                totalCumulativeVAry[i] = 0;
                paidDateAry[i] = "";
                cumulativeMCAry[i] = 0;
                totalCumulativeMCAry[i] = 0;
                setCostRateAry[i] = 0;
            }
        }


        /// <summary>
        /// セルの値確認
        /// </summary>
        /// <param name="dgv"> 確認対象データグリッドビュー</param>
        /// <param name="idx">　セルのインデックス</param>
        /// <returns>　true:すべて空白</returns>
        /// <returns>　false:どこかに値がある</returns>
        private bool checkCellValue( DataGridView dgv, int idx )
        {
            int[] rIdxArray = new int[] { 0, 2, 3, 4, 8, 11, 16 };
            for( int i = 0; i < rIdxArray.Length; i++ )
            {
                if( !String.IsNullOrEmpty( Convert.ToString( dgv.Rows[rIdxArray[i]].Cells[idx].Value ) ) ) return false;
            }

            return true;
        }


        /// <summary>
        /// "-" → "△"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private string MinusConvert( object TargetValue )
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString( TargetValue );

            if( WorkString != "" )
            {
                // "-" → "△"コンバート
                Decimal.TryParse( WorkString, out WorkDecimal );
                if( WorkDecimal < 0 )
                    return "△" + ( WorkDecimal * -1 ).ToString( "#,0" );
                else
                    return WorkDecimal.ToString( "#,0" );
            }
            return "";
        }

        /// <summary>
        /// "△" → "-"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private decimal SignConvert( object TargetValue )
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString( TargetValue );

            if( WorkString != "" )
            {
                // "△" → "-"コンバート
                if( WorkString.Substring( 0, 1 ) == "△" )
                {
                    Decimal.TryParse( WorkString.Substring( 1 ), out WorkDecimal );
                    return WorkDecimal * -1;
                }
                else
                {
                    Decimal.TryParse( WorkString, out WorkDecimal );
                    return WorkDecimal;
                }
            }
            return 0;
        }


        /// <summary>
        /// 来年度用前年データ格納
        /// </summary>
        /// <param name="ClosingYear">締め月</param>
        /// <returns></returns>
        private bool YearVolumeSave( int ClosingYear )
        {
            decimal? WorkDecimal = null;
            Calculation calc = new Calculation();

            VolumeData volume = new VolumeData();

            //業務番号
            volume.TaskCode = comboBoxTaskCode.Text;

            //年度
            volume.YearMonth = Convert.ToInt32( comboBoxYear.Text );

            //受注累計
            volume.MonthlyVolume = totalCumulativeAry[ClosingYear];

            volume.VolUncomp = 0;
            volume.VolClaimRem = 0;
            volume.VolClaim = 0;
            for( int i = 0; i <= ClosingYear; i++ )
            {
                //出来高未成業務累計
                volume.VolUncomp += SignConvert( this.dataGridView1.Rows[2].Cells[i].Value );

                //出来高未請求
                volume.VolClaimRem += SignConvert( this.dataGridView1.Rows[3].Cells[i].Value );

                //出来高請求
                volume.VolClaim += SignConvert( this.dataGridView1.Rows[4].Cells[i].Value );
            }

            //請求累計
            volume.MonthlyClaim = totalCumulativeMAry[ClosingYear];

            //請求日
            if( !String.IsNullOrEmpty( Convert.ToString( this.dataGridView1.Rows[10].Cells[ClosingYear].Value ) ) )
                volume.ClaimDate = Convert.ToDateTime( this.dataGridView1.Rows[10].Cells[ClosingYear].Value );

            //入金累計
            volume.VolPaid = totalCumulativeVAry[ClosingYear];

            //入金日
            if( !String.IsNullOrEmpty( Convert.ToString( this.dataGridView1.Rows[13].Cells[ClosingYear].Value ) ) )
                volume.PaidDate = Convert.ToDateTime( this.dataGridView1.Rows[13].Cells[ClosingYear].Value );

            //残請求高金額
            WorkDecimal = calc.MinuendVol( this.dataGridView1, ClosingYear, 2, 3, 4, 8 );
            if( WorkDecimal > 0 )
                volume.BalanceClaim = WorkDecimal;

            // 未成業務受入金(請求 - 出来高)
            WorkDecimal = calc.SubtrahendVol( this.dataGridView1, ClosingYear, 8, 2, 3, 4 );
            if( WorkDecimal > 0 )
                volume.Deposit1 = WorkDecimal;

            //原価累計
            volume.MonthlyCost = totalCumulativeMCAry[ClosingYear];

            //未収入金額
            WorkDecimal = calc.AccountsReceivable( this.dataGridView1, ClosingYear, 2, 3, 4, 8, 11 );
            if( WorkDecimal > 0 )
                volume.BalanceIncom = WorkDecimal;

            // 未成業務受入金(入金 - 出来高)
            WorkDecimal = calc.SubtrahendVol( this.dataGridView1, ClosingYear, 11, 2, 3, 4 );
            if( WorkDecimal > 0 )
                volume.Deposit2 = WorkDecimal;

            //事業所コード
            volume.OfficeCode = comboBoxOfficeCode.SelectedValue.ToString();

            //部門コード
            volume.Department = ( volume.OfficeCode == "H" ) ? comboBoxDepartment.SelectedValue.ToString() : "8";

            if( volume.ExistenceTaskCodeYearMonth( "D_YearVolume" ) )
                //更新
                return volume.UpdateYearVolume( volume );
            else
                //追加
                return volume.InsertYearVolume( volume );
        }


        private bool checkExclusive()
        {
            Exclusive excl = new Exclusive();
            if( excl.CheckAndRegistered( "FormVolume", null, comboBoxTaskCode.Text, hp.MemberCode ) )
            {
                return true;
            }
            else
            {
                exclusiveMessage( excl.UserName );
                return false;
            }
        }

        private bool checkReserved()
        {
            Exclusive excl = new Exclusive();
            //if( !excl.CheckRegisterUser( "FormVolume", comboBoxTaskCode.Text, hp.MemberCode ) )
            if( excl.CheckRegisterUser( "FormVolume", comboBoxTaskCode.Text, hp.MemberCode ) )
            {
                exclusiveMessage( excl.UserName );
                return false;
            }
            return true;
        }

        private bool releaseExclusive()
        {
            Exclusive excl = new Exclusive();
            return excl.Unregister( "FormVolume", hp.MemberCode );
        }


        private void exclusiveMessage( string memberCode )
        {
            string showMes = "業務番号：" + comboBoxTaskCode.Text + " は、";
            MembersData md = new MembersData();
            if( string.IsNullOrEmpty( md.SelectMemberName( memberCode ) ) )
            {
                showMes += "他の方が使用中です。更新できません。";
            }
            else
            {
                showMes += md.SelectMemberName( memberCode ) + "さんが使用中です。更新できません。";
            }

            MessageBox.Show( showMes );
        }


        private int closingMonth(string officeCode)
        {
            // 出来高台帳の処理は締月の翌月までを対象とする。
            CommonData com = new CommonData();          // M_Commonアクセスクラス

            DateTime closingYMD = com.SelectCloseDate( officeCode );
            int closeMonth = Convert.ToInt32( ( closingYMD.AddMonths( 1 ) ).ToString( "MM" ) );
            return closeMonth;
        }



        private bool checkDepositData(DataGridView dgv )
        {
            int[] monthArray = new int[] { 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
            int DisplyLimit = Array.IndexOf( monthArray, ClosingDate );

            for( int i = 0; i < monthArray.Length; i++ )
            {
                if( i >= DisplyLimit )
                {
                    decimal CheckDeposit = 0;

                    if( decimal.TryParse( Convert.ToString( dgv.Rows[14].Cells[i + 1].Value ), out CheckDeposit ) )
                    {
                        if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
                        {
                            MessageBox.Show( monthArray[i] + "月に残請求高が発生しています。\r\n" +
                                            "コメントを入力してください。" );
                            return false;
                        }
                    }

                    // 最終締め月の未成業務受入金を確認する
                    // 未成業務受入金(請求 - 出来高)
                    if( decimal.TryParse( Convert.ToString( dgv.Rows[15].Cells[i + 1].Value ), out CheckDeposit ) )
                    {
                        if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
                        {
                            MessageBox.Show( monthArray[i] + "月に未成業務受入金が発生しています。\r\n" +
                                            "コメントを入力してください。" );
                            return false;
                        }
                    }

                    // 未成業務受入金(入金 - 出来高)
                    if( decimal.TryParse( Convert.ToString( dgv.Rows[20].Cells[i + 1].Value ), out CheckDeposit ) )
                    {
                        if( CheckDeposit > 0 && Convert.ToString( dgv.Rows[21].Cells[i + 1].Value ) == "" )
                        {
                            MessageBox.Show( monthArray[i] + "月に未成業務受入金が発生しています。\r\n" +
                                            "コメントを入力してください。" );
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
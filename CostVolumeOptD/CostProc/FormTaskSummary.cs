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
using ListForm;

namespace CostProc
{
    public partial class FormTaskSummary :Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        CostReportData[] crd;
        HumanProperty hp;
        TaskCodeNameData[] tcd;
        private bool iniPro = true;

        const string HQOffice = "本社";
        const string noDataMes = "処理できるデータがありません。";
        const string faildProc = "処理に失敗しました。";
        private int iniRCnt = 24;
        private DateTime preReportDate = DateTime.MinValue;
        private int preSlipNo = 0;
        private int slipNoCount;
        //--------------------------------------------------------------------------//
        //     Constructor                                                          //
        //--------------------------------------------------------------------------//
        public FormTaskSummary()
        {
            InitializeComponent();
        }

        public FormTaskSummary( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }

        //--------------------------------------------------------------------------//
        //     Property                                                             //
        //--------------------------------------------------------------------------//

        //--------------------------------------------------------------------------//
        //     Method                                                               //
        //--------------------------------------------------------------------------//
        private void FormTaskSummary_Load( object sender, EventArgs e )
        {
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            uih.DgvNotSortable( dataGridView1 );

            initializeScreen();
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepartment );
        }


        private void FormTaskSummary_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            setPreData();
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonOK":
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(iniRCnt);
                    if ( !dispTaskInformation( textBoxTaskCode.Text ) ) return;
                    dispCostReportData();
                    preReportDate = DateTime.MinValue;
                    break;

                case "buttonPrint":
                    ExcelExport();
                    break;

                case "buttonEnd":
                    saveNowData();
                    this.Close();
                    break;

                default:
                    break;
            }
        }


        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( iniPro ) return;

            ComboBox cb = ( ComboBox )sender;
            ListFormDataOp lo = new ListFormDataOp();
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepartment );
            switch( cb.Name )
            {
                case "comboBoxOffice":
                    initializeTaskCostData();
                    create_cbDepart();
                    tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null );
                    break;

                case "comboBoxDepartment":
                    tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null );  // Task情報
                    break;

                default:
                    break;
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
        }


        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;

            TextBox tb = ( TextBox )sender;

            if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            {
                switch( tb.Name )
                {
                    case "textBoxTaskCode":
                        initializeTaskCostData();
                        if( !dispTaskInformation( textBoxTaskCode.Text ) )
                        {
                            MessageBox.Show( "指定された業務番号のデータはありません" );
                        }
                        break;
                    default:
                        break;
                }
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    initializeTaskCostData();
                    chooseTaskCodeNameData();
                    break;

                default:
                    break;
            }

        }

        // Wakamatsu 20170302
        private void textBoxTaskCode_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            initializeTaskCostData();
        }
        // Wakamatsu 20170302

        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            DateTimePicker dtp = ( DateTimePicker )sender;
            if( dtp.Name == "dateTimePickerTO" )
            {
                if( dateTimePickerTO.Value < dateTimePickerFR.Value )
                {
                    DMessage.Contradiction( "日付の前後関係が" );
                    dateTimePickerTO.Value = DateTime.Now;
                }
            }
            // Wakamatsu 20170302
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            // Wakamatsu 20170302
        }
        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//

        private void initializeScreen()
        {
            textBoxTaskCode.Text = "";
            initializeTaskCostData();

            create_cbOffice();
            create_cbDepart();

            dateTimePickerFR.Value = DateTime.Now;
            dateTimePickerTO.Value = DateTime.Now;

            ListFormDataOp lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameData( string.IsNullOrEmpty( TaskSummary.Default.Office ) ? "H" : TaskSummary.Default.Office,
                                             string.IsNullOrEmpty( TaskSummary.Default.Department ) ? "2" : TaskSummary.Default.Department, null );  // Task情報 
        }

        private void initializeTaskCostData()
        {
            labelTaskName.Text = "";
            labelPartnerName.Text = "";
            labelTerm.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );

        }

        // 部門
        private void create_cbDepart()
        {
            comboBoxDepartment.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepartment );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );

            if( hp.OfficeCode == Sign.HQOfficeCode )
            {
                comboBoxDepartment.SelectedValue = ( hp.Department == "0" ) ? "2" : hp.Department;
            }
            else
            {
                comboBoxDepartment.SelectedValue = "8";
            }
        }


        private void chooseTaskCodeNameData()
        {
            TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
            if( tcds == null ) return;

            textBoxTaskCode.Text = tcds.TaskCode;
            labelTaskName.Text = tcds.TaskName;
            dispTaskInformation( textBoxTaskCode.Text );
        }


        private bool dispTaskInformation( string taskCode )
        {
            ListFormDataOp lo = new ListFormDataOp();
            TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( taskCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
            TaskData td = lo.SelectTaskData( taskCode );
            if( td == null ) return false;
            labelTaskName.Text = td.TaskName;
            labelTerm.Text = "工期：";
            labelTerm.Text += ( td.StartDate.StripTime() ).ToString( "yyyy年MM月dd日" ) + " ～ ";
            labelTerm.Text += ( td.EndDate.StripTime() ).ToString( "yyyy年MM月dd日" );

            if( td.PartnerCode != null )
            {
                PartnersData pd = new PartnersData();
                labelPartnerName.Text = pd.SelectPartnerName( td.PartnerCode );
            }

            return true;
        }


        private bool dispCostReportData()
        {
            CostReportData crp = new CostReportData();
            crd = crp.SelectCostReport( dateTimePickerFR.Value.StripTime(), dateTimePickerTO.Value.StripTime(),
                                        textBoxTaskCode.Text, Conv.OfficeCode, Conv.DepartCode );
            if( crd == null ) return false;
            slipNoCount = crp.CountSlipNo( dateTimePickerFR.Value, dateTimePickerTO.Value,
                                           textBoxTaskCode.Text, Conv.OfficeCode, Conv.DepartCode );
            loadCostRecordData( crd, dataGridView1 );
            return true;
        }



        private void loadCostRecordData( CostReportData[] crd, DataGridView dgv )
        {
            StringUtility su = new StringUtility();
            dgv.Rows.Clear();
            int addRCnt = ( crd.GetLength( 0 ) == 0 ) ? iniRCnt : crd.GetLength( 0 ) + slipNoCount - 1;
            dgv.Rows.Add( addRCnt );
            decimal slipNoCost = 0M;
            decimal totalCost = 0M;
            char checkChar;
            int j = 0;
            for( int i = 0; i < crd.GetLength( 0 ); i++ )
            {
                if( preSlipNo != crd[i].SlipNo )
                {
                    if( preSlipNo > 0 )
                    {
                        dgv.Rows[j].Cells["Quantity"].Value = "伝票計";
                        dgv.Rows[j].Cells["Cost"].Value = decFormat( slipNoCost );
                        totalCost += slipNoCost;
                        slipNoCost = 0M;
                        dgv.Rows[j].Cells["Balance"].Value = decFormat( totalCost );
                        j++;
                    }
                    dgv.Rows[j].Cells["SlipNo"].Value = crd[i].SlipNo;
                    preSlipNo = crd[i].SlipNo;
                }

                if( preReportDate != crd[i].ReportDate )
                {
                    dgv.Rows[j].Cells["ReportDate"].Value = crd[i].ReportDate.ToString( "MM月dd日" );
                    preReportDate = crd[i].ReportDate;
                }
                dgv.Rows[j].Cells["ItemCode"].Value = crd[i].ItemCode;
                if( crd[i].ItemCode == "K999" )
                {
                    checkChar = crd[i].Item[0];
                    dgv.Rows[j].Cells["Item"].Value = ( checkChar == '●' ) ? crd[i].Item : "●" + crd[i].Item;
                    //dgv.Rows[j].Cells["Item"].Value = (su.SubstringByte(crd[i].Item, 0, 1) == "●") ? crd[i].Item : "●" + crd[i].Item;
                }
                else
                {
                    dgv.Rows[j].Cells["Item"].Value = crd[i].Item;
                }
                dgv.Rows[j].Cells["Quantity"].Value = ( Convert.ToString( crd[i].Quantity ) == "" ) ? "" : decPointFormat( crd[i].Quantity );
                dgv.Rows[j].Cells["UnitPrice"].Value = ( Convert.ToString( crd[i].UnitPrice ) == "0" ) ? "" : decFormat( crd[i].UnitPrice );
                dgv.Rows[j].Cells["Cost"].Value = ( Convert.ToString( crd[i].Cost ) == "0" ) ? "" : decFormat( crd[i].Cost );
                slipNoCost += ( Convert.ToString( dgv.Rows[j].Cells["Cost"].Value ) == "" ) ? 0 : Convert.ToDecimal( dgv.Rows[j].Cells["Cost"].Value );
                j++;
            }
            dgv.Rows[j].Cells["Quantity"].Value = "伝票計";
            dgv.Rows[j].Cells["Cost"].Value = decFormat( slipNoCost );
            totalCost += slipNoCost;
            dgv.Rows[j].Cells["Balance"].Value = decFormat( totalCost );

            preSlipNo = 0;
        }


        private void ExcelExport()
        {
            PublishData pd = new PublishData();
            pd.OfficeName = Convert.ToString( this.comboBoxOffice.Text );                             // 部門
            pd.Department = Convert.ToString( this.comboBoxDepartment.Text );                         // 部署
            pd.OrderStartDate = Convert.ToDateTime( this.dateTimePickerFR.Text );                     // 表示期間開始
            pd.OrderEndDate = Convert.ToDateTime( this.dateTimePickerTO.Text );                       // 表示期間終了
            pd.TaskCode = Convert.ToString( this.textBoxTaskCode.Text );                              // 業務番号
            pd.TaskName = Convert.ToString( this.labelTaskName.Text );                                // 業務名
            pd.PartnerName = Convert.ToString( this.labelPartnerName.Text );                          // 取引先名
            pd.Note = Convert.ToString( this.labelTerm.Text ).Replace( "工期：", "" );                 // 工期

            PrintOut.Publish publ = new PrintOut.Publish( Folder.DefaultExcelTemplate( "業務元帳.xlsx" ) );
            publ.ExcelFile( "TaskSummary", pd, this.dataGridView1 );
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private static string decPointFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "0.00" );
        }


        private void setPreData()
        {
            //TaskSummary.Default.Reset();

            string[] dArray = new string[3];
            if( !string.IsNullOrEmpty( TaskSummary.Default.Office ) )
            {
                comboBoxOffice.Text = TaskSummary.Default.OfficeName;
                comboBoxOffice.SelectedValue = TaskSummary.Default.Office;
            }

            if( !string.IsNullOrEmpty( TaskSummary.Default.Department ) )
                comboBoxDepartment.SelectedValue = TaskSummary.Default.Department;

            if( !string.IsNullOrEmpty( Convert.ToString( TaskSummary.Default.FR ) ) )
            {
                dArray = ( TaskSummary.Default.FR ).Split( ',' );
                dateTimePickerFR.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if( !string.IsNullOrEmpty( Convert.ToString( TaskSummary.Default.TO ) ) )
            {
                dArray = ( TaskSummary.Default.TO ).Split( ',' );
                dateTimePickerTO.Value = new DateTime( Convert.ToInt32( dArray[0] ), Convert.ToInt32( dArray[1] ), Convert.ToInt32( dArray[2] ) );
            }

            if( !string.IsNullOrEmpty( TaskSummary.Default.TaskCode ) )
                textBoxTaskCode.Text = TaskSummary.Default.TaskCode;
        }


        private void saveNowData()
        {
            if( comboBoxOffice != null )
            {
                TaskSummary.Default.Office = Conv.OfficeCode;
                TaskSummary.Default.OfficeName = Conv.Office;
            }
            if( comboBoxDepartment != null ) TaskSummary.Default.Department = Conv.DepartCode;

            TaskSummary.Default.FR = dateTimePickerFR.Value.ToString( "yyyy,MM,dd" );
            TaskSummary.Default.TO = dateTimePickerTO.Value.ToString( "yyyy,MM,dd" );

            if( textBoxTaskCode.Text != null ) TaskSummary.Default.TaskCode = Convert.ToString( textBoxTaskCode.Text );

            TaskSummary.Default.Save();
        }

    }
}

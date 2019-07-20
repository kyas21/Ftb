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
    public partial class FormCostInformation :Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        HumanProperty hp;
        private bool iniPro = true;
        private int iniRCnt = 30;
        private DateTime procDate;
        private int curNo;
        private DateTime[] dayArray;
        private int[] dowArray;
        private int[] hdayArray;
        private decimal[,] workHArray;
        private decimal[,] overHArray;
        private int[,] checkArray;


        const string HQOffice = "本社";
        const string noPrevMes = "「前データ」はありません。";
        const string noNextMes = "「次データ」はありません。";
        const string noDataMes = "処理できるデータがありません。";
        const string faildProc = "処理に失敗しました。";
        const int colNum = 7;
        //--------------------------------------------------------------------------//
        //     Constructor                                                          //
        //--------------------------------------------------------------------------//
        public FormCostInformation()
        {
            InitializeComponent();
        }


        public FormCostInformation( HumanProperty hp )
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
        private void FormCostInformation_Load( object sender, EventArgs e )
        {
            initializeScreen();
            if(displayMembersData( dataGridView1, hp.OfficeCode, hp.Department ))
                readyDataArray();
        }


        private void FormCostInformation_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;

            labelMessage.Text = "";
            switch( btn.Name )
            {
                case "buttonPrev":
                case "buttonPWeek":
                    if( curNo == 0 )
                    {
                        labelMessage.Text = noPrevMes;
                        return;
                    }
                    break;

                case "buttonNext":
                case "buttonNWeek":
                    if( ( curNo + 7 ) >= dayArray.Length )
                    {
                        labelMessage.Text = noNextMes;
                        return;
                    }
                    break;

                case "buttonEnd":
                    this.Close();
                    return;

                default:
                    break;
            }

            switch( btn.Name )
            {
                case "buttonPrev":
                    curNo--;
                    break;

                case "buttonNext":
                    curNo++;
                    break;

                case "buttonPWeek":
                    curNo = ( curNo < 7 ) ? 0 : curNo -= 7;
                    break;

                case "buttonNWeek":
                    curNo = ( ( curNo + 7 ) >= ( dayArray.Length - 7 ) ) ? curNo = dayArray.Length - 7 : curNo += 7;
                    break;

                case "buttonEnd":
                    this.Close();
                    break;

                default:
                    break;
            }
            displayWorkingData( dataGridView1, curNo );
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            ComboBox cb = ( ComboBox )sender;
            switch( cb.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    break;

                case "comboBoxDepart":
                    break;

                default:
                    break;
            }
            if(displayMembersData( dataGridView1, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) ))
            readyDataArray();
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            DateTimePicker dtp = ( DateTimePicker )sender;
            procDate = dtp.Value;
            readyDataArray();
        }

        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        private void initializeScreen()
        {
            labelMessage.Text = "";

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            uih.DgvNotSortable( dataGridView1 );
            uih.DgvColumnsWrapModeON();

            create_cbOffice();
            comboBoxOffice.Text = hp.OfficeName;
            create_cbDepart();
            comboBoxDepart.Text = hp.DepartName;        // 初期値

            create_cbMonth();

            init_dateTimePicker();
        }



        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName", hp.AccessLevel );
        }


        // 部門
        private void create_cbDepart()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == HQOffice ) ? "DEPH" : "DEPB" );
        }


        private void create_cbMonth()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxMonth );
            cbe.MonthList( 7 );
            CommonData cdp = new CommonData();
            procDate = cdp.SelectCloseDate( Convert.ToString( comboBoxOffice.SelectedValue ) );
            procDate = ( procDate == DateTime.MinValue ) ? DateTime.Today : procDate.AddDays( 1 );
            comboBoxMonth.SelectedValue = procDate.ToString( "MM" );
        }


        private void init_dateTimePicker()
        {
            CommonData cdp = new CommonData();
            procDate = cdp.SelectCloseDate( Convert.ToString( comboBoxOffice.SelectedValue ) );
            procDate = ( procDate == DateTime.MinValue ) ? DateTime.Today : procDate.AddDays( 1 );
            dateTimePickerMonth.Value = procDate;
        }


        //private void displayMembersData( DataGridView dgv, string officeCode, string department )
        private bool displayMembersData( DataGridView dgv, string officeCode, string department )
        {
            labelMessage.Text = "";
            dataGridView1.Rows.Clear();
            MembersData mdp = new MembersData();
            DataTable dt = mdp.SelectWorkReportMembersData( officeCode, department );
            //if( dt == null || dt.Rows.Count < 1 ) return;
            if( dt == null || dt.Rows.Count < 1 )
            {
                labelMessage.Text = "表示できるデータがありません。";
                return false;
            }

            //dataGridView1.Rows.Clear();
            if( dt.Rows.Count > 1 ) dataGridView1.Rows.Add( dt.Rows.Count - 1 );

            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                dgv.Rows[i].Cells["MemberCode"].Value = Convert.ToString( dr["MemberCode"] );


                string wkMCode = Convert.ToString( dr["MemberCode"] );
                string wkMName = Convert.ToString( dr["Name"] );
                if( string.IsNullOrEmpty( wkMCode) ) MessageBox.Show( wkMName );


                dgv.Rows[i].Cells["MemberName"].Value = Convert.ToString( dr["Name"] );
            }
            return true;
        }


        private void readyDataArray()
        {
            int daysInMonth = DateTime.DaysInMonth( procDate.Year, procDate.Month );
            dayArray = new DateTime[daysInMonth];
            dowArray = new int[daysInMonth];
            hdayArray = new int[daysInMonth];
            workHArray = new decimal[dataGridView1.RowCount, daysInMonth];
            overHArray = new decimal[dataGridView1.RowCount, daysInMonth];
            checkArray = new int[dataGridView1.RowCount, daysInMonth];
            CalendarData cap = new CalendarData();
            procDate.BeginOfMonth();
            for( int i = 0; i < daysInMonth; i++ )
            {
                dayArray[i] = procDate.AddDays( i );
                dowArray[i] = ( int )dayArray[i].DayOfWeek;
                hdayArray[i] = cap.ExitstenceHoliday( dayArray[i] ) ? 1 : 0;
            }

            WorkReportData wrp = new WorkReportData();
            CostData cdp = new CostData();
            for( int i = 0; i < dataGridView1.Rows.Count; i++ )
            {
                string wkMCode = Convert.ToString( dataGridView1.Rows[i].Cells["MemberCode"].Value );
                cdp = new CostData();
                cdp = cdp.SelectCostMaster( Convert.ToString( dataGridView1.Rows[i].Cells["MemberCode"].Value ), Convert.ToString( comboBoxOffice.SelectedValue ), "A" );

                decimal[] workHour;
                for( int j = 0; j < dayArray.Length; j++ )
                {
                    workHour = wrp.SelectSummaryWorkReport( Convert.ToString( dataGridView1.Rows[i].Cells["MemberCode"].Value ), dayArray[j] );
                    workHArray[i, j] = workHour[0];
                    overHArray[i, j] = workHour[1];
                    checkArray[i, j] = ( int )workHour[2];

                    if( cdp != null && cdp.Unit == "時間" )
                    {
                        if( workHour[0] > 0 ) workHArray[i, j] = workHour[0] / 8;
                    }
                }
            }

            curNo = 0;
            displayWorkingData( dataGridView1, curNo );
        }


        private void displayWorkingData( DataGridView dgv, int curNo )
        {
            int warning = 0;
            string lineStr = "";
            for( int i = 0; i < colNum; i++ )
            {
                dgv.Columns[2 + i].HeaderText = dayArray[curNo + i].ToString( "yyyy/MM/dd" ) + "(" + Conv.dowList[dowArray[curNo + i]] + ")";
                dgv.Columns[2 + i].HeaderCell.Style.ForeColor = ( hdayArray[curNo + i] == 1 ) ? Color.Red : Color.Black;

                for( int j = 0; j < dgv.RowCount; j++ )
                {
                    if( hdayArray[curNo + i] == 1 )
                    {
                        if( overHArray[j, curNo + i] != -1 )
                        {
                            lineStr = "休日出勤";
                            if( workHArray[j, curNo + i] > 0 )
                                lineStr += "\r\n× 所定内：" + Convert.ToString( workHArray[j, curNo + i] ) + " 日 \r\n所定内入力不可";
                            lineStr += "\r\n休日出勤作業：" + Convert.ToString( overHArray[j, curNo + i] ) + " 時間";
                            /*
                            if (overHArray[j,curNo + i] > 8)
                            {
                                lineStr += "\r\n△ 残業：" + Convert.ToString(overHArray[j, curNo + i]) + " 時間 \r\n残業8hオーバー";
                            }
                            else
                            {
                                lineStr += "\r\n残業：" + Convert.ToString(overHArray[j, curNo + i]) + " 時間";
                            }
                            */
                        }
                    }
                    else
                    {
                        if( workHArray[j, curNo + i] == -1 && overHArray[j, curNo + i] == -1 )
                        {
                            //lineStr = "未入力/有給休暇";
                            lineStr += "未入力";
                            warning++;
                        }
                        else
                        {
                            if( workHArray[j, curNo + i] < 1 )
                            {
                                lineStr += "× 所定内：" + Convert.ToString( workHArray[j, curNo + i] ) + " 日 \r\n所定内不足";
                                warning++;
                            }
                            if( workHArray[j, curNo + i] > 1 )
                            {
                                lineStr += "× 所定内：" + Convert.ToString( workHArray[j, curNo + i] ) + " 日 \r\n所定内超過";
                                warning++;
                            }
                            if( workHArray[j, curNo + i] == 1 )
                                lineStr += "所定内：" + Convert.ToString( workHArray[j, curNo + i] ) + " 日";
                            //if (overHArray[j, curNo + i] > 0)
                            //    lineStr += "\r\n△ 残業：" + Convert.ToString(overHArray[j, curNo + i]) + " 時間";
                        }
                    }

                    if( checkArray[j, curNo + i] > 0 )
                    {
                        lineStr += "\r\n<< 要変更 >>";
                        warning++;
                    }

                    dgv.Rows[j].Cells[2 + i].Value = lineStr;
                    dgv[2 + i, j].Style.ForeColor = ( warning > 0 ) ? Color.OrangeRed : Color.Black;

                    lineStr = "";
                    warning = 0;
                }
            }
        }





    }
}

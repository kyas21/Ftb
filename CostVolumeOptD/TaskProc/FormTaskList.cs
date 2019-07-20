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

namespace TaskProc
{
    public partial class FormTaskList :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;
        TaskIndData[] tida;
        TaskList[] tla;

        private bool iniPro = true;
        private int iniRCnt = 28;
        const string bookName = "業務一覧表.xlsx";
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormTaskList()
        {
            InitializeComponent();
        }
        public FormTaskList( HumanProperty hp )
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
        private void FormTaskList_Load( object sender, EventArgs e )
        {
            create_cbOffice();
            comboBoxOffice.Text = hp.OfficeName;        // 初期値
            create_cbDepart();

            dataGridView1.Rows.Add( iniRCnt );
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvReadyNoRHeader();

            // 20190318 asakawa edit
            // if( hp.OfficeCode == "H" && hp.Department == "0" ) hp.Department = "2";
            // displayTaskData( dataGridView1, hp.OfficeCode, hp.Department );
            if (hp.OfficeCode == "H" && hp.Department == "0")
                displayTaskData(dataGridView1, hp.OfficeCode, "1");
            else
                displayTaskData(dataGridView1, hp.OfficeCode, hp.Department);
            // 20190318 end

        }


        private void FormTaskList_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonEnd":
                    this.Close();
                    break;
                case "buttonPrint":
                    // Wakamatsu 20170301
                    //tla = collectTaskListData(dataGridView1);
                    //PublishTaskList poc = new PublishTaskList(Folder.DefaultExcelTemplate("TaskList.xlsx"), collectTaskListData(dataGridView1));
                    PublishTaskList poc = new PublishTaskList( Folder.DefaultExcelTemplate( bookName ), collectTaskListData( dataGridView1 ) );
                    // Wakamatsu 20170301
                    poc.ExcelFile();
                    break;
                default:
                    break;
            }
        }


        //private void comboBox_TextChanged(object sender, EventArgs e)
        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            ComboBox cbx = ( ComboBox )sender;
            switch( cbx.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    break;
                case "comboBoxDepart":
                    break;
                default:
                    break;
            }

            displayTaskData( dataGridView1, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
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
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
        }

        private bool displayTaskData( DataGridView dgv, string officeCode, string department )
        {
            dgv.Rows.Clear();
            TaskIndData tid = new TaskIndData();
            tida = tid.SelectTaskIndData( officeCode, department );
            if( tida == null ) return false;
            int addRCnt = ( tida.GetLength( 0 ) > iniRCnt ) ? tida.GetLength( 0 ) - 1 : iniRCnt;
            dgv.Rows.Add( addRCnt );
            MembersData md = new MembersData();
            TaskData td = new TaskData();
            PartnersData pd = new PartnersData();
            //for( int i = 0; i < tida.GetLength( 0 ); i++ )
            for( int i = 0; i < tida.Length; i++ )
            {
                dgv.Rows[i].Cells["TaskCode"].Value = tida[i].TaskCode;
                dgv.Rows[i].Cells["TaskName"].Value = tida[i].TaskName;
                dgv.Rows[i].Cells["Cost"].Value = decFormat( tida[i].Contract );
                dgv.Rows[i].Cells["LeaderM"].Value = md.SelectMemberName( tida[i].LeaderMCode );
                td = td.SelectTaskData( tida[i].TaskID );
                // Wakamatsu 20170315
                //if (td == null) return false;
                if( td != null )
                {
                    dgv.Rows[i].Cells["DateFR"].Value = td.StartDate.ToString( "yyyy/MM/dd" );
                    dgv.Rows[i].Cells["DateTO"].Value = td.EndDate.ToString( "yyyy/MM/dd" );
                    dgv.Rows[i].Cells["IssueDate"].Value = td.IssueDate.ToString( "yyyy/MM/dd" );
                    dgv.Rows[i].Cells["SalesM"].Value = md.SelectMemberName( td.SalesMCode );
                    dgv.Rows[i].Cells["Customer"].Value = pd.SelectPartnerName( td.PartnerCode );
                }
            }

            // Wakamatsu 20170315
            this.dataGridView1.CurrentCell = null;
            return true;
        }


        private TaskList[] collectTaskListData( DataGridView dgv )
        {
            // Wakamatsu 20170315
            //tla = new TaskList[dgv.Rows.Count];
            tla = new TaskList[tida.Length];
            //for (int i = 0; i < dgv.Rows.Count; i++)
            for( int i = 0; i < tla.Length; i++ )
            // Wakamatsu 20170315
            {
                tla[i] = new TaskList();
                tla[i].OfficeName = comboBoxOffice.Text;
                tla[i].DepartName = comboBoxDepart.Text;
                tla[i].TaskCode = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                tla[i].TaskName = Convert.ToString( dgv.Rows[i].Cells["TaskName"].Value );
                tla[i].PartnerName = Convert.ToString( dgv.Rows[i].Cells["Customer"].Value );
                tla[i].Contract = Convert.ToDecimal( dgv.Rows[i].Cells["Cost"].Value );
                tla[i].StartDate = Convert.ToString( dgv.Rows[i].Cells["DateFR"].Value );
                tla[i].EndDate = Convert.ToString( dgv.Rows[i].Cells["DateTO"].Value );
                tla[i].SalesM = Convert.ToString( dgv.Rows[i].Cells["SalesM"].Value );
                tla[i].LeaderM = Convert.ToString( dgv.Rows[i].Cells["LeaderM"].Value );
                tla[i].IssueDate = Convert.ToString( dgv.Rows[i].Cells["IssueDate"].Value );
            }
            return tla;
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

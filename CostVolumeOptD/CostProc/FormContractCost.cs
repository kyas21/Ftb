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
    public partial class FormContractCost :Form
    {
        //-----------------------------------------------//
        //      Field
        //-----------------------------------------------//
        HumanProperty hp;
        TaskCodeNameData[] tcd;
        CostReportData[] crda;
        ListFormDataOp lo;
        private bool iniPro = true;
        //-----------------------------------------------//
        //      Construction
        //-----------------------------------------------//
        public FormContractCost()
        {
            InitializeComponent();
        }

        public FormContractCost( HumanProperty hp )
        {
            this.hp = hp;
            InitializeComponent();
        }

        //-----------------------------------------------//
        //      Property
        //-----------------------------------------------//

        //-----------------------------------------------//
        //      Method
        //-----------------------------------------------//
        private void FormContractCost_Load( object sender, EventArgs e )
        {
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvReadyNoRHeader();
            ui.DgvColumnsAlignmentRight( 3, 29 );
            create_cbFY();
            create_cbOffice();
            create_cbDepart();

            labelTaskName.Text = "";
            labelPartner.Text = "";

            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null, "CONTRACT" );    // Task情報
        }


        private void FormContractCost_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonClose":
                    this.Close();
                    break;

                case "buttonPrint":
                    PublishData pd = new PublishData();
                    pd.vYear = comboBoxFY.Text;
                    //pd.OfficeName = comboBoxOffice.Text;
                    pd.OfficeName = Conv.Office;
                    //pd.DepartName = comboBoxDepart.Text;
                    pd.DepartName = Conv.Depart;
                    pd.TaskCode = textBoxTaskCode.Text;
                    pd.TaskName = labelTaskName.Text;
                    pd.PartnerName = labelPartner.Text;
                    PublishContractWorks pub = new PublishContractWorks( Folder.DefaultExcelTemplate( "労働保険料元請業務作業実績.xlsx" ), pd, dataGridView1 );
                    pub.ExcelFile( "ContractWorks", pd, dataGridView1 );
                    break;

                default:
                    break;
            }
        }


        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            TextBox tb = ( TextBox )sender;

            if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            {
                if( tb.Name == "textBoxTaskCode" )
                {
                    TaskIndData tidp = new TaskIndData();

                    tidp = tidp.SelectTaskIndSData( " WHERE TaskCode = '" + textBoxTaskCode.Text + "' AND OrdersType = 1" );
                    if( tidp == null )
                    {
                        allClear();
                        return;
                    }
                    createView( tidp.TaskName, tidp.TaskID );
                }
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if( e.KeyCode == Keys.A )
            {
                if( tb.Name == "textBoxTaskCode" )
                {
                    allClear();
                    TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
                    if( tcds == null ) return;
                    textBoxTaskCode.Text = tcds.TaskCode;
                    createView( tcds.TaskName, tcds.TaskID );
                }
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            ComboBox cbx = ( ComboBox )sender;

            allClear();

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

            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null, "CONTRACT" );
        }
        //-----------------------------------------------//
        //      Subroutin
        //-----------------------------------------------//
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
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            comboBoxDepart.SelectedValue = ( comboBoxOffice.Text == Sign.HQOffice ) ? "2" : "8";
        }


        private void create_cbFY()
        {
            DateTime dtNow = DateTime.Now;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxFY );
            cbe.ValueItem = new string[3];
            cbe.DisplayItem = new string[3];
            for( int i = 0; i < cbe.ValueItem.Length; i++ )
            {
                cbe.ValueItem[i] = i.ToString();
                cbe.DisplayItem[i] = ( dtNow.FiscalYear() - i ).ToString();
            }
            cbe.Basic();
        }


        private void createView( string taskName, int taskID )
        {
            labelTaskName.Text = taskName;
            dispPartnerName( taskID );
            dispMembersData( textBoxTaskCode.Text, dataGridView1 );
        }


        private void dispPartnerName( int taskID )
        {
            labelPartner.Text = "";
            TaskData td = new TaskData();
            td = td.SelectTaskData( taskID );
            if( td == null ) return;
            PartnersData pd = new PartnersData();
            labelPartner.Text = pd.SelectPartnerName( td.PartnerCode );
        }


        private void dispMembersData( string taskCode, DataGridView dgv )
        {
            CostReportData crdp = new CostReportData();
            crda = crdp.SelectCostReportItemCode( Conv.OfficeCode, taskCode, Convert.ToInt32( comboBoxFY.Text ) );
            if( crda == null ) return;

            if( crda.Length > 1 ) dgv.Rows.Add( crda.Length - 1 );
            for( int i = 0; i < crda.Length; i++ )
            {
                dgv.Rows[i].Cells["MName"].Value = crda[i].ItemCode.TrimEnd() + " " + crda[i].Item.TrimEnd();
                editDgvRow( dgv.Rows[i], crda[i].ItemCode.TrimEnd() );
            }

        }


        private void editDgvRow( DataGridViewRow dgvRow, string itemCode )
        {
            CostReportData crdp = new CostReportData();
            CostData cd = new CostData();
            int[] monthArray = new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12 , 1, 2, 3 };
            int year = Convert.ToInt32( comboBoxFY.Text );
            decimal quantity = 0M;
            decimal sumQ = 0M;

            cd = cd.SelectCostMaster( itemCode, Conv.OfficeCode );
            dgvRow.Cells["MCode"].Value = cd.MemberCode;
            dgvRow.Cells["Unit"].Value = cd.Unit;
            dgvRow.Cells["Price"].Value = decFormat( cd.Cost );

            for( int i = 0; i < 12; i++ )
            {
                quantity = crdp.SumMonthlyQuantity( Conv.OfficeCode, itemCode, DateTime.ParseExact( Convert.ToString( year * 10000 + monthArray[i] * 100 + 1 ), "yyyyMMdd", null ) );
                dgvRow.Cells["Work" + i.ToString( "00" )].Value = quantity;
                sumQ += quantity;
                dgvRow.Cells["Cost" + i.ToString( "00" )].Value = decFormat( quantity * cd.Cost );
            }

            dgvRow.Cells["WorkSum"].Value = sumQ;
            dgvRow.Cells["CostSum"].Value = decFormat( sumQ * cd.Cost );
            sumQ = 0;
        }


        private void allClear()
        {
            textBoxTaskCode.Text = "";
            labelTaskName.Text = "";
            labelPartner.Text = "";
            dataGridView1.Rows.Clear();
        }


        private string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }

    
    }
}

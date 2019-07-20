using ClassLibrary;
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

namespace TaskProc
{
    public partial class FormTaskNoConfList :Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        HumanProperty hp;
        TaskData[] tdA;
        TaskNoConfList[] tncA;
        private bool iniPro = true;
        private int rowCount = 0;
        const string bookName = "業務引継書承認未完了一覧表.xlsx";
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormTaskNoConfList()
        {
            InitializeComponent();
        }
        public FormTaskNoConfList( HumanProperty hp )
        {
            this.hp = hp;
            InitializeComponent();
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormTaskNoConfList_Load( object sender, EventArgs e )
        {
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();

            create_cbOffice();
            create_cbDepart();
            labelMsg.Text = "";

            if( dataCollection() )
            {
                editAndView( dataGridView1, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
            }
            else
            {
                MessageBox.Show( "表示できるデータはありません" );
                return;
            }
        }

        private void FormTaskNoConfList_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            EstPlanOp epo = new EstPlanOp();

            switch( btn.Name )
            {
                case "buttonPrint":
                    PublishTaskNoConfList poc = new PublishTaskNoConfList( Folder.DefaultExcelTemplate( bookName ), collectTaskNoConfListData( dataGridView1 ) );
                    poc.ExcelFile();
                    break;
                case "buttonClose":
                    this.Close();
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
                    create_cbDepart();
                    break;
                case "comboBoxDepart":
                    break;
                default:
                    break;
            }
            dataGridView1.Rows.Clear();
            editAndView( dataGridView1, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
        }
        //---------------------------------------------------------------------//
        // Subroutine                                                          //
        //---------------------------------------------------------------------//
        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
            comboBoxOffice.SelectedValue = hp.OfficeCode;
        }


        // 部門
        private void create_cbDepart()
        {
            //comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );

            if( hp.OfficeCode == Sign.HQOfficeCode )
            {
                comboBoxDepart.SelectedValue = ( hp.Department == "0" ) ? "2" : hp.Department;
            }
            else
            {
                comboBoxDepart.SelectedValue = "8";
            }
        }


        private bool dataCollection()
        {
            string para = " WHERE OldVerMark = 0 AND IssueMark = 1";
            //string para = " WHERE OldVerMark = 0 AND IssueMark = 0";

            TaskData td = new TaskData();
            tdA = td.SelectTaskDataByPara( para );
            if( tdA == null ) return false;
            return true;
        }


        private void editAndView( DataGridView dgv, string officeCode, string department )
        {

            StringUtility su = new StringUtility();
            TaskData[] tdWkA = new TaskData[tdA.Length];
            
            MembersData mbd = new MembersData();
            int j = 0;
            for( int i = 0; i < tdA.Length; i++ )
            {
                //mbd = new MembersData();
                //mbd = mbd.SelectMembersDataAll( tdA[i].SalesMCode );
                //if( mbd == null ) continue;
                //if( mbd.OfficeCode == officeCode && mbd.Department == department )
                //{
                //    tdWkA[j] = new TaskData();
                //    tdWkA[j] = ( TaskData )tdA[i].Clone();
                //    j++;
                //}
                string wkoffice = su.SubstringByte( tdA[i].TaskBaseCode, 2, 1 );
                if(su.SubstringByte(tdA[i].TaskBaseCode,2,1) == officeCode )
                {
                    tdWkA[j] = new TaskData();
                    tdWkA[j] = ( TaskData )tdA[i].Clone();
                    j++;
                }
            }

            TaskData[] tdArray = new TaskData[j];
            for( int i = 0; i < j; i++ )
            {
                tdArray[i] = new TaskData();
                tdArray[i] = ( TaskData )tdWkA[i].Clone();
            }


            MembersData[] mbdA = mbd.SelectMembersDataAll();
            if( mbdA == null ) return;
            string[] mCodeArray = new string[mbdA.Length];
            string[] mNameArray = new string[mbdA.Length];
            for( int i = 0; i < mbdA.Length; i++ )
            {
                mCodeArray[i] = mbdA[i].MemberCode.TrimEnd();
                mNameArray[i] = mbdA[i].Name.TrimEnd();
            }

            UiHandling uih = new UiHandling( dgv );
            uih.DgvRowsHeight( 26 );
            dataGridView1.Rows.Add( tdArray.Length );

            for( int i = 0; i < tdArray.Length; i++ )
            {
                dgv.Rows[i].Cells["SeqNo"].Value = i + 1;
                dgv.Rows[i].Cells["TaskCode"].Value = selTaskCode( tdArray[i], officeCode, department );
                dgv.Rows[i].Cells["TaskName"].Value = tdArray[i].TaskName;
                dgv.Rows[i].Cells["VersionNo"].Value = tdArray[i].VersionNo;
                dgv.Rows[i].Cells["IssueDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( tdArray[i].IssueDate ) ) ) ? "" : tdArray[i].IssueDate.ToShortDateString();

                if( tdArray[i].SalesMCode.Trim() == "00" ) tdArray[i].SalesMCode = null;
                dgv.Rows[i].Cells["SalesMName"].Value = ( string.IsNullOrEmpty( tdArray[i].SalesMCode ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, tdArray[i].SalesMCode.TrimEnd() )];
                dgv.Rows[i].Cells["SalesMInputDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( tdArray[i].SalesMInputDate ) ) ) ? "" : tdArray[i].SalesMInputDate.ToShortDateString();
                
                dgv.Rows[i].Cells["Approval"].Value = ( string.IsNullOrEmpty( tdArray[i].Approval ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, tdArray[i].Approval.TrimEnd() )];
                dgv.Rows[i].Cells["ApprovalDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( tdArray[i].ApprovalDate ) ) ) ? "" : tdArray[i].ApprovalDate.ToShortDateString();
                
                dgv.Rows[i].Cells["MakeOrder"].Value = ( string.IsNullOrEmpty( tdArray[i].Approval ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, tdArray[i].MakeOrder.TrimEnd() )];
                dgv.Rows[i].Cells["MakeOrderDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( tdArray[i].MakeOrderDate ) ) ) ? "" : tdArray[i].MakeOrderDate.ToShortDateString();
                
                dgv.Rows[i].Cells["ConfirmAdm"].Value = ( string.IsNullOrEmpty( tdArray[i].MakeOrder ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, tdArray[i].ConfirmAdm.TrimEnd() )];
                dgv.Rows[i].Cells["ConfirmDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( tdArray[i].ConfirmDate ) ) ) ? "" : tdArray[i].ConfirmDate.ToShortDateString();
            }
            rowCount = tdArray.Length;

            labelMsg.Text = comboBoxOffice.Text + "発行の業務引継書で承認が未完了のものを表示しました。";
        }


        private string selTaskCode( TaskData td, string officeCode, string department )
        {
            TaskIndData tid = new TaskIndData();
            TaskIndData[] tidA = tid.SelectTaskIndDataByBaseCode( td.TaskBaseCode );
            if( tidA == null ) return td.TaskBaseCode;
            for( int i = 0; i < tidA.Length; i++ )
            {
                //if( tidA[i].OfficeCode == officeCode && tidA[i].Department == department )
                if( tidA[i].OfficeCode == officeCode )
                {
                    return tidA[i].TaskCode;
                }
            }
            return tidA[0].TaskCode;
        }


        private TaskNoConfList[] collectTaskNoConfListData( DataGridView dgv )
        {
            tncA = new TaskNoConfList[rowCount];
            for( int i = 0; i < rowCount; i++ )
            {
                tncA[i] = new TaskNoConfList();
                tncA[i].OfficeName = comboBoxOffice.Text;
                tncA[i].DepartName = comboBoxDepart.Text;
                tncA[i].TaskCode = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                tncA[i].TaskName = Convert.ToString( dgv.Rows[i].Cells["TaskName"].Value );
                tncA[i].VersionNo = Convert.ToInt32( dgv.Rows[i].Cells["VersionNo"].Value );
                tncA[i].IssueDate = Convert.ToString( dgv.Rows[i].Cells["IssueDate"].Value );
                tncA[i].SalesMName = Convert.ToString( dgv.Rows[i].Cells["SalesMName"].Value );
                tncA[i].SalesMInputDate = Convert.ToString( dgv.Rows[i].Cells["SalesMInputDate"].Value );
                tncA[i].Approval = Convert.ToString( dgv.Rows[i].Cells["Approval"].Value );
                tncA[i].ApprovalDate = Convert.ToString( dgv.Rows[i].Cells["ApprovalDate"].Value );
                tncA[i].MakeOrder = Convert.ToString( dgv.Rows[i].Cells["MakeOrder"].Value );
                tncA[i].MakeOrderDate = Convert.ToString( dgv.Rows[i].Cells["MakeOrderDate"].Value );
                tncA[i].ConfirmAdm = Convert.ToString( dgv.Rows[i].Cells["ConfirmAdm"].Value );
                tncA[i].ConfirmDate = Convert.ToString( dgv.Rows[i].Cells["ConfirmDate"].Value );
            }
            return tncA;
        }
    }
}

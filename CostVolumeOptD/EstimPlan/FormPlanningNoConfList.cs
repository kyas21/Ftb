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

namespace EstimPlan
{
    public partial class FormPlanningNoConfList :Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        HumanProperty hp;
        TaskEntryData[] tedA;
        PlanningData[] pdA;
        PlanningNoConfList[] pncA;
        private bool iniPro = true;
        private int rowCount = 0;
        const string bookName = "実行予算書承認未完了一覧表.xlsx";
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormPlanningNoConfList()
        {
            InitializeComponent();
        }
        public FormPlanningNoConfList( HumanProperty hp )
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
        private void FormPlanningNoConfList_Load( object sender, EventArgs e )
        {
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();

            create_cbOffice();
            create_cbDepart();
            labelMsg.Text = "";

            if( dataCollection( Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) ) )
            {
                editAndView( dataGridView1 );
            }
            else
            {
                MessageBox.Show( "表示できるデータはありません" );
                return;
            }
        }

        private void FormPlanningNoConfList_Shown( object sender, EventArgs e )
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
                    PublishPlanningNoConfList poc = new PublishPlanningNoConfList( Folder.DefaultExcelTemplate( bookName ), collectPlanningNoConfListData( dataGridView1 ) );
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
            dataCollection( Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
            editAndView( dataGridView1 );
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
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

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


        private bool dataCollection( string officeCode, string department )
        {
            string para = " WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "'";
            TaskEntryData ted = new TaskEntryData();
            tedA = ted.SelectTaskEntryData( para );
            if( tedA == null ) return false;

            PlanningData[] wkPdA = new PlanningData[tedA.Length];
            PlanningData pd = new PlanningData();
            int j = 0;
            for( int i = 0; i < tedA.Length; i++ )
            {
                pd = pd.LatestPlanningData( tedA[i].TaskEntryID );
                if( pd == null ) continue;
                if( pd.ApPresidentStat == 0 )
                {
                    wkPdA[j] = new PlanningData();
                    wkPdA[j] = ( PlanningData )pd.Clone();
                    wkPdA[j].TaskCode = tedA[i].TaskCode;
                    wkPdA[j].TaskName = tedA[i].TaskName;
                    j++;
                }
            }

            if( j == 0 ) return false;

            pdA = new PlanningData[j];
            for( int i = 0; i < pdA.Length; i++ )
            {
                pdA[i] = new PlanningData();
                pdA[i] = ( PlanningData )wkPdA[i].Clone();
                pdA[i].TaskCode = wkPdA[i].TaskCode;
                pdA[i].TaskName = wkPdA[i].TaskName;
            }
            return true;
        }


        private void editAndView( DataGridView dgv )
        {
            // 社員番号、名テーブル作成
            MembersData mbd = new MembersData();
            MembersData[] mbdA = mbd.SelectMembersDataAll();
            if( mbdA == null ) return;
            string[] mCodeArray = new string[mbdA.Length];
            string[] mNameArray = new string[mbdA.Length];
            for( int i = 0; i < mbdA.Length; i++ )
            {
                mCodeArray[i] = mbdA[i].MemberCode.TrimEnd();
                mNameArray[i] = mbdA[i].Name.TrimEnd();
            }
            // GridView Row設定
            UiHandling uih = new UiHandling( dgv );
            uih.DgvRowsHeight( 26 );
            dataGridView1.Rows.Add( pdA.Length );
            // GirdViewデータ移送
            for( int i = 0; i < pdA.Length; i++ )
            {
                dgv.Rows[i].Cells["SeqNo"].Value = i + 1;
                dgv.Rows[i].Cells["TaskCode"].Value = ( string.IsNullOrEmpty( pdA[i].TaskCode ) ) ? "" : pdA[i].TaskCode;
                dgv.Rows[i].Cells["TaskName"].Value = pdA[i].TaskName;
                dgv.Rows[i].Cells["VersionNo"].Value = pdA[i].VersionNo;
                if( pdA[i].CreateStat == 1 )
                {
                    dgv.Rows[i].Cells["CreateMName"].Value = ( string.IsNullOrEmpty( pdA[i].CreateMCd ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, pdA[i].CreateMCd.TrimEnd() )];
                    dgv.Rows[i].Cells["CreateDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( pdA[i].CreateDate ) ) ) ? "" : pdA[i].CreateDate.ToShortDateString();
                }

                if( pdA[i].ConfirmStat == 1 )
                {
                    dgv.Rows[i].Cells["ConfirmMName"].Value = ( string.IsNullOrEmpty( pdA[i].ConfirmMCd ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, pdA[i].ConfirmMCd.TrimEnd() )];
                    dgv.Rows[i].Cells["ConfirmDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( pdA[i].ConfirmDate ) ) ) ? "" : pdA[i].ConfirmDate.ToShortDateString();
                }

                if( pdA[i].ScreeningStat == 1 )
                {
                    dgv.Rows[i].Cells["ScreeningMName"].Value = ( string.IsNullOrEmpty( pdA[i].ScreeningMCd ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, pdA[i].ScreeningMCd.TrimEnd() )];
                    dgv.Rows[i].Cells["ScreeningDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( pdA[i].ScreeningDate ) ) ) ? "" : pdA[i].ScreeningDate.ToShortDateString();
                }

                if( pdA[i].ApOfficerStat == 1 )
                {
                    dgv.Rows[i].Cells["ApOfficerMName"].Value = ( string.IsNullOrEmpty( pdA[i].ApOfficerMCd ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, pdA[i].ApOfficerMCd.TrimEnd() )];
                    dgv.Rows[i].Cells["ApOfficerDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( pdA[i].ApOfficerDate ) ) ) ? "" : pdA[i].ApOfficerDate.ToShortDateString();
                }

                if( pdA[i].ApPresidentStat == 1 )
                {
                    dgv.Rows[i].Cells["ApPresidentMName"].Value = ( string.IsNullOrEmpty( pdA[i].ApPresidentMCd ) ) ? "" : mNameArray[Array.IndexOf( mCodeArray, pdA[i].ApPresidentMCd.TrimEnd() )];
                    dgv.Rows[i].Cells["ApPresidentDate"].Value = ( string.IsNullOrEmpty( Convert.ToString( pdA[i].ApPresidentDate ) ) ) ? "" : pdA[i].ApPresidentDate.ToShortDateString();
                }

            }

            labelMsg.Text = comboBoxOffice.Text + "発行の実行予算書で承認が未完了のものを表示しました。";
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


        private PlanningNoConfList[] collectPlanningNoConfListData( DataGridView dgv )
        {
            pncA = new PlanningNoConfList[pdA.Length];
            for( int i = 0; i < pdA.Length; i++ )
            {
                pncA[i] = new PlanningNoConfList();
                pncA[i].OfficeName = comboBoxOffice.Text;
                pncA[i].DepartName = comboBoxDepart.Text;
                pncA[i].TaskCode = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                pncA[i].TaskName = Convert.ToString( dgv.Rows[i].Cells["TaskName"].Value );
                pncA[i].VersionNo = Convert.ToInt32( dgv.Rows[i].Cells["VersionNo"].Value );
                pncA[i].CreateMName = Convert.ToString( dgv.Rows[i].Cells["CreateMName"].Value );
                pncA[i].CreateDate = Convert.ToString( dgv.Rows[i].Cells["CreateDate"].Value );
                pncA[i].ConfirmMName = Convert.ToString( dgv.Rows[i].Cells["ConfirmMName"].Value );
                pncA[i].ConfirmDate = Convert.ToString( dgv.Rows[i].Cells["ConfirmDate"].Value );
                pncA[i].ScreeningMName = Convert.ToString( dgv.Rows[i].Cells["ScreeningMName"].Value );
                pncA[i].ScreeningDate = Convert.ToString( dgv.Rows[i].Cells["ScreeningDate"].Value );
                pncA[i].ApOfficerMName = Convert.ToString( dgv.Rows[i].Cells["ApOfficerMName"].Value );
                pncA[i].ApOfficerDate = Convert.ToString( dgv.Rows[i].Cells["ApOfficerDate"].Value );
                pncA[i].ApPresidentMName = Convert.ToString( dgv.Rows[i].Cells["ApPresidentMName"].Value );
                pncA[i].ApPresidentDate = Convert.ToString( dgv.Rows[i].Cells["ApPresidentDate"].Value );
            }
            return pncA;
        }














    }
}

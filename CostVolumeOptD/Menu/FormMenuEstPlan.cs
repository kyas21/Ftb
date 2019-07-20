using System;
using System.Windows.Forms;
using System.Data;
using ClassLibrary;
using Accounts;
using EstimPlan;
using Maintenance;
using ListForm;

namespace Menu
{
    public partial class FormMenuEstPlan : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private bool iniPro = true;
        FormEstimate formEstimate = null;
        FormPlanning formPlanning = null;
        FormOutsource formOutsource = null;
        FormContract formContract = null;
        FormRegular formRegular = null;
        FormVolumeInvoice formVolumeInvoice = null;
        FormInvoice formInvoice = null;
        FormImpMWorkItems formImpMWorkItems = null;
        HumanProperty hp;
        TaskCodeNameData[] tcd;
        TaskCodeNameData[] etcd;
        TaskCodeNameData tcds;
        TaskEntryData ted;

        private string msgAlready = "すでにこのプログラムは開始されています！";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuEstPlan()
        {
            InitializeComponent();
        }

        public FormMenuEstPlan( HumanProperty hp )
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
        private void FormMenuEstPlan_Load( object sender, EventArgs e )
        {
            create_cbOffice();              // ComboBox 「事業所」作成
            create_cbDepart();              // ComboBox 「部門」作成
            create_cbCostType();            // ComboBox 「原価目標」作成
            clearLabel();

            // 各ボタンに現在の最新版数表示
            //setButtonLabel(Convert.ToString(comboBoxTaskName.SelectedValue), publisher);

            //ted = new TaskEntryData();
            //ted.OfficeCode = hp.OfficeCode;
            //ted.Department = hp.Department;
            //ted.OfficeName = hp.OfficeName;
            //ted.DepartName = hp.DepartName;
            //ted.MemberCode = hp.MemberCode;
            //ted.TaxRate = hp.TaxRate;
            //ted.AdminCostRate = hp.AdminCostRate;
            //ted.OthersCostRate = hp.OthersCostRate;
            //ted.Expenses = hp.Expenses;
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepartment );
            ListFormDataOp lo = new ListFormDataOp();
            //etcd = lo.SelectTaskEntryNameData( hp.OfficeCode, Convert.ToString( comboBoxDepartment.SelectedValue ), null );
            etcd = lo.SelectTaskEntryNameData( Conv.OfficeCode, Conv.DepartCode, null );
            //tcd = lo.SelectTaskCodeNameData( hp.OfficeCode, Convert.ToString( comboBoxDepartment.SelectedValue ), null );
            tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null );
        }



        private void FormMenuEstPlan_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            Button btn = ( Button )sender;

            if ( btn.Name == "buttonClose" )
            {
                this.Close();
                return;
            }

            if ( btn.Name == "buttonStoreMWorkItems" )
            {
                if ( formImpMWorkItems == null || formImpMWorkItems.IsDisposed )
                {
                    formImpMWorkItems = new FormImpMWorkItems( hp );
                    formImpMWorkItems.Show();
                }
                else
                {
                    MessageBox.Show( msgAlready );
                }
                return;
            }

            TaskEntryData itd = new TaskEntryData();

            if (btn.Name == "buttonAdd")
            {
                //itd.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
                //itd.Department = Convert.ToString( comboBoxDepartment.SelectedValue );
                //itd.OfficeName = comboBoxOffice.Text;
                //itd.DepartName = comboBoxDepartment.Text;

                //labelTaskEntryID.Text = "";
            }
            else
            {
                if ( labelTaskEntryID.Text == "" || labelTaskEntryID.Text == "0" )
                {
                    DMessage.DataNotEnough();
                    return;
                }
            
                itd = itd.SelectTaskEntryData( Convert.ToInt32( labelTaskEntryID.Text ) );

                itd.MemberCode = hp.MemberCode;
                itd.TaxRate = hp.TaxRate;
                itd.AdminCostRate = hp.AdminCostRate;
                itd.OthersCostRate = hp.OthersCostRate;
                itd.Expenses = hp.Expenses;

                itd.OfficeCode = Conv.OfficeCode;
                itd.Department = Conv.DepartCode;

                itd.PartnerName = labelPartner.Text;

            }

            switch ( btn.Name )
            {
                case "buttonEstimate":
                    formEstimate = new FormEstimate( itd );
                    formEstimate.Show();
                    break;

                case "buttonPlanning":
                    formPlanning = new FormPlanning( itd );
                    formPlanning.Show();
                    break;

                case "buttonOsDetail":
                    formOutsource = new FormOutsource( itd );
                    formOutsource.Show();
                    break;
                    
                case "buttonContract":
                    formContract = new FormContract( itd );
                    formContract.Show();
                    break;

                case "buttonRegular":
                    formRegular = new FormRegular( itd );
                    formRegular.Show();
                    break;

                case "buttonVolumeInvoice":
                    formVolumeInvoice = new FormVolumeInvoice( itd );
                    formVolumeInvoice.Show();
                    break;

                case "buttonInvoice":
                    formInvoice = new FormInvoice( itd );
                    formInvoice.Show();
                    break;

                //case "buttonDetail":
                //    displayDetailLabel(ted);
                //    break;

                case "buttonAdd":
                    itd.OfficeCode = Conv.OfficeCode;
                    itd.Department = Conv.DepartCode;
                    itd.OfficeName = Conv.Office;
                    itd.DepartName = Conv.Depart;

                    labelTaskEntryID.Text = "";





                    itd = FormTaskEntry.DispData( itd );
                    displayDetailLabel( itd );

                    comboBoxCostType.Text = itd.CostType;
                    textBoxTaskName.Text = itd.TaskName;
                    labelPartner.Text = itd.PartnerName;
                    labelTaskPlace.Text = itd.TaskPlace;
                    labelTaskCode.Text = itd.TaskCode;
                    labelTaskEntryID.Text = Convert.ToString(itd.TaskEntryID);
                    // ? 仮版 : 正式版
                    labelTaskName.Text = (labelTaskCode.Text.Trim() == "") ? "": itd.TaskName;

                    ListFormDataOp lo = new ListFormDataOp();
                    etcd = lo.SelectTaskEntryNameData( itd.OfficeCode, itd.Department, null );

                    buttonTask.Enabled = string.IsNullOrEmpty( labelTaskCode.Text.Trim() ) ? true : false;
                    buttonTask.Visible = string.IsNullOrEmpty( labelTaskCode.Text.Trim() ) ? true : false;

                    break;

                case "buttonTask":
                    if ( String.IsNullOrEmpty( textBoxTaskName.Text ) )
                    {
                        MessageBox.Show( "対応する見積・計画用業務名がありません" );
                        return;
                    }
                    tcds = FormTaskCodeNameList.ReceiveItems( tcd );
                    if ( !writeTaskCodeToTaskEntryData() ) return;
                    if ( !displayTaskData( tcds ) ) return;
                    break;

                default:
                    break;
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if ( iniPro ) return;

            ComboBox cb = ( ComboBox )sender;
            ted = new TaskEntryData();
            Conv.OfficeAndDepartZ(comboBoxOffice,comboBoxDepartment);
            ListFormDataOp lo = new ListFormDataOp();
            switch ( cb.Name )
            {
                case "comboBoxOffice":
                    ted.OfficeName = cb.Text;
                    create_cbDepart();

                    etcd = lo.SelectTaskEntryNameData( Conv.OfficeCode, Conv.DepartCode, null );
                    tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, "DESC" );
                    this.textBoxTaskName.Text = "";
                    clearLabel();
                    break;

                case "comboBoxDepartment":
                    ted.DepartName = cb.Text;
                    etcd = lo.SelectTaskEntryNameData( Conv.OfficeCode, Conv.DepartCode, null );
                    tcd = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, "DESC" );
                    this.textBoxTaskName.Text = "";
                    clearLabel();
                    break;

                case "comboBoxCostType":
                    ted.CostType = cb.Text;
                    clearLabel();
                    break;

                default:
                    break;
            }
        }


        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            TextBox tbx = ( TextBox )sender;
            if ( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch ( e.KeyCode )
            {
                case Keys.A:
                    if ( etcd == null )
                    {
                        MessageBox.Show( "Dataがありません。" );
                        return;
                    }
                    TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( etcd );
                    if ( tcds == null ) return;
                    textBoxTaskName.Text = tcds.TaskName;
                    labelTaskEntryID.Text = Convert.ToString( tcds.TaskID );      // TaskEntryIDはTaskIDに格納されている
                    if ( !displayTaskData( tcds ) ) return;
                    buttonTask.Enabled = string.IsNullOrEmpty(labelTaskCode.Text.Trim()) ? true : false;
                    buttonTask.Visible = string.IsNullOrEmpty(labelTaskCode.Text.Trim()) ? true : false;
                    break;
                default:
                    break;
            }
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


        private void create_cbDepart()
        {
            comboBoxDepartment.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepartment );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
           
            if(hp.OfficeCode == Sign.HQOfficeCode )
            {
                comboBoxDepartment.SelectedValue = ( hp.Department == "0" ) ? "2" : hp.Department;
            }
            else
            {
                comboBoxDepartment.SelectedValue = "8";
            } 
        }


        // 原価目標区分
        private void create_cbCostType()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxCostType );
            cbe.ComDataList( "COST" );
        }


        private void setButtonLabel( string taskEntryID, string publisher )
        {
            Button[] buttonArray = new Button[] { this.buttonEstimate, this.buttonPlanning, this.buttonOsDetail };
            string[] buttonLabelArray = new string[] { "見積書   ", "予算書   ", "外注内訳書   " };
            string[] selectTableArray = new string[] { "D_Estimate", "D_Planning", "D_Outsource" };

            /***** 各ボタンに現在の最新版数表示 *****/
            string verNo;
            string wParam;
            DataTable dt;
            SqlHandling sh;
            for ( int i = 0; i < buttonLabelArray.Length; i++ )
            {
                sh = new SqlHandling( selectTableArray[i] );
                wParam = " WHERE TaskEntryID = '" + taskEntryID
                        + "' AND OfficeCode = '" + Convert.ToString( comboBoxOffice.SelectedValue )
                        + "' AND Department = '" + Convert.ToString( comboBoxDepartment.SelectedValue ) + "'";
                dt = sh.SelectAllData( wParam );

                if ( dt == null )
                {
                    verNo = "(0)";
                }
                else
                {
                    DataRow dataRow = dt.Rows[0];
                    verNo = "(" + Convert.ToString( dataRow["VersionNo"] ) + ")";
                }

                buttonArray[i].Text = buttonLabelArray[i] + verNo;
            }
        }


        private void displayLabalContents( int entryID )
        {
            clearLabel();
            if ( entryID < 1 ) return;

            ted = new TaskEntryData();
            ted = ted.SelectTaskEntryData( entryID );
            if ( ted == null ) return;
            labelPartner.Text = ted.PartnerName;
            labelTaskPlace.Text = ted.TaskPlace;
            labelTaskCode.Text = ted.TaskCode;
        }


        private void displayDetailLabel(TaskEntryData ted)
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            string[] mCodeArray = new string[2];
            string[] mNameArray = new string[2];
            mCodeArray[0] = ted.LeaderMCode;
            mCodeArray[1] = ted.SalesMCode;
            MembersData md = new MembersData();

            for ( int i = 0; i < mCodeArray.Length; i++ )
            {
                if ( !String.IsNullOrEmpty( mCodeArray[i] ) )
                    mNameArray[i] = md.SelectMemberName( mCodeArray[i] );
            }

            clearDetailLabel();
            // 業務担当者
            if ( !String.IsNullOrEmpty( mNameArray[0] ) )
            {
                labelTtlLeader.Visible = true;
                labelLeaderName.Text = mNameArray[0];
            }
            // 営業担当者
            if ( !String.IsNullOrEmpty( mNameArray[1] ) )
            {
                labelTtlSales.Visible = true;
                labelSalesMName.Text = mNameArray[1];
            }
            // 契約日
            if ( DHandling.CheckDateMiniValue( ted.ContractDate ) )
                labelContractDate.Text = "契約日： " + DHandling.PickUpTopCharacters( Convert.ToString( ted.ContractDate ), 10 );
            // 工期　開始日
            if ( DHandling.CheckDateMiniValue( ted.StartDate ) )
                labelStartDate.Text = "工期： " + DHandling.PickUpTopCharacters( Convert.ToString( ted.StartDate ), 10 );
            // 工期　終了日
            if ( DHandling.CheckDateMiniValue( ted.EndDate ) )
                labelStartDate.Text += "  ～  " + DHandling.PickUpTopCharacters( Convert.ToString( ted.EndDate ), 10 );
        }


        private bool displayTaskData( TaskCodeNameData tcds )
        {
            if ( tcds == null ) return false;

            textBoxTaskName.Text = tcds.TaskName;
            if ( String.IsNullOrEmpty( tcds.TaskCode.Trim() ) )
            {
                // 仮業務番号で運用中
                TaskEntryData tid = new TaskEntryData();
                tid = tid.SelectTaskEntryData( tcds.TaskID );

                MembersData md = new MembersData();
                labelLeaderName.Text = "業務責任者： " + md.SelectMemberName( tid.LeaderMCode );
                labelTaskPlace.Text = tid.TaskPlace;
                labelSalesMName.Text = "営業責任者： " + md.SelectMemberName( tid.SalesMCode );
                if ( DHandling.CheckDateMiniValue( tid.StartDate ) )
                    labelStartDate.Text = "工期： " + tid.StartDate.ToString( "d" );
                if ( DHandling.CheckDateMiniValue( tid.EndDate ) )
                    labelStartDate.Text += "  ～  " + tid.EndDate.ToString( "d" );

                PartnersData pd = new PartnersData();
                labelPartner.Text = pd.SelectPartnerName( tid.PartnerCode );
                labelPartnerCode.Text = tid.PartnerCode;
            }
            else
            {
                // 正式な業務番号が設定されている
                labelTaskCode.Text = tcds.TaskCode;
                labelTtlTName.Visible = true;
                labelTaskName.Text = tcds.TaskName;
                TaskIndData tid = new TaskIndData();
                tid = tid.SelectTaskIndData( tcds.TaskCode );
                if ( tid == null ) return false;

                MembersData md = new MembersData();
                // 業務担当者名
                labelTtlLeader.Visible = true;
                labelLeaderName.Text = md.SelectMemberName( tid.LeaderMCode );

                TaskData td = new TaskData();
                td = td.SelectTaskData( tid.TaskID );
                if ( td == null ) return false;

                labelTaskPlace.Text = td.TaskPlace;
                // 営業担当者名
                labelTtlSales.Visible = true;
                labelSalesMName.Text = md.SelectMemberName( td.SalesMCode );
                labelContractDate.Text = "契約日： " + td.IssueDate.ToString( "yyyy/MM/dd" );
                if ( DHandling.CheckDateMiniValue( td.StartDate ) )
                    labelStartDate.Text = "工期： " + td.StartDate.ToString( "d" );
                if ( DHandling.CheckDateMiniValue( td.EndDate ) )
                    labelStartDate.Text += "  ～  " + td.EndDate.ToString( "d" );

                PartnersData pd = new PartnersData();
                labelPartner.Text = pd.SelectPartnerName( td.PartnerCode );
                labelPartnerCode.Text = td.PartnerCode;

                setTaskCodeToTaskEntryData( tid, td );
            }
            return true;
        }


        private void setTaskCodeToTaskEntryData( TaskIndData tid, TaskData td )
        {
            ted = new TaskEntryData();
            ted.TaskEntryID = string.IsNullOrEmpty( labelTaskEntryID.Text ) ? 0 : Convert.ToInt32( labelTaskEntryID.Text );

            ted.TaskCode = tid.TaskCode;
            ted.TaskName = tid.TaskName;
            ted.TaskPlace = td.TaskPlace ?? "";
            ted.CostType = "";
            ted.PartnerCode = td.PartnerCode;
            ted.OfficeCode = tid.OfficeCode;
            ted.Department = tid.Department;
            ted.LeaderMCode = tid.LeaderMCode;
            ted.SalesMCode = td.SalesMCode;
            ted.ContractDate = td.IssueDate;
            ted.StartDate = td.StartDate;
            ted.EndDate = td.EndDate;
            ted.TaskID = td.TaskID;
            ted.TaskIndID = tid.TaskIndID;
        }


        private bool writeTaskCodeToTaskEntryData()
        {
            if ( tcds == null ) return false;
            TaskEntryData tentd = new TaskEntryData();
            if ( tentd.ExistenceTaskEntryData( "TaskCode", "@tCod", tcds.TaskCode, "Char" ) ) return false;    // 既存

            ted = new TaskEntryData();
            tcds.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            tcds.Department = Convert.ToString( comboBoxDepartment.SelectedValue );
            ted.EditTaskEntryData( ted, tcds, Convert.ToInt32( labelTaskEntryID.Text ) );

            if ( !ted.UpdateTaskEntryData( ted ) ) return false;
            return true;
        }


        private void clearLabel()
        {
            labelPartner.Text = "";
            labelTaskPlace.Text = "";
            labelTaskCode.Text = "";
            labelTtlTName.Visible = false;
            labelTaskName.Text = "";
            labelTaskEntryID.Text = "";
            clearDetailLabel();
        }


        private void clearDetailLabel()
        {
            labelTtlLeader.Visible = false;
            labelTtlSales.Visible = false;
            labelLeaderName.Text = "";
            labelSalesMName.Text = "";
            labelContractDate.Text = "";
            labelStartDate.Text = "";
            labelEndDate.Text = "";
        }
    }
}

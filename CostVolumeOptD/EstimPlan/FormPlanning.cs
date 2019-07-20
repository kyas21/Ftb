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
    public partial class FormPlanning :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        TaskEntryData ted;
        private bool iniPro = true;
        PlanningData[] plnd = new PlanningData[3];
        private int endPoint = -1;
        //private int maxVersion = -1;            // 版数未発行　0から開始
        private int maxVersion = 0;            // 版数未発行　0から開始

        private Button[] buttonPlan;

        private TextBox[] textBoxSales;
        private Label[] labelTax;
        private Label[] labelBudgets;
        private Label[] labelCostR;

        private Label[] labelDirect;
        private Label[] labelOutS;
        private Label[] labelMatel;
        private Label[] labelSum;
        private Label[] labelOther;
        private Label[] labelAdmCost;
        private Label[] labelTotal;

        private ComboBox[] cbConf0;
        private DateTimePicker[] dtConf0;
        private CheckBox[] ckConf0;

        private ComboBox[] cbConf1;
        private DateTimePicker[] dtConf1;
        private CheckBox[] ckConf1;

        private ComboBox[] cbConf2;
        private DateTimePicker[] dtConf2;
        private CheckBox[] ckConf2;

        const string BookName = "稟議書(実行予算書).xlsx";
        const string SheetName = "Planning";
        private bool updateStat = false;
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormPlanning()
        {
            InitializeComponent();
        }

        public FormPlanning( TaskEntryData ted )
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
        private void FormPlanning_Load( object sender, EventArgs e )
        {
            labelMsg.Text = "";
            createArray_Controls();                                     // Controlの配列化

            create_cbMember();                                          // 業務担当者、営業担当者
            labelPublisher.Text = ted.OfficeName + ted.DepartName;      // 発行部署
            labelTaskCode.Text = ted.TaskCode;                          // 業務番号
            labelTask.Text = ted.TaskName;                              // 業務名
            labelWorkingPlace.Text = ted.TaskPlace;                     // 業務場所
            labelPartner.Text = ted.PartnerName;                        // 発注者
            textBoxTaxRate.Text = DHandling.DecimaltoStr( ted.TaxRate * 100, "0.00" );
            textBoxOthersCostRate.Text = DHandling.DecimaltoStr( ted.OthersCostRate * 100, "0.00" );
            textBoxAdminCostRate.Text = DHandling.DecimaltoStr( ted.AdminCostRate * 100, "0.00" );

            // 日付初期値
            if( DHandling.CheckDate( ted.ContractDate ) ) dateTimePickerContract.Value = ted.ContractDate;
            if( DHandling.CheckDate( ted.StartDate ) ) dateTimePickerStart.Value = ted.StartDate;
            if( DHandling.CheckDate( ted.EndDate ) ) dateTimePickerEnd.Value = ted.EndDate;

            // 予算データ読込表示
            //PlanningData[] plnd 初期化;
            for( int i = 0; i < plnd.Length; i++ )  
            {
                plnd[i] = new PlanningData();
                plnd[i].PlanningID = -1;
            }
            // 読込
            if(loadExistingPlanningData())
            {
                // データ有
                viewPlanningData();
                viewPlanningContSummary(); 
                setConfirmTitleVisible( 0 );    // タイトル表示;
            }
            else 
            {
                // データ無し
                setConfirmTitleVisible( 1 );    // タイトル非表示;
            }
            // ボタンなど表示項目設定
            setDisabled();
            setEnabled( endPoint );
            setConfirmData( endPoint );

            updateStat = false;
        }


        private void FormPlanning_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
        }


        //--------------------------------------------------------------------//
        // Event                                                              //
        //--------------------------------------------------------------------//
        private void button_Click( object sender, EventArgs e )
        {
            labelMsg.Text = "";
            if( iniPro ) return;
            Button btn = ( Button )sender;

            editTaskEntryData();
            switch( btn.Name )
            {
                case "buttonSave":
                    if( !updatePlanningData() ) return;
                    if( !ted.UpdateTaskEntryData( ted ) ) return;     // 担当者、工期の更新
                    updateStat = false;
                    labelMsg.Text = "保存しました。";
                    break;

                case "buttonPlan0":
                    transitionPlanningCont( 0 );
                    break;

                case "buttonPlan1":
                    transitionPlanningCont( 1 );
                    break;

                case "buttonPlan2":
                    transitionPlanningCont( 2 );
                    break;

                case "buttonClose":
                    if( updateStat )
                    {
                        Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                        if( dialogRemining() == DialogResult.No ) return;
                    }
                    this.Close();
                    break;

                case "buttonPrint":
                    createExcelFile();
                    break;

                default:
                    return;
            }

            if ( btn.Name == "buttonPlan0" || btn.Name == "buttonPlan1" || btn.Name == "buttonPlan2" )
            {
                if( loadExistingPlanningData() )
                {
                    // データ有
                    viewPlanningData();
                    viewPlanningContSummary(); 
                    // タイトル表示;
                    setConfirmTitleVisible( 0 );
                }
                else
                {
                    // データ無
                    plnd[0].PlanningID = -1;
                    plnd[1].PlanningID = -1;
                    plnd[2].PlanningID = -1;
                    // タイトル非表示;
                    setConfirmTitleVisible( 1 );
                }

                setDisabled();
                setEnabled( endPoint );

                setConfirmData( endPoint );
            }
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;

            switch( dtp.Name )
            {
                case "dateTimePickerContract":
                    break;

                case "dateTimePickerStart":
                    if( dateTimePickerStart.Value < dateTimePickerContract.Value )
                        MessageBox.Show( "開始日が契約日より先（前）になっています。修正してください！" );
                    break;

                case "dateTimePickerEnd":
                    if( dateTimePickerEnd.Value < dateTimePickerStart.Value )
                        MessageBox.Show( "終了日が始日より先（前）になっています。修正してください！" );
                    break;

                default:
                    return;
            }
            updateStat = true;
        }


        private void textBoxSales_Leave( object sender, EventArgs e )
        {
            if( iniPro ) return;
            TextBox tb = ( TextBox )sender;

            decimal WorkDwcimal = 0;
            decimal.TryParse( Convert.ToString( tb.Text ), out WorkDwcimal );
            if( tb.Text != "" ) tb.Text = decFormat( WorkDwcimal );

            switch( tb.Name )
            {
                case "textBoxSales0":
                    ContractorsCalc( WorkDwcimal, this.labelTax0, this.labelOther0, this.labelAdmCost0, this.labelTotal0, tb.Text );
                    break;

                case "textBoxSales1":
                    ContractorsCalc( WorkDwcimal, this.labelTax1, this.labelOther1, this.labelAdmCost1, this.labelTotal1, tb.Text );
                    break;

                case "textBoxSales2":
                    ContractorsCalc( WorkDwcimal, this.labelTax2, this.labelOther2, this.labelAdmCost2, this.labelTotal2, tb.Text );
                    break;

                default:
                    break;
            }
            updateStat = true;
        }


        private void textBoxDiscuss_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            updateStat = true;
        }


        private void textBox_Leave( object sender, EventArgs e )
        {
            if( iniPro ) return;

            TextBox tb = ( TextBox )sender;

            decimal WorkDwcimal = 0;
            decimal.TryParse( Convert.ToString( tb.Text ), out WorkDwcimal );
            tb.Text = decPointFormat( WorkDwcimal );
            updateStat = true;
        }


        private void checked_Changed( object sender, EventArgs e )
        {
            if( iniPro ) return;
            CheckBox cx = ( CheckBox )sender;
            setEnabled( endPoint );
            updateStat = true;
        }

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // 各種コントロールを配列として扱う準備
        private void createArray_Controls()
        {
            buttonPlan = new Button[] { buttonPlan0, buttonPlan1, buttonPlan2 };
            textBoxSales = new TextBox[] { textBoxSales0, textBoxSales1, textBoxSales2 };
            labelTax = new Label[] { labelTax0, labelTax1, labelTax2 };
            labelBudgets = new Label[] { labelBudgets0, labelBudgets1, labelBudgets2 };
            labelCostR = new Label[] { labelCostR0, labelCostR1, labelCostR2 };
            labelDirect = new Label[] { labelDirect0, labelDirect1, labelDirect2 };
            labelOutS = new Label[] { labelOutS0, labelOutS1, labelOutS2 };
            labelMatel = new Label[] { labelMatel0, labelMatel1, labelMatel2 };
            labelSum = new Label[] { labelSum0, labelSum1, labelSum2 };
            labelOther = new Label[] { labelOther0, labelOther1, labelOther2 };
            labelAdmCost = new Label[] { labelAdmCost0, labelAdmCost1, labelAdmCost2 };
            labelTotal = new Label[] { labelTotal0, labelTotal1, labelTotal2 };

            clearLabelText();

            cbConf0 = new ComboBox[] { comboBoxNm00, comboBoxNm01, comboBoxNm02, comboBoxNm03, comboBoxNm04, comboBoxNm05 };
            dtConf0 = new DateTimePicker[] { dateTimePickerDt00, dateTimePickerDt01, dateTimePickerDt02, dateTimePickerDt03, dateTimePickerDt04, dateTimePickerDt05 };
            ckConf0 = new CheckBox[] { checkBoxAc00, checkBoxAc01, checkBoxAc02, checkBoxAc03, checkBoxAc04, checkBoxAc05 };
            for( int i = 0; i < ckConf0.Length; i++ ) ckConf0[i].CheckedChanged += checked_Changed;

            cbConf1 = new ComboBox[] { comboBoxNm10, comboBoxNm11, comboBoxNm12, comboBoxNm13, comboBoxNm14, comboBoxNm15 };
            dtConf1 = new DateTimePicker[] { dateTimePickerDt10, dateTimePickerDt11, dateTimePickerDt12, dateTimePickerDt13, dateTimePickerDt14, dateTimePickerDt15 };
            ckConf1 = new CheckBox[] { checkBoxAc10, checkBoxAc11, checkBoxAc12, checkBoxAc13, checkBoxAc14, checkBoxAc15 };
            for( int i = 0; i < ckConf1.Length; i++ ) ckConf1[i].CheckedChanged += checked_Changed;

            cbConf2 = new ComboBox[] { comboBoxNm20, comboBoxNm21, comboBoxNm22, comboBoxNm23, comboBoxNm24, comboBoxNm25 };
            dtConf2 = new DateTimePicker[] { dateTimePickerDt20, dateTimePickerDt21, dateTimePickerDt22, dateTimePickerDt23, dateTimePickerDt24, dateTimePickerDt25 };
            ckConf2 = new CheckBox[] { checkBoxAc20, checkBoxAc21, checkBoxAc22, checkBoxAc23, checkBoxAc24, checkBoxAc25 };
            for( int i = 0; i < ckConf2.Length; i++ ) ckConf2[i].CheckedChanged += checked_Changed;
        }


        private void clearLabelText()
        {
            for( int i = 0; i < labelTax.Length; i++ )
            {
                textBoxSales[i].Text = "";
                labelTax[i].Text = "";
                labelBudgets[i].Text = "";
                labelCostR[i].Text = "";
                labelDirect[i].Text = "";
                labelOutS[i].Text = "";
                labelMatel[i].Text = "";
                labelSum[i].Text = "";
                labelOther[i].Text = "";
                labelAdmCost[i].Text = "";
                labelTotal[i].Text = "";
            }
        }


        // ControlBox作成
        // 確認 業務担当者 営業担当者のComboBox作成
        private void create_cbMember()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxLeader );
            string wParam = " WHERE MemberType = 0 AND EnrollMent = 0 AND OfficeCode = '" + ted.OfficeCode + "' AND Department = '" + ted.Department + "'";
            cbe.TableData( "M_Members", "MemberCode", "Name", wParam );
            //if( ted.LeaderMCode != null ) comboBoxLeader.SelectedValue = ted.LeaderMCode;        // 初期値
            comboBoxLeader.SelectedValue = string.IsNullOrEmpty( ted.LeaderMCode ) ? ted.MemberCode : ted.LeaderMCode;        // 初期値

            cbe = new ComboBoxEdit( comboBoxSalesM );
            cbe.TableData( "M_Members", "MemberCode", "Name", wParam );
            if( ted.SalesMCode != null ) comboBoxSalesM.SelectedValue = ted.SalesMCode;        // 初期値

            create_ConfirmMemberCB( cbe );
            create_ConfirmOtherMemberCB();
        }


        // 確認欄 作成営業担当者、確認業務担当者
        private void create_ConfirmMemberCB( ComboBoxEdit cbe )
        {
            ComboBox[] cbCreator = new ComboBox[] { comboBoxNm00, comboBoxNm10, comboBoxNm20 };
            ComboBox[] cbConfirm = new ComboBox[] { comboBoxNm01, comboBoxNm11, comboBoxNm21 };
            ComboBoxEdit cbConf;
            for( int i = 0; i < cbCreator.Length; i++ )
            {
                cbConf = new ComboBoxEdit( cbCreator[i] );
                cbConf.ValueItem = cbe.ValueItem;
                cbConf.DisplayItem = cbe.DisplayItem;
                cbConf.Basic();
                cbCreator[i].Text = null;

                cbConf = new ComboBoxEdit( cbConfirm[i] );
                cbConf.ValueItem = cbe.ValueItem;
                cbConf.DisplayItem = cbe.DisplayItem;
                cbConf.Basic();
                cbConfirm[i].Text = null;
            }
        }


        private void create_ConfirmOtherMemberCB()
        {
            // 入力代行者
            ComboBox[] cbProxy = new ComboBox[] { comboBoxNm02, comboBoxNm12, comboBoxNm22 };
            CommonData comd = new CommonData();
            string[] memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "InputNumber", ted.OfficeCode, ted.Department );
            if( memberArray == null )
            {
                memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "InputNumber", "H", "2" );
            }
            ComboBoxEdit cbe = new ComboBoxEdit( cbProxy[0] );
            MembersData md = new MembersData();
            cbe.ValueItem = new string[memberArray.Length];
            cbe.DisplayItem = new string[memberArray.Length];
            for( int i = 0; i < memberArray.Length; i++ )
            {
                cbe.ValueItem[i] = memberArray[i];
                cbe.DisplayItem[i] = md.SelectMemberName( memberArray[i] );
            }
            cbe.Basic();

            ComboBoxEdit cbes;
            for( int i = 1; i < cbProxy.Length; i++ )
            {
                cbes = new ComboBoxEdit( cbProxy[i] );
                cbes.ValueItem = cbe.ValueItem;
                cbes.DisplayItem = cbe.DisplayItem;
                cbes.Basic();
            }

            //ComboBoxEdit cbes;
            //if(memberArray.Length == 0 )
            //{
            //    cbe.CreateEmptyComboBox();
            //}
            //else
            //{
            //    cbe.ValueItem = new string[memberArray.Length];
            //    cbe.DisplayItem = new string[memberArray.Length];
            //    for( int i = 0; i < memberArray.Length; i++ )
            //    {
            //        cbe.ValueItem[i] = memberArray[i];
            //        cbe.DisplayItem[i] = md.SelectMemberName( memberArray[i] );
            //    }
            //    cbe.Basic();

            //    for( int i = 1; i < cbProxy.Length; i++ )
            //    {
            //        cbes = new ComboBoxEdit( cbProxy[i] );
            //        cbes.ValueItem = cbe.ValueItem;
            //        cbes.DisplayItem = cbe.DisplayItem;
            //        cbes.Basic();
            //    }
            //}


            // 審査者
            ComboBox[] cbScreener = new ComboBox[] { comboBoxNm03, comboBoxNm13, comboBoxNm23 };
            memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "ConfirmAdm", ted.OfficeCode, ted.Department );
            cbe = new ComboBoxEdit( cbScreener[0] );
            md = new MembersData();
            cbe.ValueItem = new string[memberArray.Length];
            cbe.DisplayItem = new string[memberArray.Length];
            for( int i = 0; i < memberArray.Length; i++ )
            {
                cbe.ValueItem[i] = memberArray[i];
                cbe.DisplayItem[i] = md.SelectMemberName( memberArray[i] );
            }
            cbe.Basic();

            for( int i = 1; i < cbScreener.Length; i++ )
            {
                cbes = new ComboBoxEdit( cbScreener[i] );
                cbes.ValueItem = cbe.ValueItem;
                cbes.DisplayItem = cbe.DisplayItem;
                cbes.Basic();
            }

            //for( int i = 0; i < cbScreener.Length; i++ ) cbScreener[i].Text = null;

            // 承認取締役
            ComboBox[] cbApOfficer = new ComboBox[] { comboBoxNm04, comboBoxNm14, comboBoxNm24 };
            memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "MakeOrder", ted.OfficeCode, ted.Department );
            cbe = new ComboBoxEdit( cbApOfficer[0] );
            md = new MembersData();
            cbe.ValueItem = new string[memberArray.Length];
            cbe.DisplayItem = new string[memberArray.Length];
            for( int i = 0; i < memberArray.Length; i++ )
            {
                cbe.ValueItem[i] = memberArray[i];
                cbe.DisplayItem[i] = md.SelectMemberName( memberArray[i] );
            }
            cbe.Basic();

            for( int i = 1; i < cbApOfficer.Length; i++ )
            {
                cbes = new ComboBoxEdit( cbApOfficer[i] );
                cbes.ValueItem = cbe.ValueItem;
                cbes.DisplayItem = cbe.DisplayItem;
                cbes.Basic();
            }

            // 承認社長
            ComboBox[] cbApPresident = new ComboBox[] { comboBoxNm05, comboBoxNm15, comboBoxNm25 };
            memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "Approval", "H", null );
            md = new MembersData();
            string presidentName = md.SelectMemberName( memberArray[0] );

            for( int i = 0; i < cbApPresident.Length; i++ )
            {
                cbe = new ComboBoxEdit( cbApPresident[i] );
                cbe.ValueItem = new string[1];
                cbe.DisplayItem = new string[1];

                cbe.ValueItem[0] = memberArray[0];
                cbe.DisplayItem[0] = presidentName;
                cbe.Basic();
            }
        }


        private void setDisabled()
        {
            for( int i = 0; i < buttonPlan.Length; i++ ) buttonPlan[i].Enabled = false;

            for( int i = 0; i < cbConf0.Length; i++ )
            {
                cbConf0[i].Enabled = false;
                cbConf0[i].Visible = false;
                dtConf0[i].Enabled = false;
                dtConf0[i].Visible = false;
                ckConf0[i].Enabled = false;
                ckConf0[i].Visible = false;

                cbConf1[i].Enabled = false;
                cbConf1[i].Visible = false;
                dtConf1[i].Enabled = false;
                dtConf1[i].Visible = false;
                ckConf1[i].Enabled = false;
                ckConf1[i].Visible = false;

                cbConf2[i].Enabled = false;
                cbConf2[i].Visible = false;
                dtConf2[i].Enabled = false;
                dtConf2[i].Visible = false;
                ckConf2[i].Enabled = false;
                ckConf2[i].Visible = false;
            }
        }


        private void setEnabled( int endPoint )
        {
            buttonPlan[0].Enabled = true;
            if( endPoint > -1 ) buttonPlan[1].Enabled = true;
            if( endPoint > 0 )  buttonPlan[2].Enabled = true;

            for( int idx = 0; idx < endPoint + 1; idx++ )
            {
                switch( idx )
                {
                    case 0:
                        for( int i = 0; i < cbConf0.Length; i++ )
                        {
                            cbConf0[i].Visible = true;
                            dtConf0[i].Visible = true;
                            ckConf0[i].Visible = true;

                            if (i == 0 )
                            {
                                cbConf0[i].Enabled = true;
                                dtConf0[i].Enabled = true;
                                ckConf0[i].Enabled = true;
                            }
                            if(i > 0 )
                            {
                                if( ckConf0[i - 1].Checked )
                                {
                                    cbConf0[i].Enabled = true;
                                    dtConf0[i].Enabled = true;
                                    ckConf0[i].Enabled = true;
                                }
                            }
                        }
                        break;
                    case 1:
                        for( int i = 0; i < cbConf1.Length; i++ )
                        {
                            cbConf1[i].Visible = true;
                            dtConf1[i].Visible = true;
                            ckConf1[i].Visible = true;

                            if( i == 0 )
                            {
                                cbConf1[i].Enabled = true;
                                dtConf1[i].Enabled = true;
                                ckConf1[i].Enabled = true;
                            }
                            if( i > 0 )
                            {
                                if( ckConf1[i - 1].Checked )
                                {
                                    cbConf1[i].Enabled = true;
                                    dtConf1[i].Enabled = true;
                                    ckConf1[i].Enabled = true;
                                }
                            }

                        }
                        break;
                    case 2:
                        for( int i = 0; i < cbConf2.Length; i++ )
                        {
                            cbConf2[i].Visible = true;
                            dtConf2[i].Visible = true;
                            ckConf2[i].Visible = true;

                            if( i == 0 )
                            {
                                cbConf2[i].Enabled = true;
                                dtConf2[i].Enabled = true;
                                ckConf2[i].Enabled = true;
                            }
                            if( i > 0 )
                            {
                                if( ckConf2[i - 1].Checked )
                                {
                                    cbConf2[i].Enabled = true;
                                    dtConf2[i].Enabled = true;
                                    ckConf2[i].Enabled = true;
                                }
                            }

                        }
                        break;
                    default:
                        break;
                }
            }
        }


        private void setConfirmTitleVisible( int sw)
        {
            Panel[] panelTitle = new Panel[] { panelT0, panelT1, panelT2, panelT3, panelT4, panelT5 };

            for( int i = 0; i < panelTitle.Length; i++ ) panelTitle[i].Visible = sw == 0 ? true : false;    // sw:0 表示、1 非表示
        }

        
        private void setConfirmData( int endPoint )
        {
            for( int idx = 0; idx < endPoint + 1; idx++ )
            {
                //if( plnd[idx].CreateStat == 1 )
                //    setConfirmItems( idx, plnd[idx].CreateMCd, plnd[idx].CreateDate, 0 );
                if( plnd[idx].CreateStat == 1 )
                {
                    setConfirmItems( idx, plnd[idx].CreateMCd, plnd[idx].CreateDate, 0 );
                }
                else
                {
                    setConfirmItems( idx, ted.MemberCode, DateTime.Today, 0 );
                }

                if( plnd[idx].ConfirmStat == 1 )
                    setConfirmItems( idx, plnd[idx].ConfirmMCd, plnd[idx].ConfirmDate, 1 );

                if( plnd[idx].ProxyStat == 1 )
                    setConfirmItems( idx, plnd[idx].ProxyMCd, plnd[idx].ProxyDate, 2 );

                if( plnd[idx].ScreeningStat == 1 )
                    setConfirmItems( idx, plnd[idx].ScreeningMCd, plnd[idx].ScreeningDate, 3 );

                if( plnd[idx].ApOfficerStat == 1 )
                    setConfirmItems( idx, plnd[idx].ApOfficerMCd, plnd[idx].ApOfficerDate, 4 );

                if( plnd[idx].ApPresidentStat == 1 )
                    setConfirmItems( idx, plnd[idx].ApPresidentMCd, plnd[idx].ApPresidentDate, 5 );
            }
        }


        private void setConfirmItems(int idx, string mCode, DateTime confDt, int iNo )
        {
            switch( idx )
            {
                case 0:
                    cbConf0[iNo].SelectedValue = mCode.TrimEnd();
                    dtConf0[iNo].Value = confDt;
                    ckConf0[iNo].Checked = true;
                    break;
                case 1:
                    cbConf1[iNo].SelectedValue = mCode.TrimEnd();
                    dtConf1[iNo].Value = confDt;
                    ckConf1[iNo].Checked = true;
                    break;
                case 2:
                    cbConf2[iNo].SelectedValue = mCode.TrimEnd();
                    dtConf2[iNo].Value = confDt;
                    ckConf2[iNo].Checked = true;
                    break;
                default:
                    break;
            }
            //buttonEnabled( idx );
            setEnabled( endPoint );
        }
            

        private bool loadExistingPlanningData()
        {
            // 初期化
            clearLabelText();
            for( int i = 0; i < plnd.Length; i++ )
            {
                plnd[i] = new PlanningData();
                plnd[i].PlanningID = 0;
            }
            endPoint = -1;
            // 読み込み
            EstPlanOp epo = new EstPlanOp( "D_Planning" );
            DataTable dt = epo.EstPlan_Select( ted.TaskEntryID );
            if( dt == null ) return false;      // データなし

            editPlanningData( dt.Rows[0], 0);   // 最初のデータは無条件に0番にセット

            if( dt.Rows.Count == 2 ) editPlanningData( dt.Rows[1], 1 );  // データ数が2件、0番,1番のみのとき

            if( dt.Rows.Count > 2 )             // データ件数が2件より多い（=3件以上のとき）
            {
                editPlanningData( dt.Rows[dt.Rows.Count - 2], 1 );      // 最後から2番目を1に
                editPlanningData( dt.Rows[dt.Rows.Count - 1], 2 );      // 最後を2に
            }

            endPoint = ( dt.Rows.Count > 3 ) ? 2 : dt.Rows.Count - 1;
            maxVersion = plnd[endPoint].VersionNo;
            return true;
        }


        private void editPlanningData(DataRow dr, int idx )
        {
            plnd[idx] = new PlanningData( dr );

            plnd[idx].LeaderMCode = ted.LeaderMCode;
            plnd[idx].SalesMCode = ted.SalesMCode;
            plnd[idx].ContractDate = ted.ContractDate;
            plnd[idx].StartDate = ted.StartDate;
            plnd[idx].EndDate = ted.EndDate;
            plnd[idx].OfficeCode = ted.OfficeCode;
            plnd[idx].Department = ted.Department;
            //plnd[idx].MaxVersion = maxVersion;
        }


        private void viewPlanningData()
        {
            if( endPoint < 0 ) return;

            for( int i = 0; i < plnd.Length; i++ )
            {
                //if( plnd[i].PlanningID == 0 ) break;
                if( plnd[i].PlanningID < 1 ) break;

                textBoxSales[i].Text = DHandling.DecimaltoStr( plnd[i].Sales, "#,0" );
                labelTax[i].Text = DHandling.DecimaltoStr( ( plnd[i].Sales * ( Convert.ToDecimal( textBoxTaxRate.Text ) / 100 ) ), "#,0" );
                labelBudgets[i].Text = DHandling.DecimaltoStr( plnd[i].Budgets, "#,0" );
                labelCostR[i].Text = plnd[i].Sales == 0 ? "" : DHandling.DecimaltoStr( ( plnd[i].Budgets / plnd[i].Sales * 100 ), "0.00" );
            }
            if( ted.LeaderMCode != null ) comboBoxLeader.SelectedValue = ted.LeaderMCode;
            if( ted.SalesMCode != null ) comboBoxSalesM.SelectedValue = ted.SalesMCode;
            if( DHandling.CheckDate( ted.ContractDate ) ) dateTimePickerContract.Value = Convert.ToDateTime( ted.ContractDate );
            if( DHandling.CheckDate( ted.StartDate ) ) dateTimePickerStart.Value = Convert.ToDateTime( ted.StartDate );
            if( DHandling.CheckDate( ted.EndDate ) ) dateTimePickerEnd.Value = Convert.ToDateTime( ted.EndDate );

            textBoxDiscuss.Text = plnd[endPoint].Discussion;
            return;
        }


        private void viewPlanningContSummary()
        {
            decimal qty = 0;
            decimal direct = 0;
            decimal outs = 0;
            decimal matel = 0;
            decimal other = 0;
            decimal admCost = 0;
            SqlHandling sql = new SqlHandling( "D_PlanningCont" );
            DataTable dt;
            DataRow dr;

            for( int i = 0; i < plnd.Length; i++ )
            {
                if( plnd[i] == null ) break;
                if( plnd[i].PlanningID < 1 ) break;
                dt = sql.SelectAllData( "WHERE PlanningID = " + plnd[i].PlanningID );
                if( dt == null ) return;

                direct = 0;
                outs = 0;
                matel = 0;
                for( int j = 0; j < dt.Rows.Count; j++ )
                {
                    dr = dt.Rows[j];
                    qty = Convert.ToDecimal( dr["Quantity"] );
                    direct += Convert.ToDecimal( dr["Cost0"] ) * qty;
                    outs += Convert.ToDecimal( dr["Cost1"] ) * qty;
                    matel += Convert.ToDecimal( dr["Cost2"] ) * qty;
                }
                labelDirect[i].Text = DHandling.DecimaltoStr( direct, "#,0" );
                labelOutS[i].Text = DHandling.DecimaltoStr( outs, "#,0" );
                labelMatel[i].Text = DHandling.DecimaltoStr( matel, "#,0" );
                labelSum[i].Text = DHandling.DecimaltoStr( direct + outs + matel, "#,0" );
                other = plnd[i].Sales * ( Convert.ToDecimal( textBoxOthersCostRate.Text ) / 100 );
                admCost = plnd[i].Sales * ( Convert.ToDecimal( textBoxAdminCostRate.Text ) / 100 );
                labelOther[i].Text = DHandling.DecimaltoStr( other, "#,0" );
                labelAdmCost[i].Text = DHandling.DecimaltoStr( admCost, "#,0" );
                labelTotal[i].Text = DHandling.DecimaltoStr( direct + outs + matel + other + admCost, "#,0" );
            }
            return;
        }


        private void transitionPlanningCont( int buttonIdx )
        {
            // 予算計画データの有無に関係ないデータ
            for(int i = 0;i<3;i++ )
            {
                plnd[i].Sales = string.IsNullOrEmpty(textBoxSales[i].Text)?0: DecimalMask( textBoxSales[i].Text );
                plnd[i].Other = string.IsNullOrEmpty( labelOther[i].Text ) ? 0 : DecimalMask( labelOther[i].Text );
                plnd[i].AdmCost = string.IsNullOrEmpty( labelAdmCost[i].Text ) ? 0 : DecimalMask( labelAdmCost[i].Text );
                plnd[i].OfficeCode = ted.OfficeCode;
                plnd[i].Department = ted.Department;
                plnd[i].CostType = ted.CostType;
                plnd[i].TaskEntryID = ted.TaskEntryID;
                plnd[i].MaxVersion = maxVersion;
            }

            if (endPoint > -1 )
            {
                plnd[endPoint].Discussion = string.IsNullOrEmpty( textBoxDiscuss.Text ) ? "" : textBoxDiscuss.Text;
                for( int i = 0; i < endPoint; i++ ) storeConfirmDataToPlnd( i );
            }

            plnd = FormPlanningCont.SummaryData( ted, plnd, buttonIdx );

            return;
        }


        private void editTaskEntryData()
        {
            //担当者名とかいろいろ
            ted.LeaderMCode = Convert.ToString( comboBoxLeader.SelectedValue );
            ted.SalesMCode = Convert.ToString( comboBoxSalesM.SelectedValue );
            ted.ContractDate = Convert.ToDateTime( dateTimePickerContract.Text );
            ted.StartDate = Convert.ToDateTime( dateTimePickerStart.Text );
            ted.EndDate = Convert.ToDateTime( dateTimePickerEnd.Text );

            ted.TaxRate = Convert.ToDecimal( textBoxTaxRate.Text ) / 100;
            ted.OthersCostRate = Convert.ToDecimal( textBoxOthersCostRate.Text ) / 100;
            ted.AdminCostRate = Convert.ToDecimal( textBoxAdminCostRate.Text ) / 100;
            ted.CostType = String.IsNullOrEmpty( ted.CostType ) ? "" : ted.CostType;
            ted.TaskCode = String.IsNullOrEmpty( ted.TaskCode ) ? "" : ted.TaskCode;
        }

        private void storeConfirmDataToPlnd(int idx )
        {
            switch( idx )
            {
                case 0:
                    plnd[idx].CreateMCd = ( ckConf0[0].Checked ) ? Convert.ToString( cbConf0[0].SelectedValue ) : "";
                    plnd[idx].CreateDate = ( ckConf0[0].Checked ) ? dtConf0[0].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].CreateStat = ( ckConf0[0].Checked ) ? 1 : 0;

                    plnd[idx].ConfirmMCd = ( ckConf0[1].Checked ) ? Convert.ToString( cbConf0[1].SelectedValue ) : "";
                    plnd[idx].ConfirmDate = ( ckConf0[1].Checked ) ? dtConf0[1].Value.StripTime() : DateTime.MinValue;
                    plnd[idx].ConfirmStat = ( ckConf0[1].Checked ) ? 1 : 0;

                    plnd[idx].ProxyMCd = ( ckConf0[2].Checked ) ? Convert.ToString( cbConf0[2].SelectedValue ) : "";
                    plnd[idx].ProxyDate = ( ckConf0[2].Checked ) ? dtConf0[2].Value.StripTime() : DateTime.MinValue;
                    plnd[idx].ProxyStat = ( ckConf0[2].Checked ) ? 1 : 0;

                    plnd[idx].ScreeningMCd = ( ckConf0[3].Checked ) ? Convert.ToString( cbConf0[3].SelectedValue ) : "";
                    plnd[idx].ScreeningDate = ( ckConf0[3].Checked ) ? dtConf0[3].Value.StripTime() : DateTime.MinValue;
                    plnd[idx].ScreeningStat = ( ckConf0[3].Checked ) ? 1 : 0;

                    plnd[idx].ApOfficerMCd = ( ckConf0[4].Checked ) ? Convert.ToString( cbConf0[4].SelectedValue ) : "";
                    plnd[idx].ApOfficerDate = ( ckConf0[4].Checked ) ? dtConf0[4].Value.StripTime() : DateTime.MinValue;
                    plnd[idx].ApOfficerStat = ( ckConf0[4].Checked ) ? 1 : 0;

                    plnd[idx].ApPresidentMCd = ( ckConf0[5].Checked ) ? Convert.ToString( cbConf0[5].SelectedValue ) : "";
                    plnd[idx].ApPresidentDate = ( ckConf0[5].Checked ) ? dtConf0[5].Value.StripTime() : DateTime.MinValue;
                    plnd[idx].ApPresidentStat = ( ckConf0[5].Checked ) ? 1 : 0;

                    break;
                case 1:
                    plnd[idx].CreateMCd = ( ckConf1[0].Checked ) ? Convert.ToString( cbConf1[0].SelectedValue ) : "";
                    plnd[idx].CreateDate = ( ckConf1[0].Checked ) ? dtConf1[0].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].CreateStat = ( ckConf1[0].Checked ) ? 1 : 0;

                    plnd[idx].ConfirmMCd = ( ckConf1[1].Checked ) ? Convert.ToString( cbConf1[1].SelectedValue ) : "";
                    plnd[idx].ConfirmDate = ( ckConf1[1].Checked ) ? dtConf1[1].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ConfirmStat = ( ckConf1[1].Checked ) ? 1 : 0;

                    plnd[idx].ProxyMCd = ( ckConf1[2].Checked ) ? Convert.ToString( cbConf1[2].SelectedValue ) : "";
                    plnd[idx].ProxyDate = ( ckConf1[2].Checked ) ? dtConf1[2].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ProxyStat = ( ckConf1[2].Checked ) ? 1 : 0;

                    plnd[idx].ScreeningMCd = ( ckConf1[3].Checked ) ? Convert.ToString( cbConf1[3].SelectedValue ) : "";
                    plnd[idx].ScreeningDate = ( ckConf1[3].Checked ) ? dtConf1[3].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ScreeningStat = ( ckConf1[3].Checked ) ? 1 : 0;

                    plnd[idx].ApOfficerMCd = ( ckConf1[4].Checked ) ? Convert.ToString( cbConf1[4].SelectedValue ) : "";
                    plnd[idx].ApOfficerDate = ( ckConf1[4].Checked ) ? dtConf1[4].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ApOfficerStat = ( ckConf1[4].Checked ) ? 1 : 0;

                    plnd[idx].ApPresidentMCd = ( ckConf1[5].Checked ) ? Convert.ToString( cbConf1[5].SelectedValue ) : "";
                    plnd[idx].ApPresidentDate = ( ckConf1[5].Checked ) ? dtConf1[5].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ApPresidentStat = ( ckConf1[5].Checked ) ? 1 : 0;

                    break;
                case 2:
                    plnd[idx].CreateMCd = ( ckConf2[0].Checked ) ? Convert.ToString( cbConf2[0].SelectedValue ) : "";
                    plnd[idx].CreateDate = ( ckConf2[0].Checked ) ? dtConf2[0].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].CreateStat = ( ckConf2[0].Checked ) ? 1 : 0;

                    plnd[idx].ConfirmMCd = ( ckConf2[1].Checked ) ? Convert.ToString( cbConf2[1].SelectedValue ) : "";
                    plnd[idx].ConfirmDate = ( ckConf2[1].Checked ) ? dtConf2[1].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ConfirmStat = ( ckConf2[1].Checked ) ? 1 : 0;

                    plnd[idx].ProxyMCd = ( ckConf2[2].Checked ) ? Convert.ToString( cbConf2[2].SelectedValue ) : "";
                    plnd[idx].ProxyDate = ( ckConf2[2].Checked ) ? dtConf2[2].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ProxyStat = ( ckConf2[2].Checked ) ? 1 : 0;

                    plnd[idx].ScreeningMCd = ( ckConf2[3].Checked ) ? Convert.ToString( cbConf2[3].SelectedValue ) : "";
                    plnd[idx].ScreeningDate = ( ckConf2[3].Checked ) ? dtConf2[3].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ScreeningStat = ( ckConf2[3].Checked ) ? 1 : 0;

                    plnd[idx].ApOfficerMCd = ( ckConf2[4].Checked ) ? Convert.ToString( cbConf2[4].SelectedValue ) : "";
                    plnd[idx].ApOfficerDate = ( ckConf2[4].Checked ) ? dtConf2[4].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ApOfficerStat = ( ckConf2[4].Checked ) ? 1 : 0;

                    plnd[idx].ApPresidentMCd = ( ckConf2[5].Checked ) ? Convert.ToString( cbConf2[5].SelectedValue ) : "";
                    plnd[idx].ApPresidentDate = ( ckConf2[5].Checked ) ? dtConf2[5].Value.StripTime() : DateTime.MinValue.StripTime();
                    plnd[idx].ApPresidentStat = ( ckConf2[5].Checked ) ? 1 : 0;

                    break;
                default:
                    break;
            }
            //buttonEnabled( idx );
            setEnabled( endPoint );
        }

        
        //private void buttonEnabled(int idx )
        //{
        //    switch( idx )
        //    {
        //        case 0:
        //            for(int i = 0;i<ckConf0.Length - 3;i++ )
        //            {
        //                if(i < 2 )
        //                {
        //                    if( ckConf0[i].Checked )
        //                    {
        //                        cbConf0[i].Enabled = true;
        //                        dtConf0[i].Enabled = true;
        //                        ckConf0[i].Enabled = true;
        //                        cbConf0[i + 1].Enabled = true;
        //                        dtConf0[i + 1].Enabled = true;
        //                        ckConf0[i + 1].Enabled = true;
        //                    }
        //                }

        //                if( ckConf0[2].Checked )
        //                {
        //                    for(int j = 2; j < ckConf0.Length;j++ ) ckConf0[j].Enabled = true;
        //                }
        //            }

        //            break;
        //        case 1:
        //            for(int i = 0;i<ckConf1.Length - 3;i++ )
        //            {
        //                if(i < 2 )
        //                {
        //                    if( ckConf1[i].Checked )
        //                    {
        //                        cbConf1[i].Enabled = true;
        //                        dtConf1[i].Enabled = true;
        //                        ckConf1[i].Enabled = true;
        //                        cbConf1[i + 1].Enabled = true;
        //                        dtConf1[i + 1].Enabled = true;
        //                        ckConf1[i + 1].Enabled = true;
        //                    }
        //                }

        //                if( ckConf1[2].Checked )
        //                {
        //                    for(int j = 2; j < ckConf1.Length;j++ ) ckConf1[j].Enabled = true;
        //                }
        //            }

        //            break;
        //        case 2:
        //            for(int i = 0;i<ckConf2.Length - 3;i++ )
        //            {
        //                if(i < 2 )
        //                {
        //                    if( ckConf2[i].Checked )
        //                    {
        //                        cbConf2[i].Enabled = true;
        //                        dtConf2[i].Enabled = true;
        //                        ckConf2[i].Enabled = true;
        //                        cbConf2[i + 1].Enabled = true;
        //                        dtConf2[i + 1].Enabled = true;
        //                        ckConf2[i + 1].Enabled = true;
        //                    }
        //                }

        //                if( ckConf2[2].Checked )
        //                {
        //                    for(int j = 2; j < ckConf2.Length;j++ ) ckConf2[j].Enabled = true;
        //                }
        //            }

        //            break;
        //        default:
        //            break;
        //    }
        //}

        private bool updatePlanningData()
        {
            for(int i = 0; i<endPoint + 1;i++ )
            {
                if(i == endPoint ) plnd[i].Discussion = textBoxDiscuss.Text;
                storeConfirmDataToPlnd( i );
                if( plnd[i].PlanningID < 1 ) continue;
                EstPlanOp epo = new EstPlanOp();
                if( !epo.Planning_Update( plnd[i] ) ) return false;
            }


            return true;
        }


        private void createExcelFile()
        {
            Publish publ = new Publish( Folder.DefaultExcelTemplate( BookName ) );
            PublishData pd = new PublishData();
            pd.TaskCode = ted.TaskCode;
            pd.TaskName = ted.TaskName;
            pd.CostType = ted.CostType;
            pd.TaskPlace = ted.TaskPlace;
            pd.PartnerName = ted.PartnerName;
            pd.LeaderName = comboBoxLeader.Text;
            pd.SalesMName = comboBoxSalesM.Text;
            pd.Publisher = labelPublisher.Text;

            pd.Sales0 = DHandling.ToRegDecimal( textBoxSales[0].Text );
            pd.Tax0 = DHandling.ToRegDecimal( labelTax[0].Text );
            pd.Sales1 = DHandling.ToRegDecimal( textBoxSales[1].Text );
            pd.Tax1 = DHandling.ToRegDecimal( labelTax[1].Text );
            pd.Sales2 = DHandling.ToRegDecimal( textBoxSales[2].Text );
            pd.Tax2 = DHandling.ToRegDecimal( labelTax[2].Text );

            pd.Direct0 = DHandling.ToRegDecimal( labelDirect[0].Text );
            pd.Direct1 = DHandling.ToRegDecimal( labelDirect[1].Text );
            pd.Direct2 = DHandling.ToRegDecimal( labelDirect[2].Text );
            pd.OutS0 = DHandling.ToRegDecimal( labelOutS[0].Text );
            pd.OutS1 = DHandling.ToRegDecimal( labelOutS[1].Text );
            pd.OutS2 = DHandling.ToRegDecimal( labelOutS[2].Text );
            pd.Matel0 = DHandling.ToRegDecimal( labelMatel[0].Text );
            pd.Matel1 = DHandling.ToRegDecimal( labelMatel[1].Text );
            pd.Matel2 = DHandling.ToRegDecimal( labelMatel[2].Text );

            pd.Note = textBoxDiscuss.Text;

            pd.TaxRate = DHandling.ToRegDecimal( textBoxTaxRate.Text ) / 100;
            pd.OthersCostRate = DHandling.ToRegDecimal( textBoxOthersCostRate.Text ) / 100;
            pd.AdminCostRate = DHandling.ToRegDecimal( textBoxAdminCostRate.Text ) / 100;

            pd.ContractDate = dateTimePickerContract.Value;
            pd.StartDate = dateTimePickerStart.Value;
            pd.EndDate = dateTimePickerEnd.Value;

            publ.ExcelFile( SheetName, pd, null );
            return;
        }


        /// <summary>
        /// 各ラベル算出
        /// </summary>
        /// <param name="ContractorCost">請負金額</param>
        /// <param name="LabelTax">消費税表示ラベル</param>
        /// <param name="LabelOther">その他表示ラベル</param>
        /// <param name="LabelAdmCost">一般管理費表示ラベル</param>
        /// <param name="LabelTotal">合計表示ラベル</param>
        /// <param name="TargetValue">入力値設定</param>
        private void ContractorsCalc( decimal ContractorCost, Label LabelTax, Label LabelOther,
                                        Label LabelAdmCost, Label LabelTotal, string TargetValue )
        {
            if( TargetValue == "" )
            {
                // 消費税
                LabelTax.Text = "";
                // その他
                LabelOther.Text = "";
                // 一般管理費
                LabelAdmCost.Text = "";
            }
            else
            {
                // 消費税
                LabelTax.Text = decFormat( ContractorCost * DecimalMask( this.textBoxTaxRate.Text ) / 100 );
                // その他
                LabelOther.Text = decFormat( ContractorCost * DecimalMask( this.textBoxOthersCostRate.Text ) / 100 );
                // 一般管理費
                LabelAdmCost.Text = decFormat( ContractorCost * DecimalMask( this.textBoxAdminCostRate.Text ) / 100 );
            }

            // 合計算出
            TotalCalc( LabelTotal, TargetValue );
        }

        /// <summary>
        /// 合計産出
        /// </summary>
        /// <param name="LabelTotal">合計表示ラベル</param>
        /// <param name="TargetValue">入力値</param>
        private void TotalCalc( Label LabelTotal, string TargetValue )
        {
            if( LabelTotal.Name == "labelTotal0" )
            {
                LabelTotal.Text = ( TargetValue == "" ) ? ""
                    : decFormat( DecimalMask( this.labelSum0.Text ) + DecimalMask( this.labelOther0.Text ) + DecimalMask( this.labelAdmCost0.Text ) );
            }
            else if( LabelTotal.Name == "labelTotal1" )
            {
                LabelTotal.Text = ( TargetValue == "" ) ? ""
                    : decFormat( DecimalMask( this.labelSum1.Text ) + DecimalMask( this.labelOther1.Text ) + DecimalMask( this.labelAdmCost1.Text ) );

            }
            else if( LabelTotal.Name == "labelTotal2" )
            {
                LabelTotal.Text = ( TargetValue == "" ) ? ""
                    : decFormat( DecimalMask( this.labelSum2.Text ) + DecimalMask( this.labelOther2.Text ) + DecimalMask( this.labelAdmCost2.Text ) );
            }
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private static string decPointFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "0.00" );
        }


        private decimal DecimalMask( string TargetValue )
        {
            decimal WorkDecimal = 0;

            decimal.TryParse( TargetValue, out WorkDecimal );

            return WorkDecimal;
        }


        private void ControlAllClear()
        {
            for( int i = 0; i < labelTax.Length; i++ )
            {
                textBoxSales[i].Text = "";
                labelTax[i].Text = "";
                labelBudgets[i].Text = "";
                labelCostR[i].Text = "";
                labelDirect[i].Text = "";
                labelOutS[i].Text = "";
                labelMatel[i].Text = "";
                labelSum[i].Text = "";
                labelOther[i].Text = "";
                labelAdmCost[i].Text = "";
                labelTotal[i].Text = "";
            }

            this.textBoxDiscuss.Text = "";
        }

    }
}

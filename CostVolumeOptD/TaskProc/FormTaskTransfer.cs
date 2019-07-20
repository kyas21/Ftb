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

namespace TaskProc
{
    public partial class FormTaskTransfer :Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        HumanProperty hp;
        TaskCodeNameData[] tcd;
        PartnersScData[] psd;
        TaskData td;
        TaskIndData[] tid;
        TaskNoteData tn;
        PartnersData pd;
        TaskIndData[] sTID;
        const string BookName = "業務引継書.xlsx";
        private bool iniPro = true;
        private bool btnNew = false;
        private int iniRCnt = 8;
        private int totalProvMark = 0;
        private string taskBaseCode;
        private int versionNo;
        private ComboBox[] cbMgrDev;
        private ComboBox[] cbMgrNam;
        private ComboBox[] cbMbrDev;
        private ComboBox[] cbMbrNam;
        private Label[] lbTitleL;
        private Label[] lbTitleR;
        // 確認エリア
        private Label[] lbCnfT;
        private Label[] lbCnfP;
        private ComboBox[] cbCnfNam;
        private Panel[] PnCnfS;
        private DateTimePicker[] dtCnfDat;

        private ComboBox[] cbONote;

        const int rows = 7;
        const int cols = 4;
        const int dimCnt = rows * cols;
        private string[] officeArray = new string[dimCnt];
        private string[] deptArray = new string[dimCnt];
        private string[] mgrArray = new string[dimCnt];
        private string[] mbrArray = new string[dimCnt];
        private string[] tiIDArray = new string[dimCnt];
        private string[] dListArray = new string[dimCnt];

        DateTime dtToday = DateTime.Today;
        const int ivCol = 1;     // 未表示列数
        const int ivRow = 1;     // 未表示行数
        private bool selTask = true;                     // Task指定済（false) 未指定（true）
        private bool verChange = false;
        private bool cbChange = false;
        private bool ctrlChange = false;
        private bool crtCBOffice = false;                   // true comboBoxOffice 作成済、false 未作成
        private bool crtCBDepart = false;                   // true comboBoxDepart 作成済、false 未作成
        private bool readyCB = false;                   // true comboBox 作成準備完了、false 未作成
        private string holdVerNo = null;

        private int[] cBCostTypeP = new int[2];
        private int[] tBTaskNameP = new int[2];
        private int[] panelG1P = new int[2];
        private int[] panelG2P = new int[2];
        //--------------------------------------------------------------------------//
        //     Constructor                                                          //
        //--------------------------------------------------------------------------//
        public FormTaskTransfer()
        {
            InitializeComponent();
        }

        public FormTaskTransfer( HumanProperty hp )
        {
            InitializeComponent();
            this.hp = hp;
        }
        //--------------------------------------------------------------------------//
        //     Method                                                               //
        //--------------------------------------------------------------------------//
        private void FormTaskTransfer_Load( object sender, EventArgs e )
        {

            this.SuspendLayout();

            UiHandling.FormSizeSTD( this );
            UiHandling.FormPosition( this );

            cBCostTypeP[0] = comboBoxCostType.Location.X;
            cBCostTypeP[1] = comboBoxCostType.Location.Y;
            tBTaskNameP[0] = textBoxTaskName.Location.X;
            tBTaskNameP[1] = textBoxTaskName.Location.Y;
            panelG1P[0] = panelG1.Location.X;
            panelG1P[1] = panelG1.Location.Y;
            panelG2P[0] = panelG2.Location.X;
            panelG2P[1] = panelG2.Location.Y;

            initializeScreen();
            createArray_Controls();
            initializeComboBox();

            editTaskNameDataList();
            ListFormDataOp lo = new ListFormDataOp();
            psd = lo.SelectPartnersCuData();

            buttonPrint.Enabled = false;

            // TRY kusano 21070426
            readyNewRegistration();
            initializeShadowTaskIndData();
            // TRY END
          
           this.ResumeLayout();
        }


        private void FormTaskTransfer_Shown( object sender, EventArgs e )
        {
            dataGridView1.CurrentCell = null;
            iniPro = false;
        }


        private void FormTaskTransfer_FormClosing( object sender, FormClosingEventArgs e )
        {
            Properties.Settings.Default.Save();
        }


        private void button_Click( object sender, EventArgs e )
        {
            // 20180628 asakawa 自動採番から手動採番へ改良のため一部変更

            Button btn = ( Button )sender;
            if( btn.Name == "buttonEnd" ) this.Close();
            if( btn.Name != "buttonNew" && selTask ) return;

            TaskOp tp = new TaskOp();
            int newTaskID;

            switch( btn.Name )
            {
                case "buttonNew":
                    Func<DialogResult> dialogOverLoad = DMessage.DialogOverLoad;
                    if( dialogOverLoad() == DialogResult.No ) return;
                    readyNewRegistration();
                    initializeShadowTaskIndData();
                    break;

                case "buttonSave":
                    if( !checkTaskContents() ) return;
                    if( btnNew )
                    {
                        // 業務番号採番後新規登録

                        // 20180628 asakawa 手動採番のための入力チェックを追加
                        if (textBoxTaskCode.TextLength != 6)
                        {
                            MessageBox.Show("新規登録の場合は、業務番号に６桁の基本コードを入力してください", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (string.Compare(textBoxTaskCode.Text.Substring(2, 1), Convert.ToString(comboBoxOffice.SelectedValue)) != 0)
                        {
                            MessageBox.Show("選択された営業事業所が基本コードと合っていません", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // 入力チェック追加はここまで

                        // 採番ロジック
                        string newTaskBaseCode = "";
                        td = new TaskData();

                        // 20180628 asakawa 以下１行削除
                        // newTaskBaseCode = td.CreateTaskBaseCode( Convert.ToString( comboBoxOffice.SelectedValue ) );
                        // 20180628 asakawa 以下追加
                        newTaskBaseCode = textBoxTaskCode.Text;
                        int iret = td.CheckTaskBaseCode(newTaskBaseCode);
                        if (iret > 1) return;
                        if (iret == 1)
                        {
                            MessageBox.Show("入力された業務番号の基本コードは、すでに登録されています", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // 追加ここまで

                        newTaskID = insertNewVersionTaskData( newTaskBaseCode );
                        if( newTaskID > 0 ) taskBaseCode = newTaskBaseCode;
                        addTaskIndData( newTaskID, dataGridView1 );
                        addTaskNoteData( newTaskID );

                        readyUpdateTaskTransfer( newTaskID );
                        btnNew = false;
                    }
                    else
                    {
                        // 現行（画面表示内容）データを更新
                        if( updateNowVersionTaskData() )
                        {
                            updateTaskIndData( td.TaskID, dataGridView1 );
                            updateTaskNoteData( td.TaskID );
                        }
                        readyUpdateTaskTransfer( td.TaskID );
                    }
                    editTaskNameDataList();
                    labelMsg.Text = "保存しました。現在、検索・更新モードです。";
                    buttonPrint.Enabled = true;
                    break;

                case "buttonUpdate":
                    if( btnNew ) return;
                    if( checkBoxConstStat.Checked ) return;
                    // 読込んでいた業務番号、バージョンのレコードのOldVerMarkを1（過去データ）にして更新
                    tp.UpdateOldVerMark( "D_Task", td.TaskID, 1 );
                    tp.UpdateOldVerMark( "D_TaskInd", td.TaskID, 1 );
                    tp.UpdateOldVerMark( "D_TaskNote", td.TaskID, 1 );
                    // 現在画面上にあるデータをバージョンを一つ上げ、OldVerMarkを0にして書き込む
                    initializeShadowTaskIndData();
                    newTaskID = insertNewVersionTaskData( "" );
                    addTaskIndData( newTaskID, dataGridView1 );
                    addTaskNoteData( newTaskID );
                    buttonUpdate.Enabled = false;
                    // 新たなバージョンが追加されたので、バージョン選択コンボボックスを再作成し、最後のバージョンを既定値にする
                    create_cbVersionNo(textBoxTaskCode.Text);
                    comboBoxVersionNo.SelectedIndex = comboBoxVersionNo.Items.Count - 1;                    

                    labelMsg.Text = "更新しました。現在、検索・更新モードです。";
                    break;

                case "buttonDelete":
                    if( btnNew ) return;
                    // 現在表示している業務番号のデータをOldVerMarkを1にして更新する。
                    tp.UpdateOldVerMark( "D_Task", td.TaskID, 1 );
                    tp.UpdateOldVerMark( "D_TaskInd", td.TaskID, 1 );
                    tp.UpdateOldVerMark( "D_TaskNote", td.TaskID, 1 );

                    editTaskNameDataList();

                    labelMsg.Text = "削除しました。現在、検索・更新モードです。";
                    break;

                case "buttonCancel":
                    createArray_Controls();
                    initializeScreen();
                    initializeComboBox();
                    resetTaskNameGroup();
                    labelMsg.Text = "入力を取り消しました。現在、検索・更新モードです。";
                    break;

                case "buttonEnd":
                    this.Close();
                    break;

                case "buttonPrint":
                    if( textBoxTaskCode.Text == null || textBoxTaskCode.Text == "" || textBoxTaskCode.Text == " " ) return;
                    TaskOp tod = collectPersonsData( dataGridView1 );
                    PublishTaskData pt = new PublishTaskData( Folder.DefaultExcelTemplate( BookName ) );
                    taskBaseCode = td.TaskBaseCode;
                    pt.ExcelFile( editTaskData( ( TaskData )td.Clone() ), editTaskNoteData( 0 ), editTaskIndData( 0, dataGridView1 ), pd, tod );
                    labelMsg.Text = "印刷しました。";
                    break;

                default:
                    break;
            }
        }


        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            // 20180628 asakawa 自動採番から手動採番へ改良のため一部変更

            TextBox tb = ( TextBox )sender;

            if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            {
                // 20180628 asakawa 以下追加
                if (btnNew) return;
                // 追加ここまでv

                if ( tb.Name == "textBoxTaskCode" )
                {
                    TaskIndData tidp = new TaskIndData();
                    int taskID = tidp.SelectTaskID( textBoxTaskCode.Text );
                    if( taskID < 0 ) return;
                    readyUpdateTaskTransfer( taskID );
                }
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if( e.KeyCode == Keys.A )
            {
                switch( tb.Name )
                {
                    case "textBoxTaskCode":
                        TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
                        if( tcds == null ) return;
                        readyUpdateTaskTransfer( tcds.TaskID );
                        break;

                    case "textBoxPartner":
                        PartnersScData psds = FormSubComList.ReceiveItems( psd );
                        if( psds == null ) return;
                        editViewPartnerData( psds );
                        break;

                    default:
                        break;
                }
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( ctrlChange ) return;
            if( btnNew ) return;

            ComboBox cb = ( ComboBox )sender;
            ListFormDataOp lo = new ListFormDataOp();
            switch( cb.Name )
            {
                case "comboBoxVersionNo":
                    if( holdVerNo == comboBoxVersionNo.Text ) return;
                    holdVerNo = comboBoxVersionNo.Text;

                    TaskIndData tidp = new TaskIndData();
                    int taskID = tidp.SelectTaskID( textBoxTaskCode.Text, Convert.ToInt32( comboBoxVersionNo.Text ) );
                    if( taskID < 0 ) return;
                    verChange = true;
                    readyUpdateTaskTransfer( taskID );
                    //if( loadTaskAndPartnerData( taskID ) )
                    //{
                    //    editControl( dataGridView1 );
                    //}
                    verChange = false;
                    break;

                case "comboBoxDepart":
                    if( !crtCBDepart )
                    {
                        if( !crtCBOffice ) create_cbOffice();
                        create_cbDepart();
                    }
                    editTaskNameDataList();
                    create_cbConfirm();
                    break;

                default:
                    break;
            }
        }


        private void comboBoxOffice_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( ctrlChange ) return;

            ComboBox cb = ( ComboBox )sender;
            ListFormDataOp lo = new ListFormDataOp();
            if( cb.Name != "comboBoxOffice" ) return;
            create_cbDepart();
            editTaskNameDataList();

            create_cbCostType( Convert.ToString( comboBoxOffice.SelectedValue ) );
            create_cbConfirm();
        }


        private void comboBox_TextChanged2( object sender, EventArgs e )
        {
            if( selTask ) return;
            if( ctrlChange ) return;
            if( cbChange ) return;

            ComboBox cb = ( ComboBox )sender;

            //this.SuspendLayout();
            cbChange = true;
            string officeString = string.Join( null, officeArray );
            for( int i = 0; i < cbMgrDev.Length; i++ )
            {
                if( cb.Name == cbMgrDev[i].Name )
                    create_cbMemberName( cbMgrNam[i], Convert.ToString( officeString[i] ), Convert.ToString( cbMgrDev[i].SelectedValue ) );
            }

            for (int i = 0; i < cbMbrDev.Length; i++)
            {
                if (cb.Name == cbMbrDev[i].Name)
                {
                    // 2018.10.20 仕様変更依頼に基づき、一部の選択肢を社員区分0,1、全社員に広げるために変更
                    // create_cbMemberName(cbMbrNam[i], Convert.ToString(officeString[i]), Convert.ToString(cbMbrDev[i].SelectedValue));
                    create_cbMemberName2(cbMbrNam[i], Convert.ToString(officeString[i]), Convert.ToString(cbMbrDev[i].SelectedValue));
                }
            }
            cbChange = false;
            //this.ResumeLayout();
        }


        private void dataGridView_CellValidated( object sender, DataGridViewCellEventArgs e )
        {
            if( selTask ) return;

            DataGridView dgv = ( DataGridView )sender;
            calcGridView( dgv );
            editControl( dgv );
        }


        private void dataGridView_KeyDown( object sender, KeyEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            if( e.KeyCode == Keys.Delete ) dgv.CurrentCell.Value = "";
        }


        private void checkBox_CheckedChanged( object sender, EventArgs e )
        {
            if( selTask ) return;
            CheckBox cbx = ( CheckBox )sender;
            switch( cbx.Name )
            {
                case "checkBoxIssue":
                    int para = cbx.Checked ? 0 : 1;
                    clearCnfArray( para );
                    break;

                case "checkBoxConstStat":
                    if(Convert.ToInt32(comboBoxVersionNo.Text) > 0 )
                    {
                        if(cbx.Checked)
                        {
                            MessageBox.Show( "仮着工へ変更することはできません。" );
                            cbx.Checked = false;
                        }
                    }
                    break;
                default:
                    break;
            }

            
        }


        private void textBox_TextChanged( object sender, EventArgs e )
        {
            if( selTask ) return;
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( selTask ) return;

            DateTimePicker dtp = ( DateTimePicker )sender;
            if( dtp.Name == "dateTimePickerEndDate" )
            {
                if( dateTimePickerStartDate.Value > dateTimePickerEndDate.Value )
                {
                    MessageBox.Show( "日付の指定が矛盾しています。終了日は開始日より前には指定できません。" );
                    dateTimePickerEndDate.Focus();
                    return;
                }
            }
        }
        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        private void createArray_Controls()
        {
            this.lbTitleL = new Label[] { this.labelL00, this.labelL01, this.labelL02, this.labelL03, this.labelL04, this.labelL05 };
            this.cbMgrDev = new ComboBox[] { this.comboBoxTaskMDev00, this.comboBoxTaskMDev01, this.comboBoxTaskMDev02, this.comboBoxTaskMDev03, this.comboBoxTaskMDev04, this.comboBoxTaskMDev05 };
            this.cbMgrNam = new ComboBox[] { this.comboBoxTaskMgr00, this.comboBoxTaskMgr01, this.comboBoxTaskMgr02, this.comboBoxTaskMgr03, this.comboBoxTaskMgr04, this.comboBoxTaskMgr05 };

            this.lbTitleR = new Label[] { this.labelR00, this.labelR01, this.labelR02, this.labelR03, this.labelR04, this.labelR05 };
            this.cbMbrDev = new ComboBox[] { this.comboBoxTaskDev00, this.comboBoxTaskDev01, this.comboBoxTaskDev02, this.comboBoxTaskDev03, this.comboBoxTaskDev04, this.comboBoxTaskDev05 };
            this.cbMbrNam = new ComboBox[] { this.comboBoxTaskMbr00, this.comboBoxTaskMbr01, this.comboBoxTaskMbr02, this.comboBoxTaskMbr03, this.comboBoxTaskMbr04, this.comboBoxTaskMbr05 };

            this.lbCnfT = new Label[] { this.labelT0, this.labelT1, this.labelT2, this.labelT3, this.labelT4 };
            this.lbCnfP = new Label[] { this.labelP0, this.labelP1, this.labelP2, this.labelP3, this.labelP4 };
            this.cbCnfNam = new ComboBox[] { this.comboBoxSalesMN, this.comboBoxInputNo, this.comboBoxApproval, this.comboBoxMakeOrder, this.comboBoxConfirm };
            this.PnCnfS = new Panel[] { this.panelS0, this.panelS1, this.panelS2, this.panelS3, this.panelS4 };
            this.dtCnfDat = new DateTimePicker[] { this.dateTimePickerSet0, this.dateTimePickerSet1, this.dateTimePickerSet2, this.dateTimePickerSet3, this.dateTimePickerSet4 };

            clearCbMbrArray();
            clearCnfArray( -1 );
        }


        private void clearCbMbrArray()
        {
            if( cbMbrDev == null ) return;
            for( int i = 0; i < cbMbrDev.Length; i++ )
            {
                lbTitleL[i].Text = "";
                cbMgrDev[i].Enabled = false;
                cbMgrNam[i].Enabled = false;
                cbMgrDev[i].Visible = false;
                cbMgrNam[i].Visible = false;

                lbTitleR[i].Text = "";
                cbMbrDev[i].Enabled = false;
                cbMbrNam[i].Enabled = false;
                cbMbrDev[i].Visible = false;
                cbMbrNam[i].Visible = false;
            }
        }


        private void clearCnfArray( int procLev )
        {
            panelG0.Visible = true;
            lbCnfT[0].Visible = true;
            lbCnfP[0].Visible = true;
            cbCnfNam[0].Visible = true;
            PnCnfS[0].Visible = true;
            dtCnfDat[0].Visible = true;

            panelG1.Visible = false;
            panelG2.Visible = false;
            for( int i = 1; i < lbCnfT.Length; i++ )
            {
                lbCnfT[i].Visible = false;
                lbCnfP[i].Visible = false;
                cbCnfNam[i].Visible = false;
                PnCnfS[i].Visible = false;
                dtCnfDat[i].Visible = false;
            }
            //if( procLev == 1 )
            if( procLev == 1 && certification() )
            {
                panelG1.Location = new Point( panelG1P[0] + AutoScrollPosition.X, panelG1P[1] + AutoScrollPosition.Y );
                panelG1.Visible = true;
                lbCnfT[1].Visible = true;
                lbCnfP[1].Visible = true;
                cbCnfNam[1].Visible = true;
                PnCnfS[1].Visible = true;
                dtCnfDat[1].Visible = true;
            }
            if( procLev == 0 )
            {
                panelG1.Location = new Point( panelG1P[0] + AutoScrollPosition.X, panelG1P[1] + AutoScrollPosition.Y );
                panelG2.Location = new Point( panelG2P[0] + AutoScrollPosition.X, panelG2P[1] + AutoScrollPosition.Y );
                panelG1.Visible = true;
                panelG2.Visible = true;
                for( int i = 1; i < lbCnfT.Length; i++ )
                {
                    lbCnfT[i].Visible = true;
                    lbCnfP[i].Visible = true;
                    cbCnfNam[i].Visible = true;
                    PnCnfS[i].Visible = true;
                    dtCnfDat[i].Visible = true;
                }
            }
        }


        private void clearStringArray()
        {
            for( int i = 0; i < dimCnt; i++ )
            {
                officeArray[i] = "";
                deptArray[i] = "";
                tiIDArray[i] = "";
                dListArray[i] = "";
                mgrArray[i] = "";
                mbrArray[i] = "";
            }
        }

        private void initializeScreen()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvReadyNoRHeader();
            ui.DgvRowsHeight( 30 );
            ui.DgvNotResize();

            comboBoxCostType.Text = "";
            comboBoxVersionNo.Text = "";
            comboBoxLevel.Text = "";
            comboBoxOrdersForm.Text = "";
            comboBoxClaimForm.Text = "";

            labelTaskCode.Text = "";
            labelIssueDate.Text = "";
            textBoxTaskCode.Text = "";
            textBoxTaskName.Text = "";
            textBoxTaskPlace.Text = "";
            textBoxPartner.Text = "";
            labelPAddress.Text = "";
            labelPTelNo.Text = "";
            labelPFaxNo.Text = "";
            textBoxPayNote.Text = "";
            textBoxTaskPlace.Text = "";
            textBoxTaskOffice.Text = "";
            textBoxTaskLeader.Text = "";
            textBoxTelNo.Text = "";
            textBoxFaxNo.Text = "";
            textBoxEMail.Text = "";
            textBoxOrderNote.Text = "";
            textBoxSpecCont.Text = "";
            textBoxAttOtherCont.Text = "";
            textBoxNote.Text = "";

            dateTimePickerStartDate.Value = DateTime.Today;
            dateTimePickerEndDate.Value = DateTime.Today;

            reverseEnabledProperty( "init" );

            textBoxAttOtherCont.Text = "";

            textBoxOrderNote.Text = "";
            textBoxSpecCont.Text = "";

            labelPartnerCode.Text = "";

            labelCostType.Visible = false;
            comboBoxCostType.Visible = false;

            labelMsg.Text = "";
            labelIssue.Text = "";

            clearCheckBox();
            clearCbMbrArray();
            clearStringArray();
            clearDataGridView();
        }


        private void clearDataGridView()
        {
            for( int i = 0; i < dataGridView1.Rows.Count; i++ )
            {
                for( int j = 0; j < dataGridView1.Columns.Count; j++ )
                {
                    dataGridView1.Rows[i].Cells[j].Value = "";
                }
            }
        }


        private void clearCheckBox()
        {
            checkBoxConstStat.Checked = false;
            checkBoxAttProceed.Checked = false;
            checkBoxAttEstimate.Checked = false;
            checkBoxAttDesign.Checked = false;
            checkBoxAttContract.Checked = false;
            checkBoxAttOrder.Checked = false;
            checkBoxAttStart.Checked = false;
            checkBoxAttOther.Checked = false;
            checkBoxCommonSpec.Checked = false;
            checkBoxExclusiveSpec.Checked = false;
            checkBoxOtherSpec.Checked = false;
            checkBoxOrderBudget.Checked = false;
            checkBoxOrderPlanning.Checked = false;
            checkBoxOrdersType.Checked = false;
        }


        // comboBox作成
        private void initializeComboBox()
        {
            create_cbOffice();
            create_cbDepart();
            
            create_cbLevel();
            create_cbCostType( hp.OfficeCode );
            create_cbOForm();
            create_cbCForm();
            create_cbConfirm();

            create_cbOrderNote();
        }


        private void resetTaskNameGroup()
        {
            comboBoxCostType.Visible = false;
            textBoxTaskName.Visible = true;
            this.comboBoxCostType.Location = new Point( cBCostTypeP[0] + AutoScrollPosition.X, cBCostTypeP[1] + AutoScrollPosition.Y );
            this.textBoxTaskName.Location = new Point( tBTaskNameP[0] + AutoScrollPosition.X, tBTaskNameP[1] + AutoScrollPosition.Y );
            //this.comboBoxCostType.Location = new Point( 982 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            //this.textBoxTaskName.Location = new Point( 116 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            btnNew = false;
        }


        // 事業所
        private void create_cbOffice()
        {
            crtCBOffice = true;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
            comboBoxOffice.SelectedValue = hp.OfficeCode;        // 初期値
        }


        private void create_cbDepart()
        {
            crtCBDepart = true;
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            comboBoxDepart.SelectedIndex = ( comboBoxOffice.Text == Sign.HQOffice ) ? 1 : 0;
            //if( hp.OfficeCode == Sign.HQOfficeCode )
            if( Convert.ToString(comboBoxOffice.SelectedValue) == Sign.HQOfficeCode )
            {
                if(hp.Department == "0" || hp.Department == "8" || hp.Department == "9" )
                {
                    comboBoxDepart.SelectedValue = "2";        // 初期値
                }
                else
                {
                    comboBoxDepart.SelectedValue = hp.Department;        // 初期値
                }
            }
            else
            {
                comboBoxDepart.SelectedValue = "8";        // 初期値
            }

            if( crtCBOffice ) readyCB = true;
        }


        private void create_cbLevel()
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxLevel );
            cb.DisplayItem = new string[] { "一般管理", "重要管理" };
            cb.Basic();
        }


        private void create_cbVersionNo( string taskCode )
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxVersionNo );
            if( !cb.VersionDistinct( "D_TaskInd", "WHERE TaskCode = '" + taskCode + "'" ) )
            {
                comboBoxVersionNo.Text = "-";
            }
        }


        private void create_cbCostType( string officeCode )
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxCostType );
            cb.ComDataList( "COST", officeCode );
        }


        private void create_cbOForm()
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxOrdersForm );
            cb.DisplayItem = new string[] { "請負", "常傭" };
            cb.Basic();
        }


        private void create_cbCForm()
        {
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxClaimForm );
            cb.DisplayItem = new string[] { "月次", "完成" };
            cb.Basic();
        }


        private void create_cbConfirm()
        {
            if( !readyCB ) return;
            CommonData comd = new CommonData();
            MembersData md = new MembersData();
            string[] memberArray;
            string officeCode = Convert.ToString(comboBoxOffice.SelectedValue);
            string departCode = Convert.ToString(comboBoxDepart.SelectedValue);
            departCode = (officeCode == Sign.HQOfficeCode && departCode == "0") ? "2" : departCode;
            if( string.IsNullOrEmpty( departCode ) )
            {
                departCode = ( officeCode == Sign.HQOfficeCode ) ? "2": "8";
            }
            if(departCode == "9") departCode = "8";

            for( int i = 0; i < cbCnfNam.Length; i++ )
            {
                switch( i )
                {
                    case 0:
                        // 2018.10.20 仕様変更依頼に基づき、一部の選択肢を社員区分0,1、全社員に広げるために変更
                        // create_cbMemberName( cbCnfNam[i], officeCode, departCode );
                        create_cbMemberName2(cbCnfNam[i], officeCode, departCode);
                        cbCnfNam[0].SelectedValue = hp.MemberCode;
                        break;
                    case 1:
                        memberArray = comd.SelctDefaultMemberAll( Sign.NameTaskTransfer, "InputNumber" );
                        editCbName( cbCnfNam[i], memberArray );
                        break;
                    case 2:
                        memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "Approval", officeCode, null );
                        editCbName( cbCnfNam[i], memberArray );
                        break;
                    case 3:
                        memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "MakeOrder", officeCode, departCode );
                        editCbName( cbCnfNam[i], memberArray );
                        break;
                    case 4:
                        memberArray = comd.SelctDefaultMemberMulti( Sign.NameTaskTransfer, "ConfirmAdm", officeCode, departCode );
                        editCbName( cbCnfNam[i], memberArray );
                        break;
                }
            }

        }


        private void editCbName( ComboBox cb, string[] memberArray )
        {
            ComboBoxEdit cbe = new ComboBoxEdit( cb );
            MembersData md = new MembersData();
            cbe.ValueItem = new string[memberArray.Length];
            cbe.DisplayItem = new string[memberArray.Length];
            for( int j = 0; j < memberArray.Length; j++ )
            {
                cbe.ValueItem[j] = memberArray[j];
                cbe.DisplayItem[j] = md.SelectMemberName( memberArray[j] );
            }
            cbe.Basic();
        }


        private void create_cbDeptName( ComboBox cbName, string officeCode, string depart, string initial )
        {
            ComboBoxEdit cb = new ComboBoxEdit( cbName );

            if( officeCode == "H" )
            {
                switch( depart )
                {
                    case "2":
                        cb.DisplayItem = new string[] { "測量" };
                        cb.ValueItem = new string[] { "2" };
                        break;
                    case "1":
                        cb.DisplayItem = new string[] { "設計" };
                        cb.ValueItem = new string[] { "1" };
                        break;
                    case "7":
                        cb.DisplayItem = new string[] { "調査" };
                        cb.ValueItem = new string[] { "7" };
                        break;
                    default:
                        break;
                }
                if( initial == "F" )
                {
                    cb.DisplayItem = new string[] { "測量", "設計", "調査" };
                    cb.ValueItem = new string[] { "2", "1", "7" };
                }
            }
            else
            {
                cb.DisplayItem = new string[] { "技術" };
                cb.ValueItem = new string[] { "8" };
            }
            cb.Basic();
        }


        // 2018.10.20 仕様変更依頼に基づき、一部の選択肢を社員区分0,1、全社員に広げる
        private void create_cbMemberName( ComboBox cbName, string officeCode, string depart )
        {
            ComboBoxEdit cb = new ComboBoxEdit( cbName );

            string wParam = " WHERE MemberType = 0 AND EnrollMent = 0 AND OfficeCode = '" + officeCode + "'";
            if (officeCode == "H" ) wParam += " AND Department = '" + depart + "'";

            if( cbName.Name == "comboBoxSalesMN" && hp.Department != Convert.ToString(comboBoxDepart.SelectedValue) )
            {

                cb.TableData("M_Members", "MemberCode", "Name", hp.MemberName, hp.MemberCode, wParam);
            }
            else
            {
                cb.TableData( "M_Members", "MemberCode", "Name", wParam );
            }
        }

        // 2018.10.20 仕様変更依頼に基づき、一部の選択肢を社員区分0,1、全社員に広げるために追加
        private void create_cbMemberName2(ComboBox cbName, string officeCode, string depart)
        {
            ComboBoxEdit cb = new ComboBoxEdit(cbName);

            string wParam = " WHERE (MemberType = 0 OR MemberType = 1) AND EnrollMent = 0 ORDER BY OfficeCode, Department";
            cb.TableData("M_Members", "MemberCode", "Name", wParam);

        }



        private void create_cbOrderNote()
        {
            this.cbONote = new ComboBox[] { this.comboBoxONote0, this.comboBoxONote1, this.comboBoxONote2 };
            for(int i = 0;i < cbONote.Length;i++ )
            {
                ComboBoxEdit cbe = new ComboBoxEdit( cbONote[i] );
                CommonData comd = new CommonData();
                cbe.DisplayItem = comd.SelctOrderNote( "Level" + i.ToString() );
                cbe.Basic();
            }
        }


        private void editControl( DataGridView dgv )
        {
            ctrlChange = true;
            // 初期化
            clearCbMbrArray();

            // DataGridViewを全Cellチェック
            int idx = 0;
            int seq = 0;
            for( int i = 0; i < rows; i++ )
            {
                for( int j = 0; j < cols; j++ )
                {
                    if( string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells[j].Value ) ) ||
                        string.IsNullOrWhiteSpace( Convert.ToString( dgv.Rows[i].Cells[j].Value ) ) )
                    {
                        officeArray[seq] = "";
                        deptArray[seq] = "";
                        dListArray[seq] = "";
                    }
                    else
                    {
                        string work = Convert.ToString( dgv.Rows[i].Cells[j].Value );
                        lbTitleL[idx].Text = Conv.bList[j] + " " + Conv.taskList[i];
                        lbTitleR[idx].Text = Conv.bList[j] + " " + Conv.taskList[i];
                        dListArray[seq] = Conv.dList[i];
                        if( string.IsNullOrEmpty( officeArray[seq] ) ) officeArray[seq] = Conv.oList[j];
                        if( string.IsNullOrEmpty( deptArray[seq] ) )
                        {
                            deptArray[seq] = ( officeArray[seq] == "H" ) ? Conv.tdHList[Conv.DepartmentCodeIndex( Conv.dList[i] )] : "8";
                        }
                        idx++;
                    }
                    seq++;
                }
            }

            // 全Cellチェック結果に基づきComboBox編集、表示
            idx = 0;
            for( int i = 0; i < dimCnt; i++ )
            {
                if( !string.IsNullOrEmpty( officeArray[i] ) )
                {
                    cbMgrDev[idx].Enabled = true;
                    cbMbrDev[idx].Enabled = true;
                    cbMgrDev[idx].Visible = true;
                    cbMbrDev[idx].Visible = true;
                    create_cbDeptName( cbMgrDev[idx], officeArray[i], deptArray[i], dListArray[i] );
                    create_cbDeptName( cbMbrDev[idx], officeArray[i], deptArray[i], dListArray[i] );

                    cbMgrNam[idx].Enabled = true;
                    cbMbrNam[idx].Enabled = true;
                    cbMgrNam[idx].Visible = true;
                    cbMbrNam[idx].Visible = true;
                    create_cbMemberName( cbMgrNam[idx], officeArray[i], deptArray[i] );
                    // 2018.10.20 仕様変更依頼に基づき、一部の選択肢を社員区分0,1、全社員に広げるために変更
                    // create_cbMemberName( cbMbrNam[idx], officeArray[i], deptArray[i] );
                    create_cbMemberName2(cbMbrNam[idx], officeArray[i], deptArray[i]);

                    // 初期値設定
                    cbMgrDev[idx].Text = Conv.DepartName( officeArray[i], deptArray[i] );
                    cbMbrDev[idx].Text = Conv.DepartName( officeArray[i], deptArray[i] );
                    string wkMgrDev = Convert.ToString( cbMgrDev[idx].SelectedValue );
                    string wkMbrDev = Convert.ToString( cbMbrDev[idx].SelectedValue );
                    setDefaultMember( cbMgrNam[idx], officeArray[i], deptArray[i] );
                    setDefaultMember( cbMbrNam[idx], officeArray[i], deptArray[i] );
                    if( !string.IsNullOrEmpty( mgrArray[i] ) ) cbMgrNam[idx].SelectedValue = mgrArray[i];
                    ///////////////////////////////////////////////////////////////////////////////////////
                    if( !string.IsNullOrEmpty( mbrArray[i] ) ) cbMbrNam[idx].SelectedValue = mbrArray[i];
                    idx++;
                }
            }
            ctrlChange = false;
        }


        private void setDefaultMember( ComboBox cb, string office, string depart )
        {
            StringUtility su = new StringUtility();
            CommonData comd = new CommonData();
            MembersData md = new MembersData();
            switch( su.SubstringByte( cb.Name, 8, 7 ) )
            {
                case "TaskMgr":
                    ComboBoxEdit cbe = new ComboBoxEdit( cb );
                    cbe.ValueItem = new string[1];
                    cbe.DisplayItem = new string[1];
                    cbe.ValueItem[0] = comd.SelctDefaultMember( Sign.NameTaskTransfer, "AdminCode", office, depart );
                    cbe.DisplayItem[0] = md.SelectMemberName( cbe.ValueItem[0] );
                    cbe.Basic();
                    break;
                case "TaskMbr":
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "LeaderMCode", office, depart );
                    break;
                case "SalesMN":
                    if( !selTask ) cb.SelectedValue = td.SalesMCode;
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "SalesMCode", office, null );
                    break;
                case "Approva":
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "Approval", office, null );
                    break;
                case "MakeOrd":
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "MakeOrder", office, depart );
                    break;
                case "InputNo":
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "InputNumber", office, depart );
                    break;
                case "Confirm":
                    //cb.SelectedValue = comd.SelctDefaultMember( Sign.NameTaskTransfer, "ConfirmAdm", office, depart );
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// アプリで扱うTask（業務）とPartner（顧客）のデータをDBから得る
        /// </summary>
        /// <param name="taskID"></param>
        private bool loadTaskAndPartnerData( int taskID )
        {
            td = new TaskData();
            td = td.SelectTaskData( taskID );
            if( td == null ) return false;

            TaskIndData tidp = new TaskIndData();
            tid = tidp.SelectTaskIndData( taskID );

            tn = new TaskNoteData();
            tn = tn.SelectTaskNoteDate( taskID );

            pd = new PartnersData();
            pd = pd.SelectPartnersData( td.PartnerCode );

            editViewTaskData();

            return true;
        }


        // DBより得たデータを編集・表示する
        private void editViewTaskData()
        {
            textBoxTaskCode.Text = tid[0].TaskCode;
            textBoxTaskName.Text = tid[0].TaskName;

            if( !verChange ) create_cbVersionNo( tid[0].TaskCode );

            textBoxPartner.Text = ( pd == null || string.IsNullOrEmpty( pd.PartnerName ) ) ? "" : pd.PartnerName;
            labelPartnerCode.Text = ( pd == null || string.IsNullOrEmpty( td.PartnerCode ) ) ? "" : td.PartnerCode;
            labelIssueDate.Text = td.IssueDate.ToString( "yyyy年MM月dd日" ) + " 発行";
            textBoxTaskPlace.Text = td.TaskPlace;
            if( td.StartDate > DateTime.MinValue ) dateTimePickerStartDate.Value = td.StartDate;
            if( td.EndDate > DateTime.MinValue ) dateTimePickerEndDate.Value = td.EndDate;
            textBoxPayNote.Text = td.PayNote;
            textBoxTaskOffice.Text = td.TaskOffice;
            textBoxTaskLeader.Text = td.TaskLeader;
            textBoxTelNo.Text = td.TelNo;
            textBoxFaxNo.Text = td.FaxNo;
            textBoxEMail.Text = td.EMail;

            if( !string.IsNullOrEmpty( td.AttProceed.ToString() ) ) checkBoxAttProceed.Checked = td.AttProceed > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.AttEstimate.ToString() ) ) checkBoxAttEstimate.Checked = td.AttEstimate > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.AttDesign.ToString() ) ) checkBoxAttDesign.Checked = td.AttDesign > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.AttContract.ToString() ) ) checkBoxAttContract.Checked = td.AttContract > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.AttOrder.ToString() ) )
            {
                if( td.AttOther > 0 )
                {
                    checkBoxAttOther.Checked = true;
                    textBoxAttOtherCont.Text = td.AttOtherCont;
                }
            }

            textBoxOrderNote.Text = td.OrderNote;
            if( !string.IsNullOrEmpty( td.CommonSpec.ToString() ) ) checkBoxCommonSpec.Checked = td.CommonSpec > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.ExclusiveSpec.ToString() ) ) checkBoxExclusiveSpec.Checked = td.ExclusiveSpec > 0 ? true : false;
            if( !string.IsNullOrEmpty( td.OtherSpec.ToString() ) )
            {
                if( td.OtherSpec > 0 )
                {
                    checkBoxOtherSpec.Checked = true;
                    textBoxSpecCont.Text = td.SpecCont;
                }
            }

            MembersData md = new MembersData();
            if( !string.IsNullOrEmpty( td.SalesMCode ) )
            {
                cbCnfNam[0].SelectedValue = td.SalesMCode;
                cbCnfNam[0].Text = md.SelectMemberName( td.SalesMCode );
                //if( td.SalesMInputDate != DateTime.MinValue ) dtCnfDat[0].Value = td.SalesMInputDate;
                dtCnfDat[0].Value = ( td.SalesMInputDate == DateTime.MinValue ) ? td.IssueDate : td.SalesMInputDate;
            }

            clearCnfArray( td.IssueMark );
            if( td.IssueMark == 0 )
            {
                if( !string.IsNullOrEmpty( td.InputNumber ) ) cbCnfNam[1].SelectedValue = td.InputNumber;
                //if( td.InputNoDate != DateTime.MinValue ) dtCnfDat[1].Value = td.InputNoDate;
                dtCnfDat[1].Value = ( td.InputNoDate == DateTime.MinValue ) ? td.IssueDate : td.InputNoDate;

                if( !string.IsNullOrEmpty( td.Approval ) ) cbCnfNam[2].SelectedValue = td.Approval;
                //if( td.ApprovalDate != DateTime.MinValue ) dtCnfDat[2].Value = td.ApprovalDate;
                dtCnfDat[2].Value = ( td.ApprovalDate == DateTime.MinValue ) ? td.IssueDate : td.ApprovalDate;

                if( !string.IsNullOrEmpty( td.MakeOrder ) ) cbCnfNam[3].SelectedValue = td.MakeOrder;
                //if( td.MakeOrderDate != DateTime.MinValue ) dtCnfDat[3].Value = td.MakeOrderDate;
                dtCnfDat[3].Value = ( td.MakeOrderDate == DateTime.MinValue ) ? td.IssueDate : td.MakeOrderDate;

                if( !string.IsNullOrEmpty( td.ConfirmAdm ) ) cbCnfNam[4].SelectedValue = td.ConfirmAdm;
                //if( td.ConfirmDate != DateTime.MinValue ) dtCnfDat[4].Value = td.ConfirmDate;
                dtCnfDat[4].Value = ( td.ConfirmDate == DateTime.MinValue ) ? td.IssueDate : td.ConfirmDate;

                checkBoxIssue.Checked = true;
                labelIssue.Text = "";
            }
            else
            {
                if( !string.IsNullOrEmpty( td.InputNumber ) ) cbCnfNam[1].SelectedValue = td.InputNumber;
                checkBoxIssue.Checked = false;
                dtCnfDat[1].Value = DateTime.Today;
                labelIssue.Text = "仮登録 承認待ち";
            }

            if( !string.IsNullOrEmpty( td.OrderBudget.ToString() ) ) checkBoxOrderBudget.Checked = ( td.OrderBudget > 0 ) ? true : false;
            if( !string.IsNullOrEmpty( td.OrderPlanning.ToString() ) ) checkBoxOrderPlanning.Checked = ( td.OrderPlanning > 0 ) ? true : false;
            if( !string.IsNullOrEmpty( td.ConstructionStat.ToString() ) ) checkBoxConstStat.Checked = ( td.ConstructionStat > 0 ) ? true : false;
            comboBoxLevel.SelectedValue = td.AdmLevel;
            comboBoxOrdersForm.SelectedValue = td.OrdersForm;
            comboBoxClaimForm.SelectedValue = td.ClaimForm;

            textBoxNote.Text = ( tn == null || string.IsNullOrEmpty( tn.Note ) ) ? "" : tn.Note;

            if( !string.IsNullOrEmpty( td.OrdersType.ToString() ) ) checkBoxOrdersType.Checked = ( td.OrdersType > 0 ) ? true : false;

            editGridView( dataGridView1 );
            calcGridView( dataGridView1 );
        }


        private void editGridView( DataGridView dgv )
        {
            initializeShadowTaskIndData();

            tiIDArray = new string[rows * cols];
            officeArray = new string[rows * cols];
            deptArray = new string[rows * cols];
            mgrArray = new string[rows * cols];
            mbrArray = new string[rows * cols];
            
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );

            labelTaskCode.Text = "";
            int ridx;
            int cidx;
            string contractPay;
            totalProvMark = 0;
            for( int i = 0; i < tid.Length; i++ )
            {
                if( !verChange && i == 0 ) comboBoxVersionNo.Text = Convert.ToString( tid[0].VersionNo );

                if( labelTaskCode.Text == "" )
                {
                    labelTaskCode.Text = tid[i].TaskCode + "(" + tid[i].OfficeCode + ")";
                }
                else
                {
                    labelTaskCode.Text += "　、" + tid[i].TaskCode + "(" + tid[i].OfficeCode + ")";
                }

                ridx = Conv.dList.IndexOf( tid[i].TaskCode[0].ToString() );
                cidx = Conv.oList.IndexOf( tid[i].OfficeCode );

                // 2018.01 asakawa エラー対策
                if (cidx < 0)
                {
                    cidx = 0;
                    continue;
                }
                // 2018.01 //

                contractPay = decFormat( tid[i].Contract );
                if( tid[i].ProvMark == 1 )
                {
                    dgv.Rows[ridx].Cells[cidx].Value = "仮 " + contractPay;
                    totalProvMark++;
                }
                else
                {
                    dgv.Rows[ridx].Cells[cidx].Value = contractPay;
                }

                tiIDArray[ridx * 4 + cidx] = Convert.ToString( tid[i].TaskIndID );
                officeArray[ridx * 4 + cidx] = Convert.ToString( tid[i].OfficeCode );
                deptArray[ridx * 4 + cidx] = Convert.ToString( tid[i].Department );
                mgrArray[ridx * 4 + cidx] = Convert.ToString( tid[i].AdminCode );
                mbrArray[ridx * 4 + cidx] = Convert.ToString( tid[i].LeaderMCode );

                sTID[ridx * 4 + cidx] = (TaskIndData)tid[i].Clone();
            }
        }


        private void calcGridView( DataGridView dgv )
        {
            char[] removeChars = new char[] { '仮', ',', ' ' };
            string formWk;
            decimal sumH = 0M;
            int hCnt = 0;
            decimal[] sumV = new decimal[cols];
            int[] vCnt = new int[cols];
            for( int i = 0; i < rows; i++ )
            {
                for( int j = 0; j < cols; j++ )
                {
                    formWk = Convert.ToString( dgv.Rows[i].Cells[j].Value );
                    if( string.IsNullOrEmpty( formWk ) ) continue;
                    hCnt++;
                    vCnt[j]++;
                    if( formWk.IndexOf( "仮" ) > -1 )
                    {
                        if( totalProvMark == 0 ) totalProvMark = 1;      // 「仮」の文字が見つかったとき(0以上)はTotalProvMarkを1にする
                    }
                    foreach( char c in removeChars )
                    {
                        formWk = formWk.Replace( c.ToString(), "" );
                    }
                    if( DHandling.IsDecimal( formWk ) )
                    {
                        sumH += Convert.ToDecimal( formWk );
                        sumV[j] += Convert.ToDecimal( formWk );

                        formWk = decFormat( Convert.ToDecimal( formWk ) );
                        if( totalProvMark > 0 ) formWk = "仮 " + formWk;
                        dgv.Rows[i].Cells[j].Value = formWk;
                    }
                    else
                    {
                        MessageBox.Show( "入力エラー" );
                        return;
                    }
                }
                //横計
                if( hCnt == 0 )
                {
                    dgv.Rows[i].Cells[cols].Value = "";
                }
                else
                {
                    formWk = decFormat( sumH );
                    if( totalProvMark > 0 ) formWk = "仮 " + formWk;
                    dgv.Rows[i].Cells[cols].Value = formWk;
                }
                hCnt = 0;
                sumH = 0;
            }
            // 縦計
            for( int j = 0; j < cols; j++ )
            {
                if( vCnt[j] == 0 )
                {
                    dgv.Rows[rows].Cells[j].Value = "";
                }
                else
                {
                    if( sumV[j] > 0 )
                    {
                        formWk = decFormat( sumV[j] );
                        if( totalProvMark > 0 ) formWk = "仮 " + formWk;
                        dgv.Rows[rows].Cells[j].Value = formWk;
                        sumH += sumV[j];
                        hCnt += vCnt[j];
                    }
                }
            }
            // 総計
            if( hCnt != 0 )
            {
                formWk = decFormat( sumH );
                if( totalProvMark > 0 ) formWk = "仮 " + formWk;
                dgv.Rows[rows].Cells[cols].Value = formWk;
            }
        }


        private void editViewPartnerData( PartnersData psds )
        {
            textBoxPartner.Text = psds.PartnerName;
            labelPartnerCode.Text = psds.PartnerCode;
            if( !string.IsNullOrEmpty( psds.Address ) ) labelPAddress.Text = "住所 : " + psds.Address;
            if( !string.IsNullOrEmpty( psds.TelNo ) ) labelPTelNo.Text = "TEL : " + psds.TelNo;
            if( !string.IsNullOrEmpty( psds.FaxNo ) ) labelPFaxNo.Text = "FAX : " + psds.FaxNo;
        }


        private int insertNewVersionTaskData( string newTaskBaseCode )
        {
            int curVerNo = 0;

            if(td.TaskID > 0 )
            {
                TaskIndData taskIndData = new TaskIndData();
                curVerNo = taskIndData.CurrentVersionNo( td.TaskID );
            }

            TaskData std = new TaskData();
            if( td != null )
            {
                std = ( TaskData )td.Clone();
                taskBaseCode = td.TaskBaseCode;
                versionNo = td.VersionNo;
            }

            std = editTaskData( std );

            if( newTaskBaseCode == "" )          // VerUpで新規VerのInsert処理用
            {
                std.ConstructionStat = 0;       // 本着工状態
                //versionNo++;
                versionNo = curVerNo + 1;
            }
            else
            {
                std.TaskBaseCode = newTaskBaseCode;
                if( checkBoxConstStat.Checked )
                {
                    std.ConstructionStat = 1;   // 仮着工状態
                    versionNo = 0;
                }
                else
                {
                    std.ConstructionStat = 0;   // 本着工状態
                    versionNo = 1;
                }
            }

            std.VersionNo = versionNo;
            std.IssueMark = 1;                  // 新規登録時（=承認待）は仮発行とする
            return std.InsertTaskData();
        }


        private bool updateNowVersionTaskData()
        {
            taskBaseCode = td.TaskBaseCode;
            versionNo = td.VersionNo;

            TaskData std = new TaskData();
            td.IssueMark = checkBoxIssue.Checked ? 0 : 1;
            std = editTaskData( ( TaskData )td.Clone() );
            return std.UpdateTaskData();
        }


        private TaskData editTaskData( TaskData std )
        {
            std.AdmLevel = Convert.ToInt32( comboBoxLevel.SelectedValue );
            if( btnNew ) std.IssueDate = DateTime.Today.StripTime();
            std.TaskName = textBoxTaskName.Text;
            std.TaskName = btnNew ? Convert.ToString( comboBoxCostType.SelectedValue ) : "";
            std.TaskName += textBoxTaskName.Text;
            textBoxTaskName.Text = std.TaskName;
            std.StartDate = dateTimePickerStartDate.Value.StripTime();
            std.EndDate = dateTimePickerEndDate.Value.StripTime();
            std.TaskPlace = textBoxTaskPlace.Text;
            std.PartnerCode = labelPartnerCode.Text;
            std.PayNote = textBoxPayNote.Text;
            std.TaskOffice = textBoxTaskOffice.Text;
            std.TaskLeader = textBoxTaskLeader.Text;
            std.TelNo = textBoxTelNo.Text;
            std.FaxNo = textBoxFaxNo.Text;
            std.EMail = textBoxEMail.Text;
            std.AttProceed = ( checkBoxAttProceed.Checked ) ? 1 : 0;
            std.AttEstimate = ( checkBoxAttEstimate.Checked ) ? 1 : 0;
            std.AttDesign = ( checkBoxAttDesign.Checked ) ? 1 : 0;
            std.AttContract = ( checkBoxAttContract.Checked ) ? 1 : 0;
            std.AttOrder = ( checkBoxAttOrder.Checked ) ? 1 : 0;
            std.AttStart = ( checkBoxAttStart.Checked ) ? 1 : 0;
            std.AttOther = ( checkBoxAttOther.Checked ) ? 1 : 0;
            std.AttOtherCont = textBoxAttOtherCont.Text;
            std.OrderNote = textBoxOrderNote.Text;
            std.CommonSpec = ( checkBoxCommonSpec.Checked ) ? 1 : 0;
            std.ExclusiveSpec = ( checkBoxExclusiveSpec.Checked ) ? 1 : 0;
            std.OtherSpec = ( checkBoxOtherSpec.Checked ) ? 1 : 0;
            std.SpecCont = textBoxSpecCont.Text;

            std.SalesMCode = Convert.ToString( cbCnfNam[0].SelectedValue );         // 作成 - 営業担当者
            std.InputNumber = Convert.ToString( cbCnfNam[1].SelectedValue );        // 業務番号入力 - 営業事務
            std.Approval = Convert.ToString( cbCnfNam[2].SelectedValue );           // 着工承認 - 事業所長
            std.MakeOrder = Convert.ToString( cbCnfNam[3].SelectedValue );          // 作成指示 - 担当役員
            std.ConfirmAdm = Convert.ToString( cbCnfNam[4].SelectedValue );         // 確認 - 営業業務管理責任者

            std.SalesMInputDate = dtCnfDat[0].Value.StripTime();
            std.InputNoDate = dtCnfDat[1].Value.StripTime();
            std.ApprovalDate = dtCnfDat[2].Value.StripTime();
            std.MakeOrderDate = dtCnfDat[3].Value.StripTime();
            std.ConfirmDate = dtCnfDat[4].Value.StripTime();

            std.OrderBudget = ( checkBoxOrderBudget.Checked ) ? 1 : 0;
            std.OrderPlanning = ( checkBoxOrderPlanning.Checked ) ? 1 : 0;
            std.CostType = btnNew ? Convert.ToString( comboBoxCostType.SelectedValue ) : "";
            std.ConstructionStat = ( checkBoxConstStat.Checked ) ? 1 : 0;
            std.OrdersForm = Convert.ToInt32( comboBoxOrdersForm.SelectedValue );
            std.ClaimForm = Convert.ToInt32( comboBoxClaimForm.SelectedValue );
            std.OldVerMark = 0;
            std.IssueMark = checkBoxIssue.Checked ? 0 : 1;
            std.OrdersType = checkBoxOrdersType.Checked ? 1 : 0;

            return std;
        }


        private void addTaskIndData( int taskID, DataGridView dgv )
        {
            if( taskID < 1 ) return;

            TaskIndData[] stid = editTaskIndData( taskID, dgv );
            if( stid.Length == 0 || stid == null ) return;

            TaskIndData tidp = new TaskIndData();
            tidp.InsertTaskIndData( stid );
        }


        private void updateTaskIndData( int taskID, DataGridView dgv )
        {
            TaskIndData[] stid = editTaskIndData( taskID, dgv );

            TaskIndData tidp = new TaskIndData();
            tidp.UpdateOrInsertOrDeleteTaskIndData( stid );
        }


        private TaskIndData[] editTaskIndData( int taskID, DataGridView dgv )
        {
            TaskIndData[] nTID = new TaskIndData[rows * cols];
            MembersData md = new MembersData();
            char[] removeChars = new char[] { '仮', ',', ' ' };
            string formWk;
            int idx = 0;
            int dataIdx = 0;
            for( int i = 0; i < rows; i++ )
            {
                for( int j = 0; j < cols; j++ )
                {
                    idx = i * 4 + j;
                    nTID[idx] = new TaskIndData();

                    formWk = Convert.ToString( dgv.Rows[i].Cells[j].Value ).Trim();
                    if( string.IsNullOrEmpty( formWk.Trim() ))
                    {
                        formWk = "-1";
                    }
                    else
                    {
                        if( formWk.Contains( "仮" ) ) nTID[idx].ProvMark = 1;
                        foreach( char c in removeChars )
                        {
                            formWk = formWk.Replace( c.ToString(), "" );
                        }

                        if( DHandling.IsDecimal( formWk ) )
                        {
                            nTID[idx].Contract = Convert.ToDecimal( formWk );
                            nTID[idx].TaskIndID = ( string.IsNullOrEmpty( tiIDArray[idx] ) ) ? 0 : Convert.ToInt32( tiIDArray[idx] );
                            nTID[idx].TaskCode = Conv.dList[i] + taskBaseCode;
                            if( idx == 0 ) textBoxTaskCode.Text = nTID[idx].TaskCode;
                            nTID[idx].TaskName = textBoxTaskName.Text;
                            nTID[idx].OfficeCode = Conv.oList[j];
                            nTID[idx].VersionNo = versionNo;
                            nTID[idx].AdminCode = Convert.ToString( cbMgrNam[dataIdx].SelectedValue );
                            nTID[idx].LeaderMCode = Convert.ToString( cbMbrNam[dataIdx].SelectedValue );
                            nTID[idx].ConfirmDateA = dtToday;
                            nTID[idx].ConfirmDateC = dtToday;
                            nTID[idx].Department = ( nTID[idx].OfficeCode == "H" ) ? Convert.ToString( cbMbrDev[dataIdx].SelectedValue ) : "8";
                            nTID[idx].TaskID = taskID;
                            nTID[idx].IssueMark = checkBoxIssue.Checked ? 0 : 1;
                            nTID[idx].OrdersType = checkBoxOrdersType.Checked ? 1 : 0;
                            nTID[idx].OldVerMark = 0;
                            dataIdx++;
                        }
                    }
                    idx++;
                }
            }

            // Shadow → Now check
            for(int i = 0; i < sTID.Length;i++ )
            {
                if(string.IsNullOrEmpty(Convert.ToString(sTID[i].TaskIndID)) || sTID[i].TaskIndID == 0 )
                {
                    if( nTID[i].Contract > 0 ) nTID[i].ProcType = "i";
                }
                else
                {
                    if( string.IsNullOrEmpty(Convert.ToString(nTID[i].Contract)) || nTID[i].Contract == 0 )
                    {
                        nTID[i].TaskIndID = sTID[i].TaskIndID;
                        nTID[i].ProcType = "d";
                    }
                    else
                    {
                        if( nTID[i].Contract != sTID[i].Contract ) nTID[i].ProcType = "u";
                        if( nTID[i].AdminCode.TrimEnd() != sTID[i].AdminCode.TrimEnd() ) nTID[i].ProcType = "u";
                        if( nTID[i].LeaderMCode.TrimEnd() != sTID[i].LeaderMCode.TrimEnd() ) nTID[i].ProcType = "u";
                        if( string.IsNullOrEmpty( nTID[i].Department ) ) nTID[i].Department = sTID[i].Department;
                    }

                }

            }

            return nTID;
        }


        private void addTaskNoteData( int taskID )
        {
            if( taskID < 1 ) return;

            TaskNoteData stnd = editTaskNoteData( taskID );
            stnd.InsertTaskNoteData();
        }


        private void updateTaskNoteData( int taskID )
        {
            TaskNoteData stnd = editTaskNoteData( taskID );

            if( tn == null )
            {
                stnd.InsertTaskNoteData();
            }
            else
            {
                stnd.UpdateTaskNoteData();
            }
        }


        private TaskNoteData editTaskNoteData( int taskID )
        {
            TaskNoteData stnd = new TaskNoteData();
            stnd.TaskID = taskID;
            stnd.TaskCode = ( tn == null ) ? textBoxTaskCode.Text : tn.TaskCode;
            stnd.VersionNo = versionNo;
            stnd.Note = textBoxNote.Text;
            stnd.OldVerMark = 0;

            return stnd;
        }


        private void editTaskNameDataList()
        {
            ListFormDataOp lo = new ListFormDataOp();
            string officeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            string departCode = Convert.ToString( comboBoxDepart.SelectedValue );
            tcd = lo.SelectTaskCodeNameData( officeCode, officeCode == "H" ? departCode : "8", null, "ALL" );
        }

        private void readyNewRegistration()
        {
            // 20180628 asakawa 自動採番から手動採番への改善のため一部修正

            initializeScreen();
            reverseEnabledProperty( null );
            btnNew = true;
            comboBoxCostType.Visible = true;
            this.comboBoxCostType.Location = new Point( 116 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            this.textBoxTaskName.Location = new Point( 186 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            selTask = false;
            clearCnfArray( -1 );
            labelMsg.Text = "新規作成モードです。";
            // 20180628 asakawa １行削除し、２行追加
            // textBoxTaskCode.ReadOnly = true;
            textBoxTaskCode.Text = "";
            textBoxTaskCode.ReadOnly = false;
            // 追加はここまで
            comboBoxCostType.Focus();
        }


        private void readyUpdateTaskTransfer(int taskID)
        {
            initializeScreen();
            if( loadTaskAndPartnerData( taskID ) )
            {
                editControl( dataGridView1 );
                selTask = false;
                buttonPrint.Enabled = true;
                buttonUpdate.Enabled = ( checkBoxConstStat.Checked ) ? false : true;
            }
            comboBoxCostType.Visible = false;
            this.comboBoxCostType.Location = new Point( 984 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            this.textBoxTaskName.Location = new Point( 116 + AutoScrollPosition.X, 106 + AutoScrollPosition.Y );
            btnNew = false;
            labelMsg.Text = "検索・更新モードです。";
            textBoxTaskCode.ReadOnly = false;
            textBoxTaskCode.Focus();
        }


        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }


        private void reverseEnabledProperty( string st )
        {
            if( st != null )
            {
                comboBoxVersionNo.Enabled = true;
                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;
                return;
            }
            comboBoxVersionNo.Enabled = !comboBoxVersionNo.Enabled;
            buttonUpdate.Enabled = !buttonUpdate.Enabled;
            buttonDelete.Enabled = !buttonDelete.Enabled;
        }


        private TaskOp collectPersonsData( DataGridView dgv )
        {
            int dataCnt = 0;
            for( int i = 0; i < cbMgrDev.Length; i++ )
            {
                if( cbMgrDev[i].Enabled == true ) dataCnt++;
            }

            string officeString = string.Join( ",", officeArray );
            string[] tempArray = officeString.Split( ',' );
            string[] oCharArray = new string[dataCnt];
            int j = 0;
            for( int i = 0; i < tempArray.Length; i++ )
            {
                if( !string.IsNullOrEmpty( tempArray[i] ) )
                {
                    oCharArray[j] = tempArray[i];
                    j++;
                }
            }

            TaskOp tod = new TaskOp();
            tod.MgrDept = new string[dataCnt];
            tod.MgrName = new string[dataCnt];
            tod.MbrDept = new string[dataCnt];
            tod.MbrName = new string[dataCnt];
            for( int i = 0; i < dataCnt; i++ )
            {
                tod.MgrDept[i] = Conv.OfficeName( oCharArray[i] ) + cbMgrDev[i].Text;
                tod.MgrName[i] = cbMgrNam[i].Text;
                tod.MbrDept[i] = Conv.OfficeName( oCharArray[i] ) + cbMbrDev[i].Text;
                tod.MbrName[i] = cbMbrNam[i].Text;
            }

            tod.AppName = new string[cbCnfNam.Length];
            tod.AppDate = new DateTime[cbCnfNam.Length];
            int[] idx = new int[] { 0, 3, 1, 2, 4 };
            if( !checkBoxIssue.Checked ) cbCnfNam[1].Text = null;
            for( int i = 0; i < cbCnfNam.Length; i++ )
            {
                tod.AppName[idx[i]] = cbCnfNam[i].Text;
                tod.AppDate[idx[i]] = dtCnfDat[i].Value.StripTime();
            }
            return tod;
        }


        private void initializeShadowTaskIndData()
        {
            sTID = new TaskIndData[rows * cols];
            for( int i = 0; i < sTID.Length; i++ ) sTID[i] = new TaskIndData();
        }
      
        
        private bool checkTaskContents()
        {
            int errCount = 0;
            if( textBoxTaskName.Text == "" )
            {
                MessageBox.Show( "業務名が空白です。指定してください。" );
                errCount += 1;
            }

            if( string.IsNullOrEmpty( cbCnfNam[0].Text ) )
            {
                MessageBox.Show( "作成者が指定されていません。指定してください。" );
                errCount += 1;

            }
            return ( errCount == 0 ) ? true : false;
        }
        
        
        private bool certification()
        {

            if( hp.Department == "0" || hp.Department == "9" ) return true;
            CommonData comd = new CommonData();
            string[] memberArray = comd.SelctDefaultMemberAll( Sign.NameTaskTransfer, "InputNumber" );
            for(int i = 0;i<memberArray.Length;i++ )
            {
                if( memberArray[i] == hp.MemberCode ) return true; 
            }
            return false;
        } 
    }
}

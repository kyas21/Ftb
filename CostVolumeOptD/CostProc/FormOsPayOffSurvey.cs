using System;
using System.Data;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;
using PrintOut;

namespace CostProc
{
    public partial class FormOsPayOffSurvey :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        private DataGridViewCellStyle defaultCellStyle;
        HumanProperty hp;
        PartnersScData[] psd;
        TaskCodeNameData[] tca;
        TaskCodeNameData[] tcd;
        MembersScData[] msd;
        CostData[] cmd;
        OsPayOffData opd = new OsPayOffData();

        private string[] itemCdArray;
        private int itmIdx = 0;
        const string noPrevMes = "「前データ」はありません。";
        const string noNextMes = "「次データ」はありません。";
        const string costSave = "原価実績データを保存しました。";
        const string dataSave = "外注精算（起案）データを保存しました。";
        private bool iniPro = true;
        private int iniRCnt = 11;

        const string HQOffice = "本社";
        const string spchar = " ";
        const string leftPare = " [";
        const string rightPare = "] ";
        const string emptyDate = " __ ";
        const string closeDate = "月末";
        private string[] dateArray = new string[]{" 00 "," 01 "," 02 "," 03 "," 04 "," 05 "," 06 "," 07 "," 08 "," 09 "," 10 "," 11 "," 12 "," 13 "," 14 "," 15 ",
                                                  " 16 "," 17 "," 18 "," 19 "," 20 "," 21 "," 22 "," 23 "," 24 "," 25 "," 26 "," 27 "," 28 "," 29 "," 30 "," 31 "};
        private string prePartnerCode = null;
        private DateTime preReportDate;
        private DateTime[] clsArray = new DateTime[4];
        private int contIndex;
        private string[] contArray = new string[] { "請負", "常傭" };
        const string excelFile = "外注清算書(起案)測量部.xlsx";

        private CheckBox[] ckArray;
        private Label[] lbArray;
        private DateTime[] dtArray = new DateTime[3];

        private bool readyPro = true;
        private bool updateStat = false;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormOsPayOffSurvey()
        {
            InitializeComponent();
        }


        public FormOsPayOffSurvey( HumanProperty hp )
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
        private void FormOsPayOffSurvey_Load( object sender, EventArgs e )
        {
            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            //並び替えができないようにする
            //uih.NoSortable();

            labelTotal.Text = "0";
            // 現在の締日一覧
            this.clsArray = new DateTime[] { hp.CloseHDate, hp.CloseKDate, hp.CloseSDate, hp.CloseTDate };

            create_cbOffice();
            create_cbDepart();

            readyDateTimePicker();
            dateTimePickerEx1.Value = clsArray[Conv.oList.IndexOf( hp.OfficeCode )].AddMonths( 1 );     // 初期表示開始月（締月の翌月）
            preReportDate = dateTimePickerEx1.Value.EndOfMonth();
            prePartnerCode = null;

            labelACheckDate.Text = "";
            labelDCheckDate.Text = "";
            labelPCheckDate.Text = "";
            labelMsg.Text = "";
            labelItem.Text = "";


            readyCheckBox();
            buttonEnabled();
            buttonCost.Enabled = false;

            dataGridView1.Rows.Add( iniRCnt );
            seqNoReNumbering( dataGridView1 );

            // 取引先マスタより外注先一覧作成
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            ListFormDataOp lo = new ListFormDataOp();
            psd = lo.SelectPartnersScData();
            msd = lo.SelectMembersScData( Conv.OfficeCode );
            tca = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null );
            //cmd = lo.SelectCostDataJoinOsWkReport( officeCode );
            cmd = lo.SelectCostDataInitialF( Conv.OfficeCode );
            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
            if( itemCdArray != null )
            {
                textBoxCostCode.Text = itemCdArray[itmIdx];
                readyDispTaskData();
            }
        }


        private void FormOsPayOffSurvey_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            dataGridView1.CurrentCell = null;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            textBoxCostCode.Enabled = true;
            labelMsg.Text = "";
            Button btn = ( Button )sender;
            int wkItmIdx = itmIdx;
            switch( btn.Name )
            {
                case "buttonSave":
                    if( !saveOsPayOffData( dataGridView1 ) ) return;
                    if( opd.SlipNo > 0 )
                    {
                        CostReportData crp = new CostReportData();
                        if( !crp.UpdateCostReport( opd ) ) return;           // 変更内容を原価実績データに反映
                    }
                    wkItmIdx = itmIdx;
                    itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
                    itmIdx = wkItmIdx;
                    if( itemCdArray != null ) textBoxCostCode.Text = itemCdArray[itmIdx];

                    if( !saveOsPayOffNoteData() ) return;
                    labelMsg.Text = dataSave;
                    updateStat = false;
                    if( !buttonDelete.Enabled ) buttonDelete.Enabled = true;
                    break;

                case "buttonDelete":
                    if( !eraseOsPayOffData( dataGridView1 ) ) return;
                    wkItmIdx = itmIdx;
                    int wkArrayCount = itemCdArray.Length;
                    itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
                    if( itemCdArray != null ) // データがある
                    {
                        if( wkArrayCount == itemCdArray.Length )
                        {
                            itmIdx = wkItmIdx;
                        }
                        else
                        {
                            itmIdx = ( wkItmIdx < itemCdArray.Length ) ? wkItmIdx : wkItmIdx - 1;
                        }
                        textBoxCostCode.Text = itemCdArray[itmIdx];
                    }

                    break;

                case "buttonCost":
                    if( !saveCostReportData( dataGridView1 ) ) return;
                    if( !saveOsPayOffData( dataGridView1 ) ) return;
                    if( !saveOsPayOffNoteData() ) return;
                    labelMsg.Text = costSave;
                    break;

                case "buttonCancel":
                    initializeControlContents();
                    break;

                case "buttonEnd":
                    if( !unsavedCheck( dataGridView1 ) ) return;
                    this.Close();
                    break;

                case "buttonPrint":
                    PublishOsCost poc = new PublishOsCost( Folder.DefaultExcelTemplate( excelFile ),
                                                           collectPublishData(), setPayOffNoteData() );
                    poc.ExcelFile( "OsPayOffS" );
                    break;

                case "buttonPrev":
                    if( itmIdx == 0 )
                    {
                        labelMsg.Text = noPrevMes;
                        return;
                    }
                    if( !unsavedCheck() ) return;
                    itmIdx--;
                    textBoxCostCode.Text = itemCdArray[itmIdx];
                    break;

                case "buttonNext":
                    if( itmIdx + 1 >= itemCdArray.Length )
                    {
                        labelMsg.Text = noNextMes;
                        return;
                    }
                    if( !unsavedCheck() ) return;
                    itmIdx++;
                    initializeControlContents();
                    textBoxCostCode.Text = itemCdArray[itmIdx];
                    break;

                default:
                    break;
            }

            if( textBoxCostCode.Text == "" ) labelItem.Text = "";

            if( btn.Name == "buttonEnd" || btn.Name == "buttonPrint" ) return;
            selectPayOffData( dataGridView1, dateTimePickerEx1.Value.EndOfMonth(), textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
            selectOsPayOffNoteData();
        }


        // [Ctrl]と組み合わせたTextBoxの操作用Short-Cut Key
        // 前提：コントロールがTextBoxにある時
        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;

            TextBox tb = ( TextBox )sender;

            if( e.KeyCode == Keys.Enter )
            {
                switch( tb.Name )
                {
                    case "textBoxCostCode":
                        if( !selectCostMaster( textBoxCostCode.Text ) )
                        {
                            MessageBox.Show( "指定された原価コードのデータはありません" );
                            return;
                        }
                        break;

                    default:
                        break;
                }

                readyDispTaskData();
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if( e.KeyCode == Keys.A )
            {
                chooseCostData();

                if( textBoxCostCode.Text == "" )
                {
                    labelItem.Text = "";
                    return;
                }

                readyDispTaskData();
            }
        }


        // [Ctrl]と組み合わせたDataGridViewの操作用Short-Cut Key
        // 前提：コントロールがDataGridViewにある時
        private void dataGridView1_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;
            if( !this.dataGridView1.Focused ) return;

            DataGridView dgv = ( DataGridView )sender;
            if( dgv.CurrentCellAddress.Y < 0 ) return;

            if( ckArray[0].Checked ) return;        // 確認済なら入力不可とする

            switch( e.KeyCode )
            {
                case Keys.Right:
                case Keys.Tab:
                    break;

                case Keys.Left:
                    break;

                default:
                    break;
            }

            if( dgv.CurrentCellAddress.X < 2 ) dgv.CurrentCell = dgv[2, dgv.CurrentCellAddress.Y];
            if( dgv.CurrentCellAddress.X == 8 )
            {
                if( dgv.CurrentCellAddress.Y < dgv.Rows.Count - 1 )
                    dgv.CurrentCell = dgv[2, dgv.CurrentCellAddress.Y + 1];
            }

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    if( dgv.CurrentCellAddress.X == 2 || dgv.CurrentCellAddress.X == 3 )
                    {
                        chooseTaskCodeNameData( dgv.Rows[dgv.CurrentCellAddress.Y] );
                        dgv.CurrentCell = dgv[8, dgv.CurrentCellAddress.Y];
                    }
                    if( dgv.CurrentCellAddress.X == 7 )
                    {
                        chooseMemberNameData( dgv.Rows[dgv.CurrentCellAddress.Y] );
                        dgv.CurrentCell = dgv[8, dgv.CurrentCellAddress.Y];
                    }
                    break;

                case Keys.C:
                    Clipboard.SetDataObject( dgv.GetClipboardContent() );
                    break;

                case Keys.I:
                case Keys.D:
                    seqNoReNumbering( dgv );
                    break;

                case Keys.R:
                    break;

                default:
                    break;
            }
            labelMsg.Text = "";
        }


        // Cellの内容に変化があったとき
        private void dataGridView1_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;   // 初期化中

            labelMsg.Text = "";
            DataGridView dgv = ( DataGridView )sender;
            ListFormDataOp lo;

            string WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value );

            switch( e.ColumnIndex )
            {
                case 0:
                    break;

                case 2:     // 業務番号列
                    lo = new ListFormDataOp();
                    TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( Convert.ToString( dgv.Rows[e.RowIndex].Cells["TaskCode"].Value ), hp.OfficeCode );
                    if( tcnd == null ) return;
                    dgv.Rows[e.RowIndex].Cells["TaskName"].Value = tcnd.TaskName;
                    dgv.Rows[e.RowIndex].Cells["CloseDT"].Value = closeDate;
                    break;

                case 8:     // 「数量」列
                    if( dgv.Rows[e.RowIndex].Cells["Amount"].Value == null || Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value );
                    if( !( DHandling.IsDecimal( WorkString ) ) ) WorkString = "0";
                    dgv.Rows[e.RowIndex].Cells["Amount"].Value = decFormat( Convert.ToDecimal( WorkString ) );
                    calcVTotal( dgv );
                    updateStat = true;
                    break;

                case 7:
                    if( !( DHandling.IsNumeric( Convert.ToString( dgv.Rows[e.RowIndex].Cells["LeaderMName"].Value ) ) ) ) return;
                    lo = new ListFormDataOp();
                    MembersScData mscd = lo.SelectMembersScDataS( Convert.ToString( dgv.Rows[e.RowIndex].Cells["LeaderMName"].Value ) );
                    if( mscd == null ) return;
                    dgv.Rows[e.RowIndex].Cells["LeaderMName"].Value = mscd.Name;
                    dgv.Rows[e.RowIndex].Cells["LeaderMCode"].Value = mscd.MemberCode;
                    updateStat = true;
                    break;

                case 11:    // 「伝票番号」列
                    if( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value == null || Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value ) == "" ) return;
                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value );
                    if( !( DHandling.IsNumeric( WorkString ) ) ) WorkString = "";
                    dgv.Rows[e.RowIndex].Cells["SlipNo"].Value = WorkString;
                    break;

                default:
                    break;
            }
           // updateStat = true;
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            buttonEnabled();
            textBoxCostCode.Enabled = true;

            if( DHandling.CheckPastTheDeadline(dtp.Value, clsArray, Conv.OfficeCode))
            {
                buttonDisEnabled();
            }

            if( preReportDate != dtp.Value.EndOfMonth() )
            {
                Func<DialogResult> dialogOverLoad = DMessage.DialogOverLoad;
                if( dialogOverLoad() == DialogResult.No )
                {
                    dtp.Value = preReportDate;
                    return;
                }
            }

            readyCheckBox();

            resetDataGridView();

            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
            if(itemCdArray != null) textBoxCostCode.Text = itemCdArray[itmIdx];

            if( textBoxCostCode.Text == "" )
            {
                labelItem.Text = "";
            }
            else
            {
                selectPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
                selectOsPayOffNoteData();
            }
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;

            ComboBox cbx = ( ComboBox )sender;
            initializeControlContents();
            textBoxCostCode.Enabled = true;
            ListFormDataOp lo = new ListFormDataOp();
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            switch( cbx.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    cmd = lo.SelectCostDataInitialF( Conv.OfficeCode );
                    DateTime wkDate = dateTimePickerEx1.Value;
                    break;

                case "comboBoxDepart":
                    break;

                default:
                    break;
            }

            tca = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode, null );
            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
            if(itemCdArray != null) textBoxCostCode.Text = itemCdArray[itmIdx];
            if( textBoxCostCode.Text == "" )
            {
                labelItem.Text = "";
            }
            else
            {
                selectPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
                selectOsPayOffNoteData();
            };
        }


        /// <summary>
        /// チェックボックス確認
        /// ckArray = new CheckBox[] { checkBoxAdmin, checkBoxDirector, checkBoxPresident };
        /// lbArray = new Label[] { labelACheckDate, labelDCheckDate, labelPCheckDate };
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAdmin_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.AccessLevel != 3 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[0].Checked )
            {
                if( dtArray[0] == DateTime.MinValue ) dtArray[0] = DateTime.Today.StripTime();
                lbArray[0].Text = dtArray[0].ToLongDateString();
                if( checkEffectiveData( dataGridView1 ) )
                {
                    // 変更不可となるメッセージを表示し
                    // 保存済か確認
                    //ステータス更新
                    dataGridView1.ReadOnly = true;
                    updateCheckStatus( dataGridView1, 0, 1, dtArray[0] );
                }
                else
                {
                    DMessage.Unsaved();
                    ckArray[0].Checked = false;
                    lbArray[0].Text = "";
                    return;
                }
            }
            else
            {
                if( ckArray[1].Checked )
                {
                    chb.Checked = true;
                    return;
                }
                //ステータス更新
                updateCheckStatus( dataGridView1, 0, 0, DateTime.Today.StripTime() );
                lbArray[0].Text = "";
                dataGridView1.ReadOnly =false;
            }
        }


        private void checkBoxDirector_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.MemberType != 7 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[1].Checked )
            {
                if( dtArray[1] == DateTime.MinValue ) dtArray[1] = DateTime.Today.StripTime();
                lbArray[1].Text = dtArray[1].ToLongDateString();

                if( ckArray[0].Checked )
                {
                    buttonDisEnabled();
                    buttonCost.Enabled = true;
                }
                else
                {
                    ckArray[1].Checked = false;
                    buttonEnabled();
                    buttonCost.Enabled = false;
                    lbArray[1].Text = "";
                    return;
                }
                updateCheckStatus( dataGridView1, 1, 1, dtArray[1] );
                //ステータス更新
            }
            else
            {
                if( ckArray[2].Checked )
                {
                    ckArray[1].Checked = true;
                    buttonDisEnabled();
                    //buttonCost.Enabled = false; 
                    return;
                }
                else
                {
                    buttonEnabled();
                    buttonCost.Enabled = false;
                }
                lbArray[1].Text = "";
                updateCheckStatus( dataGridView1, 1, 0, dtArray[1] );
            }
        }


        private void checkBoxPresident_CheckedChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( readyPro ) return;
            //if( hp.MemberType != 7 && hp.AccessLevel != 0 ) return;

            CheckBox chb = ( CheckBox )sender;

            if( ckArray[2].Checked )
            {
                if( dtArray[2] == DateTime.MinValue ) dtArray[2] = DateTime.Today.StripTime();
                lbArray[2].Text = dtArray[2].ToLongDateString();

                if( ckArray[1].Checked )
                {
                    buttonDisEnabled();
                }
                else
                {
                    ckArray[2].Checked = false;
                    lbArray[2].Text = "";
                    return;
                }
                updateCheckStatus( dataGridView1, 2, 1, dtArray[2] );
            }
            else
            {
                ckArray[2].Checked = false;
                lbArray[2].Text = "";
                if( ckArray[1].Checked )
                {
                    buttonDisEnabled();
                    buttonCost.Enabled = true;
                }
                else
                {
                    buttonEnabled();
                    buttonCost.Enabled = false;
                }
                updateCheckStatus( dataGridView1, 2, 0, dtArray[2] );
            }

        }


        //private void check_CheckedChanged( object sender, EventArgs e )
        //{
        //    if( iniPro ) return;

        //    CheckBox chb = ( CheckBox )sender;
        //    switch( chb.Name )
        //    {
        //        case "checkBoxPresident":
        //            //if( hp.MemberType != 7 && hp.AccessLevel != 0 )
        //            //{
        //            //    chb.Checked = pCheck;
        //            //    return;
        //            //}
        //            pCheck = chb.Checked;
        //            if( pCheck )
        //            {
        //                buttonDisEnabled();
        //                if( pDate == DateTime.MinValue ) pDate = DateTime.Today.StripTime();
        //                labelPCheckDate.Text = pDate.ToLongDateString();
        //            }
        //            else
        //            {
        //                buttonEnabled();
        //                if( !dCheck ) buttonCost.Enabled = false;
        //                pDate = DateTime.MinValue;
        //                labelPCheckDate.Text = "";
        //            }
        //            break;
        //        case "checkBoxDirector":
        //            //if( hp.MemberType != 7 && hp.AccessLevel != 0 )
        //            //{
        //            //    chb.Checked = dCheck;
        //            //    return;
        //            //}
        //            dCheck = chb.Checked;
        //            if( dCheck )
        //            {
        //                buttonCost.Enabled = true;
        //                if( dDate == DateTime.MinValue ) dDate = DateTime.Today.StripTime();
        //                labelDCheckDate.Text = dDate.ToLongDateString();
        //            }
        //            else
        //            {
        //                buttonCost.Enabled = false;
        //                dDate = DateTime.MinValue;
        //                labelDCheckDate.Text = "";
        //            }
        //            break;
        //        case "checkBoxAdmin":
        //            //if( hp.AccessLevel != 3 && hp.AccessLevel != 0 )
        //            //{
        //            //    chb.Checked = aCheck;
        //            //    return;
        //            //}
        //            aCheck = chb.Checked;
        //            if( aCheck )
        //            {
        //                if( aDate == DateTime.MinValue ) aDate = DateTime.Today.StripTime();
        //                labelACheckDate.Text = aDate.ToLongDateString();
        //            }
        //            else
        //            {
        //                aDate = DateTime.MinValue;
        //                labelACheckDate.Text = "";
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //}
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        private void buttonDisEnabled()
        {
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = false;
            buttonCost.Enabled = false;
        }


        private void buttonEnabled()
        {
            buttonSave.Enabled = true;
            buttonDelete.Enabled = true;
            buttonCancel.Enabled = true;
            buttonCost.Enabled = true;
        }


        private void readyDateTimePicker()
        {
            dateTimePickerEx1.CustomFormat = "yyyy年MM月";
            dateTimePickerEx1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        }


        private void readyCheckBox()
        {
            ckArray = new CheckBox[] { checkBoxAdmin, checkBoxDirector, checkBoxPresident };
            for( int i = 0; i < ckArray.Length; i++ ) ckArray[i].Checked = false;
            lbArray = new Label[] { labelACheckDate, labelDCheckDate, labelPCheckDate };
            for( int i = 0; i < dtArray.Length; i++ ) dtArray[i] = DateTime.MinValue;
        }


        //comboBox作成
        //事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName", hp.AccessLevel );
            comboBoxOffice.SelectedValue =  hp.OfficeCode ;        // 初期値
        }


        //// 部門
        private void create_cbDepart()
        {
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            if( comboBoxOffice.Text == Sign.HQOffice )
            {
                if( hp.Department == "0" ) comboBoxDepart.SelectedValue = "2";
            }
            else
            {
                comboBoxDepart.SelectedValue = "8";
            }
        }


        private void initializeControlContents()
        {
            checkBoxAdmin.Checked = false;
            labelACheckDate.Text = "";
            checkBoxDirector.Checked = false;
            labelDCheckDate.Text = "";
            checkBoxPresident.Checked = false;
            labelPCheckDate.Text = "";
            textBoxCostCode.Text = "";
            labelTotal.Text = "";

            resetDataGridView();
        }

        private void resetDataGridView()
        {
            textBoxNote.Text = "";
            labelTotal.Text = "";
            labelOsPayOffNoteID.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            seqNoReNumbering( dataGridView1 );
        }


        // DataGridViewButtonの番号を再採番
        private void seqNoReNumbering( DataGridView dgv )
        {
            for( int startNo = 1, i = 0; i < dgv.Rows.Count; i++ )
                dgv.Rows[i].Cells["SeqNo"].Value = ( startNo + i ).ToString();
        }


        // 取引先マスタ外注データをFormSubComList画面から得る
        private void choosePartnersScData()
        {
            PartnersScData psds = FormSubComList.ReceiveItems( psd );
            if( psds == null ) return;
            textBoxAccountCode.Text = psds.AccountCode;
            textBoxPartnerCode.Text = psds.PartnerCode;
            ListFormDataOp lo = new ListFormDataOp();

            string editPartnerName = psds.PartnerName;
            cmd = lo.SelectCostData( Convert.ToString( comboBoxOffice.SelectedValue ), "CostCode", "F", editPartnerName.RemoveCorpForm() );
        }


        // 業務番号と業務名をFormTaskCodeNameList画面から得る
        private void chooseTaskCodeNameData( DataGridViewRow dgvRow )
        {
            //TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
            TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tca );
            if( tcds == null ) return;
            setTaskCodeAndNameData( dgvRow, tcds );
        }


        private void setTaskCodeAndNameData( DataGridViewRow dgvRow, TaskCodeNameData tcnd )
        {
            dgvRow.Cells["TaskCode"].Value = tcnd.TaskCode;
            dgvRow.Cells["TaskName"].Value = tcnd.TaskName;

            dgvRow.Cells["ReportCheck"].Value = selectOsWkReport( textBoxCostCode.Text, tcnd.TaskCode, dateTimePickerEx1.Value.EndOfMonth() );
            dgvRow.Cells["PartnerName"].Value = selectPartnerNameUsingTaskCode( tcnd.TaskCode );

            dgvRow.Cells["ContractForm"].Value = contArray[contIndex];

            MembersData md = new MembersData();
            dgvRow.Cells["LeaderMName"].Value = md.SelectMemberName( tcnd.LeaderMCode );
            dgvRow.Cells["LeaderMCode"].Value = tcnd.LeaderMCode;
        }


        // 社員名をFormMembersNameList画面から得る
        private void chooseMemberNameData( DataGridViewRow dgvRow )
        {
            MembersScData msds = FormMembersList.ReceiveItems( msd );
            if( msds == null ) return;
            dgvRow.Cells["LeaderMName"].Value = msds.Name;
            dgvRow.Cells["LeaderMCode"].Value = msds.MemberCode;
        }


        private void chooseCostData()
        {
            if( cmd == null )
            {
                MessageBox.Show( "対象となる原価データはありません" );
                return;
            }
            CostData cmds = FormCostList.ReceiveItems( cmd );
            if( cmds == null ) return;
            dispCostData( cmds );
        }


        private bool selectCostMaster( string costCode )
        {
            CostData cdp = new CostData();
            cdp = cdp.SelectCostMaster( costCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
            if( cdp == null ) return false;
            dispCostData( cdp );
            return true;
        }


        private void dispCostData( CostData cmds )
        {
            textBoxCostCode.Text = cmds.CostCode;
            labelItem.Text = cmds.Item.Replace( "（支払い）", "" );
            textBoxUnit.Text = cmds.Unit;
        }


        private void readyDispTaskData()
        {
            readyCheckBox();

            buttonEnabled();
            buttonCost.Enabled = false;

            DateTime wkBeginDate = dateTimePickerEx1.Value;

            ListFormDataOp lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameFromOsWkReport( textBoxCostCode.Text, wkBeginDate  );

            resetDataGridView();

            selectPayOffData( dataGridView1, wkBeginDate.EndOfMonth(), textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
            selectOsPayOffNoteData();

            //textBoxCostCode.Enabled = false;
            dataGridView1.CurrentCell = dataGridView1[2, 0];

            dataGridView1.Focus();
            //itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, officeCode, departCode );
        }


        /// <summary>
        /// 現在登録してある外注精算データの原価コード（下請け業者情報）のみ収集する
        /// </summary>
        /// <param name="reportDate"></param>
        /// <param name="officeCode"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        private string[] createItemCodeArray( DateTime reportDate, string officeCode, string department )
        {
            itemCdArray = new string[0];
            itmIdx = 0;
            DataTable dt = opd.SelectPayOffItemCode( reportDate.EndOfMonth(), officeCode, department );
            if( dt == null || dt.Rows.Count == 1 )
            {
                buttonPrev.Enabled = false;
                buttonNext.Enabled = false;
                return null;
            }

            itemCdArray = new string[dt.Rows.Count];
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                itemCdArray[i] = Convert.ToString( dr["ItemCode"] );
            }
            if( dt.Rows.Count > 1 )
            {
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
            }

            itmIdx = Array.IndexOf( itemCdArray, textBoxCostCode.Text );
            //if( itmIdx < 0 ) itmIdx = dt.Rows.Count;
            if( itmIdx < 0 ) itmIdx = 0;

            return itemCdArray;
        }


        private void selectPayOffData( DataGridView dgv, DateTime reportDate, string costCode, string officeCode, string department )
        {
            readyPro = true;
            dgv.Enabled = true;
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );

            DataTable dt = opd.SelectPayOff( reportDate.StripTime(), officeCode, department, costCode );
            if( dt == null || dt.Rows.Count < 1 )
            {
                if( tcd == null )
                {
                    labelMsg.Text = "表示できるデータはありません。";
                    seqNoReNumbering( dgv );
                    this.dataGridView1.CurrentCell = null;
                    readyPro = false;
                    //buttonCost.Enabled = true;
                    return;
                }
                dispAllTaskData( dgv );
                readyPro = false;
                labelMsg.Text = "登録可能なものを表示しています。";
                buttonDelete.Enabled = false;
                //buttonCost.Enabled = true;
                return;
            }

            TaskData tdp = new TaskData();
            if( dt.Rows.Count > iniRCnt ) dgv.Rows.Add( dt.Rows.Count - iniRCnt + 5 );
            DataRow dr;
            string taskCode;
            MembersData md = new MembersData();
            PartnersData pd = new PartnersData();
            ListFormDataOp lo = new ListFormDataOp();
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                if( i == 0 )
                {
                    labelItem.Text = Convert.ToString( dr["Item"] );
                    string[] ckItems = new string[] { "AdminCheck", "DirectorCheck", "PresidentCheck" };
                    string[] dtItems = new string[] { "ACheckDate", "DCheckDate", "PCheckDate" };
                    for( int j = 0; j < ckItems.Length; j++ )
                    {
                        int checkStat = Convert.ToInt32( dr[ckItems[j]] );
                        ckArray[j].Checked = ( checkStat == 1 ) ? true : false;
                        dtArray[j] = ( checkStat == 1 ) ? Convert.ToDateTime( dr[dtItems[j]] ) : DateTime.MinValue;
                        lbArray[j].Text = ( checkStat == 1 ) ? dtArray[j].ToLongDateString() : "";
                    }

                    if( ckArray[2].Checked )
                    {
                        buttonDisEnabled();
                    }
                    else if( ckArray[1].Checked )
                    {
                        buttonDisEnabled();
                        buttonCost.Enabled = true;
                    }
                    else if( ckArray[0].Checked )
                    {
                        buttonEnabled();
                        buttonCost.Enabled = false;
                    }
                    selectOsPayOffNoteData();
                }

                taskCode = Convert.ToString( dr["TaskCode"] );

                dgv.Rows[i].Cells["TaskCode"].Value = taskCode;
                dgv.Rows[i].Cells["Amount"].Value = decFormat( Convert.ToDecimal( dr["Cost"] ) );
                dgv.Rows[i].Cells["LeaderMCode"].Value = Convert.ToString( dr["LeaderMCode"] );

                dgv.Rows[i].Cells["SlipNo"].Value = Convert.ToInt32( dr["SlipNo"] ) == 0 ? "" : Convert.ToString( dr["SlipNo"] );

                dgv.Rows[i].Cells["PayOffID"].Value = Convert.ToString( dr["OsPayOffID"] );
                dgv.Rows[i].Cells["Check"].Value = false;
                dgv.Rows[i].Cells["LeaderMName"].Value = md.SelectMemberName( Convert.ToString( dr["LeaderMCode"] ) );
                dgv.Rows[i].Cells["CostReportID"].Value = Convert.ToInt32( dr["CostReportID"] ) == 0 ? "" : Convert.ToString( dr["CostReportID"] );
                dgv.Rows[i].Cells["ContractForm"].Value = contArray[Convert.ToInt32( dr["ContractForm"] )];
                dgv.Rows[i].Cells["CloseDT"].Value = Convert.ToString( dr["CloseInf"] );

                TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( taskCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
                if( tcnd != null )
                {
                    dgv.Rows[i].Cells["TaskName"].Value = tcnd.TaskName;

                    tdp = tdp.SelectTaskData( taskCode );
                    dgv.Rows[i].Cells["SalesMCode"].Value = "0" + tdp.SalesMCode;
                    dgv.Rows[i].Cells["PartnerCode"].Value = tdp.PartnerCode;
                    dgv.Rows[i].Cells["PartnerName"].Value = pd.SelectPartnerName( tdp.PartnerCode );
                    dgv.Rows[i].Cells["ReportCheck"].Value = selectOsWkReport( textBoxCostCode.Text, taskCode, dateTimePickerEx1.Value.EndOfMonth() );
                }
            }
            calcVTotal( dgv );
            seqNoReNumbering( dgv );
            preReportDate = dateTimePickerEx1.Value.EndOfMonth();
            prePartnerCode = textBoxPartnerCode.Text;

            this.dataGridView1.CurrentCell = null;

            readyPro = false;
            labelMsg.Text = "既に登録したものを表示しています。編集可能です。";
            //buttonCost.Enabled = true;
        }


        private string selectPartnerNameUsingTaskCode( string taskCode )
        {
            TaskData tdp = new TaskData();
            tdp = tdp.SelectTaskData( taskCode );
            if( tdp == null ) return "";
            PartnersData pd = new PartnersData();
            return pd.SelectPartnerName( tdp.PartnerCode );
        }


        private string selectOsWkDetail( string costCode, string taskCode, DateTime reportMonth )
        {
            string dateText = "";
            OsWkDetailData owd = new OsWkDetailData();
            DataTable dt = owd.SelectOsWkDetail( costCode, taskCode, reportMonth.EndOfMonth() );
            if( dt == null ) return dateText;

            DataRow dr;
            for( int j = 0; j < dt.Rows.Count; j++ )
            {
                dr = dt.Rows[j];
                for( int i = 1; i < 32; i++ )
                {
                    dateText += ( i == Convert.ToDateTime( dr["ReportDate"] ).Day ) ? dateArray[i] : emptyDate;
                    if( i == 10 || i == 20 ) dateText += "\r\n";
                }


            }
            return dateText;
        }


        private string selectOsWkReport( string costCode, string taskCode, DateTime reportMonth )
        {
            string dateText = "";
            string[] dayWkArray = new string[31];
            OsWkReportData ord = new OsWkReportData();
            //DataTable dt = ord.SelectOsWkReport(costCode, taskCode, reportMonth);
            DataTable dt = ord.SelectOsWkReport( costCode, taskCode, reportMonth.EndOfMonth() );
            if( dt == null ) return dateText;

            int dayInt;
            DataRow dr;
            for( int j = 0; j < dt.Rows.Count; j++ )
            {
                dr = dt.Rows[j];
                if( j == 0 ) contIndex = Convert.ToInt32( dr["ContractForm"] );

                for( int i = 0; i < 31; i++ )
                {
                    dayInt = Convert.ToInt32( Convert.ToDateTime( dr["ReportDate"] ).Day );
                    if( ( i + 1 ) == dayInt )
                    {
                        //dayWkArray[i] = Convert.ToString(dayInt);
                        dayWkArray[i] = dayInt.ToString("00");
                        break;
                    }
                    //dateText += ( i == Convert.ToDateTime( dr["ReportDate"] ).Day ) ? dateArray[i] : emptyDate;
                    //if( i == 10 || i == 20 ) dateText += "\r\n";
                }


            }

            for(int i = 0;i<dayWkArray.Length;i++ )
            {
                dateText += ( string.IsNullOrEmpty( dayWkArray[i] ) ) ? emptyDate : " " + dayWkArray[i] + " ";
                  if( i == 9 || i == 19 ) dateText += "\r\n";
            }

            return dateText;
        }


        private void selectOsPayOffNoteData()
        {
            textBoxNote.Text = "";
            labelOsPayOffNoteID.Text = "";
            OsPayOffNoteData opn = new OsPayOffNoteData();
            //opn = opn.SelectPayOffNote(dateTimePickerEx1.Value.StripTime(), Convert.ToString(comboBoxOffice.SelectedValue), textBoxCostCode.Text);
            opn = opn.SelectPayOffNote( dateTimePickerEx1.Value.EndOfMonth(), Convert.ToString( comboBoxOffice.SelectedValue ), textBoxCostCode.Text );
            if( opn == null ) return;
            textBoxNote.Text = opn.Note;
            labelOsPayOffNoteID.Text = ( opn.OsPayOffNoteID == 0 ) ? "" : Convert.ToString( opn.OsPayOffNoteID );
        }


        private bool saveOsPayOffData( DataGridView dgv )
        {
            if( textBoxCostCode.Text == "" )
            {
                MessageBox.Show( "原価コードが指定されていません！" );
                return false;
            }

            int procCnt = 0;
            opd = new OsPayOffData();
            opd = moveCommonData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value ) == "" ) continue;

                opd = moveIndividualData( dataGridView1.Rows[i] );
                if( Convert.ToString( dgv.Rows[i].Cells["PayOffID"].Value ) == "" )
                {
                    if( !opd.InsertPayOff() ) return false;
                    dgv.Rows[i].Cells["PayOffID"].Value = Convert.ToString( opd.OsPayOffID );
                }
                else
                {
                    opd.OsPayOffID = Convert.ToInt32( dgv.Rows[i].Cells["PayOffID"].Value );
                    if( !opd.UpdatePayOff() ) return false;
                }
                procCnt++;
            }

            if( procCnt == 0 )
            {
                MessageBox.Show( "処理対象のデータはありませんでした！" );
                return false;
            }
            return true;
        }


        private OsPayOffData moveCommonData()
        {
            opd.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            opd.Department = Convert.ToString( comboBoxDepart.SelectedValue );
            //opd.ReportDate = dateTimePickerEx1.Value.StripTime();
            opd.ReportDate = dateTimePickerEx1.Value.EndOfMonth();
            opd.ItemCode = textBoxCostCode.Text;
            opd.Item = labelItem.Text;

            // 暫定処理
            opd.AdminCode = hp.MemberCode;
            opd.AdminCheck = ( ckArray[0].Checked ) ? 1 : 0;
            opd.DirectorCheck = ( ckArray[1].Checked ) ? 1 : 0;
            opd.PresidentCheck = ( ckArray[2].Checked ) ? 1 : 0;

            opd.ACheckDate = dtArray[0].StripTime();
            opd.DCheckDate = dtArray[1].StripTime();
            opd.PCheckDate = dtArray[2].StripTime();
            // 暫定end
            return opd;
        }


        private OsPayOffData moveIndividualData( DataGridViewRow dgvRow )
        {
            opd.TaskCode = Convert.ToString( dgvRow.Cells["TaskCode"].Value );
            opd.Cost = DHandling.ToRegDecimal( Convert.ToString( dgvRow.Cells["Amount"].Value ) );
            opd.LeaderMName = Convert.ToString( dgvRow.Cells["LeaderMName"].Value );
            opd.LeaderMCode = Convert.ToString( dgvRow.Cells["LeaderMCode"].Value );
            opd.SlipNo = ( Convert.ToString( dgvRow.Cells["SlipNo"].Value ) == "" ) ? 0 : Convert.ToInt32( dgvRow.Cells["SlipNo"].Value );
            opd.CostReportID = ( Convert.ToString( dgvRow.Cells["CostReportID"].Value ) == "" ) ? 0 : Convert.ToInt32( dgvRow.Cells["CostReportID"].Value );
            opd.TaskName = Convert.ToString( dgvRow.Cells["TaskName"].Value );
            opd.Unit = "";

            opd.Customer = Convert.ToString( dgvRow.Cells["PartnerName"].Value );
            opd.ContTitle = Convert.ToString( dgvRow.Cells["ContractForm"].Value );
            opd.ContractForm = Array.IndexOf( contArray, Convert.ToString( dgvRow.Cells["ContractForm"].Value ) );
            opd.CloseInf = Convert.ToString( dgvRow.Cells["CloseDT"].Value );
            opd.ReportCheck = Convert.ToString( dgvRow.Cells["ReportCheck"].Value );
            return opd;
        }


        private bool saveOsPayOffNoteData()
        {
            if( labelOsPayOffNoteID.Text == "" && textBoxNote.Text == "" ) return true;
            OsPayOffNoteData opn = new OsPayOffNoteData();
            //opn.ReportDate = dateTimePickerEx1.Value.StripTime();
            opn.ReportDate = dateTimePickerEx1.Value.EndOfMonth();
            opn.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            opn.Department = Convert.ToString( comboBoxDepart.SelectedValue );
            opn.ItemCode = textBoxCostCode.Text;
            opn.Note = textBoxNote.Text;
            if( labelOsPayOffNoteID.Text == "" )
            {
                if( !opn.InsertPayOffNote() ) return false;
                labelOsPayOffNoteID.Text = Convert.ToString( labelOsPayOffNoteID.Text );
            }
            else
            {

                opn.OsPayOffNoteID = Convert.ToInt32( labelOsPayOffNoteID.Text );
                if( !opn.UpdatePayOffNote() ) return false;
            }
            return true;
        }


        /// <summary>
        /// 原価実績データを登録する（「原価データ作成」ボタン押下時）
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private bool saveCostReportData( DataGridView dgv )
        {
            if( textBoxCostCode.Text == "" )
            {
                MessageBox.Show( "原価コードが指定されていません！" );
                return false;
            }

            int procCnt = 0;
            CostReportData crd = new CostReportData();
            crd.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            crd.Department = Convert.ToString( comboBoxDepart.SelectedValue );
            //crd.ReportDate = dateTimePickerEx1.Value.StripTime();
            crd.ReportDate = dateTimePickerEx1.Value.EndOfMonth();
            crd.ItemCode = textBoxCostCode.Text;
            crd.Item = labelItem.Text;
            crd.UnitPrice = 0;
            crd.Quantity = 1;
            //crd.Unit = textBoxUnit.Text;
            crd.Unit = "式";
            crd.SubCoCode = textBoxCostCode.Text;
            crd.Subject = Convert.ToString( textBoxCostCode.Text[0] );
            crd.MemberCode = hp.MemberCode;
            crd.Note = "";
            //crd.AccountCode = textBoxAccountCode.Text;
            crd.AccountCode = "OSPO";
            crd.CoTaskCode = "";

            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value ) == "" ) continue;

                crd.TaskCode = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                crd.Cost = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) );
                crd.LeaderMCode = Convert.ToString( dgv.Rows[i].Cells["LeaderMCode"].Value );
                crd.SalesMCode = Convert.ToString( dgv.Rows[i].Cells["SalesMCode"].Value );
                crd.CustoCode = Convert.ToString( dgv.Rows[i].Cells["PartnerCode"].Value );
                //crd.AccountCode = "OSPO";
                //crd.CoTaskCode = "";

                if( Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value ) == "" )
                {
                    if( !crd.InsertCostReportAndGetID() ) return false;
                    dgv.Rows[i].Cells["SlipNo"].Value = Convert.ToString( crd.SlipNo );
                    dgv.Rows[i].Cells["CostReportID"].Value = Convert.ToString( crd.CostReportID );
                }
                else
                {
                    crd.SlipNo = Convert.ToInt32( dgv.Rows[i].Cells["SlipNo"].Value );
                    if( !crd.UpdateCostReport() ) return false;
                }
                procCnt++;
            }

            if( procCnt == 0 )
            {
                MessageBox.Show( "処理対象のデータはありませんでした！" );
                return false;
            }
            return true;
        }


        /// <summary>
        /// 削除マークがついたもの、一度保存されたものを対象に外注精算データを削除する
        /// 対応する原画実績データがある場合はそれも削除する。
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private bool eraseOsPayOffData( DataGridView dgv )
        {
            opd = new OsPayOffData();
            CostReportData crd = new CostReportData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( Convert.ToString( dgv.Rows[i].Cells["PayOffID"].Value ) == "" ) continue;
                if( !Convert.ToBoolean( dgv.Rows[i].Cells["Check"].Value ) ) continue;

                if( !opd.DeletePayOff( "@pOID", Convert.ToInt32( dgv.Rows[i].Cells["PayOffID"].Value ) ) ) return false;     // 精算データの削除
                if( Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value ) != "" )                                          // 原価データの削除
                {
                    if( !crd.DeleteCostReport( "@slip", Convert.ToInt32( dgv.Rows[i].Cells["SlipNo"].Value ) ) ) return false;
                }
            }
            return true;
        }


        private OsPayOffData[] collectPublishData()
        {
            int rcnt = 0;
            for( int i = 0; i < dataGridView1.Rows.Count; i++ )
                if( Convert.ToString( dataGridView1.Rows[i].Cells["TaskCode"].Value ) != "" ) rcnt++;
            if( rcnt == 0 ) return null;

            OsPayOffData[] opda = new OsPayOffData[rcnt];
            opd = new OsPayOffData();
            opd = moveCommonData();
            for( int i = 0, j = 0; i < dataGridView1.Rows.Count; i++ )
            {
                if( Convert.ToString( dataGridView1.Rows[i].Cells["TaskCode"].Value ) == "" ) continue;
                if( j < rcnt )
                {
                    opda[j] = ( OsPayOffData )( moveIndividualData( dataGridView1.Rows[i] ) ).Clone();
                    j++;
                }
            }
            return opda;
        }


        private OsPayOffNoteData setPayOffNoteData()
        {
            OsPayOffNoteData opn = new OsPayOffNoteData();
            opn.Note = textBoxNote.Text;
            return opn;
        }


        private bool unsavedCheck( DataGridView dgv )
        {
            int usCount = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
                if( Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value ) != "" && Convert.ToString( dgv.Rows[i].Cells["PayOffID"].Value ) == "" ) usCount++;

            if( usCount > 0 )
            {
                Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                if( dialogRemining() == DialogResult.No ) return false;
            }
            else
            {
                unsavedCheck();
            }
            return true;
        }


        private bool unsavedCheck()
        {
            if( updateStat )
            {
                Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                if( dialogRemining() == DialogResult.No ) return false;
            }
            updateStat = false;
            return true;
        }


        private bool checkEffectiveData( DataGridView dgv )
        {
            int effectiveCount = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value ) )
                    && !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["PayOffID"].Value ) ) ) effectiveCount++;
            }
            if( effectiveCount == 0 ) return false;
            return true;
        }


        private void calcVTotal( DataGridView dgv )
        {
            decimal aTotal = 0M;
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( dgv.Rows[i].Cells["Amount"].Value != null && Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) != "" )
                    aTotal += Convert.ToDecimal( dgv.Rows[i].Cells["Amount"].Value );
            }
            labelTotal.Text = decFormat( Convert.ToDecimal( aTotal ) );
        }


        private bool updateCheckStatus( DataGridView dgv, int ckNo, int stat, DateTime ckDt )
        {
            OsPayOffData podPro = new OsPayOffData();
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["PayOffID"].Value ) ) )
                {
                    if( !podPro.UpdatePayOffStatus( Convert.ToInt32( dgv.Rows[i].Cells["PayOffID"].Value ), ckNo, stat, ckDt ) ) return false;
                }
            }
            return true;
        }


        private void dispAllTaskData( DataGridView dgv )
        {
            if( dgv.Rows.Count < tcd.Length ) dgv.Rows.Add( tcd.Length - dgv.Rows.Count );
            for( int i = 0; i < tcd.Length; i++ ) setTaskCodeAndNameData( dgv.Rows[i], tcd[i] );
            updateStat = true;
            seqNoReNumbering( dgv );
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

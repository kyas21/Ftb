﻿using ClassLibrary;
using ListForm;
using PrintOut;
using System;
using System.Data;
using System.Windows.Forms;

namespace CostProc
{
    public partial class FormOsPayOff :Form
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
        private int iniRCnt = 19;

        //private string[] OfficeArray;
        //private string prePartnerCode = null;
        private DateTime preReportDate;
        private DateTime[] clsArray = new DateTime[4];
        const string bookName = "外注清算書(起案).xlsx";
        const string sheetName = "OsPayOff";
        private CheckBox[] ckArray;
        private Label[] lbArray;
        private DateTime[] dtArray = new DateTime[3];

        private bool readyPro = true;
        private bool updateStat = false;

        private string preOffice = "";
        private string preDepart = "";

        // 20190306 asakawa add
        private bool flagInRead = true;
        // asakawa add end
        //


        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormOsPayOff()
        {
            InitializeComponent();
        }

        public FormOsPayOff( HumanProperty hp )
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

        private void FormPayOff_Load( object sender, EventArgs e )
        {
            this.defaultCellStyle = new DataGridViewCellStyle();
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            //並び替えができないようにする

            labelTotal.Text = "0";
            // 現在の締日一覧
            this.clsArray = new DateTime[] { hp.CloseHDate, hp.CloseKDate, hp.CloseSDate, hp.CloseTDate };

            create_cbOffice();
            create_cbDepart();

            readyDateTimePicker();
            dateTimePickerEx1.Value = clsArray[Conv.oList.IndexOf( hp.OfficeCode )].AddMonths( 1 );     // 初期表示開始月（締月の翌月）
            preReportDate = dateTimePickerEx1.Value.EndOfMonth();
            //prePartnerCode = null;

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
            cmd = lo.SelectCostDataInitialF( Conv.OfficeCode );
            tca = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode,null );
            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
            if( itemCdArray != null )
            {
                itmIdx = 0;
                textBoxCostCode.Text = itemCdArray[itmIdx];
                selectOsPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
            }
        }


        private void FormPayOff_Shown( object sender, EventArgs e )
        {
            iniPro = false;       // 初期化処理終了
            dataGridView1.CurrentCell = null;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;
            textBoxCostCode.Enabled = true;

            Button btn = ( Button )sender;
            int wkItmIdx;
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
                    if( itemCdArray != null ) textBoxCostCode.Text = itemCdArray[itmIdx];
                    itmIdx = wkItmIdx;

                    labelMsg.Text = dataSave;
                    if( !ckArray[0].Checked ) dataGridView1.ReadOnly = false;
                    updateStat = false;
                    if( !buttonDelete.Enabled ) buttonDelete.Enabled = true;
                    break;

                case "buttonDelete":
                    if( !eraseOsPayOffData( dataGridView1 ) ) return;
                    wkItmIdx = itmIdx;
                    int wkArrayCount = itemCdArray.Length;
                    itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
                    if(itemCdArray != null ) // データがある
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
                    labelMsg.Text = costSave;
                    break;

                case "buttonCancel":
                    initialDataGridView();
                    break;

                case "buttonEnd":
                    if( !unsavedCheck( dataGridView1 ) ) return;
                    this.Close();
                    break;

                case "buttonPrint":
                    PublishOsCost poc = new PublishOsCost( Folder.DefaultExcelTemplate( bookName ), collectPublishData() );
                    poc.ExcelFile( sheetName );
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
                    initialDataGridView();
                    textBoxCostCode.Text = itemCdArray[itmIdx];
                    break;

                default:
                    break;
            }

            if( textBoxCostCode.Text == "" ) labelItem.Text = "";

            if( btn.Name == "buttonEnd" || btn.Name == "buttonPrint" ) return;

            // 20190306 asakawa add
            if (btn.Name == "buttonSave") return;
            // 20190306 add end

            selectOsPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
            updateStat = false;

        }


        // [Ctrl]と組み合わせたTextBoxの操作用Short-Cut Key
        // 前提：コントロールがTextBoxにある時
        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            if( iniPro ) return;

            TextBox tb = ( TextBox )sender;

            if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            {
                if( tb.Name == "textBoxCostCode" )
                {
                    if( !selectCostMaster( textBoxCostCode.Text ) )
                    {
                        MessageBox.Show( "指定された原価コードのデータはありません" );
                        return;
                    }
                    readPayOffData(dataGridView1);
                }
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
                readPayOffData(dataGridView1);
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
            if( dgv.CurrentCellAddress.X == 6 ) dgv.CurrentCell = dgv[2, dgv.CurrentCellAddress.Y + 1];

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    if( dgv.CurrentCellAddress.X == 2 || dgv.CurrentCellAddress.X == 3 )
                    {
                        chooseTaskCodeNameData( dgv.Rows[dgv.CurrentCellAddress.Y] );
                        dgv.CurrentCell = dgv[4, dgv.CurrentCellAddress.Y];
                    }
                    if( dgv.CurrentCellAddress.X == 5 )
                    {
                        chooseMemberNameData( dgv.Rows[dgv.CurrentCellAddress.Y] );
                        dgv.CurrentCell = dgv[2, dgv.CurrentCellAddress.Y + 1];
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


            DataGridView dgv = ( DataGridView )sender;
            ListFormDataOp lo;

            string WorkString = "";

            switch( e.ColumnIndex )
            {
                case 0:
                    break;

                case 2:     // 業務番号列
                    lo = new ListFormDataOp();
                    TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( Convert.ToString( dgv.Rows[e.RowIndex].Cells["TaskCode"].Value ), hp.OfficeCode );
                    if( tcnd == null ) return;

                    dgv.Rows[e.RowIndex].Cells["TaskName"].Value = tcnd.TaskName;

                    // 20190306 asakawa
                    // updateStat = true;
                    if (flagInRead != true)
                    {
                        updateStat = true;
                    }
                    // asakawa end
                    break;

                case 4:     // 「数量」列
                    if( dgv.Rows[e.RowIndex].Cells["Amount"].Value == null || Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value ) == "" ) return;

                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["Amount"].Value );
                    if( !( DHandling.IsDecimal( WorkString ) ) ) WorkString = "0";
                    dgv.Rows[e.RowIndex].Cells["Amount"].Value = decFormat( Convert.ToDecimal( WorkString ) );
                    calcVTotal( dgv );

                    // 20190306 asakawa
                    // updateStat = true;
                    if (flagInRead != true)
                    {
                        updateStat = true;
                    }
                    // asakawa end 

                    break;

                case 5:
                    if( !( DHandling.IsNumeric( Convert.ToString( dgv.Rows[e.RowIndex].Cells["LeaderMCode"].Value ) ) ) ) return;

                    lo = new ListFormDataOp();
                    MembersScData mscd = lo.SelectMembersScDataS( Convert.ToString( dgv.Rows[e.RowIndex].Cells["LeaderMCode"].Value ) );
                    if( mscd == null ) return;

                    dgv.Rows[e.RowIndex].Cells["LeaderMName"].Value = mscd.Name;
                    dgv.Rows[e.RowIndex].Cells["LeaderMCode"].Value = mscd.MemberCode;

                    // 20190306 asakawa
                    // updateStat = true;
                    if (flagInRead != true)
                    {
                        updateStat = true;
                    }
                    // asakawa end

                    break;

                case 7:     // 「伝票番号」列
                    if( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value == null || Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value ) == "" ) return;

                    WorkString = Convert.ToString( dgv.Rows[e.RowIndex].Cells["SlipNo"].Value );
                    if( !( DHandling.IsNumeric( WorkString ) ) ) WorkString = "";
                    dgv.Rows[e.RowIndex].Cells["SlipNo"].Value = WorkString;


                    // 20190306 asakawa
                    // add
                    if (flagInRead != true)
                    {
                        updateStat = true;
                    }
                    // asakawa end

                    break;

                default:
                    break;
            }

            //updateStat = true;
        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            textBoxCostCode.Enabled = true;
            buttonEnabled();

            if( DHandling.CheckPastTheDeadline( dtp.Value, clsArray, Conv.OfficeCode ) ) buttonDisEnabled();

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
            if( itemCdArray == null || textBoxCostCode.Text == "" )
            {
                labelItem.Text = "";
                return;
            }

            selectOsPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            ComboBox cbx = ( ComboBox )sender;
            textBoxCostCode.Enabled = true;
            ListFormDataOp lo = new ListFormDataOp();
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );
            initialDataGridView();
            switch( cbx.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    textBoxCostCode.Text = "";
                    cmd = lo.SelectCostDataInitialF( Convert.ToString(Conv.OfficeCode) );
                    break;

                case "comboBoxDepart":
                    textBoxCostCode.Text = "";
                    break;

                default:
                    break;
            }

            readyCheckBox();
            resetDataGridView();

            tca = lo.SelectTaskCodeNameData( Conv.OfficeCode, Conv.DepartCode,null );

            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );
            if(itemCdArray != null) textBoxCostCode.Text = itemCdArray[itmIdx];
            if( itemCdArray == null || textBoxCostCode.Text == "" )
            {
                labelItem.Text = "";
                return;
            }
            selectOsPayOffData( dataGridView1, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
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

        //    for(int i = 0;i<ckArray.Length;i++ )
        //    {
        //        if( ckArray[i].Checked )
        //        {
        //            if( dtArray[i] == DateTime.MinValue ) dtArray[i] = DateTime.Today.StripTime();
        //            lbArray[i].Text = dtArray[i].ToLongDateString();
        //        }

        //        switch( i )
        //        {
        //            case 0:
        //                if( ckArray[0].Checked )
        //                {
        //                    if( checkEffectiveData( dataGridView1 ) )
        //                    {
        //                        // 変更不可となるメッセージを表示しステータス更新
        //                        dataGridView1.ReadOnly = true;
        //                    }
        //                    else
        //                    {
        //                        ckArray[0].Checked = false;
        //                        lbArray[0].Text = "";
        //                    }
        //                }
        //                else
        //                {
        //                    //if( ckArray[1].Checked ) ckArray[0].Checked = true;
        //                    lbArray[0].Text = "";
        //                }
        //                // 更新
        //                break;
        
        //            case 1:
        //                if( ckArray[1].Checked )
        //                {
        //                    if( ckArray[0].Checked )
        //                    {
        //                        buttonCost.Enabled = true;
        //                    }
        //                    else
        //                    {
        //                        ckArray[1].Checked = false;
        //                        buttonCost.Enabled = false;
        //                        lbArray[1].Text = "";
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    if( ckArray[2].Checked )
        //                    {
        //                        //ckArray[1].Checked = true; // uncheck cancel
        //                        buttonCost.Enabled = true;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        buttonCost.Enabled = false;
        //                    }
        //                    lbArray[1].Text = "";
        //                }
        //                break;

        //            case 2:
        //                if( ckArray[2].Checked )
        //                {
        //                    if( ckArray[1].Checked )
        //                    {
        //                        buttonDisEnabled();
        //                    }
        //                    else
        //                    {
        //                        ckArray[2].Checked = false;
        //                        lbArray[2].Text = "";
        //                    }
        //                }
        //                else
        //                {
        //                    ckArray[2].Checked = false;
        //                    buttonEnabled();
        //                    buttonCost.Enabled = false;
        //                    lbArray[2].Text = "";
        //                    // ステータス変更
        //                }
        //                break;

        //            default:
        //                break;
        //        }
                
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
            for(int i = 0;i<dtArray.Length;i++ ) dtArray[i] = DateTime.MinValue;
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName", hp.AccessLevel );
            comboBoxOffice.SelectedValue =  hp.OfficeCode ;        // 初期値
        }


        // 部門
        private void create_cbDepart()
        {
            comboBoxDepart.Visible = ( comboBoxOffice.Text == Sign.HQOffice ) ? true : false;

            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxDepart );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB", 1 );
            if(comboBoxOffice.Text == Sign.HQOffice )
            {
                if( hp.Department == "0" ) comboBoxDepart.SelectedValue = "2";
            }
            else
            {
                comboBoxDepart.SelectedValue = "8";
            }
        }


        //private void convertOfficeAndDepart()
        //{
        //    officeCode = Convert.ToString( comboBoxOffice.SelectedValue );
        //    departCode = ( officeCode == Sign.HQOfficeCode ) ? Convert.ToString( comboBoxDepart.SelectedValue ) : "8";
        //}


        private void initialDataGridView()
        {
            checkBoxAdmin.Checked = false;
            labelACheckDate.Text = "";
            checkBoxDirector.Checked = false;
            labelDCheckDate.Text = "";
            checkBoxPresident.Checked = false;
            labelPCheckDate.Text = "";

            textBoxCostCode.Text = "";
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


        private void resetDataGridView()
        {
            labelTotal.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add( iniRCnt );
            seqNoReNumbering( dataGridView1 );
        }


        // 業務番号と業務名をFormTaskCodeNameList画面から得る
        private void chooseTaskCodeNameData( DataGridViewRow dgvRow )
        {
            //TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tcd );
            TaskCodeNameData tcds = FormTaskCodeNameList.ReceiveItems( tca );
            if( tcds == null ) return;
            dgvRow.Cells["TaskCode"].Value = tcds.TaskCode;
            dgvRow.Cells["TaskName"].Value = tcds.TaskName;

            MembersData md = new MembersData();
            dgvRow.Cells["LeaderMName"].Value = md.SelectMemberName( tcds.LeaderMCode );
            dgvRow.Cells["LeaderMCode"].Value = tcds.LeaderMCode;
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


        /// <summary>
        /// 現在登録してある外注精算データの原価コード（下請け業者情報）のみ収集する
        /// </summary>
        /// <param name="reportDate"></param>
        /// <param name="officeCode"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        private string[] createItemCodeArray( DateTime reportDate, string officeCode, string department )
        {
            if( preOffice == officeCode && preDepart == department ) return itemCdArray;
            preOffice = officeCode;
            preDepart = department;
            buttonPrev.Enabled = false;
            buttonNext.Enabled = false;
            itmIdx = 0;
            DataTable dt = opd.SelectPayOffItemCode( reportDate.EndOfMonth(), officeCode, department );
            //OsWkReportData owr = new OsWkReportData();
            //DataTable dt = owr.SelectOsWkReportPartnerCode( dateTimePickerEx1.Value.StripTime(), officeCode,department);
            //if( dt == null )
            //{
            //    itemCdArray = new string[1];
            //    itemCdArray[0] = "";
            //    buttonPrev.Enabled = false;
            //    buttonNext.Enabled = false;
            //    return itemCdArray;
            //}
            if(dt == null) return null;

            itemCdArray = new string[dt.Rows.Count];
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                itemCdArray[i] = Convert.ToString( dr["ItemCode"] );
                //itemCdArray[i] = Convert.ToString( dr["PartnerCode"] );
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


        private void readPayOffData(DataGridView dgv )
        {

            // 20190306 asakawa add
            flagInRead = true;
            // asakawa add end

            readyCheckBox();

            buttonEnabled();
            buttonCost.Enabled = false;


            ListFormDataOp lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameFromOsWkReport( textBoxCostCode.Text, dateTimePickerEx1.Value );

            selectOsPayOffData( dgv, dateTimePickerEx1.Value, textBoxCostCode.Text, Conv.OfficeCode, Conv.DepartCode );
            //textBoxCostCode.Enabled = false;
            dgv.CurrentCell = dgv[2, 0];
            dgv.Focus();
            itemCdArray = createItemCodeArray( dateTimePickerEx1.Value, Conv.OfficeCode, Conv.DepartCode );

            // 20190306 asakawa add
            dataGridView1.ReadOnly = false;
            buttonEnabled();
            buttonCost.Enabled = false;

            if (checkBoxAdmin.Checked == true)
            {
                dataGridView1.ReadOnly = true;
            }
            
            if (checkBoxDirector.Checked == true)
            {
                buttonDisEnabled();
                buttonCost.Enabled = true;
            }
            
            if (checkBoxPresident.Checked == true)
            {
                buttonDisEnabled();
            }
            
            // asakawa add end

            // 20190306 asakawa add
            flagInRead = false;
            // asakawa add end
        }


        private void selectOsPayOffData( DataGridView dgv, DateTime reportDate, string costCode, string officeCode, string department )
        {
            readyPro = true;
            dgv.Enabled = true;
            dgv.Rows.Clear();
            dgv.Rows.Add( iniRCnt );

            DataTable dt = opd.SelectPayOff( reportDate.EndOfMonth(), officeCode, department, costCode );
            //if( dt == null || dt.Rows.Count < 1 )
            //{
            //    labelMsg.Text = "表示できるデータはありません。";
            //    seqNoReNumbering( dgv );
            //    this.dataGridView1.CurrentCell = null;
            //    readyPro = false;
            //    return;
            //}
            if( dt == null || dt.Rows.Count < 1 )
            {
                if(tcd == null)
                {
                    labelMsg.Text = "表示できるデータはありません。";
                    seqNoReNumbering( dgv );
                    this.dataGridView1.CurrentCell = null;
                    readyPro = false;
                    //textBoxCostCode.Enabled = true;
                    return;
                }
                dispAllTaskData( dgv );
                readyPro = false;
                labelMsg.Text = "登録可能なものを表示しています。";
                buttonDelete.Enabled = false;
                //textBoxCostCode.Enabled = false;
                return;
            }

            labelMsg.Text = "";
            TaskData tdp = new TaskData();
            if( dt.Rows.Count > iniRCnt ) dgv.Rows.Add( dt.Rows.Count - iniRCnt + 5 );
            DataRow dr;
            string taskCode;
            MembersData md = new MembersData();
            ListFormDataOp lo = new ListFormDataOp();
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                if( i == 0 )
                {
                    labelItem.Text = Convert.ToString( dr["Item"] );
                    string[] ckItems = new string[] { "AdminCheck", "DirectorCheck", "PresidentCheck" };
                    string[] dtItems = new string[] { "ACheckDate", "DCheckDate", "PCheckDate" };

                    for(int j = 0;j < ckItems.Length;j++ )
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

                TaskCodeNameData tcnd = lo.SelectTaskCodeNameData( taskCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
                if( tcnd != null )
                {
                    dgv.Rows[i].Cells["TaskName"].Value = tcnd.TaskName;

                    tdp = tdp.SelectTaskData( taskCode );
                    dgv.Rows[i].Cells["SalesMCode"].Value = tdp.SalesMCode == "00" ? "000" : tdp.SalesMCode;
                    dgv.Rows[i].Cells["PartnerCode"].Value = tdp.PartnerCode;
                }
            }
            calcVTotal( dgv );
            seqNoReNumbering( dgv );
            preReportDate = dateTimePickerEx1.Value.EndOfMonth();
            //prePartnerCode = textBoxPartnerCode.Text;

            this.dataGridView1.CurrentCell = null;
            readyPro = false;
            labelMsg.Text = "既に登録したものを表示しています。編集可能です。";
            //textBoxCostCode.Enabled = false;
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
            opd.Unit = "";
            opd.ContractForm = 1;
            opd.TaskName = Convert.ToString( dgvRow.Cells["TaskName"].Value );
            opd.CloseInf = "";
            return opd;
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
            crd.ReportDate = dateTimePickerEx1.Value.EndOfMonth();
            crd.ItemCode = textBoxCostCode.Text;
            crd.Item = labelItem.Text;
            crd.UnitPrice = 0;
            crd.Quantity = 1;
            //crd.Unit = textBoxUnit.Text;
            crd.Unit = "式";
            crd.SubCoCode = textBoxCostCode.Text;       // = ItemCode
            crd.Subject = Convert.ToString( textBoxCostCode.Text[0] );
            crd.MemberCode = hp.MemberCode;
            crd.Note = "";
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
            if( updateStat)
            {
                Func<DialogResult> dialogRemining = DMessage.DialogRemining;
                if( dialogRemining() == DialogResult.No ) return false;
            }
            updateStat = false;
            return true;
        }


        private bool checkEffectiveData(DataGridView dgv )
        {
            int effectiveCount = 0 ;
            for(int i = 0; i<dgv.Rows.Count;i++ )
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
            // 20190306 asakawa add
            if (flagInRead == true) return true;
            // asakawa add end

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
            MembersData md = new MembersData();
            for( int i = 0; i < tcd.Length; i++ )
            {
                dgv.Rows[i].Cells["TaskCode"].Value = tcd[i].TaskCode;
                dgv.Rows[i].Cells["TaskName"].Value = tcd[i].TaskName;
                dgv.Rows[i].Cells["Amount"].Value = "";
                dgv.Rows[i].Cells["LeaderMCode"].Value = tcd[i].LeaderMCode;
                dgv.Rows[i].Cells["LeaderMName"].Value = md.SelectMemberName( tcd[i].LeaderMCode );
                dgv.Rows[i].Cells["PayOffID"].Value = "";
                dgv.Rows[i].Cells["Check"].Value = false;
            }
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

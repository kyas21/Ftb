using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using ListForm;

namespace ImportReport
{
    public partial class FormOsWkReportSetup : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp = new HumanProperty();
        TaskCodeNameData[] tcd;
        CostData[] cmd;
        const string HQOffice = "本社";
        private bool iniPro = true;
        private bool[] iSelPro = new bool[2];
        private string folderName;

        const string appFolder = @"\協力会社作業内訳書";
        const string binFolder = @"\bin";
        const string partnerFile = binFolder + @"\PartnersList.CSV";
        const string taskFile = binFolder + @"\TaskList.CSV";
        const string costFile = binFolder + @"\CostList.CSV";
        const string departFile = binFolder + @"\Depinf.CSV";
        const string sourceApp = @"\Release\OutSourceWorkReport.exe";

        const string dataSave = "データを保存しました。";
        const string noSelect = "がされていません！選択してください。";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormOsWkReportSetup()
        {
            InitializeComponent();
        }

        public FormOsWkReportSetup(HumanProperty hp)
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
        private void FormOsWkReportSetup_Load(object sender, EventArgs e)
        {
            UiHandling uih = new UiHandling( dataGridView1 );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();
            uih = new UiHandling( dataGridView2 );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();

            // 現在の締日一覧
            //this.clsArray = new DateTime[] { hp.CloseHDate, hp.CloseKDate, hp.CloseSDate, hp.CloseTDate };

            create_cbOffice();
            comboBoxOffice.SelectedIndex = Conv.oList.IndexOf( hp.OfficeCode );        // 初期値
            create_cbDepart();
            comboBoxDepart.Text = hp.Department;        // 初期値
            labelMessage.Text = "";
            labelItemCode.Text = "";

            // 事業所コードに応じた業務情報取得表示
            //dispTaskCodeNameList(hp.OfficeCode);
            //dispTaskCodeNameList(hp.OfficeCode,hp.Department);
            // 事業所コードに応じた原価情報取得表示
            //dispCostList(hp.OfficeCode);
        }


        private void FormOsWkReportSetup_Shown(object sender, EventArgs e)
        {
            iniPro = false;
            setPreData();
        }


        private void button_Click(object sender, EventArgs e)
        {
            if ( iniPro ) return;

            Button btn = ( Button )sender;

            switch ( btn.Name )
            {
                case "buttonSave":
                    writeTaskFile( dataGridView1 );
                    writeCostFile( dataGridView2 );
                    //writePublishFile(comboBoxOffice, comboBoxDepart);
                    labelMessage.Text = "必要なデータを保存しました。";
                    break;
                case "buttonEnd":
                    backupNowData();
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        // [Ctrl]と組み合わせたTextBoxの操作用Short-Cut Key
        // 前提：コントロールがTextBoxにある時
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            labelMessage.Text = "";
            if ( iniPro ) return;

            TextBox tb = ( TextBox )sender;

            if ( e.KeyCode == Keys.Enter )
            {
                switch ( tb.Name )
                {
                    case "textBoxItem":
                        if ( !selectCostMaster( textBoxItem.Text ) )
                        {
                            MessageBox.Show( "指定された原価コードのデータはありません" );
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }

            if ( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if ( e.KeyCode == Keys.A )
            {
                ListFormDataOp lo = new ListFormDataOp();
                cmd = lo.SelectCostDataInitialF( hp.OfficeCode );
                chooseCostData();
                iSelPro[0] = false;
                iSelPro[1] = false;
                checkBoxTSelAll.Checked = false;
                checkBoxCSelAll.Checked = false;
            }

            if ( textBoxItem.Text == "" )
            {
                labelItemCode.Text = "";
                return;
            }
            else
            {
                dispTaskCodeNameList( Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
                dispCostList( Convert.ToString( comboBoxOffice.SelectedValue ) );
            }
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            labelMessage.Text = "";
            if ( iniPro ) return;
            ComboBox cbx = ( ComboBox )sender;

            switch ( cbx.Name )
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    if ( labelItemCode.Text == "" ) return;
                    dispTaskCodeNameList( Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
                    dispCostList( Convert.ToString( comboBoxOffice.SelectedValue ) );
                    break;
                case "comboBoxDepart":
                    if ( labelItemCode.Text == "" ) return;
                    dispTaskCodeNameList( Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
                    break;
                default:
                    break;
            }
            if ( textBoxItem.Text == "" )
            {
                labelItemCode.Text = "";
                return;
            }
        }


        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if ( iniPro ) return;
            CheckBox ckb = ( CheckBox )sender;
            if ( ckb.Name == "checkBoxTSelAll" )
            {
                if ( iSelPro[0] )
                {
                    iSelPro[0] = false;
                }
                else
                {
                    dataGridViewAllChecked( dataGridView1, checkBoxTSelAll.Checked );
                    dataGridView1.CurrentCell = dataGridView1[0, 0];
                }
            }

            if ( ckb.Name == "checkBoxCSelAll" )
            {
                if ( iSelPro[1] )
                {
                    iSelPro[1] = false;
                }
                else
                {
                    dataGridViewAllChecked( dataGridView2, checkBoxCSelAll.Checked );
                    dataGridView2.CurrentCell = dataGridView2[0, 0];
                }
            }
        }



        private void dataGridView_MouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;
            if ( e.ColumnIndex != 0 ) return;
            dgv.Rows[e.RowIndex].Cells[0].Value = !( Convert.ToBoolean( dgv.Rows[e.RowIndex].Cells[0].Value ) );
            switch ( dgv.Name )
            {
                case "dataGridView1":
                    checkedScan( dgv, "TSelAll" );
                    break;
                case "dataGridView2":
                    checkedScan( dgv, "CSelAll" );
                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        private void buttonDisEnabled()
        {
            buttonSave.Enabled = false;
        }


        private void buttonEnabled()
        {
            buttonSave.Enabled = true;
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
            cbe.DepartmentList( ( comboBoxOffice.Text == HQOffice ) ? "DEPH" : "DEPB", 1 );
            //comboBoxDepart.SelectedValue = 1;
            comboBoxDepart.SelectedValue = hp.Department;
        }


        // DataGridViewButtonの番号を再採番
        private void seqNoReNumbering(DataGridView dgv, string label)
        {
            for ( int startNo = 1, i = 0; i < dgv.RowCount; i++ )
                dgv.Rows[i].Cells[label].Value = ( startNo + i ).ToString();
        }


        private void dataGridViewAllChecked(DataGridView dgv, bool checkVal)
        {
            for ( int i = 0; i < dgv.RowCount; i++ )
            {
                dgv.Rows[i].Cells[0].Value = checkVal;
            }
        }


        private void chooseCostData()
        {
            if ( cmd == null )
            {
                MessageBox.Show( "対象となる原価データはありません" );
                return;
            }

            CostData cmds = FormCostList.ReceiveItems( cmd );
            if ( cmds == null ) return;
            dispSubcontractor( cmds );

            // カレントディレクトリを指定された原価コード名称のフォルダーとする。ただし存在しなければ作成する。
            //folderName = Folder.MyDocuments() + @"\" + labelItemCode.Text + textBoxItem.Text;
            //folderName = @"C:\WorkReport\" + labelItemCode.Text + textBoxItem.Text;
            folderName = @"C:\WorkReport\" + Convert.ToString( comboBoxOffice.SelectedValue )
                        + Convert.ToString( comboBoxDepart.SelectedValue ) + labelItemCode.Text + textBoxItem.Text;
            if ( !System.IO.Directory.Exists( folderName ) )
            {
                Func<DialogResult> dialogCreateFolder = DMessage.DialogCreateFolder;
                if ( dialogCreateFolder() == DialogResult.No ) return;
                System.IO.Directory.CreateDirectory( folderName );
                MessageBox.Show( "'" + folderName + "'を作成しました。" );

                Folder.CopyFolder( Folder.MyDocuments() + appFolder, folderName + binFolder, true );
                //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Folder.MyDocuments() + appFolder, folderName + binFolder,
                //    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);

                ///// Create Shortcut
                string shortcutPath = System.IO.Path.Combine( folderName, labelItemCode.Text + textBoxItem.Text + @"内訳書入力.lnk" );
                //string targetPath = folderName + appFolder + sourceApp;
                string targetPath = folderName + binFolder + sourceApp;
                // WshShellを作成
                Type t = Type.GetTypeFromCLSID( new Guid( "72C24DD5-D70A-438B-8A42-98424B88AFB8" ) );
                dynamic shell = Activator.CreateInstance( t );
                // WshShortcutを作成
                var shortcut = shell.CreateShortcut( shortcutPath );
                // リンク先
                shortcut.TargetPath = targetPath;
                //shortcut.WorkingDirectory = folderName + appFolder + @"\Release";
                shortcut.WorkingDirectory = folderName + binFolder + @"\Release";
                // ショートカットを作成
                shortcut.Save();
                // 後始末
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject( shortcut );
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject( shell );
            }
            if ( !System.IO.File.Exists( folderName + partnerFile ) ) writeSubcontractFile();

            checkExistingTaskData();
            checkExistingCostData();
        }


        private bool selectCostMaster(string costCode)
        {
            CostData cdp = new CostData();
            cdp = cdp.SelectCostMaster( costCode, Convert.ToString( comboBoxOffice.SelectedValue ) );
            if ( cdp == null ) return false;
            dispSubcontractor( cdp );
            return true;
        }


        private void dispSubcontractor(CostData cmds)
        {
            textBoxItem.Text = cmds.Item.Replace( "（支払い）", "" );
            labelItemCode.Text = cmds.CostCode;
        }


        private void dispTaskCodeNameList(string officeCode, string department)
        {
            //string wParam = "";
            //if (officeCode == "H")
            //{
            //    switch (department)
            //    {
            //        case "1":     // 設計
            //            wParam = " AND (LEFT(TaskCode, 1) = 'B' OR LEFT(TaskCode, 1) = 'C' OR LEFT(TaskCode, 1) = 'F')";
            //            break;
            //        case "2":     // 測量
            //            wParam = " AND (LEFT(TaskCode, 1) = 'A' OR LEFT(TaskCode, 1) = 'E' OR LEFT(TaskCode, 1) = 'F')";
            //            break;
            //        default:
            //            wParam = " AND (LEFT(TaskCode, 1) = 'D' OR LEFT(TaskCode, 1) = 'F' OR LEFT(TaskCode, 1) = 'G' OR LEFT(TaskCode, 1) = 'Z')";
            //            break;
            //    }
            //}

            ListFormDataOp lo = new ListFormDataOp();
            ////tcd = lo.SelectTaskCodeNameData(officeCode);
            //tcd = lo.SelectTaskCodeNameData(officeCode, wParam, 1);
            tcd = lo.SelectTaskCodeNameData( officeCode, department, null );
            if ( tcd == null ) return;
            dataGridView1.Rows.Clear();
            //dataGridView1.Rows.Add(tcd.GetLength(0) - 1);
            //if (tcd.GetLength(0) <= 1)
            if (tcd.Length <= 1)
                dataGridView1.Rows.Add(1);
            else
                //dataGridView1.Rows.Add(tcd.GetLength(0) - 1);
                dataGridView1.Rows.Add(tcd.Length - 1);
            TaskData td = new TaskData();
            PartnersData pd = new PartnersData();
            //for ( int i = 0; i < tcd.GetLength( 0 ); i++ )
            for ( int i = 0; i < tcd.Length; i++ )
            {
                dataGridView1.Rows[i].Cells["TaskNo"].Value = tcd[i].TaskCode;
                dataGridView1.Rows[i].Cells["Task"].Value = tcd[i].TaskName;

                td = td.SelectTaskData( tcd[i].TaskID );  //taskIDからtaskを読みPartnerとStartDate,EndDateを得る
                if ( td.TaskName == null ) return;
                dataGridView1.Rows[i].Cells["Partner"].Value = pd.SelectPartnerName( td.PartnerCode );
                dataGridView1.Rows[i].Cells["StartDate"].Value = ( td.StartDate.StripTime() ).ToString( "yyyy年MM月dd日" );
                dataGridView1.Rows[i].Cells["EndDate"].Value = ( td.EndDate.StripTime() ).ToString( "yyyy年MM月dd日" );
            }
            seqNoReNumbering( dataGridView1, "TSeq" );

            checkExistingTaskData();
        }


        private void dispCostList(string officeCode)
        {
            string wParam = " ORDER BY CostCode";
            if ( labelItemCode.Text != "" )
                wParam = " AND ((CostCode LIKE '%999') OR (CostCode BETWEEN 'A' AND 'C'+'1') OR "
                        + "(CostCode LIKE 'D%' AND Item LIKE '" + textBoxItem.Text + "%') OR "
                        + "(CostCode LIKE 'E%') OR (CostCode BETWEEN 'G' AND 'Z'+'1') OR "
                        + "(CostCode = '" + labelItemCode.Text + "')) ORDER BY CostCode";
            //wParam = " AND ((CostCode BETWEEN 'A' AND 'E'+'1') OR (CostCode BETWEEN 'G' AND 'Z'+'1') OR (CostCode = '" + labelItemCode.Text + "')) ORDER BY CostCode";
            ListFormDataOp lo = new ListFormDataOp();
            cmd = lo.SelectCostData( officeCode, wParam );
            if ( cmd == null ) return;
            dataGridView2.Rows.Clear();
            //dataGridView2.Rows.Add( cmd.GetLength( 0 ) - 1 );
            dataGridView2.Rows.Add( cmd.Length - 1 );
            //for ( int i = 0; i < cmd.GetLength( 0 ); i++ )
            for ( int i = 0; i < cmd.Length; i++ )
            {
                dataGridView2.Rows[i].Cells["CItemCode"].Value = cmd[i].CostCode;
                dataGridView2.Rows[i].Cells["CItem"].Value = cmd[i].Item;
                dataGridView2.Rows[i].Cells["CItemDetail"].Value = cmd[i].ItemDetail;
                dataGridView2.Rows[i].Cells["CUnit"].Value = cmd[i].Unit;
            }
            seqNoReNumbering( dataGridView2, "CSeq" );

            checkExistingCostData();
        }


        private void checkExistingTaskData()
        {
            if ( !System.IO.File.Exists( folderName + taskFile ) ) return;
            tcd = readTaskTextFile( folderName + taskFile );
            for ( int i = 0; i < dataGridView1.Rows.Count; i++ )
            {
                //for ( int j = 0; j < tcd.GetLength( 0 ); j++ )
                for ( int j = 0; j < tcd.Length; j++ )
                {
                    if ( Convert.ToString( dataGridView1.Rows[i].Cells["TaskNo"].Value ) == tcd[j].TaskCode )
                    {
                        dataGridView1.Rows[i].Cells["TSel"].Value = true;
                        break;
                    }
                }
            }
        }


        private void checkExistingCostData()
        {
            if ( !System.IO.File.Exists( folderName + costFile ) ) return;
            cmd = readCostTextFile( folderName + costFile );
            for ( int i = 0; i < dataGridView2.Rows.Count; i++ )
            {
                //for ( int j = 0; j < cmd.GetLength( 0 ); j++ )
                for ( int j = 0; j < cmd.Length; j++ )
                {
                    if ( Convert.ToString( dataGridView2.Rows[i].Cells["CItemCode"].Value ) == cmd[j].CostCode )
                    {
                        dataGridView2.Rows[i].Cells["CSel"].Value = true;
                        break;
                    }
                }
            }
        }


        private TaskCodeNameData[] readTaskTextFile(string fileName)
        {
            int lc = 0;
            using ( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                while ( !streamReader.EndOfStream )
                {
                    var line = streamReader.ReadLine();
                    lc++;
                }
            }
            tcd = new TaskCodeNameData[lc];
            using ( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                int i = 0;
                while ( !streamReader.EndOfStream )
                {
                    tcd[i] = new TaskCodeNameData();
                    var line = streamReader.ReadLine();
                    var valArray = line.Split( ',' );
                    tcd[i].TaskCode = Convert.ToString( valArray[0] );
                    i++;
                }
            }
            return tcd;
        }


        private CostData[] readCostTextFile(string fileName)
        {
            int lc = 0;
            using ( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                while ( !streamReader.EndOfStream )
                {
                    var line = streamReader.ReadLine();
                    lc++;
                }
            }
            cmd = new CostData[lc];
            using ( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                int i = 0;
                while ( !streamReader.EndOfStream )
                {
                    cmd[i] = new CostData();
                    var line = streamReader.ReadLine();
                    var valArray = line.Split( ',' );
                    cmd[i].CostCode = Convert.ToString( valArray[0] );
                    i++;
                }
            }
            return cmd;
        }


        private void checkedScan(DataGridView dgv, string sel)
        {
            bool selFlag = false;
            iSelPro[0] = false;
            iSelPro[1] = false;

            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if ( Convert.ToBoolean( dgv.Rows[i].Cells[0].Value ) )
                {
                    selFlag = true;
                    break;
                }
            }

            if ( sel == "TSelAll" )
            {
                iSelPro[0] = true;
                checkBoxTSelAll.Checked = selFlag;
            }

            if ( sel == "CSelAll" )
            {
                iSelPro[1] = true;
                checkBoxCSelAll.Checked = selFlag;
            }
        }


        private void writeSubcontractFile()
        {
            string fileName = folderName + partnerFile;
            if ( System.IO.File.Exists( fileName ) ) System.IO.File.Delete( fileName );

            string dataList = "{0},{1},{2},{3}";
            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using ( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                writer.WriteLine( dataList, labelItemCode.Text,
                                           textBoxItem.Text,
                                           Convert.ToString( comboBoxOffice.SelectedValue ),
                                           Convert.ToString( comboBoxDepart.SelectedValue ) );
                writer.Close();
            }
        }


        private void writeTaskFile(DataGridView dgv)
        {
            if ( !checkDataSelect( dgv, "Task" ) ) return;
            string fileName = folderName + taskFile;
            if ( System.IO.File.Exists( fileName ) ) System.IO.File.Delete( fileName );

            //string dataList = "{0},{1},{2},{3},{4}";
            string dataList = "{0},{1},{2}";
            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using ( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for ( int i = 0; i < dgv.Rows.Count; i++ )
                {
                    if ( Convert.ToBoolean( dgv.Rows[i].Cells[0].Value ) == true )
                    {
                        writer.WriteLine( dataList, dgv.Rows[i].Cells["TaskNo"].Value,
                                                   dgv.Rows[i].Cells["Task"].Value,
                                                   dgv.Rows[i].Cells["Partner"].Value );
                        //writer.WriteLine(dataList, dgv.Rows[i].Cells["TaskNo"].Value,
                        //                            dgv.Rows[i].Cells["Task"].Value,
                        //                            dgv.Rows[i].Cells["Partner"].Value,
                        //                            Convert.ToDateTime(dgv.Rows[i].Cells["StartDate"].Value).StripTime(),
                        //                            Convert.ToDateTime(dgv.Rows[i].Cells["EndDate"].Value).StripTime());
                    }
                }
                writer.Close();
            }
        }


        private void writeCostFile(DataGridView dgv)
        {
            if ( !checkDataSelect( dgv, "Cost" ) ) return;

            string fileName = folderName + costFile;
            if ( System.IO.File.Exists( fileName ) ) System.IO.File.Delete( fileName );

            string dataList = "{0},{1},{2},{3},{4}";
            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using ( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for ( int i = 0; i < dgv.Rows.Count; i++ )
                {
                    if ( Convert.ToBoolean( dgv.Rows[i].Cells[0].Value ) == true )
                    {
                        writer.WriteLine( dataList, dgv.Rows[i].Cells["CItemCode"].Value,
                                                   dgv.Rows[i].Cells["CItem"].Value,
                                                   dgv.Rows[i].Cells["CItemDetail"].Value,
                                                   dgv.Rows[i].Cells["CUnit"].Value,
                                                   Convert.ToString( comboBoxOffice.SelectedValue ) );
                    }
                }
                writer.Close();
            }
        }


        private void writePublishFile(ComboBox comboBoxOffice, ComboBox comboBoxDepart)
        {
            string fileName = folderName + departFile;
            if ( System.IO.File.Exists( fileName ) ) System.IO.File.Delete( fileName );

            string dataList = "{0},{1}";
            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using ( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                writer.WriteLine( dataList, Convert.ToString( comboBoxOffice.SelectedValue ), Convert.ToString( comboBoxDepart.SelectedValue ) );
                writer.Close();
            }
        }


        private bool checkDataSelect(DataGridView dgv, string sel)
        {
            int checkCnt = 0;
            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if ( Convert.ToBoolean( dgv.Rows[i].Cells[0].Value ) == true ) checkCnt++;
            }
            if ( checkCnt == 0 )
            {
                string selNam = ( sel == "Task" ) ? "業務選択" : "原価選択";
                MessageBox.Show( selNam + noSelect );
                return false;
            }
            return true;
        }


        private void setPreData()
        {
            //OsWorkReportSetup.Default.Reset();

            if( !string.IsNullOrEmpty( OsWorkReportSetup.Default.Office ) )
            {
                comboBoxOffice.SelectedValue = OsWorkReportSetup.Default.Office;
                comboBoxOffice.Text = Conv.OfficeName(OsWorkReportSetup.Default.Office);
            }
            if( !string.IsNullOrEmpty( OsWorkReportSetup.Default.Depart ) )
            {
                comboBoxDepart.SelectedValue = OsWorkReportSetup.Default.Depart;
                comboBoxDepart.Text = Conv.DepartName(OsWorkReportSetup.Default.Office,OsWorkReportSetup.Default.Depart);
            }
        }


        private void backupNowData()
        {
            if( comboBoxOffice != null ) OsWorkReportSetup.Default.Office = Convert.ToString( comboBoxOffice.SelectedValue );
            if( comboBoxDepart != null ) OsWorkReportSetup.Default.Depart = Convert.ToString( comboBoxDepart.SelectedValue );

            OsWorkReportSetup.Default.Save();
        }

        



    }
}

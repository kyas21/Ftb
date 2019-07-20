using ClassLibrary;
using System;
using System.Windows.Forms;
using System.IO;

namespace ImportReport
{
    public partial class FormImpOsWkReports : Form
    {
        //---------------------------------------------------------------------/
        //     Field                                                           /
        //---------------------------------------------------------------------/
        private string defaultDir;
        private string[] folderArray = new string[] { @"\業務内訳書処理待", @"\業務内訳書処理済", @"\業務内訳書処理エラー" };
        private string[] bookArray;

        private int errorCnt = 0;
        private int totalErrorCnt = 0;
        const string sheetName = "業務内訳書";
        const int xlsRow = 10;
        const int volSt = 8;                        // 出来高開始行数
        const int cosSt = 21;                       // 出来高開始行数

        CostReportData crd = new CostReportData();
        OsWkReportData wrep = new OsWkReportData();
        OsWkDetailData[] wdtl;

        //---------------------------------------------------------------------/
        //     Constructor                                                     /
        //---------------------------------------------------------------------/
        public FormImpOsWkReports()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------/
        //     Property                                                        /
        //---------------------------------------------------------------------/

        //---------------------------------------------------------------------/
        //     Method                                                          /
        //---------------------------------------------------------------------/
        private void FormImpWkReport_Load(object sender, EventArgs e)
        {
            textBoxInfo.Text = "";
            buttonStart.Enabled = false;

            // 処理待ち、処理済み、エラーフォルダー存在確認　いずれかが無ければ処理中断
            defaultDir = Folder.MyDocuments();
            System.Environment.CurrentDirectory = defaultDir;

            for ( int i = 0; i < folderArray.Length; i++ )
            {
                if ( Directory.Exists( defaultDir + folderArray[i] ) == false )
                {
                    DMessage.FolderNotExistence( folderArray[i] );
                    return;
                }
            }
            // 処理待ちフォルダーの内容確認=ファイルリストを作る
            bookArray = Folder.GetFileName( defaultDir + folderArray[0] );
            if ( bookArray.Length == 0 )
            {
                textBoxInfo.AppendText( "処理待ちの業務日報はありませんでした。" + Environment.NewLine );
                buttonStart.Enabled = false;
            }
            else
            {
                textBoxInfo.AppendText( "処理待ちのExcelファイルが、" + bookArray.Length + " 件あります。" + Environment.NewLine
                                     + "取込を実行するには、「取込み」ボタンをクリックしてください。" + Environment.NewLine );
                buttonStart.Enabled = true;
            }
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            textBoxInfo.AppendText( "処理を開始しました。" + Environment.NewLine + Environment.NewLine );
            // ファイルリストの一件目から処理開始、ファイルリスト全件
            for ( int i = 0; i < bookArray.Length; i++ )
            {
                using ( var streamReader = new StreamReader( bookArray[i], System.Text.Encoding.Default ) )
                {
                    if ( !errorCheck( streamReader, bookArray[i] ) ) moveBookToErrFolder( bookArray[i] );
                }
                if ( errorCnt > 0 ) continue;

                using ( var streamReader = new StreamReader( bookArray[i], System.Text.Encoding.Default ) )
                {
                    if ( !editData( streamReader, bookArray[i] ) ) moveBookToErrFolder( bookArray[i] );
                }
                if ( errorCnt > 0 ) continue;
                if ( !storedData() ) moveBookToErrFolder( bookArray[i] );
                moveBookToProcFolder( bookArray[i] );
            }
            textBoxInfo.AppendText( Environment.NewLine + "すべての処理が完了しました。" + Environment.NewLine );

            string[] errMsg = new string[] { "すべてのファイルの内容が取り込まれました。",
                                       "問題が発生しているファイルがありました。「業務内訳書処理エラー」フォルダを確認してください。" };
            string mess = ( totalErrorCnt == 0 ) ? errMsg[0] : errMsg[1];
            textBoxInfo.AppendText( mess + Environment.NewLine );

            buttonStart.Enabled = false;
        }


        private bool errorCheck(StreamReader stR, string bookName)
        {
            errorCnt = 0;
            bool first = true;
            int errNo;
            while ( !stR.EndOfStream )
            {
                var line = stR.ReadLine();
                var valArray = line.Split( ',' );
                if ( !first ) continue;
                if ( first ) first = false;

                for ( int i = 0; i < valArray.Length; i++ )
                {
                    errNo = -1;
                    if ( i == 0 || i == 1 || i == 2 || i == 3 || i == 5 || i == 7 )
                    {
                        if ( Convert.ToString( valArray[i] ) == "" )
                        {
                            errNo = i;
                        }
                        else
                        {
                            if ( i == 3 )
                            {
                                //string tempTaskCd = string.IsNullOrEmpty( Convert.ToString( valArray[15] ) ) ? Convert.ToString( valArray[3] )
                                //                                                                         : Convert.ToString( valArray[15] );
                                //TaskIndData tid = new TaskIndData();
                                //tid = tid.SelectTaskIndData( tempTaskCd );

                                TaskIndData tid = new TaskIndData();
                                if (valArray.Length > 15)
                                {
                                    string tempTaskCd = string.IsNullOrEmpty( Convert.ToString( valArray[15] ) ) ? Convert.ToString( valArray[3] )
                                                                                                                 : Convert.ToString( valArray[15] );
                                    tid = tid.SelectTaskIndData( tempTaskCd );
                                }
                                else
                                {
                                    tid = tid.SelectTaskIndData( Convert.ToString( valArray[3] ) );
                                }
                                if( tid == null ) errNo = 4;
                            }
                        }
                    }

                    if ( errNo > -1 )
                    {
                        editErrorMsg( bookName, errNo );
                        errorCnt++;
                        totalErrorCnt++;
                    }
                }
            }
            stR.Close();
            if ( errorCnt > 0 ) return false;

            return true;
        }


        private void editErrorMsg(string bookName, int errNo)
        {
            string[] eMsgArray = new string[] { "業務年月日未記入",
                                                "協力会社名未記入",
                                                "協力業者コード未記入",
                                                "業務番号未記入",
                                                "業務番号誤記入",
                                                "発注元不明",
                                                "",
                                                "作成者未記入"    };

            textBoxInfo.AppendText( "×ERROR " + bookName + " : " + eMsgArray[errNo] + Environment.NewLine );
        }


        private bool editData(StreamReader stR, string bookName)
        {
            bool first = true;
            string dataType;
            int lno = 0;
            int cntK = 0, cntK1 = 0, cntL = 0;
            int cntA = 0, cntB = 0, cntC = 0, cntG = 0, cntD = 0, cntD1 = 0;
            //TaskData td = new TaskData();
            //TaskIndData tid = new TaskIndData();
            CostData cd;

            while ( !stR.EndOfStream )
            {
                var line = stR.ReadLine();
                var valArray = line.Split( ',' );
                if ( first )
                {
                    wrep.ReportDate = Convert.ToDateTime( valArray[0] );
                    wrep.TaskCode = Convert.ToString( valArray[3] );
                    wrep.Note = Convert.ToString( valArray[6] );
                    wrep.Author = Convert.ToString( valArray[7] );
                    wrep.OfficeCode = Convert.ToString( valArray[11] );
                    wrep.Department = Convert.ToString( valArray[12] );
                    wrep.PartnerCode = Convert.ToString( valArray[2] );   // 協力会社コードFXXX
                    wrep.PartnerName = Convert.ToString( valArray[1] );   // 協力会社名
                    wrep.ContractForm = 1;
                    wrep.PNo = Convert.ToInt32( valArray[13] );
                    wrep.TotalP = Convert.ToInt32( valArray[14] );

                    wrep.CoTaskCode = ""; 
                    if (valArray.Length > 15)
                    {
                        wrep.CoTaskCode = String.IsNullOrEmpty( Convert.ToString( valArray[15] ) ) ? "" : Convert.ToString( valArray[15] );
                    }
                    //wrep.CoTaskCode = Convert.ToString(valArray[15]) ;

                    TaskData td = new TaskData();
                    //td = td.SelectTaskData(wrep.TaskCode);
                    td = td.SelectTaskData( ( wrep.CoTaskCode == "" ) ? wrep.TaskCode : wrep.CoTaskCode );
                    wrep.SalesMCode = td.SalesMCode;
                    wrep.CustoCode = td.PartnerCode;

                    TaskIndData tid = new TaskIndData();
                    //string tempTaskCd = (wrep.CoTaskCode == "") ? wrep.TaskCode : wrep.CoTaskCode;
                    //tid = tid.SelectTaskIndData(wrep.TaskCode);
                    tid = tid.SelectTaskIndData( ( wrep.CoTaskCode == "" ) ? wrep.TaskCode : wrep.CoTaskCode );
                    //wrep.OfficeCode = tid.OfficeCode;
                    wrep.LeaderMCode = tid.LeaderMCode;

                    int volLine = Convert.ToInt32( valArray[8] );
                    int costLine = Convert.ToInt32( valArray[9] );
                    int costBLine = Convert.ToInt32( valArray[10] );

                    wdtl = new OsWkDetailData[volLine * 3 + costLine * 4 + costBLine * 2];
                    first = false;
                }
                else
                {
                    wdtl[lno] = new OsWkDetailData();
                    dataType = Convert.ToString( valArray[0] );
                    if ( Convert.ToString( valArray[1] ) == "" || Convert.ToString( valArray[1] ) == null )
                    {
                        wdtl[lno].ItemCode = "";
                        lno++;
                        continue;
                    }

                    wdtl[lno].ItemCode = Convert.ToString( valArray[1] );
                    wdtl[lno].Item = Convert.ToString( valArray[2] );
                    // Get CostData
                    cd = new CostData();
                    cd = cd.SelectCostMaster( wdtl[lno].ItemCode, wrep.OfficeCode );
                    wdtl[lno].Cost = ( cd == null ) ? 0M : cd.Cost;

                    if ( Convert.ToString( valArray[5] ) != "" ) wdtl[lno].Quantity = Convert.ToDecimal( valArray[5] );
                    wdtl[lno].Unit = ( valArray[6] == "" ) ? "式" : Convert.ToString( valArray[6] );
                    if ( dataType != "K1" )
                    {
                        //wdtl[lno].Unit = (valArray[6] == "") ? "": Convert.ToString(valArray[6]);
                        wdtl[lno].Subject = dataType;
                        wdtl[lno].ItemDetail = "";
                        wdtl[lno].Range = "";
                    }

                    switch ( dataType )
                    {
                        case "K":
                            wdtl[lno].LNo = cntK;
                            cntK++;
                            break;
                        case "K1":
                            wdtl[lno].ItemDetail = Convert.ToString( valArray[3] );
                            wdtl[lno].Range = Convert.ToString( valArray[4] );
                            wdtl[lno].Subject = "K";
                            //wdtl[lno].Unit = "";
                            wdtl[lno].LNo = cntK1;
                            cntK1++;
                            break;
                        case "L":
                            wdtl[lno].LNo = cntL;
                            cntL++;
                            break;
                        case "A":
                            // costget
                            wdtl[lno].LNo = cntA;
                            cntA++;
                            break;
                        case "D":
                            wdtl[lno].LNo = cntD;
                            cntD++;
                            break;
                        case "C":
                            wdtl[lno].LNo = cntC;
                            cntC++;
                            break;
                        case "G":
                            wdtl[lno].LNo = cntG;
                            cntG++;
                            break;
                        case "B":
                            //costget
                            wdtl[lno].LNo = cntB;
                            cntB++;
                            break;
                        case "D1":
                            wdtl[lno].Subject = "D";
                            wdtl[lno].LNo = cntD1;
                            cntD1++;
                            break;
                        default:
                            break;
                    }
                    wdtl[lno].RecType = ( dataType == "K" || dataType == "K1" || dataType == "L" ) ? 0 : 1;
                    lno++;
                }
            }
            stR.Close();
            return true;
        }


        private bool storedData()
        {
            int lineCount = 0;
            for ( int i = 0; i < wdtl.Length; i++ )
            {
                if ( wdtl[i].ItemCode == "" || wdtl[i].ItemCode == null ) continue;
                lineCount++;
            }

            OsWkDetailData[] nwdtl = new OsWkDetailData[lineCount];
            for ( int idx = 0, i = 0; i < wdtl.Length; i++ )
            {
                if ( wdtl[i].ItemCode == "" || wdtl[i].ItemCode == null ) continue;

                nwdtl[idx] = ( OsWkDetailData )wdtl[i].Clone();

                nwdtl[idx].PartnerCode = wrep.PartnerCode;
                nwdtl[idx].TaskCode = wrep.TaskCode;
                nwdtl[idx].ReportDate = wrep.ReportDate;

                nwdtl[idx].OfficeCode = wrep.OfficeCode;
                nwdtl[idx].Department = wrep.Department;
                nwdtl[idx].MmeberCode = wrep.MemberCode;
                nwdtl[idx].LeaderMCode = wrep.LeaderMCode;
                nwdtl[idx].SalesMCode = wrep.SalesMCode;
                nwdtl[idx].CustoCode = wrep.CustoCode;
                nwdtl[idx].CoTaskCode = wrep.CoTaskCode;
                idx++;
            }

            // 既に、同じReportDate,TaskCode,OfficeCode,Department,SubCoCode(PartnerCode)の
            // CostReprotがあれば伝票番号を得たのちに削除する
            // 伝票番号をもとに削除したCostReportに関連する、外注作業明細を削除する
            int slipNo = crd.SelectCostSlipNo( wrep.ReportDate, wrep.TaskCode, wrep.PartnerCode, wrep.OfficeCode, wrep.Department );
            if ( slipNo > 0 )
            {
                if ( crd.DeleteCostReport( "@slip", slipNo ) )
                {
                    OsWkDetailData osd = new OsWkDetailData();
                    int osWkReportID = osd.SelectOsWkReportID( slipNo );
                    if ( osWkReportID > 0 )
                    {
                        OsWkReportData osr = new OsWkReportData();
                        if ( !osr.DeleteOsWkReport( "OsWkReportID = " + osWkReportID ) ) return false;
                    }
                    if ( !osd.DeleteOsWkDetail( "@slip", slipNo ) ) return false;
                }
            }

            if ( ( nwdtl = crd.InsertCostReport( nwdtl ) ) == null ) return false;

            //wrep.TaskCode = (wrep.RTaskCode == "")? wrep.TaskCode : wrep.RTaskCode;
            if ( !wrep.StoreOsWkReportAndDetail( nwdtl ) ) return false;

            return true;
        }



        private decimal getCostData(string officeCode, string itemCode)
        {
            CostData cd = new CostData();
            cd = cd.SelectCostMaster( itemCode, officeCode );
            if ( cd == null ) return 0;
            return cd.Cost;
        }


        private void moveBookToProcFolder(string bookName)

        {
            bookName = moveBook( bookName, folderArray[1] );
            textBoxInfo.AppendText( "〇 " + bookName + " を「" + defaultDir + folderArray[1] + "」フォルダに移動しました。" + Environment.NewLine + Environment.NewLine );
        }


        private void moveBookToErrFolder(string bookName)
        {
            bookName = moveBook( bookName, folderArray[2] );
            textBoxInfo.AppendText( "● " + bookName + " を「" + defaultDir + folderArray[2] + "」フォルダに移動しました。" + Environment.NewLine + Environment.NewLine );
        }

        private string moveBook(string bookName, string folder)
        {
            DateTime now = DateTime.Now;
            string destFileName = Conv.FormattingName( Path.GetFileNameWithoutExtension( bookName ) );
            File.Move( bookName, defaultDir + folder + @"\" + destFileName + "(" + now.ToString( "yyMMddHHmmss" ) + ").CSV" );
            return destFileName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class GenericData :CostReportData
    {
        // 商魂用汎用データ作成
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        StringUtility util = new StringUtility();
        TaskIndData tid = new TaskIndData();
        CostData cmd = new CostData();
        MembersData mmd = new MembersData();

        private string fileName;
        private DataTable dt;
        private string[] oldDepArray = new string[] { "10", "11", "12", "17", "20", "22", "30", "32", "19", "18", "00", "00", "00", "00" };
        private string[] nowDepArray = new string[] { "H0", "H1", "H2", "H7", "K9", "K8", "S9", "S8", "T9", "T8", "H9", "K0", "S0", "T0" };

        private string[] sDeptArray = new string[] { "01", "02", "02", "04", "01", "02", "04", "07", "06",
                                                     "08", "08", "08", "08", "08", "08", "08", "11", "10",
                                                     "12", "12", "12", "12", "12", "12", "12", "15", "14",
                                                     "17", "17", "17", "17", "17", "17", "17", "18", "18" };
        private string[] sdKeyArray = new string[] { "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HZ", "H ",
                                                     "KA", "KB", "KC", "KD", "KE", "KF", "KG", "KZ", "K ",
                                                     "SA", "SB", "SC", "SD", "SE", "SF", "SG", "SZ", "S ",
                                                     "TA", "TB", "TC", "TD", "TE", "TF", "TG", "TZ", "T " };
        private string kMark = "●";
        private string holdMCode = "";
        private string holdDept = "";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public GenericData()
        {
        }

        public GenericData( string fileName )
        {
            this.fileName = fileName;
        }

        public GenericData( DataTable dt )
        {
            this.dt = dt;
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string TaskName { get; set; }
        //public string CostType { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        /// <summary>
        /// コスト情報（D_CostReport）を指定条件で読取り、
        /// 「商魂」で読込可能な汎用データファイル（売上明細情報）を
        /// 作成する
        /// </summary>
        /// <param name="encName"> 作成するファイルの文字エンコード名 </param>
        /// <param name="fileName"> 作成ファイル名 </param>
        /// <returns></returns>
        public int CreateGenricData_CostReport( string encName, string fileName )
        {
            int procCnt = 0;

            if( dt == null ) return -1;
            if( dt.Rows.Count < 1 ) return -1;
            DataRow dr;

            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for( int i = 0; i < dt.Rows.Count; i++ )
                {
                    dr = dt.Rows[i];
                    ReportDate = Convert.ToDateTime( dr["ReportDate"] );
                    SlipNo = Convert.ToInt32( dr["SlipNo"] );
                    ItemCode = Convert.ToString( dr["ItemCode"] );
                    Item = Convert.ToString( dr["Item"] );
                    Quantity = Convert.ToDecimal( dr["Quantity"] );
                    Unit = Convert.ToString( dr["Unit"] );
                    UnitPrice = Convert.ToDecimal( dr["UnitPrice"] );
                    Cost = Convert.ToDecimal( dr["Cost"] );
                    Note = Convert.ToString( dr["Note"] );
                    MemberCode = Convert.ToString( dr["MemberCode"] );
                    OfficeCode = Convert.ToString( dr["OfficeCode"] );
                    Department = Convert.ToString( dr["Department"] );
                    //LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
                    Subject = Convert.ToString( dr["Subject"] );

                    if( TaskCode != Convert.ToString( dr["TaskCode"] ) )
                    {
                        TaskCode = Convert.ToString( dr["TaskCode"] );

                        //SqlHandling sh = new SqlHandling();
                        //string sqlStr = "TaskName FROM D_TaskInd WHERE OldVerMark = 0 AND TaskCode = '" + TaskCode + "'";
                        //taskdt = sh.SelectFullDescription(sqlStr);
                        //if ((taskdt == null) || (taskdt.Rows.Count < 1))

                        tid = new TaskIndData();
                        tid = tid.SelectTaskIndSData( "WHERE OldVerMark = 0 AND TaskCode = '" + TaskCode + "'" );
                        if( tid == null )
                        {
                            TaskName = "";
                            LeaderMCode = Convert.ToString( dr["LeaderMCode"] );
                        }
                        else
                        {
                            //taskdr = taskdt.Rows[0];
                            //TaskName = Convert.ToString(taskdr["TaskName"]);
                            TaskName = tid.TaskName;
                            LeaderMCode = tid.LeaderMCode;
                        }
                    }

                    writer.WriteLine( editTextLine() );
                    procCnt++;
                }
                writer.Close();
            }
            return procCnt;
        }


        private string editTextLine()
        {
            // 2018.01 asakawa 数値がマイナスの場合の出力形式の修正のために一部変更

            string textLine = "";
            // 伝区
            textLine += "0";
            // 年号
            textLine += "1";
            // 売上年月日
            string date = ReportDate.ToShortDateString();
            textLine += util.TruncateByteRight( DHandling.RemoveNoNum( date ), 6 );         // yyyymmdd →yymmdd
            // 請求年月日
            textLine += util.TruncateByteRight( DHandling.RemoveNoNum( date ), 6 );         // yyyymmdd →yymmdd
            // 伝票番号 6桁、0Padding
            textLine += util.PaddingInBytes( Convert.ToString( SlipNo ), "Number", 6 );

            // 得意先コード（業務No)
            textLine += TaskCode;

            // 得意先名
            char[] strChar = TaskName.ToCharArray();
            for( int i = 0; i < strChar.Length; i++ ) if( strChar[i] == ' ' ) strChar[i] = '　';
            TaskName = new string( strChar );
            //string taskData = util.FormFixedByteLengthLeft("(" + CostType + ")" + TaskName, 68);
            string taskData = util.FormFixedByteLengthLeft( TaskName, 68 );
            textLine += util.TruncateByteLeft( taskData, 40 );
            // 先方担当者名
            textLine += util.TruncateByteRight( taskData, 28 );
            // 部門コード
            int dIdx = Array.IndexOf( sdKeyArray, ( OfficeCode + util.SubstringByte( TaskCode, 0, 1 ) ) );
            if( dIdx < 0 ) dIdx = Array.IndexOf( sdKeyArray, ( OfficeCode + " " ) );
            textLine += sDeptArray[dIdx];
            // 担当者コード
            //string pCode = Conv.ResizeMemberCode(MemberCode.TrimEnd(),2);
            textLine += Conv.ResizeMemberCode( LeaderMCode.TrimEnd(), 2 );
            // 摘要コード
            textLine += "00";
            // 摘要名
            textLine += util.PaddingInBytes( " ", "Char", 30 );
            // 商品コード
            textLine += util.SubstringByte( ItemCode, 0, 4 );
            // マスター区分
            textLine += "0";
            // 品名
            if( ItemCode.TrimEnd() == "K999" )
            {
                char checkChar = ' ';
                if( Item != "" ) checkChar = Item[0];
                string kItem = ( checkChar == '●' ) ? Item : kMark + Item;
                textLine += util.FormFixedByteLengthLeft( kItem, 36 );
            }
            else
            {
                textLine += util.FormFixedByteLengthLeft( Item, 36 );
            }
            //textLine += util.PaddingInBytes(Item, "Char", 36);
            //textLine += util.FormFixedByteLengthLeft(Item, 36);
            // 区 
            textLine += "0";
            // 入数
            textLine += "0000";
            // 箱数
            textLine += "00000";
            // 数量
            if( Quantity == 0 ) Quantity = 1;        // 0では商魂汎用データ作成時にエラーになる
            // 2018.01 asakawa マイナス値の場合の出力形式の修正のために変更
            // textLine += util.PaddingInBytes( Convert.ToString( Quantity ), "Number", 9 );
            if (Quantity < 0)
            {
                textLine += "-";
                decimal qq = Quantity * (-1);
                textLine += util.PaddingInBytes(Convert.ToString(qq), "Number", 8);
            }
            else
            {
                textLine += util.PaddingInBytes(Convert.ToString(Quantity), "Number", 9);
            }
            // 単位
            textLine += util.PaddingInBytes( Unit, "Char", 4 );
            // 単価
            // 2018.01 asakawa マイナス値の場合の出力形式の修正のために変更
            // textLine += util.PaddingInBytes( UnitPrice.ToString( "0" ), "Number", 10 );
            if (UnitPrice < 0)
            {
                textLine += "-";
                decimal pp = UnitPrice * (-1);
                // textLine += util.PaddingInBytes(Convert.ToString(pp), "Number", 9); // 小数点以下も出力される
                textLine += util.PaddingInBytes(pp.ToString("0"), "Number", 9);
            }
            else
            {
                textLine += util.PaddingInBytes(UnitPrice.ToString("0"), "Number", 10);
            }
            // 売上金額
            int sales = Convert.ToInt32( Quantity * UnitPrice );
            // 2018.01 asakawa マイナス値の場合の出力形式の修正のために変更
            // textLine += util.PaddingInBytes( Convert.ToString( sales ), "Number", 10 );
            if (sales < 0)
            {
                textLine += "-";
                textLine += util.PaddingInBytes(Convert.ToString(sales * (-1)), "Number", 9);
            }
            else
            {
                textLine += util.PaddingInBytes(Convert.ToString(sales), "Number", 10);
            }
            // 原単価
            textLine += util.PaddingInBytes( "0", "Number", 9 );
            // 原価額
            textLine += util.PaddingInBytes( "0", "Number", 10 );
            // 税区分
            textLine += "0";
            // 税込区分
            textLine += "0";
            // 備考区分
            textLine += "1";        //備考区分が0:備考は数字、1:文字1
            // 備考
            textLine += util.PaddingInBytes( " ", "Char", 9 );
            // 同時入荷区分
            textLine += "0";

            return textLine;
        }



        public int CreateCostReportDataByCSVData( string fileName, string officeCode )
        {
            int procCount = 0;
            int repoDate;
            string[] codeArray = new string[2];
            int[] getaArray = new int[] { 0, 300000, 500000, 700000 };
            int geta = getaArray[Conv.OfficeCodeIndex( officeCode )];
            StringUtility sutil = new StringUtility();
            MembersData md = new MembersData();
            CostReportData crd = new CostReportData();
            try
            {
                using( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
                {
                    while( !streamReader.EndOfStream )
                    {
                        var line = streamReader.ReadLine();
                        var valArray = line.Split( ',' );

                        if( !DHandling.IsNumeric( Convert.ToString( valArray[0] ) ) ) continue;
                        if(Convert.ToInt32( valArray[4] ) > 900000) continue;
                        crd.SlipNo = Convert.ToInt32( valArray[4] ) + geta;

                        repoDate = ( Convert.ToInt32( valArray[1] ) == 0 ) ? Convert.ToInt32( valArray[2] ) + 19880000 : Convert.ToInt32( valArray[2] ) + 20000000;
                        crd.ReportDate = DateTime.ParseExact( Convert.ToString( repoDate ) + "000000", "yyyyMMddHHmmss", null );
                        crd.TaskCode = Convert.ToString( valArray[5] );
                        crd.ItemCode = Convert.ToString( valArray[12] );
                        crd.Item = Convert.ToString( valArray[14] );
                        crd.Quantity = Convert.ToDecimal( valArray[18] );
                        crd.Unit = Convert.ToString( valArray[19] );
                        crd.UnitPrice = Convert.ToDecimal( valArray[20] );
                        crd.Cost = Convert.ToDecimal( valArray[21] );
                        crd.Note = Convert.ToString( valArray[27] );
                        TaskData td = new TaskData();
                        td = td.SelectTaskData( crd.TaskCode );
                        if( td != null )
                        {
                            crd.CustoCode = td.PartnerCode;
                            crd.SalesMCode = td.SalesMCode;
                        }
                        crd.Subject = sutil.SubstringByte( crd.ItemCode, 0, 1 );
                        crd.SubCoCode = ( crd.Subject == "F" ) ? crd.ItemCode : "";
                        crd.MemberCode = ( Convert.ToInt32( valArray[9] ) < 100 ) ? "0" + Convert.ToString( valArray[9] ) : Convert.ToString( valArray[9] );
                        crd.LeaderMCode = ( Convert.ToInt32( valArray[9] ) < 100 ) ? "0" + Convert.ToString( valArray[9] ) : Convert.ToString( valArray[9] );

                        // 事業所および部門
                        crd.OfficeCode = officeCode;
                        crd.Department = ( crd.OfficeCode == "H" ) ? Conv.tdHList[Conv.DepartmentCodeIndex( sutil.SubstringByte( crd.TaskCode, 0, 1 ) )]
                                                                 : Conv.tdBList[Conv.DepartmentCodeIndex( sutil.SubstringByte( crd.TaskCode, 0, 1 ) )];
                        if( crd.MemberCode != "" && crd.MemberCode == holdMCode )
                        {
                            crd.Department = holdDept;
                        }
                        else
                        {
                            codeArray = md.SelectMembersOffice( crd.MemberCode );
                            if( codeArray != null )
                            {
                                if( codeArray[0] == crd.OfficeCode ) crd.Department = codeArray[1];
                            }
                            holdMCode = crd.MemberCode;
                            holdDept = crd.Department;
                        }

                        crd.AccountCode = "SYO";
                        crd.CoTaskCode = "";
                        if( crd.ExistenceSlipNo() ) continue;
                        // 項目移送 
                        if( !crd.InsertCostReport() ) return 0;
                        procCount++;
                    }
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message );
                procCount = -1;
            }
            return procCount;
        }


        private void deleteCostReportData( string fileName, string officeCode )
        {
            using( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                SqlHandling sql = new SqlHandling( "D_CostReport" );
                while( !streamReader.EndOfStream )
                {
                    var line = streamReader.ReadLine();
                    var valArray = line.Split( ',' );
                    if( Convert.ToString( valArray[4] ) != "" )
                    {
                        if( sql.SelectAllData( "WHERE SlipNo = " + Convert.ToInt32( valArray[4] ) + " AND OfficeCode = '" + officeCode + "'" ) != null )
                            DeleteCostReport( "@slip", Convert.ToInt32( valArray[4] ) );
                    }
                }
            }
        }


        public int CountCostReportDataByCSVData( string fileName )
        {
            int procCount = 0;
            StringUtility sutil = new StringUtility();
            using( var streamReader = new StreamReader( fileName, System.Text.Encoding.Default ) )
            {
                while( !streamReader.EndOfStream )
                {
                    procCount++;
                }
            }
            return procCount;
        }


        public int CreateExportTaskData( string encName, string fileName )
        {
            int procCnt = 0;

            if( dt == null ) return -1;
            //TaskIndData tid;

            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for( int i = 0; i < dt.Rows.Count; i++ )
                {
                    tid = new TaskIndData( dt.Rows[i] );

                    writer.WriteLine( editTaskDataLine() );
                    procCnt++;
                }
                writer.Close();
            }
            return procCnt;
        }


        private string editTaskDataLine()
        {
            string textLine = "";

            TaskData td = new TaskData();
            td = td.SelectTaskData( tid.TaskID );

            // 得意先コード <- 業務番号
            textLine += tid.TaskCode;
            // 得意先名1 <- 業務名先頭40文字
            char[] strChar = tid.TaskName.ToCharArray();
            for( int i = 0; i < strChar.Length; i++ ) if( strChar[i] == ' ' ) strChar[i] = '　';
            tid.TaskName = new string( strChar );
            string taskData = util.FormFixedByteLengthLeft( tid.TaskName, 68 );
            textLine += util.TruncateByteLeft( taskData, 40 );
            // 得意先名2 <- 得意先名
            PartnersData pd = new PartnersData();
            string pName = pd.SelectPartnerName( td.PartnerCode );
            textLine += string.IsNullOrEmpty( pName ) ? util.PaddingInBytes( " ", "Char", 20 ) : util.PaddingInBytes( pName, "Char", 20 );
            // 先方担当者名 <- 業務名41文字目から28文字
            textLine += util.TruncateByteRight( taskData, 28 );
            // システム区分 <- 固定0
            textLine += "0";
            // 請求先コード <- 業務番号
            textLine += tid.TaskCode;
            // 実績管理 <- 固定0
            textLine += "0";
            // 住所1 <- 固定""
            // 20171008 Asakawa 出力データ異常の修正のため１行修正
            //textLine += util.PaddingInBytes( " ", "Char", 70 );
            textLine += util.PaddingInBytes(" ", "Char", 40);
            // 住所2 <- 契約金額
            textLine += util.PaddingInBytes( Convert.ToString( tid.Contract ), "Char", 30 );
            // 郵便番号 <- 契約年月日
            textLine += td.IssueDate.ToString( "yy/MM/dd" );
            // 電話番号 <- 工期開始
            textLine += util.PaddingInBytes( td.StartDate.ToString( "yyyy/MM/dd" ), "Char", 13 );
            // FAX番号 <- 工期終了
            textLine += util.PaddingInBytes( td.EndDate.ToString( "yyyy/MM/dd" ), "Char", 13 );
            // 得意先区分1 <- 得意先 99:その他
            textLine += "99";
            // 得意先区分2 <- 部門 
            textLine += Conv.DepartToSyoDep( tid.OfficeCode, tid.Department );
            // 得意先区分3 <- 営業担当者社員番号
            textLine += string.IsNullOrEmpty( td.SalesMCode ) ? "00" : Conv.ResizeMemberCode( td.SalesMCode.TrimEnd(), 2 );
            // 売価No <- 0固定
            textLine += "0";
            // 掛率 <- 100.0固定
            textLine += "100.0";
            // 税抜/税込 <- 0固定
            textLine += "0";
            // 主担当者コード <- 主担当者社員番号
            textLine += string.IsNullOrEmpty( tid.LeaderMCode ) ? "00" : Conv.ResizeMemberCode( tid.LeaderMCode.TrimEnd(), 2 );
            // 請求締日 <- 30固定
            textLine += "30";
            // 消費税端数 <- 0固定
            textLine += "0";
            // 消費税通知 <- 2固定
            textLine += "2";
            // 回収種別1 <- 0固定
            textLine += "0";
            // 回収種別境界額 <- 0固定
            textLine += "000000";
            // 回収種別2 <- 0固定
            textLine += "0";
            // 回収予定日 <- 0固定
            textLine += "000";
            // 回収方法 <- 0固定
            textLine += "0";
            // 与信限度額 <- 0固定
            textLine += "0000000000";
            // 繰越残高 <- 0固定
            textLine += "00000000000";
            // 官公庁区分 <- 0固定
            textLine += "0";
            // 敬称 <- 1固定
            textLine += "1";

            return textLine;
        }


        public int CreateExportCostMaster( string encName, string fileName )
        {
            int procCnt = 0;

            if( dt == null ) return -1;

            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for( int i = 0; i < dt.Rows.Count; i++ )
                {
                    cmd = new CostData( dt.Rows[i] );

                    writer.WriteLine( editCostMasterLine() );
                    procCnt++;
                }
                writer.Close();
            }
            return procCnt;
        }


        private string editCostMasterLine()
        {
            string textLine = "";

            // 商品コード <- 原価コード
            textLine += cmd.CostCode;
            // 品名 <- 品名
            textLine += string.IsNullOrEmpty( cmd.Item ) ? util.PaddingInBytes( " ", "Char", 36 ) : util.PaddingInBytes( cmd.Item, "Char", 36 );
            // システム区分 <- 固定0
            textLine += "0";
            // マスター区分 <- 固定0
            textLine += "0";
            // 在庫管理 <- 固定0
            textLine += "0";
            // 実績管理 <- 固定0
            textLine += "0";
            // 単位名 <- 単位
            textLine += string.IsNullOrEmpty( cmd.Unit ) ? util.PaddingInBytes( " ", "Char", 4 ) : util.PaddingInBytes( cmd.Unit, "Char", 4 );
            // 入数 <- 固定0000
            textLine += "0000";
            // 商品区分1 <- 固定00
            textLine += "00";
            // 商品区分2 <- 固定00
            textLine += "00";
            // 商品区分3 <- 固定00
            textLine += "00";
            // 税区分 <- 固定0
            textLine += "0";
            // 税込区分 <- 固定0
            textLine += "0";
            // 少数桁単価 <- 固定0
            textLine += "0";
            // 少数桁数量 <- 固定0
            textLine += "0";
            // 標準価格 <- 単価
            textLine += string.IsNullOrEmpty( Convert.ToString( cmd.Cost ) ) ? "000000000" : util.PaddingInBytes( cmd.Cost.ToString( "000000000" ), "Number", 9 );
            // 原価 <- 固定 000000000
            textLine += "000000000";
            // 売価1 <- 固定 000000000
            textLine += "000000000";
            // 売価2 <- 固定 000000000
            textLine += "000000000";
            // 売価3 <- 固定 000000000
            textLine += "000000000";
            // 売価4 <- 固定 000000000
            textLine += "000000000";
            // 売価5 <- 固定 000000000
            textLine += "000000000";
            // 主仕入先 <- 固定 空白4～13
            textLine += util.PaddingInBytes( " ", "Char", 4 );
            // 在庫単価 <- 固定 000000000
            textLine += "000000000";

            return textLine;
        }


        public int CreateExportKMaster( string encName, string fileName, string officeCode, string kubun )
        {
            int procCnt = 0;

            string[] nameArray = new string[99];
            Encoding sjisEnc = Encoding.GetEncoding( "Shift_JIS" );
            using( StreamWriter writer = new StreamWriter( fileName, false, sjisEnc ) )
            {
                for( int i = 0; i < 99; i++ )
                {
                    switch( kubun )
                    {
                        case "20":    // 担当者
                            nameArray[i] = mmd.SelectMemberName( ( i + 1 ).ToString( "000" ) );
                            writer.WriteLine( editKMembersLine( "20", i + 1, nameArray[i] ) );
                            break;
                        case "31":    // 得意先
                            writer.WriteLine( editKCustomersLine( i + 1 ) );
                            break;
                        case "32":    // 組織
                            writer.WriteLine( editKDepartmentsLine( i + 1 ) );
                            break;
                        case "41":    // 科目
                            writer.WriteLine( editKSubjectsLine( i + 1 ) );
                            break;
                        default:
                            break;
                    }
                    procCnt++;
                }
                if( kubun == "20" )
                {
                    for( int i = 0; i < 99; i++ )
                        writer.WriteLine( editKMembersLine( "33", i, nameArray[i] ) );
                }

                writer.Close();
            }
            return procCnt;
        }


        private string editKMembersLine( string kind, int idx, string nameStr )
        {
            string textLine = "";

            // データ種別 <- 20 or 33
            textLine += kind;
            // 区分コード <- 連番
            textLine += ( idx ).ToString( "00" );
            // 名称 <- 社員名
            textLine += string.IsNullOrEmpty( nameStr ) ? util.PaddingInBytes( " ", "Char", 30 ) : util.PaddingInBytes( nameStr, "Char", 30 );

            return textLine;
        }


        private string editKCustomersLine( int idx )
        {
            string textLine = "";

            // データ種別 <- 固定値 20
            textLine += "31";
            // 区分コード <- 連番
            textLine += ( idx + 1 ).ToString( "00" );
            // 名称 <- 部門名
            switch( idx )
            {
                case 97:
                    textLine += util.PaddingInBytes( "雑", "Char", 30 );
                    break;
                case 98:
                    textLine += util.PaddingInBytes( "その他", "Char", 30 );
                    break;
                case 99:
                    textLine += util.PaddingInBytes( "一般管理費", "Char", 30 );
                    break;
                default:
                    textLine += util.PaddingInBytes( " ", "Char", 30 );
                    break;
            }
            return textLine;
        }


        private string editKDepartmentsLine( int idx )
        {
            List<string> SyoDNmList = new List<string> { "","本社測量", "本社設計", "", "本社調査", "", "本社その他", "本社一般",
                                                        "郡山技術", "", "郡山その他", "郡山一般","", "相双技術", "", "相双その他", "相双一般", "", "関東技術", "関東一般" };
            string textLine = "";

            // データ種別 <- 固定値 20
            textLine += "32";
            // 区分コード <- 連番
            textLine += idx.ToString( "00" );
            // 名称 <- 組織名
            switch( idx )
            {
                case 1:
                case 2:
                case 4:
                case 6:
                case 7:
                case 8:
                case 10:
                case 11:
                case 12:
                case 14:
                case 15:
                case 17:
                case 18:
                    textLine += util.PaddingInBytes( SyoDNmList[idx], "Char", 30 );
                    break;
                default:
                    textLine += util.PaddingInBytes( " ", "Char", 30 );
                    break;
            }
            return textLine;
        }


        private string editKSubjectsLine( int idx )
        {
            List<string> SubjectList = new List<string> { "","人件費", "人件費（残業）", "経費", "", "外注費", "外注費（支払い）", "材料費","材料費（支払い）",
                                                        "その他", "", "出来高", "出来高資材" };
            string textLine = "";

            // データ種別 <- 固定値 20
            textLine += "41";
            // 区分コード <- 連番
            textLine += ( idx ).ToString( "00" );
            // 名称 <- 科目名
            switch( idx )
            {
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 11:
                case 12:
                    textLine += util.PaddingInBytes( SubjectList[idx], "Char", 30 );
                    break;
                default:
                    textLine += util.PaddingInBytes( " ", "Char", 30 );
                    break;
            }
            return textLine;
        }
    }
}

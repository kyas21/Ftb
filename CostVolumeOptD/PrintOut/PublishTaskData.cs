/*
 * Change history
 * 
 * 20190601 anonymous 引継ぎ部署を3行から6行に変更にともない印字開始位置変更出力範囲に部門追加対応
 * 
 */

using ClassLibrary;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PrintOut
{
    public class PublishTaskData
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishTaskData()
        {
        }

        public PublishTaskData( string fileName )
        {
            FileName = fileName;
        }
        //---------------------------------------------------------/
        //      Property
        //---------------------------------------------------------/
        public string FileName { get; set; }
        //---------------------------------------------------------/
        //      Method
        //---------------------------------------------------------/
        public void ExcelFile( TaskData td, TaskNoteData tnd, TaskIndData[] tid, PartnersData pd, TaskOp tod )
        {
            editExcelSheet( "TaskTransfer", td, tnd, tid, pd, tod );
        }


        //----------------------------------------------------------------------
        // SubRoutine
        //----------------------------------------------------------------------
        private void editExcelSheet( string sheetName, TaskData td, TaskNoteData tnd, TaskIndData[] tid, PartnersData pd, TaskOp tod )
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            using( oWBook = new XLWorkbook( FileName ) )
            {
                // シートを開く
                //oWSheet = oWBook.Worksheet(sheetName);
                oWSheet = oWBook.Worksheet( 1 );
                // 編集
                //MessageBox.Show("Excel書込み開始");
                editTaskDataPart( td );
                editTaskNoteDataPart( tnd );
                editTaskIndDataPart( tid );
                editPartnersDataPart( pd );
                editPersonsDataPart( tod );
                // 保存
                oWBook.SaveAs( tempFile );    // Excel保存
            }

            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す

            System.Diagnostics.Process.Start( "Excel.exe", tempFile );                    // 表示用Excel

            //if (File.Exists(tempFile)) File.Delete(tempFile);
        }


        private void editTaskDataPart( TaskData td )
        {
            if(td.IssueMark == 1 )
            {
                oWSheet.Cell( 1, 1 ).Value = "仮登録 承認待ち";
                oWSheet.Cell( 1, 1 ).Style.Font.FontSize = 14;
                oWSheet.Cell( 1, 1 ).Style.Font.FontColor = XLColor.Red;
            }

            if( td.AdmLevel == 0 )                              // 管理レベル 
            {
                oWSheet.Cell( 1, 12 ).Value = "■ 一般管理";
                oWSheet.Cell( 1, 10 ).Value = "□ 重要管理";
            }
            else
            {
                oWSheet.Cell( 1, 12 ).Value = "□ 一般管理";
                oWSheet.Cell( 1, 10 ).Value = "■ 重要管理";
            }

            if( td.VersionNo == 0 )                              // 発行回数
            {
                oWSheet.Cell( 2, 1 ).Value = "■ 仮着工";
                oWSheet.Cell( 2, 3 ).Value = "□ 第 0 回";
            }
            else
            {
                oWSheet.Cell( 2, 1 ).Value = "□ 仮着工";
                oWSheet.Cell( 2, 3 ).Value = "■ 第 " + td.VersionNo.ToString() + " 回";
                //oWSheet.Cell( 2, 5 ).Value = td.IssueDate;
            }
            oWSheet.Cell( 2, 5 ).Value = td.IssueDate;

            oWSheet.Cell( 3, 3 ).Value = td.TaskName;             // 業務名
            //oWSheet.Cell("C3").Value = td.TaskName;             // 業務名
            oWSheet.Cell( 4, 3 ).Value = td.TaskPlace;            // 施工箇所
            oWSheet.Cell( 5, 10 ).Value = td.StartDate.ToString( "yyyy年MM月dd日" ) + " ～ " + td.EndDate.ToString( "yyyy年MM月dd日" );      // 工期
            oWSheet.Cell( 7, 3 ).Value = td.PayNote;              // 支払条件
            oWSheet.Cell( 8, 3 ).Value = td.TaskOffice;           // 事務所
            oWSheet.Cell( 8, 11 ).Value = td.TaskLeader;          // 担当者
            oWSheet.Cell( 9, 3 ).Value = td.TelNo;                // TEL
            oWSheet.Cell( 9, 6 ).Value = td.FaxNo;                // FAX
            oWSheet.Cell( 9, 11 ).Value = td.EMail;               // e-Mail

            // 添付書類編集
            string[] itemArray = new string[] { "□打合せ協議簿　", "□見積内訳書　", "□設計図書　", "□契約書　", "□注文書　", "□着工依頼書　", "□その他（　　　）" };

            if( td.AttProceed == 1 ) itemArray[0] = "■打合せ協議簿　";
            if( td.AttEstimate == 1 ) itemArray[1] = "■見積内訳書　";
            if( td.AttDesign == 1 ) itemArray[2] = "■設計図書　";
            if( td.AttContract == 1 ) itemArray[3] = "■契約書　";
            if( td.AttOrder == 1 ) itemArray[4] = "■注文書　";
            if( td.AttStart == 1 ) itemArray[5] = "■着工依頼書　";
            if( td.AttOther == 1 ) itemArray[6] = "■その他（" + td.AttOtherCont + "）";
            oWSheet.Cell( 20, 3 ).Value = "";                     // 添付書類
            for( int i = 0; i < itemArray.Length; i++ ) oWSheet.Cell( 20, 3 ).Value += itemArray[i];

            oWSheet.Cell( 21, 5 ).Value = td.OrderNote;           // 発注形態

            // 仕様書関係編集
            itemArray = new string[] { "□共通仕様書　", "□特記仕様書　", "□その他（　　　）" };
            if( td.CommonSpec == 1 ) itemArray[0] = "■共通仕様書　";
            if( td.ExclusiveSpec == 1 ) itemArray[1] = "■特記仕様書　";
            if( td.OtherSpec == 1 ) itemArray[2] = "■その他（" + td.SpecCont + "）";
            oWSheet.Cell( 22, 5 ).Value = "";                     // 仕様書関係
            for( int i = 0; i < itemArray.Length; i++ ) oWSheet.Cell( 22, 5 ).Value += itemArray[i];

            // 指示
            itemArray = new string[] { "□実行予算書", "□業務計画書" };
            if( td.OrderBudget == 1 ) itemArray[0] = "■実行予算書";
            if( td.OrderPlanning == 1 ) itemArray[1] = "■業務計画書";
            oWSheet.Cell( 43, 5 ).Value = itemArray[0] + Environment.NewLine + itemArray[1];
        }


        private void editTaskNoteDataPart( TaskNoteData tnd )
        {
            if( tnd == null ) return;
            if( tnd.Note == null || tnd.Note == "" || tnd.Note == " " ) return;
            string[] nLine = tnd.Note.Split( new[] { Environment.NewLine }, StringSplitOptions.None );
            int i = 0;
            foreach( string item in nLine )
            {
                oWSheet.Cell( 24 + i, 2 ).Value = item;
                i++;
            }
        }


        private void editTaskIndDataPart( TaskIndData[] tid )
        {
            List<string> oList = new List<string> { "H", "K", "S", "T" };
            List<string> dList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "N" };
            List<int> col = new List<int> { 4, 6, 8, 10, 12 };
            List<int> row = new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            decimal[] sumV = new decimal[oList.Count];
            decimal[] sumH = new decimal[dList.Count];
            int[] pmV = new int[oList.Count];
            int[] pmH = new int[dList.Count];
            int oP, dP;

            // 業務番号
            string taskCodeArray = "";
            //for( int i = 0; i < tid.Length; i++ )
            //{
            //    taskCodeArray += tid[i].TaskCode + "(" + tid[i].OfficeCode + "), ";
            //}
            for( int i = 0; i < tid.Length; i++ )
            {
                if(!string.IsNullOrEmpty(tid[i].TaskCode))
                    taskCodeArray += tid[i].TaskCode + "(" + tid[i].OfficeCode + "), ";
            }
            oWSheet.Cell( 2, 10 ).Value = taskCodeArray;
            // 契約金額
            //for( int i = 0; i < tid.Length; i++ )
            //{
            //    dP = dList.IndexOf( tid[i].TaskCode[0].ToString() );
            //    oP = oList.IndexOf( tid[i].OfficeCode );
            //    if( tid[i].ProvMark == 1 ) oWSheet.Cell( row[dP], col[oP] - 1 ).Value = "仮 ";
            //    oWSheet.Cell( row[dP], col[oP] ).Value = tid[i].Contract;

            //    sumV[oP] += tid[i].Contract;
            //    sumH[dP] += tid[i].Contract;
            //    pmV[oP] += tid[i].ProvMark;
            //    pmH[dP] += tid[i].ProvMark;
            //}
            for( int i = 0; i < tid.Length; i++ )
            {
                if( string.IsNullOrEmpty( tid[i].TaskCode ) ) continue;
                dP = dList.IndexOf( tid[i].TaskCode[0].ToString() );
                oP = oList.IndexOf( tid[i].OfficeCode );
                if( tid[i].ProvMark == 1 ) oWSheet.Cell( row[dP], col[oP] - 1 ).Value = "仮 ";
                oWSheet.Cell( row[dP], col[oP] ).Value = tid[i].Contract;

                sumV[oP] += tid[i].Contract;
                sumH[dP] += tid[i].Contract;
                pmV[oP] += tid[i].ProvMark;
                pmH[dP] += tid[i].ProvMark;
            }

            decimal sumA = 0M;
            int pmA = 0;
            for( int i = 0; i < dList.Count; i++ )
            {
                oWSheet.Cell( row[i], col[oList.Count] ).Value = sumH[i];
                if( pmH[i] > 0 ) oWSheet.Cell( row[i], col[oList.Count] - 1 ).Value = "仮";
                sumA += sumH[i];
                pmA += pmH[i];
            }

            for( int i = 0; i < oList.Count; i++ )
            {
                oWSheet.Cell( row[dList.Count], col[i] ).Value = sumV[i];
                if( pmV[i] > 0 ) oWSheet.Cell( row[dList.Count], col[i] - 1 ).Value = "仮";

            }

            oWSheet.Cell( row[dList.Count], col[oList.Count] ).Value = sumA;
            if( pmA > 0 ) oWSheet.Cell( row[dList.Count], col[oList.Count] - 1 ).Value = "仮";
        }


        private void editPartnersDataPart( PartnersData pd )
        {
            oWSheet.Cell( 5, 3 ).Value = pd.PartnerName;          // 発注者名
            oWSheet.Cell( 6, 3 ).Value = pd.Address;              // 発注者住所
            oWSheet.Cell( 6, 12 ).Value = pd.TelNo;               // 電話番号
            oWSheet.Cell( 7, 12 ).Value = pd.FaxNo;               // FAX番号
        }


        private void editPersonsDataPart( TaskOp tod )
        {
            // 20190601 anonymous 帳票の業務引継部署数　3行から6行として印字開始位置変更
            //int stln = 37;
            int stln = 34;
            // 印字開始位置指定変更終了

            for( int i = 0; i < tod.MgrDept.Length; i++ )
            //for( int i = 0; i < 3; i++ )
            {
                if( !String.IsNullOrEmpty( tod.MgrDept[i] ) )
                {
                    oWSheet.Cell( stln + i, 2 ).Value = tod.MgrDept[i];
                    oWSheet.Cell( stln + i, 4 ).Value = tod.MgrName[i];
                    oWSheet.Cell( stln + i, 6 ).Value = tod.AppDate[0];

                    oWSheet.Cell( stln + i, 8 ).Value = tod.MbrDept[i];
                    oWSheet.Cell( stln + i, 10 ).Value = tod.MbrName[i];
                    oWSheet.Cell( stln + i, 12 ).Value = tod.AppDate[0];
                }
            }

            for( int i = 0; i < tod.AppName.Length; i++ )
            {
                if( !String.IsNullOrEmpty( tod.AppName[i] ) )
                {
                    oWSheet.Cell( 42, 9 - ( i * 2 ) ).Value = tod.AppName[i];
                    oWSheet.Cell( 44, 9 - ( i * 2 ) ).Value = tod.AppDate[i].ToLongDateString();
                    //oWSheet.Cell( 44, 9 - ( i * 2 ) ).Value = tod.AppDate[i].ToString("F");
                }
            }
        }

    }
}

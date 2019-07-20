using ClassLibrary;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintOut
{
    public class PublishTaskNoConfList
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        private string fileName;
        TaskNoConfList[] tncA;
        const int posRow = 28;
        private string officeName;
        private string outputFile;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishTaskNoConfList()
        {
        }

        public PublishTaskNoConfList( string fileName, TaskNoConfList[] tncA )
        {
            this.fileName = fileName;
            this.tncA = tncA;
        }

        //---------------------------------------------------------/
        //      Property
        //---------------------------------------------------------/
        public string FileName { get; set; }
        //---------------------------------------------------------/
        //      Method
        //---------------------------------------------------------/
        public void ExcelFile()
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;                // マウスカーソルを砂時計(Wait)

            try
            {
                using( oWBook = new XLWorkbook( fileName ) )
                {
                    if( tncA == null || tncA.Length == 0 )
                    {
                        DMessage.DataNotExistence( "中断します！" );
                        return;
                    }

                    // 編集
                    oWSheet = oWBook.Worksheet( 1 );      // シートを開く
                    readyExcelRows( tncA.Length, 4 );
                    int sNo = 5;
                    for( int i = 0; i < tncA.Length; i++ )
                    {
                        using( IXLRange SetRange = oWSheet.Range( "A5:M5" ) )
                            // テンプレートデータ行コピー/ペースト
                            SetRange.CopyTo( oWSheet.Cell( sNo + i, 1 ) );

                        if( i == 0 )
                        {
                            officeName = tncA[i].OfficeName;
                            oWSheet.Cell( 2, 3 ).Value = DateTime.Today;
                            oWSheet.Cell( 3, 3 ).Value = officeName;
                        }

                        oWSheet.Cell( sNo + i, 1 ).Value = i+1;
                        oWSheet.Cell( sNo + i, 2 ).Value = tncA[i].TaskCode;
                        oWSheet.Cell( sNo + i, 3 ).Value = tncA[i].TaskName;
                        oWSheet.Cell( sNo + i, 4 ).Value = tncA[i].VersionNo;
                        oWSheet.Cell( sNo + i, 5 ).Value = tncA[i].IssueDate;
                        oWSheet.Cell( sNo + i, 6 ).Value = tncA[i].SalesMName;
                        oWSheet.Cell( sNo + i, 7 ).Value = tncA[i].SalesMInputDate;
                        oWSheet.Cell( sNo + i, 8 ).Value = tncA[i].Approval;
                        oWSheet.Cell( sNo + i, 9 ).Value = tncA[i].ApprovalDate;
                        oWSheet.Cell( sNo + i, 10 ).Value = tncA[i].MakeOrder;
                        oWSheet.Cell( sNo + i, 11 ).Value = tncA[i].MakeOrderDate;
                        oWSheet.Cell( sNo + i, 12 ).Value = tncA[i].ConfirmAdm;
                        oWSheet.Cell( sNo + i, 13 ).Value = tncA[i].ConfirmDate;

                        if( i != 0 )
                            oWSheet.Range( sNo + i, 1, sNo + i, 13 ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        if( i == tncA.Length - 1 )
                            oWSheet.Range( sNo + i, 1, sNo + i, 13 ).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        else
                            oWSheet.Range( sNo + i, 1, sNo + i, 13 ).Style.Border.BottomBorder = XLBorderStyleValues.Hair;
                    }

                }

                // 保存
                oWBook.SaveAs( tempFile );                    // Excel保存
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message );
                Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
                return;
            }

            Cursor.Current = Cursors.Default;               // マウスカーソルを戻す

            System.Diagnostics.Process.Start( "Excel.exe", tempFile );                    // 表示用Excel
            // pdf出力にする場合は、上記 System.DiafnosticsのLineをコメントアウトし、下記DateTime以下のコメントを外す。
            // pdf file 出力 
            //DateTime now = DateTime.Now;
            //outputFile = System.IO.Path.GetDirectoryName( tempFile ) + @"\業務引継書承認未完了一覧表_" + officeName + "_" + "_" + now.ToString( "yyMMddHHmmss" );
            //PublishExcelToPdf etp = new PublishExcelToPdf();
            //etp.ExcelToPDF( tempFile, outputFile );
            
            //if( File.Exists( tempFile ) ) File.Delete( tempFile );

        }


        //----------------------------------------------------------------------
        // SubRoutine
        //----------------------------------------------------------------------
        /// <summary>
        /// データ件数と現在のエクセルの行数を比較し、エクセルの行数が少ない場合は
        /// エクセルに行を追加する。
        /// 比較の差異は列ヘッダーの行数、合計行の行数は除外する。
        /// </summary>
        /// <param name="lineCount"></param>
        /// <param name="exLine"></param>
        private void readyExcelRows( int lineCount, int exLine )
        {
            // 不足行追加
            if( 1 < lineCount )
            {
                var rowCount = oWSheet.Row( 6 ).InsertRowsBelow( lineCount - 1 );
                oWSheet.Rows( "6:" + ( 6 + lineCount - 1 - 1 ) ).Height = oWSheet.Row( 5 ).Height;
            }

            oWSheet.Row( 6 + lineCount - 1 ).Delete();
        }
    }
}

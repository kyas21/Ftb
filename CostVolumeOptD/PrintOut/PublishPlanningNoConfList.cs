using ClassLibrary;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintOut
{
    public class PublishPlanningNoConfList
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        private string fileName;
        PlanningNoConfList[] pncA;
        const int posRow = 28;
        private string officeName;
        private string outputFile;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishPlanningNoConfList()
        {
        }

        public PublishPlanningNoConfList( string fileName, PlanningNoConfList[] pncA )
        {
            this.fileName = fileName;
            this.pncA = pncA;
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
                    if( pncA == null || pncA.Length == 0 )
                    {
                        DMessage.DataNotExistence( "中断します！" );
                        return;
                    }

                    // 編集
                    oWSheet = oWBook.Worksheet( 1 );      // シートを開く
                    readyExcelRows( pncA.Length, 4 );
                    int sNo = 5;
                    for( int i = 0; i < pncA.Length; i++ )
                    {
                        using( IXLRange SetRange = oWSheet.Range( "A5:N5" ) )
                            // テンプレートデータ行コピー/ペースト
                            SetRange.CopyTo( oWSheet.Cell( sNo + i, 1 ) );

                        if( i == 0 )
                        {
                            officeName = pncA[i].OfficeName;
                            oWSheet.Cell( 2, 3 ).Value = DateTime.Today;
                            oWSheet.Cell( 3, 3 ).Value = officeName;
                        }

                        oWSheet.Cell( sNo + i, 1 ).Value = i + 1;
                        oWSheet.Cell( sNo + i, 2 ).Value = pncA[i].TaskCode;
                        oWSheet.Cell( sNo + i, 3 ).Value = pncA[i].TaskName;
                        oWSheet.Cell( sNo + i, 4 ).Value = pncA[i].VersionNo;
                        oWSheet.Cell( sNo + i, 5 ).Value = pncA[i].CreateMName;
                        oWSheet.Cell( sNo + i, 6 ).Value = pncA[i].CreateDate;
                        oWSheet.Cell( sNo + i, 7 ).Value = pncA[i].ConfirmMName;
                        oWSheet.Cell( sNo + i, 8 ).Value = pncA[i].ConfirmDate;
                        oWSheet.Cell( sNo + i, 9 ).Value = pncA[i].ScreeningMName;
                        oWSheet.Cell( sNo + i, 10 ).Value = pncA[i].ScreeningDate;
                        oWSheet.Cell( sNo + i, 11).Value = pncA[i].ApOfficerMName;
                        oWSheet.Cell( sNo + i, 12 ).Value = pncA[i].ApOfficerDate;
                        oWSheet.Cell( sNo + i, 13 ).Value = pncA[i].ApPresidentMName;
                        oWSheet.Cell( sNo + i, 14 ).Value = pncA[i].ApPresidentDate;

                        if( i != 0 )
                            oWSheet.Range( sNo + i, 1, sNo + i, 14 ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        if( i == pncA.Length - 1 )
                            oWSheet.Range( sNo + i, 1, sNo + i, 14 ).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        else
                            oWSheet.Range( sNo + i, 1, sNo + i, 14 ).Style.Border.BottomBorder = XLBorderStyleValues.Hair;
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

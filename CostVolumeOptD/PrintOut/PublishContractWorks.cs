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
    public class PublishContractWorks
    {

        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;

        private DataGridView dgv;
        private PublishData pd;
        private string fileName;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishContractWorks()
        {
        }

        public PublishContractWorks( string fileName, PublishData pd, DataGridView dgv )
        {
            this.fileName = fileName;
            this.pd = pd;
            this.dgv = dgv;
        }
        //---------------------------------------------------------// 
        //      Property
        //---------------------------------------------------------//
        public DataGridView Dgv
        {
            get { return this.dgv; }
            set { this.dgv = value; }
        }


        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }
        //---------------------------------------------------------//
        //      Method
        //---------------------------------------------------------//
        public void ExcelFile( string sheetName, PublishData pd, DataGridView dgv )
        {
            editExcelSheet( sheetName, pd, dgv );
        }


        //---------------------------------------------------------//
        // SubRoutine
        //---------------------------------------------------------//
        private void editExcelSheet( string sheetName, PublishData pd, DataGridView dgv )
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            try
            {
                using( oWBook = new XLWorkbook( fileName ) )
                {
                    oWSheet = oWBook.Worksheet( sheetName );
                    switch( sheetName )
                    {
                        case "ContractWorks":
                            editContractWorks( pd, dgv );
                            break;
                        case "ContractSummary":
                            editContractSummary( pd, dgv );
                            break;
                        default:
                            break;
                    }
                    oWBook.SaveAs( tempFile );    // Excel保存
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message );
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                return;
            }

            Cursor.Current = Cursors.Default;       // マウスカーソルを戻す
            System.Diagnostics.Process.Start( "Excel.exe", tempFile );                    // 表示用Excel
        }


        private void editContractWorks( PublishData pd, DataGridView dgv )
        {
            //MessageBox.Show( "Excel書込み開始" );

            oWSheet.Cell( 3, 1 ).Value = pd.vYear + "年度";
            oWSheet.Cell( 3, 3 ).Value = "部署　　：" + pd.OfficeName + pd.DepartName;
            oWSheet.Cell( 5, 1 ).Value = "業務番号：" + pd.TaskCode;
            oWSheet.Cell( 5, 3 ).Value = "業務名　：" + pd.TaskName;
            oWSheet.Cell( 6, 3 ).Value = "取引先名：" + pd.PartnerName;

            oWSheet.Row( 8 ).Height = 24;

            readyExcelRows( dgv, 1, 18, 8 );
            const int SR = 9;       // Excel Sheet Start Row No.

            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["MName"].Value );
                oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["MCode"].Value );
                oWSheet.Cell( SR + i, 3 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );
                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["Price"].Value );
                oWSheet.Cell( SR + i, 5 ).Value = Convert.ToString( dgv.Rows[i].Cells["WorkSum"].Value );
                oWSheet.Cell( SR + i, 6 ).Value = Convert.ToString( dgv.Rows[i].Cells["CostSum"].Value );
                int k = 7;
                for( int j = 0; j < 12; j++ )
                {
                    oWSheet.Cell( SR + i, k ).Value =
                                DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Work" + j.ToString( "00" )].Value ) );
                    oWSheet.Cell( SR + i, k+1).Value =
                                DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Cost" + j.ToString( "00" )].Value ) );
                    k += 2;
                }

                oWSheet.Row( SR + i ).Height = 24;
            }
        }
        

        private void editContractSummary( PublishData pd, DataGridView dgv )
        {
            //MessageBox.Show( "Excel書込み開始" );

            oWSheet.Cell( 3, 1 ).Value = pd.vYear + "年度";
            oWSheet.Cell( 3, 2 ).Value = "事業所：" + pd.OfficeName;

            oWSheet.Row( 7 ).Height = 24;

            readyExcelRows( dgv, 1, 16, 7 );
            const int SR = 8;       // Excel Sheet Start Row No.

            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["TaskCode"].Value );
                oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["PartnerName"].Value );
                oWSheet.Cell( SR + i, 3 ).Value = Convert.ToString( dgv.Rows[i].Cells["TaskName"].Value );

                if( !string.IsNullOrEmpty( Convert.ToString( dgv.Rows[i].Cells["TaskName"].Value ) ) )
                {
                    int k = 4;
                    for( int j = 0; j < 12; j++ )
                    {
                        oWSheet.Cell( SR + i, k ).Value =
                                    DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Cost" + j.ToString( "00" )].Value ) );
                        k++;
                    }
                    oWSheet.Cell( SR + i, k ).Value =
                                    DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["CostSum"].Value ) );
                }
                oWSheet.Row( SR + i ).Height = 24;
            }
        }



        public void CreateExcelForPdf( string sheetName, PublishData pd, DataGridView dgv )
        {
            //string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            string tempFile = "";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)
            try
            {
                using( oWBook = new XLWorkbook( fileName ) )
                {
                    oWSheet = oWBook.Worksheet( sheetName );
                    switch( sheetName )
                    {
                        case "Volume":
                            tempFile = Folder.DefaultLocation() + @"\" + pd.vTaskCode + ".xlsx";
                            editContractWorks( pd, dgv );
                            break;
                        default:
                            break;
                    }
                    oWBook.SaveAs( tempFile );      // Excel保存
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message );
                Cursor.Current = Cursors.Default;   // マウスカーソルを戻す
                return;
            }
            Cursor.Current = Cursors.Default;       // マウスカーソルを戻す
        }


        /// <summary>
        /// DataGridViewの最後にデータが存在する行数と現在のエクセルの行数を比較し、エクセルの行数が少ないようなら
        /// エクセルの行数を増加させる。
        /// 比較の差異は列ヘッダーの行数、合計行の行数は除外する。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="stCol"> Excel 有効な最初の列番号</param>
        /// <param name="endCol"> Excel 有効な最後の列番号</param>
        /// <param name="exLine"> Excel ヘッダー行などデータ行でない行数</param>
        private void readyExcelRows( DataGridView dgv, int stCol, int endCol, int exLine )
        {
            int lastRows = dgvRowsCount( dgv, stCol, endCol );          // 表示対象の最終行 
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                                   // タイトル行と合計行を行数から除く
            if( lastRows > sheetRowsCount )
            {
                //var rowCount = oWSheet.Row( exLine + 1 ).InsertRowsBelow( lastRows - sheetRowsCount );
                var rowCount = oWSheet.Row( exLine + 1 ).InsertRowsBelow( lastRows - sheetRowsCount - 1 );
            }
        }



        /// <summary>
        /// 対象DataGridViewの指定された列の範囲にデータが存在するか調べる。
        /// データの存在した最後の行数を返す。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="stCol"></param>
        /// <param name="endCol"></param>
        /// <returns></returns>
        private int dgvRowsCount( DataGridView dgv, int stCol, int endCol )
        {
            int rowsCount = 0;
            int colsCount = 0;
            for( int i = 0; i < dgv.Rows.Count; i++ )
            {
                for( int j = stCol; j < endCol; j++ )
                {
                    if( Convert.ToString( dgv.Rows[i].Cells[j].Value ) != "" ) colsCount++;
                }
                if( colsCount > 0 ) rowsCount = i;
                colsCount = 0;
            }
            return rowsCount + 1;
        }

    }
}

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
    public class PublishTaskList
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        private string fileName;
        TaskList[] tla;
        const int posRow = 28;
        //private string[] repoDate;
        private string outputFile;
        //private string itemCode;
        private string officeName;
        private string departName;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishTaskList()
        {
        }

        public PublishTaskList(string fileName, TaskList[] tla)
        {
            this.fileName = fileName;
            this.tla = tla;
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

            // Wakamatsu 20170315
            try
            {
                using (oWBook = new XLWorkbook(fileName))
                {
                    if (tla == null || tla.Length == 0)
                    {
                        DMessage.DataNotExistence("中断します！");
                        return;
                    }
                    // 編集

                    oWSheet = oWBook.Worksheet(1);      // シートを開く
                    // Wakamatsu 20170315
                    //readyExcelRows(tla.Length, 5);
                    readyExcelRows(tla.Length, 4);
                    int sNo = 5;
                    for (int i = 0; i < tla.Length; i++)
                    {
                        // Wakamatsu 20170315
                        using (IXLRange SetRange = oWSheet.Range("A5:I5"))
                            // テンプレートデータ行コピー/ペースト
                            SetRange.CopyTo(oWSheet.Cell(sNo + i, 1));
                        // Wakamatsu 20170315

                        if (i == 0)
                        {
                            officeName = tla[i].OfficeName;
                            departName = tla[i].DepartName;
                            // Wakamatsu 20170315
                            //oWSheet.Cell(1, 7).Value = DateTime.Today;
                            oWSheet.Cell(2, 2).Value = DateTime.Today;
                            oWSheet.Cell(3, 2).Value = officeName + " " + departName;
                            // Wakamatsu 20170315
                        }
                        oWSheet.Cell(sNo + i, 1).Value = tla[i].TaskCode;
                        oWSheet.Cell(sNo + i, 2).Value = tla[i].TaskName;
                        oWSheet.Cell(sNo + i, 3).Value = tla[i].PartnerName;
                        oWSheet.Cell(sNo + i, 4).Value = tla[i].Contract;
                        oWSheet.Cell(sNo + i, 5).Value = tla[i].StartDate;
                        oWSheet.Cell(sNo + i, 6).Value = tla[i].EndDate;
                        oWSheet.Cell(sNo + i, 7).Value = tla[i].SalesM;
                        oWSheet.Cell(sNo + i, 8).Value = tla[i].LeaderM;
                        oWSheet.Cell(sNo + i, 9).Value = tla[i].IssueDate;

                        // Wakamatsu 20170315
                        if (i != 0)
                            oWSheet.Range(sNo + i, 1, sNo + i, 9).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        if (i == tla.Length - 1)
                            oWSheet.Range(sNo + i, 1, sNo + i, 9).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        else
                            oWSheet.Range(sNo + i, 1, sNo + i, 9).Style.Border.BottomBorder = XLBorderStyleValues.Hair;
                        // Wakamatsu 20170315
                    }

                }

                // 保存
                oWBook.SaveAs(tempFile);                    // Excel保存
            }
            // Wakamatsu 20170315
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
                return;
            }
            // Wakamatsu 20170315

            Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
            //System.Diagnostics.Process.Start("Excel.exe", tempFile);                    // 表示用Excel
            // pdf file 出力
            DateTime now = DateTime.Now;
            outputFile = System.IO.Path.GetDirectoryName(tempFile) + @"\業務一覧表_" + officeName + "_" + departName + "_" + now.ToString("yyMMddHHmmss");
            PublishExcelToPdf etp = new PublishExcelToPdf();
            etp.ExcelToPDF(tempFile, outputFile);

            if (File.Exists(tempFile)) File.Delete(tempFile);

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
        private void readyExcelRows(int lineCount, int exLine)
        {
            // 不足行追加
            // Wakamatsu 20170315
            //int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            //sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            //if (sheetRowsCount < lineCount)
            if (1 < lineCount)
            // Wakamatsu 20170315
            {
                // Wakamatsu 20170315
                //var rowCount = oWSheet.Row(6).InsertRowsBelow(lineCount - sheetRowsCount);
                var rowCount = oWSheet.Row(6).InsertRowsBelow(lineCount - 1);
                oWSheet.Rows("6:" + (6 + lineCount - 1 - 1)).Height = oWSheet.Row(5).Height;
                // Wakamatsu 20170315
            }

            // Wakamatsu 20170315
            oWSheet.Row(6 + lineCount - 1).Delete();
        }


    }
}

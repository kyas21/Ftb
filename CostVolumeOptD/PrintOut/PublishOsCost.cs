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
    public class PublishOsCost
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        private string fileName;
        private OsPaymentData[] pmd;
        private OsPayOffData[] pod;
        private OsPayOffNoteData opn;
        public static readonly List<string> procList = new List<string> { "OsPayOff", "OsPayOffS", "OsPayment" };
        public static readonly List<string> pNameList = new List<string> { "外注精算書（起案）", "外注精算書（起案）2", "外注出来高調書一覧表" };
        const string payoff = "OsPayOff";
        const string payoffS = "OsPayOffS";
        const string payment = "OsPayment";
        const int posRow = 12;
        string[] repoDate;
        string outputFile;
        string itemCode;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishOsCost()
        {
        }

        public PublishOsCost(string fileName)
        {
            this.fileName = fileName;
        }

        public PublishOsCost(string fileName, OsPaymentData[] pmd)
        {
            this.fileName = fileName;
            this.pmd = pmd;
        }

        public PublishOsCost(string fileName, OsPayOffData[] pod)
        {
            this.fileName = fileName;
            this.pod = pod;
        }


        public PublishOsCost(string fileName, OsPayOffData[] pod, OsPayOffNoteData opn)
        {
            this.fileName = fileName;
            this.pod = pod;
            this.opn = opn;
        }

        public PublishOsCost(string fileName, OsPayOffData[] pod, string[] reportingDate)
        {
            this.fileName = fileName;
            this.pod = pod;
            repoDate = reportingDate;
        }
        //---------------------------------------------------------/
        //      Property
        //---------------------------------------------------------/
        public string FileName { get; set; }
        //---------------------------------------------------------/
        //      Method
        //---------------------------------------------------------/
        public void ExcelFile(string proc)
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;                // マウスカーソルを砂時計(Wait)

            // Wakamatsu 20170313
            try
            {
                using (oWBook = new XLWorkbook(fileName))
                {
                    // 編集
                    switch (procList.IndexOf(proc))
                    {
                        case 0:
                        case 1:
                            if (pod == null || pod.Length == 0)
                            {
                                DMessage.DataNotExistence("中断します！");
                                return;
                            }
                            //MessageBox.Show("Excel書込み開始");
                            if (proc == payoff)
                            {
                                oWSheet = oWBook.Worksheet(1);      // シートを開く
                                editOsPayOff(pod);                  // 起案書発行
                            }
                            else
                            {
                                // Wakamatsu 20170313
                                //decimal sum = 0M;
                                //int page = pod.Length / posRow;
                                //if (pod.Length % posRow > 0) page++;
                                //for (int i = 0; i < page; i++)
                                //{
                                //    oWSheet = oWBook.Worksheet(i + 1);            // シートを開く
                                //    oWSheet.Cell(3, 5).Value = "'" + (i + 1).ToString() + "/" + page.ToString();
                                //    sum = editOsPayOffS(pod, posRow * i, sum);     // 起案書発行
                                //}
                                oWSheet = oWBook.Worksheet(1);              // シートを開く
                                oWSheet.Cell(3, 5).Value = "'1/1";
                                editOsPayOffS(pod, posRow, 0);              // 起案書発行
                                // Wakamatsu 20170313
                            }
                            break;
                        case 2:
                            if (pmd == null || pmd.Length == 0)
                            {
                                DMessage.DataNotExistence("中断します！");
                                return;
                            }
                            //MessageBox.Show("Excel書込み開始");
                            oWSheet = oWBook.Worksheet(1);      // シートを開く
                            editOsPayment(pmd);                 // 調書発行
                            break;
                        default:
                            break;

                    }

                    // 保存
                    oWBook.SaveAs(tempFile);                    // Excel保存
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
                return;
            }
            // Wakamatsu 20170313

            Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
            //System.Diagnostics.Process.Start("Excel.exe", tempFile);                    // 表示用Excel
            // pdf file 出力
            DateTime now = DateTime.Now;
            outputFile = System.IO.Path.GetDirectoryName(tempFile) + @"\" + pNameList[procList.IndexOf(proc)] + "_" + itemCode + "_" + now.ToString("yyMMddHHmmss");
            PublishExcelToPdf etp = new PublishExcelToPdf();
            // Wakamatsu 20170313
            //etp.ExcelToPDF(tempFile, outputFile);
            if (etp.ExcelToPDF(tempFile, outputFile) == true)
                if (File.Exists(tempFile)) File.Delete(tempFile);

        }


        //----------------------------------------------------------------------
        // SubRoutine
        //----------------------------------------------------------------------

        private void editOsPayment(OsPaymentData[] pmd)
        {
            readyExcelRows(pmd.Length, 5);
            int sNo = 6;
            for (int i = 0; i < pmd.Length; i++)
            {
                if (i == 0)
                {
                    //oWSheet.Cell(3, 1).Value = pmd[i].ReportDate.ToString("Y");
                    //oWSheet.Cell(3, 3).Value = "部署：";
                    //oWSheet.Cell(3, 4).Value = Conv.bList[Conv.OfficeCodeIndex(pmd[i].OfficeCode)] + "／" + Conv.dNmList[Conv.DepNoIndex(pmd[i].Department)];
                    oWSheet.Cell(1, 4).Value = Convert.ToString(pmd[i].ReportDate.Month) + "月分 外注出来高調書（" + Conv.dNmList[Conv.DepNoIndex(pmd[i].Department)] + "部）";
                    oWSheet.Cell(1, 7).Value = DateTime.Today;
                    itemCode = pmd[i].ItemCode;
                }
                oWSheet.Cell(sNo + i, 1).Value = pmd[i].Item;
                oWSheet.Cell(sNo + i, 2).Value = pmd[i].OrderNo;
                oWSheet.Cell(sNo + i, 3).Value = pmd[i].TaskCode;
                oWSheet.Cell(sNo + i, 4).Value = pmd[i].TaskName;
                oWSheet.Cell(sNo + i, 5).Value = pmd[i].OrderAmount;
                oWSheet.Cell(sNo + i, 6).Value = pmd[i].SAmount;
                oWSheet.Cell(sNo + i, 7).Value = pmd[i].Amount;
                oWSheet.Cell(sNo + i, 8).Value = pmd[i].OrderAmount - (pmd[i].SAmount + pmd[i].Amount);
            }
        }


        private void editOsPayOff(OsPayOffData[] pod)
        {
            //readyExcelRows(pod.Length, 6);
            //int sNo = 6;
            readyExcelRows(pod.Length, 9);
            int sNo = 9;
            decimal sum = 0M;
            for (int i = 0; i < pod.Length; i++)
            {
                if (i == 0)
                {
                    //oWSheet.Cell(1, 2).Value = pod[i].ReportDate.ToString("Y");
                    //oWSheet.Cell(2, 2).Value = Conv.bList[Conv.OfficeCodeIndex(pod[i].OfficeCode)] + "／" + Conv.dNmList[Conv.DepNoIndex(pod[i].Department)];
                    //oWSheet.Cell(3, 2).Value = pod[i].Item;
                    oWSheet.Cell(2, 1).Value = Convert.ToString(pod[i].ReportDate.Month) + "月分";
                    oWSheet.Cell(2, 6).Value = Conv.bList[Conv.OfficeCodeIndex(pod[i].OfficeCode)] + "／" + Conv.dNmList[Conv.DepNoIndex(pod[i].Department)];
                    oWSheet.Cell(5, 2).Value = pod[i].Item;
                    itemCode = pod[i].ItemCode;
                }
                //oWSheet.Cell(sNo + i, 1).Value = pod[i].TaskCode;
                //oWSheet.Cell(sNo + i, 2).Value = pod[i].TaskName;
                //oWSheet.Cell(sNo + i, 5).Value = pod[i].Cost;
                //oWSheet.Cell(sNo + i, 6).Value = pod[i].LeaderMName;
                oWSheet.Cell(sNo + i, 1).Value = pod[i].TaskCode;
                oWSheet.Cell(sNo + i, 2).Value = pod[i].TaskName;
                oWSheet.Cell(sNo + i, 6).Value = pod[i].Cost;
                oWSheet.Cell(sNo + i, 7).Value = pod[i].LeaderMName;
                sum += pod[i].Cost;
            }
            int lastRows = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            //oWSheet.Cell(lastRows, 5).Value = sum;
            oWSheet.Cell(lastRows, 6).Value = sum;
        }


        private decimal editOsPayOffS(OsPayOffData[] pod, int stRec, decimal sum)
        {
            // Wakamatsu 20170313
            readyExcelRows(pod.Length, 6);
            FromatCopy(6);
            // Wakamatsu 20170313
            int sNo = 6;

            for (int i = 0; i < pod.Length; i++)
            {
                if (i == 0)
                {
                    oWSheet.Cell(2, 8).Value = Convert.ToString(pod[i].ItemCode) + "　　" + Convert.ToString(pod[i].Item);
                    oWSheet.Cell(3, 4).Value = Conv.bList[Conv.OfficeCodeIndex(pod[i].OfficeCode)] + "／" + Conv.dNmList[Conv.DepNoIndex(pod[i].Department)];
                    oWSheet.Cell(3, 6).Value = DateTime.Today;
                    oWSheet.Cell(18, 3).Value = opn.Note;
                    itemCode = pod[i].ItemCode;
                }
                // Wakamatsu 20170313
                //oWSheet.Cell(sNo + i, 2).Value = pod[stRec + i].TaskCode;
                //oWSheet.Cell(sNo + i, 3).Value = pod[stRec + i].TaskName;
                //oWSheet.Cell(sNo + i, 4).Value = pod[stRec + i].Customer;
                //oWSheet.Cell(sNo + i, 5).Value = pod[stRec + i].ContTitle;
                //oWSheet.Cell(sNo + i, 6).Value = pod[stRec + i].ReportCheck;
                //oWSheet.Cell(sNo + i, 10).Value = pod[stRec + i].LeaderMName;
                //oWSheet.Cell(sNo + i, 12).Value = pod[stRec + i].Cost;
                //oWSheet.Cell(sNo + i, 14).Value = pod[stRec + i].CloseInf;
                oWSheet.Cell(sNo + i, 2).Value = pod[i].TaskCode;
                oWSheet.Cell(sNo + i, 3).Value = pod[i].TaskName;
                oWSheet.Cell(sNo + i, 4).Value = pod[i].Customer;
                oWSheet.Cell(sNo + i, 5).Value = pod[i].ContTitle;
                oWSheet.Cell(sNo + i, 6).Value = pod[i].ReportCheck;
                oWSheet.Cell(sNo + i, 10).Value = pod[i].LeaderMName;
                oWSheet.Cell(sNo + i, 12).Value = pod[i].Cost;
                oWSheet.Cell(sNo + i, 14).Value = pod[i].CloseInf;
                // Wakamatsu 20170313
                sum += pod[i].Cost;
            }

            int lastRows = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            oWSheet.Cell(lastRows, 14).Value = sum;
            return sum;
        }




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
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            if (sheetRowsCount < lineCount)
            {
                // Wakamatsu 20170313
                //var rowCount = oWSheet.Row(6).InsertRowsBelow(lineCount - sheetRowsCount);
                var rowCount = oWSheet.Row(exLine + 1).InsertRowsBelow(lineCount - sheetRowsCount);
                oWSheet.Rows((exLine + 1) + ":" + (exLine + 1 + lineCount - sheetRowsCount)).Height = oWSheet.Row(exLine + 1).Height;
                // Wakamatsu 20170313
            }
        }

        // Wakamatsu 20170313
        /// <summary>
        /// フォーマット等設定
        /// </summary>
        /// <param name="exLine">基準列</param>
        private void FromatCopy(int exLine)
        {
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                                   // タイトル行と合計行を行数から除く
            using (IXLRange SetRange = oWSheet.Range("A6:O6"))
            {
                for (int i = 0; i < sheetRowsCount - 1; i++)
                {
                    // テンプレートデータ行コピー/ペースト
                    SetRange.CopyTo(oWSheet.Cell(exLine + 1 + i, 1));
                    // 連番再設定
                    oWSheet.Cell(exLine + 1 + i, 1).Value = i + 2;
                    oWSheet.Range(exLine + 1 + i, 1, exLine + 1 + i, 15).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                    oWSheet.Range(exLine + 1 + i, 1, exLine + 1 + i, 15).Style.Border.BottomBorder = XLBorderStyleValues.Hair;
                }
            }
        }
        // Wakamatsu 20170313
    }
}

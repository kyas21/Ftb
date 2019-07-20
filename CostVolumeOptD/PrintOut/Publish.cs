using System;
using System.Windows.Forms;
using ClassLibrary;
using ClosedXML.Excel;
// Wakamatsu
using System.Data;

namespace PrintOut
{
    public class Publish
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        //Excel.Range oRange;

        // Wakamatsu
        /// <summary>
        /// フォーマット設定構造体
        /// </summary>
        public struct FormatSet
        {
            public bool IntFlag;                                    // Int型設定フラグ
            public XLAlignmentHorizontalValues HorizontalSet;       // 水平方向設定
            public XLAlignmentVerticalValues VerticalSet;           // 垂直方向設定
            public string SetFormat;                                // フォーマット設定文字列
        }
        // Wakamatsu

        private DataGridView dgv;
        private string[] iFArray;
        private string fileName;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public Publish()
        {
        }

        public Publish(DataGridView dgv)
        {
            this.dgv = dgv;
        }

        public Publish(string fileName)
        {
            this.fileName = fileName;
        }

        public Publish(string fileName, string[] iFArray, DataGridView dgv)
        {
            this.fileName = fileName;
            this.iFArray = iFArray;
            this.dgv = dgv;
        }
        //---------------------------------------------------------/
        //      Property
        //---------------------------------------------------------/
        public DataGridView Dgv
        {
            get { return this.dgv; }
            set { this.dgv = value; }
        }

        public string[] IFArray
        {
            get { return this.iFArray; }
            set { this.iFArray = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }
        //---------------------------------------------------------/
        //      Method
        //---------------------------------------------------------/
        public void ExcelFile( string sheetName, PublishData pd, DataGridView dgv0, DataGridView dgv1,DataGridView dgv2 )
        {
            editExcelSheet( sheetName, pd, dgv0, dgv1,dgv2 );
        }

        public void ExcelFile(string sheetName, PublishData pd, DataGridView dgv0, DataGridView dgv1)
        {
            editExcelSheet(sheetName, pd, dgv0, dgv1,null);
        }

        public void ExcelFile(string sheetName, PublishData pd, DataGridView dgv)
        {
            editExcelSheet(sheetName, pd, dgv, null,null);
        }

        // Wakamatsu
        // Wakamatsu 20170301
        //public string ExcelFile(string sheetName,DataTable dt,FormatSet[] FormatSet)
        public string ExcelFile(string FileName, string sheetName, DataTable dt, FormatSet[] FormatSet)
        {
            // Wakamatsu 20170301
            //return editExcelSheet(sheetName, dt, FormatSet);
            return editExcelSheet(FileName, sheetName, dt, FormatSet);
            // Wakamatsu 20170301
        }
        // Wakamatsu

        //----------------------------------------------------------------------
        // SubRoutine
        //----------------------------------------------------------------------
        private void editExcelSheet(string sheetName, PublishData pd, DataGridView dgv, DataGridView dgv1,DataGridView dgv2)
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            using (oWBook = new XLWorkbook(fileName))
            {
                oWSheet = oWBook.Worksheet(sheetName);

                switch (sheetName)
                {
                    case "EstimateTop":
                        editEstimateTop(pd);
                        oWSheet = oWBook.Worksheet("EstimateCont");
                        editEstimateCont(dgv);
                        break;
                    case "EstimateCopy":
                        editEstimateCopy(pd);
                        oWSheet = oWBook.Worksheet("EstimateCont");
                        editEstimateCont(dgv);
                        break;
                    case "Planning":
                        editPlanning(pd);
                        break;
                    case "PlanningCont":
                        editPlanningCont(pd, dgv);
                        break;
                    case "OsOrder":
                        editOutsourceOrder(pd);
                        editOutsourceConfirm(pd);
                        break;
                    case "OsConfirm":
                        editOutsourceConfirm(pd);
                        break;
                    case "OsContent":
                        editOutsourceContent(pd, dgv);
                        break;
                    case "OsARegular":
                        editAccountsRegular(pd, dgv, dgv1, dgv2);
                        break;
                    case "OsAContract":
                        editAccountsContract(pd, dgv);
                        break;
                    case "Invoice":
                        editAccountsInvoice(pd, dgv);
                        break;
                    case "VolumeInvoice":
                        editAccountsVolumeInvoice(pd, dgv);
                        break;
                    // Wakamatsu 20170302
                    case "TaskSummary":
                        editTaskSummary(pd, dgv);
                        break;
                    // Wakamatsu 20170302
                    default:
                        break;
                }
                oWBook.SaveAs(tempFile);    // Excel保存

            }

            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す

            System.Diagnostics.Process.Start("Excel.exe", tempFile);                    // 表示用Excel
        }

        // Wakamatsu
        // Wakamatsu 20170301
        //private string editExcelSheet(string sheetName, DataTable dt, FormatSet[] FormatSet)
        private string editExcelSheet(string FileName, string sheetName, DataTable dt, FormatSet[] FormatSet)
        {
            // Wakamatsu 20170301
            //string tempFile = Folder.MyDocuments() + @"\" + sheetName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string tempFile = Folder.MyDocuments() + @"\" + FileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            // Wakamatsu 20170301
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            if (System.IO.File.Exists(fileName) == false)
            {
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                return "× テンプレートファイルが存在しません。\r\n";
            }

            // Wakamatsu 20170322
            //using(oWBook = new XLWorkbook(fileName))
            //{
            try
            {
                // Wakamatsu 20170322
                using (oWBook = new XLWorkbook(fileName))
                {

                    oWSheet = oWBook.Worksheet(sheetName);

                    // Excelファイル出力
                    if (MasterExport(dt, FormatSet) == true)
                    {
                        oWBook.SaveAs(tempFile);    // Excel保存
                        Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                        return "○ Excel出力が正常に終了しました。\r\n" +
                                " " + dt.Rows.Count + "件のデータを出力しました。\r\n";
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                        return "× Excel出力ができませんでした。\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                // Wakamatsu 20170322
                //MessageBox.Show(ex.Message);
                //return "× Excel出力ができませんでした。\r\n";
                return ex.Message + "\r\n× Excel出力ができませんでした。\r\n";
                // Wakamatsu 20170322
            }
            //}
        }
        // Wakamatsu

        private void editEstimateTop(PublishData pd)
        {
            //MessageBox.Show("Excel書込み開始");
            editEstimateCommon(pd);
            oWSheet.Cell(11, 5).Value = pd.Note;
        }


        private void editEstimateCopy(PublishData pd)
        {
            //MessageBox.Show("Excel書込み開始");
            editEstimateCommon(pd);
            oWSheet.Cell(1, 11).Value = DateTime.Today;
            oWSheet.Cell(11, 5).Value = pd.Budgets;
            oWSheet.Cell(12, 5).Value = pd.MinBid;
            oWSheet.Cell(13, 5).Value = pd.Contract;
        }


        private void editEstimateCommon(PublishData pd)
        {
            oWSheet.Cell(1, 2).Value = pd.PartnerName + " 御中";
            oWSheet.Cell(6, 5).Value = pd.TotalAmount;
            oWSheet.Cell(7, 5).Value = pd.Amount;
            oWSheet.Cell(8, 5).Value = pd.Tax;
            oWSheet.Cell(9, 5).Value = pd.TaskName;
            oWSheet.Cell(10, 5).Value = pd.TaskPlace;

            editSender( pd, 1, 10 );
        }


        private void editEstimateCont(DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");

            readyExcelRows(dgv, 2, 8, 1);
            const int SR = 2;       // Excel Sheet Start Row No.
            // Wakamatsu 20170329
            Calculation calc = new Calculation();

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["Amount"].Value) == "")
                {
                    oWSheet.Cell(SR + i, 6).Value = null;     // 金額欄
                }

                // Wakamatsu 20170324
                if (dgv.Rows[i].Cells["Item"].Value != null && dgv.Rows[i].Cells["ItemDetail"].Value != null)
                //else
                {
                    oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                    oWSheet.Cell(SR + i, 2).Value = Convert.ToString(dgv.Rows[i].Cells["ItemDetail"].Value);
                    oWSheet.Cell(SR + i, 3).Value = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value));
                    oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);
                    oWSheet.Cell(SR + i, 5).Value = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Cost"].Value));
                    if (Convert.ToString(dgv.Rows[i].Cells["Unit"].Value) == "")
                    {
                        // Wakamatsu 20170327
                        //oWSheet.Cell(SR + i, 6).Value = DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Amount"].Value));
                        oWSheet.Cell(SR + i, 3).Value = null;     // 数量欄
                        oWSheet.Cell(SR + i, 5).Value = null;     // 単価欄
                    }
                    // Wakamatsu 20170329
                    else if (calc.ExtractCalcWord(Convert.ToString(dgv.Rows[i].Cells["Item"].Value)) == Sign.Expenses)
                    {
                        oWSheet.Cell(SR + i, 3).Value = null;     // 数量欄
                        oWSheet.Cell(SR + i, 5).Value = null;     // 単価欄
                    }
                    // Wakamatsu 20170329
                    // Wakamatsu 20170327
                    oWSheet.Cell(SR + i, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Amount"].Value);
                    oWSheet.Cell(SR + i, 7).Value = Convert.ToString(dgv.Rows[i].Cells["Note"].Value);
                }
            }
        }


        // 予算書エクセル出力
        private void editPlanning(PublishData pd)
        {
            //MessageBox.Show("Excel書込み開始");
            oWSheet.Cell(26, 2).Value = pd.TaxRate;
            oWSheet.Cell(26, 3).Value = pd.OthersCostRate;
            oWSheet.Cell(26, 4).Value = pd.AdminCostRate;

            oWSheet.Cell(2, 1).Value = pd.TaskCode;
            oWSheet.Cell(2, 2).Value = pd.TaskName;
            oWSheet.Cell(2, 5).Value = pd.TaskPlace;
            oWSheet.Cell(2, 7).Value = pd.PartnerName;
            oWSheet.Cell(2, 10).Value = pd.LeaderName;
            oWSheet.Cell(2, 12).Value = pd.SalesMName;
            if (pd.Sales0 > 0) oWSheet.Cell(7, 2).Value = pd.Sales0;
            if (pd.Sales1 > 0) oWSheet.Cell(9, 2).Value = pd.Sales1;
            if (pd.Sales2 > 0) oWSheet.Cell(11, 2).Value = pd.Sales2;
            oWSheet.Cell(8, 5).Value = pd.ContractDate;
            oWSheet.Cell(10, 5).Value = pd.StartDate;
            oWSheet.Cell(12, 5).Value = pd.EndDate;

            if (pd.Direct0 > 0) oWSheet.Cell(15, 3).Value = pd.Direct0;
            if (pd.Direct1 > 0) oWSheet.Cell(15, 4).Value = pd.Direct1;
            if (pd.Direct2 > 0) oWSheet.Cell(15, 5).Value = pd.Direct2;
            if (pd.OutS0 > 0) oWSheet.Cell(16, 3).Value = pd.OutS0;
            if (pd.OutS1 > 0) oWSheet.Cell(16, 4).Value = pd.OutS1;
            if (pd.OutS2 > 0) oWSheet.Cell(16, 5).Value = pd.OutS2;
            if (pd.Matel0 > 0) oWSheet.Cell(17, 3).Value = pd.Matel0;
            if (pd.Matel1 > 0) oWSheet.Cell(17, 4).Value = pd.Matel1;
            if (pd.Matel2 > 0) oWSheet.Cell(17, 5).Value = pd.Matel2;
        }


        private void editPlanningCont(PublishData pd, DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");

            string[] titleArray = new string[] { " 原 予 算 ", " 変 更 １ 回 ", " 変 更 ２ 回 " };

            //var HeaderText = @"&U&24" + titleArray[pd.Version] + "内 訳 書";
            var HeaderText = @"&U&24" + titleArray[pd.PublishIndex] + "内 訳 書 ";
            oWSheet.PageSetup.Header.Center.AddText(HeaderText);

            readyExcelRows(dgv, 2, 12, 4);
            const int SR = 4;       // Excel Sheet Start Row No.

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                oWSheet.Cell(SR + i, 2).Value = Convert.ToString(dgv.Rows[i].Cells["ItemDetail"].Value);
                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    oWSheet.Cell(SR + i, 3).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Quantity"].Value);
                oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);

                for (int j = 0; j < 3; j++)
                {
                    if (Convert.ToString(dgv.Rows[i].Cells["Amount" + j.ToString()].Value) == "")
                    {
                        //oWSheet.Cell(SR + i, j * 2 + 5).Value = null;     // 単価
                        //oWSheet.Cell(SR + i, j * 2 + 6).Value = null;     // 金額
                    }
                    else
                    {
                        if (Convert.ToString(dgv.Rows[i].Cells["Cost" + j.ToString()].Value) == "")
                        {
                            oWSheet.Cell(SR + i, j * 2 + 6).Value =
                                DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Amount" + j.ToString()].Value));
                        }
                        else
                        {
                            oWSheet.Cell(SR + i, j * 2 + 5).Value =
                                DHandling.ToRegDecimal(Convert.ToString(dgv.Rows[i].Cells["Cost" + j.ToString()].Value));

                        }
                    }
                }
            }
        }



        // 注文書作成
        private void editOutsourceOrder(PublishData pd)
        {
            oWSheet.Cell(3, 2).Value = pd.OrderPartner + " 御中";

            oWSheet.Cell(13, 9).Value = pd.InspectDate;
            oWSheet.Cell(14, 9).Value = pd.ReceiptDate;

            editSender( pd, 3, 9 );
        }


        private void editOutsourceConfirm(PublishData pd)
        {
            //MessageBox.Show("Excel書込み開始");
            string[] payArray = new string[] { "出来高払", "完成払" };

            oWSheet.Cell(8, 3).Value = pd.OrderNo;
            oWSheet.Cell(9, 3).Value = pd.TaskCode;
            oWSheet.Cell(10, 3).Value = pd.TaskName;
            oWSheet.Cell(11, 3).Value = pd.Amount + pd.Tax;
            oWSheet.Cell(12, 3).Value = pd.Tax;
            oWSheet.Cell(14, 3).Value = pd.OrderStartDate;
            oWSheet.Cell(14, 5).Value = pd.OrderEndDate;
            oWSheet.Cell(15, 3).Value = payArray[pd.PayRoule];
            oWSheet.Cell(19, 3).Value = pd.Place;
            oWSheet.Cell(20, 3).Value = pd.Note;

            oWSheet.Cell(9, 7).Value = pd.StartDate;
            oWSheet.Cell(9, 9).Value = pd.EndDate;
        }


        private void editOutsourceContent(PublishData pd, DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");
            string[] payArray = new string[] { "出来高払", "完成払" };
            oWSheet.Cell(1, 2).Value = pd.PartnerName;
            oWSheet.Cell(1, 7).Value = pd.TaskCode;
            oWSheet.Cell(2, 2).Value = pd.TaskName;
            oWSheet.Cell(3, 2).Value = pd.OrderStartDate;
            oWSheet.Cell(3, 4).Value = pd.OrderEndDate;
            oWSheet.Cell(3, 7).Value = pd.Amount;
            oWSheet.Cell(4, 2).Value = pd.Place;
            oWSheet.Cell(4, 7).Value = payArray[pd.PayRoule];
            oWSheet.Cell(5, 2).Value = pd.Note;
            oWSheet.Cell(5, 7).Value = pd.OrderNo;

            readyExcelRows(dgv, 1, 8, 7, 9);
            int maxRowsCount = dgvRowsCount(dgv, 1, 8);
            const int SR = 8;       // Excel Sheet Start Row No.

            for (int i = 0; i < maxRowsCount; i++)
            {
                oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                oWSheet.Cell(SR + i, 2).Value = Convert.ToString(dgv.Rows[i].Cells["ItemDetail"].Value);
                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    oWSheet.Cell(SR + i, 3).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Quantity"].Value);
                oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);
                if (Convert.ToString(dgv.Rows[i].Cells["Cost"].Value) != "")
                    oWSheet.Cell(SR + i, 5).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Cost"].Value);
                oWSheet.Cell(SR + i, 7).Value = Convert.ToString(dgv.Rows[i].Cells["Note"].Value);
            }
        }


        /////// 精算書
        // 常傭精算書
        private void editAccountsRegular(PublishData pd, DataGridView dgvT, DataGridView dgvL, DataGridView dgvR)
        {
            //MessageBox.Show("Excel書込み開始");
            oWSheet.Cell(1, 1).Value = pd.OrderPartner + "御中";

            oWSheet.Cell(2, 1).Value = pd.RecordedDate;
            oWSheet.Cell(2, 5).Value = pd.PartnerName;
            oWSheet.Cell(2, 14).Value = pd.LeaderName;

            oWSheet.Cell(3, 2).Value = pd.TaskCode;
            oWSheet.Cell(3, 5).Value = pd.TaskName;
            oWSheet.Cell(3, 14).Value = pd.SalesMName;

            const int SR = 5;       // Excel Sheet Start Row No.

            for( int i = 0; i < dgvL.Rows.Count; i++ )
            {
                oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgvL.Rows[i].Cells["ItemL"].Value );
                for( int j = 0; j < 6; j++ )
                {
                    if( !string.IsNullOrEmpty(Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) ) )
                        oWSheet.Cell( SR + i, 3 + j ).Value = Convert.ToDecimal( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value );
                }
            }

            int cofs = 8;
            for (int i = 0; i < dgvR.Rows.Count; i++)
            {
                oWSheet.Cell(SR + i, 2 + cofs).Value = Convert.ToString(dgvR.Rows[i].Cells["ItemR"].Value);
                for (int j = 0; j < 6; j++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value) ) )
                        oWSheet.Cell(SR + i, 4 + j + cofs).Value = Convert.ToDecimal(dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value);
                }
            }

            const int SR1 = 22;
            for (int i = 0; i < dgvT.Rows.Count; i++)
            {
                oWSheet.Cell(SR1 + i, 3).Value = Convert.ToString(dgvT.Rows[i].Cells["ItemDetail"].Value);
                oWSheet.Cell(SR1 + i, 10).Value = Convert.ToDecimal(dgvT.Rows[i].Cells["Cost"].Value);
                oWSheet.Cell(SR1 + i, 12).Value = Convert.ToString(dgvT.Rows[i].Cells["Note"].Value);
            }
        }


        // 請負精算書
        private void editAccountsContract(PublishData pd, DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");
            oWSheet.Cell(1, 1).Value = pd.OrderPartner + "御中";
            oWSheet.Cell(1, 12).Value = pd.LeaderName;
            oWSheet.Cell(1, 14).Value = pd.SalesMName;

            oWSheet.Cell(2, 2).Value = pd.TaskCode;
            oWSheet.Cell(2, 4).Value = pd.TaskName;
            oWSheet.Cell(2, 12).Value = pd.PartnerName;

            readyExcelRows(dgv, 2, 15, 4, 5);
            int maxRowsCount = dgvRowsCount(dgv, 2, 15);
            const int SR = 4;       // Excel Sheet Start Row No.

            for (int i = 0; i < maxRowsCount; i++)
            {
                oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["PQuantity"].Value) != "")
                    oWSheet.Cell(SR + i, 3).Value = Convert.ToDecimal(dgv.Rows[i].Cells["PQuantity"].Value);

                oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Cost"].Value) != "")
                    oWSheet.Cell(SR + i, 5).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Cost"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["LQuantity"].Value) != "")
                    oWSheet.Cell(SR + i, 7).Value = Convert.ToDecimal(dgv.Rows[i].Cells["LQuantity"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    oWSheet.Cell(SR + i, 9).Value = Convert.ToDecimal(dgv.Rows[i].Cells["LQuantity"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["SQuantity"].Value) != "")
                    oWSheet.Cell(SR + i, 11).Value = Convert.ToDecimal(dgv.Rows[i].Cells["SQuantity"].Value);
            }
        }


        // 請求書
        private void editAccountsInvoice(PublishData pd, DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");
            editSender( pd, 3, 6 );

            oWSheet.Cell(2, 3).Value = pd.PartnerName + " 御中";
            oWSheet.Cell(4, 3).Value = pd.TaskName;

            readyExcelRows(dgv, 3, 8, 12, 12);
            int maxRowsCount = dgvRowsCount(dgv, 3, 8);
            const int SR = 11;      // Excel Sheet Start Row No.
            decimal totalAmount = 0;
            string wMonth = "";
            string wDay = "";

            for (int i = 0; i < maxRowsCount; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["Month"].Value) != wMonth)
                {
                    oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Month"].Value);
                    wMonth = Convert.ToString(dgv.Rows[i].Cells["Month"].Value);
                }

                if (Convert.ToString(dgv.Rows[i].Cells["Day"].Value) != wDay)
                {
                    oWSheet.Cell(SR + i, 2).Value = Convert.ToString(dgv.Rows[i].Cells["Day"].Value);
                    wDay = Convert.ToString(dgv.Rows[i].Cells["Day"].Value);
                }

                oWSheet.Cell(SR + i, 3).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["ItemDetail"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    oWSheet.Cell(SR + i, 5).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Quantity"].Value);

                oWSheet.Cell(SR + i, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Cost"].Value) != "")
                    oWSheet.Cell(SR + i, 7).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Cost"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Amount"].Value) != "")
                {
                    oWSheet.Cell(SR + i, 8).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Amount"].Value);
                    totalAmount += Convert.ToDecimal(dgv.Rows[i].Cells["Amount"].Value);
                }
            }

            oWSheet.Cell(8, 3).Value = totalAmount;
        }


        // 出来高請求書
        private void editAccountsVolumeInvoice(PublishData pd, DataGridView dgv)
        {
            //MessageBox.Show("Excel書込み開始");
            editSender( pd, 1, 10 );

            oWSheet.Cell(2, 1).Value = pd.PartnerName + " 御中";
            oWSheet.Cell(4, 3).Value = pd.TaskName;

            readyExcelRows(dgv, 3, 13, 7, 9);
            int maxRowsCount = dgvRowsCount(dgv, 2, 15);
            const int SR = 8;      // Excel Sheet Start Row No.
            string wMonth = "";
            string wDay = "";

            for (int i = 0; i < maxRowsCount; i++)
            {
                if (Convert.ToString(dgv.Rows[i].Cells["Month"].Value) != wMonth)
                {
                    oWSheet.Cell(SR + i, 1).Value = Convert.ToString(dgv.Rows[i].Cells["Month"].Value);
                    wMonth = Convert.ToString(dgv.Rows[i].Cells["Month"].Value);
                }

                if (Convert.ToString(dgv.Rows[i].Cells["Day"].Value) != wDay)
                {
                    oWSheet.Cell(SR + i, 2).Value = Convert.ToString(dgv.Rows[i].Cells["Day"].Value);
                    wDay = Convert.ToString(dgv.Rows[i].Cells["Day"].Value);
                }

                oWSheet.Cell(SR + i, 3).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                oWSheet.Cell(SR + i, 4).Value = Convert.ToString(dgv.Rows[i].Cells["ItemDetail"].Value);
                // 契約
                if (Convert.ToString(dgv.Rows[i].Cells["CQuantity"].Value) != "")
                    oWSheet.Cell(SR + i, 5).Value = Convert.ToDecimal(dgv.Rows[i].Cells["CQuantity"].Value);

                oWSheet.Cell(SR + i, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Unit"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["CCost"].Value) != "")
                    oWSheet.Cell(SR + i, 7).Value = Convert.ToDecimal(dgv.Rows[i].Cells["CCost"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["CAmount"].Value) != "")
                    oWSheet.Cell(SR + i, 8).Value = Convert.ToDecimal(dgv.Rows[i].Cells["CAmount"].Value);
                // 前回までの累積
                if (Convert.ToString(dgv.Rows[i].Cells["SQuantity"].Value) != "")
                    oWSheet.Cell(SR + i, 9).Value = Convert.ToDecimal(dgv.Rows[i].Cells["SQuantity"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["SAmount"].Value) != "")
                    oWSheet.Cell(SR + i, 10).Value = Convert.ToDecimal(dgv.Rows[i].Cells["SAmount"].Value);
                // 今回
                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    oWSheet.Cell(SR + i, 11).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Quantity"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["Amount"].Value) != "")
                    oWSheet.Cell(SR + i, 12).Value = Convert.ToDecimal(dgv.Rows[i].Cells["Amount"].Value);
                // 残
                if (Convert.ToString(dgv.Rows[i].Cells["RAmount"].Value) == "")
                {
                    oWSheet.Cell(SR + i, 13).Value = "";
                }
                else
                {
                    oWSheet.Cell(SR + i, 13).Value = Convert.ToDecimal(dgv.Rows[i].Cells["RAmount"].Value);
                }
            }
        }


        // Wakamatsu 20170302
        /// <summary>
        /// 業務元帳出力
        /// </summary>
        /// <param name="pd">出力データ</param>
        /// <param name="dgv">出力対象データグリッドビュー</param>
        private void editTaskSummary(PublishData pd, DataGridView dgv)
        {
            oWSheet.Cell(3, 2).Value = pd.OfficeName + " " + pd.Department;                 // 部門 部署
            oWSheet.Cell(3, 5).Value = pd.OrderStartDate.ToString("yyyy/MM/dd") +
                                        "～" + pd.OrderEndDate.ToString("yyyy/MM/dd");       // 表示期間
            oWSheet.Cell(5, 2).Value = pd.TaskCode;                                         // 業務番号
            oWSheet.Cell(5, 5).Value = pd.TaskName;                                         // 業務名
            oWSheet.Cell(7, 5).Value = pd.PartnerName;                                      // 取引先名
            oWSheet.Cell(7, 7).Value = pd.Note;                                             // 工期

            int WriteOffset = 12;               // 書出し位置オフセット
            bool EmptyFlag = true;              // 空データグリッドビューフラグ

            for (int i = 0; i < 8; i++)
                // Wakamatu 20170307
                //if( Convert.ToString( dgv.Rows[i].Cells[i].Value ) != "" )
                if (Convert.ToString(dgv.Rows[0].Cells[i].Value) != "")
                    EmptyFlag = false;

            if (EmptyFlag == false)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    using (IXLRange SetRange = oWSheet.Range("A11:I11"))
                        // テンプレートデータ行コピー/ペースト
                        SetRange.CopyTo(oWSheet.Cell(i + WriteOffset, 1));

                    if (Convert.ToString(dgv.Rows[i].Cells["ReportDate"].Value) != "")      // 月日
                    {
                        oWSheet.Cell(i + WriteOffset, 1).Value = Convert.ToString(dgv.Rows[i].Cells["ReportDate"].Value);
                        oWSheet.Range(i + WriteOffset, 1, i + WriteOffset, 9).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                    }

                    if (Convert.ToString(dgv.Rows[i].Cells["SlipNo"].Value) != "")          // 伝票No.
                    {
                        oWSheet.Cell(i + WriteOffset, 2).Value = Convert.ToString(dgv.Rows[i].Cells["SlipNo"].Value);
                        oWSheet.Cell(i + WriteOffset, 2).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        oWSheet.Cell(i + WriteOffset, 9).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                    }

                    if (Convert.ToString(dgv.Rows[i].Cells["ItemCode"].Value) != "")        // コード
                        oWSheet.Cell(i + WriteOffset, 3).Value = Convert.ToString(dgv.Rows[i].Cells["ItemCode"].Value);
                    else
                    {
                        oWSheet.Cell(i + WriteOffset, 2).Style.Border.RightBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 3).Style.Border.LeftBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 3).Style.Border.RightBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 4).Style.Border.LeftBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 5).Style.Border.RightBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 6).Style.Border.LeftBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 7).Style.Border.RightBorder = XLBorderStyleValues.None;
                        oWSheet.Cell(i + WriteOffset, 8).Style.Border.LeftBorder = XLBorderStyleValues.None;
                    }

                    oWSheet.Range(i + WriteOffset, 3, i + WriteOffset, 8).Style.Border.TopBorder = XLBorderStyleValues.Hair;

                    if (Convert.ToString(dgv.Rows[i].Cells["Item"].Value) != "")            // 名称
                        oWSheet.Cell(i + WriteOffset, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);

                    if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")        // 数量
                        oWSheet.Cell(i + WriteOffset, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value);

                    if (Convert.ToString(dgv.Rows[i].Cells["UnitPrice"].Value) != "")       // 単価
                        oWSheet.Cell(i + WriteOffset, 7).Value = Convert.ToString(dgv.Rows[i].Cells["UnitPrice"].Value);

                    if (Convert.ToString(dgv.Rows[i].Cells["Cost"].Value) != "")            // 金額
                        oWSheet.Cell(i + WriteOffset, 8).Value = Convert.ToString(dgv.Rows[i].Cells["Cost"].Value);

                    if (Convert.ToString(dgv.Rows[i].Cells["Balance"].Value) != "")         // 累計
                        oWSheet.Cell(i + WriteOffset, 9).Value = Convert.ToString(dgv.Rows[i].Cells["Balance"].Value);

                    // 行の高さ設定
                    oWSheet.Row(i + WriteOffset).Height = 21.00;
                    if (i == dgv.Rows.Count - 1)
                        // 最終データのみ罫線の設定
                        oWSheet.Range(i + WriteOffset, 1, i + WriteOffset, 9).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
            }

            // 先頭データのみ罫線の設定
            oWSheet.Range(WriteOffset, 1, WriteOffset, 9).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            // テンプレート行削除
            oWSheet.Row(11).Delete();
            oWSheet.PageSetup.FitToPages(1, 0);

        }
        // Wakamatsu 20170302


        /// <summary>
        /// DataGridViewの最後にデータが存在する行数と現在のエクセルの行数を比較し、エクセルの行数が少ないようなら
        /// エクセルの行数を増加させる。
        /// 比較の差異は列ヘッダーの行数、合計行の行数は除外する。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="stCol"></param>
        /// <param name="endCol"></param>
        /// <param name="headerLine"></param>
        private void readyExcelRows(DataGridView dgv, int stCol, int endCol, int exLine)
        {
            int lastRows = dgvRowsCount(dgv, stCol, endCol);    // 表示対象の最終行 
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            if (lastRows > sheetRowsCount)
            {
                var rowCount = oWSheet.Row(6).InsertRowsBelow(lastRows - sheetRowsCount);
            }
        }

        private void readyExcelRows(DataGridView dgv, int stCol, int endCol, int exLine, int rowPos)
        {
            int lastRows = dgvRowsCount(dgv, stCol, endCol);    // 表示対象の最終行 
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            if (lastRows > sheetRowsCount)
            {
                var rowCount = oWSheet.Row(rowPos).InsertRowsBelow(lastRows - sheetRowsCount + 1);
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
        private int dgvRowsCount(DataGridView dgv, int stCol, int endCol)
        {
            int rowsCount = 0;
            int colsCount = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = stCol; j < endCol; j++)
                {
                    if (Convert.ToString(dgv.Rows[i].Cells[j].Value) != "") colsCount++;
                }
                if (colsCount > 0) rowsCount = i;
                colsCount = 0;
            }

            return rowsCount + 1;
        }

        // Wakamatsu
        /// <summary>
        /// Excelファイル出力
        /// </summary>
        /// <param name="dt">出力対象データテーブル</param>
        /// <param name="FormatSet">フォーマット設定構造体</param>
        /// <returns></returns>
        private bool MasterExport(DataTable dt, FormatSet[] FormatSet)
        {
            //MessageBox.Show("Excel書込み開始");

            Type WriteData = null;                                  // レコードのデータタイプ
            int ConvertData = 0;                                    // 変換結果格納用

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (FormatSet[j].IntFlag == true)
                        {
                            // Int型で出力
                            if (int.TryParse(Convert.ToString(dt.Rows[i][j]).Trim(), out ConvertData))
                                oWSheet.Cell(i + 2, j + 1).Value = ConvertData;
                            else
                                oWSheet.Cell(i + 2, j + 1).Value = Convert.ToString(dt.Rows[i][j]).Trim();
                        }
                        else
                        {
                            WriteData = dt.Rows[i][j].GetType();

                            if (WriteData.Equals(typeof(DateTime)) == true)
                            {
                                // DateTime型の場合
                                if (Convert.ToDateTime(dt.Rows[i][j]) != DateTime.MinValue)
                                    oWSheet.Cell(i + 2, j + 1).Value = Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy/MM/dd");
                            }
                            else if (WriteData.Equals(typeof(string)) == true)
                                // String型の場合
                                oWSheet.Cell(i + 2, j + 1).Value = "'" + Convert.ToString(dt.Rows[i][j]).Trim();
                            else
                                // その他の場合
                                oWSheet.Cell(i + 2, j + 1).Value = Convert.ToString(dt.Rows[i][j]).Trim();
                        }
                    }
                }
                // 書式設定
                for (int i = 0; i < FormatSet.Length; i++)
                {
                    using (IXLRange SetRange = oWSheet.Range(2, i + 1, dt.Rows.Count + 1, i + 1))
                    {
                        SetRange.Style.Alignment.Horizontal = FormatSet[i].HorizontalSet;
                        SetRange.Style.Alignment.Vertical = FormatSet[i].VerticalSet;
                        SetRange.Style.Font.FontName = oWSheet.Cell("A1").Style.Font.FontName;
                        SetRange.Style.Font.FontSize = oWSheet.Cell("A1").Style.Font.FontSize;
                        SetRange.Style.NumberFormat.SetFormat(FormatSet[i].SetFormat);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        // Wakamatsu


        private void editSender(PublishData pd, int stRow, int stCol )
        {
            OfficeData od = new OfficeData();
            od = od.SelectOfficeData( pd.OfficeCode );
            if( od == null ) return;
            oWSheet.Cell( stRow, stCol ).Value = od.Address;
            //oWSheet.Cell( stRow+1, stCol ).Value = "フタバコンサルタント株式会社 " + pd.OfficeName + "支店";
            oWSheet.Cell( stRow + 1, stCol ).Value = "フタバコンサルタント株式会社 ";
            if(pd.PublishOffice == 0) oWSheet.Cell( stRow+1, stCol ).Value += pd.OfficeName + "支店";
            oWSheet.Cell( stRow+2, stCol ).Value = od.Title + " " + od.MemberName;
            oWSheet.Cell( stRow+3, stCol ).Value = "TEL " + od.TelNo + "（代）    FAX " + od.FaxNo;
        }
    }
}

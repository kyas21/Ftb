using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using ClosedXML.Excel;


namespace PrintOut
{
    public class PublishVolume
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        //Excel.Range oRange;

        private DataGridView dgv;
        private string[] iFArray;
        private string fileName;
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishVolume()
        {
        }

        public PublishVolume( DataGridView dgv )
        {
            this.dgv = dgv;
        }

        public PublishVolume( string fileName )
        {
            this.fileName = fileName;
        }

        public PublishVolume( string fileName, string[] iFArray, DataGridView dgv )
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
        public void ExcelFile( string sheetName, PublishData pd, DataGridView dgv0, DataGridView dgv1 )
        {
            editExcelSheet( sheetName, pd, dgv0, dgv1 );
        }

        public void ExcelFile( string sheetName, PublishData pd, DataGridView dgv )
        {
            editExcelSheet( sheetName, pd, dgv, null );
        }


        //----------------------------------------------------------------------
        // SubRoutine
        //----------------------------------------------------------------------
        private void editExcelSheet( string sheetName, PublishData pd, DataGridView dgv, DataGridView dgv1 )
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)

            // Wakamatsu 20170307
            try
            {
                using ( oWBook = new XLWorkbook( fileName ) )
                {
                    oWSheet = oWBook.Worksheet( sheetName );

                    switch ( sheetName )
                    {
                        case "EstimateTop":
                            editEstimateTop( pd );
                            oWSheet = oWBook.Worksheet( "EstimateCont" );
                            editEstimateCont( dgv );
                            break;
                        case "EstimateCopy":
                            editEstimateCopy( pd );
                            oWSheet = oWBook.Worksheet( "EstimateCont" );
                            editEstimateCont( dgv );
                            break;
                        case "Planning":
                            editPlanning( pd );
                            break;
                        case "PlanningCont":
                            editPlanningCont( pd, dgv );
                            break;
                        case "OsOrder":
                            editOutsourceOrder( pd );
                            editOutsourceConfirm( pd );
                            break;
                        case "OsConfirm":
                            editOutsourceConfirm( pd );
                            break;
                        case "OsContent":
                            editOutsourceContent( pd, dgv );
                            break;
                        case "OsARegular":
                            editAccountsRegular( pd, dgv, dgv1 );
                            break;
                        case "OsAContract":
                            editAccountsContract( pd, dgv );
                            break;
                        case "Invoice":
                            editAccountsInvoice( pd, dgv );
                            break;
                        case "VolumeInvoice":
                            editAccountsVolumeInvoice( pd, dgv );
                            break;
                        case "Volume":
                            //MessageBox.Show("Excel書込み開始");
                            editVolume( pd, dgv );
                            break;
                        case "CostDetail":
                            editCostDetail( pd, dgv );
                            break;
                        case "CostSummary":
                            editCostSummary( pd, dgv );
                            break;
                        // Wakamatsu 20170301
                        case "LedgerAggregate":
                            editVolume( dgv );
                            break;
                        // Wakamatsu 20170301
                        default:
                            break;
                    }
                    oWBook.SaveAs( tempFile );    // Excel保存
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message );
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                return;
            }
            // Wakamatsu 20170307

            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
            System.Diagnostics.Process.Start( "Excel.exe", tempFile );                    // 表示用Excel
        }



        private void editEstimateTop( PublishData pd )
        {
            MessageBox.Show( "Excel書込み開始" );
            editEstimateCommon( pd );
            oWSheet.Cell( 11, 5 ).Value = pd.Note;
        }


        private void editEstimateCopy( PublishData pd )
        {
            MessageBox.Show( "Excel書込み開始" );
            editEstimateCommon( pd );
            oWSheet.Cell( 1, 11 ).Value = DateTime.Today;
            oWSheet.Cell( 11, 5 ).Value = pd.Budgets;
            oWSheet.Cell( 12, 5 ).Value = pd.MinBid;
            oWSheet.Cell( 13, 5 ).Value = pd.Contract;
        }


        private void editEstimateCommon( PublishData pd )
        {
            oWSheet.Cell( 1, 2 ).Value = pd.PartnerName + " 御中";
            oWSheet.Cell( 6, 5 ).Value = pd.TotalAmount;
            oWSheet.Cell( 7, 5 ).Value = pd.Amount;
            oWSheet.Cell( 8, 5 ).Value = pd.Tax;
            oWSheet.Cell( 9, 5 ).Value = pd.TaskName;
            oWSheet.Cell( 10, 5 ).Value = pd.TaskPlace;
        }


        private void editEstimateCont( DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );

            readyExcelRows( dgv, 2, 8, 1 );
            const int SR = 2;       // Excel Sheet Start Row No.

            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if ( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) == "" )
                {
                    oWSheet.Cell( SR + i, 6 ).Value = null;     // 金額欄
                }
                else
                {
                    oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                    oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                    oWSheet.Cell( SR + i, 3 ).Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) );
                    oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );
                    oWSheet.Cell( SR + i, 5 ).Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) );
                    if ( Convert.ToString( dgv.Rows[i].Cells["Unit"].Value ) == "" )
                    {
                        oWSheet.Cell( SR + i, 6 ).Value = DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) );
                        oWSheet.Cell( SR + i, 3 ).Value = null;     // 数量欄
                        oWSheet.Cell( SR + i, 5 ).Value = null;     // 単価欄
                    }
                    oWSheet.Cell( SR + i, 7 ).Value = Convert.ToString( dgv.Rows[i].Cells["Note"].Value );
                }
            }
        }


        // 予算書エクセル出力
        private void editPlanning( PublishData pd )
        {
            MessageBox.Show( "Excel書込み開始" );
            oWSheet.Cell( 26, 2 ).Value = pd.TaxRate;
            oWSheet.Cell( 26, 3 ).Value = pd.OthersCostRate;
            oWSheet.Cell( 26, 4 ).Value = pd.AdminCostRate;

            oWSheet.Cell( 2, 1 ).Value = pd.TaskCode;
            oWSheet.Cell( 2, 2 ).Value = pd.TaskName;
            oWSheet.Cell( 2, 5 ).Value = pd.TaskPlace;
            oWSheet.Cell( 2, 7 ).Value = pd.PartnerName;
            oWSheet.Cell( 2, 10 ).Value = pd.LeaderName;
            oWSheet.Cell( 2, 12 ).Value = pd.SalesMName;
            if ( pd.Sales0 > 0 ) oWSheet.Cell( 7, 2 ).Value = pd.Sales0;
            if ( pd.Sales1 > 0 ) oWSheet.Cell( 9, 2 ).Value = pd.Sales1;
            if ( pd.Sales2 > 0 ) oWSheet.Cell( 11, 2 ).Value = pd.Sales2;
            oWSheet.Cell( 8, 5 ).Value = pd.ContractDate;
            oWSheet.Cell( 10, 5 ).Value = pd.StartDate;
            oWSheet.Cell( 12, 5 ).Value = pd.EndDate;

            if ( pd.Direct0 > 0 ) oWSheet.Cell( 15, 3 ).Value = pd.Direct0;
            if ( pd.Direct1 > 0 ) oWSheet.Cell( 15, 4 ).Value = pd.Direct1;
            if ( pd.Direct2 > 0 ) oWSheet.Cell( 15, 5 ).Value = pd.Direct2;
            if ( pd.OutS0 > 0 ) oWSheet.Cell( 16, 3 ).Value = pd.OutS0;
            if ( pd.OutS1 > 0 ) oWSheet.Cell( 16, 4 ).Value = pd.OutS1;
            if ( pd.OutS2 > 0 ) oWSheet.Cell( 16, 5 ).Value = pd.OutS2;
            if ( pd.Matel0 > 0 ) oWSheet.Cell( 17, 3 ).Value = pd.Matel0;
            if ( pd.Matel1 > 0 ) oWSheet.Cell( 17, 4 ).Value = pd.Matel1;
            if ( pd.Matel2 > 0 ) oWSheet.Cell( 17, 5 ).Value = pd.Matel2;
        }


        private void editPlanningCont( PublishData pd, DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );

            string[] titleArray = new string[] { "原 予 算 ", "変 更 １ 回 ", "変 更 ２ 回 " };

            var HeaderText = @"&U&24" + titleArray[pd.Version] + "内 訳 書";
            oWSheet.PageSetup.Header.Center.AddText( HeaderText );

            readyExcelRows( dgv, 2, 12, 4 );
            const int SR = 4;       // Excel Sheet Start Row No.

            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 3 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Quantity"].Value );
                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );

                for ( int j = 0; j < 3; j++ )
                {
                    if ( Convert.ToString( dgv.Rows[i].Cells["Amount" + j.ToString()].Value ) == "" )
                    {
                        //oWSheet.Cell(SR + i, j * 2 + 5).Value = null;     // 単価
                        //oWSheet.Cell(SR + i, j * 2 + 6).Value = null;     // 金額
                    }
                    else
                    {
                        if ( Convert.ToString( dgv.Rows[i].Cells["Cost" + j.ToString()].Value ) == "" )
                        {
                            oWSheet.Cell( SR + i, j * 2 + 6 ).Value =
                                DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Amount" + j.ToString()].Value ) );
                        }
                        else
                        {
                            oWSheet.Cell( SR + i, j * 2 + 5 ).Value =
                                DHandling.ToRegDecimal( Convert.ToString( dgv.Rows[i].Cells["Cost" + j.ToString()].Value ) );

                        }
                    }
                }
            }
        }



        // 注文書作成
        private void editOutsourceOrder( PublishData pd )
        {
            oWSheet.Cell( 3, 2 ).Value = pd.OrderPartner + " 御中";

            oWSheet.Cell( 13, 9 ).Value = pd.InspectDate;
            oWSheet.Cell( 14, 9 ).Value = pd.ReceiptDate;
        }


        private void editOutsourceConfirm( PublishData pd )
        {
            MessageBox.Show( "Excel書込み開始" );
            string[] payArray = new string[] { "出来高払", "完成払" };

            oWSheet.Cell( 8, 3 ).Value = pd.OrderNo;
            oWSheet.Cell( 9, 3 ).Value = pd.TaskCode;
            oWSheet.Cell( 10, 3 ).Value = pd.TaskName;
            oWSheet.Cell( 11, 3 ).Value = pd.Amount + pd.Tax;
            oWSheet.Cell( 12, 3 ).Value = pd.Tax;
            oWSheet.Cell( 14, 3 ).Value = pd.OrderStartDate;
            oWSheet.Cell( 14, 5 ).Value = pd.OrderEndDate;
            oWSheet.Cell( 15, 3 ).Value = payArray[pd.PayRoule];
            oWSheet.Cell( 19, 3 ).Value = pd.Place;
            oWSheet.Cell( 20, 3 ).Value = pd.Note;

            oWSheet.Cell( 9, 7 ).Value = pd.StartDate;
            oWSheet.Cell( 9, 9 ).Value = pd.EndDate;
        }


        private void editOutsourceContent( PublishData pd, DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );
            string[] payArray = new string[] { "出来高払", "完成払" };
            oWSheet.Cell( 1, 2 ).Value = pd.PartnerName;
            oWSheet.Cell( 1, 7 ).Value = pd.TaskCode;
            oWSheet.Cell( 2, 2 ).Value = pd.TaskName;
            oWSheet.Cell( 3, 2 ).Value = pd.OrderStartDate;
            oWSheet.Cell( 3, 4 ).Value = pd.OrderEndDate;
            oWSheet.Cell( 3, 7 ).Value = pd.Amount;
            oWSheet.Cell( 4, 2 ).Value = pd.Place;
            oWSheet.Cell( 4, 7 ).Value = payArray[pd.PayRoule];
            oWSheet.Cell( 5, 2 ).Value = pd.Note;
            oWSheet.Cell( 5, 7 ).Value = pd.OrderNo;

            readyExcelRows( dgv, 1, 8, 7, 9 );
            int maxRowsCount = dgvRowsCount( dgv, 1, 8 );
            const int SR = 8;       // Excel Sheet Start Row No.

            for ( int i = 0; i < maxRowsCount; i++ )
            {
                oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 3 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Quantity"].Value );
                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );
                if ( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) != "" )
                    oWSheet.Cell( SR + i, 5 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Cost"].Value );
                oWSheet.Cell( SR + i, 7 ).Value = Convert.ToString( dgv.Rows[i].Cells["Note"].Value );
            }
        }


        /////// 精算書
        // 常傭精算書
        private void editAccountsRegular( PublishData pd, DataGridView dgv, DataGridView dgv1 )
        {
            MessageBox.Show( "Excel書込み開始" );
            oWSheet.Cell( 1, 1 ).Value = pd.OrderPartner + "御中";

            oWSheet.Cell( 2, 1 ).Value = pd.RecordedDate;
            oWSheet.Cell( 2, 5 ).Value = pd.PartnerName;
            oWSheet.Cell( 2, 14 ).Value = pd.LeaderName;

            oWSheet.Cell( 3, 2 ).Value = pd.TaskCode;
            oWSheet.Cell( 3, 5 ).Value = pd.TaskName;
            oWSheet.Cell( 3, 14 ).Value = pd.SalesMName;

            int rofs = 0;
            int cofs = 0;
            const int SR = 5;       // Excel Sheet Start Row No.

            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                if ( i > 15 )
                {
                    rofs = -16;
                    cofs = 8;
                }

                oWSheet.Cell( SR + i + rofs, 2 + cofs ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );

                for ( int j = 0; j < 6; j++ )
                {
                    if ( Convert.ToString( dgv.Rows[i].Cells["Quantity" + j.ToString()].Value ) != "" )
                        oWSheet.Cell( SR + i + rofs, 3 + j + cofs ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Quantity" + j.ToString()].Value );
                }
            }

            const int SR1 = 22;
            for ( int i = 0; i < dgv1.Rows.Count; i++ )
            {
                oWSheet.Cell( SR1 + i, 3 ).Value = Convert.ToString( dgv1.Rows[i].Cells["ItemDetail"].Value );
                oWSheet.Cell( SR1 + i, 10 ).Value = Convert.ToDecimal( dgv1.Rows[i].Cells["Cost"].Value );
                oWSheet.Cell( SR1 + i, 12 ).Value = Convert.ToString( dgv1.Rows[i].Cells["Note"].Value );
            }
        }


        // 請負精算書
        private void editAccountsContract( PublishData pd, DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );
            oWSheet.Cell( 1, 1 ).Value = pd.OrderPartner + "御中";
            oWSheet.Cell( 1, 12 ).Value = pd.LeaderName;
            oWSheet.Cell( 1, 14 ).Value = pd.SalesMName;

            oWSheet.Cell( 2, 2 ).Value = pd.TaskCode;
            oWSheet.Cell( 2, 4 ).Value = pd.TaskName;
            oWSheet.Cell( 2, 12 ).Value = pd.PartnerName;

            readyExcelRows( dgv, 2, 15, 4, 5 );
            int maxRowsCount = dgvRowsCount( dgv, 2, 15 );
            const int SR = 4;       // Excel Sheet Start Row No.

            for ( int i = 0; i < maxRowsCount; i++ )
            {
                oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["PQuantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 3 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["PQuantity"].Value );

                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) != "" )
                    oWSheet.Cell( SR + i, 5 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Cost"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["LQuantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 7 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["LQuantity"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 9 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["LQuantity"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["SQuantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 11 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["SQuantity"].Value );
            }
        }


        // 請求書
        private void editAccountsInvoice( PublishData pd, DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );
            oWSheet.Cell( 2, 3 ).Value = pd.PartnerName + " 御中";
            oWSheet.Cell( 4, 3 ).Value = pd.TaskName;

            readyExcelRows( dgv, 3, 8, 12, 12 );
            int maxRowsCount = dgvRowsCount( dgv, 3, 8 );
            const int SR = 11;      // Excel Sheet Start Row No.
            decimal totalAmount = 0;
            string wMonth = "";
            string wDay = "";

            for ( int i = 0; i < maxRowsCount; i++ )
            {
                if ( Convert.ToString( dgv.Rows[i].Cells["Month"].Value ) != wMonth )
                {
                    oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Month"].Value );
                    wMonth = Convert.ToString( dgv.Rows[i].Cells["Month"].Value );
                }

                if ( Convert.ToString( dgv.Rows[i].Cells["Day"].Value ) != wDay )
                {
                    oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["Day"].Value );
                    wDay = Convert.ToString( dgv.Rows[i].Cells["Day"].Value );
                }

                oWSheet.Cell( SR + i, 3 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 5 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Quantity"].Value );

                oWSheet.Cell( SR + i, 6 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) != "" )
                    oWSheet.Cell( SR + i, 7 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Cost"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) != "" )
                {
                    oWSheet.Cell( SR + i, 8 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Amount"].Value );
                    totalAmount += Convert.ToDecimal( dgv.Rows[i].Cells["Amount"].Value );
                }
            }

            oWSheet.Cell( 8, 3 ).Value = totalAmount;
        }


        // 出来高請求書
        private void editAccountsVolumeInvoice( PublishData pd, DataGridView dgv )
        {
            MessageBox.Show( "Excel書込み開始" );
            oWSheet.Cell( 2, 1 ).Value = pd.PartnerName + " 御中";
            oWSheet.Cell( 4, 3 ).Value = pd.TaskName;

            readyExcelRows( dgv, 3, 13, 7, 9 );
            int maxRowsCount = dgvRowsCount( dgv, 2, 15 );
            const int SR = 8;      // Excel Sheet Start Row No.
            string wMonth = "";
            string wDay = "";

            for ( int i = 0; i < maxRowsCount; i++ )
            {
                if ( Convert.ToString( dgv.Rows[i].Cells["Month"].Value ) != wMonth )
                {
                    oWSheet.Cell( SR + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Month"].Value );
                    wMonth = Convert.ToString( dgv.Rows[i].Cells["Month"].Value );
                }

                if ( Convert.ToString( dgv.Rows[i].Cells["Day"].Value ) != wDay )
                {
                    oWSheet.Cell( SR + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["Day"].Value );
                    wDay = Convert.ToString( dgv.Rows[i].Cells["Day"].Value );
                }

                oWSheet.Cell( SR + i, 3 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                oWSheet.Cell( SR + i, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["ItemDetail"].Value );
                // 契約
                if ( Convert.ToString( dgv.Rows[i].Cells["CQuantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 5 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["CQuantity"].Value );

                oWSheet.Cell( SR + i, 6 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["CCost"].Value ) != "" )
                    oWSheet.Cell( SR + i, 7 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["CCost"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["CAmount"].Value ) != "" )
                    oWSheet.Cell( SR + i, 8 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["CAmount"].Value );
                // 前回までの累積
                if ( Convert.ToString( dgv.Rows[i].Cells["SQuantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 9 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["SQuantity"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["SAmount"].Value ) != "" )
                    oWSheet.Cell( SR + i, 10 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["SAmount"].Value );
                // 今回
                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( SR + i, 11 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Quantity"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Amount"].Value ) != "" )
                    oWSheet.Cell( SR + i, 12 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["Amount"].Value );
                // 残
                if ( Convert.ToString( dgv.Rows[i].Cells["RAmount"].Value ) == "" )
                {
                    oWSheet.Cell( SR + i, 13 ).Value = "";
                }
                else
                {
                    oWSheet.Cell( SR + i, 13 ).Value = Convert.ToDecimal( dgv.Rows[i].Cells["RAmount"].Value );
                }
            }

        }

        // 出来高台帳
        private void editVolume( PublishData pd, DataGridView dgv )
        {
            //MessageBox.Show("Excel書込み開始");

            //oWSheet.Cell("A1").Value = pd.strYeah + "年" + "  " + "出来高台帳"; //業務番号
            oWSheet.Cell( "A1" ).Value = pd.vYear + "  " + "出来高台帳";   //業務番号
            oWSheet.Cell( "D2" ).Value = pd.vTaskCode;                    //業務番号
            oWSheet.Cell( "F2" ).Value = pd.vTaskName;                    //業務名
            oWSheet.Cell( "M2" ).Value = pd.vSupplierName;                //業者名
            oWSheet.Cell( "D3" ).Value = pd.vStartDate;                   //工期開始
            oWSheet.Cell( "E3" ).Value = pd.vEndDate;                     //工期終了
            oWSheet.Cell( "G3" ).Value = pd.vOrdersForm;                  //受注形態
            oWSheet.Cell( "I3" ).Value = pd.vCarryOverPlanned;            //繰越予定額
            oWSheet.Cell( "K3" ).Value = pd.vYearCompletionHigh;          //年内完工高
            oWSheet.Cell( "D4" ).Value = pd.vContact;                     //担当者
            oWSheet.Cell( "G4" ).Value = pd.vClaimform;                   //請求形態
            oWSheet.Cell( "I4" ).Value = pd.vPayNote;                     //支払条件
            oWSheet.Cell( "M3" ).Value = pd.vTaskStat;                    //業務状態
            oWSheet.Cell( "D32" ).Value = pd.vNote;                       //備考
            // Wakamatsu
            // Wakamatsu 20170322
            //oWSheet.Cell( "D35" ).Value = pd.vNote2;                      //備考2

            string strCell = "";
            for ( int i = 0; i < 13; i++ )
            {
                switch ( i )
                {
                    case 0://
                        strCell = "D";
                        break;
                    case 1://
                        strCell = "E";
                        break;
                    case 2://
                        strCell = "F";
                        break;
                    case 3://
                        strCell = "G";
                        break;
                    case 4://
                        strCell = "H";
                        break;
                    case 5://
                        strCell = "I";
                        break;
                    case 6://
                        strCell = "J";
                        break;
                    case 7://
                        strCell = "K";
                        break;
                    case 8://
                        strCell = "L";
                        break;
                    case 9://
                        strCell = "M";
                        break;
                    case 10://
                        strCell = "N";
                        break;
                    case 11://
                        strCell = "O";
                        break;
                    case 12://
                        strCell = "P";
                        break;
                }

                for ( int j = 0; j < 22; j++ )
                {
                    string strCellData = strCell + ( j + 6 ).ToString();
                    oWSheet.Cell( strCellData ).Value = "";
                    if ( dgv.Rows[j].Cells[i].Value != null && dgv.Rows[j].Cells[i].Value != DBNull.Value )
                        oWSheet.Cell( strCellData ).Value = dgv.Rows[j].Cells[i].Value;
                }
            }
        }

        private void editVolume( DataGridView Dgv )
        {
            // Wakamatsu 20170313
            //MessageBox.Show( "Excel書込み開始" );
            if ( Dgv.Rows.Count == 0 )
            {
                // テンプレートデータ行削除
                oWSheet.Row( 2 ).Delete();
                return;
            }
            for ( int i = 0; i < Dgv.Rows.Count; i++ )
            {
                using ( IXLRange SetRange = oWSheet.Range( "A2:AD2" ) )
                {
                    // テンプレートデータ行コピー/ペースト
                    SetRange.CopyTo( oWSheet.Cell( i + 2, 1 ) );
                }
                for ( int j = 0; j < 30; j++ )
                {
                    // データ格納
                    oWSheet.Cell( i + 2, j + 1 ).Value = Dgv.Rows[i].Cells[j].Value;
                    // 行の高さ設定
                    oWSheet.Row( i + 2 ).Height = 27.75;
                    if ( i == Dgv.Rows.Count - 1 )
                        // 最終データのみ罫線の設定
                        oWSheet.Cell( i + 2, j + 1 ).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
            }
        }


        public void CreateExcelForPdf( string sheetName, PublishData pd, DataGridView dgv )
        {
            //string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            string tempFile = "";
            Cursor.Current = Cursors.WaitCursor;   // マウスカーソルを砂時計(Wait)
            try
            {
                using ( oWBook = new XLWorkbook( fileName ) )
                {
                    oWSheet = oWBook.Worksheet( sheetName );
                    switch ( sheetName )
                    {
                        case "Volume":

                            // 2018.01 asakawa
                            // tempFile = Folder.DefaultLocation() + @"\" + pd.vTaskCode + ".xlsx";
                            tempFile = Folder.DefaultLocation() + @"\出来高台帳帳票\" + pd.vTaskCode + ".xlsx";
                            // 2018.01 asakawa //

                            editVolume( pd, dgv );
                            break;
                        default:
                            break;
                    }
                    oWBook.SaveAs( tempFile );    // Excel保存
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message );
                Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
                return;
            }
            // Wakamatsu 20170308
            Cursor.Current = Cursors.Default;  // マウスカーソルを戻す
        }


        /// <summary>
        /// DataGridViewの最後にデータが存在する行数と現在のエクセルの行数を比較し、エクセルの行数が少ないようなら
        /// エクセルの行数を増加させる。
        /// 比較の差異は列ヘッダーの行数、合計行の行数は除外する。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="stCol"></param>
        /// <param name="endCol"></param>
        /// <param name="headerLine"></param>
        private void readyExcelRows( DataGridView dgv, int stCol, int endCol, int exLine )
        {
            int lastRows = dgvRowsCount( dgv, stCol, endCol );    // 表示対象の最終行 
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            if ( lastRows > sheetRowsCount )
            {
                var rowCount = oWSheet.Row( 6 ).InsertRowsBelow( lastRows - sheetRowsCount );
            }
        }


        private void readyExcelRows( DataGridView dgv, int stCol, int endCol, int exLine, int rowPos )
        {
            int lastRows = dgvRowsCount( dgv, stCol, endCol );    // 表示対象の最終行 
            // 不足行追加
            int sheetRowsCount = oWSheet.LastRowUsed().RowNumber();     // Excel最終行位置
            sheetRowsCount -= exLine;                           // タイトル行と合計行を行数から除く
            if ( lastRows > sheetRowsCount )
            {
                var rowCount = oWSheet.Row( rowPos ).InsertRowsBelow( lastRows - sheetRowsCount + 1 );
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
            for ( int i = 0; i < dgv.Rows.Count; i++ )
            {
                for ( int j = stCol; j < endCol; j++ )
                {
                    if ( Convert.ToString( dgv.Rows[i].Cells[j].Value ) != "" ) colsCount++;
                }
                if ( colsCount > 0 ) rowsCount = i;
                colsCount = 0;
            }

            return rowsCount + 1;
        }


        private void editCostDetail( PublishData pd, DataGridView dgv )
        {
            //MessageBox.Show("Excel書込み開始");
            oWSheet.Cell( 3, 2 ).Value = pd.CostOffice;   //部署
            oWSheet.Cell( 4, 2 ).Value = pd.CostReportDate;   //計上日
            oWSheet.Cell( 5, 2 ).Value = pd.CostTypeData;     //明細表
            oWSheet.Cell( 6, 2 ).Value = pd.CostRange;        //出力範囲

            oWSheet.Cell( 12, 3 ).Value = dgv.Columns[2].HeaderCell.Value;
            oWSheet.Cell( 12, 4 ).Value = dgv.Columns[3].HeaderCell.Value;

            if ( Convert.ToString( oWSheet.Cell( 12, 3 ).Value ) == Sign.lblItem )
            {
                oWSheet.Column( 3 ).Width = 20;
                oWSheet.Column( 4 ).Width = 36;
            }
            else
            {
                oWSheet.Column( 3 ).Width = 36;
                oWSheet.Column( 4 ).Width = 20;
            }

            oWSheet.Range( "A12:H12" ).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            oWSheet.Range( "A12:H12" ).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            int cols = 8;       // カラム数
            for ( int j = 1; j < cols + 1; j++ )
            {
                if ( j == 1 ) oWSheet.Cell( 12, j ).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                if ( j == cols ) oWSheet.Cell( 12, j ).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                if ( j != 1 && j != cols ) oWSheet.Cell( 12, j ).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
            }

            string wkItem = "";
            bool cell1 = false;
            bool cell3 = false;
            bool cellAll = false;
            int stLine = 13;

            for ( int i = 0; i < dgv.RowCount; i++ )
            {
                oWSheet.Row( stLine + 1 + i ).InsertRowsAbove( 1 );   //行の挿入


                if ( Convert.ToString( dgv.Rows[i].Cells["Date"].Value ) != "" )
                {
                    oWSheet.Cell( stLine + i, 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Date"].Value );
                    cell1 = true;
                }
                else
                {
                    cell1 = false;
                }


                if ( Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value ) != "" )
                    oWSheet.Cell( stLine + i, 2 ).Value = Convert.ToString( dgv.Rows[i].Cells["SlipNo"].Value );


                if ( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) != "" )
                {
                    if ( wkItem != Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) )
                    {
                        // Wakamatsu 20170307
                        //oWSheet.Cell(stLine + i, 3).Value = Convert.ToString(dgv.Rows[i].Cells["Item"].Value);
                        oWSheet.Cell( stLine + i, dgv.Columns["Item"].Index + 1 ).Value = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                        // Wakamatsu 20170307
                        wkItem = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                        cell3 = true;
                    }
                    else
                    {
                        cell3 = false;
                    }
                }


                string wkName = Convert.ToString( dgv.Rows[i].Cells["Task"].Value );
                cellAll = false;
                if ( wkName != "" )
                    // Wakamatsu 20170307
                    //oWSheet.Cell(stLine + i, 4).Value = wkName;
                    oWSheet.Cell( stLine + i, dgv.Columns["Task"].Index + 1 ).Value = wkName;
                // Wakamatsu 20170307

                if ( wkName == Sign.lblsumTask || wkName == Sign.lblsumItem || wkName == Sign.lblsumTerm )
                    cellAll = true;

                if ( Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value ) != "" )
                    oWSheet.Cell( stLine + i, 5 ).Value = Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Unit"].Value ) != "" )
                    oWSheet.Cell( stLine + i, 6 ).Value = Convert.ToString( dgv.Rows[i].Cells["Unit"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["UnitPrice"].Value ) != "" )
                    oWSheet.Cell( stLine + i, 7 ).Value = Convert.ToString( dgv.Rows[i].Cells["UnitPrice"].Value );

                if ( Convert.ToString( dgv.Rows[i].Cells["Cost"].Value ) != "" )
                    oWSheet.Cell( stLine + i, 8 ).Value = Convert.ToString( dgv.Rows[i].Cells["Cost"].Value );

                for ( int j = 1; j < cols + 1; j++ )
                {
                    if ( j == 1 ) oWSheet.Cell( stLine + i, 1 ).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    if ( j == cols ) oWSheet.Cell( stLine + i, cols ).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    if ( cellAll )
                    {
                        oWSheet.Cell( stLine + i, j ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        if ( j == 4 || j == 5 || j == 6 || j == 8 )
                        {
                            oWSheet.Cell( stLine + i, j ).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
                        }
                        else
                        {
                            if ( j != 1 )
                            {
                                oWSheet.Cell( stLine + i, j ).Style.Border.LeftBorder = XLBorderStyleValues.None;
                                oWSheet.Cell( stLine + i, j - 1 ).Style.Border.RightBorder = XLBorderStyleValues.None;
                            }
                        }
                    }
                    else
                    {
                        if ( cell3 )
                        {
                            oWSheet.Cell( stLine + i, j ).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        }
                        else
                        {
                            if ( j == 1 && cell1 )
                                oWSheet.Cell( stLine + i, 1 ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                            if ( j != 1 && j != 3 )
                                oWSheet.Cell( stLine + i, j ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                            if ( j != 1 && j != cols ) oWSheet.Cell( stLine + i, j ).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
                        }
                    }
                }
            }

            // Wakamatsu 20170307
            oWSheet.Row( stLine + dgv.RowCount ).Delete();            // 行削除

            // 2018.01 asakawa
            for (int j = 1; j < cols + 1; j++)
            {
                oWSheet.Cell(stLine + dgv.RowCount, j).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            }
            // 2018.01 asakawa //

        }


        private void editCostSummary( PublishData pd, DataGridView dgv )
        {
            //MessageBox.Show("Excel書込み開始");

            oWSheet.Cell( 3, 1 ).Value = "      部署： " + pd.CostOffice;          //部署
            oWSheet.Cell( 4, 1 ).Value = "   計上日： " + pd.CostReportDate;    //計上日
            oWSheet.Cell( 5, 1 ).Value = "   明細表： " + pd.CostTypeData;      //明細表
            oWSheet.Cell( 6, 1 ).Value = "出力範囲：";                       //出力範囲 タイトル
            oWSheet.Cell( 7, 1 ).Value = pd.CostRange;                     //出力範囲 内容

            // Wakamatsu 20170316
            //oWSheet.Cell(12, 1).Value = dgv.Columns[0].HeaderCell.Value;
            //oWSheet.Cell(12, 2).Value = dgv.Columns[1].HeaderCell.Value;
            // Wakamatsu 20170316

            // Wakamatsu 20170316
            //if ( Convert.ToString( oWSheet.Cell( 12, 1 ).Value ) == Sign.lblItem )
            //{
            //    oWSheet.Column( 1 ).Width = 22;
            //    oWSheet.Column( 2 ).Width = 50;
            //}
            //else
            //{
            //    oWSheet.Column( 1 ).Width = 50;
            //    oWSheet.Column( 2 ).Width = 22;
            //}
            string LeftCell = "";               // A列格納列名設定用
            string MidCell = "";                // B列格納列名設定用
            string RightCell = "";              // C列格納列名設定用

            // データグリッドビュー1列目確認
            if (dgv.Columns[0].Visible == true)
                LeftCell = "Class0";

            // データグリッドビュー2列目確認
            if (dgv.Columns[1].Visible == true)
            {
                if (LeftCell == "")
                    LeftCell = "Class1";
                else
                    MidCell = "Class1";
            }

            // データグリッドビュー3列目確認
            if (dgv.Columns[2].Visible == true)
            {
                if (LeftCell == "")
                    LeftCell = "Class2";
                else
                {
                    if (MidCell == "")
                        MidCell = "Class2";
                    else
                        RightCell = "Class2";
                }
            }

            // どれも設定されなかった場合1列目を設定
            if (LeftCell == "")
                LeftCell = "Class0";

            // 項目名を設定
            oWSheet.Cell(12, 1).Value = dgv.Columns[LeftCell].HeaderCell.Value;
            if (MidCell != "")
                oWSheet.Cell(12, 2).Value = dgv.Columns[MidCell].HeaderCell.Value;
            if (RightCell != "")
                oWSheet.Cell(12, 3).Value = dgv.Columns[RightCell].HeaderCell.Value;

            // 設定された項目名で列幅設定
            if (Convert.ToString(oWSheet.Cell(12, 1).Value) == Sign.lblTask)
            {
                // 1列目が業務番号
                oWSheet.Column(1).Width = 50;
                oWSheet.Column(2).Width = 22;
                oWSheet.Column(3).Width = 22;
            }
            else if (Convert.ToString(oWSheet.Cell(12, 2).Value) == Sign.lblTask)
            {
                // 2列目が業務番号
                oWSheet.Column(1).Width = 22;
                oWSheet.Column(2).Width = 50;
                oWSheet.Column(3).Width = 22;
            }
            else if (Convert.ToString(oWSheet.Cell(12, 3).Value) == Sign.lblTask)
            {
                // 3列目が業務番号
                oWSheet.Column(1).Width = 22;
                oWSheet.Column(2).Width = 22;
                oWSheet.Column(3).Width = 50;
            }
            else
            {
                // 業務番号なし
                oWSheet.Column(1).Width = 30;
                oWSheet.Column(2).Width = 30;
                oWSheet.Column(3).Width = 30;
            }
            // Wakamatsu 20170316

            // Wakamatsu 20170316
            //int cols = 5;       // カラム数
            int cols = 6;       // カラム数
            // Wakamatsu 20170316
            for (int j = 1; j < cols + 1; j++)
            {
                if (j == 1) oWSheet.Cell(12, j).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                if (j == cols) oWSheet.Cell(12, j).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                if (j != 1 && j != cols) oWSheet.Cell(12, j).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
            }

            // Wakamatsu 20170316
            //string wkItem = "";
            //string wkName = "";
            string Class0String = "";               // 前回書き込み文字列格納用
            string Class1String = "";               // 前回書き込み文字列格納用
            string Class2String = "";               // 前回書き込み文字列格納用
            //bool cell1 = false;
            //bool cell2 = false;
            // Wakamatsu 20170316
            bool cellAll = false;
            int stLine = 13;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                oWSheet.Row(stLine + 1 + i).InsertRowsAbove(1);   //行の挿入

                // Wakamatsu 20170316
                //// 原価項目
                //if ( Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) != "" )
                //{
                //    if ( wkItem != Convert.ToString( dgv.Rows[i].Cells["Item"].Value ) )
                //    {
                //        wkItem = Convert.ToString( dgv.Rows[i].Cells["Item"].Value );
                //        oWSheet.Cell( stLine + i, dgv.Columns["Item"].Index + 1 ).Value = wkItem;
                //        cell1 = true;
                //    }
                //    else
                //    {
                //        cell1 = false;
                //    }
                //}
                //else
                //{
                //    cell1 = false;
                //}

                //// 業務
                //if ( Convert.ToString( dgv.Rows[i].Cells["Task"].Value ) != "" )
                //{
                //    if ( wkName != Convert.ToString( dgv.Rows[i].Cells["Task"].Value ) )
                //    {
                //        wkName = Convert.ToString( dgv.Rows[i].Cells["Task"].Value );
                //        oWSheet.Cell( stLine + i, dgv.Columns["Task"].Index + 1 ).Value = wkName;
                //        cell2 = true;
                //    }
                //    else
                //    {
                //        cell2 = false;
                //    }
                //    cellAll = false;
                //}

                // A列設定(同じ文字列は格納しない)
                if (Class0String != Convert.ToString(dgv.Rows[i].Cells[LeftCell].Value))
                {
                    oWSheet.Cell(i + stLine, 1).Value = Convert.ToString(dgv.Rows[i].Cells[LeftCell].Value);
                    if (Convert.ToString(dgv.Rows[i].Cells[LeftCell].Value) != "")
                    {
                        Class0String = Convert.ToString(dgv.Rows[i].Cells[LeftCell].Value);
                        oWSheet.Range(stLine + i, 1, stLine + i, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cellAll = true;
                    }
                }

                if (MidCell != "")
                {
                    // B列設定(同じ文字列は格納しない)
                    if (Class1String != Convert.ToString(dgv.Rows[i].Cells[MidCell].Value))
                    {
                        oWSheet.Cell(i + stLine, 2).Value = Convert.ToString(dgv.Rows[i].Cells[MidCell].Value);
                        if (Convert.ToString(dgv.Rows[i].Cells[MidCell].Value) != "")
                        {
                            Class1String = Convert.ToString(dgv.Rows[i].Cells[MidCell].Value);
                            if (cellAll == false)
                                oWSheet.Range(stLine + i, 2, stLine + i, 6).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                        }
                    }
                }

                if (RightCell != "")
                {
                    // C列設定(常に設定)
                    oWSheet.Cell(i + stLine, 3).Value = Convert.ToString(dgv.Rows[i].Cells[RightCell].Value);
                    Class2String = Convert.ToString(dgv.Rows[i].Cells[RightCell].Value);
                    if (cellAll == false)
                        oWSheet.Range(stLine + i, 3, stLine + i, 6).Style.Border.TopBorder = XLBorderStyleValues.Hair;
                }
                //if ( wkName == Sign.lblsumTask || wkName == Sign.lblsumItem || wkName == Sign.lblsumTerm )
                //    cellAll = true;
                // Wakamatsu 20170316

                if (Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value) != "")
                    // Wakamatsu 20170316
                    //oWSheet.Cell( i + stLine, 3 ).Value = Convert.ToString( dgv.Rows[i].Cells["Quantity"].Value );
                    oWSheet.Cell(i + stLine, 4).Value = Convert.ToString(dgv.Rows[i].Cells["Quantity"].Value);

                if (Convert.ToString(dgv.Rows[i].Cells["UnitPrice"].Value) != "")
                    // Wakamatsu 20170316
                    //oWSheet.Cell( i + stLine, 4 ).Value = Convert.ToString( dgv.Rows[i].Cells["UnitPrice"].Value );
                    oWSheet.Cell(i + stLine, 5).Value = Convert.ToString(dgv.Rows[i].Cells["UnitPrice"].Value);
                else
                    if (MidCell == "" && RightCell == "")
                    oWSheet.Range(stLine + i, 1, stLine + i, 6).Style.Border.TopBorder = XLBorderStyleValues.Hair;

                if (Convert.ToString(dgv.Rows[i].Cells["Cost"].Value) != "")
                    // Wakamatsu 20170316
                    //oWSheet.Cell( i + stLine, 5 ).Value = Convert.ToString( dgv.Rows[i].Cells["Cost"].Value );
                    oWSheet.Cell(i + stLine, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Cost"].Value);

                //if (Convert.ToString(dgv.Rows[i].Cells["Customer"].Value) != "")
                //    oWSheet.Cell(i + stLine, 6).Value = Convert.ToString(dgv.Rows[i].Cells["Customer"].Value);

                //if (Convert.ToString(dgv.Rows[i].Cells["LeaderMName"].Value) != "")
                //    oWSheet.Cell(i + stLine, 7).Value = Convert.ToString(dgv.Rows[i].Cells["LeaderMName"].Value);

                //if (Convert.ToString(dgv.Rows[i].Cells["SalesMName"].Value) != "")
                //    oWSheet.Cell(i + stLine, 8).Value = Convert.ToString(dgv.Rows[i].Cells["SalesMName"].Value);

                for (int j = 1; j < cols + 1; j++)
                {
                    switch (j)
                    {
                        case 1:         // A列
                            oWSheet.Cell(stLine + i, j).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            // Wakamatsu 20170316
                            //if (cell1) oWSheet.Cell(stLine + i, j).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            oWSheet.Cell(stLine + i, j).Style.Border.RightBorder = XLBorderStyleValues.Hair;
                            // Wakamatsu 20170316
                            break;
                        //case 2:
                        //    oWSheet.Cell( stLine + i, j ).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
                        //    checkCell1And2( cell1, cell2, stLine + i, j );
                        //    break;
                        case 5:         // E列
                            // Wakamatsu 20170316
                            //oWSheet.Cell(stLine + i, cols).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            //if (cellAll)
                            //{
                            //    oWSheet.Cell(stLine + i, j).Style.Border.LeftBorder = XLBorderStyleValues.None;
                            //    oWSheet.Cell(stLine + i, j - 1).Style.Border.RightBorder = XLBorderStyleValues.None;
                            //}
                            //else
                            //{
                            //    oWSheet.Cell(stLine + i, j).Style.Border.LeftBorder = XLBorderStyleValues.Hair;
                            //}
                            //checkCell1And2(cell1, cell2, stLine + i, j);
                            if (Convert.ToString(oWSheet.Cell(i + stLine, 5).Value) != "")
                                oWSheet.Cell(stLine + i, j).Style.Border.RightBorder = XLBorderStyleValues.Hair;
                            // Wakamatsu 20170316
                            break;
                        // Wakamatsu 20170316
                        case 6:         // F列
                            oWSheet.Cell(stLine + i, j).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            break;
                        // Wakamatsu 20170316
                        default:        // 上記以外
                            oWSheet.Cell(stLine + i, j).Style.Border.RightBorder = XLBorderStyleValues.Hair;
                            // Wakamatsu 20170316
                            //checkCell1And2(cell1, cell2, stLine + i, j);
                            // Wakamatsu 20170316
                            break;
                    }
                }
                // Wakamatsu 20170316
                cellAll = false;
            }

            // Wakamatsu 20170316
            // 1行目設定
            oWSheet.Range(stLine, 1, stLine, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            // 最終行設定
            oWSheet.Range(stLine + dgv.RowCount - 1, 1, stLine + dgv.RowCount - 1, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            // 列削除
            if (RightCell == "")
            {
                oWSheet.Column(3).Delete();
                oWSheet.Column(6).Width = 0;
                oWSheet.Column(1).Width += 11;
                oWSheet.Column(2).Width += 11;
            }
            if (MidCell == "")
            {
                oWSheet.Column(2).Delete();
                oWSheet.Column(6).Width = 0;
                oWSheet.Column(1).Width += 33;
            }
            // Wakamatsu 20170316

            oWSheet.Row(stLine + dgv.RowCount).Delete();            // 行削除

            // Wakamatsu 20170316
            oWSheet.PageSetup.FitToPages(1, 0);
        }


        private void checkCell1And2( bool cell1, bool cell2, int rIdx, int cIdx )
        {
            if ( cell1 )
            {
                oWSheet.Cell( rIdx, cIdx ).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            }
            else
            {
                if ( cell2 ) oWSheet.Cell( rIdx, cIdx ).Style.Border.TopBorder = XLBorderStyleValues.Hair;
            }
        }







    }
}

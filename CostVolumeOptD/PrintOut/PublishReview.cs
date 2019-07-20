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
    public class PublishReview
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/

        ClosedXML.Excel.XLWorkbook oWBook = null;  // Excel Workbookオブジェクト
        ClosedXML.Excel.IXLWorksheet oWSheet;
        private string fileName;
        private VolumeData [] volt;
        private int[] rowNo = new int[] { 4, 6, 7, 8, 12, 14, 16, 17, 18, 21, 22 };
        const int Columns = 13;
        private string[,] pubDat = new string[12, 13];
        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        public PublishReview()
        {
        }

        public PublishReview(string fileName)
        {
            this.fileName = fileName;
        }

        public PublishReview(string fileName, VolumeData[] volt)
        {
            this.fileName = fileName;
            this.volt = volt;
        }

        public PublishReview(string fileName, string[,] pubDat)
        {
            this.fileName = fileName;
            this.pubDat = pubDat;
        }
        //---------------------------------------------------------//
        //      Property
        //---------------------------------------------------------//
        public string FileName { get; set; }
        //---------------------------------------------------------//
        //      Method
        //---------------------------------------------------------//
        public void ExcelFile()
        {
            string tempFile = Folder.DefaultLocation() + @"\.~temp.xlsx";
            Cursor.Current = Cursors.WaitCursor;                // マウスカーソルを砂時計(Wait)

            try
            {
                using (oWBook = new XLWorkbook(fileName))
                {
                    // 編集
                    oWSheet = oWBook.Worksheet(1);      // シートを開く
                    editReviewData(pubDat);

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

            Cursor.Current = Cursors.Default;               // マウスカーソルを戻す
            System.Diagnostics.Process.Start("Excel.exe", tempFile);                    // 表示用Excel
        }


        //-----------------------------------------------------------//
        // SubRoutine
        //-----------------------------------------------------------//
        private void editReviewData(string[,] pubDat)
        {
            int limCol = Convert.ToInt32( pubDat[11, 5] );

            int sCNo = 4;
            for( int i = 0; i < Columns; i++ )
            {
                for (int j = 0; j < rowNo.Length; j++)
                {
                    oWSheet.Cell(rowNo[j], sCNo + i).Value = pubDat[j, i] ?? null;
                }

                if(i >= limCol )
                {
                    oWSheet.Cell(5, sCNo + i).Value = null;
                    oWSheet.Cell(11, sCNo + i).Value = null;
                }
            }

            oWSheet.Cell(1, 7).Value = pubDat[11, 0] + "年度 " + pubDat[11, 1] + pubDat[11, 2] + " 総括表";
            oWSheet.Cell(2, 4).Value = pubDat[11, 3];
            oWSheet.Cell(2, 6).Value = pubDat[11, 4];
        }

    }
}

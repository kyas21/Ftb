using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace PrintOut
{
    class PublishExcelToPdf
    {
        //---------------------------------------------------------/
        //      Field
        //---------------------------------------------------------/
        private bool delFlag;                        // Excelファイル削除フラグ

        //---------------------------------------------------------/
        //      Construction
        //---------------------------------------------------------/
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PublishExcelToPdf()
        {
            this.delFlag = true;
        }

        /// <summary>
        /// コンストラクター(Excelファイル削除設定付き
        /// </summary>
        /// <param name="delFlag">Excelファイル削除フラグ True:削除 Flase:削除なし</param>
        public PublishExcelToPdf(bool delFlag)
        {
            this.delFlag = delFlag;
        }
        //---------------------------------------------------------/
        //      Property
        //---------------------------------------------------------/
        //---------------------------------------------------------/
        //      Method
        //---------------------------------------------------------/
        /// <summary>
        /// ExcelファイルをPDFファイルに変換
        /// </summary>
        /// <param name="sourceFile">処理対象Excelパス(フルパス)</param>
        /// <returns>処理結果 True:正常終了 False:異常終了</returns>
        public bool ExcelToPDF(string sourceFile)
        {
            return convertPDF(sourceFile);
        }


        /// <summary>
        /// Excelファイルを指定された名称のPDFファイルに変換
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public bool ExcelToPDF(string sourceFile, string targetFile)
        {
            // Wakamatsu 20170313
            try
            {
                File.Copy(sourceFile, targetFile, true);
                File.Delete(sourceFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            // Wakamatsu 20170313

            return convertPDF(targetFile);
        }


        private bool convertPDF(string excelFile)
        {
            string pdfFile = excelFile.Replace(".xlsx", ".pdf");          // PDFファイルフルパス

            XlFixedFormatType format = XlFixedFormatType.xlTypePDF;
            XlFixedFormatQuality quality = XlFixedFormatQuality.xlQualityStandard;

            Microsoft.Office.Interop.Excel.Application app = null;
            Workbook workbook = null;
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                workbook = app.Workbooks.Open(excelFile);                    //---ブックを開いて
                workbook.ExportAsFixedFormat(format, pdfFile, quality);      //--- PDF形式で出力

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                if (workbook != null)
                {
                    // Wakamatsu 20170313
                    workbook.Close(false);
                    // Wakamatsu 20170313
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;

                if (excelFile != "" && delFlag == true)
                {
                    if(File.Exists(excelFile))
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(1000);
                            // 対象Excelファイル削除
                            File.Delete(excelFile);
                        }
                        catch(IOException)
                        {
                        }
                    }
                }
            }

        }


    }
}

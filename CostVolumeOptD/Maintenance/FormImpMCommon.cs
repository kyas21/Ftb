using ClassLibrary;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maintenance
{
    public partial class FormImpMCommon : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        // Wakamatsu 20170301
        const string masterName = "共通マスタ";
        const string BookName = masterName + ".xlsx";
        const string SheetName = "M_Common";
        // Wakamatsu 20170301
        private bool initProc = true;               // 初期処理中true,初期処理完了false
        private string fileName;
        ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormImpMCommon()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpMCommon_Load(object sender, EventArgs e)
        {
            textBoxMsg.Text = "";
        }


        private void FormImpMCommon_Shown(object sender, EventArgs e)
        {
            initProc = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (initProc) return;

            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonOpen":
                    // Wakamatsu 20170301
                    //fileName = Files.Open("M_Common.xlsx", Folder.MyDocuments(), "xlsx");
                    fileName = Files.Open(BookName, Folder.MyDocuments(), "xlsx");
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× " + fileName + "は不適切なファイルです。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "の内容を共通利用データマスタに書き込みます。\r\n");
                    }
                    break;
                case "buttonCancel":
                    // Wakamatsu 20170323
                    fileName = null;
                    textBoxMsg.Text = "";
                    break;
                case "buttonStart":
                    if (fileName == null)
                    {
                        // Wakamatsu 20170323
                        textBoxMsg.AppendText("× 取り込むファイルを指定してください。\r\n");
                        return;
                    }

                    MasterMaintOp mmo = new MasterMaintOp();
                    // Wakamatsu 20170227
                    //if (!mmo.AllMCommon_Delete())
                    //{
                    //    textBoxMsg.AppendText("× 旧データの削除に失敗しました処理を中断します。\r\n");
                    //    return;
                    //}
                    //mmo = new MasterMaintOp();
                    // Wakamatsu 20170227
                    int procCount = 0;
                    switch (System.IO.Path.GetExtension(fileName))
                    {
                        case ".xlsx":
                            // Wakamatsu 20170227
                            try
                            {
                                oWBook = new XLWorkbook(fileName);
                                procCount = mmo.MaintCommonByExcelData(oWBook.Worksheet(1));

                                // Wakamatsu 20170227
                                if (procCount < 0)
                                {
                                    if (procCount == -2)
                                        textBoxMsg.AppendText("× 旧データの削除に失敗しました処理を中断します。\r\n");

                                    textBoxMsg.AppendText("× " + fileName + "を処理できませんでした。\r\n");
                                    return;
                                }
                                // Wakamatsu 20170227
                            }
                            // Wakamatsu 20170227
                            catch (Exception ex)
                            {
                                textBoxMsg.AppendText(ex.Message + "\r\n");
                                textBoxMsg.AppendText("× " + fileName + "を処理できませんでした。\r\n");
                                return;
                            }
                            // Wakamatsu 20170227
                            break;
                        default:
                            procCount = -1;
                            textBoxMsg.AppendText("〇 " + fileName + "は処理できないファイルです。\r\n");
                            break;
                    }

                    if (procCount < 0) return;
                    textBoxMsg.AppendText("〇 " + fileName + "を処理しました。\r\n");
                    textBoxMsg.AppendText(procCount + "件のデータを登録しました。\r\n");
                    break;
                // Wakamatsu
                case "buttonExport":
                    textBoxMsg.AppendText("☆ 処理を開始しました。\r\n");
                    string SetSQL = "";

                    SetSQL += "Kind, ComData, ComSignage, ComSymbol, UsedNote ";
                    SetSQL += "FROM M_Common ";
                    SetSQL += "ORDER BY Kind";

                    SqlHandling sqlh = new SqlHandling();               // SQL実行クラス
                    // レコードを取得する
                    DataTable dt = sqlh.SelectFullDescription(SetSQL);
                    if (dt == null)
                    {
                        textBoxMsg.AppendText("× Excel出力ができませんでした。\r\n");
                        return;
                    }

                    // フォーマット設定用構造体
                    PrintOut.Publish.FormatSet[] FormatSet = new PrintOut.Publish.FormatSet[dt.Columns.Count];
                    // フォーマット設定
                    FormatSetting(ref FormatSet);

                    // Excel出力クラス
                    // Wakamatsu 20170301
                    //PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate("M_Common.xlsx"));
                    PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate(BookName));
                    // Wakamatsu 20170301
                    // Excelファイル出力
                    // Wakamatsu 20170301
                    //textBoxMsg.AppendText(publ.ExcelFile("M_Common", dt, FormatSet));
                    textBoxMsg.AppendText(publ.ExcelFile(masterName, SheetName, dt, FormatSet));
                    // Wakamatsu 20170301
                    break;
                // Wakamatsu
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// フォーマット設定
        /// </summary>
        /// <param name="FormatSet">フォーマット設定構造体</param>
        private void FormatSetting(ref PrintOut.Publish.FormatSet[] FormatSet)
        {
            for (int i = 0; i < FormatSet.Length; i++)
            {
                switch (i)
                {
                    case 0:             // 種別
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                    case 1:             // 内容
                    case 2:             // 内容表記
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 3:             // 記号
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                    default:            // 上記以外
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                }
            }
        }
        // Wakamatsu
    }
}

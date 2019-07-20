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
    public partial class FormImpMPartners : Form
    {
        //----------------------------------------------------------------------
        //     Field
        //----------------------------------------------------------------------
        // Wakamatsu 20170301
        const string masterName = "取引先マスタ";
        const string BookName = masterName + ".xlsx";
        const string SheetName = "M_Partners";
        // Wakamatsu 20170301
        private bool initProc = true;       // 初期処理中true,初期処理完了false

        private string fileName;
        ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        //----------------------------------------------------------------------
        //     Contructor
        //----------------------------------------------------------------------
        public FormImpMPartners()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpMPartners_Load(object sender, EventArgs e)
        {
            textBoxMsg.Text = "";
        }



        private void FormImpMPartners_Shown(object sender, EventArgs e)
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
                    //fileName = Files.Open("M_Partners.xlsx", Folder.MyDocuments(), "xlsx");
                    fileName = Files.Open(BookName, Folder.MyDocuments(), "xlsx");
                    // Wakamatsu 20170301
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× " + fileName + "は不適切なファイルです。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "の内容で取引先マスタを登録・更新します。\r\n");
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
                    /*
                    if (!mmo.AllMPartners_Delete())
                    {
                        labelMsg.Text += "旧データの削除に失敗しました処理を中断します。\r\n";
                        return;
                    }
                    */
                    int[] procArray = new int[] { 0, 0 };
                    switch (System.IO.Path.GetExtension(fileName))
                    {
                        case ".xlsx":
                            // Wakamatsu 20170227
                            try
                            {
                                oWBook = new XLWorkbook(fileName);
                                procArray = mmo.MaintPartnersByExcelData(oWBook.Worksheet(1));

                                // Wakamatsu 20170227
                                if (procArray[0] < 0)
                                {
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
                            procArray[0] = -1;
                            textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです。\r\n");
                            break;
                    }

                    if (procArray[0] < 0) return;
                    textBoxMsg.AppendText("〇 " + fileName + "を処理しました。\r\n");
                    textBoxMsg.AppendText(procArray[0] + "件のデータを登録しました。\r\n");
                    textBoxMsg.AppendText(procArray[1] + "件のデータを更新しました。\r\n");
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                case "buttonExport":
                    textBoxMsg.AppendText("☆ 処理を開始しました。\r\n");
                    string SetSQL = "";

                    SetSQL += "PartnerCode, PartnerName, PartnerPhonetic, CorporateForm, ";
                    SetSQL += "SignPosition, PostCode, Address, TelNo, FaxNo, Capital, ";
                    SetSQL += "Representative, Title, CellularNo, EMail, BankName, ";
                    SetSQL += "BBranchName, AccountType, AccountNo, ClosingDay, PayDay, ";
                    SetSQL += "PayType, PayLT, RelCusto, RelSubco, RelSuppl, RelOther, ";
                    SetSQL += "StartDate, AccountCode, ChiefTrans ";
                    SetSQL += "FROM M_Partners ";
                    SetSQL += "ORDER BY RIGHT('000000' + PartnerCode,6)";

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
                    //PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate("M_Partners.xlsx"));
                    PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate(BookName));
                    // Wakamatsu 20170301
                    // Excelファイル出力
                    // Wakamatsu 20170301
                    //textBoxMsg.AppendText(publ.ExcelFile("M_Partners",dt,FormatSet));
                    textBoxMsg.AppendText(publ.ExcelFile(masterName, SheetName, dt, FormatSet));
                    // Wakamatsu 20170301
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
            for(int i = 0;i < FormatSet.Length;i++)
            {
                switch(i)
                {
                    case 0:             // ID
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Right;
                        break;
                    case 9:             // 資本金
                        FormatSet[i].SetFormat = "#,0";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 17:            // 口座番号
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = true;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 26:            // 開始日
                        FormatSet[i].SetFormat = "yyyy-mm-dd";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
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
    }
}

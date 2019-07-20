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
    public partial class FormImpMCost : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        // Wakamatsu 20170301
        const string masterName = "原価マスタ";
        const string BookName = masterName + ".xlsx";
        const string SheetName = "M_Cost";
        // Wakamatsu 20170301
        private bool initProc = true;               // 初期処理中true,初期処理完了false
        private string fileName;
        ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormImpMCost()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpMCost_Load(object sender, EventArgs e)
        {
            textBoxMsg.Text = "";
        }


        private void FormImpMCost_Shown(object sender, EventArgs e)
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
                    //fileName = Files.Open("M_Cost.xlsx", Folder.MyDocuments(), "xlsx");
                    fileName = Files.Open(BookName, Folder.MyDocuments(), "xlsx");
                    // Wakamatsu 20170301
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× " + fileName + "は不適切なファイルです。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "の内容を原価情報マスタに書き込みます。\r\n");
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
                    if (!mmo.AllMCost_Delete())
                    {
                        labelMsg.Text += "旧データの削除に失敗しました処理を中断します。\r\n";
                        return;
                    }
                    mmo = new MasterMaintOp();
                    */
                    int[] procCount = new int[] { 0, 0 };
                    switch (System.IO.Path.GetExtension(fileName))
                    {
                        //case ".csv":
                        //procCount = mmo.MaintCostByCSVData(fileName);
                        //break;
                        case ".xlsx":
                            // Wakamatsu 20170227
                            try
                            {
                                oWBook = new XLWorkbook(fileName);
                                procCount = mmo.MaintCostByExcelData(oWBook.Worksheet(1));

                                // Wakamatsu 20170227
                                if (procCount[0] < 0)
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
                            textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです。\r\n");
                            break;
                    }

                    if (procCount[0] < 0) return;
                    textBoxMsg.AppendText("〇 " + fileName + "を処理しました。\r\n");
                    textBoxMsg.AppendText(procCount[0] + "件のデータを登録しました。\r\n");
                    textBoxMsg.AppendText(procCount[1] + "件のデータを更新しました。\r\n");
                    break;
                // Wakamatsu
                case "buttonExport":
                    textBoxMsg.AppendText("☆ 処理を開始しました。\r\n");
                    string SetSQL = "";

                    // Wakamatsu 20170303
                    SqlHandling sqlh = new SqlHandling();               // SQL実行クラス

                    SetSQL += "OfficeCode, OfficeName ";
                    SetSQL += "FROM M_Office ";
                    SetSQL += "ORDER BY OfficeID";

                    DataTable dt = sqlh.SelectFullDescription(SetSQL);
                    if (dt == null)
                    {
                        textBoxMsg.AppendText("× Excel出力ができませんでした。\r\n");
                        return;
                    }

                    PrintOut.Publish.FormatSet[] FormatSet = null;              // フォーマット設定用構造体
                    PrintOut.Publish publ = null;                               // Excel出力クラス

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SetSQL = "";
                        SetSQL += "CostCode, Item, ItemDetail, Unit, Cost, ";
                        SetSQL += "OfficeCode, MemberCode ";
                        SetSQL += "FROM M_Cost ";
                        SetSQL += "WHERE OfficeCode = '" + Convert.ToString(dt.Rows[i][0]) + "' ";
                        SetSQL += "ORDER BY CostCode";

                        DataTable dt1 = sqlh.SelectFullDescription(SetSQL);
                        if (dt1 != null)
                        {
                            FormatSet = new PrintOut.Publish.FormatSet[dt1.Columns.Count];
                            // フォーマット設定
                            FormatSetting(ref FormatSet);

                            publ = new PrintOut.Publish(Folder.DefaultExcelTemplate(BookName));
                            // Excelファイル出力
                            textBoxMsg.AppendText(publ.ExcelFile(masterName +"(" + Convert.ToString(dt.Rows[i][1]) +")", SheetName, dt1, FormatSet));
                        }
                    }
                    //SetSQL += "CST.CostCode, CST.Item, CST.ItemDetail, CST.Unit, CST.Cost, ";
                    //SetSQL += "CST.OfficeCode, CST.MemberCode ";
                    //SetSQL += "FROM M_Cost AS CST ";
                    //SetSQL += "LEFT JOIN M_Office AS OFC ";
                    //SetSQL += "ON CST.OfficeCode = OFC.OfficeCode ";
                    //SetSQL += "ORDER BY OFC.OfficeID, CST.CostCode";

                    //SqlHandling sqlh = new SqlHandling();               // SQL実行クラス
                    //// レコードを取得する
                    //DataTable dt = sqlh.SelectFullDescription(SetSQL);
                    //if (dt == null)
                    //{
                    //    textBoxMsg.AppendText("× Excel出力ができませんでした。\r\n");
                    //    return;
                    //}

                    //// フォーマット設定用構造体
                    //PrintOut.Publish.FormatSet[] FormatSet = new PrintOut.Publish.FormatSet[dt.Columns.Count];
                    //// フォーマット設定
                    //FormatSetting(ref FormatSet);

                    //// Excel出力クラス
                    //// Wakamatsu 20170301
                    ////PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate("M_Cost.xlsx"));
                    //PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate(BookName));
                    //// Wakamatsu 20170301
                    //// Excelファイル出力
                    //// Wakamatsu 20170301
                    ////textBoxMsg.AppendText(publ.ExcelFile("M_Cost", dt, FormatSet));
                    //textBoxMsg.AppendText(publ.ExcelFile(masterName, SheetName, dt, FormatSet));
                    //// Wakamatsu 20170301
                    // Wakamatsu 20170303
                    break;
                // Wakamatsu
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        // Wakamatsu
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
                    case 0:             // 原価コード
                    case 6:             // 社員コード
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                    case 4:             // 原価
                        FormatSet[i].SetFormat = "#,0.00";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 3:             // 単位
                    case 5:             // 事業所コード
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

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
    public partial class FormImpMMembers:Form
    {
        //----------------------------------------------------------------------
        //     Field
        //----------------------------------------------------------------------
        // Wakamatsu 20170301
        const string masterName = "社員マスタ";
        const string BookName = masterName + ".xlsx";
        const string SheetName = "M_Members";
        // Wakamatsu 20170301
        private bool initProc = true;       // 初期処理中true,初期処理完了false

        private string fileName;
        ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        //----------------------------------------------------------------------
        //     Contructor
        //----------------------------------------------------------------------
        public FormImpMMembers()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpMMembers_Load(object sender,EventArgs e)
        {
            textBoxMsg.Text = "";
        }



        private void FormImpMMembers_Shown(object sender,EventArgs e)
        {
            initProc = false;
        }


        private void button_Click(object sender,EventArgs e)
        {
            if(initProc) return;

            Button btn = (Button)sender;

            switch(btn.Name)
            {
                case "buttonOpen":
                    // Wakamatsu 20170301
                    //fileName = Files.Open("M_Members.xlsx",Folder.MyDocuments(),"xlsx");
                    fileName = Files.Open(BookName, Folder.MyDocuments(), "xlsx");
                    // Wakamatsu 20170301
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× " + fileName + "は不適切なファイルです。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "の内容で社員マスタを登録・更新します。\r\n");
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
                    int[] procArray = new int[] { 0,0 };
                    switch(System.IO.Path.GetExtension(fileName))
                    {
                        case ".xlsx":
                            // Wakamatsu 20170227
                            try
                            {
                                oWBook = new XLWorkbook(fileName);
                                procArray = mmo.MaintMembersByExcelData(oWBook.Worksheet(1));

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

                    if(procArray[0] < 0) return;
                    textBoxMsg.AppendText("〇 " + fileName + "を処理しました。\r\n");
                    textBoxMsg.AppendText(procArray[0] + "件のデータを登録しました。\r\n");
                    textBoxMsg.AppendText(procArray[1] + "件のデータを更新しました。\r\n");
                    break;
                case "buttonExport":
                    textBoxMsg.AppendText("☆ 処理を開始しました。\r\n");
                    string SetSQL = "";

                    SetSQL += "MemberCode, Name, Phonetic, OfficeCode + Department, ";
                    SetSQL += "BirthDate, PostCode, Address, PostCode2, Address2, TelNo, ";
                    SetSQL += "CellularNo, CellularNo2, EMail, MobileEMail, BloodType, ";
                    SetSQL += "JoinDate, FinalEducation, GradDate, BasicPNo, HealthInsNo, ";
                    SetSQL += "EmploymentInsNo, GainQDate, BankName, BBranchName, AccountType, ";
                    SetSQL += "AccountNo, EContact, RadiationMedical, MedicalCheckup, ";
                    SetSQL += "FormWage, MemberType, AccessLevel, Enrollment, Note ";
                    SetSQL += "FROM M_Members ";
                    SetSQL += "ORDER BY RIGHT('0000' + MemberCode,4)";

                    SqlHandling sqlh = new SqlHandling();               // SQL実行クラス
                    // レコードを取得する
                    DataTable dt = sqlh.SelectFullDescription(SetSQL);
                    if(dt == null)
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
                    //PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate("M_Members.xlsx"));
                    PrintOut.Publish publ = new PrintOut.Publish(Folder.DefaultExcelTemplate(BookName));
                    // Wakamatsu 20170301
                    // Excelファイル出力
                    // Wakamatsu 20170301
                    //textBoxMsg.AppendText(publ.ExcelFile("M_Members",dt,FormatSet));
                    textBoxMsg.AppendText(publ.ExcelFile(masterName, SheetName, dt, FormatSet));
                    // Wakamatsu 20170301
                    break;
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
            for(int i = 0;i < FormatSet.Length;i++)
            {
                switch(i)
                {
                    case 0:             // 社員ID
                        FormatSet[i].SetFormat = "@";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                    case 4:             // 生年月日
                    case 15:            // 雇用年月日
                    case 17:            // 卒業年月
                    case 21:            // 被保険者資格取得年月日
                    case 27:            // 電離放射線健康診断受信日
                    case 28:            // 一般健康診断受信日
                        FormatSet[i].SetFormat = "yyyy-mm-dd";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                    case 6:             // 住所
                    case 7:             // 郵便番号2
                    case 8:             // 住所2
                    case 26:            // 緊急連絡先
                    case 29:            // 賃金形態
                    case 30:            // 社員区分
                    case 31:            // アクセスレベル
                    case 32:            // 在籍区分
                    case 33:            // 摘要
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Left;
                        break;
                    case 12:            // 会社メールアドレス
                    case 13:            // 携帯メールアドレス
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Bottom;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    case 25:            // 口座番号
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = true;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.General;
                        break;
                    default:            // 上記以外
                        FormatSet[i].SetFormat = "";
                        FormatSet[i].IntFlag = false;
                        FormatSet[i].VerticalSet = XLAlignmentVerticalValues.Center;
                        FormatSet[i].HorizontalSet = XLAlignmentHorizontalValues.Center;
                        break;
                }
            }
        }
    }
}

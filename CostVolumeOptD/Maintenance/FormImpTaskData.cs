using ClassLibrary;
using System;
using System.Windows.Forms;

namespace Maintenance
{
    public partial class FormImpTaskData : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        private bool initProc = true;               // 初期処理中true,初期処理完了false
        private string fileName;
        //ClosedXML.Excel.XLWorkbook oWBook = null;   // Excel Workbookオブジェクト
        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormImpTaskData()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpTaskData_Load(object sender, EventArgs e)
        {
            textBoxMsg.Text = "";
        }


        private void FormImpTaskData_Shown(object sender, EventArgs e)
        {
            initProc = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (initProc) return;

            Button btn = (Button)sender;
            MasterMaintOp mmo;

            switch (btn.Name)
            {
                case "buttonOpen":
                    fileName = Files.Open("G.csv", Folder.MyDocuments(), "csv");
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× ファイルが指定せれていません。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "を基に業務データ（D_Task）と業務個別データ（D_TaskInd）を作成します。\r\n");
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
                    mmo = new MasterMaintOp();
                    int[] procCount = new int[2];
                    if (System.IO.Path.GetExtension(fileName) == ".csv" || System.IO.Path.GetExtension(fileName) == ".CSV")
                    {
                        procCount = mmo.MaintTaskDataByCSVData(fileName);
                    }
                    else
                    { 
                        procCount[0] = -1;
                        textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです。\r\n");
                    }

                    if (procCount[0] < 0 || procCount[1] < 0)
                    {
                        textBoxMsg.AppendText("× " + fileName + "の内容登録に失敗しました。\r\n");
                        return;
                    }
                    textBoxMsg.AppendText("〇 " + fileName + "の処理が正常終了しました。\r\n");
                    textBoxMsg.AppendText("〇 " + procCount[0] + "件の業務データを登録しました。\r\n");
                    textBoxMsg.AppendText("〇 " + procCount[1] + "件の業務個別データを登録しました。\r\n");
                    break;
                case "buttonDateSet":
                    if (fileName == null) return;
                    mmo = new MasterMaintOp();
                    int setCount;
                    if (System.IO.Path.GetExtension(fileName) == ".csv" || System.IO.Path.GetExtension(fileName) == ".CSV")
                    {
                        setCount = mmo.MaintTaskDataDateByCSVData(fileName);
                    }
                    else
                    { 
                        setCount = -1;
                        textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです。\r\n");
                    }
                    if (setCount < 0)
                    {
                        textBoxMsg.AppendText("× " + fileName + "の内容登録に失敗しました。\r\n");
                        return;
                    }
                    textBoxMsg.AppendText("〇 " + fileName + "の処理が正常終了しました。\r\n");
                    textBoxMsg.AppendText("〇 " + setCount + "件の業務データの日付を変更しました。\r\n");
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}

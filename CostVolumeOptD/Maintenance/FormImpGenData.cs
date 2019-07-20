using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;

namespace Maintenance
{
    public partial class FormImpGenData : Form
    {
        //---------------------------------------------------------------------
        //      Field
        //---------------------------------------------------------------------
        private bool iniPro = true;               // 初期処理中true,初期処理完了false
        private string fileName;
        //---------------------------------------------------------------------
        //      Constructor
        //---------------------------------------------------------------------
        public FormImpGenData()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------
        //      Property
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //      Method
        //---------------------------------------------------------------------
        private void FormImpGenData_Load(object sender, EventArgs e)
        {
            create_cbOffice();
        }


        private void FormImpGenData_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            Button btn = (Button)sender;
            GenericData gd;

            switch (btn.Name)
            {
                case "buttonOpen":
                    fileName = Files.Open("zan.csv", Folder.MyDocuments(), "csv");
                    if (fileName == null)
                    {
                        textBoxMsg.AppendText("× ファイルが指定せれていません。処理続行不可能です。\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + fileName + "を基に" + comboBoxOffice.Text + "の出来高システム原価データ（D_CostReport）を作成します。" + Environment.NewLine);
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

                    gd = new GenericData();
                    int procCount = 0;
                    if (System.IO.Path.GetExtension(fileName) == ".csv" || System.IO.Path.GetExtension(fileName) == ".CSV")
                    {
                        procCount = gd.CreateCostReportDataByCSVData(fileName, Convert.ToString(comboBoxOffice.SelectedValue));
                    }
                    else
                    {
                        procCount = -1;
                        textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです!" + Environment.NewLine);
                    }

                    if (procCount < 0)
                    {
                        textBoxMsg.AppendText("× " + fileName + "の内容登録に失敗しました!" + Environment.NewLine);
                        return;
                    }
                    textBoxMsg.AppendText("〇 " + fileName + "の処理が正常終了しました。" + Environment.NewLine);
                    textBoxMsg.AppendText("〇 " + procCount + "件のデータが原価データに登録されました。" + Environment.NewLine);
                    textBoxMsg.AppendText(Environment.NewLine);

                    break;
                case "buttonDateSet":
                    if (fileName == null) return;
                    gd = new GenericData();
                    int setCount;
                    if (System.IO.Path.GetExtension(fileName) == ".csv" || System.IO.Path.GetExtension(fileName) == ".CSV")
                    {
                        setCount = gd.CountCostReportDataByCSVData(fileName);
                    }
                    else
                    {
                        setCount = -1;
                        textBoxMsg.AppendText("× " + fileName + "は処理できないファイルです!" + Environment.NewLine);
                    }
                    if (setCount < 0)
                    {
                        textBoxMsg.AppendText("× " + fileName + "には登録できる内容がありませんでした!" + Environment.NewLine);
                        return;
                    }
                    textBoxMsg.AppendText("〇 " + fileName + "の確認処理が正常終了しました。\r\n");
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            ComboBox cbx = (ComboBox)sender;
            if (fileName != "")
                textBoxMsg.AppendText("☆ " + fileName + "を基に" + comboBoxOffice.Text + "の出来高システム原価データ（D_CostReport）を作成します。" + Environment.NewLine);
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }

    }
}

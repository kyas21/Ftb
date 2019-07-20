using ClassLibrary;
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
    public partial class FormExpCostData : Form
    {
        //--------------------------------------------------------------------//
        //      Field
        //--------------------------------------------------------------------//
        private bool iniPro = true;

        HumanProperty hp;

        private DateTimePicker[] dtpDate;
        private DateTime startDate;

        //--------------------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------------------//
        public FormExpCostData()
        {
            InitializeComponent();
        }


        public FormExpCostData(HumanProperty hp)
        {
            InitializeComponent();
            this.hp = hp;
        }
        //--------------------------------------------------------------------//
        //      Property
        //--------------------------------------------------------------------//

        //--------------------------------------------------------------------//
        //      Method
        //--------------------------------------------------------------------//
        private void FormExpCostData_Load(object sender, EventArgs e)
        {
            textBoxMsg.Text = "";
            createArray_Controls();
            create_cbOffice();
            initial_dtpDate();
        }


        private void FormExpCostData_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (iniPro) return;

            DataTable dt;
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "buttonCheck":
                    dt = takeInCostReportData();
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        textBoxMsg.AppendText("× 処理対象となるデータがありません!\r\n");
                    }
                    else
                    {
                        textBoxMsg.AppendText("☆ " + Convert.ToString(dt.Rows.Count)
                            + " 件のデータが処理対象となります。\r\n 商魂取込用「売上明細データ」を作成するためには「開始」ボタンをクリックしてください。\r\n");
                    }
                    break;
                case "buttonOK":
                    //string fileName = Folder.MyDocuments() + @"\作業内訳原価_" + comboBoxOffice.Text + "_" 
                    string fileName = Folder.MyDocuments() + @"\出来高管理原価明細_" + comboBoxOffice.Text + "_" 
                                    + (dtpDate[0].Value).ToString("yyMMdd") + "-" + (dtpDate[1].Value).ToString("yyMMdd") + ".TXT";　
                    dt = takeInCostReportData();
                    GenericData gd = new GenericData(dt);
                    int procCnt = gd.CreateGenricData_CostReport("Shift_JIS", fileName);
                    if (procCnt < 0)
                    {
                        textBoxMsg.AppendText("× 「売上明細データ」の作成に失敗しました。\r\n");
                        return;
                    }
                    else
                    {
                        textBoxMsg.AppendText("〇 " + Convert.ToString(procCnt)
                            + " 件のデータを商魂取込用「売上明細データ」として、\r\nファイル：" + fileName + "\r\nに出力しました。\r\n");
                    }
                    //gd.writeLine();
                    break;
                case "buttonCancel":
                    textBoxMsg.Text = "";
                    initial_dtpDate();
                    createArray_Controls();
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
        }


        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (iniPro) return;

            DateTimePicker dtp = (DateTimePicker)sender;
            switch (dtp.Name)
            {
                case "dateTimePickerDateFR":
                    if (dateTimePickerDateFR.Value < startDate)
                        MessageBox.Show("既に締処理を完了している日付を選択していますがよろしいでしょうか？");
                    break;
                case "dateTimePickerDateTO":
                    if (dateTimePickerDateTO.Value < dateTimePickerDateFR.Value)
                    {
                        MessageBox.Show("右の日付（終了日）は左の日付（開始日）より前の日付は指定できません。");
                        dateTimePickerDateFR.Value = startDate;
                        dateTimePickerDateTO.Value = DateTime.Today; 
                    }

                    break;
                default:
                    break;
            }
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            ComboBox cbb = (ComboBox)sender;
            initial_dtpDate();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            //if (buttonOK.Enabled)
            System.Diagnostics.Process.Start(Folder.MyDocuments());
        }

        //--------------------------------------------------------------------//
        //     SubRoutine
        //--------------------------------------------------------------------//
        // コントロールをプログラミングの効率化を目的として配列化する
        private void createArray_Controls()
        {
            this.dtpDate = new DateTimePicker[] { this.dateTimePickerDateFR, this.dateTimePickerDateTO };
        }


        // ComboBox作成
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }

        private void initial_dtpDate()
        {
            for (int i = 0; i < dtpDate.Length; i++) dtpDate[i].Value = DateTime.Today.StripTime();
            CommonData com = new CommonData();
            startDate = com.SelectCloseDate(Convert.ToString(comboBoxOffice.SelectedValue));
            if (startDate == DateTime.MinValue) return;
            dateTimePickerDateFR.Value = startDate.AddDays(1);

            /*
            DataTable dt = com.SelectCommonData("CLS", Convert.ToString(comboBoxOffice.SelectedValue));
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow dr = dt.Rows[0];
            startDate = Convert.ToDateTime(dr["ComData"]).AddDays(1);
            dateTimePickerDateFR.Value = startDate;
            */

            /*
            SqlHandling sh = new SqlHandling("D_CostReport");
            string sqlStr = "MIN(ReportDate) AS DateFR, MAX(ReportDate) AS DateTO FROM D_CostReport "
                            + "WHERE " + selCondition;
            DataTable dt = sh.SelectFullDescription(sqlStr);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow dr = dt.Rows[0];
                //dateTimePickerDateFR.Value = Convert.ToDateTime(dr["DateFR"]);
                dateTimePickerDateTO.Value = Convert.ToDateTime(dr["DateTO"]);
            }
            else
            {
                for (int i = 0; i < dtpDate.Length; i++) dtpDate[i].Value = System.DateTime.Today;
            }
            */

            if (dateTimePickerDateTO.Value < dateTimePickerDateFR.Value) dateTimePickerDateTO.Value = dateTimePickerDateFR.Value;
        }
        

        private DataTable takeInCostReportData()
        {
            //string sqlStr = " * FROM D_CostReport WHERE " + selCondition;
            string sqlStr = " * FROM D_CostReport WHERE ";

            //sqlStr += " AND (ReportDate BETWEEN '" + dtpDate[0].Value + "' AND '" + dtpDate[1].Value + "')";
            sqlStr += " (ReportDate BETWEEN '" + dtpDate[0].Value + "' AND '" + dtpDate[1].Value + "')";
            sqlStr += " AND (OfficeCode = '" + Convert.ToString(comboBoxOffice.SelectedValue) + "')";

            SqlHandling sh = new SqlHandling("D_CostReport");
            return sh.SelectFullDescription(sqlStr);
        }


        private void createGenericData(DataTable dt)
        {
            GenericData gd = new GenericData();
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                gd.ReportDate = Convert.ToDateTime(dr["ReportDate"]);

            }
        }

        
    }
}

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
using ListForm;
using ListFrom;

namespace TaskProc
{
    public partial class FormTaskChange : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;
        CostData[] cost;
        TaskCodeNameData[] tcad;
        TaskCodeNameData[] tcas;
        //const string selPara = " AND (ItemCode LIKE 'A%' OR (SubCoCode BETWEEN 'F001' AND 'F998')) ORDER BY SubCoCode, ItemCode"; 
        private bool iniPro = true;
        const string NoSelect = "未選択";
        private string[] MesArray = new string[] { "更新が成功しました。", "更新が失敗しました。" };

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormTaskChange()
        {
            InitializeComponent();
        }
        public FormTaskChange(HumanProperty hp)
        {
            InitializeComponent();

            this.hp = hp;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        private void FormTaskChange_Load(object sender, EventArgs e)
        {
            create_cbOffice();
            comboBoxOffice.Text = hp.OfficeCode;        // 初期値
            create_cbDepart();
            comboBoxDepart.Text = hp.Department;        // 初期値
            labelItem.Text = "";
            labelSTaskName.Text = "";
            labelDTaskName.Text = "";
            labelMes.Text = "";

            ListFormDataOp lo = new ListFormDataOp();
            tcad = lo.SelectTaskCodeNameData(hp.OfficeCode, hp.Department, "DESC");
            DateTime dtToday = DateTime.Today;
            cost = setupCostDataArray(dtToday, dtToday, hp.OfficeCode, hp.Department, 0);

        }

        private void FormTaskChange_Shown(object sender, EventArgs e)
        {
            iniPro = false;
        }

        private void button_Click(object sender, EventArgs e)
        {
            labelMes.Text = "";
            if (iniPro) return;

            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "buttonEnd":
                    this.Close();
                    break;
                case "buttonChange":
                    // Wakamatsu 20170322
                    if (this.textBoxItem.Text == "")
                    {
                        MessageBox.Show("項目を設定してください。");
                        return;
                    }
                    // Wakamatsu 20170322
                    if (textBoxDTask.Text == textBoxSTask.Text)
                    {
                        MessageBox.Show("同じ業務番号は選択できません。やり直してください。");
                        return;
                    }
                    mainProc();
                    break;
                default:
                    break;
            }
        }


        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            labelMes.Text = "";
            if (iniPro) return;

            ComboBox cbx = (ComboBox)sender;
            ListFormDataOp lo = new ListFormDataOp();
            // Debug
            dateTimePickerTo.Value = dateTimePickerFr.Value;
            switch (cbx.Name)
            {
                case "comboBoxOffice":
                    create_cbDepart();
                    tcad = lo.SelectTaskCodeNameData(Convert.ToString(comboBoxOffice.SelectedValue),
                                                      Convert.ToString(comboBoxDepart.SelectedValue), "DESC");

                    cost = setupCostDataArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                               Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxDepart.SelectedValue), 0);
                    break;
                case "comboBoxDepart":
                    tcad = lo.SelectTaskCodeNameData(Convert.ToString(comboBoxOffice.SelectedValue),
                                                      Convert.ToString(comboBoxDepart.SelectedValue), "DESC");

                    cost = setupCostDataArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                               Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxDepart.SelectedValue), 0);
                    break;
                default:
                    break;
            }
            // Wakamatsu 20170322
            ControlClear(1);
            // Wakamatsu 20170322
        }


        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            labelMes.Text = "";
            if (iniPro) return;

            DateTimePicker dtp = (DateTimePicker)sender;

            cost = setupCostDataArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                       Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxDepart.SelectedValue), 0);

            // Wakamatsu 20170322
            if (dtp.Name == "dateTimePickerFr")
                ControlClear(1);
            // Wakamatsu 20170322

            if (!String.IsNullOrEmpty(textBoxItem.Text))
                tcas = setupSourceTaskCodeNameArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                                    textBoxItem.Text, Convert.ToString(comboBoxOffice.SelectedValue), 0);
        }


        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            labelMes.Text = "";
            if (iniPro) return;

            TextBox tb = (TextBox)sender;
            ListFormDataOp lo = new ListFormDataOp();
            TaskCodeNameData tcd;

            if (e.KeyCode == Keys.Enter)
            {
                switch (tb.Name)
                {
                    case "textBoxItem":
                        if (!selectCostMaster(textBoxItem.Text))
                        {
                            MessageBox.Show("指定された原価コードのデータはありません");
                            return;
                        }
                        tcas = setupSourceTaskCodeNameArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                                             textBoxItem.Text, Convert.ToString(comboBoxOffice.SelectedValue), 0);
                        break;

                    case "textBoxSTask":
                        tcd = lo.SelectTaskCodeNameData(textBoxSTask.Text, Convert.ToString(comboBoxOffice.SelectedValue));
                        if (tcd != null) labelSTaskName.Text = tcd.TaskName;
                        break;

                    case "textBoxDTask":
                        tcd = lo.SelectTaskCodeNameData(textBoxDTask.Text, Convert.ToString(comboBoxOffice.SelectedValue));
                        if (tcd != null) labelDTaskName.Text = tcd.TaskName;
                        break;

                    default:
                        break;
                }
            }

            if ((e.Modifiers & Keys.Control) != Keys.Control) return;   // Ctrlキーが押下された時のみ以下処理

            if (e.KeyCode == Keys.A)
            {
                switch (tb.Name)
                {
                    case "textBoxItem":
                        chooseCostData();
                        if (String.IsNullOrEmpty(textBoxItem.Text))
                        {
                            labelItem.Text = "";
                            return;
                        }
                        tcas = setupSourceTaskCodeNameArray(dateTimePickerFr.Value, dateTimePickerTo.Value,
                                                             textBoxItem.Text, Convert.ToString(comboBoxOffice.SelectedValue), 0);
                        break;

                    case "textBoxSTask":
                        chooseTaskCodeNameData(((tcas == null) ? tcad : tcas), textBoxSTask, labelSTaskName);
                        //if ( tcas == null )
                        //{
                        //    chooseTaskCodeNameData( tcad, textBoxSTask, labelSTaskName );
                        //}
                        //else
                        //{ 
                        //    chooseTaskCodeNameData( tcas, textBoxSTask, labelSTaskName );
                        //}
                        break;

                    case "textBoxDTask":
                        chooseTaskCodeNameData(tcad, textBoxDTask, labelDTaskName);
                        break;

                    default:
                        break;
                }
            }
        }

        // Wakamatsu 20170322
        private void textBoxItem_TextChanged(object sender, EventArgs e)
        {
            if (iniPro) return;
            ControlClear(0);
        }
        // Wakamatsu 20170322

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.TableData("M_Office", "OfficeCode", "OfficeName");
        }

        // 部門
        private void create_cbDepart()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxDepart);
            cbe.DepartmentList((comboBoxOffice.Text == Sign.HQOffice) ? "DEPH" : "DEPB");
        }


        private bool selectCostMaster(string costCode)
        {
            CostData cdp = new CostData();
            cdp = cdp.SelectCostMaster(costCode, Convert.ToString(comboBoxOffice.SelectedValue));
            if (cdp == null) return false;
            dispCostData(cdp);
            return true;
        }


        private CostData[] setupCostDataArray(DateTime dateFr, DateTime dateTo, string officeCode, string department, int val)
        {
            if (val == 0) dateTo = dateFr;
            if (dateTo.StripTime() < dateFr.StripTime()) dateTo = dateFr;

            ListFormDataOp lo = new ListFormDataOp();
            return cost = lo.SelectCostDataFromCostReport(dateFr.StripTime(), dateTo.StripTime(), officeCode, department);
        }

        private void dispCostData(CostData cmds)
        {
            textBoxItem.Text = cmds.CostCode;
            labelItem.Text = cmds.Item;
        }


        private void chooseCostData()
        {
            if (cost == null)
            {
                MessageBox.Show("対象となる原価データはありません");
                return;
            }
            CostData dcost = FormCostListSimple.ReceiveItems(cost);
            if (dcost == null) return;
            dispCostData(dcost);
        }


        private TaskCodeNameData[] setupSourceTaskCodeNameArray(DateTime dateFr, DateTime dateTo, string item, string officeCode, int val)
        {
            if (val == 0) dateTo = dateFr;
            if (dateTo.StripTime() < dateFr.StripTime()) dateTo = dateFr;

            ListFormDataOp lo = new ListFormDataOp();
            return tcas = lo.SelectTaskCodeNameFromCostReport(dateFr.StripTime(), dateTo.StripTime(), item, officeCode);
        }




        private void chooseTaskCodeNameData(TaskCodeNameData[] tca, TextBox tBox, Label tLbl)
        {
            TaskCodeNameData tcnd = FormTaskCodeNameList.ReceiveItems(tca);
            if (tcnd == null) return;
            tBox.Text = tcnd.TaskCode;
            tLbl.Text = tcnd.TaskName;
        }


        private void mainProc()
        {
            dateTimePickerTo.Value = dateTimePickerFr.Value;
            CostReportData[] crdArray;
            CostReportData crd = new CostReportData();

            if (textBoxItem.Text[0] == 'F')
            {
                // 基業務番号（STaskCode）、協力会社原価コード（textBoxItem.Text）などを基に対象となる原価実績（D_CostReport）からデータを得る
                // 得た伝票番号（SlipNo）から、原価実績（D_CostReport）、協力会社作業内訳書明細（D_OsWkDetail）、協力会社作業内訳書（D_OsWkReport）の業務番号を付け替える
                crdArray = crd.SelectCostReport(dateTimePickerFr.Value.StripTime(), dateTimePickerTo.Value.StripTime(), textBoxSTask.Text, textBoxItem.Text,
                                                 Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxDepart.SelectedValue), "SubCoCode");
                // Wakamatsu 20170322
                if (crdArray == null)
                {
                    MessageBox.Show("指定された原価コードまたは業務番号のデータはありません");
                    return;
                }
                // Wakamatsu 20170322
                OsWkDetailData owdd = new OsWkDetailData();
                OsWkReportData owrd = new OsWkReportData();
                int reportID;
                for (int i = 0; i < crdArray.Length; i++)
                {
                    switch (crdArray[i].AccountCode)
                    {
                        case "OSWR":
                            reportID = owdd.SelectOsWkReportID(crdArray[i].SlipNo);
                            if (!crd.UpdateTaskCodeOsWkReport(crdArray[i].SlipNo, reportID, textBoxDTask.Text))
                            {
                                labelMes.Text = MesArray[1];
                                return;
                            }
                            break;

                        case "OSPO":
                            if (!crd.UpdateTaskCode(crdArray[i].SlipNo, textBoxDTask.Text, "D_OsPayOff"))
                            {
                                labelMes.Text = MesArray[1];
                                return;
                            }
                            break;

                        case "OSPM":
                            if (!crd.UpdateTaskCode(crdArray[i].SlipNo, textBoxDTask.Text, "D_OsPayment"))
                            {
                                labelMes.Text = MesArray[1];
                                return;
                            }
                            break;
                    }
                }
            }
            else
            {
                // 基業務番号（STaskCode）、直営(AorB)原価コード（textBoxItem.Text）などを基に対象となる原価実績（D_CostReport）からデータを得る
                // 得た伝票番号（SlipNo）から、原価実績（D_CostReport）、作業内訳書（D_WorkReport）の業務番号を付け替える
                // 原価実績1件に対し、作業内訳書は「作業者」と「作業内容」の組み合わせがあるので複数件のデータがある可能性がある。
                // 原価実績の伝票番号と一致する作業内訳書の業務番号はすべて修正する
                crdArray = crd.SelectCostReport(dateTimePickerFr.Value.StripTime(), dateTimePickerTo.Value.StripTime(), textBoxSTask.Text, textBoxItem.Text,
                                                 Convert.ToString(comboBoxOffice.SelectedValue), Convert.ToString(comboBoxDepart.SelectedValue), "ItemCode");

                // Wakamatsu 20170322
                if (crdArray == null)
                {
                    MessageBox.Show("指定された原価コードまたは業務番号のデータはありません");
                    return;
                }
                // Wakamatsu 20170322

                for (int i = 0; i < crdArray.Length; i++)
                {
                    if (!crd.UpdateTaskCode(crdArray[i].SlipNo, textBoxDTask.Text, "D_WorkReport"))
                    {
                        labelMes.Text = MesArray[1];
                        return;
                    }
                }
            }
            labelMes.Text = MesArray[0];
        }

        // Wakamatsu 20170322
        /// <summary>
        /// テキストボックスクリア
        /// </summary>
        /// <param name="ModeSet">項目テキストボックスクリア設定 1：クリア</param>
        private void ControlClear(int ModeSet)
        {
            if (ModeSet == 1)
                this.textBoxItem.Text = "";             // 項目
            this.labelItem.Text = "";
            this.textBoxSTask.Text = "";            // 元業務番号     
            this.labelSTaskName.Text = "";
            this.textBoxDTask.Text = "";            // 新業務番号
            this.labelDTaskName.Text = "";
        }
        // Wakamatsu 20170322
    }
}

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
    public partial class FormClosingProc : Form
    {
        //--------------------------------------------------------------------------//
        //     Field                                                                //
        //--------------------------------------------------------------------------//
        HumanProperty hp;
        CommonData com;
        private DateTime recentClosingDate;
        private int currentFY;
        private int switchMonth = 7;
        private bool initProc = true;

        //--------------------------------------------------------------------------//
        //     Constructor                                                          //
        //--------------------------------------------------------------------------//
        public FormClosingProc()
        {
            InitializeComponent();
        }

        public FormClosingProc(HumanProperty hp)
        {
            InitializeComponent();
            this.hp = hp;
        }
        //--------------------------------------------------------------------------//
        //     Method                                                               //
        //--------------------------------------------------------------------------//
        private void FormClosingProc_Load(object sender, EventArgs e)
        {
            labelHistory.Text = "";
            labelNow.Text = "";
            labelNowEx.Text = "";
            labelMessage.Text = "";
            labelCaution.Text = "";

            create_cbOffice();
            loadCommonData();
            editClosingLabel();
        }


        private void FormClosingProc_Shown(object sender, EventArgs e)
        {
            initProc = false;
        }


        private void button_Click(object sender, EventArgs e)
        {
            if (initProc) return;

            Button btn = (Button)sender;

            loadCommonData();
            switch (btn.Name)
            {
                case "buttonClose":
                    if (!com.UpdateCLOSEMonth(Convert.ToString(comboBoxOffice.SelectedValue), (recentClosingDate.AddMonths(1)).EndOfMonth())) return;
                    break;
                case "buttonOpen":
                    if (!com.UpdateCLOSEMonth(Convert.ToString(comboBoxOffice.SelectedValue), (recentClosingDate.AddMonths(-1)).EndOfMonth())) return;
                    break;
                case "buttonEnd":
                    this.Close();
                    break;
                default:
                    break;
            }
            loadCommonData();
            editClosingLabel();
        }


        private void comboBoxOffice_TextChanged(object sender, EventArgs e)
        {
            if (initProc) return;

            loadCommonData();
            editClosingLabel();
            if (Convert.ToString(comboBoxOffice.SelectedValue) == "A")
            {
                labelCaution.Text = "全ての締日を本社に合わせます。";
            }
            else
            {
                labelCaution.Text = "";
            }
        }
        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        private void loadCommonData()
        {
            com = new CommonData();
            string extWord = Convert.ToString(comboBoxOffice.SelectedValue);
            if (extWord == "A") extWord = "H";


            recentClosingDate = com.SelectCloseDate(extWord);
            if (recentClosingDate == DateTime.MinValue)
            {
                labelMessage.Text = "締め月データがありません。最新締月日を前月末日に設定します。";
                recentClosingDate = DateTime.Today.AddMonths(-1).EndOfMonth();
                currentFY = DateTime.Today.Year;
                return;
            }
            currentFY = Convert.ToInt32(com.ComSignage);
            switchMonth = Convert.ToInt32(com.ComSymbol);

            /*
            DataTable dt = com.SelectCommonData("CLS", extWord);
            if (dt == null || dt.Rows.Count == 0)
            {
                labelMessage.Text = "締め月データがありません。最新締月日を前月末日に設定します。";
                recentClosingDate = DateTime.Today.AddMonths(-1).EndOfMonth();
                currentFY = DateTime.Today.Year;
                return;
            }
            com = new CommonData(dt.Rows[0]);
            recentClosingDate = Convert.ToDateTime(com.ComData);
            currentFY = Convert.ToInt32(com.ComSignage);
            switchMonth = Convert.ToInt32(com.ComSymbol);
            */
        }


        private void editClosingLabel()
        {
            int closedMonth = recentClosingDate.Month;
            labelHistory.Text = closedMonth.ToString("00") + "月締処理が完了しています。";
           // if (closedMonth == 6) labelHistory.Text += Convert.ToString(currentFY + 1) + "年度締処理が完了しています。";

            DateTime nextCloseDate = (recentClosingDate.AddMonths(1)).EndOfMonth();
            labelNow.Text = nextCloseDate.Month.ToString("00") + "月締処理待ちです。";
            labelNowEx.Text = "";
            if (nextCloseDate.Month == 7)
                labelNowEx.Text = "当月の締処理は、同時に" + Convert.ToString(currentFY) + "年度締処理となります。";
        }


        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit(comboBoxOffice);
            cbe.ValueItem = new string [] { "H","K","S","T","A"};
            cbe.DisplayItem = new string[] { "本社", "郡山", "相双", "関東", "全て" };
            cbe.Basic();
        }

        
    }
}

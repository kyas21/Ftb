using ClassLibrary;
using CostProc;
using EstimPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskProc;

namespace Menu
{
    public partial class FormMenuInfo : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        FormCostInformation formCostInformation = null;
        FormTaskList formTaskList = null;
        FormTaskNoConfList formTaskNoConfList = null;
        FormPlanningNoConfList formPlanningNoConfList = null;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuInfo()
        {
            InitializeComponent();
        }
        public FormMenuInfo(HumanProperty hp)
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
        private void FormMenuInfo_Load(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Name)
            {
                
                case "buttonCostInfo":
                    //内訳書入力状況
                    if (formCostInformation == null || formCostInformation.IsDisposed)
                    {
                        formCostInformation = new FormCostInformation(hp);
                        formCostInformation.Show();
                    }
                    else
                    {
                        MessageBox.Show("すでにこのプログラムは開始されています。");
                    }
                    break;
                case "buttonTaskList":
                    if (formTaskList == null || formTaskList.IsDisposed)
                    {
                        formTaskList = new FormTaskList(hp);
                        formTaskList.Show();
                    }
                    else
                    {
                        MessageBox.Show("すでにこのプログラムは開始されています。");
                    }
                    break;
                case "buttonTaskNoConfList":
                    if (formTaskNoConfList == null || formTaskNoConfList.IsDisposed)
                    {
                        formTaskNoConfList = new FormTaskNoConfList(hp);
                        formTaskNoConfList.Show();
                    }
                    else
                    {
                        MessageBox.Show("すでにこのプログラムは開始されています。");
                    }
                    break;
                case "buttonPlanNoConfList":
                    if( formPlanningNoConfList == null || formPlanningNoConfList.IsDisposed )
                    {
                        formPlanningNoConfList = new FormPlanningNoConfList( hp );
                        formPlanningNoConfList.Show();
                    }
                    else
                    {
                        MessageBox.Show( "すでにこのプログラムは開始されています。" );
                    }
                    break;
                default:
                    break;
            }

        }

    }
}

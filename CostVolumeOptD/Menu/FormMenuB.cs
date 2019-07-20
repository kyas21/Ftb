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
using CostProc;
using ImportReport;

namespace Menu
{
    public partial class FormMenuB : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        FormMenuCostVol formMenuCostVol = null;
        FormMenuEstPlan formMenuEstPlan = null;
        FormMenuOutsource formMenuOutsource = null;
        FormMenuInfo formMenuInfo = null;

        //FormCostInformation formCostInformation = null;
        //FormImpOsWkReports formImpOsWkReport = null;
        //FormOsWkReportSetup formOsWkReportSetup = null;
        //FormOsPayOff formOsPayOff = null;
        //FormOsPayOffSurvey formOsPayOffSurvey = null;
        //FormOsPayment formOsPayment = null;
        const string MsgAlready = "すでにこのプログラムは開始されています。"; 

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuB()
        {
            InitializeComponent();
        }
        public FormMenuB(HumanProperty hp)
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
        private void FormMenuB_Load(object sender, EventArgs e)
        {
            //if (hp.AccessLevel == 0)
            if (hp.AccessLevel >= 9999)
            {
                //buttonImportOsWkReports.Visible = true;
                //buttonImportOsWkReports.Enabled = true;
                //buttonCostInfo.Visible = false;
                //buttonPayOff.Visible = false;
                //buttonPayOffSurvey.Visible = false;
                //buttonPayment.Visible = false;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonVol":
                    if( formMenuCostVol == null || formMenuCostVol.IsDisposed )
                    {
                        formMenuCostVol = new FormMenuCostVol( hp );
                        formMenuCostVol.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPlan":
                    if( formMenuEstPlan == null || formMenuEstPlan.IsDisposed )
                    {
                        formMenuEstPlan = new FormMenuEstPlan( hp );
                        formMenuEstPlan.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonOs":
                    if( formMenuOutsource == null || formMenuOutsource.IsDisposed )
                    {
                        formMenuOutsource = new FormMenuOutsource( hp );
                        formMenuOutsource.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonInfo":
                    if( formMenuInfo == null || formMenuInfo.IsDisposed )
                    {
                        formMenuInfo = new FormMenuInfo( hp );
                        formMenuInfo.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                //case "buttonCostInfo":
                //    //内訳書入力状況
                //    if (formCostInformation == null || formCostInformation.IsDisposed)
                //    {
                //        formCostInformation = new FormCostInformation(hp);
                //        formCostInformation.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;
                //case "buttonImportOsWkReports":
                //    if (formImpOsWkReport == null || formImpOsWkReport.IsDisposed)
                //    {
                //        formImpOsWkReport = new FormImpOsWkReports();
                //        formImpOsWkReport.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;

                //case "buttonOsWkReportSetup":
                //    if (formOsWkReportSetup == null || formOsWkReportSetup.IsDisposed)
                //    {
                //        formOsWkReportSetup = new FormOsWkReportSetup(hp);
                //        formOsWkReportSetup.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;

                //case "buttonPayOff":
                //    //外注精算書
                //    if (formOsPayOff == null || formOsPayOff.IsDisposed)
                //    {
                //        formOsPayOff = new FormOsPayOff(hp);
                //        formOsPayOff.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;
                //case "buttonPayOffSurvey":
                //    //外注精算書
                //    if (formOsPayOffSurvey == null || formOsPayOffSurvey.IsDisposed)
                //    {
                //        formOsPayOffSurvey = new FormOsPayOffSurvey(hp);
                //        formOsPayOffSurvey.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;
                //case "buttonPayment":
                //    //外注出来高調書一覧表
                //    if (formOsPayment == null || formOsPayment.IsDisposed)
                //    {
                //        formOsPayment = new FormOsPayment(hp);
                //        formOsPayment.Show();
                //    }
                //    else
                //    {
                //        MessageBox.Show("すでにこのプログラムは開始されています。");
                //    }
                //    break;
                default:
                    break;
            }

        }





    }
}

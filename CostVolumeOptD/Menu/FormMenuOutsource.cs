using ClassLibrary;
using CostProc;
using ImportReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menu
{
    public partial class FormMenuOutsource :Form
    {

        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        FormCostInformation formCostInformation = null;
        FormImpOsWkReports formImpOsWkReport = null;
        FormOsWkReportSetup formOsWkReportSetup = null;
        FormOsPayOff formOsPayOff = null;
        FormOsPayOffSurvey formOsPayOffSurvey = null;
        FormOsPayment formOsPayment = null;

        const string MsgAlready = "すでにこのプログラムは開始されています。";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuOutsource()
        {
            InitializeComponent();
        }
        public FormMenuOutsource( HumanProperty hp )
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
        private void FormMenuOutsource_Load( object sender, EventArgs e )
        {
            //if (hp.AccessLevel == 0)
            //if( hp.AccessLevel >= 9999 )
            //{
                //buttonImportOsWkReports.Visible = true;
                //buttonImportOsWkReports.Enabled = true;
                //buttonCostInfo.Visible = false;
                //buttonPayOff.Visible = false;
                //buttonPayOffSurvey.Visible = false;
                //buttonPayment.Visible = false;
            //}
            buttonCostInfo.Visible = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            Button btn = ( Button )sender;

            switch( btn.Name )
            {

                case "buttonCostInfo":
                    //内訳書入力状況
                    if( formCostInformation == null || formCostInformation.IsDisposed )
                    {
                        formCostInformation = new FormCostInformation( hp );
                        formCostInformation.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                case "buttonImportOsWkReports":
                    if( formImpOsWkReport == null || formImpOsWkReport.IsDisposed )
                    {
                        formImpOsWkReport = new FormImpOsWkReports();
                        formImpOsWkReport.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                case "buttonOsWkReportSetup":
                    if( formOsWkReportSetup == null || formOsWkReportSetup.IsDisposed )
                    {
                        formOsWkReportSetup = new FormOsWkReportSetup( hp );
                        formOsWkReportSetup.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                case "buttonPayOff":
                    //外注精算書
                    if( formOsPayOff == null || formOsPayOff.IsDisposed )
                    {
                        formOsPayOff = new FormOsPayOff( hp );
                        formOsPayOff.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPayOffSurvey":
                    //外注精算書
                    if( formOsPayOffSurvey == null || formOsPayOffSurvey.IsDisposed )
                    {
                        formOsPayOffSurvey = new FormOsPayOffSurvey( hp );
                        formOsPayOffSurvey.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPayment":
                    //外注出来高調書一覧表
                    if( formOsPayment == null || formOsPayment.IsDisposed )
                    {
                        formOsPayment = new FormOsPayment( hp );
                        formOsPayment.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                default:
                    break;
            }

        }
























    }
}

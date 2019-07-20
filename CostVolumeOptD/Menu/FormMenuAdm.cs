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

namespace Menu
{
    public partial class FormMenuAdm :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        FormMenuCostVol formMenuCostVol = null;
        FormMenuEstPlan formMenuEstPlan = null;
        FormMenuInfo formMenuInfo = null;

        const string MsgAlready = "すでにこのプログラムは開始されています。";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuAdm()
        {
            InitializeComponent();
        }
        public FormMenuAdm( HumanProperty hp )
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
        private void FormMenuAdm_Load( object sender, EventArgs e )
        {
            if( hp.AccessLevel >= 9999 )
            {
                //buttonImportOsWkReports.Visible = true;
                //buttonImportOsWkReports.Enabled = true;
                //buttonCostInfo.Visible = false;
                //buttonPayOff.Visible = false;
                //buttonPayOffSurvey.Visible = false;
                //buttonPayment.Visible = false;
            }
        }

        private void button_Click( object sender, EventArgs e )
        {
            Button btn = ( Button )sender;

            switch( btn.Name )
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
                default:
                    break;
            }

        }
    }
}

using ClassLibrary;
using CostProc;
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
    public partial class FormMenuS :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        //FormMenuCostVol formMenuCostVol = null;
        FormTaskTransfer formTaskTransfer = null;
        FormMenuEstPlan formMenuEstPlan = null;
        //FormMenuInfo formMenuInfo = null;
        FormTaskSummary formTaskSummary = null;

        const string MsgAlready = "すでにこのプログラムは開始されています。";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuS()
        {
            InitializeComponent();
        }
        public FormMenuS( HumanProperty hp )
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
        private void FormMenuS_Load( object sender, EventArgs e )
        {
            
        }

        private void button_Click( object sender, EventArgs e )
        {
            Button btn = ( Button )sender;

            switch( btn.Name )
            {
                case "buttonTaskNote":
                    //受注業務引継書編集・発行
                    if( formTaskTransfer == null || formTaskTransfer.IsDisposed )
                    {
                        formTaskTransfer = new FormTaskTransfer( hp );
                        formTaskTransfer.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonTaskSummary":
                    //業務別元帳（得意先元帳）
                    if( formTaskSummary == null || formTaskSummary.IsDisposed )
                    {
                        formTaskSummary = new FormTaskSummary( hp );
                        formTaskSummary.Show();
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
                default:
                    break;
            }

        }





    }
}

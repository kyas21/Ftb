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
using ImportReport;

namespace Menu
{
    public partial class FormMenuA : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        FormMenuCostVol formMenuCostVol = null;
        FormMenuEstPlan formMenuEstPlan = null;
        FormMenuDataMnt formMenuDataMnt = null;
        FormMenuOutsource formMenuOutsource = null;
        FormMenuInfo formMenuInfo = null;
        FormMenuSYO formMenuSYO = null;

        HumanProperty hp;

        const string MsgAlready = "すでにこのプログラムは開始されています。"; 

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuA()
        {
            InitializeComponent();
        }


        public FormMenuA(HumanProperty hp)
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


        private void FormMenuA_Load(object sender, EventArgs e)
        {
            // ***** 重要 *****
            // 総務部の特定メンバ（2017.03.01時点では、佐伯様、市川様）のみメンテナンスボタンを表示し
            // それ以外は表示しないようにするには下記 if文をコメントアウトを解除する。

            //if (hp.AccessLevel >= 9999)
            //{
            //    buttonMnt.Visible = true;
            //    buttonMnt.Enabled = true;
            //}
            //else
            //{
            //    buttonMnt.Visible = false;
            //    buttonMnt.Enabled = false;
            //}
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            // Login->当Menu の順に処理される時

            switch (btn.Name)
            {
                case "buttonWeb":
                    System.Diagnostics.Process.Start("http://server-ma/CostVolumeOptW/Form/Login");
                    break;

                case "buttonPlan":
                    if (formMenuEstPlan == null || formMenuEstPlan.IsDisposed)
                    {
                        formMenuEstPlan = new FormMenuEstPlan(hp);
                        formMenuEstPlan.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                case "buttonVol":
                    if (formMenuCostVol == null || formMenuCostVol.IsDisposed)
                    {
                        formMenuCostVol = new FormMenuCostVol(hp);
                        formMenuCostVol.Show();
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
                    if (formMenuInfo == null || formMenuInfo.IsDisposed)
                    {
                        formMenuInfo = new FormMenuInfo(hp);
                        formMenuInfo.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                case "buttonSyo":
                    if( formMenuSYO == null || formMenuSYO.IsDisposed )
                    {
                        formMenuSYO = new FormMenuSYO( hp );
                        formMenuSYO.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;

                default:
                    if (formMenuDataMnt == null || formMenuDataMnt.IsDisposed)
                    {
                        formMenuDataMnt = new FormMenuDataMnt(hp);
                        formMenuDataMnt.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
            }

        }

        
    }
}

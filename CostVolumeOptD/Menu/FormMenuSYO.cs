using ClassLibrary;
using Export;
using Maintenance;
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
    public partial class FormMenuSYO :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        FormExpCostData formExpCostData = null;
        FormExportTask formExportTask = null;
        FormExportCostMaster formExportCostMaster = null;
        FormExportKMaster formExportKMaster = null;
        HumanProperty hp;
        private string msgAlready = "すでにこのプログラムは開始されています！";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuSYO()
        {
            InitializeComponent();
        }


        public FormMenuSYO( HumanProperty hp )
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


        private void FormMenuSYO_Load( object sender, EventArgs e )
        {
        }

        private void button_Click( object sender, EventArgs e )
        {
            Button btn = ( Button )sender;

            // Login->当Menu の順に処理される時

            switch( btn.Name )
            {
                case "buttonExportCostData":
                    if( formExpCostData == null || formExpCostData.IsDisposed )
                    {
                        formExpCostData = new FormExpCostData( hp );
                        formExpCostData.Show();
                    }
                    else
                    {
                        MessageBox.Show( msgAlready );
                    }
                    break;

                case "buttonExportTask":
                    if( formExportTask == null || formExportTask.IsDisposed )
                    {
                        formExportTask = new FormExportTask( hp );
                        formExportTask.Show();
                    }
                    else
                    {
                        MessageBox.Show( msgAlready );
                    }
                    break;

                case "buttonExportCostMaster":
                    if( formExportCostMaster == null || formExportCostMaster.IsDisposed )
                    {
                        formExportCostMaster = new FormExportCostMaster( hp );
                        formExportCostMaster.Show();
                    }
                    else
                    {
                        MessageBox.Show( msgAlready );
                    }
                    break;
                case "buttonExportKMaster":
                    if( formExportKMaster == null || formExportKMaster.IsDisposed )
                    {
                        formExportKMaster = new FormExportKMaster( hp );
                        formExportKMaster.Show();
                    }
                    else
                    {
                        MessageBox.Show( msgAlready );
                    }
                    break;
                default:
                    break;
            }

        }

    }
}

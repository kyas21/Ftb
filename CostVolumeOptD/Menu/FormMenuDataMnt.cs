using System;
using System.Windows.Forms;
using ClassLibrary;
using Maintenance;
using CostProc;
using TaskProc;

namespace Menu
{
    public partial class FormMenuDataMnt : Form
    {
        //---------------------------------------------------------------------------//
        //     Field
        //---------------------------------------------------------------------------//
        HumanProperty hp = new HumanProperty();
        FormImpMOffice formImpMOffice = null;
        FormImpMWorkItems formImpMWorkItems = null;
        FormImpMCalendar formImpMCalendar = null;
        FormImpMCost formImpMCost = null;
        FormImpMCommon formImpMCommon = null;
        FormImpMMembers formImpMMembers = null;
        FormImpMPartners formImpMPartners = null;

        FormImpTaskData formImpTaskData = null;
        FormImpGenData formImpGenData = null;
        FormExpCostData formExpCostData = null;
        FormClosingProc formClosingProc = null;
        FormImpAuthority formImpAuthority = null;
        FormImpLedger formImpLedger = null;
        FormTaskChange formTaskChange = null;
        FormMCostMnt formMCostMnt = null;

        private string msgAlready = "すでにこのプログラムは開始されています！";
        
        //---------------------------------------------------------------------------//
        //     Constructor
        //---------------------------------------------------------------------------//
        public FormMenuDataMnt()
        {
            InitializeComponent();
        }


        public FormMenuDataMnt(HumanProperty hp)
        {
            InitializeComponent();
            this.hp = hp;
        }
        //---------------------------------------------------------------------------//
        //      Property
        //---------------------------------------------------------------------------//
        
        //---------------------------------------------------------------------------//
        //      Method
        //---------------------------------------------------------------------------//
        private void FormMenuDataMnt_Load(object sender, EventArgs e)
        {
            if (hp.MemberName == "ROOT") buttonStoreMWorkItems.Visible = true;   
        }


        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonExpCostData":
                    if (formExpCostData == null || formExpCostData.IsDisposed)
                    {
                        formExpCostData = new FormExpCostData(hp);
                        formExpCostData.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMOffice":
                    if (formImpMOffice == null || formImpMOffice.IsDisposed)
                    {
                        formImpMOffice = new FormImpMOffice();
                        formImpMOffice.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMCost":
                    if (formImpMCost == null || formImpMCost.IsDisposed)
                    {
                        formImpMCost = new FormImpMCost();
                        formImpMCost.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;
                    
                case "buttonStoreMWorkItems":
                    if (formImpMWorkItems == null || formImpMWorkItems.IsDisposed)
                    {
                        formImpMWorkItems = new FormImpMWorkItems(hp);
                        formImpMWorkItems.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMCalendar":
                    if (formImpMCalendar == null || formImpMCalendar.IsDisposed)
                    {
                        formImpMCalendar = new FormImpMCalendar();
                        formImpMCalendar.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMCommon":
                    if (formImpMCommon == null || formImpMCommon.IsDisposed)
                    {
                        formImpMCommon = new FormImpMCommon();
                        formImpMCommon.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMMembers":
                    if (formImpMMembers == null || formImpMMembers.IsDisposed)
                    {
                        formImpMMembers = new FormImpMMembers();
                        formImpMMembers.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreMPartners":
                    if (formImpMPartners == null || formImpMPartners.IsDisposed)
                    {
                        formImpMPartners = new FormImpMPartners();
                        formImpMPartners.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStoreTaskData":
                    if (formImpTaskData == null || formImpTaskData.IsDisposed)
                    {
                        formImpTaskData = new FormImpTaskData();
                        formImpTaskData.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonClosing":
                    if (formClosingProc == null || formClosingProc.IsDisposed)
                    {
                        formClosingProc = new FormClosingProc();
                        formClosingProc.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonMntDBBackup":
                    System.Diagnostics.Process.Start(@"c:\CostVolumeSys\Backup\Backup.bat");
                    break;

                case "buttonMntGenData":
                    if (formImpGenData == null || formImpGenData.IsDisposed)
                    {
                        formImpGenData = new FormImpGenData();
                        formImpGenData.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonAuth":
                    if (formImpAuthority == null || formImpAuthority.IsDisposed)
                    {
                        formImpAuthority = new FormImpAuthority();
                        formImpAuthority.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonMntVolume":
                    if (formImpLedger == null || formImpLedger.IsDisposed)
                    {
                        formImpLedger = new FormImpLedger();
                        formImpLedger.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonTaskChange":
                    if (formTaskChange == null || formTaskChange.IsDisposed)
                    {
                        formTaskChange = new FormTaskChange(hp);
                        formTaskChange.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonMCostMnt":
                    if(formMCostMnt == null || formMCostMnt.IsDisposed)
                    {
                        formMCostMnt = new FormMCostMnt();
                        formMCostMnt.Show();
                    }
                    else
                    {
                        MessageBox.Show(msgAlready);
                    }
                    break;

                case "buttonStart":
                    break;

                case "buttonCancel":
                    break;

                default:
                    break;

            }
        }


    }

}

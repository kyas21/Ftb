using System;
using System.Windows.Forms;
using ClassLibrary;
using CostProc;
using TaskProc;
using VolumeProc;

namespace Menu
{
    public partial class FormMenuCostVol : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        HumanProperty hp;

        FormTaskTransfer formTaskTransfer = null;
        FormInputCostData formInputCostData = null;
        FormOsPayOff formOsPayOff = null;
        FormOsPayOffSurvey formOsPayOffSurvey = null;
        FormOsPayment formOsPayment = null;
        FormVolume formVolume = null;
        FormCostDetailCond formCostDetailCond = null;
        FormCostSummaryCond formCostSummaryCond = null;

        FormCostInformation formCostInformation = null;
        FormTaskSummary formTaskSummary = null;

        FormReview formReview = null;

        FormVolumeBook formVolumeBook = null;
        FormContractCost formContractCost = null;
        FormContractCostSummary formContractCostSummary = null;

        const string MsgAlready = "すでにこのプログラムは開始されています。"; 
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMenuCostVol()
        {
            InitializeComponent();
        }


        public FormMenuCostVol(HumanProperty hp)
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
        private void FormMenuCostVol_Load(object sender, EventArgs e)
        {
            if( hp.AccessLevel < 9999 )
            {
                buttonInputCostData.Visible = false;
                buttonContractCost.Visible = false;
                buttonContractCostSummary.Visible = false;
                this.Width = 550;
                this.Height = 310;
            }
        }


        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Name)
            {
                case "buttonTaskNote":
                    //受注業務引継書編集・発行
                    if (formTaskTransfer == null || formTaskTransfer.IsDisposed)
                    {
                        formTaskTransfer = new FormTaskTransfer(hp);
                        formTaskTransfer.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonInputCostData":
                    //原価データ入力
                    if (formInputCostData == null || formInputCostData.IsDisposed)
                    {
                        formInputCostData = new FormInputCostData(hp);
                        formInputCostData.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPayOff":
                    //外注精算書
                    if (formOsPayOff == null || formOsPayOff.IsDisposed)
                    {
                        formOsPayOff = new FormOsPayOff(hp);
                        formOsPayOff.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPayOffSurvey":
                    //外注精算書
                    if (formOsPayOffSurvey == null || formOsPayOffSurvey.IsDisposed)
                    {
                        formOsPayOffSurvey = new FormOsPayOffSurvey(hp);
                        formOsPayOffSurvey.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonPayment":
                    //外注出来高調書一覧表
                    if (formOsPayment == null || formOsPayment.IsDisposed)
                    {
                        formOsPayment = new FormOsPayment(hp);
                        formOsPayment.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonVolume":
                    //出来高台帳
                    if (formVolume == null || formVolume.IsDisposed)
                    {
                        formVolume = new FormVolume(hp);
                        formVolume.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonSummaryTable":
                    
                        MessageBox.Show("対象となるプログラムは削除されました。");
                    break;
                case "buttonReview":
                    //総括表
                    if (formReview == null || formReview.IsDisposed)
                    {
                        formReview = new FormReview(hp);
                        formReview.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonCostDetail":
                    //原価明細表
                    if (formCostDetailCond == null || formCostDetailCond.IsDisposed)
                    {
                        formCostDetailCond = new FormCostDetailCond();
                        formCostDetailCond.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonCostSummary":
                    //原価集計表
                    if (formCostSummaryCond == null || formCostSummaryCond.IsDisposed)
                    {
                        formCostSummaryCond = new FormCostSummaryCond();
                        formCostSummaryCond.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonCostInfo":
                    //内訳書入力状況
                    if (formCostInformation == null || formCostInformation.IsDisposed)
                    {
                        formCostInformation = new FormCostInformation(hp);
                        formCostInformation.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonTaskSummary":
                    //業務別元帳（得意先元帳）
                    if (formTaskSummary == null || formTaskSummary.IsDisposed)
                    {
                        formTaskSummary = new FormTaskSummary(hp);
                        formTaskSummary.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonTaskBook":
                    // 業務台帳集計
                    if( formVolumeBook == null || formVolumeBook.IsDisposed )
                    {
                        formVolumeBook = new FormVolumeBook( hp );
                        formVolumeBook.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonContractCost":
                    // 労働保険資料作成 元請作業従事者一覧
                    if( formContractCost == null || formContractCost.IsDisposed )
                    {
                        formContractCost = new FormContractCost(hp);
                        formContractCost.Show();
                    }
                    else
                    {
                        MessageBox.Show( MsgAlready );
                    }
                    break;
                case "buttonContractCostSummary":
                    // 労働保険資料作成 工事原価総括表
                    if( formContractCostSummary == null || formContractCostSummary.IsDisposed )
                    {
                        formContractCostSummary = new FormContractCostSummary( hp );
                        formContractCostSummary.Show();
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

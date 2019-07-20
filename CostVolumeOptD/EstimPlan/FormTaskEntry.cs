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
using ListForm;

namespace EstimPlan
{
    public partial class FormTaskEntry :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        TaskEntryData ted;
        TaskCodeNameData[] tcd;
        TaskCodeNameData tcds;
        TaskData td;
        TaskIndData tid;
        PartnersScData[] psd;
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormTaskEntry()
        {
            InitializeComponent();
        }

        public FormTaskEntry( TaskEntryData ted )
        {
            InitializeComponent();
            this.ted = ted;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        public static TaskEntryData DispData( TaskEntryData ted )
        {
            FormTaskEntry formTaskEntry = new FormTaskEntry( ted );
            formTaskEntry.ShowDialog();

            formTaskEntry.Dispose();

            return ted;
        }


        private void FormTaskEntry_Load( object sender, EventArgs e )
        {
            labelPublisher.Text = ted.OfficeName + " " + ted.DepartName;
            labelTaskCode.Text = "";

            /***** ComboBox 「原価目標」作成 *****/
            ComboBoxEdit cb = new ComboBoxEdit( comboBoxCostType );
            cb.ComDataList( "COST", ted.OfficeCode );
            comboBoxCostType.Text = "選択してください";         // 初期値

            /***** ComboBox 「事業所」作成 *****/
            //cb = new ComboBoxEdit( comboBoxPartner );
            //cb.TableData( "M_Partners", "PartnerCode", "PartnerName", " WHERE RelCusto = 1 ORDER BY PartnerCode" );
            //comboBoxPartner.Text = "選択してください";          // 初期値

            ListFormDataOp lo = new ListFormDataOp();
            tcd = lo.SelectTaskCodeNameData( ted.OfficeCode, ( ted.OfficeCode == Sign.HQOfficeCode ? ted.Department : "8" ), null );
            psd = lo.SelectPartnersCuData();
            td = new TaskData();
            tid = new TaskIndData();
        }


        private void buttonAdd_Click( object sender, EventArgs e )
        {
            ted.TaskEntryID = 0;

            if( !string.IsNullOrEmpty( textBoxTaskName.Text ) )
            {
                if( string.IsNullOrEmpty( labelTaskCode.Text ) || ted == null )
                {
                    ted.TaskEntryID = 0;
                    ted.TaskCode = "";
                    ted.TaskName = comboBoxCostType.Text + textBoxTaskName.Text;
                    ted.CostType = "";
                    ted.LeaderMCode = "";
                    //ted.PartnerCode = Convert.ToString( comboBoxPartner.SelectedValue );
                    ted.PartnerCode = "";
                    ted.SalesMCode = "";
                    ted.ContractDate = DateTime.MinValue;
                    ted.StartDate = DateTime.MinValue;
                    ted.EndDate = DateTime.MinValue;
                    ted.TaskID = 0;
                    ted.TaskIndID = 0;
                    //ted.PartnerName = comboBoxPartner.Text;
                    ted.PartnerName = textBoxPartner.Text;
                    ted.TaskPlace = string.IsNullOrEmpty( textBoxTaskPlace.Text ) ? "" : textBoxTaskPlace.Text;
                }
                if(string.IsNullOrEmpty(ted.TaskName))
                {
                    MessageBox.Show( "業務名が指定されていません。" );
                    return;
                }
                if(string.IsNullOrEmpty(ted.PartnerName))
                {
                    MessageBox.Show( "取引先名を空白にはできません。" );
                    return;
                }
                int entryID = ted.InsertTaskEntryData( ted );
                if( entryID < 0 ) DMessage.DataExistence();
                ted.TaskEntryID = entryID;
            } 
            this.Close();
        }


        private void buttonCancel_Click( object sender, EventArgs e )
        {
            ted.TaskEntryID = 0;
            ted.TaskCode = "";
            ted.TaskName = "";
            ted.CostType = "";
            ted.LeaderMCode = "";
            ted.PartnerCode = "";
            ted.SalesMCode = "";
            ted.ContractDate = DateTime.MinValue;
            ted.StartDate = DateTime.MinValue;
            ted.EndDate = DateTime.MinValue;
            ted.TaskID = 0;
            ted.TaskIndID = 0;
            ted.PartnerName = "";
            ted.TaskPlace = "";
            this.Close();
        }


        //private void FormTaskEntry_KeyDown( object sender, KeyEventArgs e )
        //{
        //    if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

        //    switch( e.KeyCode )
        //    {
        //        case Keys.A:
        //            chooseTaskName( tcd );
        //            break;
        //        default:
        //            break;
        //    }
        //}


        private void textBoxTaskName_KeyDown( object sender, KeyEventArgs e )
        {
            TextBox tbx = ( TextBox )sender;
            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            switch( e.KeyCode )
            {
                case Keys.A:
                    //chooseTaskName( tcd );
                    chooseTaskName();
                    break;
                default:
                    break;
            }
        }


        private void textBoxPartner_KeyDown( object sender, KeyEventArgs e )
        {
            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if( e.KeyCode == Keys.A )
            {
                choosePartnerData();
            }
        }


        private void textBoxPartner_TextChanged( object sender, EventArgs e )
        {
            TextBox tbx = ( TextBox )sender;
        }

        private void textBoxTaskPlace_TextChanged( object sender, EventArgs e )
        {
            TextBox tbx = ( TextBox )sender;
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//

        // 業務名を業務名一覧画面から得る
        //private void chooseTaskName( TaskCodeNameData[] tcd )
        private void chooseTaskName( )
        {
            comboBoxCostType.Visible = false;
            this.textBoxTaskName.Location = new Point( 101, 61 );

            tcds = FormTaskCodeNameList.ReceiveItems( tcd );

            if( tcds == null ) return;

            ted.TaskEntryID = 0;

            ted.EditTaskEntryData( ted, tcds, 0 );

            textBoxTaskName.Text = ted.TaskName;
            comboBoxPartner.Text = ted.PartnerName;
            textBoxTaskPlace.Text = ted.TaskPlace;
            labelTaskCode.Text = ted.TaskCode;
        }


        private void choosePartnerData()
        {
            PartnersScData psds = FormSubComList.ReceiveItems( psd );
            if( psds == null ) return;
            textBoxPartner.Text = psds.PartnerName;
        }

    }
}

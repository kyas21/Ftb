using ClassLibrary;
using System;
using System.Windows.Forms;

namespace ListForm
{
    public partial class FormMembersList:Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        MembersScData[] msd;
        MembersScData msds;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormMembersList()
        {
            InitializeComponent();
        }

        public FormMembersList(MembersScData[] msd)
        {
            InitializeComponent();
            this.msd = msd;
        }

        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        public static MembersScData ReceiveItems(MembersScData[] msd)
        {
            FormMembersList formMembersList = new FormMembersList(msd);
            formMembersList.ShowDialog();

            if(formMembersList.msds == null) return null;

            formMembersList.Dispose();
            return formMembersList.msds;
        }


        private void FormMembersList_Load(object sender,EventArgs e)
        {
            // DataGridView 設定 
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();

            // Wakamatsu
            //if(msd.GetLength(0) > 1)
            if(msd.Length > 1)
                // データをdataGridViewにセット
                //dataGridView1.Rows.Add(msd.GetLength(0) - 1);
                dataGridView1.Rows.Add(msd.Length - 1);
            // Wakamatsu
            //for(int i = 0;i < msd.GetLength(0);i++)
            for(int i = 0;i < msd.Length;i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = msd[i].MemberCode;
                dataGridView1.Rows[i].Cells[1].Value = msd[i].Name;
            }
        }


        private void dataGridView1_CellMouseClick(object sender,DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            returnValue(dgv);
        }


        private void dataGridView1_KeyDown(object sender,KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if(e.KeyCode == Keys.Enter) returnValue(dgv);
        }


        private void returnValue(DataGridView dgv)
        {
            msds = (MembersScData)msd[dgv.CurrentCellAddress.Y].Clone();

            this.Close();
        }


    }
}

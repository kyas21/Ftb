using ClassLibrary;
using System;
using System.Windows.Forms;

namespace ListForm
{
    public partial class FormTaskCodeNameList : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        TaskCodeNameData[] tcd;
        TaskCodeNameData tcds;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormTaskCodeNameList()
        {
            InitializeComponent();
        }

        public FormTaskCodeNameList(TaskCodeNameData[] tcd)
        {
            InitializeComponent();
            this.tcd = tcd;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        public static TaskCodeNameData ReceiveItems(TaskCodeNameData[] tcd)
        {
            FormTaskCodeNameList formTaskCodeNameList = new FormTaskCodeNameList(tcd);
            formTaskCodeNameList.ShowDialog();

            if (formTaskCodeNameList.tcds == null) return null;

            formTaskCodeNameList.Dispose();
            return formTaskCodeNameList.tcds;
        }


        private void FormTaskCodeNameList_Load(object sender, EventArgs e)
        {
            // DataGridView 設定 
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();

            // データをdataGridViewにセット
            if (tcd == null)
            {
                MessageBox.Show("Dataがありません。");
                return;
            }

            //if (tcd.GetLength(0) > 1) dataGridView1.Rows.Add(tcd.GetLength(0) - 1);
            if (tcd.Length > 1) dataGridView1.Rows.Add(tcd.Length - 1);

            //for (int i = 0; i < tcd.GetLength(0); i++)
            for (int i = 0; i < tcd.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = tcd[i].TaskCode;
                dataGridView1.Rows[i].Cells[1].Value = tcd[i].TaskName;
                dataGridView1.Rows[i].Cells[2].Value = Convert.ToString(tcd[i].TaskID);
                dataGridView1.Rows[i].Cells[3].Value = Convert.ToString(tcd[i].TaskIndID);

            }

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            returnValue(dgv);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.KeyCode == Keys.Enter) returnValue(dgv);
        }

        private void returnValue(DataGridView dgv)
        {
            tcds = (TaskCodeNameData)tcd[dgv.CurrentCellAddress.Y].Clone();
            this.Close();
        }

    }
}

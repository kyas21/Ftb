using System;
using System.Windows.Forms;
using ClassLibrary;

namespace ListForm
{
    public partial class FormItemList : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        
        //private string[] ReturnValue;
        WorkItemsData[] wid;
        WorkItemsData wids;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormItemList()
        {
            InitializeComponent();
        }

        public FormItemList(WorkItemsData[] wid)
        {
            InitializeComponent();
            this.wid = wid;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//



        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        public static WorkItemsData ReceiveItems(WorkItemsData[] wid)
        {
            FormItemList formItemList = new FormItemList(wid);
            formItemList.ShowDialog();

            if (formItemList.wids == null) return null;

            formItemList.Dispose();
            return formItemList.wids;
        }


        private void FormItemList_Load(object sender, EventArgs e)
        {
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();
            
            // 列幅設定、入力不可設定   
            for (int i = 0; i < dataGridView1.ColumnCount; i++) dataGridView1.Columns[i].ReadOnly = true;
            
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 150;
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[5].Width = 80;

            //dataGridView1.Rows.Add(wid.GetLength(0));
            dataGridView1.Rows.Add(wid.Length);
            //for (int i = 0; i < wid.GetLength(0); i++)
            for (int i = 0; i < wid.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = wid[i].ItemCode;
                dataGridView1.Rows[i].Cells[1].Value = wid[i].UItem;
                dataGridView1.Rows[i].Cells[2].Value = wid[i].Item;
                dataGridView1.Rows[i].Cells[3].Value = wid[i].ItemDetail;
                dataGridView1.Rows[i].Cells[4].Value = wid[i].Unit;
                if (wid[i].StdCost == 0)
                {
                    dataGridView1.Rows[i].Cells[5].Value = null;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[5].Value = wid[i].StdCost;
                }
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
            wids = (WorkItemsData)wid[dgv.CurrentCellAddress.Y].Clone();

            this.Close();
        }
        
    }
}

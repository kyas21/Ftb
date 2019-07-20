using System;
using System.Windows.Forms;
using ClassLibrary;

namespace ListForm
{
    public partial class FormCostList : Form
    {

        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//

        //private string[] ReturnValue;
        CostData[] cmd;
        CostData cmds;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormCostList()
        {
            InitializeComponent();
        }

        public FormCostList(CostData[] cmd)
        {
            InitializeComponent();
            this.cmd = cmd;
        }
        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//



        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        //public static string[] ReceiveItems(CostData[] cmd)
        public static CostData ReceiveItems(CostData[] cmd)
        {
            FormCostList formCostList = new FormCostList(cmd);
            formCostList.ShowDialog();

            if (formCostList.cmds == null) return null;

            formCostList.Dispose();
            return formCostList.cmds;
        }


        private void FormCostList_Load(object sender, EventArgs e)
        {
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();

            // 列幅設定、入力不可設定   
            for (int i = 0; i < dataGridView1.ColumnCount; i++) dataGridView1.Columns[i].ReadOnly = true;
            //if (cmd.GetLength(0) > 1) dataGridView1.Rows.Add(cmd.GetLength(0) - 1);
            if (cmd.Length > 1) dataGridView1.Rows.Add(cmd.Length - 1);
            //for (int i = 0; i < cmd.GetLength(0); i++)
            for (int i = 0; i < cmd.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = cmd[i].CostCode;
                dataGridView1.Rows[i].Cells[1].Value = cmd[i].Item.Replace("（支払い）","");
                dataGridView1.Rows[i].Cells[2].Value = cmd[i].ItemDetail;
                dataGridView1.Rows[i].Cells[3].Value = cmd[i].Unit;
                dataGridView1.Rows[i].Cells[4].Value = (cmd[i].Cost == 0) ? "": DHandling.DecimaltoStr(cmd[i].Cost,"0.00");
                dataGridView1.Rows[i].Cells[5].Value = cmd[i].CostID;
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
            //ReturnValue = new string[dgv.ColumnCount];
            //for (int i = 0; i < dgv.ColumnCount; i++) ReturnValue[i] = Convert.ToString(dgv.Rows[dgv.CurrentCellAddress.Y].Cells[i].Value);
            cmds = (CostData)cmd[dgv.CurrentCellAddress.Y].Clone();

            this.Close();
        }
    }
}

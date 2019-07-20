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

namespace ListFrom
{
    public partial class FormCostListSimple : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        ClassLibrary.CostData[] cmd;
        CostData cmds;

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormCostListSimple()
        {
            InitializeComponent();
        }


        public FormCostListSimple(CostData[] cmd)
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
        public static CostData ReceiveItems(CostData[] cmd)
        {
            FormCostListSimple formCostListSimple = new FormCostListSimple(cmd);
            formCostListSimple.ShowDialog();

            if (formCostListSimple.cmds == null) return null;

            formCostListSimple.Dispose();
            return formCostListSimple.cmds;
        }


        private void FormCostListSimple_Load(object sender, EventArgs e)
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
                dataGridView1.Rows[i].Cells[1].Value = cmd[i].Item.Replace("（支払い）", "");
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
            cmds = (CostData)cmd[dgv.CurrentCellAddress.Y].Clone();

            this.Close();
        }


    }
}

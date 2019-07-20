using System;
using System.Windows.Forms;
using ClassLibrary;
using System.Reflection;

namespace ListForm
{
    public partial class FormTaskNameList : Form
    {
        //----------------------------------------------------------------------------//
        /*     Field                                                                  */
        //----------------------------------------------------------------------------//
        TaskCodeNameData[] tcd;
        private string[] ReturnValue;

        //----------------------------------------------------------------------------//
        /*     Constructor                                                            */
        //----------------------------------------------------------------------------//
        public FormTaskNameList()
        {
            InitializeComponent();
        }

        public FormTaskNameList(TaskCodeNameData[] tcd)
        {
            InitializeComponent();
            this.tcd = tcd;
        }
        //----------------------------------------------------------------------------//
        /*     Property                                                               */
        //----------------------------------------------------------------------------//



        //----------------------------------------------------------------------------//
        /*     Method                                                                 */
        //----------------------------------------------------------------------------//
        //public static string[] ReceiveTaskName()
        public static string[] ReceiveTaskName(TaskCodeNameData[] tcd)
        {
            FormTaskNameList formTaskNameList = new FormTaskNameList(tcd);
            formTaskNameList.ShowDialog();

            if (formTaskNameList.ReturnValue == null) return null;

            //string[] receiveText = new string[formTaskNameList.ReturnValue.GetLength(0)];
            string[] receiveText = new string[formTaskNameList.ReturnValue.Length];
            //for (int i = 0; i < formTaskNameList.ReturnValue.GetLength(0); i++) receiveText[i] = formTaskNameList.ReturnValue[i];
            for (int i = 0; i < formTaskNameList.ReturnValue.Length; i++) receiveText[i] = formTaskNameList.ReturnValue[i];

            formTaskNameList.Dispose();
            return receiveText;
        }



        private void FormTaskNameList_Load(object sender, EventArgs e)
        {
            UiHandling uih = new UiHandling(dataGridView1);
            uih.DgvReadyNoRHeader();
            // 罫線設定
            dataGridView1.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
            // 列幅設定、入力不可設定   
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].Width = 600;

            // データをdataGridViewにセット
            if (tcd == null) return;
            int wkcnt = tcd.Length;
            if (tcd.Length > 1)
                dataGridView1.Rows.Add(tcd.Length);

            for (int i = 0; i < tcd.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = tcd[i].TaskName;
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
            PropertyInfo[] infoArray = tcd[dgv.CurrentCellAddress.Y].GetType().GetProperties();
            int i = 0;
            foreach (PropertyInfo info in infoArray) i++;
            ReturnValue = new string[i];

            i = 0;
            foreach (PropertyInfo info in infoArray)
            {
                ReturnValue[i] = Convert.ToString(info.GetValue(tcd[dgv.CurrentCellAddress.Y], null));
                i++;
            }
            this.Close();
        }


        //------------------------------------------------//
        //  Subroutine                                    //
        //------------------------------------------------//
    }
}

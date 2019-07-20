using System;
using System.Windows.Forms;
using ClassLibrary;

namespace ListForm
{
    public partial class FormSubComList : Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        PartnersScData[] psd;
        PartnersScData psds;
        
        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormSubComList()
        {
            InitializeComponent();
        }

        public FormSubComList(PartnersScData[] psd)
        {
            InitializeComponent();
            this.psd = psd;
        }

        //----------------------------------------------------------------------------//
        //     Property                                                               //
        //----------------------------------------------------------------------------//

        //----------------------------------------------------------------------------//
        //     Method                                                                 //
        //----------------------------------------------------------------------------//
        public static PartnersScData ReceiveItems(PartnersScData[] psd)
        {
            FormSubComList formSubComList = new FormSubComList(psd);
            formSubComList.ShowDialog();

            if (formSubComList.psds == null) return null;

            formSubComList.Dispose();
            return formSubComList.psds;
        }


        private void FormSubComList_Load(object sender, EventArgs e)
        {
            // DataGridView 設定 
            UiHandling ui = new UiHandling(dataGridView1);
            ui.DgvReadyNoRHeader();

            // データをdataGridViewにセット
            //dataGridView1.Rows.Add(psd.GetLength(0)-1);
            dataGridView1.Rows.Add(psd.Length-1);
            //for (int i = 0; i < psd.GetLength(0); i++)
            for (int i = 0; i < psd.Length; i++)
            {
                //dataGridView1.Rows[i].Cells[0].Value = psd[i].PartnerCode;
                dataGridView1.Rows[i].Cells[0].Value = psd[i].AccountCode;
                dataGridView1.Rows[i].Cells[1].Value = psd[i].PartnerName;
                dataGridView1.Rows[i].Cells[2].Value = psd[i].PostCode;
                dataGridView1.Rows[i].Cells[3].Value = psd[i].Address;
                dataGridView1.Rows[i].Cells[4].Value = psd[i].TelNo;
                dataGridView1.Rows[i].Cells[5].Value = psd[i].FaxNo;
                dataGridView1.Rows[i].Cells[6].Value = psd[i].PartnerCode;
                dataGridView1.Rows[i].Cells[7].Value = Convert.ToString(psd[i].PartnerID);
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
            psds = (PartnersScData)psd[dgv.CurrentCellAddress.Y].Clone();
            this.Close();
        }

    }
}

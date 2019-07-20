using ClassLibrary;
using ListForm;
using PrintOut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostProc
{
    public partial class FormContractCostSummary :Form
    {
        //-----------------------------------------------//
        //      Field
        //-----------------------------------------------//
        HumanProperty hp;
        TaskCodeNameData[] tcd;
        TaskCodeNameData[] tcdG;
        private bool iniPro = true;
        private decimal[] costArray = new decimal[12];
        //-----------------------------------------------//
        //      Construction
        //-----------------------------------------------//
        public FormContractCostSummary()
        {
            InitializeComponent();
        }

        public FormContractCostSummary( HumanProperty hp )
        {
            this.hp = hp;
            InitializeComponent();
        }

        //-----------------------------------------------//
        //      Property
        //-----------------------------------------------//

        //-----------------------------------------------//
        //      Method
        //-----------------------------------------------//
        private void FormContractCostSummary_Load( object sender, EventArgs e )
        {
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvReadyNoRHeader();
            ui.DgvColumnsAlignmentRight( 3, 29 );
            create_cbFY();
            create_cbOffice();
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );

            getInformationAboutTask();
        }


        private void FormContractCost_Shown( object sender, EventArgs e )
        {
            iniPro = false;
        }


        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            switch( btn.Name )
            {
                case "buttonDisplay":
                    displayCostSummaryData(dataGridView1);
                    break;

                case "buttonClose":
                    this.Close();
                    break;

                case "buttonPrint":
                    PublishData pd = new PublishData();
                    pd.vYear = comboBoxFY.Text;
                    pd.OfficeName = comboBoxOffice.Text;
                    PublishContractWorks pub = new PublishContractWorks( Folder.DefaultExcelTemplate( "労働保険料工事原価総括表.xlsx" ), pd, dataGridView1 );
                    pub.ExcelFile( "ContractSummary", pd, dataGridView1 );
                    break;

                default:
                    break;
            }

        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            ComboBox cbx = ( ComboBox )sender;

            allClear();

            switch( cbx.Name )
            {
                case "comboBoxFY":
                case "comboBoxOffice":
                    // D_TaskInd,D_Taskから必要データ抽出
                    // 年、事業所、元請ステータス（OrdersType = 1）が条件
                    // 必要な情報：業務番号、業務名、発注者コード（発注者名）
                    // 格納先：TaskCodeNameData tcd
                    getInformationAboutTask();
                      
                    break;

                case "comboBoxDepart":
                    break;

                default:
                    break;
            }
            Conv.OfficeAndDepartZ( comboBoxOffice, comboBoxDepart );

        }
        //-----------------------------------------------//
        //      Subroutin
        //-----------------------------------------------//
        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName", hp.AccessLevel );
        }



        private void create_cbFY()
        {
            DateTime dtNow = DateTime.Now;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxFY );
            cbe.ValueItem = new string[3];
            cbe.DisplayItem = new string[3];
            for( int i = 0; i < cbe.ValueItem.Length; i++ )
            {
                cbe.ValueItem[i] = i.ToString();
                cbe.DisplayItem[i] = ( dtNow.FiscalYear() - i ).ToString();
            }
            cbe.Basic();
        }

        /// <summary>
        /// 処理対象となる業務番号、業務名、発注者名を検索しTaskCodeNameList(tcd)に格納する
        /// </summary>
        private void getInformationAboutTask()
        {
            string sqlStr = "dTI.TaskCode, dTI.TaskName, mP.PartnerName FROM D_TaskInd AS dTI LEFT JOIN D_Task AS dT ON dTI.TaskID = dT.TaskID "
                            + "LEFT JOIN M_Partners AS mP ON dT.PartnerCode = mP.PartnerCode "
                            + "WHERE dTI.OrdersType = 1 AND dTI.OfficeCode = '" + Conv.OfficeCode + "' AND LEFT(dTI.TaskCode,1) ";
                            //+ "WHERE dTI.OfficeCode = '" + officeCode + "' AND LEFT(dTI.TaskCode,1) ";
            string addSql = "!= 'G' AND LEFT(dTI.TaskCode, 1) != 'Z' ORDER BY dTI.TaskCode";
            string addSqlG = "= 'G'";

            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( sqlStr + addSql );
            if( dt == null ) return;
            tcd = new TaskCodeNameData[dt.Rows.Count];
            for(int i = 0;i<dt.Rows.Count;i++ ) tcd[i] = new TaskCodeNameData( dt.Rows[i], i );

            dt = sh.SelectFullDescription( sqlStr + addSqlG );
            if( dt == null ) return;
            tcdG = new TaskCodeNameData[dt.Rows.Count];
            for(int i = 0;i<dt.Rows.Count;i++ ) tcdG[i] = new TaskCodeNameData( dt.Rows[i], i );
        }


        private void displayCostSummaryData( DataGridView dgv )
        {
            if( tcd == null && tcdG == null ) return;

            //int rows = 0;
            //if( tcd == null )
            //{
            //    rows = tcdG.Length + 1;
            //}
            //else
            //{
            //    rows = tcd.Length + 1;
            //    if( tcdG != null ) rows += ( tcdG.Length + 1 );
            //}

            //dgv.Rows.Add( rows );

            int lNo = 0;

            if (tcd != null )
            {
                dgv.Rows.Add( tcd.Length + 1 );
                for( int i = 0; i < costArray.Length; i++ ) costArray[i] = 0;
                for( int i = 0; i < tcd.Length; i++ )
                {
                    editDgvRow( dgv.Rows[lNo], tcd[i], null );
                    lNo++;
                }
                editDgvRowSum( dgv.Rows[lNo] );
                lNo++;
            }

            if(tcdG != null )
            {
                // 1行空行
                lNo++;

                dgv.Rows.Add( tcdG.Length + 1 );
                for( int i = 0; i < costArray.Length; i++ ) costArray[i] = 0;
                for( int i = 0; i < tcdG.Length; i++ )
                {
                    editDgvRow( dgv.Rows[lNo], tcdG[i], "G" );
                    lNo++;
                }
                editDgvRowSum( dgv.Rows[lNo] );
            }
        }


        private void editDgvRow( DataGridViewRow dgvRow, TaskCodeNameData tcnd, string itemIni )
        {
            CostReportData crdp = new CostReportData();
            CostData cd = new CostData();
            int[] monthArray = new int[] {  4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };
            int year = Convert.ToInt32( comboBoxFY.Text );
            decimal cost = 0M;
            decimal costSum = 0M;

            dgvRow.Cells["TaskCode"].Value = tcnd.TaskCode;
            dgvRow.Cells["PartnerName"].Value = tcnd.Partner;
            dgvRow.Cells["TaskName"].Value = tcnd.TaskName;
            for( int i = 0; i < monthArray.Length; i++ )
            {
                cost = crdp.SumMonthlyCost( tcnd.TaskCode, Conv.OfficeCode, DateTime.ParseExact( Convert.ToString( year * 10000 + monthArray[i] * 100 + 1 ), "yyyyMMdd", null ) );
                dgvRow.Cells["Cost" + i.ToString( "00" )].Value = decFormat( cost );
                costSum += cost;
                costArray[i] += cost;
            }

            dgvRow.Cells["CostSum"].Value = decFormat( costSum );
            costSum = 0;
        }


        private void editDgvRowSum( DataGridViewRow dgvRow )
        {
            dgvRow.Cells["TaskCode"].Value = "";
            dgvRow.Cells["PartnerName"].Value = "";
            dgvRow.Cells["TaskName"].Value = "合計";
            decimal costSum = 0M;
            for( int i = 0; i < costArray.Length; i++ )
            {
                dgvRow.Cells["Cost" + i.ToString( "00" )].Value = decFormat( costArray[i] );
                costSum += costArray[i];
            }

            dgvRow.Cells["CostSum"].Value = decFormat( costSum );
        }


        private void allClear()
        {
            dataGridView1.Rows.Clear();
        }


        private string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }
    }
}

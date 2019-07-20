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
using PrintOut;
using ListForm;

namespace Accounts
{
    public partial class FormRegular :Form
    {
        //----------------------------------------------------------------------------//
        //     Field                                                                  //
        //----------------------------------------------------------------------------//
        TaskEntryData ted;
        OsResultsData[] ord;
        PartnersScData[] psd;
        private bool iniPro = true;
        private int sumRCnt = 7;
        private int subjCnt = 6;
        private int cpg = 0;            // Current Page Number
        private DateTime recordedDate;
        private Label[] labelSum;
        private DateTime nowDate;
        private string noSubCo = "協力会社の指定がありません。";

        //----------------------------------------------------------------------------//
        //     Constructor                                                            //
        //----------------------------------------------------------------------------//
        public FormRegular()
        {
            InitializeComponent();
        }
        public FormRegular( TaskEntryData ted )
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
        private void FormRegular_Load( object sender, EventArgs e )
        {
            UiHandling uih = new UiHandling( dataGridViewL );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();

            uih = new UiHandling( dataGridViewR );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();

            uih = new UiHandling( dataGridViewT );
            uih.DgvReadyNoRHeader();
            uih.NoSortable();

            labelPublisher.Text = ted.OfficeName + ted.DepartName;    // 部署名

            create_cbOffice();                // 事業所comboBox
            comboBoxOffice.Text = ted.OfficeName;
            create_cbWork();                  // 部門ComboBox
            comboBoxWork.Text = ted.DepartName;

            ListFormDataOp lo = new ListFormDataOp();
            psd = lo.SelectPartnersScData();

            create_dtPicker();

            edit_Labels();

            initialViewSetting(0);

            nowMonth();
        }

        private void FormRegular_Shown( object sender, EventArgs e )
        {
            dataGridViewL.ClearSelection();
            dataGridViewR.ClearSelection();
            dataGridViewT.ClearSelection();
            dataGridViewL.CurrentCell = null;
            dataGridViewR.CurrentCell = null;
            dataGridViewT.CurrentCell = null;
            iniPro = false;
        }


        private void dataGridView_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;

            switch( dgv.Name )
            {
                case "dataGridViewL":
                    dataGridViewR.ClearSelection();
                    dataGridViewT.ClearSelection();
                    dataGridViewR.CurrentCell = null;
                    dataGridViewT.CurrentCell = null;
                    break;
                case "dataGridViewR":
                    dataGridViewL.ClearSelection();
                    dataGridViewT.ClearSelection();
                    dataGridViewL.CurrentCell = null;
                    dataGridViewT.CurrentCell = null;
                    break;
                case "dataGridViewT":
                    dataGridViewL.ClearSelection();
                    dataGridViewR.ClearSelection();
                    dataGridViewL.CurrentCell = null;
                    dataGridViewR.CurrentCell = null;
                    break;
            }
        }


        private void dataGridView_CellValidated( object sender, DataGridViewCellEventArgs e )
        {
            if( iniPro ) return;

            DataGridView dgv = ( DataGridView )sender;
            //if( dgv.Name == "dataGridViewL" || dgv.Name == "dataGridViewR" )
                reCalculateAll( dataGridViewL, dataGridViewR, dataGridViewT );
        }

        private void button_Click( object sender, EventArgs e )
        {
            if( iniPro ) return;

            Button btn = ( Button )sender;
            BillingOp blo = new BillingOp();

            switch( btn.Name )
            {
                case "buttonPrevData":
                    if( cpg != 0 )
                    {
                        cpg--;
                        viewData( cpg );
                    }
                    break;

                case "buttonNextData":
                    if( ord == null ) return;
                    if( cpg != ( ord.Length - 1 ) )
                    {
                        cpg++;
                        viewData( cpg );
                    }
                    break;

                case "buttonCopyAndNext":
                    clearOsResultsContView( dataGridViewL, "L" );
                    clearOsResultsContView( dataGridViewR, "R" );
                    OsResultsData[] newOrd = new OsResultsData[ord.Length + 1];
                    Array.Copy( ord, newOrd, Math.Min( ord.Length, newOrd.Length ) );
                    newOrd[newOrd.Length - 1] = ( OsResultsData )ord[cpg].Clone();
                    newOrd[newOrd.Length - 1].OsResultsID = 0;
                    ord = newOrd;

                    buttonGrpDisabled();
                    buttonPrevData.Enabled = true;
                    buttonNew.Enabled = true;

                    cpg++;
                    break;

                case "buttonReCalc":
                    reCalculateAll( dataGridViewL, dataGridViewR, dataGridViewT );
                    break;

                case "buttonOverWrite":
                    Func<DialogResult> dialogOverWrite = DMessage.DialogOverWrite;
                    if( dialogOverWrite() == DialogResult.No ) return;

                    if( string.IsNullOrEmpty( textBoxSubCo.Text ) )
                    {
                        MessageBox.Show( noSubCo );
                        return;
                    }

                    blo.UpdateOsResults( ord[cpg] );
                    updateOsResultsContData( dataGridViewL, dataGridViewR, ord[cpg] );
                    blo.UpdateOsResultsSum( dataGridViewT, ord[cpg] );
                    labelMsg.Text = "上書しました。";
                    break;

                case "buttonNew":
                    if( string.IsNullOrEmpty( textBoxSubCo.Text ) )
                    {
                        MessageBox.Show( noSubCo );
                        return;
                    }

                    if( ord == null || ord[cpg].OsResultsID < 1 )
                    {
                        if( ord == null )
                        {
                            ord = new OsResultsData[1];
                            cpg = 0;
                            ord[cpg] = new OsResultsData();
                        }

                        storeViewToOsResultsData( ord[cpg] );
                        ord[cpg].OsResultsID = blo.InsertOsResults( ord[cpg] );
                        if( ord[cpg].OsResultsID < 1 )
                        {
                            labelMsg.Text = "新規書込みできませんでした。";
                            return;
                        }
                    }
                    if( !blo.InsertOsResultsCont( dataGridViewL, dataGridViewR, ord[cpg] ) ) return;
                    if( !blo.InsertOsResultsSum( dataGridViewT, ord[cpg] ) ) return;
                    initialViewSetting(cpg);
                    labelMsg.Text = "新規書込みしました。";
                    break;

                case "buttonDelete":
                    Func<DialogResult> dialogDelete = DMessage.DialogDelete;
                    if( dialogDelete() == DialogResult.No ) return;
                    blo.DeleteOsResultsCont( ord[cpg].OsResultsID );
                    blo.DeleteOsResults( ord[cpg].OsResultsID );
                    cpg--;
                    initialViewSetting(cpg);
                    labelMsg.Text = "削除しました。";
                    break;

                case "buttonCancel":
                    Func<DialogResult> dialogCancel = DMessage.DialogCancel;
                    if( dialogCancel() == DialogResult.No ) return;
                    readyDataGridView( dataGridViewL, dataGridViewR, dataGridViewT );
                    break;

                case "buttonPrint":
                    CreateExcelFile( "外注清算書(常傭).xlsx", "OsARegular" );
                    break;

                case "buttonClose":
                    this.Close();
                    break;

                default:
                    break;
            }

        }


        private void comboBox_TextChanged( object sender, EventArgs e )
        {

        }


        private void dateTimePicker_ValueChanged( object sender, EventArgs e )
        {
            if( iniPro ) return;
            if( ord == null ) return;
            DateTimePicker dtp = ( DateTimePicker )sender;
            switch( dtp.Name )
            {
                case "dateTimePickerRecDate":
                    dateTimePickerRecDate.Value = ( dateTimePickerRecDate.Value ).EndOfMonth();
                    recordedDate = ( dateTimePickerRecDate.Value ).EndOfMonth();
                    nowMonth(dateTimePickerRecDate.Value);
                    readyDataGridView(dataGridViewL,dataGridViewR,dataGridViewT);
                    break;
                default:
                    break;
            }
        }


        private void textBox_KeyDown( object sender, KeyEventArgs e )
        {
            TextBox tb = ( TextBox )sender;

            //if( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab )
            //{

            //}

            if( ( e.Modifiers & Keys.Control ) != Keys.Control ) return;   // Ctrlキーが押下された時のみ以下処理

            if( e.KeyCode == Keys.A )
            {
                switch( tb.Name )
                {
                    case "textBoxSubCo":
                        PartnersScData psds = FormSubComList.ReceiveItems( psd );
                        if( psds == null ) return;
                        textBoxSubCo.Text = psds.PartnerName;
                        labelSubCoCode.Text = psds.PartnerCode;
                        break;

                    default:
                        break;
                }
            }
        }
        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        // Label編集
        private void edit_lblPageNo()
        {
            if( ord == null ) return;
            labelPageNo.Text = ( cpg + 1 ).ToString() + " / " + ord.Length.ToString();
        }


        private void edit_Labels()
        {
            labelTaskCode.Text = ted.TaskCode;
            labelTask.Text = ted.TaskName;
            PartnersData pd = new PartnersData();
            labelPartner.Text = pd.SelectPartnerName( ted.PartnerCode );
            MembersData md = new MembersData();
            labelSalesM.Text = md.SelectMemberName( ted.SalesMCode );
            labelLeader.Text = md.SelectMemberName( ted.LeaderMCode );

            recordedDate = DHandling.EndOfMonth( dateTimePickerRecDate.Value );

            labelOrderNo.Text = "";

            labelSubCo.Text = "";
            textBoxSubCo.Text = "";
        }


        private void edit_ResultsLabels( int idx )
        {
            if( ord == null ) return;
            PartnersData pd = new PartnersData();
            labelSubCo.Text = pd.SelectPartnerName( ord[idx].PartnerCode );
            textBoxSubCo.Text = pd.SelectPartnerName( ord[idx].PartnerCode );
            labelOrderNo.Text = ord[idx].OrderNo;
            labelSubCoCode.Text = ord[idx].PartnerCode;
        }


        // comboBox作成
        // 事業所
        private void create_cbOffice()
        {
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxOffice );
            cbe.TableData( "M_Office", "OfficeCode", "OfficeName" );
            comboBoxOffice.SelectedValue = ted.OfficeCode;
        }


        // 部門
        private void create_cbWork()
        {
            if( comboBoxOffice.Text != Sign.HQOffice ) comboBoxWork.Visible = false;
            ComboBoxEdit cbe = new ComboBoxEdit( comboBoxWork );
            cbe.DepartmentList( ( comboBoxOffice.Text == Sign.HQOffice ) ? "DEPH" : "DEPB" );
            comboBoxWork.SelectedValue = ted.Department;
        }


        private void create_dtPicker()
        {
            dateTimePickerRecDate.Value = (DateTime.Today).EndOfMonth();
            nowDate = dateTimePickerRecDate.Value;
        }


        private void nowMonth()
        {
            labelMsg.Text = Convert.ToString( nowDate.Month ) + "月分を処理します";
        }

        private void nowMonth( DateTime ymd )
        {
            labelMsg.Text = Convert.ToString( ymd.Month ) + "月分を処理します";
        }


        private void buttonGrpDisabled()
        {
            buttonOverWrite.Enabled = false;
            buttonPrevData.Enabled = false;
            buttonNextData.Enabled = false;
            buttonCopyAndNext.Enabled = false;
        }


        private void buttonGrpEnabled()
        {
            buttonOverWrite.Enabled = true;
            buttonPrevData.Enabled = (ord.Length > 1) ? true : false;
            buttonNextData.Enabled = (ord.Length > 1) ? true : false;
            buttonCopyAndNext.Enabled = true;
        }


        private void initialProcessing()
        {
            readyDataGridView( dataGridViewL, dataGridViewR, dataGridViewT );
            if( loadExistingOsResultsData() )
            {
                cpg = 0;
                loadOsResultsContData( ord[cpg] );
                loadOsResultsSumData( ord[cpg] );
                reCalculateAll( dataGridViewL, dataGridViewR, dataGridViewT );
                viewOsResultsData( 0 );
                edit_lblPageNo();
                buttonGrpEnabled();
                buttonNew.Enabled = false;
            }
            else
            {
                DMessage.DataNotExistence( "空の状態で表示します。" );
                buttonGrpDisabled();
                buttonNew.Enabled = true;
            }
        }


        private void initialViewSetting(int idx)
        {
            readyDataGridView( dataGridViewL, dataGridViewR, dataGridViewT );
            if( loadExistingOsResultsData() )
            {
                if( idx < 0 ) idx = 0;
                if( idx > ord.Length ) idx = 0;
                loadOsResultsContData( ord[idx] );
                loadOsResultsSumData( ord[idx] );
                reCalculateAll( dataGridViewL, dataGridViewR, dataGridViewT );
                viewOsResultsData( idx );
                edit_lblPageNo();
                buttonGrpEnabled();
                buttonNew.Enabled = false;
            }
            else
            {
                DMessage.DataNotExistence( "空の状態で表示します。" );
                buttonGrpDisabled();
            }
        }

        // Load Existing Data 既存データの読取表示
        private bool loadExistingOsResultsData()
        {
            if( ted.TaskCode == null ) return false;
            // 選択された業務でその部署のデータすべてを処理対象とする。
            BillingOp blo = new BillingOp();
            //string wParam = " WHERE TaskCode = '" + ted.TaskCode + "'"
            //                + " AND OfficeCode = '" + ted.OfficeCode + "' AND Department = '" + ted.Department + "'"
            //                + " AND ContractForm = 1";
            string wParam = " WHERE TaskEntryID = " + ted.TaskEntryID + " AND ContractForm = 1";
            DataTable dt = blo.UsingParamater_Select( "D_OsResults", wParam );
            if( dt == null ) return false;
            if( !( dt.Rows.Count > 0 ) ) return false;
            ord = new OsResultsData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) ord[i] = new OsResultsData( dt.Rows[i] );
            return true;
        }


        private bool loadOsResultsContData( OsResultsData ord )
        {
            if( ord.OsResultsID == 0 ) return false;

            BillingOp blo = new BillingOp();
            string wParam = " WHERE OsResultsID = " + Convert.ToString( ord.OsResultsID )
                            + " AND RecordedDate = '" + ( dateTimePickerRecDate.Value ).EndOfMonth() + "' ORDER BY PublishDate";
            DataTable dt = blo.UsingParamater_Select( "D_OsResultsCont", wParam );
            if( dt == null ) return false;

            int didx;
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                DateTime pdt = DHandling.StripTime( Convert.ToDateTime( dr["PublishDate"] ) );
                didx = Convert.ToInt32( pdt.Day ) - 1;
                if( didx > 15 )
                {
                    viewOsResultsContToDgv( dr, dataGridViewR.Rows[didx - 16], "R" );
                }
                else
                {
                    viewOsResultsContToDgv( dr, dataGridViewL.Rows[didx], "L" );
                }
            }
            return true;
        }


        private bool loadOsResultsSumData( OsResultsData ord )
        {
            if( ord.OsResultsID == 0 ) return false;

            BillingOp blo = new BillingOp();
            string wParam = " WHERE OsResultsID = " + Convert.ToString( ord.OsResultsID )
                + " AND RecordedDate = '" + ( dateTimePickerRecDate.Value ).EndOfMonth() + "' ORDER BY Subject";
            DataTable dt = blo.UsingParamater_Select( "D_OsResultsSum", wParam );
            if( dt == null ) return false;
            viewOsResultsSumToDgv( dt, dataGridViewT );
            return true;
        }


        // Store Data
        private void storeViewToOsResultsData( OsResultsData ord )
        {
            ord.TaskCode = string.IsNullOrEmpty(ted.TaskCode) ? "":ted.TaskCode;
            ord.VersionNo = 1;
            ord.OrderNo = labelOrderNo.Text;
            ord.PartnerCode = labelSubCoCode.Text;
            ord.PayRoule = -1;
            ord.Amount = 0;
            ord.PublishDate = DateTime.Today.StripTime();
            ord.StartDate = Convert.ToDateTime( dateTimePickerRecDate.Value );
            ord.EndDate = Convert.ToDateTime( dateTimePickerRecDate.Value );
            ord.InspectDate = Convert.ToDateTime( dateTimePickerRecDate.Value );
            ord.ReceiptDate = Convert.ToDateTime( dateTimePickerRecDate.Value );
            ord.Place = "";
            ord.Note = "";
            ord.OfficeCode = Convert.ToString( comboBoxOffice.SelectedValue );
            ord.Department = Convert.ToString( comboBoxWork.SelectedValue );
            ord.Publisher = ord.OfficeCode + ord.Department;
            ord.ContractForm = 1;
            ord.RecordedDate = ( dateTimePickerRecDate.Value ).EndOfMonth();
            ord.TaskEntryID = ted.TaskEntryID;
        }


        // View  Data
        private void viewOsResultsContToDgv( DataRow dr, DataGridViewRow dgvRow, string pos )
        {
            DateTime pdt = ( Convert.ToDateTime( dr["PublishDate"] ) ).StripTime();
            dgvRow.Cells["ItemCode" + pos].Value = Convert.ToString( dr["ItemCode"] );
            dgvRow.Cells["Item" + pos].Value = Convert.ToString( dr["Item"] );
            int subNo = Convert.ToInt32( dr["Subject"] );

            dgvRow.Cells["Quantity" + pos + subNo.ToString()].Value = ( Convert.ToDecimal( dr["Quantity"] ) == 0 ) ? ""
                : DHandling.DecimaltoStr( Convert.ToDecimal( dr["Quantity"] ), "0.00" );

            dgvRow.Cells["PublishDate" + pos].Value = Convert.ToString( Convert.ToDateTime( dr["PublishDate"] ) );
        }


        private void viewOsResultsSumToDgv( DataTable dt, DataGridView dgv )
        {
            DataRow dr;
            int rno;
            decimal qty;
            decimal cost;
            decimal total = 0;
            for( int i = 0; i < sumRCnt - 1; i++ )
            {
                dr = dt.Rows[i];
                rno = Convert.ToInt32( dr["Subject"] );
                dgv.Rows[rno].Cells["ItemDetail"].Value = Convert.ToString( dr["ItemDetail"] );
                qty = Convert.ToDecimal( dr["Quantity"] );
                dgv.Rows[rno].Cells["Quantity"].Value = DHandling.DecimaltoStr( qty, "0.00" );
                cost = Convert.ToDecimal( dr["Cost"] );
                dgv.Rows[rno].Cells["Cost"].Value = DHandling.DecimaltoStr( cost, "0.00" );
                dgv.Rows[rno].Cells["Amount"].Value = DHandling.DecimaltoStr( qty * cost, "#,00" );
                dgv.Rows[rno].Cells["Note"].Value = Convert.ToString( dr["Note"] );
                total += qty * cost;
            }
            dgv.Rows[sumRCnt - 1].Cells["Amount"].Value = DHandling.DecimaltoStr( total, "#,00" );
        }


        private void viewData( int idx )
        {
            viewOsResultsData( idx );
            viewOsResultsContData( idx );
            buttonOverWrite.Enabled = true;
        }


        private void viewOsResultsData( int idx )
        {
            edit_lblPageNo();
            edit_ResultsLabels( idx );
        }


        private void viewOsResultsContData( int idx )
        {
            readyDataGridView( dataGridViewL, dataGridViewR, dataGridViewT );
            loadOsResultsContData( ord[idx] );
            loadOsResultsSumData( ord[idx] );
            reCalculateAll( dataGridViewL, dataGridViewR, dataGridViewT );
        }


        // DataGridViewの全体計算（横計算&縦計算）
        private bool reCalculateAll( DataGridView dgvL, DataGridView dgvR, DataGridView dgvT )
        {
            if( !checkInputContData( dgvL ) ) return false;

            decimal[] qty = new decimal[subjCnt];

            for( int i = 0; i < dgvL.RowCount; i++ )
            {
                for( int j = 0; j < subjCnt; j++ )
                {
                    if( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value != null )
                    {
                        qty[j] += DHandling.ToRegDecimal( Convert.ToString( dgvL.Rows[i].Cells["QuantityL" + j.ToString()].Value ) );
                    }

                }
            }

            for( int i = 0; i < dgvR.RowCount; i++ )
            {
                for( int j = 0; j < subjCnt; j++ )
                {
                    if( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value != null )
                    {
                        qty[j] += DHandling.ToRegDecimal( Convert.ToString( dgvR.Rows[i].Cells["QuantityR" + j.ToString()].Value ) );
                    }

                }
            }

            this.labelSum = new Label[] { this.labelSumQty0, this.labelSumQty1, this.labelSumQty2, this.labelSumQty3, this.labelSumQty4, this.labelSumQty5 };
            for( int j = 0; j < subjCnt; j++ )
            {
                labelSum[j].Text = DHandling.DecimaltoStr( qty[j], "0.00" );
                dgvT.Rows[j].Cells["Quantity"].Value = DHandling.DecimaltoStr( qty[j], "0.00" );
            }

            if( !checkInputSumData( dgvT ) ) return false;

            decimal cost;
            for( int i = 0; i < subjCnt; i++ )
            {
                if( dgvT.Rows[i].Cells["Cost"].Value != null )
                {
                    cost = DHandling.ToRegDecimal( Convert.ToString( dgvT.Rows[i].Cells["Cost"].Value ) );
                    dgvT.Rows[i].Cells["Amount"].Value = DHandling.DecimaltoStr( cost * qty[i], "0.00" );

                }
            }
            return true;
        }


        private void clearOsResultsContView( DataGridView dgv,string pos )
        {
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                for( int j = 0; j < subjCnt; j++ )
                    dgv.Rows[i].Cells["Quantity" + pos + j.ToString()].Value = null;
            }
        }


        private void readyDataGridView( DataGridView dgvL, DataGridView dgvR, DataGridView dgvT )
        {
            DateTime setdt = Convert.ToDateTime( dateTimePickerRecDate.Value );
            int mmDays = Convert.ToInt32( ( setdt.EndOfMonth() ).Day );
            int lRows = 16;
            int rRows = mmDays - lRows;

            // 指定月に合わせた行数の調整
            //dgvL.Rows.Clear();
            if( dgvL.Rows.Count != lRows )
            {
                dgvL.Rows.Clear();
                dgvL.Rows.Add( lRows );
            }

            //dgvR.Rows.Clear();
            if( dgvR.RowCount < 2 )
            {
                dgvR.Rows.Clear();
                dgvR.Rows.Add( rRows );
            }
            else if( dgvR.Rows.Count < rRows )
            {
                dgvR.Rows.Add( rRows - dgvR.Rows.Count);
            }
            else if (dgvR.Rows.Count > rRows )
            {
                int nowCount = dgvR.Rows.Count;
                int delCount = nowCount - rRows;
                for(int i = 0;i < delCount;i++ ) dgvR.Rows.RemoveAt( dgvR.Rows.Count - 1);
            }

            dgvL.Rows[0].Cells["MonthL"].Value = Convert.ToString( setdt.Month );
            for( int i = 0; i < mmDays; i++ )
            {
                if( i < lRows )
                {
                    dgvL.Rows[i].Cells["DayL"].Value = ( i + 1 ).ToString();
                }
                else
                {
                    dgvR.Rows[i - lRows].Cells["DayR"].Value = ( i + 1 ).ToString();
                }
            }

            dgvT.Rows.Clear();
            dgvT.Rows.Add( sumRCnt );
            string[] rTitle = new string[] { "技師", "技師補", "作業員", "機器車両損料", "残業", "その他", "合計" };
            string[] unitAry = new string[] { "工", "工", "工", "日", "h", "", "" };
            for( int i = 0; i < dgvT.RowCount; i++ )
            {
                dgvT.Rows[i].Cells["Subject"].Value = rTitle[i];
                dgvT.Rows[i].Cells["Unit"].Value = unitAry[i];
            }
        }


        private bool checkInputContData( DataGridView dgv )
        {
            if( iniPro ) return true;
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                for( int j = 4; j < dgv.ColumnCount - 1; j++ )
                {
                    if( !checkDecimalValue( Convert.ToString( dgv.Rows[i].Cells[j].Value ) ) )
                    {
                        dgv.CurrentCell = dgv[j, i];
                        return false;
                    }
                }
            }
            return true;
        }


        private bool checkInputSumData( DataGridView dgv )
        {
            for( int i = 0; i < dgv.RowCount; i++ )
            {
                for( int j = 2; j < dgv.ColumnCount - 1; j++ )
                {
                    if( j != 3 )
                    {
                        if( !checkDecimalValue( Convert.ToString( dgv.Rows[i].Cells[j].Value ) ) )
                        {
                            dgv.CurrentCell = dgv[j, i];
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        private bool checkDecimalValue( string checkStr )
        {
            if( !String.IsNullOrEmpty( checkStr ) )
            {
                if( !DHandling.IsDecimal( checkStr ) )
                {
                    DMessage.ValueErrMsg();
                    return false;
                }
            }
            return true;
        }


        private void dataGridViewRowsRemove( DataGridView dgv )
        {
            foreach( DataGridViewRow r in dgv.SelectedRows )
            {
                if( !r.IsNewRow )
                    dgv.Rows.Remove( r );
            }
        }


        public void CreateExcelFile( string templateFileName, string sheetName )
        {

            Publish publ = new Publish( Folder.DefaultExcelTemplate( templateFileName ) );
            PublishData pd = new PublishData();

            pd.TaskCode = string.IsNullOrEmpty( ted.TaskCode ) ? "":ted.TaskCode ;
            pd.TaskName = ted.TaskName;
            pd.CostType = ted.CostType;
            pd.TaskPlace = ted.TaskPlace;
            pd.StartDate = ted.StartDate;
            pd.EndDate = ted.EndDate;
            pd.TaxRate = ted.TaxRate;
            pd.PartnerName = ted.PartnerName;
            pd.LeaderName = labelLeader.Text;
            pd.SalesMName = labelSalesM.Text;
            pd.OrderPartner = labelSubCo.Text;
            pd.RecordedDate = dateTimePickerRecDate.Value;

            publ.ExcelFile( sheetName, pd, dataGridViewT, dataGridViewL, dataGridViewR );
            return;
        }


        private void updateOsResultsContData(DataGridView dgvL,DataGridView dgvR,OsResultsData ord )
        {
            BillingOp blo = new BillingOp();
            if(blo.DeleteOsResultsCont( ord.OsResultsID ))
                blo.InsertOsResultsCont( dataGridViewL, dataGridViewR, ord );
        }


        private void DataGridView_RowPrePaint( object sender,DataGridViewRowPrePaintEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            //フォーカス枠を描画しない
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }





    }
}

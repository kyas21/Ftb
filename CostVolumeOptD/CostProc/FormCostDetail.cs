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

namespace CostProc
{
    public partial class FormCostDetail : Form
    {
        //-----------------------------------------------//
        //      Field
        //-----------------------------------------------//
        CostInfoIFOp cif;
        //string[] hTextArray1 = new string[] { "（業務番号）業務", "（コード）原価項目" };
        string[] hTextArray1 = new string[] { Sign.lblTask, Sign.lblItem };
        //string[] hTextArray2 = new string[] { "（コード）原価項目", "（業務番号）業務" };
        string[] hTextArray2 = new string[] { Sign.lblItem, Sign.lblTask };
        string[] nTextArray1 = new string[] { "Task", "Item" };
        string[] nTextArray2 = new string[] { "Item", "Task" };
        //string[] hTextItemName = new string[] { "【業務番号 計】", "【原価項目 計】", "【期間合計】" };
        string[] hTextItemName = new string[] { Sign.lblsumTask,Sign.lblsumItem,Sign.lblsumTerm };

        const int NEW = 0;
        const int OLD = 1;

        const string bookName = "原価明細表.xlsx";
        const string sheetName = "CostDetail";


        //-----------------------------------------------//
        //      Construction
        //-----------------------------------------------//
        public FormCostDetail()
        {
            InitializeComponent();
        }

        public FormCostDetail( CostInfoIFOp cif )
        {
            this.cif = cif;
            InitializeComponent();
        }

        //-----------------------------------------------//
        //      Property
        //-----------------------------------------------//

        //-----------------------------------------------//
        //      Method
        //-----------------------------------------------//
        private void FormCostDetail_Load( object sender, EventArgs e )
        {
            // DataGridViewの列タイトル設定
            UiHandling ui = new UiHandling( dataGridView1 );
            ui.DgvColumnName( 2, cif.EditColumnNameArray( cif.Class0, cif.Class1 ) );
            ui.DgvColumnHeader( 2, cif.EditColumnHeaderArray( cif.Class0, cif.Class1 ) );

            //原価計上日
            labelFrom.Text = cif.DateSOP.ToString( "yyyy/MM/dd" );
            labelTo.Text = cif.DateEOP.ToString( "yyyy/MM/dd" );

            //明細表
            labelType.Text = cif.ClassificationItem;

            //出力範囲
            labelRange.Text = cif.OutputRange;

            labelOffice.Text = cif.Office;

            // データ読み込み
            string strSql = " D_CR.ReportDate AS ReportDate, D_CR.SlipNo AS SlipNo, D_CR.TaskCode AS TaskCode, SUBSTRING(D_CR.TaskCode, 2, 6) AS TaskBaseCode, D_CR.ItemCode AS ItemCode, D_CR.Quantity AS Quantity,"
                 + " D_CR.Unit AS Unit, D_CR.UnitPrice AS UnitPrice, D_CR.Cost AS Cost ,D_T.PartnerCode AS CustoCode, "
                 + " CASE WHEN LEN(ISNULL(D_CR.LeaderMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.LeaderMCode, '')), 3) ELSE ISNULL(D_CR.LeaderMCode, '') END AS LeaderMCode, "
                 + " CASE WHEN LEN(ISNULL(D_CR.SalesMCode, '')) < 3 THEN RIGHT('00' + RTRIM(ISNULL(D_CR.SalesMCode, '')), 3) ELSE ISNULL(D_CR.SalesMCode, '') END AS SalesMCode, "
                 //+ " M_C.CostCode AS CostCode, M_C.Item AS Item FROM D_CostReport AS D_CR "
                 + " M_C.CostCode AS CostCode, D_CR.Item AS Item FROM D_CostReport AS D_CR "
                 + " LEFT JOIN M_Cost AS M_C ON D_CR.ItemCode = M_C.CostCode AND D_CR.OfficeCode = M_C.OfficeCode "
                 + " LEFT JOIN D_Task AS D_T ON D_T.TaskBaseCode = SUBSTRING(D_CR.TaskCode, 2, 6) ";
            strSql = strSql + cif.WherePhraseDate;
            displayDetailInformation( dataGridView1, strSql );
        }


        private void buttonPrint_Click( object sender, EventArgs e )
        {
            // print 処理
            // Wakamatsu 20170301
            //PublishVolume publ = new PublishVolume(Folder.DefaultExcelTemplate("CostDetail.xlsx"));
            PublishVolume publ = new PublishVolume( Folder.DefaultExcelTemplate( bookName ) );
            // Wakamatsu 20170301
            PublishData pd = new PublishData();
            pd.CostOffice = labelOffice.Text;
            pd.CostReportDate = labelFrom.Text + "～" + labelTo.Text;
            pd.CostTypeData = labelType.Text.ToString();
            pd.CostRange = labelRange.Text.ToString();
            publ.ExcelFile( sheetName, pd, dataGridView1 );

        }


        private void buttonCancel_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        //--------------------------------------------------------------------------//
        //     SubRoutine                                                           //
        //--------------------------------------------------------------------------//
        // データ読み込み
        private bool displayDetailInformation( DataGridView dgv, string strSql )
        {
            DbAccess dba = new DbAccess();
            DataTable dt = dba.UsingSqlstring_Select( strSql );

            if ( dt == null || dt.Rows.Count < 1 )
                return false; // エラー処理

            string[] valArray = new string[2];
            dgv.Rows.Add( dt.Rows.Count );
            DataRow dr;

            //DBから取得したデータを保持しておくための変数
            string[] date = new string[] { "", "", };
            string[] slipNo = new string[] { "", "", };
            string[] task = new string[] { "", "", };
            string[] item = new string[] { "", "", };
            string[] quantity = new string[] { "", "", };
            string[] unit = new string[] { "", "", };
            string[] unitPrice = new string[] { "", "", };
            string[] cost = new string[] { "", "", };
            string[] customer = new string[] { "", "", };
            string[] leaderMName = new string[] { "", "", };
            string[] salesMName = new string[] { "", "", };
            string[] costCode = new string[] { "", "", };


            string editDate = "";

            //金額
            decimal totalSectionCost = 0;   //totalCostよりさらに小さい区分け
            decimal totalCost = 0;          //【作業項目 計】【業務番号 計】に表示する金額値
            decimal periodTotalCost = 0;    //【期間合計】に表示する金額値
            decimal costCalc = 0;           //金額

            //数量
            decimal totalSectionQuantity = 0;
            decimal totalQuantity = 0;
            decimal periodTotalQuantity = 0;
            decimal quantityCalc = 0;

            int intGridCnt = 0;             //グリッドの行数

            string itemName;
            string cellName;

            //【業務番号 計】又は【作業項目 計】を表示する為の項目を設定 
            if ( cif.Class1 == "2" )
            {
                itemName = hTextItemName[0];//【業務番号 計】
                cellName = nTextArray1[0];  //Task
            }
            else
            {
                itemName = hTextItemName[1];//【作業項目 計】
                cellName = nTextArray1[1];  //Item
            }

            //DBから取得したデータをグリッドへセット
            for ( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                //データを一時退避
                date[NEW] = ( Convert.ToDateTime( dr["ReportDate"] ) ).ToShortDateString();
                slipNo[NEW] = Convert.ToString( dr["SlipNo"] );
                valArray = editTaskInfo( Convert.ToString( dr["TaskCode"] ) );
                task[NEW] = valArray[0];
                //item[NEW] = Convert.ToString(dr["Item"]);
                item[NEW] = ( Convert.ToString( dr["ItemCode"] ) )[0] == 'K' ? "●" + Convert.ToString( dr["Item"] ) : Convert.ToString( dr["Item"] );
                quantity[NEW] = decPointFormat( Convert.ToDecimal( dr["Quantity"] ) );
                unit[NEW] = Convert.ToString( dr["Unit"] );
                unitPrice[NEW] = decFormat( Convert.ToDecimal( dr["UnitPrice"] ) );
                costCode[NEW] = Convert.ToString( dr["CostCode"] );

                if ( Convert.ToDecimal( dr["Cost"] ) == 0 )
                {
                    if ( Convert.ToDecimal( dr["UnitPrice"] ) == 0 )
                    {
                        unitPrice[NEW] = "";
                        cost[NEW] = "";
                    }
                    else
                    {
                        cost[NEW] = decFormat( Convert.ToDecimal( dr["Quantity"] ) * Convert.ToDecimal( dr["UnitPrice"] ) );
                    }
                }
                else
                {
                    cost[NEW] = decFormat( Convert.ToDecimal( dr["Cost"] ) );
                }
                customer[NEW] = valArray[1];
                leaderMName[NEW] = editMemberName( Convert.ToString( dr["LeaderMCode"] ) );
                salesMName[NEW] = editMemberName( Convert.ToString( dr["SalesMCode"] ) );

                if ( cost[NEW].ToString().Trim() == "" )
                {
                    costCalc = 0;
                }
                else
                {
                    costCalc = Convert.ToDecimal( cost[NEW] );
                }

                if ( quantity[NEW].ToString().Trim() == "" )
                {
                    quantityCalc = 0;
                }
                else
                {
                    quantityCalc = Convert.ToDecimal( quantity[NEW] );
                }


                if ( i > 0 )//一回目かの判断（一回目の場合は前回との比較を行わない）
                {
                    editDate = date[NEW];

                    if ( date[OLD] == date[NEW] )//計上日の比較（前回と同じ日付の場合表示させない）
                        editDate = "";
                    if ( cif.Class0 == "0" && cif.Class1 == "0" )//"指定なし", "指定なし"
                    {
                    }
                    else if ( ( cif.Class0 == "0" && cif.Class1 == "2" ) || //"指定なし", "業務番号""
                        ( cif.Class0 == "1" && cif.Class1 == "2" ) )        //"原価計上日", "業務番号"
                    {
                        if ( task[OLD] != task[NEW] )
                        {
                            decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );
                            totalCost = 0;
                            totalQuantity = 0;
                        }
                    }
                    else if ( ( cif.Class0 == "0" && cif.Class1 == "4" ) || //"指定なし", "作業項目"
                        ( cif.Class0 == "1" && cif.Class1 == "4" ) )        //"原価計上日", "作業項目"
                    {
                        if ( item[OLD] != item[NEW] )
                        {
                            decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );
                            totalCost = 0;
                            totalQuantity = 0;
                        }
                    }

                    else if ( cif.Class0 == "2" && cif.Class1 == "4" )//"業務番号","作業項目"
                    {
                        if ( ( task[OLD] != task[NEW] ) || ( item[OLD] != item[NEW] ) )
                        {
                            if ( task[OLD] != task[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( item[OLD] != item[NEW] )//作業項目の変化チェック（作業項目を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "3" && cif.Class1 == "2" )//"顧客", "業務番号"
                    {
                        if ( ( customer[OLD] != customer[NEW] ) || ( task[OLD] != task[NEW] ) )
                        {
                            if ( customer[OLD] != customer[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( task[OLD] != task[NEW] )//業務番号の変化チェック（業務番号を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "3" && cif.Class1 == "4" )//"顧客", "作業項目"
                    {
                        if ( ( customer[OLD] != customer[NEW] ) || ( item[OLD] != item[NEW] ) )
                        {
                            if ( customer[OLD] != customer[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( item[OLD] != item[NEW] )//作業項目の変化チェック（作業項目を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "4" && cif.Class1 == "2" )//"作業項目", "業務番号"
                    {
                        if ( ( item[OLD] != item[NEW] ) || ( task[OLD] != task[NEW] ) )
                        {
                            if ( item[OLD] != item[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( task[OLD] != task[NEW] )//業務番号の変化チェック（業務番号を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "5" && cif.Class1 == "2" )//"業務担当者", "業務番号"
                    {
                        if ( ( leaderMName[OLD] != leaderMName[NEW] ) || ( task[OLD] != task[NEW] ) )
                        {
                            if ( leaderMName[OLD] != leaderMName[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( task[OLD] != task[NEW] )//業務番号の変化チェック（業務番号を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "5" && cif.Class1 == "4" )//"業務担当者", "作業項目"
                    {
                        if ( ( leaderMName[OLD] != leaderMName[NEW] ) || ( item[OLD] != item[NEW] ) )
                        {
                            if ( leaderMName[OLD] != leaderMName[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( item[OLD] != item[NEW] )//作業項目の変化チェック（作業項目を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "6" && cif.Class1 == "2" )//"営業担当者","業務番号"
                    {
                        if ( ( salesMName[OLD] != salesMName[NEW] ) || ( task[OLD] != task[NEW] ) )
                        {
                            if ( salesMName[OLD] != salesMName[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( task[OLD] != task[NEW] )//業務番号の変化チェック（業務番号を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }
                    else if ( cif.Class0 == "6" && cif.Class1 == "4" )//"営業担当者","作業項目"
                    {
                        if ( ( salesMName[OLD] != salesMName[NEW] ) || ( item[OLD] != item[NEW] ) )
                        {
                            if ( salesMName[OLD] != salesMName[NEW] )//作業項目の変化チェック
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );

                                totalCost = 0;
                                totalSectionCost = 0;
                                totalQuantity = 0;
                                totalSectionQuantity = 0;
                            }
                            else if ( item[OLD] != item[NEW] )//作業項目の変化チェック（作業項目を出力）
                            {
                                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                                totalSectionCost = 0;
                                totalSectionQuantity = 0;
                            }
                        }
                    }

                    //グリッドへデータをセット
                    dgv.Rows[intGridCnt].Cells["Date"].Value = editDate;
                }
                else
                {
                    //グリッドへデータをセット
                    dgv.Rows[intGridCnt].Cells["Date"].Value = date[NEW];
                }

                //グリッドへデータをセット
                dgv.Rows[intGridCnt].Cells["SlipNo"].Value = slipNo[NEW];
                dgv.Rows[intGridCnt].Cells["Task"].Value = task[NEW];






                //dgv.Rows[intGridCnt].Cells["Item"].Value = item[NEW];
                dgv.Rows[intGridCnt].Cells["Item"].Value = "( " + costCode[NEW] + " )  " + item[NEW];




                dgv.Rows[intGridCnt].Cells["Quantity"].Value = quantity[NEW];
                dgv.Rows[intGridCnt].Cells["Unit"].Value = unit[NEW];
                dgv.Rows[intGridCnt].Cells["UnitPrice"].Value = unitPrice[NEW];
                dgv.Rows[intGridCnt].Cells["Cost"].Value = cost[NEW];
                dgv.Rows[intGridCnt].Cells["Customer"].Value = customer[NEW];
                dgv.Rows[intGridCnt].Cells["LeaderMName"].Value = leaderMName[NEW];
                dgv.Rows[intGridCnt].Cells["SalesMName"].Value = salesMName[NEW];

                dgv.Columns["SlipNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["Cost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                //データの保持（次に取得したデータに異なるか比較する）
                date[OLD] = date[NEW];
                slipNo[OLD] = slipNo[NEW];
                task[OLD] = task[NEW];
                item[OLD] = item[NEW];
                customer[OLD] = customer[NEW];
                leaderMName[OLD] = leaderMName[NEW];
                salesMName[OLD] = salesMName[NEW];

                //作業項目計、業務番号計の計算
                //金額
                totalCost = totalCost + costCalc;
                totalSectionCost = totalSectionCost + costCalc; //作業項目, 業務番号などの組み合わせで使用
                periodTotalCost = periodTotalCost + costCalc;

                //数量
                totalQuantity = totalQuantity + quantityCalc;
                totalSectionQuantity = totalSectionQuantity + quantityCalc; //作業項目, 業務番号などの組み合わせで使用
                periodTotalQuantity = periodTotalQuantity + quantityCalc;

                intGridCnt++;
            }

            //分類項目が「指定なし」又は「売り上げ計上日」
            if ( ( cif.Class0 == "0" ) || ( cif.Class0 == "1" ) )
            {
                decTotalCostQuantitySet( dgv, 3, itemName, totalCost, totalQuantity, ref intGridCnt );
            }
            else if ( ( cif.Class0 == "2" && cif.Class1 == "4" ) ||
                    ( cif.Class0 == "3" && cif.Class1 == "4" ) ||
                    ( cif.Class0 == "5" && cif.Class1 == "4" ) ||
                    ( cif.Class0 == "6" && cif.Class1 == "4" ) )
            {
                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalCost, totalQuantity, ref intGridCnt );
            }
            else if ( ( cif.Class0 == "3" && cif.Class1 == "2" ) ||
                    ( cif.Class0 == "4" && cif.Class1 == "2" ) ||
                    ( cif.Class0 == "5" && cif.Class1 == "2" ) ||
                    ( cif.Class0 == "6" && cif.Class1 == "2" ) )
            {
                decTotalCostQuantitySet( dgv, 3, hTextItemName[0], totalSectionCost, totalSectionQuantity, ref intGridCnt );
                decTotalCostQuantitySet( dgv, 3, hTextItemName[1], totalCost, totalQuantity, ref intGridCnt );
            }

            //期間合計の表示
            dgv.Rows[intGridCnt].Cells[3].Value = hTextItemName[2];
            dgv.Rows[intGridCnt].Cells["Cost"].Value = decFormat( periodTotalCost );
            dgv.Rows[intGridCnt].Cells["Quantity"].Value = decPointFormat( periodTotalQuantity );
            labelTotalCount.Text = intGridCnt.ToString();
            return true;
        }

        private string[] editTaskInfo( string taskCode )
        {
            string debugstring = DHandling.NumberOfCharacters( taskCode, 1 );
            string[] retval = new string[2];
            string sqlStr = "T.TaskName, P.PartnerName FROM D_Task AS T LEFT JOIN M_Partners AS P ON T.PartnerCode = P.PartnerCode WHERE T.TaskBaseCode = '" + DHandling.NumberOfCharacters( taskCode, 1 ) + "'";
            DbAccess dba = new DbAccess();
            DataTable dt = dba.UsingSqlstring_Select( sqlStr );
            if ( dt == null || dt.Rows.Count < 1 )
            {
                retval[0] = "（" + taskCode + "）";
                retval[1] = "";
            }
            else
            {
                DataRow dr = dt.Rows[0];
                retval[0] = "（" + taskCode + "）" + Convert.ToString( dr["TaskName"] );
                retval[1] = Convert.ToString( dr["PartnerName"] );
            }
            return retval;
        }


        private string editMemberName( string memberCode )
        {
            if ( String.IsNullOrEmpty( memberCode.Trim() ) )
                return "";

            memberCode = memberCode.Trim();
            if ( memberCode.Length < 3 )
            {
                memberCode = "00" + memberCode;
                memberCode = memberCode.Substring( memberCode.Length - 3, 3 );
            }

            DbAccess dba = new DbAccess();
            DataTable dt = dba.UsingParamater_Select( "M_Members", "WHERE MemberCode = '" + memberCode + "'" );
            if ( dt == null || dt.Rows.Count < 1 )
            {
                return "";
            }
            DataRow dr = dt.Rows[0];
            return Convert.ToString( dr["Name"] );
        }

        private static decimal toRegDecimal( string decStr )
        {
            return DHandling.ToRegDecimal( decStr );
        }

        private static string decFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "#,0" );
        }

        private static string decPointFormat( decimal decNum )
        {
            return DHandling.DecimaltoStr( decNum, "0.00" );
        }

        private void decTotalCostQuantitySet( DataGridView dgv, int cellNo, string itemName, decimal totalSectionCost, decimal totalSectionQuantity, ref int intGridCnt )
        {
            dgv.Rows.Add();
            dgv.Rows[intGridCnt].Cells[cellNo].Value = itemName;                                //項目名
            dgv.Rows[intGridCnt].Cells["Cost"].Value = decFormat( totalSectionCost );
            dgv.Rows[intGridCnt].Cells["Quantity"].Value = decPointFormat( totalSectionQuantity );
            intGridCnt++;
        }
    }
}

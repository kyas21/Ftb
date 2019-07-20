using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class CostReportData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        const string insSql = "INSERT INTO D_CostReport "
                                + "(SlipNo, TaskCode, OfficeCode, Department, ReportDate, ItemCode, Item, UnitPrice, Quantity, Cost,"
                                + "Unit, CustoCode, SubCoCode, Subject, MemberCode, LeaderMCode, SalesMCode, AccountCode, Note, CoTaskCode) VALUES ("
                                + "@slip, @tCod, @oCod, @dept, @rDat, @iCod, @item, @uPri, @qty, @cost, "
                                + "@unit, @cust, @subc, @subj, @mCod, @lCod, @sCod, @aCod, @note, @cTcd )";

        const string updSqlAll = "UPDATE D_CostReport SET "
                                + "SlipNo = @slip, TaskCode = @tCod, OfficeCode = @oCod, Department = @dept, ReportDate = @rDat, ItemCode = @iCod, Item = @item, UnitPrice = @uPri, Quantity = @qty, Cost = @cost, "
                                + "Unit = @unit, CustoCode = @cust, SubCoCode = @subc, Subject = @subj, MemberCode = @mCod, LeaderMCode = @lCod, SalesMCode = @sCod, AccountCode = @aCod, Note = @note, CoTaskCode = @cTcd "
                                + "WHERE CostReportID = @crID";

        const string updSql = "UPDATE D_CostReport SET "
                                + "TaskCode = @tCod, OfficeCode = @oCod, Department = @dept, Quantity = @qty, Cost = @cost, LeaderMCode = @lCod "
                                + "WHERE SlipNo = @slip";

        const string updTaskCode = "UPDATE D_CostReport SET TaskCode = @tCod WHERE SlipNo = @slip";

        const string delSql = "DELETE FROM D_CostReport WHERE ";

        const string delSqlUsedCostReportID = "DELETE FROM D_CostReport WHERE CostReportID = @crID";

        const string seliDSql = ";SELECT CAST(SCOPE_IDENTITY() AS int)";

        const string slipNoExitence = "SELECT * FROM D_CostReport WHERE SlipNo = @slipNo AND TaskCode = @tCod AND OfficeCode = @oCod "
                                        + "AND Department = @dept AND ReportDate = @rDat AND ItemCode = @iCod AND Item = @item AND Cost = @cost";

        const string updCostReport = "UPDATE D_CostReport SET TaskCode = @tCod, CustoCode = @cust, LeaderMCode = @lCod, SalesMCode = @sCod WHERE SlipNo = @slip";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public CostReportData()
        {

        }

        public CostReportData(DataRow dr)
        {
            CostReportID = dr.Field<Int32?>("CostReportID") ?? default(Int32);
            SlipNo = dr.Field<Int32?>("SlipNo") ?? default(Int32);
            OfficeCode = dr.Field<String>("OfficeCode") ?? default(String);
            Department = dr.Field<String>("Department") ?? default(String);
            TaskCode = dr.Field<String>("TaskCode") ?? default(String);
            ReportDate = dr.Field<DateTime?>("ReportDate") ?? DateTime.MinValue;
            ItemCode = dr.Field<String>("ItemCode") ?? default(String);
            Item = dr.Field<String>("Item") ?? default(String);
            UnitPrice = dr.Field<Decimal?>("UnitPrice") ?? default(Decimal);
            Quantity = dr.Field<Decimal?>("Quantity") ?? default(Decimal);
            Cost = dr.Field<Decimal?>("Cost") ?? default(Decimal);
            Unit = dr.Field<String>("Unit") ?? default(String);
            CustoCode = dr.Field<String>("CustoCode") ?? default(String);
            SubCoCode = dr.Field<String>("SubCoCode") ?? default(String);
            Subject = dr.Field<String>("Subject") ?? default(String);
            MemberCode = dr.Field<String>("MemberCode") ?? default(String);
            LeaderMCode = dr.Field<String>("LeaderMCode") ?? default(String);
            SalesMCode = dr.Field<String>("SalesMCode") ?? default(String);
            AccountCode = dr.Field<String>("AccountCode") ?? default(String);
            Note = dr.Field<String>("Note") ?? default(String);

            //CostReportID = Convert.ToInt32(dr["CostReportID"]);
            //SlipNo = Convert.ToInt32(dr["SlipNo"]);
            //OfficeCode = Convert.ToString(dr["OfficeCode"]);
            //Department = Convert.ToString(dr["Department"]);
            //TaskCode = Convert.ToString(dr["TaskCode"]);
            //ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            //ItemCode = Convert.ToString(dr["ItemCode"]);                // = CostCode
            //Item = Convert.ToString(dr["Item"]);
            //UnitPrice = (Convert.ToString(dr["UnitPrice"]) == "") ? 0 : Convert.ToDecimal(dr["UnitPrice"]);
            //Quantity = (Convert.ToString(dr["Quantity"]) == "") ? 0 : Convert.ToDecimal(dr["Quantity"]);
            //Cost = (Convert.ToString(dr["Cost"]) == "") ? 0 : Convert.ToDecimal(dr["Cost"]);
            ////UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
            ////Quantity = Convert.ToDecimal(dr["Quantity"]);
            ////Cost = Convert.ToDecimal(dr["Cost"]);
            //Unit = Convert.ToString(dr["Unit"]);
            //CustoCode = Convert.ToString(dr["CustoCode"]);
            //SubCoCode = Convert.ToString(dr["SubCoCode"]);
            //Subject = Convert.ToString(dr["Subject"]);
            //MemberCode = Convert.ToString(dr["MemberCode"]);
            //LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
            //SalesMCode = Convert.ToString(dr["SalesMCode"]);
            //AccountCode = Convert.ToString(dr["AccountCode"]);
            //Note = Convert.ToString(dr["Note"]);
            Publisher = OfficeCode + Department;
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int CostReportID { get; set; }
        public int SlipNo { get; set; }
        public string TaskCode { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public DateTime ReportDate { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }
        public string CustoCode { get; set; }
        public string SubCoCode { get; set; }
        public string Subject { get; set; }
        public string MemberCode { get; set; }
        public string LeaderMCode { get; set; }
        public string SalesMCode { get; set; }
        public string AccountCode { get; set; }
        public string Note { get; set; }
        public string CoTaskCode { get; set; }
        public string Publisher { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public static readonly string InsertSqlCostReport = "INSERT INTO D_CostReport "
                                + "(SlipNo, TaskCode, OfficeCode, Department, ReportDate, ItemCode, Item, UnitPrice, Quantity, Cost,"
                                + "Unit, CustoCode, SubCoCode, Subject, MemberCode, LeaderMCode, SalesMCode, AccountCode, Note, CoTaskCode) VALUES ("
                                + "@slip, @tCod, @oCod, @dept, @rDat, @iCod, @item, @uPri, @qty, @cost, "
                                + "@unit, @cust, @subc, @subj, @mCod, @lCod, @sCod, @aCod, @note, @cTcd )";

        public object Clone()
        {
            CostReportData cloneData = new CostReportData();

            cloneData.CostReportID = this.CostReportID;
            cloneData.SlipNo = this.SlipNo;
            cloneData.TaskCode = this.TaskCode;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.ReportDate = this.ReportDate;
            cloneData.ItemCode = this.ItemCode;
            cloneData.Item = this.Item;
            cloneData.UnitPrice = this.UnitPrice;
            cloneData.Quantity = this.Quantity;
            cloneData.Cost = this.Cost;
            cloneData.Unit = this.Unit;
            cloneData.CustoCode = this.CustoCode;
            cloneData.SubCoCode = this.SubCoCode;
            cloneData.Subject = this.Subject;
            cloneData.MemberCode = this.MemberCode;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.SalesMCode = this.SalesMCode;
            cloneData.AccountCode = this.AccountCode;
            cloneData.Note = this.Note;
            cloneData.CoTaskCode = this.CoTaskCode;
            cloneData.Publisher = this.Publisher;

            return cloneData;
        }


        public static int ReadNowSlipNo()
        {
            SqlHandling sql = new SqlHandling("D_CostReport");
            return sql.MaxValue("SlipNo");
        }


        public static int ReadMinSlipNo()
        {
            SqlHandling sql = new SqlHandling("D_CostReport");
            return sql.MinValue("SlipNo");
        }


        public bool ExistenceSlipNo()
        {
            using ( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand( slipNoExitence, conn );
                    cmd.Parameters.Add( "@slipNo", SqlDbType.Int );
                    cmd.Parameters.Add("@tCod", SqlDbType.Char);
                    cmd.Parameters.Add("@oCod", SqlDbType.Char);
                    cmd.Parameters.Add("@dept", SqlDbType.Char);
                    cmd.Parameters.Add("@rDat", SqlDbType.Date);
                    cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
                    cmd.Parameters.Add("@item", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@cost", SqlDbType.Decimal);
                    cmd.Parameters["@slipNo"].Value = SlipNo;
                    cmd.Parameters["@tCod"].Value = TaskCode;
                    cmd.Parameters["@oCod"].Value = OfficeCode;
                    cmd.Parameters["@dept"].Value = Department;
                    cmd.Parameters["@rDat"].Value = ReportDate;
                    cmd.Parameters["@iCod"].Value = ItemCode;
                    cmd.Parameters["@item"].Value = Item;
                    cmd.Parameters["@cost"].Value = Cost;
                    SqlDataReader dr = TryExReader( conn, cmd );
                    if( !dr.HasRows ) return false;
                    dr.Close();
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
            }
            return true;
        }


        public CostReportData[] SelectCostReportItemCode( string officeCode, string taskCode, int fYear )
        {
            //string sqlStr = " DISTINCT LEFT(ItemCode,4) AS iCod, Item, MemberCode FROM D_CostReport WHERE OfficeCode = '" + officeCode + "' AND TaskCode = '" + taskCode + "' AND (ItemCode LIKE 'A%' OR ItemCode LIKE 'B%')"
            //              + " AND ( ReportDate BETWEEN '" + DHandling.FisicalYearStartDate( fYear ) + "' AND '" + DHandling.FisicalYearEndDate( fYear ) + "') ORDER BY LEFT(ItemCode,4)";
            //string sqlStr = " ItemCode, Item, MemberCode FROM D_CostReport WHERE OfficeCode = '" + officeCode + "' AND TaskCode = '" + taskCode + "' AND (ItemCode LIKE 'A%' OR ItemCode LIKE 'B%')"
            //              + " AND ( ReportDate BETWEEN '" + DHandling.FisicalYearStartDate( fYear ) + "' AND '" + DHandling.FisicalYearEndDate( fYear ) + "') ORDER BY SUBSTRING LEFT(ItemCode,4)";
            string sqlStr = " ItemCode, Item, MemberCode FROM D_CostReport WHERE OfficeCode = '" + officeCode + "' AND TaskCode = '" + taskCode + "' AND (ItemCode LIKE 'A%' OR ItemCode LIKE 'B%')"
                          + " AND ( ReportDate BETWEEN '" + DHandling.FisicalYearStartDate( fYear ) + "' AND '" + DHandling.FisicalYearEndDate( fYear ) + "') ORDER BY ItemCode";

            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core( sqlStr );
            if( dt == null ) return null;


            string wkItemCode = "";
            int rowIdx = 0;
            DataRow dr;
            for(int i = 0;i<dt.Rows.Count;i++ )
            {
                dr = dt.Rows[i];
                if( wkItemCode != ( Convert.ToString( dr["ItemCode"] ) ).TrimEnd() )
                {
                    rowIdx++;
                    wkItemCode = ( Convert.ToString( dr["ItemCode"] ) ).TrimEnd();
                }
            }

            CostReportData[] crd = new CostReportData[rowIdx];

            wkItemCode = "";
            rowIdx = 0;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                if( wkItemCode != ( Convert.ToString( dr["ItemCode"] ) ).TrimEnd() )
                {
                    wkItemCode = ( Convert.ToString( dr["ItemCode"] ) ).TrimEnd();

                    crd[rowIdx] = new CostReportData();
                    crd[rowIdx].ItemCode = wkItemCode;
                    crd[rowIdx].Item = Convert.ToString( dr["Item"] );

                    rowIdx++;
                }
            }

            return crd;
        }


        public CostReportData[] SelectCostReport(int slipNo)
        {
            SqlHandling sh = new SqlHandling("D_CostReport");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
            if (dt == null || dt.Rows.Count < 1) return null;

            CostReportData[] crd = new CostReportData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) crd[i] = new CostReportData(dt.Rows[i]);

            return crd;
        }


        public CostReportData[] SelectCostReport(DateTime dateFr, DateTime dateTo, string taskCode, string officeCode, string department)
        {
            SqlHandling sh = new SqlHandling("D_CostReport");
            DataTable dt = sh.SelectAllData("WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND TaskCode = '" + taskCode
                                            + "' AND ( ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo + "') ORDER BY ReportDate");
            if (dt == null || dt.Rows.Count < 1) return null;

            CostReportData[] crd = new CostReportData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) crd[i] = new CostReportData(dt.Rows[i]);

            return crd;
        }


        public CostReportData[] SelectCostReport(DateTime dateFr, DateTime dateTo, string taskCode, string itemCode, string officeCode, string department, string colName)
        {
            SqlHandling sh = new SqlHandling("D_CostReport");
            DataTable dt = sh.SelectAllData("WHERE ( ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo + "') "
                                            + "AND TaskCode = '" + taskCode + "' AND " + colName + " = '" + itemCode + "' "
                                            + "AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "' ORDER BY ReportDate");
            if (dt == null || dt.Rows.Count < 1) return null;

            CostReportData[] crd = new CostReportData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) crd[i] = new CostReportData(dt.Rows[i]);

            return crd;
        }


        public int SelectCostSlipNo(DateTime reportDate, string taskCode, string subCoCode, string officeCode, string department)
        {
            string sqlStr = " DISTINCT SlipNo FROM D_CostReport WHERE ReportDate = '" + reportDate + "' AND TaskCode = '" + taskCode
                            + "' AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND SubCoCode = '" + subCoCode + "'";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr);
            if (dt == null || dt.Rows.Count < 1) return 0;

            DataRow dr = dt.Rows[0];
            return  Convert.ToInt32(dr["SlipNo"]);
        }


        public int[] SelectCostSlipNo(DateTime reportDate, string taskCode, string officeCode)
        {
            int[] sNoDArray = new int[] { -1 };
            string sqlStr= " DISTINCT SlipNo FROM D_CostReport WHERE ReportDate = '" + reportDate + "' AND OfficeCode = '" + officeCode + "'";
            if (taskCode != "")
                sqlStr += " AND TaskCode = '" + taskCode + "'";
            sqlStr += " ORDER BY SlipNo"; 
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr);
            if (dt == null || dt.Rows.Count < 1) return sNoDArray;
            sNoDArray = new int[dt.Rows.Count];
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                sNoDArray[i] = Convert.ToInt32(dr["SlipNo"]);
            }
            return sNoDArray;
        }


        public int CountSlipNo(DateTime dateFr, DateTime dateTo, string taskCode, string officeCode, string department)
        {
            string sqlStr = " COUNT(DISTINCT SlipNo) AS sNC FROM D_CostReport WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND TaskCode = '" + taskCode
                          + "' AND ( ReportDate BETWEEN '" + dateFr.StripTime() + "' AND '" + dateTo.StripTime() + "')";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr);
            if (dt == null || dt.Rows.Count < 1) return 0;
            DataRow dr = dt.Rows[0];
            return Convert.ToInt32(dr["sNC"]);
        }


        public Decimal SumMonthlyQuantity( string officeCode, string itemCode,DateTime month )
        {
            string sqlStr = "SUM( Quantity ) AS SumQty FROM D_CostReport "
                          + "WHERE OfficeCode = '" + officeCode + "' AND "
                          + "ItemCode = '" + itemCode + "' AND "
                          + "( ReportDate BETWEEN '" + month + "' AND '" + month.EndOfMonth() + "' )";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr);
            if (dt == null) return 0;

            CostReportData crd = new CostReportData();
            DataRow dr = dt.Rows[0];
            return ( string.IsNullOrEmpty( Convert.ToString( dr["SumQty"] ) ) ) ? 0 : Convert.ToDecimal( dr["SumQty"] );
        }


        public Decimal SumMonthlyCost( string taskCode, string officeCode, DateTime month )
        {
            string sqlStr = "SUM( Cost ) AS SumCost FROM D_CostReport "
                          + "WHERE TaskCode = '" + taskCode + "' AND "
                          + "OfficeCode = '" + officeCode + "' AND "
                          + "( LEFT(ItemCode, 1 ) = 'A' OR LEFT( ItemCode, 1 ) = 'B' ) AND "
                          + "( ReportDate BETWEEN '" + month + "' AND '" + month.EndOfMonth() + "' )";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core( sqlStr );
            if( dt == null ) return 0;

            CostReportData crd = new CostReportData();
            DataRow dr = dt.Rows[0];
            return ( string.IsNullOrEmpty( Convert.ToString( dr["SumCost"] ) ) ) ? 0 : Convert.ToDecimal( dr["SumCost"] );
        }


        public bool InsertCostReportAndGetID()
        {
            SlipNo = ReadNowSlipNo() + 1;
            return InsertCostReport();
        }


        public bool InsertCostReport()
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql + seliDSql, conn);
                    cmd = parametersSqlDbType(cmd);
                    cmd = parametersValue(cmd);

                    if ((CostReportID = TryExScalar(conn, cmd)) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool InsertCostReport(CostReportData[] crd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql, conn);
                    cmd = parametersSqlDbType(cmd);

                    //for (int i = 0; i < crd.GetLength(0); i++)
                    for (int i = 0; i < crd.Length; i++)
                    {
                        crd[i].SlipNo = crd[0].SlipNo;
                        cmd = parametersValue(cmd, crd[i]);

                        if (TryExecute(conn, cmd) < 0) return false;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public OsWkDetailData[] InsertCostReport(OsWkDetailData[] dtl)
        {
            SlipNo = ReadNowSlipNo() + 1;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql + seliDSql, conn);
                    cmd = parametersSqlDbType(cmd);

                    //for (int i = 0; i < dtl.GetLength(0); i++)
                    for (int i = 0; i < dtl.Length; i++)
                    {
                        editCostReportData(dtl[i]);
                        cmd = parametersValue(cmd);

                        if ((CostReportID = TryExScalar(conn, cmd)) < 0) return null;
                        dtl[i].SlipNo = SlipNo;
                        dtl[i].CostReportID = CostReportID;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return null;
                }
                conn.Close();
                tran.Complete();
            }
            return dtl;
        }


        public bool UpdateCostReport()
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updSql, conn);

                    cmd = parametersSqlDbTypeA(cmd);
                    cmd = parametersValueA(cmd);

                    if (TryExecute(conn, cmd) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool UpdateCostReport(OsPayOffData pod)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updSql, conn);

                    cmd = parametersSqlDbTypeA(cmd);
                    cmd = parametersValueA(cmd, pod);

                    if (TryExecute(conn, cmd) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool UpdateCostReport(OsPaymentData opd)
        {
            SlipNo = opd.SlipNo;
            OfficeCode = opd.OfficeCode;
            Department = opd.Department;
            TaskCode = opd.TaskCode;
            Quantity = 1;
            Cost = opd.Amount;
            LeaderMCode = opd.LeaderMCode;
            return UpdateCostReport();
        }


        public bool MergeCostReport(CostReportData[] crd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    //for (int i = 0; i < crd.GetLength(0); i++)
                    for (int i = 0; i < crd.Length; i++)
                    {
                        if (crd[i].CostReportID == 0)
                        {
                            cmd = new SqlCommand(insSql, conn);
                        }
                        else
                        {
                            if (crd[i].ItemCode == " " || crd[i].ItemCode == "")
                            {
                                cmd = new SqlCommand(delSqlUsedCostReportID, conn);
                            }
                            else
                            {
                                cmd = new SqlCommand(updSqlAll, conn);
                            }
                            cmd.Parameters.Add("@crID", SqlDbType.Int);
                            cmd.Parameters["@crID"].Value = crd[i].CostReportID;
                        }

                        if (crd[i].ItemCode != " " && crd[i].ItemCode != "")
                        {
                            cmd = parametersSqlDbType(cmd);
                            cmd = parametersValue(cmd, crd[i]);
                        }

                        if (TryExecute(conn, cmd) < 0) return false;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        // 全レコードの削除
        public bool DeleteCostReport(string para, int value)
        {
            if (para != "@slip" && para != "@crID") return false;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string deleteSql; 
                    if (para == "@slip")
                    {
                        deleteSql = delSql + "SlipNo = @slip";
                    }
                    else
                    {
                        deleteSql = delSql + "CostReportID = @crID";
                    }

                    SqlCommand cmd = new SqlCommand(deleteSql, conn);
                    cmd.Parameters.Add(para, SqlDbType.Int);
                    cmd.Parameters[para].Value = value;
                    
                    if (TryExecute(conn, cmd) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public int StoreCostReportData(CostReportData[] crd)
        {
            if (crd[0].SlipNo == 0)
            {
                crd[0].SlipNo = ReadNowSlipNo() + 1;
                if (InsertCostReport(crd)) return crd[0].SlipNo;
            }
            else
            {
                if (MergeCostReport(crd)) return 0;
            }
            return -1;
        }


        public bool UpdateTaskCodeOsWkReport(int slipNo, int reportID, string dTaskCode)
        {
            TaskIndData tid = new TaskIndData();
            tid = tid.SelectInfoAboutTask(dTaskCode);

            string updOsWkDetail = "UPDATE D_OsWkDetail SET TaskCode = @tCod WHERE SlipNo = @slip";
            string updOsWkReport = "UPDATE D_OsWkReport SET TaskCode = @tCod WHERE OsWkReportID = @oRID";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdCostReport = new SqlCommand(updCostReport, conn);
                    cmdCostReport = forTaskCodeSetCostReportDataforTaskCode(cmdCostReport, slipNo, tid);
                    if (TryExecute(conn, cmdCostReport) < 0) return false;

                    SqlCommand cmdOsWkDetail = new SqlCommand(updOsWkDetail, conn);
                    cmdOsWkDetail.Parameters.Add("@slip", SqlDbType.Int);
                    cmdOsWkDetail.Parameters.Add("@tCod", SqlDbType.Char);
                    cmdOsWkDetail.Parameters["@slip"].Value = slipNo;
                    cmdOsWkDetail.Parameters["@tCod"].Value = dTaskCode;
                    if (TryExecute(conn, cmdOsWkDetail) < 0) return false;

                    SqlCommand cmdOsWkReport = new SqlCommand(updOsWkReport, conn);
                    cmdOsWkReport.Parameters.Add("@oRID", SqlDbType.Int);
                    cmdOsWkReport.Parameters.Add("@tCod", SqlDbType.Char);
                    cmdOsWkReport.Parameters["@oRID"].Value = reportID;
                    cmdOsWkReport.Parameters["@tCod"].Value = dTaskCode;
                    if (TryExecute(conn, cmdOsWkReport) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public bool UpdateTaskCode( int slipNo, string dTaskCode, string table )
        {
            TaskIndData tid = new TaskIndData();
            tid = tid.SelectInfoAboutTask( dTaskCode );

            string updTable = "UPDATE " + table + " SET TaskCode = @tCod WHERE SlipNo = @slip";

            using ( TransactionScope tran = new TransactionScope() )
            using ( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdCostReport = new SqlCommand( updCostReport, conn );
                    cmdCostReport = forTaskCodeSetCostReportDataforTaskCode( cmdCostReport, slipNo, tid );
                    if ( TryExecute( conn, cmdCostReport ) < 0 ) return false;

                    SqlCommand cmd = new SqlCommand( updTable, conn );
                    cmd.Parameters.Add( "@slip", SqlDbType.Int );
                    cmd.Parameters.Add( "@tCod", SqlDbType.Char );
                    cmd.Parameters[ "@slip" ].Value = slipNo;
                    cmd.Parameters[ "@tCod" ].Value = dTaskCode;
                    if ( TryExecute( conn, cmd ) < 0 ) return false;
                }
                catch ( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        //--------------------------------------------------------//
        //      SubRoutine
        //--------------------------------------------------------//
        public SqlCommand parametersSqlDbType(SqlCommand cmd)
        {
            cmd = parametersSqlDbTypeA(cmd);
            cmd = parametersSqlDbTypeB(cmd);
            return cmd;
        }


        private SqlCommand parametersSqlDbTypeA(SqlCommand cmd)
        {
            cmd.Parameters.Add("@slip", SqlDbType.Int);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@qty", SqlDbType.Decimal);
            cmd.Parameters.Add("@cost", SqlDbType.Decimal);
            cmd.Parameters.Add("@lCod", SqlDbType.Char);
            return cmd;
        }


        private SqlCommand parametersSqlDbTypeB(SqlCommand cmd)
        {
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@uPri", SqlDbType.Decimal);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cust", SqlDbType.Char);
            cmd.Parameters.Add("@subc", SqlDbType.Char);
            cmd.Parameters.Add("@subj", SqlDbType.Char);
            cmd.Parameters.Add("@mCod", SqlDbType.Char);
            cmd.Parameters.Add("@sCod", SqlDbType.Char);
            cmd.Parameters.Add("@aCod", SqlDbType.Char);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cTCd", SqlDbType.Char);
            return cmd;
        }


        private SqlCommand parametersValue(SqlCommand cmd, CostReportData crd)
        {
            cmd = parametersValueA(cmd, crd);
            cmd = parametersValueB(cmd, crd);
            return cmd;
        }


        private SqlCommand parametersValueA(SqlCommand cmd, CostReportData crd)
        {
            cmd.Parameters["@slip"].Value = crd.SlipNo;         // 0
            cmd.Parameters["@tCod"].Value = crd.TaskCode;       // 1
            cmd.Parameters["@oCod"].Value = crd.OfficeCode;     // 2
            cmd.Parameters["@dept"].Value = crd.Department;     // 3
            cmd.Parameters["@qty"].Value = crd.Quantity;        // 8
            cmd.Parameters["@cost"].Value = crd.Cost;           // 9
            cmd.Parameters["@lCod"].Value = crd.LeaderMCode;    // 15
            return cmd;
        }


        private SqlCommand parametersValueB(SqlCommand cmd, CostReportData crd)
        {
            cmd.Parameters["@rDat"].Value = crd.ReportDate;     // 4
            cmd.Parameters["@iCod"].Value = crd.ItemCode;       // 5
            cmd.Parameters["@item"].Value = crd.Item;           // 6
            cmd.Parameters["@uPri"].Value = crd.UnitPrice;      // 7
            cmd.Parameters["@unit"].Value = crd.Unit;           // 10
            cmd.Parameters["@cust"].Value = crd.CustoCode;      // 11
            cmd.Parameters["@subc"].Value = crd.SubCoCode;      // 12
            cmd.Parameters["@subj"].Value = crd.Subject;        // 13
            cmd.Parameters["@mCod"].Value = crd.MemberCode;     // 14
            cmd.Parameters["@sCod"].Value = crd.SalesMCode;     // 16
            cmd.Parameters["@aCod"].Value = crd.AccountCode;    // 17
            cmd.Parameters["@note"].Value = crd.Note;           // 18
            cmd.Parameters["@cTCd"].Value = crd.CoTaskCode;     // 19
            //cmd.Parameters["@cTCd"].Value = String.IsNullOrEmpty(crd.CoTaskCode)?"":crd.CoTaskCode;     // 19
            return cmd;
        }


        private SqlCommand parametersValueA(SqlCommand cmd, OsPayOffData pod)
        {
            cmd.Parameters["@slip"].Value = pod.SlipNo;         // 0
            cmd.Parameters["@tCod"].Value = pod.TaskCode;       // 1
            cmd.Parameters["@oCod"].Value = pod.OfficeCode;     // 2
            cmd.Parameters["@dept"].Value = pod.Department;     // 3
            cmd.Parameters["@qty"].Value = 1;                   // 8
            cmd.Parameters["@cost"].Value = pod.Cost;           // 9
            cmd.Parameters["@lCod"].Value = pod.LeaderMCode;    // 15
            return cmd;
        }


        public SqlCommand parametersValue(SqlCommand cmd)
        {
            cmd = parametersValueA(cmd);
            cmd = parametersValueB(cmd);
            return cmd;
        }


        private SqlCommand parametersValueA(SqlCommand cmd)
        {
            cmd.Parameters["@slip"].Value = SlipNo;         // 0
            cmd.Parameters["@tCod"].Value = TaskCode;       // 1
            cmd.Parameters["@oCod"].Value = OfficeCode;     // 2
            cmd.Parameters["@dept"].Value = Department;     // 3
            cmd.Parameters["@qty"].Value = Quantity;        // 8
            cmd.Parameters["@cost"].Value = Cost;           // 9
            cmd.Parameters["@lCod"].Value = LeaderMCode;    // 15
            return cmd;
        }


        private SqlCommand parametersValueB(SqlCommand cmd)
        {
            cmd.Parameters["@rDat"].Value = ReportDate;     // 4
            cmd.Parameters["@iCod"].Value = ItemCode;       // 5
            cmd.Parameters["@item"].Value = Item;           // 6
            cmd.Parameters["@uPri"].Value = UnitPrice;      // 7
            cmd.Parameters["@unit"].Value = Unit;           // 10
            cmd.Parameters["@cust"].Value = CustoCode;      // 11
            cmd.Parameters["@subc"].Value = SubCoCode;      // 12
            cmd.Parameters["@subj"].Value = Subject;        // 13
            cmd.Parameters["@mCod"].Value = MemberCode;     // 14
            cmd.Parameters["@sCod"].Value = SalesMCode;     // 16
            cmd.Parameters["@aCod"].Value = AccountCode;    // 17
            cmd.Parameters["@note"].Value = Note;           // 18
            cmd.Parameters["@cTCd"].Value = CoTaskCode;     // 19
            return cmd;
        }


        private void editCostReportData(OsWkDetailData wdtl)
        {
            TaskCode = wdtl.TaskCode;
            OfficeCode = wdtl.OfficeCode;
            Department = wdtl.Department;
            ReportDate = wdtl.ReportDate;
            ItemCode = wdtl.ItemCode;
            Item = wdtl.Item;
            UnitPrice = wdtl.Cost;
            Quantity = wdtl.Quantity;
            Cost = (wdtl.Cost == 0) ? 0 : wdtl.Quantity * wdtl.Cost;
            Unit = (wdtl.Unit == null) ? "":wdtl.Unit;
            SalesMCode = wdtl.SalesMCode;
            CustoCode = wdtl.CustoCode;
            SubCoCode = wdtl.PartnerCode;
            Subject = wdtl.Subject;
            MemberCode = wdtl.LeaderMCode;
            LeaderMCode = wdtl.LeaderMCode;
            AccountCode = "OSWR";
            Note = "";
            CoTaskCode = wdtl.CoTaskCode;

            Publisher = wdtl.OfficeCode + wdtl.Department;
        }


        private SqlCommand forTaskCodeSetCostReportDataforTaskCode( SqlCommand cmd, int slipNo, TaskIndData tid )
        {
            //TaskIndData tid = new TaskIndData();
            //tid = tid.SelectInfoAboutTask( taskCode );

            cmd.Parameters.Add( "@slip", SqlDbType.Int );
            cmd.Parameters.Add( "@tCod", SqlDbType.Char );
            cmd.Parameters.Add( "@cust", SqlDbType.Char );
            cmd.Parameters.Add( "@lCod", SqlDbType.Char );
            cmd.Parameters.Add( "@sCod", SqlDbType.Char );
            cmd.Parameters[ "@slip" ].Value = slipNo;
            cmd.Parameters[ "@tCod" ].Value = tid.TaskCode;
            cmd.Parameters[ "@cust" ].Value = tid.PartnerCode;
            cmd.Parameters[ "@lCod" ].Value = tid.LeaderMCode;
            cmd.Parameters[ "@sCod" ].Value = tid.SalesMCode;

            return cmd;
        }

    }
}

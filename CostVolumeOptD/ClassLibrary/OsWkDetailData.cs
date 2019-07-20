using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class OsWkDetailData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        const int subjCnt = 6;

        const string updSql = "UPDATE D_OsWkDetail SET "
                    + "PartnerCode = @pCod, TaskCode = @tCod, ReportDate = @rDat, ItemCode = @iCod, Item = @item, ItemDetail = @iDtl, "
                    + "Range = @rang, Quantity = @qty, Unit = @unit, Cost = @cost, Subject = @subj, OsWkReportID = @oRID, LNo = @lNo, "
                    + "PNo = @pNo, SlipNo = @slip, RecType = @rTyp "
                    + "WHERE CostReportID = @cRID";

        const string updTaskCode = "UPDATE D_OsWkDetail SET TaskCode = @tCod WHERE SlipNo = @slip";

        const string delSql = "DELETE FROM D_OsWkDetail WHERE ";

        const string selSql = "SELECT * FROM D_OsWkDetail WHERE SlipNo = @slip AND CostReportID = @cRID";

        const string seliDSql = ";SELECT CAST(SCOPE_IDENTITY() AS int)";

        const string selOsWkReportID = "SELECT OsWkReportID FROM D_OsWkDetail WHERE SlipNo = @slip AND CostReportID = @cRID";

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsWkDetailData()
        {
        }


        public OsWkDetailData(DataRow dr)
        {
            OsWkDetailID = Convert.ToInt32(dr["OsWkDetailID"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            ItemCode = Convert.ToString(dr["ItemCode"]);
            Item = Convert.ToString(dr["Item"]);
            ItemDetail = Convert.ToString(dr["ItemDetail"]);
            Range = Convert.ToString(dr["Range"]);
            Quantity = Convert.ToInt32(dr["Quantity"]);
            Unit = Convert.ToString(dr["Unit"]);
            Cost = Convert.ToInt32(dr["Cost"]);
            Subject = Convert.ToString(dr["Subject"]);
            OsWkReportID = Convert.ToInt32(dr["OsWkReportID"]);
            LNo = Convert.ToInt32(dr["LNo"]);
            PNo = Convert.ToInt32(dr["PNo"]);
            SlipNo = Convert.ToInt32(dr["SlipNo"]);
            RecType = Convert.ToInt32(dr["RecType"]);
            CostReportID = Convert.ToInt32(dr["CostReportID"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsWkDetailID { get; set; }
        public string PartnerCode { get; set; }
        public string TaskCode { get; set; }
        public DateTime ReportDate { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public string ItemDetail { get; set; }
        public string Range { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Cost { get; set; }
        public string Subject { get; set; }
        public int OsWkReportID { get; set; }
        public int LNo { get; set; }
        public int PNo { get; set; }
        public int SlipNo { get; set; }
        public int RecType { get; set; }
        public int CostReportID { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string MmeberCode { get; set; }
        public string LeaderMCode { get; set; }
        public string SalesMCode { get; set; }
        public string CustoCode { get; set; }
        public string CoTaskCode { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            OsWkDetailData cloneData = new OsWkDetailData();
            cloneData.OsWkDetailID = this.OsWkDetailID;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.TaskCode = this.TaskCode;
            cloneData.ReportDate = this.ReportDate;
            cloneData.ItemCode = this.ItemCode;
            cloneData.Item = this.Item;
            cloneData.ItemDetail = this.ItemDetail;
            cloneData.Range = this.Range;
            cloneData.Quantity = this.Quantity;
            cloneData.Unit = this.Unit;
            cloneData.Cost = this.Cost;
            cloneData.Subject = this.Subject;
            cloneData.OsWkReportID = this.OsWkReportID;
            cloneData.LNo = this.LNo;
            cloneData.PNo = this.PNo;
            cloneData.SlipNo = this.SlipNo;
            cloneData.RecType = this.RecType;
            cloneData.CostReportID = this.CostReportID;

            return cloneData;
        }


        public static readonly string sqlInsDtl = "INSERT INTO D_OsWkDetail "
                    + "(PartnerCode, TaskCode, ReportDate, ItemCode, Item, ItemDetail, Range, Quantity, Unit, Cost, Subject, OsWkReportID, LNo, PNo, SlipNo, RecType, CostReportID) "
                    + "VALUES "
                    + "(@pCod, @tCod, @rDat, @iCod, @item, @iDtl, @rang, @qty, @unit, @cost, @subj, @oRID, @lNo, @pNo, @slip, @rTyp, @cRID)";
        

        public bool ExistenceSlipNo(int slipNo, int costReportID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(selSql, conn);
                    cmd.Parameters.Add("@slip", SqlDbType.Int);
                    cmd.Parameters.Add("@cRID", SqlDbType.Int);
                    cmd.Parameters["@slip"].Value = slipNo;
                    cmd.Parameters["@cRID"].Value = costReportID;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (!dr.HasRows) return false;
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
            }
            return true;
        }


        public DataTable SelectOsWkDetail(string partnerCode,string taskCode, DateTime reportMM)
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core("DISTINCT ReportDate FROM D_OsWkDetail WHERE PartnerCode = '" + partnerCode + "' AND TaskCode = '" + taskCode
                                + "' AND ( ReportDate BETWEEN '" + reportMM.BeginOfMonth() + "' AND '" + reportMM.EndOfMonth() + "') ORDER BY ReportDate");
            if (dt.Rows.Count == 0) return null;
            return dt;
        }


        public int SelectOsWkReportID(int slipNo)
        {
            string sqlStr = " DISTINCT OsWkReportID FROM D_OsWkDetail WHERE SlipNo = " + slipNo;
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr);
            if (dt == null || dt.Rows.Count < 1) return 0;

            DataRow dr = dt.Rows[0];
            return Convert.ToInt32(dr["OsWkReportID"]);
        }


        public bool UpdatePartOsWkDetail(int slipNo, int costReportID, decimal qty, decimal cost )
        {
            OsWkDetailData wdd = loadOsWkDetail(slipNo, costReportID);
            wdd.Quantity = qty;
            wdd.Cost = cost;
            return UpdateOsWkDetail(wdd);

        }


        public bool UpdateOsWkDetail(OsWkDetailData wdd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updSql, conn);

                    cmd = addCmdPara(cmd);
                    cmd = addValue(cmd, wdd);

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


        public bool UpdateOsWkDetailTaskCode(int slipNo, string dTaskCode)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updTaskCode + seliDSql, conn);

                    cmd.Parameters.Add("@slip", SqlDbType.Int);
                    cmd.Parameters.Add("@tCod", SqlDbType.Char);
                    cmd.Parameters["@slip"].Value = slipNo;
                    cmd.Parameters["@tCod"].Value = dTaskCode;

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


        public bool DeleteOsWkDetail(string para, int value)
        {
            if (para != "@slip" && para != "@oRID") return false;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string deleteSql;
                    deleteSql = (para == "@slip") ? (delSql + "SlipNo = @slip") : (delSql + "OsWkReportID = @oRID");

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


        private OsWkDetailData loadOsWkDetail(int slipNo, int costReportID)
        {
            SqlHandling sh = new SqlHandling("D_OsWkDetail");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo + " AND CostReportID = " + costReportID);
            if (dt == null || dt.Rows.Count < 1) return null;
            OsWkDetailData wdd = new OsWkDetailData(dt.Rows[0]);
            return wdd;
        }


        public static SqlCommand addCmdPara(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pCod", SqlDbType.Char);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@iCod", SqlDbType.Char);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@rang", SqlDbType.NVarChar);
            cmd.Parameters.Add("@qty", SqlDbType.Decimal);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cost", SqlDbType.Decimal);
            cmd.Parameters.Add("@subj", SqlDbType.Char);
            cmd.Parameters.Add("@oRID", SqlDbType.Int);
            cmd.Parameters.Add("@lNo", SqlDbType.Int);
            cmd.Parameters.Add("@pNo", SqlDbType.Int);
            cmd.Parameters.Add("@slip", SqlDbType.Int);
            cmd.Parameters.Add("@rTyp", SqlDbType.Int);
            cmd.Parameters.Add("@cRID", SqlDbType.Int);
            return cmd;
        }


        public static SqlCommand addValue(SqlCommand cmd, OsWkDetailData wdtl)
        {
            cmd.Parameters["@pCod"].Value = wdtl.PartnerCode;
            cmd.Parameters["@tCod"].Value = wdtl.TaskCode;
            cmd.Parameters["@rDat"].Value = wdtl.ReportDate;
            cmd.Parameters["@iCod"].Value = wdtl.ItemCode;
            cmd.Parameters["@item"].Value = wdtl.Item;
            cmd.Parameters["@iDtl"].Value = wdtl.ItemDetail;
            cmd.Parameters["@rang"].Value = wdtl.Range;
            cmd.Parameters["@qty"].Value = wdtl.Quantity;
            cmd.Parameters["@unit"].Value = wdtl.Unit;
            cmd.Parameters["@cost"].Value = wdtl.Cost;
            cmd.Parameters["@subj"].Value = wdtl.Subject;
            cmd.Parameters["@oRID"].Value = wdtl.OsWkReportID;
            cmd.Parameters["@lNo"].Value = wdtl.LNo;
            cmd.Parameters["@pNo"].Value = wdtl.PNo;
            cmd.Parameters["@slip"].Value = wdtl.SlipNo;
            cmd.Parameters["@rTyp"].Value = wdtl.RecType;
            cmd.Parameters["@cRID"].Value = wdtl.CostReportID;
            return cmd;
        }

        
        //public static SqlCommand addValueB(SqlCommand cmd, OsWkDetailData wdtl)
        //{
        //    cmd.Parameters["@iCod"].Value = wdtl.ItemCode;
        //    cmd.Parameters["@item"].Value = wdtl.Item;
        //    cmd.Parameters["@iDtl"].Value = wdtl.ItemDetail;
        //    cmd.Parameters["@rang"].Value = wdtl.Range;
        //    cmd.Parameters["@qty"].Value = wdtl.Quantity;
        //    cmd.Parameters["@unit"].Value = wdtl.Unit;
        //    cmd.Parameters["@subj"].Value = wdtl.Subject;
        //    cmd.Parameters["@lNo"].Value = wdtl.LNo;
        //    cmd.Parameters["@pNo"].Value = wdtl.PNo;
        //    cmd.Parameters["@slip"].Value = wdtl.SlipNo;
        //    cmd.Parameters["@rTyp"].Value = wdtl.RecType;
        //    cmd.Parameters["@cRID"].Value = wdtl.CostReportID;
        //    return cmd;
        //}


    }
}

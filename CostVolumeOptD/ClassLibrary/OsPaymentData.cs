using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class OsPaymentData:DbAccess
    {

        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string insSql = "INSERT INTO D_OsPayment "
                                + "(ReportDate, OfficeCode, Department, ItemCode, Item, OrderNo, TaskCode, OrderAmount, SAmount, Amount, LeaderMCode, AdminCode, "
                                + "AdminCheck, DirectorCheck, PresidentCheck, ACheckDate, DCheckDate, PCheckDate, SlipNo, CostReportID, Unit) VALUES ("
                                + "@rDat, @oCod, @dept, @iCod, @item, @odNo, @tCod, @oAmt, @sAmt, @Amt, @lCod, @aCod, "
                                + "@aChk, @dChk, @pChk, @aDat, @dDat, @pDat, @slip, @cRID, @unit )";

        private string updSql = "UPDATE D_OsPayment SET "
                                + "ReportDate = @rDat, OfficeCode = @oCod, Department = @dept, ItemCode = @iCod, Item = @item, OrderNo = @odNo, TaskCode = @tCod, "
                                + "OrderAmount = @oAmt, SAmount = @sAmt, Amount = @amt, LeaderMCode = @lCod, AdminCode = @aCod, "
                                + "AdminCheck = @aChk, DirectorCheck = @dChk, PresidentCheck = @pChk, "
                                + "ACheckDate = @aDat, DCheckDate = @dDat, PCheckDate = @pDat, SlipNo = @slip, CostReportID = @cRID, Unit = @unit "
                                + "WHERE OsPaymentID = @pMID";

        //private string selSql = "SELECT * FROM D_OsPayment WHERE ItemCode = @iCod AND TaskCode = @tCod AND OfficeCode = @oCod AND ReportDate = @rDat";
        private string delSql = "DELETE FROM D_OsPayment WHERE ";
        //private string delPartSql = "DELETE FROM D_OsPayment WHERE CostReportID = @crID";
        private string existSql = "SELECT * FROM D_OsPayment WHERE SlipNo = @slip";
        const string sqlSelID = ";SELECT CAST(SCOPE_IDENTITY() AS int)";

        //CostReportData crd;

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsPaymentData()
        {
        }

        public OsPaymentData(DataRow dr)
        {
            OsPaymentID = Convert.ToInt32(dr["OsPaymentID"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            ItemCode = Convert.ToString(dr["ItemCode"]);                // = CostCode
            Item = Convert.ToString(dr["Item"]);
            OrderNo = Convert.ToString(dr["OrderNo"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            OrderAmount = Convert.ToDecimal(dr["OrderAmount"]);
            SAmount = Convert.ToDecimal(dr["SAmount"]);
            Amount = Convert.ToDecimal(dr["Amount"]);
            LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
            AdminCode = Convert.ToString(dr["AdminCode"]);
            AdminCheck = Convert.ToInt32(dr["AdminCheck"]);
            DirectorCheck = Convert.ToInt32(dr["DirectorCheck"]);
            PresidentCheck = Convert.ToInt32(dr["PresidentCheck"]);
            ACheckDate = Convert.ToDateTime(dr["ACheckDate"]);
            DCheckDate = Convert.ToDateTime(dr["DCheckDate"]);
            PCheckDate = Convert.ToDateTime(dr["PCheckDate"]);
            SlipNo = Convert.ToInt32(dr["SlipNo"]);
            CostReportID = Convert.ToInt32(dr["CostReportID"]);
            Unit = Convert.ToString(dr["Unit"]);
            Publisher = OfficeCode + Department;
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsPaymentID { get; set; }
        public DateTime ReportDate { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public string OrderNo { get; set; }
        public string TaskCode { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal SAmount { get; set; }
        public decimal Amount { get; set; }
        public string LeaderMCode { get; set; }
        public string AdminCode { get; set; }
        public int AdminCheck { get; set; }
        public int DirectorCheck { get; set; }
        public int PresidentCheck { get; set; }
        public DateTime ACheckDate { get; set; }
        public DateTime DCheckDate { get; set; }
        public DateTime PCheckDate { get; set; }
        public int SlipNo { get; set; }
        public int CostReportID { get; set; }
        public string Unit { get; set; }
        public string Publisher { get; set; }
        public string TaskName { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            OsPaymentData cloneData = new OsPaymentData();
            cloneData.OsPaymentID = this.OsPaymentID;
            cloneData.ReportDate = this.ReportDate;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.ItemCode = this.ItemCode;
            cloneData.Item = this.Item;
            cloneData.OrderNo = this.OrderNo;
            cloneData.TaskCode = this.TaskCode;
            cloneData.OrderAmount = this.OrderAmount;
            cloneData.SAmount = this.SAmount;
            cloneData.Amount = this.Amount;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.AdminCode = this.AdminCode;
            cloneData.AdminCheck = this.AdminCheck;
            cloneData.DirectorCheck = this.DirectorCheck;
            cloneData.PresidentCheck = this.PresidentCheck;
            cloneData.ACheckDate = this.ACheckDate;
            cloneData.DCheckDate = this.DCheckDate;
            cloneData.PCheckDate = this.PCheckDate;
            cloneData.SlipNo = this.SlipNo;
            cloneData.CostReportID = this.CostReportID;
            cloneData.Unit = this.Unit;
            cloneData.Publisher = this.Publisher;
            cloneData.TaskName = this.TaskName;

            return cloneData;
        }


        public bool ExistenceSlipNo(int slipNo)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(existSql, conn);
                    cmd.Parameters.Add("@slip", SqlDbType.Int);
                    cmd.Parameters["@slip"].Value = slipNo;
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


        public OsPaymentData SelectPayment(int slipNo)
        {
            SqlHandling sh = new SqlHandling("D_OsPayment");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
            if (dt == null || dt.Rows.Count < 1) return null;
            OsPaymentData pmd = new OsPaymentData(dt.Rows[0]);
            return pmd;
        }



        public DataTable SelectPayment(DateTime reportDate, string officeCode, string department)
        {
            string sqlStr = " * FROM D_OsPayment "
                        + "WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND ReportDate = '" + reportDate + "'";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr); ;
            if (dt == null || dt.Rows.Count < 1) return null;
            return dt;
        }


        public decimal SelectSumAmountPayment(string iCod, string tCod, string oCod, string dept, DateTime rDat)
        {
            decimal Kei;

            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(" SUM(Amount) AS Kei FROM D_OsPayment WHERE ItemCode = '" + iCod + "' AND TaskCode = '" + tCod + "' AND OfficeCode = '" + oCod
                                                    + "' AND Department = '" + dept + "' AND ReportDate < '" + rDat + "'");
            if (dt == null || dt.Rows.Count == 0) return 0;
            DataRow dr = dt.Rows[0];
            Kei = (Convert.ToString(dr["Kei"]) == "") ? 0 : Convert.ToDecimal(dr["Kei"]);
            return Kei;
        }


        public decimal SelectOAmountPayment(string iCod, string tCod, string oCod, string dept, DateTime rDat)
        {
            decimal orderAmount;
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(" *  FROM D_OsPayment WHERE ItemCode = '" + iCod + "' AND TaskCode = '" + tCod + "' AND OfficeCode = '" + oCod
                                                    + "' AND Department = '" + dept + "' AND ReportDate < '" + rDat.StripTime() + "' ORDER BY ReportDate DESC");
            if (dt == null || dt.Rows.Count == 0) return 0;
            DataRow dr = dt.Rows[0];
            orderAmount = (Convert.ToString(dr["OrderAmount"]) == "") ? 0 : Convert.ToDecimal(dr["OrderAmount"]);
            return orderAmount;
        }


        public bool InsertPayment()
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql + sqlSelID, conn);
                    cmd = addParamSqlDbType(cmd);
                    cmd = addParamValue(cmd);

                    if ((OsPaymentID = TryExScalar(conn, cmd)) < 0) return false;
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


        //public bool InsertPayment(OsPaymentData opd)
        //{
        //    using (TransactionScope tran = new TransactionScope())
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(insSql + sqlSelID, conn);
        //            cmd = addParamSqlDbType(cmd);
        //            cmd = addParamValue(cmd, opd);
        //            //if (TryExecute(conn, cmd) < 0) return false;
        //            if ((OsPaymentID = TryExScalar(conn, cmd)) < 0) return false;
        //        }
        //        catch (SqlException sqle)
        //        {
        //            MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
        //            conn.Close();
        //            return false;
        //        }
        //        conn.Close();
        //        tran.Complete();
        //    }
        //    return true;
        //}


        //public bool UpdatePartPayment(int slipNo, decimal newAmount)
        //{
        //    OsPaymentData pmd = loadOsPayment(slipNo);
        //    pmd.Amount = newAmount;
        //    //return UpdatePayment(pmd);
        //    return UpdatePayment();
        //}

        
        //public bool UpdatePayment(OsPaymentData pmd)
        //{
        //    using (TransactionScope tran = new TransactionScope())
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(updSql, conn);
        //            cmd = addParamSqlDbType(cmd);
        //            cmd = addParamValue(cmd, pmd);
        //            if (TryExecute(conn, cmd) < 0) return false;
        //        }
        //        catch (SqlException sqle)
        //        {
        //            MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
        //            conn.Close();
        //            return false;
        //        }
        //        conn.Close();
        //        tran.Complete();
        //    }
        //    return true;
        //}
     

        public bool UpdatePayment()
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updSql, conn);

                    cmd = addParamSqlDbType(cmd);
                    cmd = addParamValue(cmd);

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


        public bool UpdatePaymentStatus( int paymentID, int iNo, int stat, DateTime ckDate )
        {
            string updStatSql = "";
            switch( iNo )
            {
                case 0:
                    updStatSql = "UPDATE D_OsPayment SET AdminCheck = @stat, ACheckDate = @ckDt WHERE OsPaymentID = @pmID";
                    break;
                case 1:
                    updStatSql = "UPDATE D_OsPayment SET DirectorCheck = @stat, DCheckDate = @ckDt WHERE OsPaymentID = @pmID";
                    break;
                case 2:
                    updStatSql = "UPDATE D_OsPayment SET PresidentCheck = @stat, PCheckDate = @ckDt WHERE OsPaymentID = @pmID";
                    break;
                default:
                    break;
            }

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( updStatSql, conn );
                    cmd.Parameters.Add( "@pmID", SqlDbType.Int );
                    cmd.Parameters.Add( "@stat", SqlDbType.Int );
                    cmd.Parameters.Add( "@ckDt", SqlDbType.Date );
                    cmd.Parameters["@pmID"].Value = paymentID;
                    cmd.Parameters["@stat"].Value = stat;
                    cmd.Parameters["@ckDt"].Value = ckDate;
                    if( TryExecute( conn, cmd ) < 0 ) return false;
                }
                catch( SqlException sqle )
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


        public bool DeletePayment(string para, int value)
        {
            if (para != "@slip" && para != "@pMID") return false;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string deleteSql;
                    deleteSql = (para == "@slip") ? (delSql + "SlipNo = @slip") : (delSql + "OsPaymentID = @pMID");

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

        private OsPaymentData loadOsPayment(int slipNo)
        {
            SqlHandling sh = new SqlHandling("D_OsPayment");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
            if (dt == null || dt.Rows.Count < 1) return null;
            OsPaymentData pmd = new OsPaymentData(dt.Rows[0]);
            return pmd;
        }


        private SqlCommand addParamSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pMID", SqlDbType.Int);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@odNo", SqlDbType.Char);
            cmd.Parameters.Add("@oAmt", SqlDbType.Decimal);
            cmd.Parameters.Add("@sAmt", SqlDbType.Decimal);
            cmd.Parameters.Add("@Amt", SqlDbType.Decimal);
            cmd.Parameters.Add("@lCod", SqlDbType.Char);
            cmd.Parameters.Add("@aCod", SqlDbType.Char);
            cmd.Parameters.Add("@aChk", SqlDbType.Int);
            cmd.Parameters.Add("@dChk", SqlDbType.Int);
            cmd.Parameters.Add("@pChk", SqlDbType.Int);
            cmd.Parameters.Add("@aDat", SqlDbType.Date);
            cmd.Parameters.Add("@dDat", SqlDbType.Date);
            cmd.Parameters.Add("@pDat", SqlDbType.Date);
            cmd.Parameters.Add("@slip", SqlDbType.Int);
            cmd.Parameters.Add("@cRID", SqlDbType.Int);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            return cmd;
        }


        //private SqlCommand addParamValue(SqlCommand cmd, OsPaymentData pmd)
        //{
        //    cmd.Parameters["@pMID"].Value = pmd.OsPaymentID;
        //    cmd.Parameters["@rDat"].Value = pmd.ReportDate;
        //    cmd.Parameters["@oCod"].Value = pmd.OfficeCode;
        //    cmd.Parameters["@dept"].Value = pmd.Department;
        //    cmd.Parameters["@iCod"].Value = pmd.ItemCode;
        //    cmd.Parameters["@item"].Value = pmd.Item;
        //    cmd.Parameters["@tCod"].Value = pmd.TaskCode;
        //    cmd.Parameters["@odNo"].Value = pmd.OrderNo;
        //    cmd.Parameters["@oAmt"].Value = pmd.OrderAmount;
        //    cmd.Parameters["@sAmt"].Value = pmd.SAmount;
        //    cmd.Parameters["@Amt"].Value = pmd.Amount;
        //    cmd.Parameters["@lCod"].Value = pmd.LeaderMCode;
        //    cmd.Parameters["@aCod"].Value = pmd.AdminCode;
        //    cmd.Parameters["@aChk"].Value = pmd.AdminCheck;
        //    cmd.Parameters["@dChk"].Value = pmd.DirectorCheck;
        //    cmd.Parameters["@pChk"].Value = pmd.PresidentCheck;
        //    cmd.Parameters["@aDat"].Value = pmd.ACheckDate;
        //    cmd.Parameters["@dDat"].Value = pmd.DCheckDate;
        //    cmd.Parameters["@pDat"].Value = pmd.PCheckDate;
        //    cmd.Parameters["@slip"].Value = pmd.SlipNo;
        //    cmd.Parameters["@cRID"].Value = pmd.CostReportID;
        //    return cmd;
        //}


        private SqlCommand addParamValue(SqlCommand cmd)
        {
            cmd.Parameters["@pMID"].Value = OsPaymentID;
            cmd.Parameters["@rDat"].Value = ReportDate;
            cmd.Parameters["@oCod"].Value = OfficeCode;
            cmd.Parameters["@dept"].Value = Department;
            cmd.Parameters["@iCod"].Value = ItemCode;
            cmd.Parameters["@item"].Value = Item;
            cmd.Parameters["@tCod"].Value = TaskCode;
            cmd.Parameters["@odNo"].Value = OrderNo;
            cmd.Parameters["@oAmt"].Value = OrderAmount;
            cmd.Parameters["@sAmt"].Value = SAmount;
            cmd.Parameters["@Amt"].Value = Amount;
            cmd.Parameters["@lCod"].Value = LeaderMCode;
            cmd.Parameters["@aCod"].Value = AdminCode;
            cmd.Parameters["@aChk"].Value = AdminCheck;
            cmd.Parameters["@dChk"].Value = DirectorCheck;
            cmd.Parameters["@pChk"].Value = PresidentCheck;
            cmd.Parameters["@aDat"].Value = ACheckDate;
            cmd.Parameters["@dDat"].Value = DCheckDate;
            cmd.Parameters["@pDat"].Value = PCheckDate;
            cmd.Parameters["@slip"].Value = SlipNo;
            cmd.Parameters["@cRID"].Value = CostReportID;
            cmd.Parameters["@unit"].Value = Unit;
            return cmd;
        }

    }
}

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
    public class OsPayOffData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string insSql = "INSERT INTO D_OsPayOff "
                                + "(ReportDate, OfficeCode, Department, ItemCode, Item, TaskCode, Cost, LeaderMCode, AdminCode, AdminCheck, DirectorCheck, PresidentCheck, "
                                + "ACheckDate, DCheckDate, PCheckDate, SlipNo, CostReportID, Unit, ContractForm, CloseInf) VALUES ("
                                + "@rDat, @oCod, @dept, @iCod, @item, @tCod, @cost, @lCod, @aCod, @aChk, @dChk, @pChk, "
                                + "@aDat, @dDat, @pDat, @slip, @cRID, @unit, @cFrm, @cInf )";

        private string updSql = "UPDATE D_OsPayOff SET "
                                + "ReportDate = @rDat, OfficeCode = @oCod, Department = @dept, ItemCode = @iCod, Item = @item, TaskCode = @tCod, Cost = @cost, "
                                + "LeaderMCode = @lCod, AdminCode = @aCod, AdminCheck = @aChk, DirectorCheck = @dChk, PresidentCheck = @pChk, "
                                + "ACheckDate = @aDat, DCheckDate = @dDat, PCheckDate = @pDat, SlipNo = @slip, CostReportID = @cRID, Unit = @unit, ContractForm = @cFrm, CloseInf = @cInf "
                                + "WHERE OsPayOffID = @pOID";

        private string existSql = "SELECT * FROM D_OsPayOff WHERE SlipNo = @slip";

        private string delSql = "DELETE FROM D_OsPayOff WHERE ";

        const string sqlSelID = ";SELECT CAST(SCOPE_IDENTITY() AS int)";

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsPayOffData()
        {
        }

        public OsPayOffData(DataRow dr)
        {
            OsPayOffID = Convert.ToInt32(dr["OsPayOffID"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            ItemCode = Convert.ToString(dr["ItemCode"]);                // = CostCode
            Item = Convert.ToString(dr["Item"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            Cost = Convert.ToDecimal(dr["Cost"]);
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
            ContractForm = (Convert.ToString(dr["ContractForm"]) == "") ? 1: Convert.ToInt32(dr["ContractForm"]);
            CloseInf = Convert.ToString(dr["CloseInf"]);
            Publisher = OfficeCode + Department;
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsPayOffID { get; set; }
        public DateTime ReportDate { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public string TaskCode { get; set; }
        public decimal Cost { get; set; }
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
        public int ContractForm { get; set; }
        public string CloseInf { get; set; }
        public string Publisher { get; set; }
        public string TaskName { get; set; }
        public string LeaderMName { get; set; }
        public string Customer { get; set; }
        public string ContTitle { get; set; }
        public string ReportCheck { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            OsPayOffData cloneData = new OsPayOffData();
            cloneData.OsPayOffID = this.OsPayOffID;
            cloneData.ReportDate = this.ReportDate;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.ItemCode = this.ItemCode;
            cloneData.Item = this.Item;
            cloneData.TaskCode = this.TaskCode;
            cloneData.Cost = this.Cost;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.AdminCheck = this.AdminCheck;
            cloneData.AdminCode = this.AdminCode;
            cloneData.DirectorCheck = this.DirectorCheck;
            cloneData.PresidentCheck = this.PresidentCheck;
            cloneData.ACheckDate = this.ACheckDate;
            cloneData.DCheckDate = this.DCheckDate;
            cloneData.PCheckDate = this.PCheckDate;
            cloneData.SlipNo = this.SlipNo;
            cloneData.CostReportID = this.CostReportID;
            cloneData.Unit = this.Unit;
            cloneData.ContractForm = this.ContractForm;
            cloneData.CloseInf = this.CloseInf;
            cloneData.Publisher = this.Publisher;
            cloneData.TaskName = this.TaskName;
            cloneData.LeaderMName = this.LeaderMName;
            cloneData.Customer = this.Customer;
            cloneData.ContTitle = this.ContTitle;
            cloneData.ReportCheck = this.ReportCheck;

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


        public OsPayOffData SelectOsPayOff(int slipNo)
        {
            SqlHandling sh = new SqlHandling("D_OsPayOff");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
            if (dt == null || dt.Rows.Count < 1) return null;
            OsPayOffData pod = new OsPayOffData(dt.Rows[0]);
            return pod;
        }


        public DataTable SelectPayOffItemCode(DateTime reportDate, string officeCode, string department)
        {
            string sqlStr = " DISTINCT ItemCode FROM D_OsPayOff "
                        + "WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND ReportDate = '" + reportDate + "' ORDER BY ItemCode";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr); ;
            if (dt == null || dt.Rows.Count < 1) return null;
            return dt;
        }


        public DataTable SelectPayOff(DateTime reportDate, string officeCode, string department, string costCode)
        {
            string sqlStr = " * FROM D_OsPayOff "
                        + "WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND ItemCode = '" + costCode + "' AND ReportDate = '" + reportDate + "'";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core(sqlStr); ;
            if (dt == null || dt.Rows.Count < 1) return null;
            return dt;
        }


        public bool InsertPayOff()
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

                    if ((OsPayOffID = TryExScalar(conn, cmd)) < 0) return false;
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


        public bool UpdatePayOff()
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


        public bool UpdatePayOffStatus(int payOffID, int iNo, int stat, DateTime ckDate)
        {
            string updStatSql = "";
            switch( iNo )
            {
                case 0:
                    updStatSql = "UPDATE D_OsPayOff SET AdminCheck = @stat, ACheckDate = @ckDt WHERE OsPayOffID = @pOID";
                    break;
                case 1:
                    updStatSql = "UPDATE D_OsPayOff SET DirectorCheck = @stat, DCheckDate = @ckDt WHERE OsPayOffID = @pOID";
                    break;
                case 2:
                    updStatSql = "UPDATE D_OsPayOff SET PresidentCheck = @stat, PCheckDate = @ckDt WHERE OsPayOffID = @pOID";
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
                    cmd.Parameters.Add( "@pOID", SqlDbType.Int );
                    cmd.Parameters.Add( "@stat", SqlDbType.Int );
                    cmd.Parameters.Add( "@ckDt", SqlDbType.Date );
                    cmd.Parameters["@pOID"].Value = payOffID;
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


        // 全レコードの削除
        public bool DeletePayOff(string para, int value)
        {
            if (para != "@slip" && para != "@pOID") return false;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    string deleteSql;
                    deleteSql = (para == "@slip") ? (delSql + "SlipNo = @slip") : (delSql + "OsPayOffID = @pOID");

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


        //private OsPayOffData loadOsPayOff(int slipNo)
        //{
        //    SqlHandling sh = new SqlHandling("D_OsPayOff");
        //    DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
        //    if (dt == null || dt.Rows.Count < 1) return null;
        //    OsPayOffData pod = new OsPayOffData(dt.Rows[0]);
        //    return pod;
        //}


        private SqlCommand addParamSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pOID", SqlDbType.Int);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@cost", SqlDbType.Decimal);
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
            cmd.Parameters.Add("@cFrm", SqlDbType.Int);
            cmd.Parameters.Add("@cInf", SqlDbType.NVarChar);
            return cmd;
        }


        //private SqlCommand addParamValue(SqlCommand cmd, OsPayOffData pod)
        //{
        //    cmd.Parameters["@pOID"].Value = pod.OsPayOffID;
        //    cmd.Parameters["@rDat"].Value = pod.ReportDate;
        //    cmd.Parameters["@oCod"].Value = pod.OfficeCode;
        //    cmd.Parameters["@dept"].Value = pod.Department;
        //    cmd.Parameters["@iCod"].Value = pod.ItemCode;
        //    cmd.Parameters["@item"].Value = pod.Item;
        //    cmd.Parameters["@tCod"].Value = pod.TaskCode;
        //    cmd.Parameters["@cost"].Value = pod.Cost;
        //    cmd.Parameters["@lCod"].Value = pod.LeaderMCode;
        //    cmd.Parameters["@aCod"].Value = pod.AdminCode;
        //    cmd.Parameters["@aChk"].Value = pod.AdminCheck;
        //    cmd.Parameters["@dChk"].Value = pod.DirectorCheck;
        //    cmd.Parameters["@pChk"].Value = pod.PresidentCheck;
        //    cmd.Parameters["@aDat"].Value = pod.ACheckDate;
        //    cmd.Parameters["@dDat"].Value = pod.DCheckDate;
        //    cmd.Parameters["@pDat"].Value = pod.PCheckDate;
        //    cmd.Parameters["@slip"].Value = pod.SlipNo;
        //    cmd.Parameters["@cRID"].Value = pod.CostReportID;
        //    return cmd;
        //}


        private SqlCommand addParamValue(SqlCommand cmd)
        {
            cmd.Parameters["@pOID"].Value = OsPayOffID;
            cmd.Parameters["@rDat"].Value = ReportDate;
            cmd.Parameters["@oCod"].Value = OfficeCode;
            cmd.Parameters["@dept"].Value = Department;
            cmd.Parameters["@iCod"].Value = ItemCode;
            cmd.Parameters["@item"].Value = Item;
            cmd.Parameters["@tCod"].Value = TaskCode;
            cmd.Parameters["@cost"].Value = Cost;
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
            cmd.Parameters["@cFrm"].Value = ContractForm;
            cmd.Parameters["@cInf"].Value = CloseInf;
            return cmd;
        }

    }
}

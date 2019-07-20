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
    public class OsWkReportData : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        // OsWorkReportOp
        private string tableName;
        const int subjCnt = 6;

        const string insSql = "INSERT INTO D_OsWkReport "
                    + "(PartnerCode, PartnerName, OfficeCode, Department, TaskCode, ReportDate, ContractForm, Note, Author, PNo, TotalP ) "
                    + "VALUES "
                    + "(@pCod, @pNam, @oCod, @dept, @tCod, @rDat, @cFrm, @note, @auth, @pNo, @totP )";
        const string seliDSql = ";SELECT CAST(SCOPE_IDENTITY() AS int)";


        const string updTaskCode = "UPDATE D_OsWkReport SET TaskCode = @tCod WHERE OsWkReportID = @oRID";

        const string delSql = "DELETE FROM D_OsWkReport WHERE ";

        CostReportData crd;

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsWkReportData()
        {
        }


        public OsWkReportData(string tableName)
        {
            this.tableName = tableName;
        }


        public OsWkReportData(DataRow dr)
        {
            OsWkReportID = Convert.ToInt32(dr["OsWkReportID"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            PartnerName = Convert.ToString(dr["PartnerName"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            ContractForm = Convert.ToInt32(dr["ContractForm"]);
            Note = Convert.ToString(dr["Note"]);
            Author = Convert.ToString(dr["Author"]);
            PNo = Convert.ToInt32(dr["PNo"]);
            TotalP = Convert.ToInt32(dr["TotalP"]);
            MemberCode = Convert.ToString(dr["MemberCode"]);
            LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
            SalesMCode = Convert.ToString(dr["SalesMCode"]);
            //CustoCode = Convert.ToString(dr["CustoCode"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsWkReportID { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string TaskCode { get; set; }
        public DateTime ReportDate { get; set; }
        public int ContractForm { get; set; }
        public string Note { get; set; }
        public string Author { get; set; }
        public int PNo { get; set; }
        public int TotalP { get; set; }
        public string MemberCode { get; set; }
        public string LeaderMCode { get; set; }
        public string SalesMCode { get; set; }
        public string CustoCode { get; set; }
        public string CoTaskCode { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public DataTable SelectOsWkReport(string partnerCode, string taskCode, DateTime reportMM)
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core("DISTINCT ReportDate, ContractForm FROM D_OsWkReport WHERE PartnerCode = '" + partnerCode + "' AND TaskCode = '" + taskCode
                                + "' AND ( ReportDate BETWEEN '" + reportMM.BeginOfMonth() + "' AND '" + reportMM.EndOfMonth() + "') ORDER BY ReportDate");
            if (dt == null || dt.Rows.Count < 1) return null;
            return dt;
        }


        public DataTable SelectOsWkReportPartnerCode( DateTime dateFr, string officeCode, string department )
        {
            DateTime dateTo = DHandling.EndOfMonth( dateFr );
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFull_Core( "DISTINCT PartnerCode FROM D_OsWkReport WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department
                                             + "' AND ( ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo + "') ORDER BY PartnerCode" ) ;
            if( dt == null || dt.Rows.Count < 1 ) return null;
            return dt;
        }


        public bool StoreOsWkReportAndDetail(OsWkDetailData[] wdtl)
        {
            crd = new CostReportData();

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql + seliDSql, conn);
                    cmd = addParaRep(cmd);
                    cmd = addValueRep(cmd);
                    if ((OsWkReportID = TryExScalar(conn, cmd)) < 0) return false;

                    cmd = new SqlCommand(OsWkDetailData.sqlInsDtl, conn);
                    cmd = OsWkDetailData.addCmdPara(cmd);
                    
                    for (int i = 0; i < wdtl.Length; i++)
                    {
                        wdtl[i].OsWkReportID = OsWkReportID;
                        cmd = OsWkDetailData.addValue(cmd, wdtl[i]);
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


        public bool UpdateOsWkReportTaskCode(int reportID, string dTaskCode)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updTaskCode, conn);

                    cmd.Parameters.Add("@oRID", SqlDbType.Int);
                    cmd.Parameters.Add("@tCod", SqlDbType.Char);
                    cmd.Parameters["@oRID"].Value = reportID;
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


        public bool DeleteOsWkReport(string para)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(delSql + para, conn);
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


        private SqlCommand addParaRep(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@pNam", SqlDbType.NVarChar);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@cFrm", SqlDbType.Int);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters.Add("@auth", SqlDbType.NVarChar);
            cmd.Parameters.Add("@pNo", SqlDbType.Int);
            cmd.Parameters.Add("@totP", SqlDbType.Int);
            return cmd;
        }


        private SqlCommand addValueRep(SqlCommand cmd)
        {
            cmd.Parameters["@pCod"].Value = PartnerCode;
            cmd.Parameters["@pNam"].Value = PartnerName;
            cmd.Parameters["@oCod"].Value = OfficeCode;
            cmd.Parameters["@dept"].Value = Department;
            cmd.Parameters["@tCod"].Value = TaskCode;
            cmd.Parameters["@rDat"].Value = ReportDate;
            cmd.Parameters["@cFrm"].Value = ContractForm;
            cmd.Parameters["@note"].Value = Note;
            cmd.Parameters["@auth"].Value = Author;
            cmd.Parameters["@pNo"].Value = PNo;
            cmd.Parameters["@totP"].Value = TotalP;

            return cmd;
        }

    }
}

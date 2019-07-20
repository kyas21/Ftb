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
    public class OsPayOffNoteData : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string insSql = "INSERT INTO D_OsPayOffNote "
                                + "(ReportDate, OfficeCode, Department, ItemCode, Note) VALUES ("
                                + "@rDat, @oCod, @dept, @iCod, @note)";

        private string updSql = "UPDATE D_OsPayOffNote SET "
                                + "ReportDate = @rDat, OfficeCode = @oCod, Department = @dept, ItemCode = @iCod, Note = @note "
                                + "WHERE OsPayOffNoteID = @pOID";


        private string delSql = "DELETE FROM D_OsPayOffNote WHERE ReportDate = @rDat AND OfficeCode = @oCod AND ItemCode = @iCod";

        const string sqlSelID = ";SELECT CAST(SCOPE_IDENTITY() AS int)";

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsPayOffNoteData()
        {
        }

        public OsPayOffNoteData(DataRow dr)
        {
            OsPayOffNoteID = Convert.ToInt32(dr["OsPayOffNoteID"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            ItemCode = Convert.ToString(dr["ItemCode"]);                // = CostCode
            Note = Convert.ToString(dr["Note"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsPayOffNoteID { get; set; }
        public DateTime ReportDate { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string ItemCode { get; set; }
        public string Note { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            OsPayOffNoteData cloneData = new OsPayOffNoteData();
            cloneData.OsPayOffNoteID = this.OsPayOffNoteID;
            cloneData.ReportDate = this.ReportDate;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.ItemCode = this.ItemCode;
            cloneData.Note = this.Note;

            return cloneData;
        }


        public OsPayOffNoteData SelectPayOffNote(DateTime reportDate, string officeCode, string itemCode)
        {
            SqlHandling sh = new SqlHandling("D_OsPayOffNote");
            DataTable dt = sh.SelectAllData("WHERE OfficeCode = '" + officeCode  + "' AND ItemCode = '" + itemCode + "' AND ReportDate = '" + reportDate + "'");
            if (dt == null || dt.Rows.Count < 1) return null;
            OsPayOffNoteData pod = new OsPayOffNoteData(dt.Rows[0]);
            return pod;
        }


        public bool InsertPayOffNote()
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

                    if ((OsPayOffNoteID = TryExScalar(conn, cmd)) < 0) return false;
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


        public bool UpdatePayOffNote()
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


        // レコードの削除
        public bool DeletePayOffNote()
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(delSql, conn);
                    cmd = addParamSqlDbType(cmd);
                    cmd.Parameters["@rDat"].Value = ReportDate;
                    cmd.Parameters["@oCod"].Value = OfficeCode;
                    cmd.Parameters["@iCod"].Value = ItemCode;
                    
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


        private SqlCommand addParamSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pOID", SqlDbType.Int);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            return cmd;
        }


        private SqlCommand addParamValue(SqlCommand cmd)
        {
            cmd.Parameters["@pOID"].Value = OsPayOffNoteID;
            cmd.Parameters["@rDat"].Value = ReportDate;
            cmd.Parameters["@oCod"].Value = OfficeCode;
            cmd.Parameters["@dept"].Value = Department;
            cmd.Parameters["@iCod"].Value = ItemCode;
            cmd.Parameters["@note"].Value = Note;
            return cmd;
        }

    }
}

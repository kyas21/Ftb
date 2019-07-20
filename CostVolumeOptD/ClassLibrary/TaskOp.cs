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
    public class TaskOp:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;
        const int subjCnt = 6;
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskOp()
        {
        }

        public TaskOp(string tableName)
        {
            this.tableName = tableName;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string[] MgrDept { get; set; }
        public string[] MgrName { get; set; }
        public string[] MbrDept { get; set; }
        public string[] MbrName { get; set; }
        public string[] AppName { get; set; }
        public DateTime[] AppDate { get; set; }
        //public int IssueMark { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public bool UpdateOldVerMark(string tableName, int taskID, int oldVerMark)
        {
            string sqlStr = "UPDATE " + tableName + " SET OldVerMark = @oVM WHERE TaskID = @tID";
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.Parameters.Add("@oVM", SqlDbType.Int);
                    cmd.Parameters.Add("@tID", SqlDbType.Int);
                    cmd.Parameters["@oVM"].Value = oldVerMark;
                    cmd.Parameters["@tID"].Value = taskID;
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

    }
}

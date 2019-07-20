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
    public class TaskNoteData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskNoteData()
        {
        }

        public TaskNoteData(DataRow dr)
        {
            TaskNoteID = Convert .ToInt32(dr["TaskNoteID"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            TaskID = Convert .ToInt32(dr["TaskID"]);
            LNo = Convert.ToInt32(dr["LNo"]);
            Note = Convert.ToString(dr["Note"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            OldVerMark = Convert.ToInt32(dr["OldVerMark"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int TaskNoteID { get; set; }
        public string TaskCode { get; set; }
        public int TaskID { get; set; }
        public int LNo { get; set; }
        public string Note { get; set; }
        public int VersionNo { get; set; }
        public int OldVerMark { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            TaskNoteData cloneData = new TaskNoteData();

            cloneData.TaskNoteID = this.TaskNoteID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.TaskID = this.TaskID;
            cloneData.LNo = this.LNo;
            cloneData.Note = this.Note;
            cloneData.VersionNo = this.VersionNo;
            cloneData.OldVerMark = this.OldVerMark;
            return cloneData;
        }


        public TaskNoteData SelectTaskNoteDate(int taskID)
        {
            SqlHandling sh = new SqlHandling("D_TaskNote");
            DataTable dt = sh.SelectAllData("WHERE TaskID = " + taskID);
            if (dt == null || dt.Rows.Count < 1) return null;
            TaskNoteData tnd = new TaskNoteData(dt.Rows[0]);
            return tnd;
        }



        public bool UpdateTaskNoteData()
        {
            string sqlStr = "UPDATE D_TaskNote SET "
                        + "LNo = @lNo, Note = @note, VersionNo = @vNo, OldVerMark = @oVM "
                        + "WHERE TaskID = @tID";
            if (!executeProcess(sqlStr)) return false;

            return true;
        }


        public bool InsertTaskNoteData()
        {
            string sqlStr = "INSERT INTO D_TaskNote ("
                        + "TaskCode, TaskID, LNo, Note, VersionNo, OldVerMark "
                        + " ) VALUES ("
                        + "@tCod, @tID, @lNo, @note, @vNo, @oVM )";

            if (!executeProcess(sqlStr)) return false;

            return true;
        }



        private bool executeProcess(string sqlStr)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = addSqlDbType(cmd);
                    cmd = addValue(cmd);

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


        private SqlCommand addSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@tID", SqlDbType.Int);
            cmd.Parameters.Add("@lNo", SqlDbType.Int);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters.Add("@vNo", SqlDbType.Int);
            cmd.Parameters.Add("@oVM", SqlDbType.Int);
            return cmd;
        }


        private SqlCommand addValue(SqlCommand cmd)
        {
            cmd.Parameters["@tCod"].Value = TaskCode;
            cmd.Parameters["@tID"].Value = TaskID;
            cmd.Parameters["@lNo"].Value = LNo;
            cmd.Parameters["@note"].Value = Note;
            cmd.Parameters["@vNo"].Value = VersionNo;
            cmd.Parameters["@oVM"].Value = OldVerMark;
            return cmd;
        }









    }
}

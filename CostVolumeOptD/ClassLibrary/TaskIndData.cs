using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class TaskIndData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string updateSql = "UPDATE D_TaskInd SET "
                        + "Contract = @cont, ProvMark = @pMrk, TaskID = @tID, AdminCode = @adCd, ConfirmDateA = @cDtA, LeaderMCode = @lMCd, "
                        + "ConfirmDateC = @cDtC, OfficeCode = @oCod, TaskName = @tNam, VersionNo = @vNo, OldVerMark = @oVM, Department = @dept, IssueMark = @isMk, OrdersType = @oTyp "
                        + " WHERE TaskIndID = @tIID";

        private string insertSql = "INSERT INTO D_TaskInd ("
                        + "TaskCode, Contract, ProvMark, TaskID, AdminCode, ConfirmDateA, LeaderMCode, ConfirmDateC, OfficeCode, "
                        + "TaskName, VersionNo, OldVerMark, Department, IssueMark, OrdersType "
                        + " ) VALUES ("
                        + "@tCod, @cont, @pMrk, @tID, @adCD, @cDtA, @lMCd, @cDtC, @oCod, @tNam, @vNo, @oVM, @dept, @isMk, @oTyp )";

        private string deleteSql = "DELETE FROM D_TaskInd WHERE TaskIndID = @tiID";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskIndData()
        {
        }

        public TaskIndData(DataRow dr)
        {
            TaskIndID = Convert.ToInt32(dr["TaskIndID"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);

            Contract = dr.Field<Decimal?>("Contract") ?? default(Decimal);
            ProvMark = dr.Field<Int32?>("ProvMark") ?? default(Int32);

            TaskID = Convert.ToInt32(dr["TaskID"]);
            AdminCode = Convert.ToString(dr["AdminCode"]);
            ConfirmDateA = dr.Field<DateTime?>("ConfirmDateA") ?? default(DateTime);
            LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
            ConfirmDateC = dr.Field<DateTime?>("ConfirmDateC") ?? default(DateTime);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            TaskName = Convert.ToString(dr["TaskName"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            OldVerMark = Convert.ToInt32(dr["OldVerMark"]);
            Department = Convert.ToString(dr["Department"]);
            IssueMark = dr.Field<Int32?>("IssueMark") ?? default(Int32);
            OrdersType = dr.Field<Int32?>("OrdersType") ?? default(Int32);
            ProcType = "";
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//

        public int TaskIndID { get; set; }
        public string TaskCode { get; set; }
        public decimal Contract { get; set; }
        public int ProvMark { get; set; }
        public int TaskID { get; set; }
        public string AdminCode { get; set; }
        public DateTime ConfirmDateA { get; set; }
        public string LeaderMCode { get; set; }
        public DateTime ConfirmDateC { get; set; }
        public string OfficeCode { get; set; }
        public string TaskName { get; set; }
        public int VersionNo { get; set; }
        public int OldVerMark { get; set; }
        public string Department { get; set; }
        public int IssueMark { get; set; }
        public int OrdersType { get; set; }
        public string SalesMCode { get; set; }
        public string PartnerCode { get; set; }
        public string ProcType { get; set; }


        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public object Clone()
        {
            TaskIndData cloneData = new TaskIndData();

            cloneData.TaskIndID = this.TaskIndID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.Contract = this.Contract;
            cloneData.ProvMark = this.ProvMark;
            cloneData.TaskID = this.TaskID;
            cloneData.AdminCode = this.AdminCode;
            cloneData.ConfirmDateA = this.ConfirmDateA;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.ConfirmDateC = this.ConfirmDateC;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.TaskName = this.TaskName;
            cloneData.VersionNo = this.VersionNo;
            cloneData.OldVerMark = this.OldVerMark;
            cloneData.Department = this.Department;
            cloneData.IssueMark = this.IssueMark;
            cloneData.OrdersType = this.OrdersType;

            cloneData.ProcType = this.ProcType;
            return cloneData;
        }


        public int CurrentVersionNo(int taskID)
        {
            string sqlStr = " MAX(VersionNo) AS MaxVer FROM D_TaskInd WHERE TaskID = " + taskID;
            return editTaskIndDataItem( sqlStr, "MaxVer" );
        }



        public int SelectTaskID(string taskCode)
        {
            string sqlStr = " TaskID FROM D_TaskInd WHERE OldVerMark = 0 AND TaskCode = '" + taskCode + "'";
            return editTaskIndDataItem( sqlStr, "TaskID" );
        }


        public int SelectTaskID(string taskCode, int verNo)
        {
            string sqlStr = " TaskID FROM D_TaskInd WHERE TaskCode = '" + taskCode + "' AND VersionNo = " + verNo;
            return editTaskIndDataItem( sqlStr, "TaskID" );
        }


        private int editTaskIndDataItem( string sqlStr, string retVal )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( sqlStr );
            if( dt == null || dt.Rows.Count < 1 ) return -1;

            DataRow dr = dt.Rows[0];
            return Convert.ToInt32( dr[retVal] );
        }


        public TaskIndData[] SelectTaskIndData(int taskID)
        {
            return editTaskIndDataArray( "WHERE TaskID = " + taskID );
        }


        public TaskIndData[] SelectTaskIndData(string officeCode, string department)
        {
            string sqlStr = "WHERE OldverMark = 0 AND IssueMark = 0 AND OfficeCode = '" + officeCode + "'";
            if( !string.IsNullOrEmpty( department ) ) sqlStr += " AND Department = '" + department + "'";

            return editTaskIndDataArray( sqlStr );
        }

        
        public TaskIndData[] SelectTaskIndDataByBaseCode(string taskBaseCode)
        {
            return editTaskIndDataArray( "WHERE TaskCode LIKE '_" + taskBaseCode + "'" );
        }


        private TaskIndData[] editTaskIndDataArray(string sqlStr )
        {
            SqlHandling sh = new SqlHandling( "D_TaskInd" );
            DataTable dt = sh.SelectAllData( sqlStr );
            if( dt == null ) return null;

            TaskIndData[] tid;
            tid = new TaskIndData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) tid[i] = new TaskIndData( dt.Rows[i] );
            return tid;
        }


        public TaskIndData SelectTaskIndData(string taskCode)
        {
            TaskIndData tid = new TaskIndData();
            SqlHandling sh = new SqlHandling("D_TaskInd");
            DataTable dt = sh.SelectAllData("WHERE TaskCode = '" + taskCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;
            tid = new TaskIndData(dt.Rows[0]);
            
            return tid;
        }


        public TaskIndData SelectTaskIndSData(string wParam)
        {
            TaskIndData tid;
            SqlHandling sh = new SqlHandling("D_TaskInd");
            DataTable dt = sh.SelectAllData(wParam);
            if (dt == null || dt.Rows.Count < 1) return null;
            tid = new TaskIndData(dt.Rows[0]);
            return tid;
        }


        public TaskIndData SelectInfoAboutTask( string taskCode )
        {
            TaskIndData tid = new TaskIndData();
            tid = SelectTaskIndData( taskCode );
            TaskData td = new TaskData();
            td = td.SelectTaskData( tid.TaskID );
            tid.SalesMCode = td.SalesMCode;
            tid.PartnerCode = td.PartnerCode;
            return tid;
        }

        public TaskIndData SelectTaskIndData( int taskID, string officeCode, string department )
        {
            TaskIndData tid = new TaskIndData();
            SqlHandling sh = new SqlHandling( "D_TaskInd" );
            DataTable dt = sh.SelectAllData( "WHERE TaskID = " + taskID + " AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "'" );
            if( dt == null || dt.Rows.Count < 1 ) return null;
            tid = new TaskIndData( dt.Rows[0] );

            return tid;
        }



        public bool UpdateOrInsertOrDeleteTaskIndData( TaskIndData[] tid )
        {
            if( !executeProcess( tid ) ) return false;
            return true;
        }

        public bool InsertTaskIndData(TaskIndData[] tid)
        {
            if (!executeProcess(insertSql, tid)) return false;
            return true;
        }


        private bool executeProcess(string sqlStr, TaskIndData[] tid)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = addSqlDbType(cmd);

                    for (int i = 0; i < tid.Length; i++)
                    {
                        if( tid[i].ProcType != "i" ) continue;
                        cmd = addValue(cmd,tid[i]);
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


        private bool executeProcess( TaskIndData[] tid )
        {
            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdu = new SqlCommand( updateSql, conn );
                    cmdu = addSqlDbType( cmdu );

                    SqlCommand cmdi = new SqlCommand( insertSql, conn );
                    cmdi = addSqlDbType( cmdi );

                    SqlCommand cmdd = new SqlCommand( deleteSql, conn );
                    cmdd.Parameters.Add( "@tIID", SqlDbType.Int );

                    for( int i = 0; i < tid.Length; i++ )
                    {
                        switch( tid[i].ProcType )
                        {
                            case "i":
                                cmdi = addValue( cmdi, tid[i] );                        // 新規作成
                                if( TryExecute( conn, cmdi ) < 0 ) return false;
                                break;
                            case "u":
                                cmdu = addValue( cmdu, tid[i] );                    // 更新
                                if( TryExecute( conn, cmdu ) < 0 ) return false;
                                break;
                            case "d":
                                cmdd.Parameters["@tIID"].Value = tid[i].TaskIndID;  // 削除
                                if( TryExecute( conn, cmdd ) < 0 ) return false;
                                break;
                            default:
                                break;
                        }
                    }
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


        private SqlCommand addSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@tIID", SqlDbType.Int);
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@cont", SqlDbType.Decimal);
            cmd.Parameters.Add("@pMrk", SqlDbType.Int);
            cmd.Parameters.Add("@tID", SqlDbType.Int);
            cmd.Parameters.Add("@adCD", SqlDbType.Char);
            cmd.Parameters.Add("@cDtA", SqlDbType.Date);
            cmd.Parameters.Add("@lMCd", SqlDbType.Char);
            cmd.Parameters.Add("@cDtC", SqlDbType.Date);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@tNam", SqlDbType.NVarChar);
            cmd.Parameters.Add("@vNo", SqlDbType.Int);
            cmd.Parameters.Add("@oVM", SqlDbType.Int);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@isMk", SqlDbType.Int);
            cmd.Parameters.Add("@oTyp", SqlDbType.Int);
            return cmd;
        }


        private SqlCommand addValue(SqlCommand cmd, TaskIndData tid)
        {
            cmd.Parameters["@tIID"].Value = tid.TaskIndID;
            cmd.Parameters["@tCod"].Value = tid.TaskCode;
            cmd.Parameters["@cont"].Value = tid.Contract;
            cmd.Parameters["@pMrk"].Value = tid.ProvMark;
            cmd.Parameters["@tID"].Value = tid.TaskID;
            cmd.Parameters["@adCD"].Value = tid.AdminCode;
            cmd.Parameters["@cDtA"].Value = tid.ConfirmDateA;
            cmd.Parameters["@lMCd"].Value = tid.LeaderMCode;
            cmd.Parameters["@cDtC"].Value = tid.ConfirmDateC;
            cmd.Parameters["@oCod"].Value = tid.OfficeCode;
            cmd.Parameters["@tNam"].Value = tid.TaskName;
            cmd.Parameters["@vNo"].Value = tid.VersionNo;
            cmd.Parameters["@oVM"].Value = tid.OldVerMark;
            cmd.Parameters["@dept"].Value = tid.Department;
            cmd.Parameters["@isMk"].Value = tid.IssueMark;
            cmd.Parameters["@oTyp"].Value = tid.OrdersType;
            return cmd;
        }

    }
}

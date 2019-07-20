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
    public class TaskEntryData :HumanProperty
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //private string insSql = "INSERT INTO D_TaskEntry(TaskName, TaskPlace, PartnerCode, OfficeCode, Department, CostType, TaskCode) "
        //                        + "VALUES (@tNam, @tPla, @pCod, @oCod, @dept, @cTyp, @tCod)"
        //                        + ";SELECT CAST(scope_identity() AS int)";

        private string insSql = "INSERT INTO D_TaskEntry(TaskName, TaskPlace, PartnerCode, OfficeCode, Department, CostType, TaskCode, "
                                + "LeaderMCode, SalesMCode, ContractDate, StartDate, EndDate, TaskID, TaskIndID ) "
                                + "VALUES (@tNam, @tPla, @pCod, @oCod, @dept, @cTyp, @tCod, "
                                + " @lCod, @sCod, @cDat, @sDat, @eDat, @tID, @tIID )"
                                + ";SELECT CAST(scope_identity() AS int)";

        private string updSql = "UPDATE D_TaskEntry SET TaskName = @tNam, TaskPlace = @tPla, PartnerCode = @pCod, OfficeCode = @oCod, "
                                + "Department = @dept, CostType = @cTyp, TaskCode = @tCod, LeaderMCode = @lCod, SalesMCode = @sCod, "
                                + "ContractDate = @cDat, StartDate = @sDat, EndDate = @eDat, TaskID = @tID, TaskIndID = @tIID "
                                + "WHERE TaskEntryID = @tEID";

        //private string selSql = "SELECT * FROM D_TaskEntry WHERE TaskEntryID = @tEID";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskEntryData()
        {
        }

        public TaskEntryData( DataRow dr )
        {
            TaskEntryID = Convert.ToInt32( dr["TaskEntryID"] );
            TaskName = Convert.ToString( dr["TaskName"] );
            TaskPlace = Convert.ToString( dr["TaskPlace"] );
            PartnerCode = Convert.ToString( dr["PartnerCode"] );
            OfficeCode = Convert.ToString( dr["OfficeCode"] );
            Department = Convert.ToString( dr["Department"] );
            CostType = Convert.ToString( dr["CostType"] );
            TaskCode = Convert.ToString( dr["TaskCode"] );
            LeaderMCode = Convert.ToString( dr["LeaderMCode"] );
            SalesMCode = Convert.ToString( dr["SalesMCode"] );
            ContractDate = dr.Field<DateTime?>( "ContractDate" ) ?? default( DateTime );
            StartDate = dr.Field<DateTime?>( "StartDate" ) ?? default( DateTime );
            EndDate = dr.Field<DateTime?>( "EndDate" ) ?? default( DateTime );
            TaskID = dr.Field<Int32?>( "TaskID" ) ?? default( Int32 );
            TaskIndID = dr.Field<Int32?>( "TaskIndID" ) ?? default( Int32 );
            //TaskID = Convert.ToInt32(dr["TaskID"]);
            //TaskIndID = Convert.ToInt32(dr["TaskIndID"]);

            PartnersData pd = new PartnersData();
            PartnerName = pd.SelectPartnerName( PartnerCode );
            Publisher = OfficeCode + Department;
            OfficeName = Conv.OfficeName( OfficeCode );
            DepartName = Conv.DepartName( OfficeCode, Department );
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int TaskEntryID { get; set; }
        public string TaskName { get; set; }
        public string TaskPlace { get; set; }
        public string PartnerCode { get; set; }
        //public string OfficeCode { get; set; }
        //public string Department { get; set; }
        public string CostType { get; set; }
        public string TaskCode { get; set; }
        public string LeaderMCode { get; set; }
        public string SalesMCode { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TaskID { get; set; }
        public int TaskIndID { get; set; }
        public string PartnerName { get; set; }
        public string Publisher { get; set; }
        //public string OfficeName { get; set; }
        //public string DepartName { get; set; }
        //public int EstimateID { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------// i
        public object Clone()
        {
            TaskEntryData cloneData = new TaskEntryData();
            cloneData.TaskEntryID = this.TaskEntryID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.TaskName = this.TaskName;
            cloneData.TaskPlace = this.TaskPlace;
            cloneData.CostType = this.CostType;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.SalesMCode = this.SalesMCode;
            cloneData.ContractDate = this.ContractDate;
            cloneData.StartDate = this.StartDate;
            cloneData.EndDate = this.EndDate;
            cloneData.TaskID = this.TaskID;
            cloneData.TaskIndID = this.TaskIndID;
            cloneData.Publisher = this.Publisher;
            cloneData.PartnerName = this.PartnerName;
            cloneData.OfficeName = this.OfficeName;
            cloneData.DepartName = this.DepartName;
            //cloneData.EstimateID = this.EstimateID;
            return cloneData;
        }


        public bool ExistenceTaskEntryData( string colName, string param, string val, string valType )
        {
            string selParam = "SELECT * FROM D_TaskEntry WHERE " + colName + " = " + param;


            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand( selParam, conn );
                    switch( valType )
                    {
                        case "Int":
                            cmd.Parameters.Add( param, SqlDbType.Int );
                            cmd.Parameters[param].Value = Convert.ToInt32( val );
                            break;
                        case "Char":
                            cmd.Parameters.Add( param, SqlDbType.Char );
                            cmd.Parameters[param].Value = val;
                            break;
                        default:
                            cmd.Parameters.Add( param, SqlDbType.Char );
                            cmd.Parameters[param].Value = val;
                            break;
                    }

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


        public TaskEntryData SelectTaskEntryData( int taskEntryID )
        {
            TaskEntryData ted = new TaskEntryData();
            SqlHandling sh = new SqlHandling( "D_TaskEntry" );
            DataTable dt = sh.SelectAllData( "WHERE TaskEntryID = " + taskEntryID );
            if( dt == null || dt.Rows.Count < 1 ) return null;
            ted = new TaskEntryData( dt.Rows[0] );
            return ted;
        }


        public TaskEntryData[] SelectTaskEntryData( string param )
        {
            SqlHandling sh = new SqlHandling( "D_TaskEntry" );
            DataTable dt = sh.SelectAllData( param );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            TaskEntryData[] tedA = new TaskEntryData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) tedA[i] = new TaskEntryData( dt.Rows[i] );
            return tedA;
        }


        public int InsertTaskEntryData( TaskEntryData ted )
        {
            if( ted == null ) return -1;

            Int32 newProdID = -1;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( insSql, conn );

                    cmd = addSqlDbType( cmd );
                    cmd = addValue( cmd, ted );
                    newProdID = TryExScalar( conn, cmd );
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            return ( int )newProdID;
        }


        public bool UpdateTaskEntryData( TaskEntryData ted )
        {
            if( ted == null ) return false;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( updSql, conn );

                    cmd = addSqlDbType( cmd );
                    cmd = addValue( cmd, ted );
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



        public bool EditTaskEntryData( TaskEntryData ted, TaskCodeNameData tcnd, int entryID )
        {
            ted.TaskEntryID = entryID;
            ted.TaskCode = tcnd.TaskCode;
            ted.TaskName = tcnd.TaskName;
            ted.CostType = "";
            ted.OfficeCode = tcnd.OfficeCode;
            ted.Department = tcnd.Department;
            TaskIndData tid = new TaskIndData();
            tid = tid.SelectTaskIndData( tcnd.TaskCode );
            ted.LeaderMCode = tid.LeaderMCode;
            TaskData td = new TaskData();
            td = td.SelectTaskData( tid.TaskID );
            ted.PartnerCode = td.PartnerCode;
            ted.SalesMCode = td.SalesMCode;
            ted.ContractDate = td.IssueDate;
            ted.StartDate = td.StartDate;
            ted.EndDate = td.EndDate;
            ted.TaskID = td.TaskID;
            ted.TaskIndID = tid.TaskIndID;
            PartnersData pd = new PartnersData();
            ted.PartnerName = pd.SelectPartnerName( td.PartnerCode );
            ted.TaskPlace = string.IsNullOrEmpty( td.TaskPlace ) ? "" : td.TaskPlace;
            return true;
        }

        //---------------------------------------------------------------------//
        // Private Define                                                      //
        //---------------------------------------------------------------------//
        private SqlCommand addSqlDbType( SqlCommand cmd )
        {
            cmd.Parameters.Add( "@tEID", SqlDbType.Int );
            cmd.Parameters.Add( "@tNam", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@tPla", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@pCod", SqlDbType.Char );
            cmd.Parameters.Add( "@oCod", SqlDbType.Char );
            cmd.Parameters.Add( "@dept", SqlDbType.Char );
            cmd.Parameters.Add( "@cTyp", SqlDbType.NVarChar );
            cmd.Parameters.Add( "@tCod", SqlDbType.Char );
            cmd.Parameters.Add( "@lCod", SqlDbType.Char );
            cmd.Parameters.Add( "@sCod", SqlDbType.Char );
            cmd.Parameters.Add( "@cDat", SqlDbType.Date );
            cmd.Parameters.Add( "@sDat", SqlDbType.Date );
            cmd.Parameters.Add( "@eDat", SqlDbType.Date );
            cmd.Parameters.Add( "@tID", SqlDbType.Int );
            cmd.Parameters.Add( "@tIID", SqlDbType.Int );
            return cmd;
        }

        private SqlCommand addValue( SqlCommand cmd )
        {
            cmd.Parameters["@tEID"].Value = TaskEntryID;
            cmd.Parameters["@tNam"].Value = TaskName;
            cmd.Parameters["@tPla"].Value = TaskPlace;
            cmd.Parameters["@pCod"].Value = PartnerCode;
            cmd.Parameters["@oCod"].Value = OfficeCode;
            cmd.Parameters["@dept"].Value = Department;
            cmd.Parameters["@cTyp"].Value = CostType;
            cmd.Parameters["@tCod"].Value = TaskCode;
            cmd.Parameters["@lCod"].Value = LeaderMCode;
            cmd.Parameters["@sCod"].Value = SalesMCode;
            cmd.Parameters["@cDat"].Value = ContractDate;
            cmd.Parameters["@sDat"].Value = StartDate;
            cmd.Parameters["@eDat"].Value = EndDate;
            cmd.Parameters["@tID"].Value = TaskID;
            cmd.Parameters["@tIID"].Value = TaskIndID;
            return cmd;
        }


        private SqlCommand addValue( SqlCommand cmd, TaskEntryData ted )
        {
            cmd.Parameters["@tEID"].Value = ted.TaskEntryID;
            cmd.Parameters["@tNam"].Value = ted.TaskName;
            cmd.Parameters["@tPla"].Value = ted.TaskPlace;
            cmd.Parameters["@pCod"].Value = ted.PartnerCode;
            cmd.Parameters["@oCod"].Value = ted.OfficeCode;
            cmd.Parameters["@dept"].Value = ted.Department;
            cmd.Parameters["@cTyp"].Value = ted.CostType;
            cmd.Parameters["@tCod"].Value = ted.TaskCode;
            cmd.Parameters["@lCod"].Value = ted.LeaderMCode;
            cmd.Parameters["@sCod"].Value = ted.SalesMCode;
            cmd.Parameters["@cDat"].Value = ted.ContractDate;
            cmd.Parameters["@sDat"].Value = ted.StartDate;
            cmd.Parameters["@eDat"].Value = ted.EndDate;
            cmd.Parameters["@tID"].Value = ted.TaskID;
            cmd.Parameters["@tIID"].Value = ted.TaskIndID;
            return cmd;
        }


    }
}

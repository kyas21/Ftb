using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace ClassLibrary
{
    public class DbAccess
    {
        //--------------------------------------//
        //     Field
        //--------------------------------------//
        //private string connectionString;

        //--------------------------------------//
        //     Constructor
        //--------------------------------------//
        public DbAccess()
        {
            // 本番環境とテスト環境は、以下をコメントアウトして切り替える
            //     本番 = SERVER_MA,  テスト = SERVER_PC
            //ConnectionString = "Data Source=SERVER-MA;Initial Catalog=CostVolumeDB;User ID=sa;Password=futaba#1";
            ConnectionString = "Data Source=SERVER-PC;Initial Catalog=CostVolumeDB;User ID=sa;Password=futaba#1";
            //ConnectionString = "Data Source=KsPC;Initial Catalog=CostVolumeDB;User ID=sa;Password=futaba#1";
        }


        //--------------------------------------//
        //     Property
        //--------------------------------------//
        public string ConnectionString { get; set; }

        //--------------------------------------//
        //     Method
        //--------------------------------------//
        //***** DB接続開始 *****/
        public SqlConnection DbOpen()
        {
            SqlConnection sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = ConnectionString;
                sqlConnection.Open();
                return sqlConnection;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "DB オープンエラー" );
                return null;
            }
        }


        //***** DB切断 *****/
        public bool DbClose( SqlConnection sqlConnection )
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "DB クローズエラー" );
                return false;
            }
        }


        //***** SQLを実行し結果をDataTableで返す *****/
        public DataTable GetDataTable( string sqlString )
        {
            SqlConnection sqlConnection = new SqlConnection();
            DataTable dataTable = new DataTable();
            SqlCommand sqlCommand = new SqlCommand();

            if ( ( sqlConnection = DbOpen() ) == null )
            {
                MessageBox.Show( "DB オープンエラー" );
                return null;
            }

            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = sqlString;
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter( sqlString, ConnectionString );
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill( dataTable );

                DbClose( sqlConnection );
                return dataTable;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "DB データテーブル取得エラー" );
                DbClose( sqlConnection );
                return null;
            }

        }
        //--------------------------------------------------------------------//
        //       共通
        //--------------------------------------------------------------------//
        public DataTable UsingSqlstring_Select( string sqlStr )
        {
            return SelectFull_Core( sqlStr );
        }


        public DataTable UsingParamater_Select( string tableName, string wParam )
        {
            return SelectAllData_Core( tableName, wParam );
        }


        public DataTable SelectAllData_Core( string tblNam, string wPar )
        {
            SqlHandling sh = new SqlHandling( tblNam );
            DataTable dt = sh.SelectAllData( wPar );
            if ( dt == null ) return null;
            //if ( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DataTable SelectFull_Core( string sqlStr )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( sqlStr );
            if ( dt == null ) return null;
            //if ( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DataTable CreateSchemaDataTable( SqlDataReader reader )
        {
            if ( reader == null ) return null;
            if ( reader.IsClosed ) return null;

            DataTable schema = reader.GetSchemaTable();
            DataTable dt = new DataTable();

            foreach ( DataRow row in schema.Rows )
            {
                // Column情報を追加してください。
                DataColumn col = new DataColumn();
                col.ColumnName = row["ColumnName"].ToString();
                col.DataType = Type.GetType( row["DataType"].ToString() );

                if ( col.DataType.Equals( typeof( string ) ) )
                    col.MaxLength = ( int )row["ColumnSize"];

                dt.Columns.Add( col );
            }
            return dt;
        }


        public DataTable UsingTryExReader( string sqlStr )
        {
            DataTable dt;
            using ( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( sqlStr, conn );

                    SqlDataReader dr = TryExReader( conn, cmd );
                    if ( dr == null ) return null;
                    dt = CreateSchemaDataTable( dr );
                }
                finally
                {
                    conn.Close();
                }
            }
            return dt;
        }


        public bool UsingTryExecute( string sqlStr )
        {
            using ( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    using ( SqlCommand cmd = new SqlCommand( sqlStr, conn ) )
                    {
                        if ( TryExecute( conn, cmd ) < 0 ) return false;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }


        public int TryExecute( SqlConnection conn, SqlCommand cmd )
        {
            Int32 rVal = -1;
            try
            {
                rVal = ( Int32 )cmd.ExecuteNonQuery();
            }
            catch ( SqlException sqle )
            {
                MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                conn.Close();
                return -1;
            }
            return ( int )rVal;
        }


        public int TryExScalar( SqlConnection conn, SqlCommand cmd )
        {
            Int32 rID = -1;
            try
            {
                rID = ( Int32 )cmd.ExecuteScalar();
            }
            catch ( SqlException sqle )
            {
                MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                conn.Close();
                return -1;
            }
            return ( int )rID;
        }


        public SqlDataReader TryExReader( SqlConnection conn, SqlCommand cmd )
        {
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
            }
            catch ( SqlException sqle )
            {
                MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                conn.Close();
                return null;
            }
            return dr;
        }

    }
}

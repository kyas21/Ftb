using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class CommonData :DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        const int switchMonth = 7;
        private string comUpdCLOSE = "UPDATE M_Common SET "
                                    + "ComData = @recentClosingDate, ComSignage = @currentFY, ComSymbol = @switchMonth, UpdateDate = @updateDate "
                                    + "WHERE Kind = @kind";
        private string idExistence = "SELECT * FROM M_Common WHERE Kind = 'LOGIN' AND ComData = @id";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public CommonData()
        {
        }


        public CommonData( DataRow dr )
        {
            CommonID = Convert.ToInt32( dr["CommonID"] );
            Kind = Convert.ToString( dr["Kind"] );
            ComData = Convert.ToString( dr["ComData"] );
            ComSignage = Convert.ToString( dr["ComSignage"] );
            ComSymbol = Convert.ToString( dr["ComSymbol"] );
            UsedNote = Convert.ToString( dr["UsedNote"] );
            UpdateDate = ( Convert.ToString( dr["UpdateDate"] ) == "" ) ? DateTime.Today.StripTime() : Convert.ToDateTime( dr["UpdateDate"] );
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int CommonID { get; set; }
        public string Kind { get; set; }
        public string ComData { get; set; }
        public string ComSignage { get; set; }
        public string ComSymbol { get; set; }
        public string UsedNote { get; set; }
        public DateTime UpdateDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public DataTable SelectCommonData( string comKind )
        {
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SelectAllData( " WHERE Kind = '" + comKind + "'" );
            if( dt == null || dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DateTime SelectCloseDate( string officeCode )
        {
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SelectAllData( " WHERE Kind = 'CLS" + officeCode + "'" );
            if( dt == null ) return DateTime.MinValue;
            DataRow dr = dt.Rows[0];
            ComSignage = Convert.ToString( dr["ComSignage"] );
            return Convert.ToDateTime( dr["ComData"] );
        }


        public string SelctDefaultMember( string program, string dataItem, string office, string depart )
        {
            DataTable dt = selectDefaultMember_Base( program, dataItem, office, depart );
            if( dt == null ) return null;
            DataRow dr = dt.Rows[0];
            return Convert.ToString( dr["ComSymbol"] );
        }


        public string[] SelctDefaultMemberAll( string program, string dataItem )
        {
            string sqlStr = " WHERE Kind = 'DEF_" + program + "' AND ComData = '" + dataItem + "'";
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SelectAllData( sqlStr );
            if( dt == null ) return null;
            string[] memberArray = new string[dt.Rows.Count];
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                memberArray[i] = Convert.ToString( dr["ComSymbol"] );
            }
            return memberArray;
        }


        public string[] SelctDefaultMemberMulti( string program, string dataItem, string office, string depart )
        {
            DataTable dt = selectDefaultMember_Base( program, dataItem, office, depart );
            if( dt == null ) return null;
            string[] memberArray = new string[dt.Rows.Count];
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                memberArray[i] = Convert.ToString( dr["ComSymbol"] );
            }
            return memberArray;
        }


        private DataTable selectDefaultMember_Base( string program, string dataItem, string office, string depart )
        {
            string sqlStr = " WHERE Kind = 'DEF_" + program + "' AND ComData = '" + dataItem + "' AND ComSignage ";
            if( depart == "LIKE" )
            {
                sqlStr += "LIKE '" + office + "%'";
            }
            else
            {
                sqlStr += "= '" + office;
                sqlStr += (string.IsNullOrEmpty(depart)) ? "'" : depart + "'";

            }
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SelectAllData( sqlStr );
            if( dt == null ) return null;
            return dt;
        }


        public string[] SelctOrderNote( string key )
        {
            string sqlStr = " WHERE ComData = '" + key + "'";
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SelectAllData( sqlStr );
            if( dt == null ) return null;
            string[] dVal = new string[dt.Rows.Count + 1];
            dVal[0] = "";
            DataRow dr;
            for(int i = 0; i < dt.Rows.Count;i++ )
            {
                dr = dt.Rows[i];
                dVal[i + 1] = Convert.ToString( dr["ComSignage"] );
            }
            return dVal;
        }

        public bool ExitstenceLogin( string idStr )
        {
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand( idExistence, conn );
                    cmd.Parameters.Add( "@id", SqlDbType.Char );
                    cmd.Parameters["@id"].Value = idStr;
                    SqlDataReader dr = TryExReader( conn, cmd );
                    if( !dr.HasRows ) return false;
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



        public bool UpdateCLOSEMonth( string officeCode, DateTime closeDate )
        {
            int currentFY;
            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand( comUpdCLOSE, conn );
                    cmd.Parameters.Add( "@recentClosingDate", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@currentFY", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@switchMonth", SqlDbType.NVarChar );
                    cmd.Parameters.Add( "@updateDate", SqlDbType.Date );
                    cmd.Parameters.Add( "@kind", SqlDbType.VarChar );

                    string[] ofArray;
                    if( officeCode == "A" )
                    {
                        ofArray = new string[] { "H", "K", "S", "T" };
                    }
                    else
                    {
                        ofArray = new string[1];
                        ofArray[0] = officeCode;
                    }

                    for( int i = 0; i < ofArray.Length; i++ )
                    {
                        cmd.Parameters["@kind"].Value = "CLS" + ofArray[i];

                        cmd.Parameters["@recentClosingDate"].Value = Convert.ToString( closeDate );
                        currentFY = closeDate.Year;
                        if( closeDate.Month < switchMonth ) currentFY--;
                        cmd.Parameters["@currentFY"].Value = Convert.ToString( currentFY );
                        cmd.Parameters["@switchMonth"].Value = switchMonth;
                        cmd.Parameters["@updateDate"].Value = DateTime.Today.StripTime();

                        if( TryExecute( conn, cmd ) < 0 ) return false;
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

    }
}

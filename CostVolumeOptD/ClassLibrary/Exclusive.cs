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
    public class Exclusive:DbAccess
    {

        //--------------------------------------//
        // Field
        //--------------------------------------//

        const string sqlInsert = "INSERT INTO M_Exclusive ( KeyWord, KeyItemName, Item, UserName ) VALUES ( @kWd, @kIN, @itm, @uNm)";
        const string sqlErase = "INSERT INTO M_Exclusive ( KeyWord, KeyItemName, Item, UserName ) VALUES ( @kWd, @kIN, @itm, @uNm )";
        const string sqlDelete = "DELETE FROM M_Exclusive WHERE KeyWord = @kWd AND KeyItemName = @kIN AND Item = @itm";
        const string sqlDeleteNoName = "DELETE FROM M_Exclusive WHERE KeyWord = @kWd AND Item = @itm";
        const string sqlDeleteKeyAndUser = "DELETE FROM M_Exclusive WHERE KeyWord = @kWd AND UserName = @uNm";
        const string sqlDeleteUser = "DELETE FROM M_Exclusive WHERE UserName = @uNm";
        //--------------------------------------//
        // Constructor
        //--------------------------------------//
        public Exclusive()
        {
        }

        public Exclusive( DataRow dr )
        {
            KeyWord = Convert.ToString( dr["KeyWord"] );
            KeyItemName = Convert.ToString( dr["KeyItemName"] );
            Item = Convert.ToString( dr["Item"] );
            UserName = Convert.ToString( dr["UserName"] );
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string KeyWord { get; set; }
        public string KeyItemName { get; set; }
        public string Item { get; set; }
        public string UserName { get; set; }

        //--------------------------------------//
        // Property
        //--------------------------------------//

        //--------------------------------------//
        // Method
        //--------------------------------------//
        /// <summary>
        /// 指定された項目が使用中か否かを確認する
        /// </summary>
        /// <returns> true:使用中、false:未使用 </returns>
        public bool CheckRegister()
        {
            if( string.IsNullOrEmpty( KeyItemName ) )
            {
                return CheckRegister( KeyWord, Item );
            }
            else
            {
                return CheckRegister( KeyWord, KeyItemName, Item );
            }
        }


        /// <summary>
        /// 指定された項目が使用中か否かを確認する
        /// </summary>
        /// <param name="keyWord"> プログラム名 </param>
        /// <param name="item"> 項目内容 </param>
        /// <returns> true:使用中、false:未使用 </returns>
        public bool CheckRegister( string keyWord, string item )
        {
            return selectExclusiveData( " WHERE KeyWord = '" + keyWord + "' AND Item = '" + item + "'" );
        }

        /// <summary>
        /// 指定された項目が使用中か否かを確認する
        /// </summary>
        /// <param name="keyWord"> プログラム名 </param>
        /// <param name="itemName"> キーとなる項目名 </param>
        /// <param name="item"> 項目内容 </param>
        /// <returns> true:使用中、false:未使用 </returns>
        public bool CheckRegister( string keyWord, string itemName, string item )
        {
            return selectExclusiveData( " WHERE KeyWord = '" + keyWord + "' AND KeyItemName = '" + itemName + "' AND Item = '" + item + "'" );
        }


        public bool CheckRegister( string keyWord, string itemName, string item,string uName )
        {
            return selectExclusiveData( " WHERE KeyWord = '" + keyWord + "' AND KeyItemName = '" + itemName + "' AND Item = '" + item + "' AND UserName = '" + uName + "'" );
        }

        
        public bool CheckRegisterUser( string keyWord, string item, string uName )
        {
            return selectExclusiveData( " WHERE KeyWord = '" + keyWord + "' AND Item = '" + item + "' AND UserName <> '" + uName + "'" );
        }

        public bool CheckRegisterSameUser( string keyWord, string item, string uName )
        {
            return selectExclusiveData( " WHERE KeyWord = '" + keyWord + "' AND Item = '" + item + "' AND UserName = '" + uName + "'" );
        }

        private bool selectExclusiveData( string sqlStr )
        {
            SqlHandling sh = new SqlHandling( "M_Exclusive" );
            DataTable dt = sh.SelectAllData( sqlStr );
            //return ( dt == null ) ? false : true;
            if( dt == null ) return false;
            DataRow dr = dt.Rows[0];
            UserName = Convert.ToString( dr["UserName"] );
            return true;
        }

        public bool CheckAndRegistered()
        {
            return CheckRegister() ? false: Register();
        }


        public bool CheckAndRegistered( string keyWord, string itemName, string item, string uName )
        {
            // 20170425 TRY kusano
            //if( CheckRegister( keyWord, itemName, item ) ) return false;
            //return Register( keyWord, itemName, item, uName );

            if( CheckRegister(keyWord,item) )
            {
                if( CheckRegisterSameUser( keyWord, item, uName ) ) return true;
                return false; 
            }
            return Register( keyWord, itemName, item, uName );
        }


        public bool CheckAndRegistered( string keyWord, string item )
        {
            if( CheckRegister( keyWord, item ) ) return false;
            return Register( keyWord, null, item, null );
        }


        public bool Register( )
        {
            return Register( KeyWord, KeyItemName, Item, UserName );
        }



        public bool Register( string keyWord, string itemName, string item, string uName )
        {
            if( string.IsNullOrEmpty( keyWord )||string.IsNullOrEmpty(item) ) return false;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand( sqlInsert, conn );
                    cmd.Parameters.Add( "@kWd", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@kIN", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@itm", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@uNm", SqlDbType.NVarChar );
                    cmd.Parameters["@kWd"].Value = keyWord;
                    cmd.Parameters["@kIN"].Value = ( string.IsNullOrEmpty( itemName ) ) ? "" : itemName;
                    cmd.Parameters["@itm"].Value = item;
                    cmd.Parameters["@uNm"].Value = ( string.IsNullOrEmpty( uName ) ) ? "" : uName;

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


        public bool Unregister( )
        {
            return Unregister( KeyWord, KeyItemName, Item );
        }


        public bool Unregister( string keyWord, string itemName, string item )
        {
            if( string.IsNullOrEmpty( keyWord ) || string.IsNullOrEmpty( item ) ) return false;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = string.IsNullOrEmpty(itemName) ? new SqlCommand(sqlDeleteNoName,conn): new SqlCommand( sqlDelete, conn );
                    cmd.Parameters.Add( "@kWd", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@itm", SqlDbType.VarChar );
                    cmd.Parameters["@kWd"].Value = keyWord;
                    cmd.Parameters["@itm"].Value = item;
                    if( !string.IsNullOrEmpty( itemName ))
                    {
                        cmd.Parameters.Add( "@kIN", SqlDbType.VarChar );
                        cmd.Parameters["@kIN"].Value =itemName;
                    }
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


        public bool Unregister( string keyWord, string uName )
        {
            if( string.IsNullOrEmpty( keyWord ) || string.IsNullOrEmpty( uName ) ) return false;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand( sqlDeleteKeyAndUser,conn );
                    cmd.Parameters.Add( "@kWd", SqlDbType.VarChar );
                    cmd.Parameters.Add( "@uNm", SqlDbType.VarChar );
                    cmd.Parameters["@kWd"].Value = keyWord;
                    cmd.Parameters["@uNm"].Value = uName;
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


        public bool Unregister( string uName )
        {
            if( string.IsNullOrEmpty( uName ) ) return false;

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand( sqlDeleteUser, conn );
                    cmd.Parameters.Add( "@uNm", SqlDbType.VarChar );
                    cmd.Parameters["@uNm"].Value = uName;
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
    }
}

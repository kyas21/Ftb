using System;
using System.Data;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class ComboBoxEdit
    {
        //--------------------------------------//
        //Field
        //--------------------------------------//
        private string[] valItem;
        private string[] dispItem;
        private ComboBox cBox;

        //--------------------------------------//
        //Constructor
        //--------------------------------------//
        public ComboBoxEdit()
        {
        }

        public ComboBoxEdit( ComboBox cBox )
        {
            this.cBox = cBox;
        }
        //--------------------------------------//
        //Property
        //--------------------------------------//
        public string[] ValueItem
        {
            set { valItem = value; }
            get { return valItem; }
        }


        public string[] DisplayItem
        {
            set { dispItem = value; }
            get { return dispItem; }
        }
        //--------------------------------------//
        // Method
        //--------------------------------------//
        public bool Basic()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add( "VM", typeof( string ) );
            dt.Columns.Add( "DM", typeof( string ) );

            if( valItem == null || valItem.Length != dispItem.Length )
            {
                valItem = new string[dispItem.Length];
                for( int i = 0; i < dispItem.Length; i++ ) valItem[i] = i.ToString();
            }

            for( int i = 0; i < dispItem.Length; i++ )
            {
                DataRow dr = dt.NewRow();
                dr["VM"] = valItem[i];
                dr["DM"] = dispItem[i];
                dt.Rows.Add( dr );
            }

            dt.AcceptChanges();
            cBox.DataSource = dt;
            cBox.ValueMember = "VM";      // 非表示
            cBox.DisplayMember = "DM";    // 表示
            cBox.SelectedIndex = 0;
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cBox.Text = "";

            return true;
        }

        public bool Basic( int selectPos )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add( "VM", typeof( string ) );
            dt.Columns.Add( "DM", typeof( string ) );

            if( valItem == null || valItem.Length != dispItem.Length )
            {
                valItem = new string[dispItem.Length];
                for( int i = selectPos; i < dispItem.Length; i++ ) valItem[i] = i.ToString();
            }

            for( int i = selectPos; i < dispItem.Length; i++ )
            {
                DataRow dr = dt.NewRow();
                dr["VM"] = valItem[i];
                dr["DM"] = dispItem[i];
                dt.Rows.Add( dr );
            }

            if( dt.Rows.Count != 0 )
            {
                dt.AcceptChanges();
                cBox.DataSource = dt;
                cBox.SelectedIndex = 0;
            }
            cBox.ValueMember = "VM";      // 非表示
            cBox.DisplayMember = "DM";    // 表示
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cBox.Text = "";

            return true;
        }


        public bool Version( string table, string param )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SelectAllData( param );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }

            //DataRow dr;
            //valItem = new string[dt.Rows.Count];
            //dispItem = new string[dt.Rows.Count];

            //for( int i = 0; i < dt.Rows.Count; i++ )
            //{
            //    dr = dt.Rows[i];
            //    valItem[i] = Convert.ToString( i );
            //    dispItem[i] = Convert.ToString( dr["VersionNo"] );
            //}
            if( SetValueDt( dt, "VersionNo" ) == false ) return false;
            return Basic();
        }


        public bool VersionDistinct( string table, string param )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SelectFullDescription( "DISTINCT VersionNo FROM " + table + " " + param );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }

            //DataRow dr;
            //valItem = new string[dt.Rows.Count];
            //dispItem = new string[dt.Rows.Count];

            //for( int i = 0; i < dt.Rows.Count; i++ )
            //{
            //    dr = dt.Rows[i];
            //    valItem[i] = Convert.ToString( i );
            //    dispItem[i] = Convert.ToString( dr["VersionNo"] );
            //}
            if( SetValueDt( dt, "VersionNo" ) == false ) return false;
            return Basic();
        }


        public bool MonthList( int startMonth )
        {
            int month;
            ValueItem = new string[12];
            DisplayItem = new string[12];
            for( int i = 0; i < 12; i++ )
            {
                month = startMonth + i;
                if( ( startMonth + i ) > 12 ) month -= 12;

                ValueItem[i] = month.ToString( "00" );
                DisplayItem[i] = month.ToString( "00" ) + "月";
            }
            return Basic();
        }


        public bool TableData( string table, string vItem, string dItem, int aLevel )
        {
            if( aLevel > 1111 ) aLevel = 1111;
            int[] lvlArray = new int[4];
            int[] paraArray = new int[] { 1000, 100, 10, 1 };
            int rCount = 0;
            for( int i = 0; i < lvlArray.Length; i++ )
            {
                lvlArray[i] = aLevel / paraArray[i];
                aLevel = aLevel % paraArray[i];
                rCount += lvlArray[i];
            }
            if( rCount == 0 )
            {
                emptyComboBox();
                return Basic();
            }

            valItem = new string[rCount];
            dispItem = new string[rCount];

            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SpecifiedData( vItem, dItem );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            DataRow dr;
            int idx = 0;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                if( lvlArray[i] == 1 )
                {
                    dr = dt.Rows[i];
                    valItem[idx] = Convert.ToString( dr[vItem] );
                    dispItem[idx] = Convert.ToString( dr[dItem] );
                    idx++;
                }
            }
            return Basic();
        }


        public bool TableData( string table, string vItem, string dItem )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SpecifiedData( vItem, dItem );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            DataRow dr;
            valItem = new string[dt.Rows.Count];
            dispItem = new string[dt.Rows.Count];

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                valItem[i] = Convert.ToString( dr[vItem] );
                dispItem[i] = Convert.ToString( dr[dItem] );
            }
            return Basic();
        }


        public bool TableData( string table, string vItem, string dItem, string wParam )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SpecifiedData( vItem, dItem, wParam );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            if( SetValueDt( dt, vItem, dItem ) == false ) return false;

            return Basic();
        }


        public bool TableData( string table, string vItem, string dItem, string sItem, string sCode, int aLevel )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SpecifiedData( vItem, dItem );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            if( SetValueDt( dt, vItem, dItem, sItem, sCode, aLevel ) == false ) return false;

            return Basic();
        }


        public bool TableData( string table, string vItem, string dItem, string sItem, string sCode, string wParam )
        {
            SqlHandling sh = new SqlHandling( table );
            DataTable dt = sh.SpecifiedData( vItem, dItem, wParam );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            if( SetValueDt( dt, vItem, dItem, sItem, sCode ) == false ) return false;

            return Basic();
        }

        public bool DepartmentList( string kind )
        {
            return commonDataCore( " WHERE Kind = '" + kind + "' ORDER BY ComSignage" );
        }

        public bool DepartmentList( string kind, int pos )
        {
            //return commonDataCorePosSelect(" WHERE Kind = '" + kind + "'",pos);
            return commonDataCorePosSelect( " WHERE Kind = '" + kind + "' ORDER BY ComSignage", pos );
        }


        public bool ComDataList( string kind )
        {
            return commonDataCore( " WHERE Kind = '" + kind + "'" );
        }


        public bool ComDataList( string kind, string usedNote )
        {
            string addStr = ( kind == "COST" ) ? " AND UsedNote LIKE '%" + usedNote + "%'" : " AND UsedNote = '" + usedNote + "'";
            return commonDataCore( " WHERE Kind = '" + kind + "'" + addStr );
        }


        private bool commonDataCore( string sqlStr )
        {
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SpecifiedData( "ComSymbol", "ComData", sqlStr );
            if( dt == null )
            {
                emptyComboBox();
                return Basic();
            }
            if( SetValueDt( dt ) == false ) return false;

            return Basic();
        }


        private bool commonDataCorePosSelect( string sqlStr, int selectPos )
        {
            SqlHandling sh = new SqlHandling( "M_Common" );
            DataTable dt = sh.SpecifiedData( "ComSymbol", "ComData", sqlStr );
            if( dt == null )
            {
                emptyComboBox();
                return Basic( selectPos );
            }
            if( SetValueDt( dt ) == false ) return false;

            return Basic( 1 );
        }


        public bool SetValueStep( int maxVal )
        {
            for( int i = 0; i < maxVal; i++ )
            {
                valItem[i] = i.ToString();
                dispItem[i] = i.ToString();
            }
            return true;
        }


        public bool SetValueDt( DataTable dt, string vItem, string dItem, string sItem, string sCode )
        {
            DataRow dr;
            valItem = new string[dt.Rows.Count + 1];
            dispItem = new string[dt.Rows.Count + 1];
            valItem[0] = sCode;
            dispItem[0] = sItem;

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                valItem[1 + i] = Convert.ToString( dr[vItem] ).TrimEnd();
                dispItem[1 + i] = Convert.ToString( dr[dItem] ).TrimEnd();
            }
            return true;
        }


        public bool SetValueDt( DataTable dt, string vItem, string dItem )
        {
            DataRow dr;
            valItem = new string[dt.Rows.Count];
            dispItem = new string[dt.Rows.Count];

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                valItem[i] = Convert.ToString( dr[vItem] );
                dispItem[i] = Convert.ToString( dr[dItem] );
            }
            return true;
        }


        public bool SetValueDt( DataTable dt, string dItem )
        {
            DataRow dr;
            valItem = new string[dt.Rows.Count];
            dispItem = new string[dt.Rows.Count];

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                valItem[i] = i.ToString();
                dispItem[i] = Convert.ToString( dr[dItem] );
            }
            return true;
        }


        public bool SetValueDt( DataTable dt )
        {
            DataRow dr;
            valItem = new string[dt.Rows.Count];
            dispItem = new string[dt.Rows.Count];

            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                valItem[i] = Convert.ToString( dr["ComSymbol"] );
                dispItem[i] = Convert.ToString( dr["ComData"] );
            }
            return true;
        }


        public bool SetValueDt( DataTable dt, string vItem, string dItem, string sItem, string sCode, int aLevel )
        {
            // 設定文字列をカンマ区切りとして配列に格納
            string[] sItemArray = sItem.Split( ',' );
            int RowsCount = ( sItem != "" ? sItemArray.Length : 0 );
            // アクセスコントロールをする場合は（a01）から（a99）をコメントアウト
            // （a01）
            valItem = new string[dt.Rows.Count + RowsCount];
            dispItem = new string[dt.Rows.Count + RowsCount];
            DataRow dr;

            for( int i = 0; i < dt.Rows.Count + RowsCount; i++ )
            {
                if( i < RowsCount )
                {
                    // 設定文字列
                    valItem[i] = sCode;
                    dispItem[i] = sItemArray[i];
                }
                else
                {
                    // レコード
                    dr = dt.Rows[i - RowsCount];
                    valItem[i] = Convert.ToString( dr[vItem] );
                    dispItem[i] = Convert.ToString( dr[dItem] );
                }
            }
            // （a99）

            // アクセス権限をコントロールする場合は、上記valItemの領域確保以降をコメントアウトし下記を生かす。

            //if( aLevel > 1111 ) aLevel = 1111;
            //int[] lvlArray = new int[4];
            //int[] paraArray = new int[] { 1000, 100, 10, 1 };
            //int rCount = 0;
            //for( int i = 0; i < lvlArray.Length; i++ )
            //{
            //    lvlArray[i] = aLevel / paraArray[i];
            //    aLevel = aLevel % paraArray[i];
            //    rCount += lvlArray[i];
            //}

            //valItem = new string[rCount + RowsCount];
            //dispItem = new string[rCount + RowsCount];
            //DataRow dr;
            //int idx = 0;
            //for( int i = 0; i < dt.Rows.Count + RowsCount; i++ )
            //{
            //    if( i < RowsCount )
            //    {
            //        // 設定文字列
            //        valItem[i] = sCode;
            //        dispItem[i] = sItemArray[i];
            //        idx++;
            //    }
            //    else
            //    {
            //        // レコード
            //        if( lvlArray[i - RowsCount] == 1 )
            //        {
            //            dr = dt.Rows[i - RowsCount];
            //            valItem[idx] = Convert.ToString( dr[vItem] );
            //            dispItem[idx] = Convert.ToString( dr[dItem] );
            //            idx++;
            //        }
            //    }
            //}
            return true;
        }


        private void emptyComboBox()
        {
            valItem = new string[1];
            dispItem = new string[1];
            valItem[0] = "";
            dispItem[0] = "";
        }


        public bool TableDataForCostData( string dItem, string strSql )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( strSql );
            if( dt == null ) return false;

            if( !SetValueDt( dt, dItem ) ) return false;
            return Basic();
        }


        public bool CreateEmptyComboBox()
        {
            emptyComboBox();
            return Basic();
        }


    }
    
}

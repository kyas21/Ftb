using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class Conv
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public static string OfficeCode { get; set; }
        public static string Office { get; set; }
        public static string DepartCode { get; set; }
        public static string Depart { get; set; }
        public static DateTime FrDate { get; set; }
        public static DateTime ToDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public static readonly List<string> oList = new List<string> { "H", "K", "S", "T" };
        public static readonly List<string> bList = new List<string> { "本社", "郡山", "相双", "関東" };
        public static readonly List<string> dList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "Z" };
        public static readonly List<string> dowList = new List<string> { "日", "月", "火", "水", "木", "金", "土" };
        public static readonly List<string> dNoList = new List<string> { "0", "1", "2", "7", "8", "9" };
        public static readonly List<string> dNmList = new List<string> { "総務営業", "設計", "測量", "調査", "技術", "総務" };
        public static readonly List<string> tdHList = new List<string> { "2", "1", "1", "7", "2", "1", "7", "0" };
        public static readonly List<string> tdBList = new List<string> { "8", "8", "8", "8", "8", "8", "8", "9" };
        public static readonly List<string> taskList = new List<string> { "A 測量", "B 設計", "C 施工管理", "D 地質調査", "E 家屋調査", "F 空撮", "G 工事" };
        public static readonly int FisicalYearStartMonth = 7;
        public static readonly int FisicalYearEndMonth = 6;
        public static readonly int StartMMDD = 701;
        public static readonly int EndMMDD = 630;

        public static readonly List<string> sdNoList = new List<string> { "01", "02", "04", "06", "08", "11", "12", "15", "17", "18" };
        public static readonly List<string> odCdList = new List<string> { "H2", "H1", "H7", "H0", "K8", "K9", "S8", "S9", "T8", "T9" };

        public static int OfficeCodeIndex( string officeCode )
        {
            return oList.IndexOf( officeCode );
        }


        public static int OfficeNameIndex( string officeName )
        {
            return bList.IndexOf( officeName );
        }


        public static int DepartmentCodeIndex( string depCode )
        {
            return dList.IndexOf( depCode );
        }

        public static int DepNoIndex( string depCode )
        {
            return dNoList.IndexOf( depCode );
        }


        public static string OfficeName( string officeCode )
        {
            int idx = OfficeCodeIndex( officeCode );
            if( idx < 0 ) return "";
            return bList[idx];
        }

        public static string DepartName( string officeCode, string department )
        {
            int idx = OfficeCodeIndex( officeCode );
            if( idx < 0 ) return "";
            idx = DepNoIndex( department );
            if( idx < 0 ) return "";
            return dNmList[idx];
        }


        public static string DepartToSyoDep( string officeCode, string depCode )
        {
            return sdNoList[odCdList.IndexOf( officeCode + depCode )];
        }


        public static List<string> AuthMembers( string authModule )
        {
            List<string> authList = new List<string>();
            SqlHandling sh = new SqlHandling( "M_Authorization" );
            DataTable dt = sh.SelectAllData( " WHERE ModuleName = '" + authModule + "'" );
            if( dt == null ) return null;
            DataRow dr = dt.Rows[0];
            var line = Convert.ToString( dr["AuthMember"] );
            var valArray = line.Split( ',' );
            foreach( string val in valArray )
            {
                authList.Add( val );
            }
            return authList;
        }


        public static string ResizeMemberCode( string code, int length )
        {
            StringUtility util = new StringUtility();
            char[] cArray;

            // 2018.06.03 asakawa １行修正
            // if ( code == null )
            if (code == null || code == "")
            {
                cArray = new char[length];
                for( int i = 0; i < length; i++ ) cArray[i] = '0';
                return code = new string( cArray );
            }
            cArray = code.ToCharArray();
            if( cArray.Length == length ) return code;

            for( int i = 0; i < cArray.Length; i++ )
            {
                if( cArray[i] < '0' || '9' < cArray[i] ) cArray[i] = '0';
            }

            code = new string( cArray );
            return code.Substring( cArray.Length - length, length );
        }


        public static string FormattingName( string fileName )
        {
            bool move = true;
            char[] cArray = fileName.ToCharArray();
            char[] dArray = new char[cArray.Length];
            for( int i = 0, j = 0; i < cArray.Length; i++ )
            {
                if( cArray[i] == '(' || cArray[i] == '（' ) move = false;
                dArray[i] = ' ';
                if( move )
                {
                    dArray[j] = cArray[i];
                    j++;
                }
                if( cArray[i] == ')' || cArray[i] == '）' || cArray[i] == '.' ) move = true;
            }
            fileName = new string( dArray );
            return fileName.TrimEnd();
        }


        public static bool OfficeAndDepart(ComboBox office, ComboBox depart )
        {
            OfficeCode = Convert.ToString( office.SelectedValue );
            Office = OfficeName( OfficeCode );
            DepartCode = Convert.ToString( depart.SelectedValue );
            Depart = DepartName( OfficeCode, DepartCode );
            return true;
        }


        public static bool OfficeAndDepartZ( ComboBox office, ComboBox depart )
        {
            OfficeAndDepart(office, depart);
            if (OfficeCode == Sign.HQOfficeCode )
            {
                if( DepartCode == "0" ) DepartCode = "2";
            }
            else
            {
                DepartCode = "8";
            }
            Depart = DepartName( OfficeCode, DepartCode );
            return true;
        }


        public static bool StartAndEnd(DateTimePicker dtp )
        {
            FrDate = dtp.Value.BeginOfMonth();
            ToDate = dtp.Value.EndOfMonth();
            return true;
        }

    }
}

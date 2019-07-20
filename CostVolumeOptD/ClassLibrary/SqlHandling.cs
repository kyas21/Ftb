using System;
using System.Data;

namespace ClassLibrary
{
    public class SqlHandling:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public SqlHandling()
        {
        }

        public SqlHandling(string tableName)
        {
            this.tableName = tableName;
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        //********** DB SELECT
        public int MaxValue(string item)
        {
            DataTable dt = GetDataTable("SELECT " + item + " FROM " + tableName + " ORDER BY " + item + " DESC");
            if (dt == null) return 0;
            if (dt.Rows.Count == 0) return 0;

            DataRow dr = dt.Rows[0];
            if (Convert.ToString(dr[item]) == "") return 0;

            return Convert.ToInt32(dr[item]);
        }


        public int MaxValue(string item, string wherepar)
        {
            DataTable dt = GetDataTable("SELECT " + item + " FROM " + tableName + " " + wherepar +" ORDER BY " + item + " DESC");
            if (dt == null) return 0;
            if (dt.Rows.Count == 0) return 0;

            DataRow dr = dt.Rows[0];
            if (Convert.ToString(dr[item]) == " ") return 0;

            return Convert.ToInt32(dr[item]);
        }

        public int MinValue(string item)
        {
            DataTable dt = GetDataTable("SELECT " + item + " FROM " + tableName + " ORDER BY " + item + " ASC");
            if (dt == null) return 0;
            if (dt.Rows.Count == 0) return 0;

            DataRow dr = dt.Rows[0];
            if (Convert.ToString(dr[item]) == "") return 0;

            return Convert.ToInt32(dr[item]);
        }

        public DataTable SelectAllData()
        {
            DataTable dt = GetDataTable("SELECT * FROM " + tableName);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }


        public DataTable SelectAllData(string wParam)
        {
            DataTable dt = GetDataTable("SELECT * FROM " + tableName + " " + wParam);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }


        public DataTable SpecifiedData(string dItem)
        {
            DataTable dt = GetDataTable("SELECT " + dItem + " FROM " + tableName + " ORDER BY " + dItem);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }


        public DataTable SpecifiedData(string vItem, string dItem)
        {
            DataTable dt = GetDataTable("SELECT " + vItem + "," + dItem + " FROM " + tableName);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }


        public DataTable SpecifiedData(string vItem, string dItem, string wParam)
        {
            DataTable dt = GetDataTable("SELECT " + vItem + "," + dItem + " FROM " + tableName + " " + wParam);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }


        public DataTable SelectFullDescription(string sqlStr)
        {
            DataTable dt = GetDataTable("SELECT " + sqlStr);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;

            return dt;
        }

        // 以下は使用しないこと
        public string SelectPartnerName(string pCode)
        {
            DataTable dt = GetDataTable("SELECT PartnerName FROM M_Partners WHERE PartnerCode = '" + pCode + "'");
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            return Convert.ToString(dr["PartnerName"]); 
        }


        public string SelectMemberName(string mCode)
        {
            DataTable dt = GetDataTable("SELECT Name FROM M_Members WHERE MemberCode = '" + mCode + "'");
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            return Convert.ToString(dr["Name"]);
        }
        

      

    }
}

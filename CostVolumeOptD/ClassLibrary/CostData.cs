using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CostData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public CostData()
        {
        }

        public CostData(DataRow dr)
        {
            CostID = Convert.ToInt32(dr["CostID"]);
            CostCode = Convert.ToString(dr["CostCode"]);
            Item = Convert.ToString(dr["Item"]);
            ItemDetail = Convert.ToString(dr["ItemDetail"]);
            Unit = Convert.ToString(dr["Unit"]);
            Cost = Convert.ToDecimal(dr["Cost"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            MemberCode = Convert.ToString(dr["MemberCode"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int CostID { get; set; }
        public string CostCode { get; set; }
        public string Item { get; set; }
        public string ItemDetail { get; set; }
        public string Unit { get; set; }
        public decimal Cost { get; set; }
        public string OfficeCode { get; set; }
        public string MemberCode { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            CostData cloneData = new CostData();

            cloneData.CostID = this.CostID;
            cloneData.CostCode = this.CostCode;
            cloneData.Item = this.Item;
            cloneData.ItemDetail = this.ItemDetail;
            cloneData.Unit = this.Unit;
            cloneData.Cost = this.Cost;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.MemberCode = this.MemberCode;

            return cloneData;
        }


        public CostData SelectCostMaster(string memberCode, string officeCode, string subject)
        {
            SqlHandling sh = new SqlHandling("M_Cost");
            DataTable dt = sh.SelectAllData("WHERE MemberCode = '" + memberCode + "' AND OfficeCode = '" + officeCode + "' AND CostCode LIKE '" + subject + "%'");
            if (dt == null || dt.Rows.Count < 1) return null;

            CostData cd = new CostData(dt.Rows[0]);
            return cd;
        }

        public CostData SelectCostMaster(string wParam)
        {
            SqlHandling sh = new SqlHandling("M_Cost");
            DataTable dt = sh.SelectAllData("WHERE " + wParam);
            if (dt == null || dt.Rows.Count < 1) return null;

            CostData cd = new CostData(dt.Rows[0]);
            return cd;
        }

        public CostData SelectCostMaster(string costCode, string officeCode)
        {
            SqlHandling sh= new SqlHandling("M_Cost");
            DataTable dt = sh.SelectAllData("WHERE CostCode = '" + costCode + "' AND OfficeCode = '" + officeCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;

            CostData cd = new CostData(dt.Rows[0]);
            return cd;
        }


        public string SelectCostName(string costCode)
        {
            SqlHandling sh = new SqlHandling("M_Cost");
            DataTable dt = sh.SelectAllData("WHERE CostCode = '" + costCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;
            DataRow dr = dt.Rows[0];
            return Convert.ToString(dr["Item"]);
        }
        //public decimal SelectCostData(string costCode, string officeCode)
        //{
        //    CostData cd = SelectCostMaster(costCode, officeCode);
        //    if (cd == null) return 0M;

        //    return cd.Cost;
        //}
    }
}

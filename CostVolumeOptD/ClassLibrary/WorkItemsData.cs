using System;
using System.Data;

namespace ClassLibrary
{
    public class WorkItemsData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public WorkItemsData()
        {
        }

        public WorkItemsData(DataRow dr)
        {
            ItemCode = Convert.ToString(dr["ItemCode"]);
            UItem = Convert.ToString(dr["UItem"]);
            Item = Convert.ToString(dr["Item"]);
            ItemDetail = Convert.ToString(dr["ItemDetail"]);
            Unit = Convert.ToString(dr["Unit"]);
            StdCost = Convert.ToDecimal(dr["StdCost"]);
            MemberCode = Convert.ToString(dr["MemberCode"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string ItemCode { get; set; }
        public string UItem { get; set; }
        public string Item { get; set; }
        public string ItemDetail { get; set; }
        public string Unit { get; set; }
        public decimal StdCost { get; set; }
        public string MemberCode { get; set; }
        public DateTime UpdateDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            WorkItemsData cloneData = new WorkItemsData();

            cloneData.ItemCode = this.ItemCode;
            cloneData.UItem = this.UItem;
            cloneData.Item = this.Item;
            cloneData.ItemDetail = this.ItemDetail;
            cloneData.Unit = this.Unit;
            cloneData.StdCost = this.StdCost;
            cloneData.MemberCode = this.MemberCode;

            return cloneData;
        }
    }
}

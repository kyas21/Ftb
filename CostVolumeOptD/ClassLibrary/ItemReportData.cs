using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ItemReportData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
       
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public ItemReportData()
        {
        }

        public ItemReportData(DataRow dr)
        {
            ItemReportID = Convert.ToInt32(dr["ItemReportID"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            Publisher = Convert.ToString(dr["Publisher"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            //ItemID = Convert.ToInt32(dr["ItemID"]);
            ItemCode = Convert.ToString(dr["ItemCode"]);
            Item = Convert.ToString(dr["Item"]);
            UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
            Quantity = Convert.ToDecimal(dr["Quantity"]);
            Cost = Convert.ToDecimal(dr["Cost"]);
            Unit = Convert.ToString(dr["Unit"]);
            Note = Convert.ToString(dr["Note"]);
            SlipNo = Convert.ToInt32(dr["SlipNo"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int ItemReportID { get; set; }
        public string TaskCode { get; set; }
        public string Publisher { get; set; }
        public DateTime ReportDate { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public int SlipNo { get; set; }
        
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
    }
}

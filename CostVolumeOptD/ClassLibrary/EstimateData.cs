using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class EstimateData:TaskEntryData,ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public EstimateData()
        {
        }

        public EstimateData(DataRow dr)
        {
            EstimateID = Convert.ToInt32(dr["EstimateID"]);
            //TaskEntryID = Convert.ToInt32(dr["TaskEntryID"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            Total = Convert.ToDecimal(dr["Total"]);
            Budgets = Convert.ToDecimal(dr["Budgets"]);
            MinimalBid = Convert.ToDecimal(dr["MinimalBid"]);
            Contract = Convert.ToDecimal(dr["Contract"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            Note = Convert.ToString(dr["Note"]);
            Publisher = OfficeCode + Department;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int EstimateID { get; set; }
        public int VersionNo { get; set; }
        public decimal Total { get; set; }
        public decimal Budgets { get; set; }
        public decimal MinimalBid { get; set; }
        public decimal Contract { get; set; }
        public string Note { get; set; }
        public new int TaskEntryID { get; set; }
        
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public new object Clone()
        {
            EstimateData cloneData = new EstimateData();
            cloneData.EstimateID = this.EstimateID;
            cloneData.TaskEntryID = this.TaskEntryID;
            cloneData.VersionNo = this.VersionNo;
            cloneData.Total = this.Total;
            cloneData.Budgets = this.Budgets;
            cloneData.MinimalBid = this.MinimalBid;
            cloneData.Contract = this.Contract;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.Note = this.Note;
            return cloneData;
        }

    }
}

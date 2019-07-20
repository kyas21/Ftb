using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class OsResultsData:OutsourceData,ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
      
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OsResultsData()
        {
        }

        public OsResultsData(DataRow dr)
        {
            OsResultsID = Convert.ToInt32(dr["OsResultsID"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            OrderNo = Convert.ToString(dr["OrderNo"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            PayRoule = Convert.ToInt32(dr["PayRoule"]);
            Amount = Convert.ToDecimal(dr["Amount"]);
            PublishDate = Convert.ToDateTime(dr["PublishDate"]);
            StartDate = Convert.ToDateTime(dr["StartDate"]);
            EndDate = Convert.ToDateTime(dr["EndDate"]);
            InspectDate = Convert.ToDateTime(dr["InspectDate"]);
            ReceiptDate = Convert.ToDateTime(dr["ReceiptDate"]);
            Place = Convert.ToString(dr["Place"]);
            Note = Convert.ToString(dr["Note"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            ContractForm = Convert.ToInt32(dr["ContractForm"]);
            RecordedDate = Convert.ToDateTime(dr["RecordedDate"]);
            TaskEntryID = Convert.ToInt32(dr["TaskEntryID"]);
            Publisher = OfficeCode + Department;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OsResultsID { get; set; }
        public DateTime PublishDate { get; set; }
        public int ContractForm { get; set; }
        public DateTime RecordedDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public new object Clone()
        {
            OsResultsData cloneData = new OsResultsData();
            cloneData.OsResultsID = this.OsResultsID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.VersionNo = this.VersionNo;
            cloneData.OrderNo = this.OrderNo;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.PayRoule = this.PayRoule;
            cloneData.Amount = this.Amount;
            cloneData.PublishDate = this.PublishDate;
            cloneData.StartDate = this.StartDate;
            cloneData.EndDate = this.EndDate;
            cloneData.InspectDate = this.InspectDate;
            cloneData.ReceiptDate = this.ReceiptDate;
            cloneData.Place = this.Place;
            cloneData.Note = this.Note;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.ContractForm = this.ContractForm;
            cloneData.RecordedDate = this.RecordedDate;
            cloneData.TaskEntryID = this.TaskEntryID;
            cloneData.Publisher = this.Publisher;
            cloneData.OutsourceID = this.OutsourceID;

            return cloneData;
        }
    }
}

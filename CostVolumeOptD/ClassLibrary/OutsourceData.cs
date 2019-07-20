using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class OutsourceData:PlanningData,ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OutsourceData()
        {
        }

        public OutsourceData(DataRow dr)
        {
            OutsourceID = Convert.ToInt32(dr["OutsourceID"]);
            TaskEntryID = Convert.ToInt32(dr["TaskEntryID"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            OrderNo = Convert.ToString(dr["OrderNo"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            PayRoule = Convert.ToInt32(dr["PayRoule"]);
            Amount = Convert.ToDecimal(dr["Amount"]);
            OrderDate = Convert.ToDateTime(dr["OrderDate"]);
            StartDate = Convert.ToDateTime(dr["StartDate"]);
            EndDate = Convert.ToDateTime(dr["EndDate"]);
            InspectDate = Convert.ToDateTime(dr["InspectDate"]);
            ReceiptDate = Convert.ToDateTime(dr["ReceiptDate"]);
            Place = Convert.ToString(dr["Place"]);
            Note = Convert.ToString(dr["Note"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            OrderFlag = Convert.ToInt32(dr["OrderFlag"]);
            Publisher = OfficeCode + Department;
        }

        public OutsourceData(int TaskEntryID,int PlanningID)
        {
            this.TaskEntryID = TaskEntryID;
            this.PlanningID = PlanningID; 
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int OutsourceID { get; set; }
        public string OrderNo { get; set; }
        public int PayRoule { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime InspectDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Place { get; set; }
        public string Note { get; set; }
        public int OrderFlag { get; set; }
       
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public new object Clone()
        {
            OutsourceData cloneData = new OutsourceData();
            cloneData.OutsourceID = this.OutsourceID;
            cloneData.TaskEntryID = this.TaskEntryID;
            cloneData.VersionNo = this.VersionNo;
            cloneData.OrderNo = this.OrderNo;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.PayRoule = this.PayRoule;
            cloneData.Amount = this.Amount;
            cloneData.OrderDate = this.OrderDate;
            cloneData.StartDate = this.StartDate;
            cloneData.EndDate = this.EndDate;
            cloneData.InspectDate = this.InspectDate;
            cloneData.ReceiptDate = this.ReceiptDate;
            cloneData.Place = this.Place;
            cloneData.Note = this.Note;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.OrderFlag = this.OrderFlag;
            cloneData.PlanningID = this.PlanningID;
            cloneData.Publisher = this.Publisher;
            return cloneData;
        }


        public DataTable SelectOutsource(int taskEntryID, string officeCode, string department )
        {
            string wParam = " WHERE TaskEntryID = " + taskEntryID + " AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "'";
            return SelectAllData_Core( "D_Outsource", wParam );
        }


        public DataTable SelectOutsourceCont( int outsourceID )
        {
            string wParam = "WHERE OutsourceID = " + outsourceID  + " ORDER BY LNo";
            return SelectAllData_Core( "D_OutsourceCont", wParam );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PublishData:TaskEntryData,ICloneable
    {

        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public PublishData()
        {
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Budgets { get; set; }
        public decimal MinBid { get; set; }
        public decimal Contract { get; set; }
        //public string Publisher { get; set; }
        public string OrderPartner { get; set; }
        public string LeaderName { get; set; }
        public string SalesMName { get; set; }
        public decimal Sales0 { get; set; }
        public decimal Sales1 { get; set; }
        public decimal Sales2 { get; set; }
        public decimal Tax0 { get; set; }
        public decimal Tax1 { get; set; }
        public decimal Tax2 { get; set; }
        public decimal Direct0 { get; set; }
        public decimal Direct1 { get; set; }
        public decimal Direct2 { get; set; }
        public decimal OutS0 { get; set; }
        public decimal OutS1 { get; set; }
        public decimal OutS2 { get; set; }
        public decimal Matel0 { get; set; }
        public decimal Matel1 { get; set; }
        public decimal Matel2 { get; set; }
        public DateTime RecordedDate { get; set; }
        public int Version { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderStartDate { get; set; }
        public DateTime OrderEndDate { get; set; }
        public DateTime InspectDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int PayRoule { get; set; }
        public string Place { get; set; }
        public int ContractForm { get; set; }
        public string vTaskCode { get; set; }
        public string vTaskName { get; set; }
        public string vStartDate { get; set; }
        public string vEndDate { get; set; }
        public string vSupplierName { get; set; }
        public string vOrdersForm { get; set; }
        public string vCarryOverPlanned { get; set; }
        public string vYearCompletionHigh { get; set; }
        public string vContact { get; set; }
        public string vClaimform { get; set; }
        public string vPayNote { get; set; }
        public string vYear { get; set; }
        public string vTaskStat { get; set; }
        public string vNote { get; set; }
        public string CostReportDate { get; set; }
        public string CostTypeData { get; set; }
        public string CostRange { get; set; }
        public string CostOffice { get; set; }
        public int PublishIndex { get; set; }
        public int PublishOffice { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

    }
}

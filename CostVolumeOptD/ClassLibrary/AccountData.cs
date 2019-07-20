using System;
using System.Data;

namespace ClassLibrary
{
    public class AccountData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        const string seliDSql = ";SELECT CAST(SCOPE_IDENTITY() AS int)";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public AccountData()
        {
        }

        public AccountData(DataRow dr)
        {
            AccountID = Convert.ToInt32(dr["AccountID"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            TaskCode = Convert.ToString(dr["TaskCode"]);
            CAmount = Convert.ToDecimal(dr["CAmount"]);
            RecordedDate = Convert.ToDateTime(dr["RecordedDate"]);
            Amount = Convert.ToDecimal(dr["Amount"]);
            SAmount = Convert.ToDecimal(dr["SAmount"]);
            InvoiceType = Convert.ToInt32(dr["InvoiceType"]);
            TaskEntryID = Convert.ToInt32(dr["TaskEntryID"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int AccountID { get; set; }
        public string PartnerCode { get; set; }
        public string TaskCode { get; set; }
        public decimal CAmount { get; set; }
        public DateTime RecordedDate { get; set; }
        public decimal Amount { get; set; }
        public decimal SAmount { get; set; }
        public int InvoiceType { get; set; }
        public int TaskEntryID { get; set; }
        public int EstimateID { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string Publisher { get; set; }
       
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            AccountData cloneData = new AccountData();
            cloneData.AccountID = this.AccountID;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.TaskCode = this.TaskCode;
            cloneData.CAmount = this.CAmount;
            cloneData.RecordedDate = this.RecordedDate;
            cloneData.Amount = this.Amount;
            cloneData.SAmount = this.SAmount;
            cloneData.InvoiceType = this.InvoiceType;
            cloneData.TaskEntryID = this.TaskEntryID;

            cloneData.EstimateID = this.EstimateID;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.Publisher = this.Publisher;
            return cloneData;
        }


        public AccountData SelectLastAccountData( int taskEntryID, DateTime recordedDate )
        {
            if( taskEntryID == 0 ) return null;
            string wParam = "WHERE TaskEntryID = " + taskEntryID + " AND RecordedDate < '" + recordedDate + "' ORDER BY RecordedDate ";
            DataTable dt = SelectAllData_Core( "D_Account", wParam );
            if( dt == null ) return null;
            if( dt.Rows.Count < 1 ) return null;

            AccountData acntd = new AccountData( dt.Rows[0] );
            return acntd;
        }


        public AccountData SelectAccountData( int accountID )
        {
            if( accountID == 0 ) return null;
            DataTable dt = SelectAllData_Core( "D_Account", "WHERE AccountID = " + accountID );
            if( dt == null ) return null;
            if( dt.Rows.Count < 1 ) return null;

            AccountData acntd = new AccountData( dt.Rows[0] );
            return acntd;
        }
    }
}

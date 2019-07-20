using System;
using System.Data;

namespace ClassLibrary
{
    public class PlanningData:TaskEntryData,ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public PlanningData()
        {
            PlanningID = 0;
            TaskEntryID = 0;
            VersionNo = 0;
            Sales = 0;
            Budgets = 0;
            MaxVersion = 0;
            EstimateID = 0;
            EstimateVer = 0;
        }

        public PlanningData(DataRow dr)
        {
            PlanningID = Convert.ToInt32(dr["PlanningID"]);
            //TaskEntryID = Convert.ToInt32(dr["TaskEntryID"]);
            VersionNo = Convert.ToInt32(dr["VersionNo"]);
            Sales = Convert.ToDecimal(dr["Sales"]);
            Budgets = Convert.ToDecimal(dr["Budgets"]);
            Discussion = Convert.ToString(dr["Discussion"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            CreateStat = dr.Field<Int32?>("CreateStat") ?? default(Int32);
            CreateMCd = Convert.ToString( dr["CreateMCd"] );
            CreateDate = dr.Field<DateTime?>("CreateDate") ?? DateTime.MinValue;
            ConfirmStat = dr.Field<Int32?>("ConfirmStat") ?? default(Int32);
            ConfirmMCd = Convert.ToString( dr["ConfirmMCd"] );
            ConfirmDate = dr.Field<DateTime?>("ConfirmDate") ?? DateTime.MinValue;
            ScreeningStat = dr.Field<Int32?>("ScreeningStat") ?? default(Int32);
            ScreeningMCd = Convert.ToString( dr["ScreeningMCd"] );
            ScreeningDate = dr.Field<DateTime?>("ScreeningDate") ?? DateTime.MinValue;
            ApOfficerStat = dr.Field<Int32?>("ApOfficerStat") ?? default(Int32);
            ApOfficerMCd = Convert.ToString( dr["ApOfficerMCd"] );
            ApOfficerDate = dr.Field<DateTime?>("ApOfficerDate") ?? DateTime.MinValue;
            ApPresidentStat = dr.Field<Int32?>("ApPresidentStat") ?? default(Int32);
            ApPresidentMCd = Convert.ToString( dr["ApPresidentMCd"] );
            ApPresidentDate = dr.Field<DateTime?>("ApPresidentDate") ?? DateTime.MinValue;
            ProxyStat = dr.Field<Int32?>("ProxyStat") ?? default(Int32);
            ProxyMCd = Convert.ToString( dr["ProxyMCd"] );
            ProxyDate = dr.Field<DateTime?>("ProxyDate") ?? DateTime.MinValue;

            Publisher = OfficeCode + Department;
            MaxVersion = 0;
            EstimateID = 0;
            EstimateVer = 0;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int PlanningID { get; set; }
        public int VersionNo { get; set; }
        public decimal Sales { get; set; }
        public decimal Budgets { get; set; }
        public int CreateStat { get; set; }
        public string Discussion { get; set; }
        public string CreateMCd { get; set; }
        public DateTime CreateDate { get; set; }
        public int ConfirmStat { get; set; }
        public string ConfirmMCd { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int ScreeningStat { get; set; }
        public string ScreeningMCd { get; set; }
        public DateTime ScreeningDate { get; set; }
        public int ApOfficerStat { get; set; }
        public string ApOfficerMCd { get; set; }
        public DateTime ApOfficerDate { get; set; }
        public int ApPresidentStat { get; set; }
        public string ApPresidentMCd { get; set; }
        public DateTime ApPresidentDate { get; set; }
        public int ProxyStat { get; set; }
        public string ProxyMCd { get; set; }
        public DateTime ProxyDate { get; set; }

        public int MaxVersion { get; set; }
        public int EstimateID { get; set; }
        public int EstimateVer { get; set; }
        public decimal Direct { get; set; }
        public decimal OutS { get; set; }
        public decimal Matel { get; set; }
        public decimal Sum { get; set; }
        public decimal Other { get; set; }
        public decimal AdmCost { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public new object Clone()
        {
            PlanningData cloneData = new PlanningData();
            cloneData.PlanningID = this.PlanningID;
            cloneData.TaskEntryID = this.TaskEntryID;
            cloneData.VersionNo = this.VersionNo;
            cloneData.Sales = this.Sales;
            cloneData.Budgets = this.Budgets;
            cloneData.Discussion = this.Discussion;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.CreateStat = this.CreateStat;
            cloneData.CreateMCd = this.CreateMCd;
            cloneData.CreateDate = this.CreateDate;
            cloneData.ConfirmStat = this.ConfirmStat;
            cloneData.ConfirmMCd = this.ConfirmMCd;
            cloneData.ConfirmDate = this.ConfirmDate;
            cloneData.ScreeningStat = this.ScreeningStat;
            cloneData.ScreeningMCd = this.ScreeningMCd;
            cloneData.ScreeningDate = this.ScreeningDate;
            cloneData.ApOfficerStat = this.ApOfficerStat;
            cloneData.ApOfficerMCd = this.ApOfficerMCd;
            cloneData.ApOfficerDate = this.ApOfficerDate;
            cloneData.ApPresidentStat = this.ApPresidentStat;
            cloneData.ApPresidentMCd = this.ApPresidentMCd;
            cloneData.ApPresidentDate = this.ApPresidentDate;
            cloneData.ProxyStat = this.ProxyStat;
            cloneData.ProxyMCd = this.ProxyMCd;
            cloneData.ProxyDate = this.ProxyDate;

            cloneData.Publisher = this.Publisher;
            cloneData.MaxVersion = this.MaxVersion;
            cloneData.EstimateID = this.EstimateID;
            cloneData.EstimateVer = this.EstimateVer;

            cloneData.Direct = this.Direct;
            cloneData.OutS = this.OutS;
            cloneData.Matel = this.Matel;
            cloneData.Sum = this.Sum;
            cloneData.Other = this.Other;
            cloneData.AdmCost = this.AdmCost;

            return cloneData;
        }


        public PlanningData LatestPlanningData( int taskEntryID )
        {
            if( taskEntryID == 0 ) return null;
            string wParam = "WHERE TaskEntryID = " + taskEntryID + " ORDER BY VersionNo DESC";
            DataTable dt = SelectAllData_Core( "D_Planning", wParam );
            if( dt == null ) return null;
            if( dt.Rows.Count < 1 ) return null;

            PlanningData pld = new PlanningData( dt.Rows[0] );
            return pld;
        }


        
    }
}

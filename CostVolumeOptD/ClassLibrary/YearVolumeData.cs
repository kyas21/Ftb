using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class YearVolumeData:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string sumPara = "SUM(Volume) AS sMV, SUM(VolUncomp) AS sVU, SUM(VolClaimRem) AS sVCR, SUM(VolClaim) AS sVC, "
                               + "SUM(Claim) AS sMC, SUM(VolPaid) AS sVP, SUM(BalanceClaim) AS sBC, SUM(BalanceIncom) AS sBI, "
                               + "SUM(Cost) AS sMCO, SUM(CarryOverPlanned) AS sCOP, SUM(Deposit1) AS sDP1, SUM(Deposit2) AS sDP2 FROM D_YearVolume WHERE ";


        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public YearVolumeData()
        {
        }

        public YearVolumeData( DataRow dr )
        {
            TaskCode = dr.Field<String>( "TaskCode" ) ?? "";
            OfficeCode = dr.Field<String>( "OfficeCode" ) ?? "";
            Department = dr.Field<String>( "Department" ) ?? "";
            YearMonth = dr.Field<Int32?>( "YearMonth" ) ?? default( Int32 );
            Volume = dr.Field<Decimal?>( "Volume" ) ?? null;
            VolUncomp = dr.Field<Decimal?>( "VolUncomp" ) ?? null;
            VolClaimRem = dr.Field<Decimal?>( "VolClaimRem" ) ?? null;
            VolClaim = dr.Field<Decimal?>( "VolClaim" ) ?? null;
            Claim = dr.Field<Decimal?>( "Claim" ) ?? null;
            VolPaid = dr.Field<Decimal?>( "VolPaid" ) ?? null;
            BalanceClaim = dr.Field<Decimal?>( "BalanceClaim" ) ?? null;
            BalanceIncom = dr.Field<Decimal?>( "BalanceIncom" ) ?? null;
            Deposit1 = dr.Field<Decimal?>( "Deposit1" ) ?? null;
            Cost = dr.Field<Decimal?>( "Cost" ) ?? null;
            ClaimDate = dr.Field<DateTime?>( "ClaimDate" ) ?? null;
            PaidDate = dr.Field<DateTime?>( "PaidDate" ) ?? null;
            Deposit2 = dr.Field<Decimal?>( "Deposit2" ) ?? null;
            CarryOverPlanned = dr.Field<Decimal?>( "CarryOverPlanned" ) ?? null;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int VolumeID { get; set; }
        public string TaskCode { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public int YearMonth { get; set; }
        public decimal? Volume { get; set; }
        public decimal? VolUncomp { get; set; }
        public decimal? VolClaimRem { get; set; }
        public decimal? VolClaim { get; set; }
        public decimal? Claim { get; set; }
        public decimal? VolPaid { get; set; }
        public decimal? BalanceClaim { get; set; }
        public decimal? BalanceIncom { get; set; }
        public decimal? Deposit1 { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? ClaimDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? Deposit2 { get; set; }
        public decimal? CarryOverPlanned { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public object Clone()
        {
            YearVolumeData cloneData = new YearVolumeData();
            cloneData.VolumeID = this.VolumeID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.YearMonth = this.YearMonth;
            cloneData.Volume = this.Volume;
            cloneData.VolUncomp = this.VolUncomp;
            cloneData.VolClaimRem = this.VolClaimRem;
            cloneData.VolClaim = this.VolClaim;
            cloneData.Claim = this.Claim;
            cloneData.VolPaid = this.VolPaid;
            cloneData.BalanceClaim = this.BalanceClaim;
            cloneData.BalanceIncom = this.BalanceIncom;
            cloneData.Deposit1 = this.Deposit1;
            cloneData.Cost = this.Cost;
            cloneData.ClaimDate = this.ClaimDate;
            cloneData.PaidDate = this.PaidDate;
            cloneData.Deposit2 = this.Deposit2;
            cloneData.CarryOverPlanned = this.CarryOverPlanned;

            return cloneData;
        }


        public YearVolumeData SelectSummaryYearVolume( string officeCode, string department, int yymm )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth = " + yymm );
            if( dt.Rows.Count == 0 ) return null;
            DataRow dr = dt.Rows[0];
            YearVolumeData yvold = setYearVolumeData( dr );

            yvold.OfficeCode = officeCode;
            yvold.Department = department;
            return yvold;
        }


        public YearVolumeData[] SelectYearVolume( string officeCode, string department, int yymm )
        {
            SqlHandling sh = new SqlHandling("D_YearVolume");
            DataTable dt = sh.SelectAllData( "WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth = " + yymm );
            if( dt == null || dt.Rows.Count == 0 ) return null;
            YearVolumeData[]  yVolA = new YearVolumeData[dt.Rows.Count];
            for(int i = 0;i<dt.Rows.Count;i++ )
            {
                yVolA[i] = new YearVolumeData(dt.Rows[i]);
            }
            return yVolA;
        }


        public YearVolumeData setYearVolumeData( DataRow dr )
        {
            YearVolumeData vol = new YearVolumeData();

            vol.Volume = ( Convert.ToString( dr["sMV"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sMV"] );
            vol.VolUncomp = ( Convert.ToString( dr["sVU"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sVU"] );
            vol.VolClaimRem = ( Convert.ToString( dr["sVCR"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sVCR"] );
            vol.VolClaim = ( Convert.ToString( dr["sVC"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sVC"] );
            vol.Claim = ( Convert.ToString( dr["sMC"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sMC"] );
            vol.VolPaid = ( Convert.ToString( dr["sVP"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sVP"] );
            vol.BalanceClaim = ( Convert.ToString( dr["sBC"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sBC"] );
            vol.BalanceIncom = ( Convert.ToString( dr["sBI"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sBI"] );
            vol.Cost = ( Convert.ToString( dr["sMCO"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sMCO"] );
            vol.CarryOverPlanned = ( Convert.ToString( dr["sCOP"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sCOP"] );
            vol.Deposit1 = ( Convert.ToString( dr["sDP1"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sDP1"] );
            vol.Deposit2 = ( Convert.ToString( dr["sDP2"] ) == "" ) ? 0M : Convert.ToDecimal( dr["sDP2"] );

            return vol;
        }
    }
}

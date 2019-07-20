using System;
using System.Data;

namespace ClassLibrary
{
    public class PartnersScData:PartnersData, ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public PartnersScData()
        {
        }

        public PartnersScData(DataRow dr)
        {
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            PartnerName = Convert.ToString(dr["PartnerName"]);
            PostCode = Convert.ToString(dr["PostCode"]);
            Address = Convert.ToString(dr["Address"]);
            TelNo = Convert.ToString(dr["TelNo"]);
            FaxNo = Convert.ToString(dr["FaxNo"]);
            AccountCode = Convert.ToString(dr["AccountCode"]);
            PartnerID = Convert.ToInt32(dr["PartnerID"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public object Clone()
        {
            PartnersScData cloneData = new PartnersScData();
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.PartnerName = this.PartnerName;
            cloneData.PostCode = this.PostCode;
            cloneData.Address = this.Address;
            cloneData.TelNo = this.TelNo;
            cloneData.FaxNo = this.FaxNo;
            cloneData.AccountCode = this.AccountCode;
            cloneData.PartnerID = this.PartnerID;
            return cloneData;
        }
    }
}

using System;
using System.Data;

namespace ClassLibrary
{
    public class MembersScData : MembersData, ICloneable
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public MembersScData()
        {
        }

        public MembersScData(DataRow dr)
        {
            MemberCode = Convert.ToString(dr["MemberCode"]);
            Name = Convert.ToString(dr["Name"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            AccessLevel = Convert.ToInt32(dr["AccessLevel"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//


        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            MembersScData cloneData = new MembersScData();
            cloneData.MemberCode = this.MemberCode;
            cloneData.Name = this.Name;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.AccessLevel = this.AccessLevel;
            return cloneData;
        }


    }
}

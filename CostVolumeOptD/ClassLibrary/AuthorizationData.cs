using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AuthorizationData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public AuthorizationData()
        {
        }

        public AuthorizationData(DataRow dr)
        {
            AuthID = Convert.ToInt32(dr["AuthID"]);
            ModuleLabel = Convert.ToString(dr["ModuleLabel"]);
            ModuleName = Convert.ToString(dr["ModuleName"]);
            AuthMember = Convert.ToString(dr["AuthMember"]);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int AuthID { get; set; }
        public string ModuleLabel { get; set; }
        public string ModuleName { get; set; }
        public string AuthMember { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            AuthorizationData cloneData = new AuthorizationData();

            cloneData.AuthID = this.AuthID;
            cloneData.ModuleLabel = this.ModuleLabel;
            cloneData.ModuleName = this.ModuleName;
            cloneData.AuthMember = this.AuthMember;

            return cloneData;
        }


        public DataTable SelectAuthMaster()
        {
            SqlHandling sh = new SqlHandling("M_Authorization");
            DataTable dt = sh.SelectAllData("ORDER BY AuthID");
            if (dt == null || dt.Rows.Count < 1) return null;
            return dt;
        }

        
    }
}

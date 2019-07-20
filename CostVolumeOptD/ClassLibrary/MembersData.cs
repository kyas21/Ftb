using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class MembersData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public MembersData()
        {
        }

        public MembersData( DataRow dr )
        {
            MemberCode = Convert.ToString( dr["MemberCode"] );
            Name = Convert.ToString( dr["Name"] );
            Phonetic = Convert.ToString( dr["Phonetic"] );
            OfficeCode = Convert.ToString( dr["OfficeCode"] );
            Department = Convert.ToString( dr["Department"] );
            BirthDate = Convert.ToDateTime( dr["BirthDate"] );
            PostCode = Convert.ToString( dr["PostCode"] );
            Address = Convert.ToString( dr["Address"] );
            PostCode2 = Convert.ToString( dr["PostCode2"] );
            Address2 = Convert.ToString( dr["Address2"] );
            TelNo = Convert.ToString( dr["TelNo"] );
            CellularNo = Convert.ToString( dr["CellularNo"] );
            CellularNo2 = Convert.ToString( dr["CellularNo2"] );
            EMail = Convert.ToString( dr["EMail"] );
            MobileEMail = Convert.ToString( dr["MobileEMail"] );
            BloodType = Convert.ToString( dr["BloodType"] );
            JoinDate = Convert.ToDateTime( dr["JoinDate"] );
            FinalEducation = Convert.ToString( dr["FinalEducation"] );
            GradDate = Convert.ToDateTime( dr["GradDate"] );
            BasicPNo = Convert.ToString( dr["BasicPNo"] );
            HealthInsNo = Convert.ToString( dr["HealthInsNo"] );
            EmploymentInsNo = Convert.ToString( dr["EmploymentInsNo"] );
            GainQDate = Convert.ToDateTime( dr["GainQDate"] );
            BankName = Convert.ToString( dr["BankName"] );
            BBranchName = Convert.ToString( dr["BBranchName"] );
            AccountType = Convert.ToInt32( dr["AccountType"] );
            AccountNo = Convert.ToString( dr["AccountNo"] );
            EContact = Convert.ToString( dr["EContact"] );
            RadiationMedical = Convert.ToDateTime( dr["RadiationMedical"] );
            MedicalCheckup = Convert.ToDateTime( dr["MedicalCheckup"] );
            FormWage = Convert.ToInt32( dr["FormWage"] );
            MemberType = Convert.ToInt32( dr["MemberType"] );
            AccessLevel = Convert.ToInt32( dr["AccessLevel"] );
            Enrollment = Convert.ToInt32( dr["Enrollment"] );
            Note = Convert.ToString( dr["Note"] );
            UpdateDate = Convert.ToDateTime( dr["UpdateDate"] );
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string Phonetic { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public DateTime BirthDate { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string PostCode2 { get; set; }
        public string Address2 { get; set; }
        public string TelNo { get; set; }
        public string CellularNo { get; set; }
        public string CellularNo2 { get; set; }
        public string EMail { get; set; }
        public string MobileEMail { get; set; }
        public string BloodType { get; set; }
        public DateTime JoinDate { get; set; }
        public string FinalEducation { get; set; }
        public DateTime GradDate { get; set; }
        public string BasicPNo { get; set; }
        public string HealthInsNo { get; set; }
        public string EmploymentInsNo { get; set; }
        public DateTime GainQDate { get; set; }
        public string BankName { get; set; }
        public string BBranchName { get; set; }
        public int AccountType { get; set; }
        public string AccountNo { get; set; }
        public string EContact { get; set; }
        public DateTime RadiationMedical { get; set; }
        public DateTime MedicalCheckup { get; set; }
        public int FormWage { get; set; }
        public int MemberType { get; set; }
        public int AccessLevel { get; set; }
        public int Enrollment { get; set; }
        public string Note { get; set; }
        public DateTime UpdateDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public string SelectMemberName( string mCode )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( " Name FROM M_Members WHERE MemberCode = '" + mCode + "'" );
            if( dt == null ) return "";
            if( dt.Rows.Count == 0 ) return "";
            DataRow dr = dt.Rows[0];
            return Convert.ToString( dr["Name"] );
        }


        public DataTable SelectMembersData( string officeCode, string department )
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( "WHERE Enrollment = 0 AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "'" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DataTable SelectMembersData( string officeCode )
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( "WHERE Enrollment = 0 AND OfficeCode = '" + officeCode + "'" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DataTable SelectWorkReportMembersData( string officeCode, string department )
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            // MemberType < 2 社員および作業内訳書入力作業員
            DataTable dt = sh.SelectAllData( "WHERE Enrollment = 0 AND MemberType < 2 AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "'" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public DataTable SelectMembersData()
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( "WHERE Enrollment = 0" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            return dt;
        }


        public string[] SelectMembersOffice( string mCode )
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectFullDescription( " OfficeCode, Department FROM M_Members WHERE MemberCode = '" + mCode + "'" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            string[] codeArray = new string[2];
            DataRow dr = dt.Rows[0];
            codeArray[0] = Convert.ToString( dr["OfficeCode"] );
            codeArray[1] = Convert.ToString( dr["Department"] );
            return codeArray;
        }


        public MembersData SelectMembersDataAll( string mCode )
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( "WHERE MemberCode = '" + mCode + "'" );
            if( dt == null ) return null;
            MembersData rmd = new MembersData( dt.Rows[0] );
            return rmd;
        }

        public MembersData[] SelectMembersDataAll()
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( "WHERE (MemberType = 0 OR MemberType = 3) AND Enrollment = 0 ORDER BY MemberCode" );
            if( dt == null ) return null;

            MembersData[] rmdA = new MembersData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) rmdA[i] = new MembersData( dt.Rows[i] );

            return rmdA;
        }
    }
}

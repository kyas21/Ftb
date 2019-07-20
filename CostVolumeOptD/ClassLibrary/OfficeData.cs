using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class OfficeData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public OfficeData()
        {
        }

        public OfficeData( DataRow dr )
        {
            OfficeCode = Convert.ToString( dr["OfficeCode"] );
            MemberCode = Convert.ToString( dr["MemberCode"] );
            MemberName = Convert.ToString( dr["MemberName"] );
            Title = Convert.ToString( dr["Title"] );
            PostCode = Convert.ToString( dr["PostCode"] );
            Address = Convert.ToString( dr["Address"] );
            TelNo = Convert.ToString( dr["TelNo"] );
            FaxNo = Convert.ToString( dr["FaxNo"] );
            OrderSeqNo = Convert.ToInt32( dr["OrderSeqNo"] );
            OrderLastNo = Convert.ToInt32( dr["OrderLastNo"] );
            PurchaseSeqNo = Convert.ToInt32( dr["PurchaseSeqNo"] );
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string OfficeCode { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string Title { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public int OrderSeqNo { get; set; }
        public int OrderLastNo { get; set; }
        public int PurchaseSeqNo { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public OfficeData SelectOfficeData( string officeCode )
        {
            SqlHandling sh = new SqlHandling( "M_Office" );
            DataTable dt = sh.SelectAllData( "WHERE OfficeCode = '" + officeCode + "'" );
            if( dt == null ) return null;
            if( dt.Rows.Count == 0 ) return null;
            OfficeData od = new OfficeData( dt.Rows[0] );
            return od;
        }
    }
}

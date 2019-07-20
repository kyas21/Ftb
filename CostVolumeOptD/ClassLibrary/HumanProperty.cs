using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class HumanProperty : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public string OfficeName { get; set; }
        public string DepartName { get; set; }
        public int MemberType { get; set; }
        public int AccessLevel { get; set; }
        public string Password { get; set; }
        public decimal TaxRate { get; set; }
        public decimal AdminCostRate { get; set; }
        public decimal OthersCostRate { get; set; }
        public decimal Expenses { get; set; }
        public int Enrollment { get; set; }
        public DateTime ClosingDate { get; set; }
        public int ClosingMonth { get; set; }
        public DateTime CloseHDate { get; set; }
        public DateTime CloseKDate { get; set; }
        public DateTime CloseSDate { get; set; }
        public DateTime CloseTDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        //public object Clone()
        //{
        //    HumanProperty cloneData = new HumanProperty();
        //    cloneData.MemberCode = this.MemberCode;
        //    cloneData.MemberName = this.MemberName;
        //    cloneData.OfficeCode = this.OfficeCode;
        //    cloneData.Department = this.Department;
        //    cloneData.OfficeName = this.OfficeName;
        //    cloneData.DepartName = this.DepartName;
        //    cloneData.MemberType = this.MemberType;
        //    cloneData.AccessLevel = this.AccessLevel;
        //    cloneData.Password = this.Password;
        //    cloneData.TaxRate = this.TaxRate;
        //    cloneData.AdminCostRate = this.AdminCostRate;
        //    cloneData.OthersCostRate = this.OthersCostRate;
        //    cloneData.Expenses = this.Expenses;
        //    cloneData.Enrollment = this.Enrollment;
        //    cloneData.ClosingDate = this.ClosingDate;
        //    cloneData.ClosingMonth = this.ClosingMonth;
        //    cloneData.CloseHDate = this.CloseHDate;
        //    cloneData.CloseKDate = this.CloseKDate;
        //    cloneData.CloseSDate = this.CloseSDate;
        //    cloneData.CloseTDate = this.CloseTDate;
        //    return cloneData;
        //}
        
    }
}

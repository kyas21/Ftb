using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CostInfoIFData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        public string[] VItemArray0 = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public string[] DItemArray0 = new string[] { "指定なし", "原価計上日", "業務番号", "顧客", "作業項目", "業務担当者", "営業担当者" };

        public string[] VItemArray1 = new string[] { "0", "1", "2" };
        public string[] DItemArray1 = new string[] { "指定なし", "業務番号", "作業項目" };

        public string[] TItemArray = new string[] { "empty", "ReportDate", "TaskCode", "CustoCode", "ItemCode", "LeaderMCode", "SalesMCode" };
        public string[] DITableArray = new string[] { "empty", "empty", "D_Task", "M_Partners", "M_Cost", "M_Members", "M_Members" };
        public string[] DItmKeyArray = new string[] { "empty", "ReportDate", "TaskCode", "PartnerCode", "CostCode", "MemberCode", "MemberCode" };
        public string[] DItmNamArray = new string[] { "empty", "empty", "TaskName", "PartnerName", "Item", "Name", "Name" };
        public string[] TItemArray1 = new string[] { "empty", "TaskCode", "ItemCode" };

        string[] hTextArray1 = new string[] { "（業務番号）業務", "（コード）作業項目" };
        string[] hTextArray2 = new string[] { "（コード）作業項目", "（業務番号）業務" };
        string[] nTextArray1 = new string[] { "Task", "Item" };
        string[] nTextArray2 = new string[] { "Item", "Task" };

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public DateTime DateSOP { get; set; }
        public DateTime DateEOP { get; set; }
        public string Class0 { get; set; }
        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Item0 { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string ItemFR0 { get; set; }
        public string ItemTO0 { get; set; }
        public string ItemFR1 { get; set; }
        public string ItemTO1 { get; set; }
        public string ItemFR2 { get; set; }
        public string ItemTO2 { get; set; }
        public DateTime DateFR0 { get; set; }
        public DateTime DateTO0 { get; set; }
        public DateTime DateFR1 { get; set; }
        public DateTime DateTO1 { get; set; }
        public DateTime DateFR2 { get; set; }
        public DateTime DateTO2 { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public string[] EditColumnHeaderArray(string class1)
        {
            string[] headerArray;

            switch (class1)
            {
                case "1":
                    headerArray = new string[] { "（業務番号）業務", "（コード）作業項目" };
                    break;
                case "2":
                    headerArray = new string[] {  "（コード）作業項目" , "（業務番号）業務" };
                    break;
                default:
                    headerArray = new string[] { "（業務番号）業務", "（コード）作業項目" };
                    break;
            }

            return headerArray;
        }

        public string[] EditColumnNameArray(string class1)
        {
            string[] nameArray;

            switch (class1)
            {
                case "1":
                    nameArray = new string[] { "Task", "Item" };
                    break;
                case "2":
                    nameArray = new string[] { "Item", "Task" };
                    break;
                default:
                    nameArray = new string[] { "Task", "Item" };
                    break;
            }

            return nameArray;
        }

        public string EditODERBYPhrase(string class0, string class1)
        {
            if (class0 == "0" && class1 == "0") return "";

            string obPhrase = "ORDER BY "  ;
            if (class0 != "0") obPhrase += TItemArray[Convert.ToInt32(class0)] + " ASC";
            if (class1 != "0") obPhrase += " " + TItemArray1[Convert.ToInt32(class1)] + " ASC";

            return obPhrase;
        }

    }
}

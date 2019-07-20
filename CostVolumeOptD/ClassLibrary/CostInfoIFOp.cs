/*
 * Change history
 * 
 * 20190518 anonymous 出力範囲に部門追加対応
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CostInfoIFOp
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        // 作成方法、分類項目左
        public string[] VItemArray0 = new string[] { "0", "1", "2", "3", "4", "5", "6" };
        public string[] DItemArray0 = new string[] { "指定なし", "原価計上日", "業務番号", "得意先", "原価項目", "業務担当者", "営業担当者" };
        // 作成方法、分類項目右
        public string[] VItemArray1 = new string[] { "0", "2", "4" };
        public string[] DItemArray1 = new string[] { "指定なし", "業務番号", "原価項目" };
        public string[] TItemArray1 = new string[] { "empty", "TaskCode", "ItemCode" };

        // 出力範囲、項目1～3
        public string[] VItemArray2 = new string[] { "0", "1", "2", "3", "4", "5" };
        public string[] DItemArray2 = new string[] { "指定なし", "業務番号", "得意先", "原価項目", "業務担当者", "営業担当者" };

        public string[] TItemArray = new string[] { "empty", "TaskCode", "CustoCode", "ItemCode", "LeaderMCode", "SalesMCode" };
        public string[] DITableArray = new string[] { "empty", "D_TaskInd", "M_Partners", "M_Cost", "M_Members", "M_Members" };
        public string[] DItmKeyArray = new string[] { "empty", "TaskCode", "PartnerCode", "CostCode", "MemberCode", "MemberCode" };
        public string[] DItmNamArray = new string[] { "empty", "TaskName", "PartnerName", "Item", "Name", "Name" };

        string[] hTextArray1 = new string[] { "（業務番号）業務", "（コード）原価項目" };
        string[] hTextArray2 = new string[] { "（コード）原価項目", "（業務番号）業務" };
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
        public string WherePhraseDate { get; set; }
        public string WherePhrase0 { get; set; }
        public string WherePhrase1 { get; set; }
        public string WherePhrase2 { get; set; }
        public string OrderPhrase0 { get; set; }
        public string OrderPhrase1 { get; set; }
        public string ClassificationItem { get; set; }
        public string OutputRange { get; set; }
        public string Office { get; set; }
        public string SqlStr { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public string[] EditColumnHeaderArray(string class0, string class1)
        {
            string[] headerArray;

            switch (class1)
            {
                case "2":
                    if (class0 == "4")
                    {
                        headerArray = new string[] { "（コード）原価項目", "（業務番号）業務" };
                    }
                    else
                    {
                        headerArray = new string[] { "（業務番号）業務", "（コード）原価項目" };
                    }
                    break;
                case "4":
                    headerArray = new string[] { "（コード）原価項目", "（業務番号）業務" };
                    break;
                default:
                    headerArray = new string[] { "（業務番号）業務", "（コード）原価項目" };
                    break;
            }

            return headerArray;
        }

        public string[] EditColumnNameArray(string class0, string class1)
        {
            string[] nameArray;

            switch (class1)
            {
                case "2":
                    if (class0 == "4")
                    {
                        nameArray = new string[] { "Item", "Task" };
                    }
                    else
                    {
                        nameArray = new string[] { "Task", "Item" };
                    }
                    break;
                case "4":
                    nameArray = new string[] { "Item", "Task" };
                    break;
                default:
                    nameArray = new string[] { "Task", "Item" };
                    break;
            }

            return nameArray;
        }

        public string[] EditColumnHeaderArraySummary(string class0, string class1, string class2)
        {
            string[] headerArray;

            // Wakamatsu 20170316
            //headerArray = new string[] { "（業務番号）業務", "（コード）原価項目" };
            //switch (class0)
            //{
            //    case "3":
            //        if (class1 == "1")
            //        {
            //            if (class2 == "4")
            //            {
            //                headerArray = new string[] { "（コード）原価項目", "（業務番号）業務" };
            //            }
            //        }
            //        break;
            //    default:
            //        headerArray = new string[] { "（業務番号）業務", "（コード）原価項目" };
            //        break;
            //}

            headerArray = new string[3] { "", "", "" };
            string[] TargetArray = new string[3] { class0, class1, class2 };
            for (int i = 0; i < headerArray.Length; i++)
            {
                switch (TargetArray[i])
                {
                    case "1":
                        headerArray[i] = "（業務番号）業務";
                        break;
                    case "2":
                        headerArray[i] = "得意先";
                        break;
                    case "3":
                        headerArray[i] = "（原価コード）原価項目";
                        break;
                    case "4":
                        headerArray[i] = "業務担当者";
                        break;
                    case "5":
                        headerArray[i] = "営業担当者";
                        break;
                    default:
                        headerArray[i] = "";
                        break;
                }
            }
            // Wakamatsu 20170316

            return headerArray;
        }


        public string[] EditColumnNameArraySummary(string class0, string class1, string class2)
        {
            string[] nameArray;

            nameArray = new string[] { "Task", "Item" };
            switch (class0)
            {
                case "3":
                    if (class1 == "1")
                    {
                        if (class2 == "4")
                        {
                            nameArray = new string[] { "Item", "Task" };
                        }
                    }
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

        // 20190519 anonymous add
        public void RangeArraySet(string officecode)
        {
            if (officecode == "H")
            {
                VItemArray2 = new string[] { "0", "1", "2", "3", "4", "5", "6" };
                DItemArray2 = new string[] { "指定なし", "業務番号", "得意先", "原価項目", "業務担当者", "営業担当者", "部門" };

                TItemArray = new string[] { "empty", "TaskCode", "CustoCode", "ItemCode", "LeaderMCode", "SalesMCode", "Department" };
                DITableArray = new string[] { "empty", "D_TaskInd", "M_Partners", "M_Cost", "M_Members", "M_Members", "D_TaskInd" };
                DItmKeyArray = new string[] { "empty", "TaskCode", "PartnerCode", "CostCode", "MemberCode", "MemberCode", "Department" };
                DItmNamArray = new string[] { "empty", "TaskName", "PartnerName", "Item", "Name", "Name", "Department" };
            }
            else
            {
                VItemArray2 = new string[] { "0", "1", "2", "3", "4", "5" };
                DItemArray2 = new string[] { "指定なし", "業務番号", "得意先", "原価項目", "業務担当者", "営業担当者" };

                TItemArray = new string[] { "empty", "TaskCode", "CustoCode", "ItemCode", "LeaderMCode", "SalesMCode" };
                DITableArray = new string[] { "empty", "D_TaskInd", "M_Partners", "M_Cost", "M_Members", "M_Members" };
                DItmKeyArray = new string[] { "empty", "TaskCode", "PartnerCode", "CostCode", "MemberCode", "MemberCode" };
                DItmNamArray = new string[] { "empty", "TaskName", "PartnerName", "Item", "Name", "Name" };
            }
        }
        // 20190519 anonymous add end
    }
}

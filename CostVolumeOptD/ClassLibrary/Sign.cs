using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class Sign
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        public static string HQOffice = "本社";
        public static string HQOfficeCode = "H";
        public static string SubTotal = "小計";
        public static string Total = "計";
        public static string GrandTotal = "総計";
        public static string SumTotal = "合計";
        public static string Expenses = "諸経費";
        public static string Discount = "値引";
        public static string Tax = "消費税";
        public static string LWelfare = "法定福利費";
        public static string GExpenses = "一般経費";
        public static string[] StArray = new string[] { SubTotal, Total, SumTotal, LWelfare, GExpenses, Expenses, Discount, Tax, GrandTotal };

        //private static string[] corpFormArray = new string[] { "株式会社", "有限会社", "合同会社", "(株)", "(有)", "(合)", "（株）", "（有）", "（合）" };

        public static string lblsumTask = "【業務番号 計】";
        public static string lblsumItem = "【原価項目 計】";
        public static string lblsumTerm = "【期間合計】";
        public static string lblTask = "（業務番号）業務";
        public static string lblItem = "（コード）原価項目";

        public static string NameTaskTransfer = "業務引継書"; 

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        /*
         * DHandlingに移行
        public static string RemoveCorpForm(this string CorpName)
        {
            string[] names = new string[corpFormArray.Length + 1];
            names[0] = CorpName; 
            for (int i = 0; i < corpFormArray.Length; i++)
            {
                names[i+1] =  names[i].Replace(corpFormArray[i], "");
            }
            return names[corpFormArray.Length];
        }
        */
    }
}

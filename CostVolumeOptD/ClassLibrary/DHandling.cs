using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class DHandling
    {
        //--------------------------------------//
        // Field
        //--------------------------------------//

        //--------------------------------------//
        // Constructor
        //--------------------------------------//

        //--------------------------------------//
        // Property
        //--------------------------------------//

        //--------------------------------------//
        // Method
        //--------------------------------------//
        // 文字列の指定した位置から指定した長さを取得する
        public static string NumberOfCharacters(string characterString, int startPosition, int length)
        {
            if (startPosition < 0) throw new ArgumentException("引数'start'は0以上でなければなりません。");
            startPosition++;
            if (length < 0) throw new ArgumentException("引数'len'は0以上でなければなりません。");
            if (characterString == null || characterString.Length < startPosition) return "";
            if (characterString.Length < (startPosition + length)) return characterString.Substring(startPosition - 1);
            return characterString.Substring(startPosition - 1, length);
        }


        /// <summary>
        /// 文字列の指定した位置から末尾までを取得する 
        /// </summary>
        /// <param name="characterString"></param>
        /// <param name="startPosition"> 0 から開始する　 </param>
        /// <returns></returns>
        public static string NumberOfCharacters(string characterString, int startPosition)
        {
            return NumberOfCharacters(characterString, startPosition, characterString.Length);
        }


        /// <summary>
        /// 文字列の先頭から指定した長さの文字列を取得する 
        /// </summary>
        /// <param name="characterString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PickUpTopCharacters(string characterString, int length)
        {
            if (length < 0) throw new ArgumentException("引数'len'は0以上でなければなりません。");
            if (characterString == null) return "";
            if (characterString.Length <= length) return characterString;
            return characterString.Substring(0, length);
        }


        // 文字列の末尾から指定した長さの文字列を取得する
        public static string PickUpEndCharacters(string characterString, int length)
        {
            if (length < 0) throw new ArgumentException("引数'len'は0以上でなければなりません。");
            if (characterString == null) return "";
            if (characterString.Length <= length) return characterString;
            return characterString.Substring(characterString.Length - length, length);
        }


        // 降順配列作成　大→小
        public static string[] ArrayNumDescend(int startNum, int arrayCount)
        {
            string[] arrayNum = new string[arrayCount];
            for (int i = 0; i < arrayCount; i++) arrayNum[i] = Convert.ToString(startNum - i);
            return arrayNum;
        }


        // 昇順配列作成　小→大
        public static string[] ArrayNumAscend(int startNum, int arrayCount)
        {
            string[] arrayNum = new string[arrayCount];
            for (int i = 0; i < arrayCount; i++) arrayNum[i] = Convert.ToString(startNum + i);
            return arrayNum;
        }

        // 文字列の中の特定の文字をカウント
        public static int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }



        //********** フォーマットパターンでフォーマット
        public static string FormatStr(string targetString, string formatPattern)
        {
            decimal decimalNum = Convert.ToDecimal(targetString);
            string retValue = decimalNum.ToString(formatPattern);
            return retValue;
        }



        public static string DecimaltoStr(decimal targetDecNum, string formatPattern)
        {
            string retValue = targetDecNum.ToString(formatPattern);
            return retValue;
        }



        //********** 金額単位の変更
        public static string DecimaltoStr(decimal targetDecNum, string formatPattern, int fIndex)
        {
            double coefNum = Math.Pow(10, fIndex);
            targetDecNum = targetDecNum / Convert.ToDecimal(coefNum);
            string retValue = targetDecNum.ToString(formatPattern);
            return retValue;
        }


        public static bool IsDecimal(string str)
        {
            // Decimal型で許容できる数字以外の文字を消去する
            char[] removeChars = new char[] { '-', '+', '.', ',' };
            string tempString = removeChars.Aggregate(str, (s, c) => s.Replace(c.ToString(), ""));
            // 符号、小数点を除いて、0～9の数字であればOK
            if (Regex.IsMatch(tempString, "^[0-9]+$")) return true;
            return false;
        }

        public static bool IsNumeric(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || '9' < c)
                {
                    return false;
                }
            }
            return true;
        }

        public static decimal ToRegDecimal(string str)
        {
            //カンマを除く
            //char[] removeChars = new char[] { '-', '+', '.', ',' };
            char[] removeChars = new char[] {  ',' };
            if (str == null || str == "") return 0;
            string rStr = removeChars.Aggregate(str, (s, c) => s.Replace(c.ToString(), ""));
            return Convert.ToDecimal(rStr);
        }


        public static string ToRegDecStr(string str)
        {
            //カンマを除く
            //char[] removeChars = new char[] { '-', '+', '.', ',' };
            char[] removeChars = new char[] { ',' };
            if (str == null || str == "") return "";
            string rStr = removeChars.Aggregate(str, (s, c) => s.Replace(c.ToString(), ""));
            return rStr;
        }


        public static string RemoveNoNum(string str)
        {
            //ハイフンを除く
            char[] removeChars = new char[] { '-', '+', '.', ',', '/' };
            //char[] removeChars = new char[] { '-' };
            if (str == null || str == "") return "";
            return removeChars.Aggregate(str, (s, c) => s.Replace(c.ToString(), ""));
        }

        public static bool CheckDate(DateTime dt)
        {
            if (dt > DateTime.MinValue && dt < DateTime.MaxValue) return true;
            return false;
        }

        public static bool CheckDateMiniValue(DateTime dt)
        {
            if (dt == DateTime.MinValue) return false;
            if (dt > DateTime.MinValue && dt < DateTime.MaxValue) return true;
            return false;
        }

        public static int FisicalYear()
        {
            DateTime dtNow = DateTime.Now;
            return FisicalYear(dtNow);
        }

        public static int FisicalYear(this DateTime dt)
        {
            if (dt.Month < FiscalYearStartingMonth) return (int)(dt.Year - 1);
            return (int)dt.Year;
        }

        public static DateTime FisicalYearStartDate()
        {
            return DateTime.ParseExact( Convert.ToString( FisicalYear() * 10000 + Conv.StartMMDD ), "yyyyMMdd", null );
        }

        public static DateTime FisicalYearEndDate()
        {
            return DateTime.ParseExact( Convert.ToString ( (FisicalYear() + 1) * 10000  + Conv.EndMMDD), "yyyyMMdd",null);
        }

        public static DateTime FisicalYearStartDate(int year)
        {
            return DateTime.ParseExact( Convert.ToString( year * 10000 + Conv.StartMMDD ), "yyyyMMdd", null );
        }

        public static DateTime FisicalYearEndDate(int year)
        {
            return DateTime.ParseExact( Convert.ToString( ( year + 1 ) * 10000 + Conv.EndMMDD ), "yyyyMMdd", null );
        }

        public static DateTime MonthDay(this DateTime dt)
        {
            return dt.Date;
        }


        //private static readonly int FiscalYearStartingMonth = 7;
        private static readonly int FiscalYearStartingMonth = Conv.FisicalYearStartMonth;

        /// <summary>
        /// 該当年月の日数を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static int DaysInMonth(this DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }

        /// <summary>
        /// 月初日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>Datetime</returns>
        public static DateTime BeginOfMonth(this DateTime dt)
        {
            return dt.AddDays((dt.Day - 1) * -1);
        }

        /// <summary>
        /// 月末日を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime EndOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DaysInMonth(dt));
        }

        /// <summary>
        /// 時刻を落として日付のみにする
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime StripTime(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        /// 日付を落として時刻のみにする
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <param name="base_date">DateTime* : 基準日</param>
        /// <returns>DateTime</returns>
        public static DateTime StripDate(this DateTime dt, DateTime? base_date = null)
        {
            base_date = base_date ?? DateTime.MinValue;
            return new DateTime(base_date.Value.Year, base_date.Value.Month, base_date.Value.Day, dt.Hour, dt.Minute, dt.Second);
        }

        /// <summary>
        /// 該当日付の年度を返す
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <param name="startingMonth">int? : 年度の開始月</param>
        /// <returns>int</returns>
        public static int FiscalYear(this DateTime dt, int? startingMonth = null)
        {
            return (dt.Month >= (startingMonth ?? FiscalYearStartingMonth)) ? dt.Year : dt.Year - 1;
        }


        /// <summary>
        /// 文字列からcorpFormArrayの文字列を削除する。
        /// </summary>
        /// <param name="CorpName"></param>
        /// <returns></returns>
        public static string RemoveCorpForm(this string CorpName)
        {
            string[] corpFormArray = new string[] { "株式会社", "有限会社", "合同会社", "(株)", "(有)", "(合)", "（株）", "（有）", "（合）" };
            string[] names = new string[corpFormArray.Length + 1];
            names[0] = CorpName;
            for (int i = 0; i < corpFormArray.Length; i++)
            {
                names[i + 1] = names[i].Replace(corpFormArray[i], "");
            }
            return names[corpFormArray.Length];
        }

        /// <summary>
        /// 指定された日付が既に締切日を過ぎているか否かを調べる
        /// </summary>
        /// <param name="sDate"> 検査したい日付 </param>
        /// <param name="clsArray"> 最新締切日テーブル（本社、郡山、相双、関東）</param>
        /// <param name="officeCode"> 検査対象となる事業所 </param>
        /// <returns> True : 締切日を過ぎている　false ：締切日にはなっていない　</returns>
        public static bool CheckPastTheDeadline(DateTime sDate, DateTime[] clsArray, string officeCode )
        {
            if( sDate.BeginOfMonth() < clsArray[Conv.oList.IndexOf( officeCode )])
            {
                DMessage.PastDay( clsArray[Conv.oList.IndexOf(officeCode )] );
                return true;
            }
            return false;
        }


        /// <summary>
        /// 指定された日付が既に締切日を過ぎているか否かを調べる、メッセージ無
        /// </summary>
        /// <param name="sDate"> 検査したい日付 </param>
        /// <param name="clsArray"> 最新締切日テーブル（本社、郡山、相双、関東）</param>
        /// <param name="officeCode"> 検査対象となる事業所 </param>
        /// <returns> True : 締切日を過ぎている　false ：締切日にはなっていない　</returns>
        public static bool CheckPastTheDeadlineWoMessage( DateTime sDate, DateTime[] clsArray, string officeCode )
        {
            if( sDate.BeginOfMonth() < clsArray[Conv.oList.IndexOf( officeCode )] )
                return true;
            return false;
        }

    }
}

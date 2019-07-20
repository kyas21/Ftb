using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class MainteOp:EstPlanOp
    {
        /*--------------------------------------------------------*/
        //      Field
        /*--------------------------------------------------------*/
        /*--------------------------------------------------------*/
        //      Construction
        /*--------------------------------------------------------*/
        /*--------------------------------------------------------*/
        //      Property
        /*--------------------------------------------------------*/
        /*--------------------------------------------------------*/
        //      Method
        /*--------------------------------------------------------*/
        public bool WorkItems_Delete(string args)
        {
            string[] valArray = new string[] { "@mCod" };
            string sqlStr = "DELETE FROM M_WorkItems WHERE memberCode = @mCod";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                    {
                        cmd.Parameters.Add(valArray[0], SqlDbType.Char);
                        cmd.Parameters[valArray[0]].Value = args;
                        if (tryExecute(conn, cmd) < 0) return false;
                    }
                }
                finally
                { 
                    conn.Close();
                }
            }
            return true;
        }




        public bool WorkItems_Insert(string[] args)
        {
            string[] valArray = new string[] { "@iCod", "@uItm", "@item", "@iDtl", "@unit", "@sCst", "@mCod" };
            if (args.Length != valArray.Length) return false;

            string sqlStr = "INSERT INTO M_WorkItems(ItemCode, UItem, Item, ItemDetail, Unit, StdCost, MemberCode) VALUES (";
            for (int i = 0; i < valArray.Length; i++)
            {
                if (i == valArray.Length - 1)
                {
                    sqlStr += valArray[i] + ")";
                }
                else
                {
                    sqlStr += valArray[i] + ", ";
                }
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                    {
                        cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
                        cmd.Parameters.Add("@uItm", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@item", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@sCst", SqlDbType.Decimal);
                        cmd.Parameters.Add("@mCod", SqlDbType.Char);
                        for (int i = 0; i < valArray.Length; i++)
                        {
                            if (i == 5)
                            {
                                if (args[i] == null || args[i] == "")
                                {
                                    cmd.Parameters[valArray[i]].Value = 0;
                                }
                                else
                                {
                                    cmd.Parameters[valArray[i]].Value = Convert.ToDecimal(args[i]);
                                }
                            }
                            else
                            {
                                cmd.Parameters[valArray[i]].Value = args[i];
                            }
                        }
                        if (tryExecute(conn, cmd) < 0) return false;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }



        public object WorkItems_Delete(SqlConnection conn, string args)
        {
            string[] valArray = new string[] { "@mCod" };
            string sqlStr = "DELETE FROM M_WorkItems WHERE memberCode = @mCod";
            SqlCommand cmd = new SqlCommand(sqlStr, conn);

            cmd.Parameters.Add("@mCod", SqlDbType.Char);
            cmd.Parameters[valArray[0]].Value = args;

            int num = cmd.ExecuteNonQuery();
            Console.WriteLine(num);

            return true;
        }




        public bool WorkItems_Insert(SqlConnection conn, string[] args)
        {
            string[] valArray = new string[] { "@iCod", "@uItm", "@item", "@iDtl", "@unit", "@sCst", "@mCod" };
            if (args.Length != valArray.Length) return false;

            string sqlStr = "INSERT INTO M_WorkItems(ItemCode, UItem, Item, ItemDetail, Unit, StdCost, MemberCode) VALUES (";
            for (int i = 0; i < valArray.Length; i++)
            {
                if (i == valArray.Length - 1)
                {
                    sqlStr += valArray[i] + ")";
                }
                else
                {
                    sqlStr += valArray[i] + ", ";
                }
            }
            SqlCommand cmd = new SqlCommand(sqlStr, conn);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@uItm", SqlDbType.NVarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@sCst", SqlDbType.Decimal);
            cmd.Parameters.Add("@mCod", SqlDbType.Char);

            for (int i = 0; i < valArray.Length; i++)
            {
                if (i == 5)
                {
                    if (args[i] == null || args[i] == "")
                    {
                        cmd.Parameters[valArray[i]].Value = 0;
                    }
                    else
                    {
                        cmd.Parameters[valArray[i]].Value = Convert.ToDecimal(args[i]);
                    }
                }
                else
                {
                    cmd.Parameters[valArray[i]].Value = args[i];
                }
            }

            int num = cmd.ExecuteNonQuery();
            Console.WriteLine(num);

            return true;
        }















    }
}

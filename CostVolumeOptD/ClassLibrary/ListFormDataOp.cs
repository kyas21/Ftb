using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class ListFormDataOp:DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;
        StringUtility util = new StringUtility();

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public ListFormDataOp()
        {
        }

        public ListFormDataOp(string tableName)
        {
            this.tableName = tableName;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string TableName { get; set; }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public PartnersScData[] SelectPartnersScData()
        {
            string paramStr = "WHERE RelSubco = 1";
            return createPartnersScData( paramStr );
        }


        public PartnersScData[] SelectPartnersCuData()
        {
            string paramStr = "WHERE RelCusto = 1";
            return createPartnersScData( paramStr );
        }


        public TaskCodeNameData[] SelectTaskCodeNameData(string officeCode)
        {
            string sqlString = "WHERE OldVerMark = 0 AND IssueMark = 0 AND OfficeCode = '" + officeCode + "' ORDER BY TaskCode";
            return cleateTaskCodeNameData(sqlString);
        }


        public TaskCodeNameData[] SelectTaskCodeNameData(string officeCode, string department, string direction)
        {
            if (direction == null) direction = "ASC";
            string sqlString = "WHERE OldVerMark = 0 AND IssueMark = 0 AND  OfficeCode = '" + officeCode + "' AND Department = '" + department +  "' ORDER BY TaskCode " + direction;
            return cleateTaskCodeNameData(sqlString);
        }


        public TaskCodeNameData[] SelectTaskCodeNameData( string officeCode, string department, string direction, string range )
        {
            if( direction == null ) direction = "ASC";
            string sqlString = "WHERE OldVerMark = 0 AND OfficeCode = '" + officeCode + "' AND Department = '" + department + "'";

            if( range == "CONTRACT" ) sqlString += " AND OrdersType = 1";
            sqlString += " ORDER BY TaskCode " + direction;
            return cleateTaskCodeNameData( sqlString );
        }


        public TaskCodeNameData[] SelectTaskCodeNameData(string officeCode, string wParam, int pos)
        {
            string sqlString = "WHERE OldVerMark = 0 AND OfficeCode = '" + officeCode + "'" + wParam;
            return cleateTaskCodeNameData(sqlString);
        }


        public TaskCodeNameData SelectTaskCodeNameData(string taskCode, string officeCode)
        {
            SqlHandling sh = new SqlHandling("D_TaskInd");
            DataTable dt = sh.SelectAllData("WHERE OldVerMark = 0 AND TaskCode = '" + taskCode + "' AND OfficeCode = '" + officeCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;

            TaskCodeNameData tcd = new TaskCodeNameData(dt.Rows[0]);
            return tcd;
        }


        public TaskCodeNameData[] SelectTaskCodeNameFromCostReport(DateTime dateFr, DateTime dateTo, string scCode, string officeCode)
        {
            string selParam = "DISTINCT TaskCode FROM D_CostReport WHERE ( ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo + "') "
                                                                          + "AND OfficeCode = '" + officeCode + "' AND ";
            selParam += scCode[0] == 'F' ? "SubCoCode = " : "ItemCode = ";
            selParam += "'" + scCode + "' ORDER BY TaskCode";

            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(selParam);
            if (dt == null || dt.Rows.Count < 1) return null;
            string[] taskCdArray = new string[dt.Rows.Count];
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                taskCdArray[i] = Convert.ToString(dr["TaskCode"]);
            }

            StringUtility utl = new StringUtility();
            TaskIndData tid = new TaskIndData();
            TaskCodeNameData[] tcd = new TaskCodeNameData[taskCdArray.Length];
            for (int i = 0; i < taskCdArray.Length; i++)
            {
                tcd[i] = new TaskCodeNameData();
                tcd[i].TaskCode = taskCdArray[i];
                tid = tid.SelectTaskIndData(utl.SubstringByte(tcd[i].TaskCode, 2, 3) == "999" 
                                            ? utl.SubstringByte(tcd[i].TaskCode, 0, 1) + Convert.ToString(DHandling.FisicalYear() - 2000) + utl.SubstringByte(tcd[i].TaskCode, 1, 1) + "999"
                                            : tcd[i].TaskCode);
                tcd[i].TaskName = tid.TaskName;
                tcd[i].LeaderMCode = tid.LeaderMCode;
            }

            return tcd;
        }


        public TaskCodeNameData[] SelectTaskCodeNameFromOsWkReport( string pCode, DateTime dateFr )
        {
            DateTime dateTo = DHandling.EndOfMonth(dateFr);

            string selParam = "DISTINCT WR.TaskCode AS TaskCode, TI.TaskName AS TaskName, TI.LeaderMCode AS LeaderMCode FROM "
                            + "D_TaskInd TI INNER JOIN D_OsWkReport WR ON TI.TaskCode = WR.TaskCode "
                            + "WHERE (WR.ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo + "') AND WR.PartnerCode = '" + pCode + "' ORDER BY WR.TaskCode";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( selParam );
            if( dt == null || dt.Rows.Count < 1 ) return null;
            TaskCodeNameData[] tcd = new TaskCodeNameData[dt.Rows.Count];
            DataRow dr;
            for(int i = 0;i<dt.Rows.Count;i++ )
            {
                dr = dt.Rows[i];
                tcd[i] = new TaskCodeNameData();
                tcd[i].TaskCode = Convert.ToString( dr["TaskCode"] );
                tcd[i].TaskName = Convert.ToString( dr["TaskName"] );
                tcd[i].LeaderMCode = Convert.ToString( dr["LeaderMCode"]);
            }
            return tcd;
        }

        public TaskCodeNameData[] SelectTaskEntryNameData(string officeCode,string department,string direction)
        {
            if(direction == null) direction = "ASC";
            string sqlString = "WHERE OfficeCode = '" + officeCode + "' AND Department = '" + department + "' ORDER BY TaskEntryID " + direction;
            return cleateTaskEntryNameData(sqlString);
        }
        



        public TaskData SelectTaskData(string taskCode)
        {
            SqlHandling sh = new SqlHandling("D_Task");
            DataTable dt = sh.SelectAllData("WHERE OldVerMark = 0 AND TaskBaseCode = '" + DHandling.NumberOfCharacters(taskCode, 1) + "'");
            if (dt == null || dt.Rows.Count < 1) return null;

            TaskData td = new TaskData(dt.Rows[0]);
            return td;
        }


        public MembersScData[] SelectMembersScData()
        {
            string paramStr = "WHERE MemberType = 0 AND Enrollment = 0";
            return createMembersScDataArray( paramStr );;
        }


        public MembersScData[] SelectMembersScData(string officeCode)
        {
            string paramStr = "WHERE OfficeCode = '" + officeCode + "' AND MemberType = 0 AND Enrollment = 0";
            return createMembersScDataArray( paramStr );;
        }


        public MembersScData[] SelectMembersScData(string MemberName,int EnrollmentCD)
        {
            string paramStr = "WHERE Name LIKE '%" + MemberName + "%' AND Enrollment = " + EnrollmentCD;
            return createMembersScDataArray( paramStr );;
        }


        public MembersScData SelectMembersScDataS(string memberCode)
        {
            SqlHandling sh = new SqlHandling("M_Members");
            DataTable dt = sh.SelectAllData("WHERE MemberCode = '" + memberCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;

            MembersScData msd = new MembersScData(dt.Rows[0]);
            return msd;
        }


        public CostData[] SelectCostData(string officeCode)
        {
            string paramStr = "WHERE OfficeCode = '" + officeCode + "' OR OfficeCode = ' ' OR OfficeCode = NULL ORDER BY CostCode";
            return createCostDataArray( paramStr);
        }


        public CostData[] SelectCostData(string officeCode,string wParam)
        {
            string parmStr = "WHERE OfficeCode = '" + officeCode + "'" + wParam;
            return createCostDataArray( parmStr );
        }

        public CostData[] SelectCostDataInitialF( string officeCode )
        {
            string wParam = "WHERE OfficeCode = '" + officeCode + "' AND CostCode BETWEEN 'F001' AND 'F998' ORDER BY CostCode";
            return createCostDataArray(wParam);
        }


        public CostData[] SelectCostDataJoinOsWkReport( string officeCode )
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( "DISTINCT WR.PartnerCode AS CostCode, MC.CostID AS CostID, MC.Item AS Item, "
                                                    + "MC.ItemDetail AS ItemDetail, MC.Unit AS Unit, MC.Cost AS Cost, "
                                                    + "MC.OfficeCode AS OfficeCode, MC.MemberCode AS MemberCode "
                                                    + "FROM M_Cost MC INNER JOIN D_OsWkReport WR ON MC.CostCode = WR.PartnerCode "
                                                    + "WHERE MC.OfficeCode = '" + officeCode + "' AND "
                                                    + "WR.PartnerCode BETWEEN 'F001' AND 'F998' ORDER BY WR.PartnerCode" );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            CostData[] cmd = new CostData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                cmd[i] = new CostData( dt.Rows[i] );
            }
            return cmd;
        }


        public CostData[] SelectCostData(string officeCode, string field, string str)
        {
            string paramStr = "WHERE OfficeCode = '" + officeCode + "' AND " + field + " LIKE '" + str + "%'";
            return createCostDataArray( paramStr );
        }


        public CostData[] SelectCostData(string officeCode, string field, string str, string name)
        {
            string paramStr = "WHERE OfficeCode = '" + officeCode + "' AND Item LIKE '" + name[0] +"%' AND "  + field + " LIKE '" + str + "%'";
            return createCostDataArray( paramStr );
        }


        public CostData[] SelectCostDataFromCostReport(DateTime dateFr, DateTime dateTo, string officeCode, string department)
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription("DISTINCT ItemCode FROM D_CostReport WHERE (ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo 
                                            + "') AND OfficeCode = '" + officeCode + "' AND Department = '" + department 
                                            + "' AND ItemCode LIKE 'A%' ORDER BY ItemCode");
            string[] itemArray = new string[1];
            int itemCnt = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                itemArray = new string[dt.Rows.Count];
                itemCnt = dt.Rows.Count;
                DataRow dr;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    itemArray[i] = Convert.ToString(dr["ItemCode"]);
                }
            }

            dt = sh.SelectFullDescription("DISTINCT SubCoCode FROM D_CostReport WHERE (ReportDate BETWEEN '" + dateFr + "' AND '" + dateTo
                                            + "') AND OfficeCode = '" + officeCode + "' AND Department = '" + department 
                                            + "' AND SubCoCode BETWEEN 'F001' AND 'F998' ORDER BY SubCoCode");
            string[] subCoArray = new string[1];
            int subCoCnt = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                subCoArray = new string[dt.Rows.Count];
                subCoCnt = dt.Rows.Count;
                DataRow dr;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    subCoArray[i] = Convert.ToString(dr["SubCoCode"]);
                }
            }

            if ((itemCnt + subCoCnt) == 0) return null;

            string[] costCodeArray = new string[itemCnt+subCoCnt];

            if (itemCnt == 0)
            {
                Array.Copy(subCoArray, costCodeArray, subCoCnt);
            }
            else
            {
                Array.Copy(itemArray, costCodeArray, itemCnt);
                if(subCoCnt != 0)
                    Array.Copy(subCoArray, 0, costCodeArray, itemCnt, subCoCnt);
            }

            CostData cd = new CostData();
            CostData[] cmd = new CostData[costCodeArray.Length];
            for (int i = 0; i < costCodeArray.Length; i++)
            {
                cmd[i] = new CostData();
                cmd[i].CostCode = costCodeArray[i]; 
                cmd[i].Item = cd.SelectCostName(cmd[i].CostCode);
            }

            return cmd;
        }

        //----------------------------------------------------------------------------//
        //     SubRoutine                                                             //
        //----------------------------------------------------------------------------//
        private PartnersScData[] createPartnersScData(string wParam)
        {
            SqlHandling sh = new SqlHandling( "M_Partners" );
            DataTable dt = sh.SelectAllData( wParam );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            PartnersScData[] psd = new PartnersScData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                psd[i] = new PartnersScData( dt.Rows[i] );
            }
            return psd;
        }


        private TaskCodeNameData[] cleateTaskCodeNameData(string sqlString)
        {
            SqlHandling sh = new SqlHandling("D_TaskInd");
            DataTable dt = sh.SelectAllData(sqlString);
            if(dt == null || dt.Rows.Count < 1) return null;

            TaskCodeNameData[] tcd = new TaskCodeNameData[dt.Rows.Count];
            for(int i = 0;i < dt.Rows.Count;i++)
            {
                tcd[i] = new TaskCodeNameData(dt.Rows[i]);
            }
            return tcd;
        }


        private TaskCodeNameData[] cleateTaskEntryNameData( string sqlString )
        {
            SqlHandling sh = new SqlHandling( "D_TaskEntry" );
            DataTable dt = sh.SelectAllData( sqlString );
            if ( dt == null || dt.Rows.Count < 1 ) return null;

            TaskCodeNameData[] tcd = new TaskCodeNameData[dt.Rows.Count];
            DataRow dr;
            for ( int i = 0; i < dt.Rows.Count; i++ )
            {
                tcd[i] = new TaskCodeNameData( dt.Rows[i] );
                dr = dt.Rows[i];
                string wkTaskCode = Convert.ToString( dr["TaskCode"] ).Trim();
                
                tcd[i].TaskCode = Convert.ToString(dr["TaskCode"]).Trim();
                tcd[i].TaskName = Convert.ToString(dr["TaskName"]);
                tcd[i].LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
                tcd[i].OfficeCode = Convert.ToString( dr["OfficeCode"] );
                tcd[i].TaskID = Convert.ToInt32( dr["TaskEntryID"] );
                tcd[i].TaskIndID = Convert.ToInt32(dr["TaskIndID"]);
                tcd[i].Partner = Convert.ToString(dr["PartnerCode"]);
            }
            return tcd;
        }


        private MembersScData[] createMembersScDataArray(string wParam)
        {
            SqlHandling sh = new SqlHandling( "M_Members" );
            DataTable dt = sh.SelectAllData( wParam );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            MembersScData[] msd = new MembersScData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                msd[i] = new MembersScData( dt.Rows[i] );
            }
            return msd;
        }


        private CostData[] createCostDataArray( string wParam )
        {
            SqlHandling sh = new SqlHandling( "M_Cost" );
            DataTable dt = sh.SelectAllData( wParam );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            CostData[] cmd = new CostData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                cmd[i] = new CostData( dt.Rows[i] );
            }
            return cmd;
        }


    }
}

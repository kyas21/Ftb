using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class WorkReportData:DbAccess
    {

        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        //private string insSql = "INSERT INTO D_WorkReport "
        //                        + "TaskCode, Publisher, MemberCode, ReportDate, WorkCont, WorkDetail, MCostCode, MemberNH, MemberOH, "
        //                        + "WCostCode, WorkingNH, WorkingOH, Note, SlipNo, MemNHCostReportID, MemOHCostReportID, WorkNHCostReportID, WorkOHCostReportID, WorkContCostReportID, "
        //                        + "MOfficeCode, MDepartment, WOfficeCode, WDepartment, UserCheck) VALUES ("
        //                        + "@tCod, @publ, @mCod, @rDat, @wCon, @wDtl, @mCCd, @mNH, @mOH, "
        //                        + "@wCCd, @wNH, @wOH, @note, @slip, @mNID, @mOID, @wNID, @wOID, @wCID, "
        //                        + "@mOCd, @mDep, @wOCd, @wDep, @uChk )";

        private string updSql = "UPDATE D_WorkReport SET "
                                + "TaskCode = @tCod, Publisher = @publ, MemberCode = @mCod, ReportDate = @rDat, WorkCont = @wCon, "
                                + "WorkDetail = @wDtl, MCostCode = @mCCd, MemberNH = @mNH, MemberOH = @mOH, WCostCode = @wCCd, "
                                + "WorkingNH = @wNH, WorkingOH = @wOH, Note = @note, MemNHCostReportID = @mNID, MemOHCostReportID = @mOID, "
                                + "WorkNHCostReportID = @wNID, WorkOHCostReportID = @wOID, WorkContCostReportID = @wCID, "
                                + "MOfficeCode = @mOCd, MDepartment = @mDep, WOfficeCode = @wOCd, WDepartment = @wDep, UserCheck = @uChk "
                                + "WHERE SlipNo = @slip";

        //private string updMemberSql = "UPDATE D_WorkReport SET "
        //                        + "MCostCode = @mCCd, MemberNH = @mNH, MemberOH = @mOH, MemNHCostReportID = @mNID, MemOHCostReportID = @mOID "
        //                        + "WHERE SlipNo = @slip";

        //private string updWorkerSql = "UPDATE D_WorkReport SET "
        //                        + "WCostCode = @wCCd, WorkingNH = @wNH, WorkingOH = @wOH, WorkNHCostReportID = @wNID, WorkOHCostReportID = @wOID "
        //                        + "WHERE SlipNo = @slip";

        //private string updContentSql = "UPDATE D_WorkReport SET WorkCont = @wCon WHERE SlipNo = @slip";

        private string selSql = "SELECT * FROM D_WorkReport WHERE SlipNo = @slip";

        private string delSql = "DELETE FROM D_WorkReport WHERE SlipNo = @slip";

        //WorkReportData wrd;

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public WorkReportData()
        {
        }

        public WorkReportData(DataRow dr)
        {

            TaskCode = Convert.ToString(dr["TaskCode"]);
            Publisher = Convert.ToString(dr["Publisher"]);
            MemberCode = Convert.ToString(dr["MemberCode"]);
            ReportDate = Convert.ToDateTime(dr["ReportDate"]);
            WorkCont = Convert.ToString(dr["WorkCont"]);
            WorkDetail = Convert.ToString(dr["WorkDetail"]);
            MCostCode = Convert.ToString(dr["MCostCode"]);
            MemberNH = Convert.ToDecimal(dr["MemberNH"]);
            MemberOH = Convert.ToDecimal(dr["MemberOH"]);
            WCostCode = Convert.ToString(dr["WCostCode"]);
            WorkingNH = Convert.ToDecimal(dr["WorkingNH"]);
            WorkingOH = Convert.ToDecimal(dr["WorkingOH"]);
            Note = Convert.ToString(dr["Note"]);
            SlipNo = Convert.ToInt32(dr["SlipNo"]);
            MemNHCostReportID = Convert.ToInt32(dr["MemNHCostReportID"]);
            MemOHCostReportID = Convert.ToInt32(dr["MemOHCostReportID"]);
            WorkNHCostReportID = Convert.ToInt32(dr["WorkNHCostReportID"]);
            WorkOHCostReportID = Convert.ToInt32(dr["WorkOHCostReportID"]);
            WorkContCostReportID = Convert.ToInt32(dr["WorkContCostReportID"]);
            MOfficeCode = Convert.ToString(dr["MOfficeCode"]);
            MDepartment = Convert.ToString(dr["MDepartment"]);
            WOfficeCode = Convert.ToString(dr["WOfficeCode"]);
            WDepartment = Convert.ToString(dr["WDepartment"]);
            //if (Convert.ToString(dr["UserCheck"]) == null)
            //{
            //    UserCheck = 0;
            //}
            //else
            //{
            //    UserCheck = Convert.ToInt32(dr["UserCheck"]);
            //}

            OfficeCode = Convert.ToString(Publisher[0]); 
            Department = Convert.ToString(Publisher[1]); 
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int WorkReportID { get; set; }
        public string TaskCode { get; set; }
        public string Publisher { get; set; }
        public string MemberCode { get; set; }
        public DateTime ReportDate { get; set; }
        public string WorkCont { get; set; }
        public string WorkDetail { get; set; }
        public string MCostCode { get; set; }
        public decimal MemberNH { get; set; }
        public decimal MemberOH { get; set; }
        public string WCostCode { get; set; }
        public decimal WorkingNH { get; set; }
        public decimal WorkingOH { get; set; }
        public string Note { get; set; }
        public int SlipNo { get; set; }
        public int MemNHCostReportID { get; set; }
        public int MemOHCostReportID { get; set; }
        public int WorkNHCostReportID { get; set; }
        public int WorkOHCostReportID { get; set; }
        public int WorkContCostReportID { get; set; }
        public string MOfficeCode { get; set; }
        public string MDepartment { get; set; }
        public string WOfficeCode { get; set; }
        public string WDepartment { get; set; }
        public int UserCheck { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            WorkReportData cloneData = new WorkReportData();

            cloneData.WorkReportID = this.WorkReportID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.Publisher = this.Publisher;
            cloneData.MemberCode = this.MemberCode;
            cloneData.ReportDate = this.ReportDate;
            cloneData.WorkCont = this.WorkCont;
            cloneData.WorkDetail = this.WorkDetail;
            cloneData.MCostCode = this.MCostCode;
            cloneData.MemberNH = this.MemberNH;
            cloneData.MemberOH = this.MemberOH;
            cloneData.WCostCode = this.WCostCode;
            cloneData.WorkingNH = this.WorkingNH;
            cloneData.WorkingOH = this.WorkingOH;
            cloneData.Note = this.Note;
            cloneData.SlipNo = this.SlipNo;
            cloneData.MemNHCostReportID = this.MemNHCostReportID;
            cloneData.MemOHCostReportID = this.MemOHCostReportID;
            cloneData.WorkNHCostReportID = this.WorkNHCostReportID;
            cloneData.WorkOHCostReportID = this.WorkOHCostReportID;
            cloneData.WorkContCostReportID = this.WorkContCostReportID;
            cloneData.MOfficeCode = this.MOfficeCode;
            cloneData.MDepartment = this.MDepartment;
            cloneData.WOfficeCode = this.WOfficeCode;
            cloneData.WDepartment = this.WDepartment;
            cloneData.UserCheck = this.UserCheck;
                                                                            
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;

            return cloneData;
        }

        
        public bool ClearPartWorkReport(int slipNo,int crID)
        {
            WorkReportData wrd = loadWorkReport(slipNo);
            if (wrd.MemNHCostReportID == crID)
            {
                wrd.MemberNH = 0;
                wrd.MemNHCostReportID = 0;
            }

            if (wrd.MemOHCostReportID == crID)
            {
                wrd.MemberOH = 0;
                wrd.MemOHCostReportID = 0;
            }

            if (wrd.WorkNHCostReportID == crID)
            {
                wrd.WorkingNH = 0;
                wrd.WorkNHCostReportID = 0;
            }

            if (wrd.WorkOHCostReportID == crID)
            {
                wrd.WorkingOH = 0;
                wrd.WorkOHCostReportID = 0;
            }

            if (wrd.WorkContCostReportID == crID)
            {
                wrd.WorkCont = "";
                wrd.WorkContCostReportID = 0;
            }

            if (wrd.MemberNH == 0 && wrd.MemberOH == 0) wrd.MCostCode = "";
            if (wrd.WorkingNH == 0 && wrd.WorkingOH == 0)
            {
                wrd.WCostCode = "";
                wrd.WOfficeCode = "";
                wrd.WDepartment = "";
            }

            if (wrd.MemNHCostReportID == 0 && wrd.MemOHCostReportID == 0 && wrd.WorkNHCostReportID == 0 && wrd.WorkOHCostReportID == 0 && wrd.WorkContCostReportID == 0)
                return DeleteWorkReport(slipNo);

            return UpdateWorkReport(wrd);
        }


        public bool UpdatePartWorkReport(int slipNo, int crID, decimal qty)
        {
            WorkReportData wrd = loadWorkReport(slipNo);
            if (wrd.MemNHCostReportID == crID)
            {
                wrd.MemberNH = qty;
            }

            if (wrd.MemOHCostReportID == crID)
            {
                wrd.MemberOH = qty;
            }

            if (wrd.WorkNHCostReportID == crID)
            {
                wrd.WorkingNH = qty;
            }

            if (wrd.WorkOHCostReportID == crID)
            {
                wrd.WorkingOH = qty;
            }

            return UpdateWorkReport(wrd);
        }

        public bool UpdateWorkReport(WorkReportData wrd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(updSql, conn);

                    cmd = parametersSqlDbType(cmd);
                    cmd = parametersValue(cmd, wrd);

                    if (TryExecute(conn, cmd) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }



        public bool DeleteWorkReport(int slipNo)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(delSql, conn);

                    cmd.Parameters.Add("@slip", SqlDbType.Int);
                    cmd.Parameters["@slip"].Value = slipNo;

                    if (TryExecute(conn, cmd) < 0) return false;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        //public bool SelectWorkReportData(int slipNo)
        public bool ExistenceSlipNo(int slipNo)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(selSql, conn);
                    cmd.Parameters.Add("@slip", SqlDbType.Int);
                    cmd.Parameters["@slip"].Value = slipNo;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (!dr.HasRows) return false;
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return false;
                }
                conn.Close();
            }
            return true;
        }


        public decimal[] SelectSummaryWorkReport(string memberCode, DateTime date)
        {
            //decimal[] workHour = new decimal[2];
            decimal[] workHour = new decimal[3];

            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(" SUM(MemberNH) AS NHours,SUM(MemberOH) AS OHours, SUM(UserCheck) AS UCheck FROM D_WorkReport WHERE MemberCode = '" + memberCode + "' AND ReportDate = '" + date.StripTime() + "'");
            //DataTable dt = sh.SelectFullDescription(" SUM(MemberNH) AS NHours,SUM(MemberOH) AS OHours FROM D_WorkReport WHERE MemberCode = '" + memberCode + "' AND ReportDate = '" + date.StripTime() + "'");
            if (dt == null || dt.Rows.Count == 0)
            {
                workHour[0] = -1;
                workHour[1] = -1;
            }
            else
            {
                DataRow dr = dt.Rows[0];
                if (Convert.ToString(dr["NHours"]) == "")
                {
                    workHour[0] = -1;
                    workHour[1] = -1;
                }
                else
                {
                    workHour[0] = Convert.ToDecimal(dr["NHours"]);
                    workHour[1] = Convert.ToDecimal(dr["OHours"]);
                }
                // 20161115 append
                workHour[2] = (Convert.ToString(dr["UCheck"]) == "") ? 0 : Convert.ToDecimal(dr["UCheck"]); 
            }
            return workHour;
        }


        private WorkReportData loadWorkReport(int slipNo)
        {
            SqlHandling sh = new SqlHandling("D_WorkReport");
            DataTable dt = sh.SelectAllData("WHERE SlipNo = " + slipNo);
            if (dt == null || dt.Rows.Count < 1) return null;
            WorkReportData wrd = new WorkReportData(dt.Rows[0]);
            return wrd;
        }


        private SqlCommand parametersSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@tCod", SqlDbType.Char);
            cmd.Parameters.Add("@publ", SqlDbType.Char);
            cmd.Parameters.Add("@mCod", SqlDbType.Char);
            cmd.Parameters.Add("@rDat", SqlDbType.Date);
            cmd.Parameters.Add("@wCon", SqlDbType.NVarChar);
            cmd.Parameters.Add("@wDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@mCCd", SqlDbType.VarChar);
            cmd.Parameters.Add("@mNH", SqlDbType.Decimal);
            cmd.Parameters.Add("@mOH", SqlDbType.Decimal);
            cmd.Parameters.Add("@wCCd", SqlDbType.VarChar);
            cmd.Parameters.Add("@wNH", SqlDbType.Decimal);
            cmd.Parameters.Add("@wOH", SqlDbType.Decimal);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters.Add("@slip", SqlDbType.Int);
            cmd.Parameters.Add("@mNID", SqlDbType.Int);
            cmd.Parameters.Add("@mOID", SqlDbType.Int);
            cmd.Parameters.Add("@wNID", SqlDbType.Int);
            cmd.Parameters.Add("@wOID", SqlDbType.Int);
            cmd.Parameters.Add("@wCID", SqlDbType.Int);
            cmd.Parameters.Add("@mOCd", SqlDbType.Char);
            cmd.Parameters.Add("@mDep", SqlDbType.Char);
            cmd.Parameters.Add("@wOCd", SqlDbType.Char);
            cmd.Parameters.Add("@wDep", SqlDbType.Char);
            cmd.Parameters.Add("@uChk", SqlDbType.Int);
            return cmd;
        }


        private SqlCommand parametersValue(SqlCommand cmd, WorkReportData wrd)
        {
            cmd.Parameters["@tCod"].Value = wrd.TaskCode;
            cmd.Parameters["@publ"].Value = wrd.Publisher;
            cmd.Parameters["@mCod"].Value = wrd.MemberCode;
            cmd.Parameters["@rDat"].Value = wrd.ReportDate;
            cmd.Parameters["@wCon"].Value = wrd.WorkCont;
            cmd.Parameters["@wDtl"].Value = wrd.WorkDetail;
            cmd.Parameters["@mCCd"].Value = wrd.MCostCode;
            cmd.Parameters["@mNH"].Value = wrd.MemberNH;
            cmd.Parameters["@mOH"].Value = wrd.MemberOH;
            cmd.Parameters["@wCCd"].Value = wrd.WCostCode;
            cmd.Parameters["@wNH"].Value = wrd.WorkingNH;
            cmd.Parameters["@wOH"].Value = wrd.WorkingOH;
            cmd.Parameters["@note"].Value = wrd.Note;
            cmd.Parameters["@slip"].Value = wrd.SlipNo;
            cmd.Parameters["@mNID"].Value = wrd.MemNHCostReportID;
            cmd.Parameters["@mOID"].Value = wrd.MemOHCostReportID;
            cmd.Parameters["@wNID"].Value = wrd.WorkNHCostReportID;
            cmd.Parameters["@wOID"].Value = wrd.WorkOHCostReportID;
            cmd.Parameters["@wCID"].Value = wrd.WorkContCostReportID;
            cmd.Parameters["@mOCd"].Value = wrd.MOfficeCode;
            cmd.Parameters["@mDep"].Value = wrd.MDepartment;
            cmd.Parameters["@wOCd"].Value = wrd.WOfficeCode;
            cmd.Parameters["@wDep"].Value = wrd.WDepartment;
            cmd.Parameters["@uChk"].Value = wrd.UserCheck;

            return cmd;
        }
























    }
}

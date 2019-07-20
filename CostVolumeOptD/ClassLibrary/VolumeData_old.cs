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
    public class VolumeData : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        private string insSql = "INSERT INTO D_Volume "
                              + "(TaskCode, OfficeCode, Department, YearMonth, MonthlyVolume, VolUncomp, VolClaimRem, VolClaim, MonthlyClaim, VolPaid, BalanceClaim, BalanceIncom, MonthlyCost, ClaimDate, PaidDate, CarryOverPlanned, Comment, TaskStat, Note) VALUES ( "
                              + "@TaskCode, @OfficeCode, @Department, @YearMonth, @MonthlyVolume, @VolUncomp, @VolClaimRem, @VolClaim, @MonthlyClaim, @VolPaid, @BalanceClaim, @BalanceIncom, @MonthlyCost, @ClaimDate,@PaidDate, @CarryOverPlanned, @Comment, @TaskStat, @Note )";

        private string updSql = "UPDATE D_Volume SET "
                                + "TaskCode = @TaskCode, OfficeCode = @OfficeCode, Department = @Department, YearMonth = @YearMonth, MonthlyVolume = @MonthlyVolume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, "
                                + "VolClaim = @VolClaim, MonthlyClaim = @MonthlyClaim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, BalanceIncom = @BalanceIncom, MonthlyCost = @MonthlyCost, ClaimDate = @ClaimDate, "
                                + "PaidDate = @PaidDate, CarryOverPlanned = @CarryOverPlanned, Comment = @Comment, TaskStat = @TaskStat, Note = @Note "
                                + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";

        private string updSql1 = "UPDATE D_Volume SET "
                                + "TaskCode = @TaskCode, OfficeCode = @OfficeCode, Department = @Department, YearMonth = @YearMonth, MonthlyVolume = @MonthlyVolume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, "
                                + "VolClaim = @VolClaim, MonthlyClaim = @MonthlyClaim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, BalanceIncom = @BalanceIncom, MonthlyCost = @MonthlyCost, ClaimDate = @ClaimDate, "
                                + "PaidDate = @PaidDate, CarryOverPlanned = @CarryOverPlanned, Comment = @Comment, TaskStat = @TaskStat, Note = @Note "
                                + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode";

        private string selSql = "SELECT * FROM D_Volume WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";

        private string selSql1 = "SELECT * FROM D_Volume WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode";

        private string delSql = "DELETE FROM D_Volume WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";
        

        //private string updSql2 = "UPDATE D_Volume SET "
        //                        + "MonthlyVolume = @mVol, VolUncomp = @vUnc, VolClaimRem = @vRem, VolClaim = @vClm, MonthlyClaim = @mClm, VolPaid = @vPai, "
        //                        + "BalanceClaim = @bClm, BalanceIncom = @bIcm, MonthlyCost = @mCos, ClaimDate = @cDat, PaidDate = @pDat, "
        //                        + "CarryOverPlanned = @cOvr, Comment = @comt, TaskStat = @tSt "
        //                        + "WHERE TaskCode = @tCod AND OfficeCode = @oCod AND Department = @dCod AND YearMonth = @yM";

        private string sumPara = "SUM(MonthlyVolume) AS sMV, SUM(VolUncomp) AS sVU, SUM(VolClaimRem) AS sVCR, SUM(VolClaim) AS sVC, "
                                + "SUM(MonthlyClaim) AS sMC, SUM(VolPaid) AS sVP, SUM(BalanceClaim) AS sBC, SUM(BalanceIncom) AS sBI, "
                                + "SUM(MonthlyCost) AS sMCO, SUM(CarryOverPlanned) AS sCOP FROM D_Volume WHERE ";
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public VolumeData()
        {
        }

        public VolumeData(DataRow dr)
        {
            //if (dr["TaskCode"] != DBNull.Value) TaskCode = Convert.ToString(dr["TaskCode"]);
            //if (dr["OfficeCode"] != DBNull.Value) OfficeCode = Convert.ToString(dr["OfficeCode"]);
            //if (dr["Department"] != DBNull.Value) Department = Convert.ToString(dr["Department"]);
            //if (dr["YearMonth"] != DBNull.Value) YearMonth = Convert.ToInt32(dr["YearMonth"]);
            //if (dr["MonthlyVolume"] != DBNull.Value) MonthlyVolume = Convert.ToDecimal(dr["MonthlyVolume"]);
            //if (dr["VolUncomp"] != DBNull.Value) VolUncomp = Convert.ToDecimal(dr["VolUncomp"]);
            //if (dr["VolClaimRem"] != DBNull.Value) VolClaimRem = Convert.ToDecimal(dr["VolClaimRem"]);
            //if (dr["VolClaim"] != DBNull.Value) VolClaim = Convert.ToDecimal(dr["VolClaim"]);
            //if (dr["MonthlyClaim"] != DBNull.Value) MonthlyClaim = Convert.ToDecimal(dr["MonthlyClaim"]);
            //if (dr["VolPaid"] != DBNull.Value) VolPaid = Convert.ToDecimal(dr["VolPaid"]);
            //if (dr["BalanceClaim"] != DBNull.Value) BalanceClaim = Convert.ToDecimal(dr["BalanceClaim"]);
            //if (dr["BalanceIncom"] != DBNull.Value) BalanceIncom = Convert.ToDecimal(dr["BalanceIncom"]);
            //if (dr["MonthlyCost"] != DBNull.Value) MonthlyCost = Convert.ToDecimal(dr["MonthlyCost"]);
            //if (dr["ClaimDate"] != DBNull.Value) ClaimDate = Convert.ToDateTime(dr["ClaimDate"]);
            //if (dr["PaidDate"] != DBNull.Value) PaidDate = Convert.ToDateTime(dr["PaidDate"]);
            //if (dr["CarryOverPlanned"] != DBNull.Value) CarryOverPlanned = Convert.ToDecimal(dr["CarryOverPlanned"]);
            //if (dr["Comment"] != DBNull.Value) Comment = Convert.ToString(dr["Comment"]);
            //if (dr["TaskStat"] != DBNull.Value) TaskStat = Convert.ToInt32(dr["TaskStat"]);
            //if (dr["Note"] != DBNull.Value) Note = Convert.ToString(dr["Note"]);

            TaskCode = dr.Field<String>("TaskCode") ?? default(String);
            OfficeCode = dr.Field<String>("OfficeCode") ?? default(String);
            Department = dr.Field<String>("Department") ?? default(String);
            YearMonth = dr.Field<Int32?>("YearMonth") ?? default(Int32);
            //MonthlyVolume = dr.Field<Decimal?>("MonthlyVolume") ?? default(Decimal);
            MonthlyVolume = dr.Field<Decimal?>("MonthlyVolume") ?? null;
            VolUncomp = dr.Field<Decimal?>("VolUncomp") ?? default(Decimal);
            VolClaimRem = dr.Field<Decimal?>("VolClaimRem") ?? default(Decimal);
            VolClaim = dr.Field<Decimal?>("VolClaim") ?? default(Decimal);
            MonthlyClaim = dr.Field<Decimal?>("MonthlyClaim") ?? default(Decimal);
            VolPaid = dr.Field<Decimal?>("VolPaid") ?? default(Decimal);
            BalanceClaim = dr.Field<Decimal?>("BalanceClaim") ?? default(Decimal);
            BalanceIncom = dr.Field<Decimal?>("BalanceIncom") ?? default(Decimal);
            MonthlyCost = dr.Field<Decimal?>("MonthlyCost") ?? default(Decimal);
            ClaimDate = dr.Field<DateTime?>("ClaimDate") ?? default(DateTime);
            PaidDate = dr.Field<DateTime?>("PaidDate") ?? default(DateTime);
            CarryOverPlanned = dr.Field<Decimal?>("CarryOverPlanned") ?? default(Decimal);
            Comment = dr.Field<String>("Comment") ?? default(String);
            TaskStat = dr.Field<Int32?>("TaskStat") ?? default(Int32);
            Note = dr.Field<String>("Note") ?? default(String);
        }

        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //public int VolumeID { get; set; }
        //public string TaskCode { get; set; }
        //public string OfficeCode { get; set; }
        //public string Department { get; set; }
        ////public DateTime YearMonth { get; set; }
        //public int YearMonth { get; set; }
        //public decimal MonthlyVolume { get; set; }
        //public decimal VolUncomp { get; set; }
        //public decimal VolClaimRem { get; set; }
        //public decimal VolClaim { get; set; }
        //public decimal MonthlyClaim { get; set; }
        //public decimal VolPaid { get; set; }
        //public decimal BalanceClaim { get; set; }
        //public decimal BalanceIncom { get; set; }
        //public decimal MonthlyCost { get; set; }
        //public DateTime ClaimDate { get; set; }
        //public DateTime PaidDate { get; set; }
        //public decimal CarryOverPlanned { get; set; }
        //public string Comment { get; set; }
        //public int TaskStat { get; set; }
        public int VolumeID { get; set; }
        public string TaskCode { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public int YearMonth { get; set; }
        public decimal? MonthlyVolume { get; set; }
        public decimal? VolUncomp { get; set; }
        public decimal? VolClaimRem { get; set; }
        public decimal? VolClaim { get; set; }
        public decimal? MonthlyClaim { get; set; }
        public decimal? VolPaid { get; set; }
        public decimal? BalanceClaim { get; set; }
        public decimal? BalanceIncom { get; set; }
        public decimal? MonthlyCost { get; set; }
        public DateTime? ClaimDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? CarryOverPlanned { get; set; }
        public string Comment { get; set; }
        public int TaskStat { get; set; }
        public string Note { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            VolumeData cloneData = new VolumeData();
            cloneData.VolumeID = this.VolumeID;
            cloneData.TaskCode = this.TaskCode;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.YearMonth = this.YearMonth;
            cloneData.MonthlyVolume = this.MonthlyVolume;
            cloneData.VolUncomp = this.VolUncomp;
            cloneData.VolClaimRem = this.VolClaimRem;
            cloneData.VolClaim = this.VolClaim;
            cloneData.MonthlyClaim = this.MonthlyClaim;
            cloneData.VolPaid = this.VolPaid;
            cloneData.BalanceClaim = this.BalanceClaim;
            cloneData.BalanceIncom = this.BalanceIncom;
            cloneData.MonthlyCost = this.MonthlyCost;
            cloneData.ClaimDate = this.ClaimDate;
            cloneData.PaidDate = this.PaidDate;
            cloneData.CarryOverPlanned = this.CarryOverPlanned;
            cloneData.Comment = this.Comment;
            cloneData.TaskStat = this.TaskStat;
            cloneData.Note = this.Note;

            return cloneData;
        }


        public VolumeData[] SelectVolumeDate(string strTaskCode, int intYearMonth, string strOfficeCode, string strDepartment)
        {
            SqlHandling sh = new SqlHandling("D_Volume");

            string strSqlDepartment = "";
            if (strDepartment != "")
                strSqlDepartment = " AND Department = '" + strDepartment + "'";
            DataTable dt = sh.SelectAllData("WHERE TaskCode = '" + strTaskCode + "' AND OfficeCode = '" + strOfficeCode + "'" + strSqlDepartment + " AND YearMonth Like  '" + intYearMonth + "%" + "'");
            if (dt == null || dt.Rows.Count < 1) return null;

            VolumeData[] vd = new VolumeData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) vd[i] = new VolumeData(dt.Rows[i]);
            return vd;
        }


        public VolumeData[] SelectVolumeDate(string strSql)
        {
            SqlHandling sh = new SqlHandling("D_Volume");
            DataTable dt = sh.SelectFullDescription(strSql);
            if (dt == null || dt.Rows.Count < 1) return null;

            VolumeData[] vd = new VolumeData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) vd[i] = new VolumeData(dt.Rows[i]);

            return vd;
        }

        public bool ClearPartWorkReport(int volumeID, int crID)
        {
            VolumeData vd = loadVolume(volumeID);
            return UpdateVolume(vd);
        }

        public bool InsertVolume(VolumeData vd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSql, conn);
                    cmd = parametersSqlDbType(cmd);
                    cmd = parametersValue(cmd, vd);

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


        public bool UpdateVolume(VolumeData vd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    if (vd.Department != "")
                    {
                        cmd = new SqlCommand(updSql, conn);
                    }
                    else
                    {
                        cmd = new SqlCommand(updSql1, conn);
                    }

                    cmd = parametersSqlDbType(cmd);
                    cmd = parametersValue(cmd, vd);

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

        public bool DeleteVolume(string strTaskCode, int intYearMonth, string strOfficeCode, string strDepartment)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(delSql, conn);
                    cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                    cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                    cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                    cmd.Parameters.Add("@Department", SqlDbType.Char);
                    cmd.Parameters["@TaskCode"].Value = TaskCode;
                    cmd.Parameters["@YearMonth"].Value = intYearMonth;
                    cmd.Parameters["@OfficeCode"].Value = strOfficeCode;
                    cmd.Parameters["@Department"].Value = strDepartment;

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

        public bool ExistenceTaskCodeYearMonth(string TaskCode, int intYearMonth, string strOfficeCode, string strDepartment)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    string strSelSql = selSql1;
                    if (strDepartment.Trim().ToString() != "")
                        strSelSql = selSql;

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strSelSql, conn);
                    if (strDepartment.Trim().ToString() != "")
                    {
                        cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                        cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                        cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                        cmd.Parameters.Add("@Department", SqlDbType.Char);
                        cmd.Parameters["@TaskCode"].Value = TaskCode;
                        cmd.Parameters["@YearMonth"].Value = intYearMonth;
                        cmd.Parameters["@OfficeCode"].Value = strOfficeCode;
                        cmd.Parameters["@Department"].Value = strDepartment;
                    }
                    else
                    {
                        cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                        cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                        cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                        cmd.Parameters["@TaskCode"].Value = TaskCode;
                        cmd.Parameters["@YearMonth"].Value = intYearMonth;
                        cmd.Parameters["@OfficeCode"].Value = strOfficeCode;
                    }

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

        private VolumeData loadVolume(int volumeID)
        {
            SqlHandling sh = new SqlHandling("D_Volume");
            DataTable dt = sh.SelectAllData("WHERE VolumeID = " + volumeID);
            if (dt == null || dt.Rows.Count < 1) return null;
            VolumeData vd = new VolumeData(dt.Rows[0]);
            return vd;
        }

        private SqlCommand parametersSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
            cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
            cmd.Parameters.Add("@Department", SqlDbType.Char);
            cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
            cmd.Parameters.Add("@MonthlyVolume", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolUncomp", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolClaimRem", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolClaim", SqlDbType.Decimal);
            cmd.Parameters.Add("@MonthlyClaim", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolPaid", SqlDbType.Decimal);
            cmd.Parameters.Add("@BalanceClaim", SqlDbType.Decimal);
            cmd.Parameters.Add("@BalanceIncom", SqlDbType.Decimal);
            cmd.Parameters.Add("@MonthlyCost", SqlDbType.Decimal);
            cmd.Parameters.Add("@ClaimDate", SqlDbType.Date);
            cmd.Parameters.Add("@PaidDate", SqlDbType.Date);
            cmd.Parameters.Add("@CarryOverPlanned", SqlDbType.Decimal);
            cmd.Parameters.Add("@Comment", SqlDbType.VarChar);
            cmd.Parameters.Add("@TaskStat", SqlDbType.Int);
            cmd.Parameters.Add("@Note", SqlDbType.NVarChar);
            return cmd;
        }


        private SqlCommand parametersValue(SqlCommand cmd, VolumeData vd)
        {
            cmd.Parameters["@TaskCode"].Value = vd.TaskCode;
            cmd.Parameters["@OfficeCode"].Value = vd.OfficeCode;
            cmd.Parameters["@Department"].Value = DBNull.Value;
            if (vd.Department != "")
                cmd.Parameters["@Department"].Value = vd.Department;
            cmd.Parameters["@YearMonth"].Value = vd.YearMonth;
            cmd.Parameters["@MonthlyVolume"].Value = DBNull.Value;
            cmd.Parameters["@MonthlyVolume"].Value = DBNull.Value;
            if (vd.MonthlyVolume != null)
                cmd.Parameters["@MonthlyVolume"].Value = vd.MonthlyVolume;
            cmd.Parameters["@VolUncomp"].Value = DBNull.Value;
            if (vd.VolUncomp != null)
                cmd.Parameters["@VolUncomp"].Value = vd.VolUncomp;
            cmd.Parameters["@VolClaimRem"].Value = DBNull.Value;
            if (vd.VolClaimRem != null)
                cmd.Parameters["@VolClaimRem"].Value = vd.VolClaimRem;
            cmd.Parameters["@VolClaim"].Value = DBNull.Value;
            if (vd.VolClaim != null)
                cmd.Parameters["@VolClaim"].Value = vd.VolClaim;
            cmd.Parameters["@MonthlyClaim"].Value = DBNull.Value;
            if (vd.MonthlyClaim != null)
                cmd.Parameters["@MonthlyClaim"].Value = vd.MonthlyClaim;
            cmd.Parameters["@VolPaid"].Value = DBNull.Value;
            if (vd.VolPaid != null)
                cmd.Parameters["@VolPaid"].Value = vd.VolPaid;
            cmd.Parameters["@BalanceClaim"].Value = DBNull.Value;
            if (vd.BalanceClaim != null)
                cmd.Parameters["@BalanceClaim"].Value = vd.BalanceClaim;
            cmd.Parameters["@BalanceIncom"].Value = DBNull.Value;
            if (vd.BalanceIncom != null)
                cmd.Parameters["@BalanceIncom"].Value = vd.BalanceIncom;
            cmd.Parameters["@MonthlyCost"].Value = DBNull.Value;
            if (vd.MonthlyCost != null)
                cmd.Parameters["@MonthlyCost"].Value = vd.MonthlyCost;
            cmd.Parameters["@ClaimDate"].Value = DBNull.Value;
            if (vd.ClaimDate != null)
                cmd.Parameters["@ClaimDate"].Value = vd.ClaimDate;
            cmd.Parameters["@PaidDate"].Value = DBNull.Value;
            if (vd.PaidDate != null)
                cmd.Parameters["@PaidDate"].Value = vd.PaidDate;
            cmd.Parameters["@CarryOverPlanned"].Value = DBNull.Value;
            if (vd.CarryOverPlanned != null)
                cmd.Parameters["@CarryOverPlanned"].Value = vd.CarryOverPlanned;
            cmd.Parameters["@Comment"].Value = vd.Comment;
            cmd.Parameters["@TaskStat"].Value = vd.TaskStat;
            cmd.Parameters["@Note"].Value = vd.Note;
            return cmd;
        }








        //public bool UpdateVolume(VolumeData vol)
        //{
        //    using (TransactionScope tran = new TransactionScope())
        //    using (SqlConnection conn = new SqlConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(updSql2, conn);

        //            cmd = parametersSqlDbType(cmd);
        //            cmd = parametersValue(cmd, vol);

        //            if (TryExecute(conn, cmd) < 0) return false;
        //        }
        //        catch (SqlException sqle)
        //        {
        //            MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
        //            conn.Close();
        //            return false;
        //        }
        //        conn.Close();
        //        tran.Complete();
        //    }
        //    return true;
        //}


        //public VolumeData SelectSummaryVolume(string officeCode, string department, DateTime yymm)
        //{
        //    VolumeData vold = new VolumeData();
        //    SqlHandling sh = new SqlHandling();
        //    DataTable dt = sh.SelectFullDescription(sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth = '" + yymm.EndOfMonth() + "'");
        //    if (dt == null) return vold;
        //    DataRow dr = dt.Rows[0];
        //    vold  = setVolumeData(dr);

        //    vold.OfficeCode = officeCode;
        //    vold.Department = department;
        //    vold.YearMonth = Convert.ToInt32(yymm);
        //    return vold;
        //}


        public VolumeData SelectSummaryVolume(string officeCode, string department, int yymm)
        {
            VolumeData vold = new VolumeData();
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth = " + yymm);
            //if (dt == null) return vold;
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            vold = setVolumeData(dr);

            vold.OfficeCode = officeCode;
            vold.Department = department;
            //vold.YearMonth = Convert.ToDateTime(yymm).EndOfMonth();
            vold.YearMonth = yymm;
            return vold;
        }


        //public VolumeData[] SelectSummaryVolume(string officeCode, string department)
        //{
        //    VolumeData[] vold;
        //    SqlHandling sh = new SqlHandling();
        //    DataTable dt = sh.SelectFullDescription(sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "'");
        //    if (dt == null)
        //    {
        //        vold = new VolumeData[1];
        //    }
        //    else
        //    {
        //        vold = new VolumeData[dt.Rows.Count];
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            vold[i] = (VolumeData)(setVolumeData(dt.Rows[i])).Clone();
        //            vold[i].OfficeCode = officeCode;
        //            vold[i].Department = department;
        //        }
        //    }

        //    return vold;
        //}


        public VolumeData setVolumeData(DataRow dr)
        {
            VolumeData vol = new VolumeData();

            vol.MonthlyVolume = (Convert.ToString(dr["sMV"]) == "") ? 0M : Convert.ToDecimal(dr["sMV"]);
            vol.VolUncomp = (Convert.ToString(dr["sVU"]) == "") ? 0M : Convert.ToDecimal(dr["sVU)"]);
            vol.VolClaimRem = (Convert.ToString(dr["sVCR"]) == "") ? 0M : Convert.ToDecimal(dr["sVCR"]);
            vol.VolClaim = (Convert.ToString(dr["sVC"]) == "") ? 0M : Convert.ToDecimal(dr["sVC"]);
            vol.MonthlyClaim = (Convert.ToString(dr["sMC"]) == "") ? 0M : Convert.ToDecimal(dr["sMC"]);
            vol.VolPaid = (Convert.ToString(dr["sVP"]) == "") ? 0M : Convert.ToDecimal(dr["sVP"]);
            vol.BalanceClaim = (Convert.ToString(dr["sBC"]) == "") ? 0M : Convert.ToDecimal(dr["sBC"]);
            vol.BalanceIncom = (Convert.ToString(dr["sBI"]) == "") ? 0M : Convert.ToDecimal(dr["sBI"]);
            vol.MonthlyCost = (Convert.ToString(dr["sMCO"]) == "") ? 0M : Convert.ToDecimal(dr["sMCO"]);
            vol.CarryOverPlanned = (Convert.ToString(dr["sCOP"]) == "") ? 0M : Convert.ToDecimal(dr["sCOP"]);

            return vol;
        }


        //private SqlCommand parametersSqlDbType(SqlCommand cmd)
        //{
        //    cmd.Parameters.Add("@vID", SqlDbType.Int);
        //    cmd.Parameters.Add("@tCod", SqlDbType.Char);
        //    cmd.Parameters.Add("@oCod", SqlDbType.Char);
        //    cmd.Parameters.Add("@dCod", SqlDbType.Char);
        //    cmd.Parameters.Add("@yM", SqlDbType.Date);
        //    cmd.Parameters.Add("@mVol", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@vUnc", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@vRem", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@vClm", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@mClm", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@vPai", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@bClm", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@bIcm", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@mCos", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@cDat", SqlDbType.Date);
        //    cmd.Parameters.Add("@pDat", SqlDbType.Date);
        //    cmd.Parameters.Add("@cOvr", SqlDbType.Decimal);
        //    cmd.Parameters.Add("@comt", SqlDbType.NVarChar);
        //    cmd.Parameters.Add("@tSt", SqlDbType.Int);
        //    return cmd;
        //}


        //private SqlCommand parametersValue(SqlCommand cmd, VolumeData vol) 
        //{
        //    cmd.Parameters["@vID"].Value = this.VolumeID;
        //    cmd.Parameters["@tCod"].Value = this.TaskCode;
        //    cmd.Parameters["@oCod"].Value = this.OfficeCode;
        //    cmd.Parameters["@dCod"].Value = this.Department;
        //    cmd.Parameters["@yM"].Value = this.YearMonth;
        //    cmd.Parameters["@mVol"].Value = this.MonthlyVolume;
        //    cmd.Parameters["@vUnc"].Value = this.VolUncomp;
        //    cmd.Parameters["@vRem"].Value = this.VolClaimRem;
        //    cmd.Parameters["@vClm"].Value = this.VolClaim;
        //    cmd.Parameters["@mClm"].Value = this.MonthlyClaim;
        //    cmd.Parameters["@vPai"].Value = this.VolPaid;
        //    cmd.Parameters["@bClm"].Value = this.BalanceClaim;
        //    cmd.Parameters["@bIcm"].Value = this.BalanceIncom;
        //    cmd.Parameters["@mCos"].Value = this.MonthlyCost;
        //    cmd.Parameters["@cDat"].Value = this.ClaimDate;
        //    cmd.Parameters["@pDat"].Value = this.PaidDate;
        //    cmd.Parameters["@cOvr"].Value = this.CarryOverPlanned;
        //    cmd.Parameters["@comt"].Value = this.Comment;
        //    cmd.Parameters["@tst"].Value = this.TaskStat;

        //    return cmd;
        //}























    }
}

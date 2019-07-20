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
                              + "(TaskCode, OfficeCode, Department, YearMonth, MonthlyVolume, VolUncomp, VolClaimRem, VolClaim, MonthlyClaim, VolPaid, BalanceClaim, BalanceIncom, MonthlyCost, ClaimDate, PaidDate, CarryOverPlanned, Comment, TaskStat, Note, Deposit1, Deposit2) VALUES ( "
                              + "@TaskCode, @OfficeCode, @Department, @YearMonth, @MonthlyVolume, @VolUncomp, @VolClaimRem, @VolClaim, @MonthlyClaim, @VolPaid, @BalanceClaim, @BalanceIncom, @MonthlyCost, @ClaimDate,@PaidDate, @CarryOverPlanned, @Comment, @TaskStat, @Note, @Deposit1, @Deposit2 )";

        private string updSql = "UPDATE D_Volume SET "
                                + "TaskCode = @TaskCode, OfficeCode = @OfficeCode, Department = @Department, YearMonth = @YearMonth, MonthlyVolume = @MonthlyVolume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, "
                                + "VolClaim = @VolClaim, MonthlyClaim = @MonthlyClaim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, BalanceIncom = @BalanceIncom, MonthlyCost = @MonthlyCost, ClaimDate = @ClaimDate, "
                                + "PaidDate = @PaidDate, CarryOverPlanned = @CarryOverPlanned, Comment = @Comment, TaskStat = @TaskStat, Note = @Note, Deposit1 = @Deposit1, Deposit2 = @Deposit2 "
                                + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";

        private string updSql1 = "UPDATE D_Volume SET "
                                + "TaskCode = @TaskCode, OfficeCode = @OfficeCode, Department = @Department, YearMonth = @YearMonth, MonthlyVolume = @MonthlyVolume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, "
                                + "VolClaim = @VolClaim, MonthlyClaim = @MonthlyClaim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, BalanceIncom = @BalanceIncom, MonthlyCost = @MonthlyCost, ClaimDate = @ClaimDate, "
                                + "PaidDate = @PaidDate, CarryOverPlanned = @CarryOverPlanned, Comment = @Comment, TaskStat = @TaskStat, Note = @Note, Deposit1 = @Deposit1, Deposit2 = @Deposit2 "
                                + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode";

        private string selSql = "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode";

        private string delSql = "DELETE FROM D_Volume WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";

        private string sumPara = "SUM(MonthlyVolume) AS sMV, SUM(VolUncomp) AS sVU, SUM(VolClaimRem) AS sVCR, SUM(VolClaim) AS sVC, "
                                + "SUM(MonthlyClaim) AS sMC, SUM(VolPaid) AS sVP, SUM(BalanceClaim) AS sBC, SUM(BalanceIncom) AS sBI, "
                                + "SUM(MonthlyCost) AS sMCO, SUM(CarryOverPlanned) AS sCOP, SUM(Deposit1) AS sDP1, SUM(Deposit2) AS sDP2 FROM D_Volume WHERE ";

        private string insSqlYear = "INSERT INTO D_YearVolume "
                              + "(TaskCode, OfficeCode, Department, YearMonth, Volume, VolUncomp, VolClaimRem, VolClaim, Claim, VolPaid, BalanceClaim, BalanceIncom, Deposit1, Cost, ClaimDate, PaidDate, Deposit2) "
                              + "VALUES (@TaskCode, @OfficeCode, @Department, @YearMonth, @Volume, @VolUncomp, @VolClaimRem, @VolClaim, @Claim, @VolPaid, @BalanceClaim, @BalanceIncom, @Deposit1, @Cost, @ClaimDate, @PaidDate, @Deposit2)";

        private string updSqlYear = "UPDATE D_YearVolume SET "
                        + "Volume = @Volume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, VolClaim = @VolClaim, Claim = @Claim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, "
                        + "BalanceIncom = @BalanceIncom, Deposit1 = @Deposit1, Cost = @Cost, ClaimDate = @ClaimDate, PaidDate = @PaidDate, Deposit2 = @Deposit2 "
                        + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode AND Department = @Department";

        private string updSqlYear1 = "UPDATE D_YearVolume SET "
                        + "Volume = @Volume, VolUncomp = @VolUncomp, VolClaimRem = @VolClaimRem, VolClaim = @VolClaim, Claim = @Claim, VolPaid = @VolPaid, BalanceClaim = @BalanceClaim, "
                        + "BalanceIncom = @BalanceIncom,, Deposit1 = @Deposit1 Cost = @Cost, ClaimDate = @ClaimDate, PaidDate = @PaidDate, Deposit2 = @Deposit2 "
                        + "WHERE TaskCode = @TaskCode AND YearMonth = @YearMonth AND OfficeCode = @OfficeCode";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public VolumeData()
        {
        }

        public VolumeData(DataRow dr)
        {
            TaskCode = dr.Field<String>("TaskCode") ?? "";
            OfficeCode = dr.Field<String>("OfficeCode") ?? "";
            Department = dr.Field<String>("Department") ?? "";
            YearMonth = dr.Field<Int32?>("YearMonth") ?? default(Int32);
            MonthlyVolume = dr.Field<Decimal?>("MonthlyVolume") ?? null;
            VolUncomp = dr.Field<Decimal?>("VolUncomp") ?? null;
            VolClaimRem = dr.Field<Decimal?>("VolClaimRem") ?? null;
            VolClaim = dr.Field<Decimal?>("VolClaim") ?? null;
            MonthlyClaim = dr.Field<Decimal?>("MonthlyClaim") ?? null;
            VolPaid = dr.Field<Decimal?>("VolPaid") ?? null;
            BalanceClaim = dr.Field<Decimal?>("BalanceClaim") ?? null;
            BalanceIncom = dr.Field<Decimal?>("BalanceIncom") ?? null;
            MonthlyCost = dr.Field<Decimal?>("MonthlyCost") ?? null;
            ClaimDate = dr.Field<DateTime?>("ClaimDate") ?? null;
            PaidDate = dr.Field<DateTime?>("PaidDate") ?? null;
            CarryOverPlanned = dr.Field<Decimal?>("CarryOverPlanned") ?? null;
            Comment = dr.Field<String>("Comment") ?? "";
            TaskStat = dr.Field<Int32?>("TaskStat") ?? default(Int32);
            Note = dr.Field<String>("Note") ?? "";
            Deposit1 = dr.Field<Decimal?>("Deposit1") ?? null;
            Deposit2 = dr.Field<Decimal?>("Deposit2") ?? null;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
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
        public decimal? Deposit1 { get; set; }
        public decimal? Deposit2 { get; set; }
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
            cloneData.Deposit1 = this.Deposit1;
            cloneData.Deposit2 = this.Deposit2;

            return cloneData;
        }


        public VolumeData[] SelectVolumeData(string taskCode, int yearMonth, string officeCode, string department)
        {
            SqlHandling sh = new SqlHandling("D_Volume");

            string sqlDepartment = "";
            if (department != "")
                sqlDepartment = " AND Department = '" + department + "'";
            DataTable dt = sh.SelectAllData("WHERE TaskCode = '" + taskCode + "' AND OfficeCode = '" + officeCode + "'" + sqlDepartment + " AND YearMonth = " + yearMonth);
            if (dt == null || dt.Rows.Count < 1) return null;

            VolumeData[] vd = new VolumeData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) vd[i] = new VolumeData(dt.Rows[i]);
            return vd;
        }


        public VolumeData[] SelectVolumeDate(string strSql)
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(strSql);
            if (dt == null || dt.Rows.Count < 1) return null;

            VolumeData[] vd = new VolumeData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) vd[i] = new VolumeData(dt.Rows[i]);

            return vd;
        }


        public string[] SelectCompleteTascCode( string officeCode, string departCode, string ymRange )
        {
            string strSql = " DISTINCT TaskCode FROM D_Volume WHERE TaskStat = 3 ";
            if( !string.IsNullOrEmpty( officeCode ) ) strSql += "AND OfficeCode = '" + officeCode + "' ";
            if( !string.IsNullOrEmpty( departCode ) ) strSql += "AND Department = '" + departCode + "' ";
            if( !string.IsNullOrEmpty( ymRange ) ) strSql += "AND YearMonth " + ymRange;
            strSql += "ORDER BY TaskCode";
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( strSql );
            if( dt == null || dt.Rows.Count < 1 ) return null;
            string[] completeTask = new string[dt.Rows.Count];
            DataRow dr;
            for( int i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dt.Rows[i];
                completeTask[i] = Convert.ToString( dr["TaskCode"] );

            }
            return completeTask;
        }


        public VolumeData[] SelectVolumeData(string officeCode, string department, string taskCode, int year, int preYear)
        {
            string sqlDepartment = "";
            if (!String.IsNullOrEmpty(department)) sqlDepartment = " AND Department =" + "'" + department + "'";
            string strSql = " NULL AS TaskCode, NULL AS YearMonth, NULL AS Publisher, ClaimDate, PaidDate, NULL AS Comment, NULL AS TaskStat, NULL AS CarryOverPlanned, NULL AS OfficeCode, NULL AS Department, NULL AS Note, "
                            + "Volume AS MonthlyVolume, VolUncomp, VolClaimRem, VolClaim, VolPaid, Claim AS MonthlyClaim, Cost AS MonthlyCost, BalanceClaim, BalanceIncom, Deposit1, Deposit2 FROM D_YearVolume "
                            + "WHERE TaskCode = " + "'" + taskCode + "' "
                            + "AND OfficeCode =" + "'" + officeCode + "' "
                            + sqlDepartment
                            + " AND YearMonth = " + "'" + preYear + "'";
            SqlHandling sh = new SqlHandling("D_YearVolume");
            DataTable dt = sh.SelectFullDescription(strSql);
            if (dt == null) return null;
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

        public bool DeleteVolume(string taskCode, int yearMonth, string officeCode, string department)
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
                    cmd.Parameters["@TaskCode"].Value = taskCode;
                    cmd.Parameters["@YearMonth"].Value = yearMonth;
                    cmd.Parameters["@OfficeCode"].Value = officeCode;
                    cmd.Parameters["@Department"].Value = department;

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

        // Wakamatsu 20170331
        public bool InsertYearVolume(VolumeData vd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insSqlYear, conn);
                    cmd = parametersSqlYearDbType(cmd);
                    cmd = parametersYearValue(cmd, vd);

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

        public bool UpdateYearVolume(VolumeData vd)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    if (vd.Department != "")
                        cmd = new SqlCommand(updSqlYear, conn);
                    else
                        cmd = new SqlCommand(updSqlYear1, conn);

                    cmd = parametersSqlYearDbType(cmd);
                    cmd = parametersYearValue(cmd, vd);

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

        //public bool ExistenceTaskCodeYearMonth(string taskCode, int yearMonth, string officeCode, string department)
        public bool ExistenceTaskCodeYearMonth(string TableName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (Department.Trim().ToString() != "")
                        selSql += " AND Department = @Department";

                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM " + TableName + " " + selSql, conn);

                    cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                    cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                    cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                    cmd.Parameters["@TaskCode"].Value = TaskCode;
                    cmd.Parameters["@YearMonth"].Value = YearMonth;
                    cmd.Parameters["@OfficeCode"].Value = OfficeCode;

                    if (Department.Trim().ToString() != "")
                    {
                        //cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                        //cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                        //cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                        cmd.Parameters.Add("@Department", SqlDbType.Char);
                        //cmd.Parameters["@TaskCode"].Value = taskCode;
                        //cmd.Parameters["@YearMonth"].Value = yearMonth;
                        //cmd.Parameters["@OfficeCode"].Value = officeCode;
                        //cmd.Parameters["@Department"].Value = department;
                        cmd.Parameters["@Department"].Value = Department;
                    }
                    //else
                    //{
                    //    cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
                    //    cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
                    //    cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
                    //    cmd.Parameters["@TaskCode"].Value = taskCode;
                    //    cmd.Parameters["@YearMonth"].Value = yearMonth;
                    //    cmd.Parameters["@OfficeCode"].Value = officeCode;
                    //}

                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (dr == null) return false;
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
            cmd.Parameters.Add("@Deposit1", SqlDbType.Decimal);
            cmd.Parameters.Add("@Deposit2", SqlDbType.Decimal);
            return cmd;
        }


        private SqlCommand parametersValue(SqlCommand cmd, VolumeData vd)
        {
            cmd.Parameters["@TaskCode"].Value = vd.TaskCode;
            cmd.Parameters["@OfficeCode"].Value = vd.OfficeCode;

            cmd.Parameters["@Department"].Value = DBNull.Value;
            if( vd.Department != "" ) cmd.Parameters["@Department"].Value = vd.Department;

            cmd.Parameters["@YearMonth"].Value = vd.YearMonth;

            cmd.Parameters["@MonthlyVolume"].Value = DBNull.Value;
            if (vd.MonthlyVolume != null) cmd.Parameters["@MonthlyVolume"].Value = vd.MonthlyVolume;

            cmd.Parameters["@VolUncomp"].Value = DBNull.Value;
            if (vd.VolUncomp != null) cmd.Parameters["@VolUncomp"].Value = vd.VolUncomp;

            cmd.Parameters["@VolClaimRem"].Value = DBNull.Value;
            if (vd.VolClaimRem != null) cmd.Parameters["@VolClaimRem"].Value = vd.VolClaimRem;

            cmd.Parameters["@VolClaim"].Value = DBNull.Value;
            if (vd.VolClaim != null) cmd.Parameters["@VolClaim"].Value = vd.VolClaim;

            cmd.Parameters["@MonthlyClaim"].Value = DBNull.Value;
            if (vd.MonthlyClaim != null) cmd.Parameters["@MonthlyClaim"].Value = vd.MonthlyClaim;

            cmd.Parameters["@VolPaid"].Value = DBNull.Value;
            if (vd.VolPaid != null) cmd.Parameters["@VolPaid"].Value = vd.VolPaid;

            cmd.Parameters["@BalanceClaim"].Value = DBNull.Value;
            if (vd.BalanceClaim != null) cmd.Parameters["@BalanceClaim"].Value = vd.BalanceClaim;

            cmd.Parameters["@BalanceIncom"].Value = DBNull.Value;
            if (vd.BalanceIncom != null) cmd.Parameters["@BalanceIncom"].Value = vd.BalanceIncom;

            cmd.Parameters["@MonthlyCost"].Value = DBNull.Value;
            if (vd.MonthlyCost != null) cmd.Parameters["@MonthlyCost"].Value = vd.MonthlyCost;

            cmd.Parameters["@ClaimDate"].Value = DBNull.Value;
            if (vd.ClaimDate != null) cmd.Parameters["@ClaimDate"].Value = vd.ClaimDate;

            cmd.Parameters["@PaidDate"].Value = DBNull.Value;
            if (vd.PaidDate != null) cmd.Parameters["@PaidDate"].Value = vd.PaidDate;

            cmd.Parameters["@CarryOverPlanned"].Value = DBNull.Value;
            if (vd.CarryOverPlanned != null) cmd.Parameters["@CarryOverPlanned"].Value = vd.CarryOverPlanned;

            cmd.Parameters["@Comment"].Value = vd.Comment;
            cmd.Parameters["@TaskStat"].Value = vd.TaskStat;
            cmd.Parameters["@Note"].Value = vd.Note;

            cmd.Parameters["@Deposit1"].Value = DBNull.Value;
            if (vd.Deposit1 != null) cmd.Parameters["@Deposit1"].Value = vd.Deposit1;

            cmd.Parameters["@Deposit2"].Value = DBNull.Value;
            if (vd.Deposit2 != null) cmd.Parameters["@Deposit2"].Value = vd.Deposit2;

            return cmd;
        }


        private SqlCommand parametersSqlYearDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@TaskCode", SqlDbType.Char);
            cmd.Parameters.Add("@OfficeCode", SqlDbType.Char);
            cmd.Parameters.Add("@Department", SqlDbType.Char);
            cmd.Parameters.Add("@YearMonth", SqlDbType.Int);
            cmd.Parameters.Add("@Volume", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolUncomp", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolClaimRem", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolClaim", SqlDbType.Decimal);
            cmd.Parameters.Add("@Claim", SqlDbType.Decimal);
            cmd.Parameters.Add("@VolPaid", SqlDbType.Decimal);
            cmd.Parameters.Add("@BalanceClaim", SqlDbType.Decimal);
            cmd.Parameters.Add("@BalanceIncom", SqlDbType.Decimal);
            cmd.Parameters.Add("@Deposit1", SqlDbType.Decimal);
            cmd.Parameters.Add("@Cost", SqlDbType.Decimal);
            cmd.Parameters.Add("@ClaimDate", SqlDbType.Date);
            cmd.Parameters.Add("@PaidDate", SqlDbType.Date);
            cmd.Parameters.Add("@Deposit2", SqlDbType.Decimal);
            return cmd;
        }

        private SqlCommand parametersYearValue(SqlCommand cmd, VolumeData vd)
        {
            cmd.Parameters["@TaskCode"].Value = vd.TaskCode;
            cmd.Parameters["@OfficeCode"].Value = vd.OfficeCode;

            cmd.Parameters["@Department"].Value = DBNull.Value;
            if (vd.Department != "") cmd.Parameters["@Department"].Value = vd.Department;

            cmd.Parameters["@YearMonth"].Value = vd.YearMonth;

            cmd.Parameters["@Volume"].Value = DBNull.Value;
            if (vd.MonthlyVolume != null) cmd.Parameters["@Volume"].Value = vd.MonthlyVolume;

            cmd.Parameters["@VolUncomp"].Value = DBNull.Value;
            if (vd.VolUncomp != null) cmd.Parameters["@VolUncomp"].Value = vd.VolUncomp;

            cmd.Parameters["@VolClaimRem"].Value = DBNull.Value;
            if (vd.VolClaimRem != null) cmd.Parameters["@VolClaimRem"].Value = vd.VolClaimRem;

            cmd.Parameters["@VolClaim"].Value = DBNull.Value;
            if (vd.VolClaim != null) cmd.Parameters["@VolClaim"].Value = vd.VolClaim;

            cmd.Parameters["@Claim"].Value = DBNull.Value;
            if (vd.MonthlyClaim != null) cmd.Parameters["@Claim"].Value = vd.MonthlyClaim;

            cmd.Parameters["@VolPaid"].Value = DBNull.Value;
            if (vd.VolPaid != null) cmd.Parameters["@VolPaid"].Value = vd.VolPaid;

            cmd.Parameters["@BalanceClaim"].Value = DBNull.Value;
            if (vd.BalanceClaim != null) cmd.Parameters["@BalanceClaim"].Value = vd.BalanceClaim;

            cmd.Parameters["@BalanceIncom"].Value = DBNull.Value;
            if (vd.BalanceIncom != null) cmd.Parameters["@BalanceIncom"].Value = vd.BalanceIncom;

            cmd.Parameters["@Deposit1"].Value = DBNull.Value;
            if (vd.Deposit1 != null) cmd.Parameters["@Deposit1"].Value = vd.Deposit1;

            cmd.Parameters["@Cost"].Value = DBNull.Value;
            if (vd.MonthlyCost != null) cmd.Parameters["@Cost"].Value = vd.MonthlyCost;

            cmd.Parameters["@ClaimDate"].Value = DBNull.Value;
            if (vd.ClaimDate != null) cmd.Parameters["@ClaimDate"].Value = vd.ClaimDate;

            cmd.Parameters["@PaidDate"].Value = DBNull.Value;
            if (vd.PaidDate != null) cmd.Parameters["@PaidDate"].Value = vd.PaidDate;

            cmd.Parameters["@Deposit2"].Value = DBNull.Value;
            if (vd.Deposit2 != null) cmd.Parameters["@Deposit2"].Value = vd.Deposit2;

            return cmd;
        }


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
            vold.YearMonth = yymm;
            return vold;
        }


        public VolumeData SelectSummaryVolume(string officeCode, string department, string ymPara)
        {
            VolumeData vold = new VolumeData();
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription( sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth " + ymPara );
            //DataTable dt = sh.SelectFullDescription( sumPara + " OfficeCode = '" + officeCode + "' AND Department = '" + department + "' AND YearMonth " + ymPara + " AND TaskStat != 3" ); // 休止以外
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            vold = setVolumeData(dr);

            vold.OfficeCode = officeCode;
            vold.Department = department;
            return vold;
        }


        public VolumeData setVolumeData(DataRow dr)
        {
            VolumeData vol = new VolumeData();

            vol.MonthlyVolume = (Convert.ToString(dr["sMV"]) == "") ? 0M : Convert.ToDecimal(dr["sMV"]);
            vol.VolUncomp = (Convert.ToString(dr["sVU"]) == "") ? 0M : Convert.ToDecimal(dr["sVU"]);
            vol.VolClaimRem = (Convert.ToString(dr["sVCR"]) == "") ? 0M : Convert.ToDecimal(dr["sVCR"]);
            vol.VolClaim = (Convert.ToString(dr["sVC"]) == "") ? 0M : Convert.ToDecimal(dr["sVC"]);
            vol.MonthlyClaim = (Convert.ToString(dr["sMC"]) == "") ? 0M : Convert.ToDecimal(dr["sMC"]);
            vol.VolPaid = (Convert.ToString(dr["sVP"]) == "") ? 0M : Convert.ToDecimal(dr["sVP"]);
            vol.BalanceClaim = (Convert.ToString(dr["sBC"]) == "") ? 0M : Convert.ToDecimal(dr["sBC"]);
            vol.BalanceIncom = (Convert.ToString(dr["sBI"]) == "") ? 0M : Convert.ToDecimal(dr["sBI"]);
            vol.MonthlyCost = (Convert.ToString(dr["sMCO"]) == "") ? 0M : Convert.ToDecimal(dr["sMCO"]);
            vol.CarryOverPlanned = (Convert.ToString(dr["sCOP"]) == "") ? 0M : Convert.ToDecimal(dr["sCOP"]);
            vol.Deposit1 = (Convert.ToString(dr["sDP1"]) == "") ? 0M : Convert.ToDecimal(dr["sDP1"]);
            vol.Deposit2 = (Convert.ToString(dr["sDP2"]) == "") ? 0M : Convert.ToDecimal(dr["sDP2"]);

            return vol;
        }

    }
}

using ClassLibrary;
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
    public class EstPlanOp : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;
        StringUtility util = new StringUtility();
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public EstPlanOp()
        {
        }

        public EstPlanOp(string tableName)
        {
            this.tableName = tableName;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public string NumberingOrder(string officeCode, string department)
        {
            string orderNo = department;
            if (officeCode != "H") orderNo = "A";

            orderNo += util.TruncateByteRight(Convert.ToString(DHandling.FiscalYear(DateTime.Today)), 2);
            orderNo += officeCode;
            SqlHandling sh = new SqlHandling("M_Office");
            DataTable dt = sh.SelectAllData("WHERE OfficeCode = '" + officeCode + "'");
            if (dt == null) return null;
            DataRow dr;
            dr = dt.Rows[0];
            int orderSeqNo = Convert.ToInt32(dr["OrderSeqNo"]);
            orderSeqNo++;
            orderNo += orderSeqNo.ToString("000");

            if (!office_Update(officeCode, orderSeqNo)) return null;

            return orderNo;
        }


        private bool office_Update( string officeCode, int orderSeqNo )
        {
            string sqlStr = "UPDATE M_Office SET OrderSeqNo = @orderSeq WHERE OfficeCode = @oCode";

            using( TransactionScope tran = new TransactionScope() )
            using( SqlConnection conn = new SqlConnection( ConnectionString ) )
            {
                try
                {
                    conn.Open();
                    using( SqlCommand cmd = new SqlCommand( sqlStr, conn ) )
                    {
                        cmd.Parameters.Add( "@orderSeq", SqlDbType.Int );
                        cmd.Parameters.Add( "@oCode", SqlDbType.Char );
                        cmd.Parameters["@orderSeq"].Value = orderSeqNo;
                        cmd.Parameters["@oCode"].Value = officeCode;
                        if( TryExecute( conn, cmd ) < 0 ) return false;
                    }
                }
                catch( SqlException sqle )
                {
                    MessageBox.Show( "SQLエラー errorno " + Convert.ToString( sqle.Number ) + " " + sqle.Message );
                    conn.Close();
                    return false;
                }
                conn.Close();
                tran.Complete();
            }
            return true;
        }


        public int[] CrtVersionNoArray()
        {
            int[] verArray;
            DataTable dt = GetDataTable("SELECT  VersionNo  FROM " + tableName);
            if (dt == null || dt.Rows.Count == 0)
            {
                verArray = new int[] { -1 };
            }
            else
            {
                DataRow dr;
                verArray = new int[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    verArray[i] = Convert.ToInt32(dr["VersionNo"]);
                }
            }

            return verArray;
        }


        public WorkItemsData[] StoreWorkItemsData(string mCode)
        {
            SqlHandling sh = new SqlHandling("M_WorkItems");
            DataTable dt = sh.SelectAllData("WHERE MemberCode = '" + mCode + "'");
            if (dt == null || dt.Rows.Count < 1)
            {
                return StoreWorkItemsData();
            }
            else
            {
                WorkItemsData[] wid = new WorkItemsData[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    wid[i] = new WorkItemsData(dt.Rows[i]);
                }
                return wid;
            }
        }


        public WorkItemsData[] StoreWorkItemsData()
        {
            SqlHandling sh = new SqlHandling("M_WorkItems");
            DataTable dt = sh.SelectAllData("WHERE MemberCode = '000'");
            if (dt == null || dt.Rows.Count < 1)
            {
                MessageBox.Show("M_WorkItemsが未登録です。");
                return null;
            }

            WorkItemsData[] wid = new WorkItemsData[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                wid[i] = new WorkItemsData(dt.Rows[i]);
            }
            return wid;
        }


        public WorkItemsData LoadWorkItemsData(WorkItemsData[] wid, string itemCode)
        {
            for (int i = 0; i < wid.Length; i++)
            {
                if (wid[i].ItemCode == itemCode)
                {
                    if (wid[i].Item == "" && wid[i].ItemDetail == "") wid[i].StdCost = 0;
                    WorkItemsData wids = (WorkItemsData)wid[i].Clone();
                    return wids;
                }
            }
            return null;
        }


        // 見積・予算Table読込
        public DataTable EstPlan_Select( int taskEntryID )
        {
            if( taskEntryID == 0 ) return null;
            return SelectAllData_Core( tableName, " WHERE TaskEntryID = " + taskEntryID );
        }


        public DataTable EstPlan_Select(int taskEntryID, int verNo)
        {
            if (taskEntryID == 0) return null;
            string wParam = "WHERE TaskEntryID = " + taskEntryID + " AND VersionNo = " + verNo;
            return SelectAllData_Core(tableName, wParam);
        }


        // 見積Table読込
        public DataTable Estimate_Select(int taskEntryID, int verNo)
        {
            tableName = "D_Estimate";
            return EstPlan_Select( taskEntryID, verNo );
        }


        // 見積内訳Table読込
        public DataTable EstimateCont_Select(int estimateID)
        {
            string wParam = "WHERE EstimateID = " + Convert.ToString(estimateID) + " ORDER BY LNo";
            return SelectAllData_Core("D_EstimateCont", wParam);
        }


        public bool EstimateCont_StoreDgv(DataTable dt, DataGridView dgv)
        {
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                storeItemsToDgv(dgv.Rows[i], dr);
                if (Convert.ToString(dr["Unit"]) != "") storeCostsToDgv(dgv.Rows[i], dr);
                dgv.Rows[i].Cells["Note"].Value = Convert.ToString(dr["Note"]);
            }
            return true;
        }


        public bool EstimateCont_StoreDgvForPlan(DataTable dt, DataGridView dgv)
        {
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                storeItemsToDgv(dgv.Rows[i], dr);
                if (Convert.ToString(dr["Unit"]) != "") storeCostsToDgvExcCost(dgv.Rows[i], dr);
            }
            return true;
        }


        public bool Estimate_Update(EstimateData estd)
        {
            if (estd == null) return false;

            string sqlStr = "UPDATE D_Estimate SET TaskEntryID = @tenID, VersionNo = @verN, Total = @total, Budgets = @bget, MinimalBid = @mBid,"
                            + "Contract = @cont, OfficeCode = @oCod, Department = @dept, Note=@note WHERE EstimateID = @estID";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = estim_cmd(cmd, estd);
                    cmd.Parameters.Add("@estID", SqlDbType.Int);
                    cmd.Parameters["@estID"].Value = Convert.ToInt32(estd.EstimateID);
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


        public int Estimate_Insert(EstimateData estd)
        {
            if (estd == null) return -1;

            string sqlStr = "INSERT INTO D_Estimate (TaskEntryID, VersionNo, Total, Budgets, MinimalBid, Contract, OfficeCode, Department, Note) "
                           + "VALUES (@tenID, @verN, @total, @bget, @mBid, @cont, @oCod, @dept, @note)";

            string sqlStr2 = "SELECT EstimateID FROM D_Estimate WHERE TaskEntryID = " + Convert.ToString(estd.TaskEntryID)
                            + " AND VersionNo = " + Convert.ToString(estd.VersionNo)
                            + " AND OfficeCode = '" + estd.OfficeCode + "' AND Department = '" + estd.Department + "'";
            int id = -1;

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = estim_cmd(cmd, estd);
                    if (TryExecute(conn, cmd) < 0) return -1;

                    cmd = new SqlCommand(sqlStr2, conn);
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (dr == null) return -1;
                    while (dr.Read())
                        id = Convert.ToInt32(dr["EstimateID"]);
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            return id;
        }


        public bool Estimate_Delete(int estimateID)
        {
            string sqlStr = "DELETE FROM D_Estimate WHERE EstimateID = " + Convert.ToString(estimateID);
            return UsingTryExecute(sqlStr);
        }


        public bool EstimateCont_Delete(int estimateID)
        {
            string sqlStr = "DELETE FROM D_EstimateCont WHERE EstimateID = " + Convert.ToString(estimateID);
            return UsingTryExecute(sqlStr);
        }


        public bool EstimateCont_Insert(DataGridView dgv, int estimateID)
        {
            string sqlStr = "INSERT INTO D_EstimateCont (EstimateID, LNo, ItemCode, Item, ItemDetail, Quantity, Unit, Cost, Note) "
               + "VALUES (@estID, @lNo, @iCod, @item, @iDtl, @qty, @unit, @cost, @note)";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = estContParaAdd(cmd);
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        // Wakamatsu 20170324
                        //if ( dgv.Rows[i].Cells["Item"].Value == null && dgv.Rows[i].Cells["ItemDetail"].Value == null ) break;
                        // Wakamatsu 20170330
                        //if (dgv.Rows[i].Cells["Item"].Value != null && dgv.Rows[i].Cells["ItemDetail"].Value != null)
                        if (dgv.Rows[i].Cells["Item"].Value != null || dgv.Rows[i].Cells["ItemDetail"].Value != null)
                        {
                            cmd = estContParaValue(cmd, dgv.Rows[i]);
                            cmd.Parameters["@estID"].Value = estimateID;
                            cmd.Parameters["@lNo"].Value = i + 1;
                            if (TryExecute(conn, cmd) < 0) return false;
                        }
                    }
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


        public bool EstimateCont_Update(DataGridView dgv, int estimateID)
        {
            EstimateCont_Delete(estimateID);
            EstimateCont_Insert(dgv, estimateID);
            return true;
        }


        private SqlCommand estim_cmd(SqlCommand cmd, EstimateData estd)
        {
            cmd.Parameters.Add("@tenID", SqlDbType.Int);
            cmd.Parameters.Add("@verN", SqlDbType.Int);
            cmd.Parameters.Add("@total", SqlDbType.Decimal);
            cmd.Parameters.Add("@bget", SqlDbType.Decimal);
            cmd.Parameters.Add("@mBid", SqlDbType.Decimal);
            cmd.Parameters.Add("@cont", SqlDbType.Decimal);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters["@tenID"].Value = estd.TaskEntryID;
            cmd.Parameters["@verN"].Value = estd.VersionNo;
            cmd.Parameters["@total"].Value = estd.Total;
            cmd.Parameters["@bget"].Value = estd.Budgets;
            cmd.Parameters["@mBid"].Value = estd.MinimalBid;
            cmd.Parameters["@cont"].Value = estd.Contract;
            cmd.Parameters["@oCod"].Value = estd.OfficeCode;
            cmd.Parameters["@dept"].Value = estd.Department;
            cmd.Parameters["@note"].Value = estd.Note;

            return cmd;
        }


        private SqlCommand estContParaAdd(SqlCommand cmd)
        {
            cmd.Parameters.Add("@estID", SqlDbType.Int);
            cmd.Parameters.Add("@lNo", SqlDbType.Int);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@qty", SqlDbType.Decimal);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cost", SqlDbType.Decimal);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            return cmd;
        }


        static private SqlCommand estContParaValue(SqlCommand cmd, DataGridViewRow dgvr)
        {
            cmd.Parameters["@iCod"].Value = Convert.ToString(dgvr.Cells["ItemCode"].Value);
            cmd.Parameters["@item"].Value = Convert.ToString(dgvr.Cells["Item"].Value);
            cmd.Parameters["@iDtl"].Value = Convert.ToString(dgvr.Cells["ItemDetail"].Value);
            cmd.Parameters["@qty"].Value = Convert.ToDecimal(dgvr.Cells["Quantity"].Value);
            cmd.Parameters["@unit"].Value = Convert.ToString(dgvr.Cells["Unit"].Value);
            // Wakamatsu 20170324
            //cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells["Cost"].Value));

            Calculation calc = new Calculation();
            if (calc.ExtractCalcWord(Convert.ToString(dgvr.Cells["Item"].Value)) == Sign.Discount)
                // Wakamatsu 20170327
                //cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells["Amount"].Value));
                cmd.Parameters["@cost"].Value = SignConvert(Convert.ToString(dgvr.Cells["Amount"].Value));
            // Wakamatsu 20170327
            else
                cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells["Cost"].Value));
            // Wakamatsu 20170324
            cmd.Parameters["@note"].Value = Convert.ToString(dgvr.Cells["Note"].Value);
            return cmd;
        }


        // 予算
        public bool PlanningCont_StoreDgv(DataTable dt, DataGridView dgv)
        {
            DataRow dr;
            decimal qty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                storeItemsToDgv(dgv.Rows[i], dr);
                qty = Convert.ToDecimal(dr["Quantity"]);
                if (Convert.ToString(dr["Unit"]) != "")
                {
                    dgv.Rows[i].Cells["Quantity"].Value = qty;
                    dgv.Rows[i].Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
                    dgv.Rows[i].Cells["Cost0"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Cost0"]), "#,0");
                    dgv.Rows[i].Cells["Amount0"].Value = DHandling.DecimaltoStr(qty * Convert.ToDecimal(dr["Cost0"]), "#,0");
                    dgv.Rows[i].Cells["Cost1"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Cost1"]), "#,0");
                    dgv.Rows[i].Cells["Amount1"].Value = DHandling.DecimaltoStr(qty * Convert.ToDecimal(dr["Cost1"]), "#,0");
                    dgv.Rows[i].Cells["Cost2"].Value = DHandling.DecimaltoStr(Convert.ToDecimal(dr["Cost2"]), "#,0");
                    dgv.Rows[i].Cells["Amount2"].Value = DHandling.DecimaltoStr(qty * Convert.ToDecimal(dr["Cost2"]), "#,0");
                }
            }
            return true;
        }


        public DataTable Planning_Select(int taskEntryID, int verNo)
        {
            if (taskEntryID == 0) return null;
            string wParam = "WHERE TaskEntryID = " + taskEntryID + " AND VersionNo = " + verNo;
            return SelectAllData_Core("D_Planning", wParam);
        }


        public DataTable Planning_Select_Latest(int taskEntryID)
        {
            if (taskEntryID == 0) return null;
            string wParam = "WHERE TaskEntryID = " + taskEntryID + " ORDER BY VersionNo DESC";
            DataTable dt = SelectAllData_Core("D_Planning", wParam);
            if (dt == null) return null;
            if (dt.Rows.Count < 1) return null;
            return dt;
        }

        


        public DataTable PlanningCont_Select(int planningID)
        {
            string wParam = "WHERE PlanningID = " + planningID + " ORDER BY LNo";
            return SelectAllData_Core("D_PlanningCont", wParam);
        }


        public DataTable PlanningCont_Select(int planningID, string cond)
        {
            string wParam = "WHERE PlanningID = " + planningID + " AND " + cond + " ORDER BY LNo";
            return SelectAllData_Core("D_PlanningCont", wParam);
        }


        public bool Planning_Update(PlanningData plnd)
        {
            if (plnd == null) return false;

            ////string sqlStr = "UPDATE D_Planning SET TaskEntryID = @tenID, VersionNo = @verN,"
            //string sqlStr = "UPDATE D_Planning SET VersionNo = @verN,"
            //                + " Sales = @sales, Budgets = @bget, Discussion = @disc, OfficeCode = @oCod, Department = @dept"
            //                + " WHERE PlanningID = @plnID";

            string sqlStr = "UPDATE D_Planning SET VersionNo = @verN,"
                            + " Sales = @sales, Budgets = @bget, Discussion = @disc,"
                            + " OfficeCode = @oCod, Department = @dept,"
                            + " CreateStat = @cSta, CreateMCd = @cMCd, CreateDate = @cDat,"
                            + " ConfirmStat = @fSta, ConfirmMCd = @fMCd, ConfirmDate = @fDat,"
                            + " ScreeningStat = @sSta, ScreeningMCd = @sMCd, ScreeningDate = @sDat,"
                            + " ApOfficerStat = @oSta, ApOfficerMCd = @oMCd, ApOfficerDate = @oDat,"
                            + " ApPresidentStat = @pSta, ApPresidentMCd = @pMCd, ApPresidentDate = @pDat,"
                            + " ProxyStat = @xSta, ProxyMCd = @xMCd, ProxyDate = @xDat"
                            + " WHERE PlanningID = @plnID";
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = planning_cmd(cmd, plnd);
                    cmd.Parameters.Add("@plnID", SqlDbType.Int);
                    cmd.Parameters["@plnID"].Value = plnd.PlanningID;
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


        public int Planning_Insert(PlanningData plnd)
        {
            if (plnd == null) return -1;

            //string sqlStr = "INSERT INTO D_Planning (TaskEntryID, VersionNo, Sales, Budgets, Discussion, OfficeCode, Department) "
            //                + "VALUES (@tenID, @verN, @sales, @bget, @disc, @oCod, @dept)";

            string sqlStr = "INSERT INTO D_Planning (TaskEntryID, VersionNo, Sales, Budgets, Discussion, OfficeCode, Department, "
                            + "CreateStat, CreateMCd, CreateDate, ConfirmStat, ConfirmMCd, ConfirmDate, ScreeningStat, ScreeningMCd, ScreeningDate, "
                            + "ApOfficerStat, ApOfficerMCd, ApOfficerDate, ApPresidentStat, ApPresidentMCd, ApPresidentDate, ProxyStat, ProxyMCd, ProxyDate) "
                            + "VALUES (@tenID, @verN, @sales, @bget, @disc, @oCod, @dept,"
                            + "@cSta, @cMCd, @cDat, @fSta, @fMCd, @fDat, @sSta, @sMCd, @sDat, @oSta, @oMCd, @oDat, @pSta, @pMCd, @pDat, @xSta, @xMCd, @xDat)";

            string sqlStr2 = "SELECT PlanningID FROM D_Planning WHERE TaskEntryID = " +plnd.TaskEntryID
                            + " AND VersionNo = " + plnd.VersionNo
                            + " AND OfficeCode = '" + plnd.OfficeCode + "' AND Department = '" + plnd.Department + "'";
            int id = -1;

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = planning_cmd(cmd, plnd);
                    if (TryExecute(conn, cmd) < 0) return -1;
                    cmd = new SqlCommand(sqlStr2, conn);
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (dr == null) return -1;
                    while (dr.Read())
                        id = Convert.ToInt32(dr["PlanningID"]);
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            return id;
        }


        public bool Planning_Delete(int planningID)
        {
            string sqlStr = "DELETE FROM D_Planning WHERE PlanningID = " + Convert.ToString(planningID);
            return UsingTryExecute(sqlStr);
        }


        public bool PlanningCont_Delete(int planningID)
        {
            string sqlStr = "DELETE FROM D_PlanningCont WHERE PlanningID = " + Convert.ToString(planningID);
            return UsingTryExecute(sqlStr);
        }


        public bool PlanningCont_Insert(DataGridView dgv, int planningID)
        {
            string sqlStr = "INSERT INTO D_PlanningCont (PlanningID, LNo, ItemCode, Item, ItemDetail, Quantity, Unit, Cost0, Cost1, Cost2) "
                            + "VALUES (@plnID, @lNo, @iCod, @item, @iDtl, @qty, @unit, @cost0, @cost1, @cost2)";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = planningContParaAdd(cmd);
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        // Wakamatsu 20170329
                        //if( dgv.Rows[i].Cells["Item"].Value == null && dgv.Rows[i].Cells["ItemDetail"].Value == null ) break;
                        if (dgv.Rows[i].Cells["Item"].Value != null && dgv.Rows[i].Cells["ItemDetail"].Value != null)
                        {
                            cmd = planningContParaValue(cmd, dgv.Rows[i]);
                            cmd.Parameters["@plnID"].Value = planningID;
                            cmd.Parameters["@lNo"].Value = i + 1;
                            if (TryExecute(conn, cmd) < 0) return false;
                        }
                    }
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

        // Wakamatsu 20170330
        public bool PlanningCont_Insert(DataTable dt, int planningID)
        {
            string sqlStr = "INSERT INTO D_PlanningCont (PlanningID, LNo, ItemCode, Item, ItemDetail, Quantity, Unit, Cost0, Cost1, Cost2) "
                            + "VALUES (@plnID, @lNo, @iCod, @item, @iDtl, @qty, @unit, @cost0, @cost1, @cost2)";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    Calculation calc = new Calculation();

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = planningContParaAdd(cmd);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["Item"] != null || dr["ItemDetail"] != null)
                        {
                            if (calc.ExtractCalcWord(Convert.ToString(dr["Item"])) == null)
                            {
                                cmd = planningContParaValue(cmd, dr);
                                cmd.Parameters["@plnID"].Value = planningID;
                                cmd.Parameters["@lNo"].Value = i + 1;
                                if (TryExecute(conn, cmd) < 0) return false;
                            }
                        }
                    }
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
        // Wakamatsu 20170330

        public bool PlanningCont_Update(DataGridView dgv, int planningID)
        {
            PlanningCont_Delete(planningID);
            PlanningCont_Insert(dgv, planningID);
            return true;
        }


        private SqlCommand planning_cmd(SqlCommand cmd, PlanningData plnd)
        {
            cmd.Parameters.Add("@tenID", SqlDbType.Int);
            cmd.Parameters.Add("@verN", SqlDbType.Int);
            cmd.Parameters.Add("@sales", SqlDbType.Decimal);
            cmd.Parameters.Add("@bget", SqlDbType.Decimal);
            cmd.Parameters.Add("@disc", SqlDbType.NVarChar);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@cSta", SqlDbType.Int);
            cmd.Parameters.Add("@cMCd", SqlDbType.Char);
            cmd.Parameters.Add("@cDat", SqlDbType.Date);
            cmd.Parameters.Add("@fSta", SqlDbType.Int);
            cmd.Parameters.Add("@fMCd", SqlDbType.Char);
            cmd.Parameters.Add("@fDat", SqlDbType.Date);
            cmd.Parameters.Add("@sSta", SqlDbType.Int);
            cmd.Parameters.Add("@sMCd", SqlDbType.Char);
            cmd.Parameters.Add("@sDat", SqlDbType.Date);
            cmd.Parameters.Add("@oSta", SqlDbType.Int);
            cmd.Parameters.Add("@oMCd", SqlDbType.Char);
            cmd.Parameters.Add("@oDat", SqlDbType.Date);
            cmd.Parameters.Add("@pSta", SqlDbType.Int);
            cmd.Parameters.Add("@pMCd", SqlDbType.Char);
            cmd.Parameters.Add("@pDat", SqlDbType.Date);
            cmd.Parameters.Add("@xSta", SqlDbType.Int);
            cmd.Parameters.Add("@xMCd", SqlDbType.Char);
            cmd.Parameters.Add("@xDat", SqlDbType.Date);

            cmd.Parameters["@tenID"].Value = plnd.TaskEntryID;
            cmd.Parameters["@verN"].Value = plnd.VersionNo;
            cmd.Parameters["@sales"].Value = plnd.Sales;
            cmd.Parameters["@bget"].Value = plnd.Budgets;
            cmd.Parameters["@disc"].Value = string.IsNullOrEmpty( plnd.Discussion ) ? "" : plnd.Discussion;
            cmd.Parameters["@oCod"].Value = plnd.OfficeCode;
            cmd.Parameters["@dept"].Value = plnd.Department;
            cmd.Parameters["@cSta"].Value = plnd.CreateStat;
            cmd.Parameters["@cMCd"].Value = string.IsNullOrEmpty( plnd.CreateMCd ) ? "" : plnd.CreateMCd;
            cmd.Parameters["@cDat"].Value = plnd.CreateDate;
            cmd.Parameters["@fSta"].Value = plnd.ConfirmStat;
            cmd.Parameters["@fMCd"].Value = string.IsNullOrEmpty( plnd.ConfirmMCd ) ? "" : plnd.ConfirmMCd;
            cmd.Parameters["@fDat"].Value = plnd.ConfirmDate;
            cmd.Parameters["@sSta"].Value = plnd.ScreeningStat;
            cmd.Parameters["@sMCd"].Value = string.IsNullOrEmpty( plnd.ScreeningMCd ) ? "" : plnd.ScreeningMCd;
            cmd.Parameters["@sDat"].Value = plnd.ScreeningDate;
            cmd.Parameters["@oSta"].Value = plnd.ApOfficerStat;
            cmd.Parameters["@oMCd"].Value = string.IsNullOrEmpty( plnd.ApOfficerMCd ) ? "" : plnd.ApOfficerMCd;
            cmd.Parameters["@oDat"].Value = plnd.ApOfficerDate;
            cmd.Parameters["@pSta"].Value = plnd.ApPresidentStat;
            cmd.Parameters["@pMCd"].Value = string.IsNullOrEmpty( plnd.ApPresidentMCd ) ? "" : plnd.ApPresidentMCd;
            cmd.Parameters["@pDat"].Value = plnd.ApPresidentDate;
            cmd.Parameters["@xSta"].Value = plnd.ProxyStat;
            cmd.Parameters["@xMCd"].Value = string.IsNullOrEmpty( plnd.ProxyMCd ) ? "" : plnd.ProxyMCd;
            cmd.Parameters["@xDat"].Value = plnd.ProxyDate;
            return cmd;
        }


        private SqlCommand planningContParaAdd(SqlCommand cmd)
        {
            cmd.Parameters.Add("@plnID", SqlDbType.Int);
            cmd.Parameters.Add("@lNo", SqlDbType.Int);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@qty", SqlDbType.Decimal);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cost0", SqlDbType.Decimal);
            cmd.Parameters.Add("@cost1", SqlDbType.Decimal);
            cmd.Parameters.Add("@cost2", SqlDbType.Decimal);
            return cmd;
        }


        static private SqlCommand planningContParaValue(SqlCommand cmd, DataGridViewRow dgvr)
        {
            cmd.Parameters["@iCod"].Value = Convert.ToString(dgvr.Cells[1].Value);
            cmd.Parameters["@item"].Value = Convert.ToString(dgvr.Cells[2].Value);
            cmd.Parameters["@iDtl"].Value = Convert.ToString(dgvr.Cells[3].Value);
            cmd.Parameters["@qty"].Value = Convert.ToDecimal(dgvr.Cells[4].Value);
            cmd.Parameters["@unit"].Value = Convert.ToString(dgvr.Cells[5].Value);
            // Wakamatsu 20170329
            //cmd.Parameters["@cost0"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvr.Cells[6].Value ) );
            //cmd.Parameters["@cost1"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvr.Cells[8].Value ) );
            //cmd.Parameters["@cost2"].Value = DHandling.ToRegDecimal( Convert.ToString( dgvr.Cells[10].Value ) );

            Calculation calc = new Calculation();
            if (calc.ExtractCalcWord(Convert.ToString(dgvr.Cells["Item"].Value)) == Sign.Discount)
            {
                cmd.Parameters["@cost0"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[7].Value));
                cmd.Parameters["@cost1"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[9].Value));
                cmd.Parameters["@cost2"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[11].Value));
            }
            else
            {
                cmd.Parameters["@cost0"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[6].Value));
                cmd.Parameters["@cost1"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[8].Value));
                cmd.Parameters["@cost2"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells[10].Value));
            }
            // Wakamatsu 20170329
            return cmd;
        }

        // Wakamatsu 20170330
        static private SqlCommand planningContParaValue(SqlCommand cmd, DataRow dr)
        {
            cmd.Parameters["@iCod"].Value = Convert.ToString(dr["ItemCode"]);
            cmd.Parameters["@item"].Value = Convert.ToString(dr["Item"]);
            cmd.Parameters["@iDtl"].Value = Convert.ToString(dr["ItemDetail"]);
            cmd.Parameters["@qty"].Value = Convert.ToDecimal(dr["Quantity"]);
            cmd.Parameters["@unit"].Value = Convert.ToString(dr["Unit"]);
            cmd.Parameters["@cost0"].Value = Convert.ToString(dr["Cost"]);
            cmd.Parameters["@cost1"].Value = 0;
            cmd.Parameters["@cost2"].Value = 0;

            return cmd;
        }
        // Wakamatsu 20170330

        public bool OutsourceCont_StoreDgv(DataTable dt, DataGridView dgv)
        {
            DataRow dr;
            decimal qty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                storeItemsToDgv(dgv.Rows[i], dr);
                if (Convert.ToDecimal(dr["Cost1"]) == 0)
                {
                    qty = 0;
                }
                else
                {
                    qty = Convert.ToDecimal(dr["Quantity"]);
                }
                if (Convert.ToString(dr["Unit"]) != "")
                {
                    dgv.Rows[i].Cells["Quantity"].Value = qty;
                    dgv.Rows[i].Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
                }
            }
            return true;
        }


        public DataTable SelectOutsourceCont(OutsourceData osd)
        {
            string wParam = "WHERE OutsourceID = " + Convert.ToString(osd.OutsourceID) + " ORDER BY LNo";
            return SelectAllData_Core("D_OutsourceCont", wParam);
        }


        public int InsertOutsource(OutsourceData osd)
        {
            if (osd == null) return -1;

            string sqlStr = "INSERT INTO D_Outsource "
                            + "(TaskEntryID, VersionNo, OrderNo, PartnerCode, PayRoule, Amount, "
                            + "OrderDate, StartDate, EndDate, InspectDate, ReceiptDate, Place, Note, OfficeCode, Department, OrderFlag) "
                            + "VALUES (@tenID, @verN, @order, @pCode, @pRoule, @amt, @oDate, @sDate, @eDate, @iDate, @rDate, @place, @note, @oCod, @dept, @oFlag)"
                            + ";SELECT CAST(scope_identity() AS int)";

            Int32 newProdID = -1;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = outsource_cmd(cmd, osd);
                    newProdID = TryExScalar(conn, cmd);
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return -1;
                }
                conn.Close();
                tran.Complete();
            }
            //return id;
            return (int)newProdID;
        }


        public bool UpdateOutsource(OutsourceData osd)
        {
            if (osd == null) return false;

            string sqlStr = "UPDATE D_Outsource SET TaskEntryID = @tenID, VersionNo = @verN, OrderNo = @order, PartnerCode = @pCode, PayRoule = @pRoule, Amount = @amt,"
                            + " OrderDate = @oDate, StartDate = @sDate, EndDate = @eDate, InspectDate = @iDate, ReceiptDate = @rDate,"
                            + " Place = @place, Note = @note, OfficeCode = @oCod, Department = @dept, OrderFlag = @oFlag WHERE OutsourceID = @osID";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = outsource_cmd(cmd, osd);
                    cmd.Parameters.Add("@osID", SqlDbType.Int);
                    cmd.Parameters["@osID"].Value = osd.OutsourceID;
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


        public bool DeleteOutsource(int outsourceID)
        {
            string sqlStr = "DELETE FROM D_Outsource WHERE outsourceID = " + Convert.ToString(outsourceID);
            return UsingTryExecute(sqlStr);
        }


        public bool DeleteOutsourceCont(int outsourceID)
        {
            string sqlStr = "DELETE FROM D_OutsourceCont WHERE OutsourceID = " + Convert.ToString(outsourceID);
            return UsingTryExecute(sqlStr);
        }


        public bool InsertOutsourceCont(DataGridView dgv, int outsourceID)
        {
            string sqlStr = "INSERT INTO D_OutsourceCont (OutsourceID, LNo, ItemCode, Item, ItemDetail, Quantity, Unit, Cost, Note) "
                            + "VALUES (@osID, @lNo, @iCod, @item, @iDtl, @qty, @unit, @cost, @note)";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = outsourceContParaAdd(cmd);
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (dgv.Rows[i].Cells["Item"].Value == null && dgv.Rows[i].Cells["ItemDetail"].Value == null) break;
                        cmd = outsourceContParaValue(cmd, dgv.Rows[i]);
                        cmd.Parameters["@osID"].Value = outsourceID;
                        cmd.Parameters["@lNo"].Value = i + 1;
                        if (TryExecute(conn, cmd) < 0) return false;
                    }
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


        public bool UpdateOutsourceCont(DataGridView dgv, int outsourceID)
        {
            DeleteOutsourceCont(outsourceID);
            InsertOutsourceCont(dgv, outsourceID);
            return true;
        }


        private SqlCommand outsource_cmd(SqlCommand cmd, OutsourceData osd)
        {
            cmd.Parameters.Add("@tenID", SqlDbType.Int);
            cmd.Parameters.Add("@verN", SqlDbType.Int);
            cmd.Parameters.Add("@order", SqlDbType.Char);
            cmd.Parameters.Add("@pCode", SqlDbType.Char);
            cmd.Parameters.Add("@pRoule", SqlDbType.Int);
            cmd.Parameters.Add("@amt", SqlDbType.Decimal);
            cmd.Parameters.Add("@oDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@sDate", SqlDbType.Date);
            cmd.Parameters.Add("@eDate", SqlDbType.Date);
            cmd.Parameters.Add("@iDate", SqlDbType.Date);
            cmd.Parameters.Add("@rDate", SqlDbType.Date);
            cmd.Parameters.Add("@place", SqlDbType.NVarChar);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            cmd.Parameters.Add("@oCod", SqlDbType.Char);
            cmd.Parameters.Add("@dept", SqlDbType.Char);
            cmd.Parameters.Add("@oFlag", SqlDbType.Int);
            cmd.Parameters["@tenID"].Value = osd.TaskEntryID;
            cmd.Parameters["@verN"].Value = osd.VersionNo;
            cmd.Parameters["@order"].Value = osd.OrderNo;
            cmd.Parameters["@pCode"].Value = osd.PartnerCode;
            cmd.Parameters["@pRoule"].Value = osd.PayRoule;
            cmd.Parameters["@amt"].Value = osd.Amount;
            cmd.Parameters["@oDate"].Value = osd.OrderDate;
            cmd.Parameters["@sDate"].Value = osd.StartDate;
            cmd.Parameters["@eDate"].Value = osd.EndDate;
            cmd.Parameters["@iDate"].Value = osd.InspectDate;
            cmd.Parameters["@rDate"].Value = osd.ReceiptDate;
            cmd.Parameters["@place"].Value = osd.Place;
            cmd.Parameters["@note"].Value = osd.Note;
            cmd.Parameters["@oCod"].Value = osd.OfficeCode;
            cmd.Parameters["@dept"].Value = osd.Department;
            cmd.Parameters["@oFlag"].Value = osd.OrderFlag;
            return cmd;
        }


        private SqlCommand outsourceContParaAdd(SqlCommand cmd)
        {
            cmd.Parameters.Add("@osID", SqlDbType.Int);
            cmd.Parameters.Add("@lNo", SqlDbType.Int);
            cmd.Parameters.Add("@iCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters.Add("@iDtl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@qty", SqlDbType.Decimal);
            cmd.Parameters.Add("@unit", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cost", SqlDbType.Decimal);
            cmd.Parameters.Add("@note", SqlDbType.NVarChar);
            return cmd;
        }


        static private SqlCommand outsourceContParaValue(SqlCommand cmd, DataGridViewRow dgvr)
        {
            cmd.Parameters["@iCod"].Value = Convert.ToString(dgvr.Cells["ItemCode"].Value);
            cmd.Parameters["@item"].Value = Convert.ToString(dgvr.Cells["Item"].Value);
            cmd.Parameters["@iDtl"].Value = Convert.ToString(dgvr.Cells["ItemDetail"].Value);
            cmd.Parameters["@qty"].Value = Convert.ToDecimal(dgvr.Cells["Quantity"].Value);
            cmd.Parameters["@unit"].Value = Convert.ToString(dgvr.Cells["Unit"].Value);
            cmd.Parameters["@cost"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells["Cost"].Value));
            cmd.Parameters["@note"].Value = DHandling.ToRegDecimal(Convert.ToString(dgvr.Cells["Note"].Value));
            return cmd;
        }


        //----------------------------------------------------------------------------//
        /*     SubRoutine                                                             */
        //----------------------------------------------------------------------------//
        private void storeItemsToDgv(DataGridViewRow dgvR, DataRow dr)
        {
            dgvR.Cells["ItemCode"].Value = Convert.ToString(dr["ItemCode"]);
            storeItemsToDgvExcItemCode(dgvR, dr);
        }

        private void storeItemsToDgvExcItemCode(DataGridViewRow dgvR, DataRow dr)
        {
            dgvR.Cells["Item"].Value = Convert.ToString(dr["Item"]);
            dgvR.Cells["ItemDetail"].Value = Convert.ToString(dr["ItemDetail"]);
        }

        private void storeCostsToDgv(DataGridViewRow dgvR, DataRow dr)
        {
            storeCostsToDgvExcCost(dgvR, dr);
            dgvR.Cells["Cost"].Value = Convert.ToDecimal(dr["Cost"]);
        }

        private void storeCostsToDgvExcCost(DataGridViewRow dgvR, DataRow dr)
        {
            dgvR.Cells["Quantity"].Value = Convert.ToDecimal(dr["Quantity"]);
            dgvR.Cells["Unit"].Value = Convert.ToString(dr["Unit"]);
        }

        // Wakamatsu 20170327
        /// <summary>
        /// "△" → "-"変換
        /// </summary>
        /// <param name="TargetValue">対象値</param>
        /// <returns>変換結果</returns>
        private static decimal SignConvert(object TargetValue)
        {
            decimal WorkDecimal = 0;
            string WorkString = Convert.ToString(TargetValue);

            if (WorkString != "")
            {
                // "△" → "-"コンバート
                if (WorkString.Substring(0, 1) == "△")
                {
                    Decimal.TryParse(WorkString.Substring(1), out WorkDecimal);
                    return WorkDecimal * -1;
                }
                else
                {
                    Decimal.TryParse(WorkString, out WorkDecimal);
                    return WorkDecimal;
                }
            }
            return 0;
        }
        // Wakamatsu 20170327
    }
}

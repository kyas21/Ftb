using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class TaskData : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string[] paramArray = new string[]
                                    { "@tID", "@tBas", "@tNam", "@aLvl", "@vNo", "@iDat", "@tPlc", "@pCod", "@sDat", "@eDat",
                                      "@pNot","@tOff", "@tLdr", "@telN", "@faxN", "@eMl", "@aPrc", "@aEst", "@aDsn", "@aCon",
                                      "@aOdr","@aSta", "@aOtr", "@aCon", "@oNot", "@cSpc", "@eSpc", "@oSpc", "@sCon", "@sCod",
                                      //"@appr", "@mOdr", "@inNo", "@conf", "@oBud", "@oPln", "@cTyp", "@conS", "@oFrm", "@cFrm", "@oVM" };
                                      "@appr", "@mOdr", "@inNo", "@conf", "@sCDt", "@apDt", "@mODt", "@iNDt", "@cfDt",
                                      "@oBud", "@oPln", "@cTyp", "@conS", "@oFrm", "@cFrm", "@oVM", "@isMk", "@oTyp" };
        
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskData()
        {
        }

        public TaskData(DataRow dr)
        {
            TaskID = dr.Field<Int32?>("TaskID") ?? default(Int32);
            TaskBaseCode = dr.Field<String>("TaskBaseCode") ?? default(String);
            TaskName = dr.Field<String>("TaskName") ?? default(String);
            AdmLevel = dr.Field<Int32?>("AdmLevel") ?? default(Int32);
            VersionNo = dr.Field<Int32?>("VersionNo") ?? default(Int32);
            //IssueDate = (Convert.ToString(dr["IssueDate"]) == "") ? DateTime.MinValue : Convert.ToDateTime(dr["IssueDate"]);
            IssueDate = dr.Field<DateTime?>("IssueDate") ?? DateTime.MinValue;
            TaskPlace = dr.Field<String>("TaskPlace") ?? default(String);
            PartnerCode = dr.Field<String>("PartnerCode") ?? default(String);
            //StartDate = (Convert.ToString(dr["StartDate"]) == "") ? DateTime.MinValue : Convert.ToDateTime(dr["StartDate"]);
            StartDate = dr.Field<DateTime?>("StartDate") ?? DateTime.MinValue;
            //EndDate = (Convert.ToString(dr["EndDate"]) == "") ? DateTime.MinValue : Convert.ToDateTime(dr["EndDate"]);
            EndDate = dr.Field<DateTime?>("EndDate") ?? DateTime.MinValue;
            PayNote = dr.Field<String>("PayNote") ?? default(String);
            TaskOffice = dr.Field<String>("TaskOffice") ?? default(String);
            TaskLeader = dr.Field<String>("TaskLeader") ?? default(String);
            TelNo = dr.Field<String>("TelNo") ?? default(String);
            FaxNo = dr.Field<String>("FaxNo") ?? default(String);
            EMail = dr.Field<String>("EMail") ?? default(String);
            AttProceed = dr.Field<Int32?>("AttProceed") ?? default(Int32);
            AttEstimate = dr.Field<Int32?>("AttEstimate") ?? default(Int32);
            AttDesign = dr.Field<Int32?>("AttDesign") ?? default(Int32);
            AttContract = dr.Field<Int32?>("AttContract") ?? default(Int32);
            AttOrder = dr.Field<Int32?>("AttOrder") ?? default(Int32);
            AttStart = dr.Field<Int32?>("AttStart") ?? default(Int32);
            AttOther = dr.Field<Int32?>("AttOther") ?? default(Int32);
            AttOtherCont = dr.Field<String>("AttOtherCont") ?? default(String);
            OrderNote = dr.Field<String>("OrderNote") ?? default(String);
            CommonSpec = dr.Field<Int32?>("CommonSpec") ?? default(Int32);
            ExclusiveSpec = dr.Field<Int32?>("ExclusiveSpec") ?? default(Int32);
            OtherSpec = dr.Field<Int32?>("OtherSpec") ?? default(Int32);
            SpecCont = dr.Field<String>("SpecCont") ?? default(String);
            SalesMCode = dr.Field<String>("SalesMCode") ?? default(String);
            Approval = dr.Field<String>("Approval") ?? default(String);
            MakeOrder = dr.Field<String>("MakeOrder") ?? default(String);
            InputNumber = dr.Field<String>("InputNumber") ?? default(String);
            ConfirmAdm = dr.Field<String>("ConfirmAdm") ?? default(String);

            SalesMInputDate = dr.Field<DateTime?>("SalesMInputDate") ?? DateTime.MinValue;
            ApprovalDate = dr.Field<DateTime?>("ApprovalDate") ?? DateTime.MinValue;
            MakeOrderDate = dr.Field<DateTime?>("MakeOrderDate") ?? DateTime.MinValue;
            InputNoDate = dr.Field<DateTime?>("InputNoDate") ?? DateTime.MinValue;
            ConfirmDate = dr.Field<DateTime?>("ConfirmDate") ?? DateTime.MinValue;

            OrderBudget = dr.Field<Int32?>("OrderBudget") ?? default(Int32);
            OrderPlanning = dr.Field<Int32?>("OrderPlanning") ?? default(Int32);
            CostType = dr.Field<String>("CostType") ?? default(String);
            ConstructionStat = dr.Field<Int32?>("ConstructionStat") ?? default(Int32);
            OrdersForm = dr.Field<Int32?>("OrdersForm") ?? default(Int32);
            ClaimForm = dr.Field<Int32?>("ClaimForm") ?? default(Int32);
            OldVerMark = dr.Field<Int32?>("OldVerMark") ?? default(Int32);
            IssueMark = dr.Field<Int32?>("IssueMark") ?? default(Int32);
            OrdersType = dr.Field<Int32?>("OrdersType") ?? default(Int32);

        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int TaskID { get; set; }
        public string TaskBaseCode { get; set; }
        public string TaskName { get; set; }
        public int AdmLevel { get; set; }
        public int VersionNo { get; set; }
        public DateTime IssueDate { get; set; }
        public string TaskPlace { get; set; }
        public string PartnerCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PayNote { get; set; }
        public string TaskOffice { get; set; }
        public string TaskLeader { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string EMail { get; set; }
        public int AttProceed { get; set; }
        public int AttEstimate { get; set; }
        public int AttDesign { get; set; }
        public int AttContract { get; set; }
        public int AttOrder { get; set; }
        public int AttStart { get; set; }
        public int AttOther { get; set; }
        public string AttOtherCont { get; set; }
        public string OrderNote { get; set; }
        public int CommonSpec { get; set; }
        public int ExclusiveSpec { get; set; }
        public int OtherSpec { get; set; }
        public string SpecCont { get; set; }
        public string SalesMCode { get; set; }
        public string Approval { get; set; }
        public string MakeOrder { get; set; }
        public string InputNumber { get; set; }
        public string ConfirmAdm { get; set; }

        public DateTime SalesMInputDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime MakeOrderDate { get; set; }
        public DateTime InputNoDate { get; set; }
        public DateTime ConfirmDate { get; set; }

        public int OrderBudget { get; set; }
        public int OrderPlanning { get; set; }
        public string CostType { get; set; }
        public int ConstructionStat { get; set; }
        public int OrdersForm { get; set; }
        public int ClaimForm { get; set; }
        public int OldVerMark { get; set; }
        public int IssueMark { get; set; }
        public int OrdersType { get; set; }


        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public object Clone()
        {
            TaskData cloneData = new TaskData();

            cloneData.TaskID = this.TaskID;
            cloneData.TaskBaseCode = this.TaskBaseCode;
            cloneData.TaskName = this.TaskName;
            cloneData.AdmLevel = this.AdmLevel;
            cloneData.VersionNo = this.VersionNo;
            cloneData.IssueDate = this.IssueDate;
            cloneData.TaskPlace = this.TaskPlace;
            cloneData.PartnerCode = this.PartnerCode;
            cloneData.StartDate = this.StartDate;
            cloneData.EndDate = this.EndDate;
            cloneData.PayNote = this.PayNote;
            cloneData.TaskOffice = this.TaskOffice;
            cloneData.TaskLeader = this.TaskLeader;
            cloneData.TelNo = this.TelNo;
            cloneData.FaxNo = this.FaxNo;
            cloneData.EMail = this.EMail;
            cloneData.AttProceed = this.AttProceed;
            cloneData.AttEstimate = this.AttEstimate;
            cloneData.AttDesign = this.AttDesign;
            cloneData.AttContract = this.AttContract;
            cloneData.AttOrder = this.AttOrder;
            cloneData.AttStart = this.AttStart;
            cloneData.AttOther = this.AttOther;
            cloneData.AttOtherCont = this.AttOtherCont;
            cloneData.OrderNote = this.OrderNote;
            cloneData.CommonSpec = this.CommonSpec;
            cloneData.ExclusiveSpec = this.ExclusiveSpec;
            cloneData.OtherSpec = this.OtherSpec;
            cloneData.SpecCont = this.SpecCont;
            cloneData.SalesMCode = this.SalesMCode;
            cloneData.Approval = this.Approval;
            cloneData.MakeOrder = this.MakeOrder;
            cloneData.InputNumber = this.InputNumber;
            cloneData.ConfirmAdm = this.ConfirmAdm;

            cloneData.SalesMInputDate = this.SalesMInputDate;
            cloneData.ApprovalDate = this.ApprovalDate;
            cloneData.MakeOrderDate = this.MakeOrderDate;
            cloneData.InputNoDate = this.InputNoDate;
            cloneData.ConfirmDate = this.ConfirmDate;

            cloneData.OrderBudget = this.OrderBudget;
            cloneData.OrderPlanning = this.OrderPlanning;
            cloneData.CostType = this.CostType;
            cloneData.ConstructionStat = this.ConstructionStat;
            cloneData.OrdersForm = this.OrdersForm;
            cloneData.ClaimForm = this.ClaimForm;
            cloneData.OldVerMark = this.OldVerMark;
            cloneData.IssueMark = this.IssueMark;
            cloneData.OrdersType = this.OrdersType;
            return cloneData;
        }


        public string CreateTaskBaseCode(string officeCode)
        {
            int seqNo = 0;
            string taskBaseCode = "";
            string selectSql = "SELECT OrderSeqNo FROM M_Office WHERE OfficeCode = @oCod";
            string updateSql = "UPDATE M_Office SET OrderSeqNo = @oSeq WHERE OfficeCode = @oCod";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(selectSql, conn);
                    cmd.Parameters.Add("@oCod", SqlDbType.Char);
                    SqlCommand cmd1 = new SqlCommand(updateSql, conn);
                    cmd1.Parameters.Add("@oCod", SqlDbType.Char);
                    cmd1.Parameters.Add("@oSeq", SqlDbType.Int);

                    cmd.Parameters["@oCod"].Value = officeCode;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (!dr.HasRows) return null;
                    while (dr.Read())
                    {
                        seqNo = Convert.ToInt32(dr["OrderSeqNo"]);
                    }
                    dr.Close();

                    cmd1.Parameters["@oCod"].Value = officeCode;
                    seqNo++;
                    cmd1.Parameters["@oSeq"].Value = seqNo;
                    if (TryExecute(conn, cmd1) < 0) return null;
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return null;
                }
                conn.Close();
                tran.Complete();
            }

            DateTime nowdt = DateTime.Now;
            taskBaseCode = (DHandling.FiscalYear(nowdt) - 2000).ToString() + officeCode + seqNo.ToString("000");
            return taskBaseCode;
        }

        public int CheckTaskBaseCode(string taskBaseCode)
        {
            // 20180628 asakawa 自動採番から手動採番への改修に伴い、新たに追加した関数
            // ユーザーによって指定された基本業務番号がすでに登録済でないかどうかをチェックする。
            // 重複している場合は１を、していない場合は０を、エラーは２を返す。

            int retValue = 1;
            string selectSql = "SELECT TaskID FROM D_Task WHERE TaskBaseCode = @oCod AND OldVerMark = 0";

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(selectSql, conn);
                    cmd.Parameters.Add("@oCod", SqlDbType.Char);

                    cmd.Parameters["@oCod"].Value = taskBaseCode;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (!dr.HasRows)
                        retValue = 0; // 重複なし
                    else
                        retValue = 1; // すでに登録済
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    retValue = 2;
                }
                conn.Close();
                tran.Complete();
            }

            return retValue;
        }


        public TaskData SelectTaskData(int taskID)
        {
            SqlHandling sh = new SqlHandling("D_Task");
            DataTable dt = sh.SelectAllData("WHERE TaskID = " + taskID);
            if (dt == null || dt.Rows.Count < 1)
            {
                DMessage.DataNotExistence("D_Task TaskID = " + Convert.ToString(taskID));
                return null;
            }
            TaskData td = new TaskData(dt.Rows[0]);
            return td;
        }


        public TaskData SelectTaskData( int taskID, int oldVerMark )
        {
            SqlHandling sh = new SqlHandling( "D_Task" );
            DataTable dt = sh.SelectAllData( "WHERE OldVerMark = " + oldVerMark + " AND TaskID = " + taskID );
            if( dt == null || dt.Rows.Count < 1 )
            {
                DMessage.DataNotExistence( "D_Task TaskID = " + Convert.ToString( taskID ) );
                return null;
            }
            TaskData td = new TaskData( dt.Rows[0] );
            return td;
        }


        public TaskData SelectTaskData(string taskCode)
        {
            StringUtility str = new StringUtility();
            SqlHandling sh = new SqlHandling("D_Task");
            DataTable dt = sh.SelectAllData("WHERE TaskBaseCode = '" + str.SubstringByte(taskCode,1,6) + "'");
            if (dt == null || dt.Rows.Count < 1)
            {
                DMessage.DataNotExistence("D_Task TaskBaseCode = " + str.SubstringByte(taskCode,1,6));
                return null;
            }
            TaskData td = new TaskData(dt.Rows[0]);
            return td;
        }


        public TaskData[] SelectTaskDataByPara( string param )
        {
            SqlHandling sh = new SqlHandling( "D_Task" );
            DataTable dt = sh.SelectAllData( param );
            if( dt == null || dt.Rows.Count < 1 ) return null;

            TaskData[] rtd = new TaskData[dt.Rows.Count];
            for( int i = 0; i < dt.Rows.Count; i++ ) rtd[i] = new TaskData( dt.Rows[i] );      

            return rtd;
        }


        public bool UpdateOldVerMark(string tbleName, int taskID, int oldVerMark)
        {
            string sqlStr = "UPDATE D_Task SET OldVerMark = @oVM WHERE TaskID = @tID";
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.Parameters.Add("@oVM", SqlDbType.Int);
                    cmd.Parameters.Add("@tID", SqlDbType.Int);
                    cmd.Parameters["@oVM"].Value = oldVerMark;
                    cmd.Parameters["@tID"].Value = taskID;
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


        public bool UpdateTaskData()
        {
            string sqlStr = "UPDATE D_Task SET "
                        + "TaskName = @tNam, AdmLevel = @aLvl, VersionNo = @vNo, IssueDate = @isDt, TaskPlace = @tPla, "
                        + "PartnerCode = @pCod, StartDate = @stDt, EndDate = @enDt, PayNote = @pNot, TaskOffice = @tOff, "
                        + "TaskLeader = @tLdr, TelNo = @telN, FaxNo = @faxN, Email = @eMl, AttProceed = @aPrc, "
                        + "AttEstimate = @aEst, AttDesign = @aDsn, AttContract = @aCon, AttOrder = @aOdr, AttStart = @aSta, "
                        + "AttOther = @aOtr, AttOtherCont = @aOCo, OrderNote = @oNot, CommonSpec = @cSpc, ExclusiveSpec = @eSpc, "
                        + "OtherSpec = @oSpc, SpecCont = @sCon, SalesMCode = @sCod, Approval = @appr, MakeOrder = @mOdr, "
                        + "InputNumber = @inNo, ConfirmAdm = @conf, SalesMInputDate = @sCDt, ApprovalDate = @apDt, MakeOrderDate = @mODt, "
                        + "InputNoDate = @iNDt, ConfirmDate = @cfDt, "
                        + "OrderBudget = @oBud, OrderPlanning = @oPln, CostType =@cTyp, "
                        + "ConstructionStat = @conS, OrdersForm = @oFrm, ClaimForm = @cFrm, OldVerMark = @oVM, IssueMark = @isMk, OrdersType = @oTyp "
                        + "WHERE TaskID = @tID";
            if (!executeProcess(sqlStr)) return false;

            return true;
        }


        public int InsertTaskData()
        {
            string sqlStr = "INSERT INTO D_Task ("
                        + "TaskBaseCode, TaskName, AdmLevel, VersionNo, IssueDate, TaskPlace, PartnerCode, StartDate, EndDate, PayNote, "
                        + "TaskOffice, TaskLeader, TelNo, FaxNo, Email, AttProceed, AttEstimate, AttDesign, AttContract, AttOrder, "
                        + "AttStart, AttOther, AttOtherCont, OrderNote, CommonSpec, ExclusiveSpec, OtherSpec, SpecCont, SalesMCode, Approval, "
                        + "MakeOrder, InputNumber, ConfirmAdm, SalesMInputDate, ApprovalDate, MakeOrderDate, InputNoDate, ConfirmDate, "
                        + "OrderBudget, OrderPlanning, CostType, ConstructionStat, OrdersForm, ClaimForm, OldVerMark, IssueMark, OrdersType"
                        + " ) VALUES ("
                        + "@tBas, @tNam, @aLvl, @vNo, @isDt, @tPla, @pCod, @stDt, @enDt, @pNot, "
                        + "@tOff, @tLdr, @telN, @faxN, @eMl, @aPrc, @aEst, @aDsn, @aCon, @aOdr, "
                        + "@aSta, @aOtr, @aOCo, @oNot, @cSpc, @eSpc, @oSpc, @sCon, @sCod, @appr, "
                        + "@mOdr, @inNo, @conf, @sCDt, @apDt, @mODt, @iNDt, @cfDt, "
                        + "@oBud, @oPln, @cTyp, @conS, @oFrm, @cFrm, @oVM, @isMk, @oTyp )";

            if (!executeProcess(sqlStr)) return -1;

            // Insertしたもの再読み込み
            string taskExistence = "SELECT * FROM D_Task WHERE TaskBaseCode = @tBas AND OldVerMark = 0";
            int newTaskID = -1;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(taskExistence, conn);
                    cmd.Parameters.Add("@tBas", SqlDbType.Char);
                    cmd.Parameters["@tBas"].Value = TaskBaseCode;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            newTaskID = Convert.ToInt32(dr["TaskID"]);
                        }
                    }
                    dr.Close();
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    return -1;
                }
                conn.Close();
            }
            return newTaskID;
        }


        private bool executeProcess(string sqlStr)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd = addSqlDbType(cmd);
                    cmd = addValue(cmd);

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


        private SqlCommand addSqlDbType(SqlCommand cmd)
        {
            cmd.Parameters.Add("@tID", SqlDbType.Int);
            cmd.Parameters.Add("@tBas", SqlDbType.Char);
            cmd.Parameters.Add("@tNam", SqlDbType.NVarChar);
            cmd.Parameters.Add("@aLvl", SqlDbType.Int);
            cmd.Parameters.Add("@vNo", SqlDbType.Int);
            cmd.Parameters.Add("@isDt", SqlDbType.Date);
            cmd.Parameters.Add("@tPla", SqlDbType.NVarChar);
            cmd.Parameters.Add("@pCod", SqlDbType.Char);
            cmd.Parameters.Add("@stDt", SqlDbType.Date);
            cmd.Parameters.Add("@enDt", SqlDbType.Date);
            cmd.Parameters.Add("@pNot", SqlDbType.NVarChar);
            cmd.Parameters.Add("@tOff", SqlDbType.NVarChar);
            cmd.Parameters.Add("@tLdr", SqlDbType.NVarChar);
            cmd.Parameters.Add("@telN", SqlDbType.Char);
            cmd.Parameters.Add("@faxN", SqlDbType.Char);
            cmd.Parameters.Add("@eMl", SqlDbType.VarChar);
            cmd.Parameters.Add("@aPrc", SqlDbType.Int);
            cmd.Parameters.Add("@aEst", SqlDbType.Int);
            cmd.Parameters.Add("@aDsn", SqlDbType.Int);
            cmd.Parameters.Add("@aCon", SqlDbType.Int);
            cmd.Parameters.Add("@aOdr", SqlDbType.Int);
            cmd.Parameters.Add("@aSta", SqlDbType.Int);
            cmd.Parameters.Add("@aOtr", SqlDbType.Int);
            cmd.Parameters.Add("@aOCo", SqlDbType.NVarChar);
            cmd.Parameters.Add("@oNot", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cSpc", SqlDbType.Int);
            cmd.Parameters.Add("@eSpc", SqlDbType.Int);
            cmd.Parameters.Add("@oSpc", SqlDbType.Int);
            cmd.Parameters.Add("@sCon", SqlDbType.NVarChar);
            cmd.Parameters.Add("@sCod", SqlDbType.Char);
            cmd.Parameters.Add("@appr", SqlDbType.Char);
            cmd.Parameters.Add("@mOdr", SqlDbType.Char);
            cmd.Parameters.Add("@inNo", SqlDbType.Char);
            cmd.Parameters.Add("@conf", SqlDbType.Char);

            cmd.Parameters.Add("@sCDt", SqlDbType.Date);
            cmd.Parameters.Add("@apDt", SqlDbType.Date);
            cmd.Parameters.Add("@mODt", SqlDbType.Date);
            cmd.Parameters.Add("@iNDt", SqlDbType.Date);
            cmd.Parameters.Add("@cfDt", SqlDbType.Date);

            cmd.Parameters.Add("@oBud", SqlDbType.Int);
            cmd.Parameters.Add("@oPln", SqlDbType.Int);
            cmd.Parameters.Add("@cTyp", SqlDbType.NVarChar);
            cmd.Parameters.Add("@conS", SqlDbType.Int);
            cmd.Parameters.Add("@oFrm", SqlDbType.Int);
            cmd.Parameters.Add("@cFrm", SqlDbType.Int);
            cmd.Parameters.Add("@oVM", SqlDbType.Int);
            cmd.Parameters.Add("@isMk", SqlDbType.Int);
            cmd.Parameters.Add("@oTyp", SqlDbType.Int);

            return cmd;
        }


        private SqlCommand addValue(SqlCommand cmd)
        {
            cmd.Parameters["@tID"].Value = TaskID;
            cmd.Parameters["@tBas"].Value = TaskBaseCode;
            cmd.Parameters["@tNam"].Value = TaskName;
            cmd.Parameters["@aLvl"].Value = AdmLevel;
            cmd.Parameters["@vNo"].Value = VersionNo;
            cmd.Parameters["@isDt"].Value = IssueDate;
            cmd.Parameters["@tPla"].Value = TaskPlace;
            cmd.Parameters["@pCod"].Value = PartnerCode;
            cmd.Parameters["@stDt"].Value = StartDate;
            cmd.Parameters["@enDt"].Value = EndDate;
            cmd.Parameters["@pNot"].Value = PayNote;
            cmd.Parameters["@tOff"].Value = TaskOffice;
            cmd.Parameters["@tLdr"].Value = TaskLeader;
            cmd.Parameters["@telN"].Value = TelNo;
            cmd.Parameters["@faxN"].Value = FaxNo;
            cmd.Parameters["@eMl"].Value = EMail;
            cmd.Parameters["@aPrc"].Value = AttProceed;
            cmd.Parameters["@aEst"].Value = AttEstimate;
            cmd.Parameters["@aDsn"].Value = AttDesign;
            cmd.Parameters["@aCon"].Value = AttContract;
            cmd.Parameters["@aOdr"].Value = AttOrder;
            cmd.Parameters["@aSta"].Value = AttStart;
            cmd.Parameters["@aOtr"].Value = AttOther;
            cmd.Parameters["@aOCo"].Value = AttOtherCont;
            cmd.Parameters["@oNot"].Value = OrderNote;
            cmd.Parameters["@cSpc"].Value = CommonSpec;
            cmd.Parameters["@eSpc"].Value = ExclusiveSpec;
            cmd.Parameters["@oSpc"].Value = OtherSpec;
            cmd.Parameters["@sCon"].Value = SpecCont;
            cmd.Parameters["@sCod"].Value = SalesMCode;
            cmd.Parameters["@appr"].Value = Approval;
            cmd.Parameters["@mOdr"].Value = MakeOrder;
            cmd.Parameters["@inNo"].Value = InputNumber;
            cmd.Parameters["@conf"].Value = ConfirmAdm;

            cmd.Parameters["@sCDt"].Value = SalesMInputDate;
            cmd.Parameters["@apDt"].Value = ApprovalDate;
            cmd.Parameters["@mODt"].Value = MakeOrderDate;
            cmd.Parameters["@iNDt"].Value = InputNoDate;
            cmd.Parameters["@cfDt"].Value = ConfirmDate;

            cmd.Parameters["@oBud"].Value = OrderBudget;
            cmd.Parameters["@oPln"].Value = OrderPlanning;
            cmd.Parameters["@cTyp"].Value = CostType;
            cmd.Parameters["@conS"].Value = ConstructionStat;
            cmd.Parameters["@oFrm"].Value = OrdersForm;
            cmd.Parameters["@cFrm"].Value = ClaimForm;
            cmd.Parameters["@oVM"].Value = OldVerMark;
            cmd.Parameters["@isMk"].Value = IssueMark;
            cmd.Parameters["@oTyp"].Value = OrdersType;

            return cmd;
        }


    }
}

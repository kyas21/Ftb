using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;
using Microsoft.VisualBasic;

namespace ClassLibrary
{
    public class MasterMaintOp : EstPlanOp
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        StringUtility util = new StringUtility();
        int idx;

        // 事務所マスタ（M_Office）
        private string officeIns = "INSERT INTO M_Office("
                                + "OfficeCode, OfficeName, MemberCode, MemberName, Title, PostCode, Address, TelNo, FaxNo, OrderSeqNo, OrderLastNo, PurchaseSeqNo, UpdateDate"
                                + ") VALUES (";

        private string[] ofcPArray = new string[] { "@oCod", "@oNam", "@mCod", "@mNam", "@ttl", "@pCod", "@addr", "@telN", "@faxN", "@oSeq", "@oLNo", "@pSeq", "@uDt" };

        private string officeUpd = "UPDATE M_Office SET "
                                + "OfficeName = @oNam, MemberCode = @mCod , MemberName = @mNam, Title = @ttl, PostCode = @pCod, Address = @addr, TelNo = @telN, FaxNo = @faxN, OrderSeqNo = @oSeq, OrderLastNo = @oLNo, PurchaseSeqNo = @pSeq, UpdateDate = @uDt "
                                + " WHERE OfficeCode = @oCod";

        private string officeExistence = "SELECT * FROM M_Office WHERE OfficeCode = @oCod";

        // 社員マスタ（M_Members）
        private string membersIns = "INSERT INTO M_Members("
                               + "MemberCode, Name, Phonetic, OfficeCode, Department, BirthDate, "
                               + "PostCode, Address, PostCode2, Address2, TelNo, CellularNo, CellularNo2, EMail, MobileEmail, "
                               + "BloodType, JoinDate, FinalEducation, GradDate, BasicPNo, HealthInsNo, EmploymentInsNo, "
                               + "GainQDate, BankName, BBranchName, AccountType, AccountNo, EContact, "
                               + "RadiationMedical, MedicalCheckup, FormWage, MemberType, AccessLevel, Enrollment, Note, UpdateDate"
                               + ") VALUES (";

        private string[] memPArray = new string[] {
                                "@mCod", "@name", "@phn", "@oCod", "@dCod", "@bDay",
                                "@pCod", "@addr", "@pCod2", "@addr2", "@telN", "@celN", "@celN2", "@eMl", "@mEMl",
                                "@bTyp", "@jDt", "@fEdu", "@gDt", "@bNo", "@hNo", "@eNo",
                                "@gaDt", "@bNam", "@bBNm", "@aTyp", "@aNo", "@eCon",
                                "@rMdc", "@mChk", "@frmW", "@mTyp", "@aLvl", "@erol", "@note", "@uDt" };

        private string membersUpd = "UPDATE M_Members SET "
                               + "Name = @name, Phonetic = @phn, OfficeCode = @oCod, Department = @dCod, BirthDate = @bDay, "
                               + "PostCode = @pCod, Address = @addr, PostCode2 = @pCod2, Address2 = @addr2, "
                               + "TelNo = @telN, CellularNo = @celN, CellularNo2 = @celN2, EMail = @eMl, MobileEmail = @mEMl, "
                               + "BloodType = @bTyp, JoinDate = @jDt, FinalEducation = @fEdu, GradDate = @gDt, "
                               + "BasicPNo = @bNo, HealthInsNo = @hNo, EmploymentInsNo = @eNo, GainQDate = @gaDt, "
                               + "BankName = @bNam, BBranchName = @bBNm, AccountType = @aTyp, AccountNo = @aNo, EContact = @eCon, "
                               + "RadiationMedical = @rMdc, MedicalCheckup = @mChk, FormWage = @frmW, MemberType = @mTyp, "
                               + "AccessLevel = @aLvl, Enrollment = @erol, Note = @note, UpdateDate = @uDt "
                               + "WHERE MemberCode = @mCod";

        private string allMembersDel = "DELETE FROM M_Members";

        private string membersExistence = "SELECT * FROM M_Members WHERE MemberCode = @mCod";

        // 取引先マスタ（M_Partners）
        private string partnersIns = "INSERT INTO M_Partners("
                               + "PartnerCode, PartnerName, PartnerPhonetic, CorporateForm, SignPosition, "
                               + "PostCode, Address, TelNo, FaxNo, Capital, Representative, Title, CellularNo, EMail, BankName, BBranchName, AccountType, AccountNo, "
                               + "ClosingDay, PayDay, PayType, PayLT, RelCusto, RelSubco, RelSuppl, RelOther, StartDate, AccountCode, UpdateDate, ChiefTrans"
                               + ") VALUES (";

        private string[] ptnPArray = new string[] { "@pCod", "@name", "@phn", "@cFrm", "@sPos", "@post", "@addr", "@telN", "@faxN", "@cptl", "@rPre", "@ttl", "@celN", "@email",
                                                    "@bNam", "@bBrn", "@aTyp", "@aNo", "@cDay", "@pDay", "@pTyp", "@pLT", "@rCust", "@rSubC", "@rSupl", "@rOther", "@sDt", "@aCod", "@uDt", "@cTrn" };

        private string partnersUpd = "UPDATE M_Partners SET "
                               + "PartnerName = @name, PartnerPhonetic = @phn, CorporateForm = @cFrm, SignPosition = @sPos, PostCode = @post, Address = @addr, TelNo = @telN, FaxNo = @faxN, "
                               + "Capital = @cptl, Representative = @rPre, Title = @ttl, CellularNo = @celN, EMail = @email, BankName = @bNam, BBranchName = @bBrn, AccountType = @aTyp, AccountNo = @aNo, ClosingDay = @cDay, "
                               + "PayDay = @pDay, PayType = @pTyp, PayLT = @pLT, RelCusto = @rCust, RelSubco = @rSubC, RelSuppl = @rSupl, RelOther = @rOther, StartDate = @sDt, AccountCode = @aCod, UpdateDate = @uDt, ChiefTrans = @cTrn"
                               + " WHERE PartnerCode = @pCod";

        private string allPartnersDel = "DELETE FROM M_Partners";

        private string partnersExistence = "SELECT * FROM M_Partners WHERE PartnerCode = @pCod";

        // 原価項目マスタ（M_Cost）
        private string costIns = "INSERT INTO M_Cost(CostCode, Item, ItemDetail, Unit, Cost, OfficeCode, MemberCode, UpdateDate) VALUES (";

        private string[] costPArray = new string[] { "@cCod", "@item", "@iDtl", "@unit", "@cost", "@oCod", "@mCod", "@uDt" };

        private string costUpd = "UPDATE M_Cost SET "
                                + "Item = @item, ItemDetail = @iDtl, Unit = @unit, Cost = @cost, UpdateDate = @uDt"
                                + " WHERE CostCode = @cCod AND OfficeCode = @oCod AND MemberCode = @mCod";

        private string costExistence = "SELECT * FROM M_Cost WHERE CostCode = @cCod AND OfficeCode = @oCod AND MemberCode = @mCod";

        private string allCostDel = "DELETE FROM M_Cost";

        // 共通マスタ（M_Common）
        private string commonIns = "INSERT INTO M_Common(Kind, ComData, ComSignage, ComSymbol, UsedNote, UpdateDate) VALUES (";

        private string[] comPArray = new string[] { "@kind", "@cDat", "@cSign", "@cSbl", "@uNot", "@uDt" };

        private string allCommonDel = "DELETE FROM M_Common";

        // 作業項目マスタ（M_WorkItems）
        private string wkItemsIns = "INSERT INTO M_WorkItems(ItemCode, UItem, Item, ItemDetail, Unit, StdCost, MemberCode, UpdateDate) VALUES (";

        private string[] wkItmPArray = new string[] { "@iCod", "@uItm", "@item", "@iDtl", "@unit", "@sCst", "@mCod", "@uDt" };

        private string wkItemsDel = "DELETE FROM M_WorkItems WHERE MemberCode = @mCod";

        // カレンダ（M_Calendar）
        private string calendarIns = "INSERT INTO M_Calendar(MDate, Dtype, UpdateDate) VALUES (";

        private string[] calPArray = new string[] { "@mDt", "@dTyp", "@uDt" };

        private string allCalendarDel = "DELETE FROM M_Calendar";

        // -------- 通常データ
        // 業務データ（D_Task）
        //private string taskIns = "INSERT INTO D_Task(TaskBaseCode, TaskName, PartnerCode, SalesMCode, VersionNo, OldVerMark) VALUES (";
        //private string[] taskPArray = new string[] { "@tBC", "@tNam", "@pCod", "@sCod", "@vNo", "@oVMk" };

        private string taskIns = "INSERT INTO D_Task(TaskBaseCode, TaskName, IssueDate, PartnerCode, StartDate, EndDate, SalesMCode, VersionNo, OldVerMark, IssueMark) VALUES (";
        private string[] taskPArray = new string[] { "@tBC", "@tNam", "@iDat", "@pCod", "@sDat", "@eDat", "@sCod", "@vNo", "@oVMk", "@isMk" };

        private string taskUpdtDate = "UPDATE D_Task SET IssueDate = @iDat, StartDate = @sDat, EndDate = @eDat WHERE TaskBaseCode = @tBC";


        private string taskExistence = "SELECT * FROM D_Task WHERE TaskBaseCode = @tBC";

        // 業務個別データ（D_TaskInd）
        //private string taskIndIns = "INSERT INTO D_TaskInd (TaskCode, Contract, TaskID, LeaderMCode, OfficeCode, TaskName, VersionNo, OldVerMark) "
        //                        + "SELECT @tCod, @cont, @tID, @lCod, @oCod, @tNam, @vNo, @oVMk "
        //                        + "WHERE NOT EXISTS(SELECT* FROM D_TaskInd WHERE TaskCode = @tCod AND OfficeCode = @oCod)";
        private string taskIndIns = "INSERT INTO D_TaskInd (TaskCode, Contract, TaskID, LeaderMCode, OfficeCode, TaskName, VersionNo, OldVerMark, Department, IssueMark) "
                                + "SELECT @tCod, @cont, @tID, @lCod, @oCod, @tNam, @vNo, @oVMk, @dept, @isMk "
                                + "WHERE NOT EXISTS(SELECT* FROM D_TaskInd WHERE TaskCode = @tCod AND OfficeCode = @oCod)";
        //private string[] taskIndPArray = new string[] { "@tCod", "@cont", "@tID", "@lCod", "@oCod", "@tNam", "@vNo", "@oVMk" };
        private string[] taskIndPArray = new string[] { "@tCod", "@cont", "@tID", "@lCod", "@oCod", "@tNam", "@vNo", "@oVMk", "@dept", "@isMk" };
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        //----------------------------------
        // 事務所マスタ（M_Office）
        //----------------------------------
        public int[] MaintOfficeByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            string[] proArray = new string[] { "INS", "UPD" };
            int p;
            int[] procCount = new int[] { 0, 0 };
            string sqlStr = editSql(officeIns, ofcPArray);

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= rowLimit; i++)
                    {

                        cmd = new SqlCommand(officeExistence, conn);
                        cmd.Parameters.Add("@oCod", SqlDbType.Char);
                        cmd.Parameters["@oCod"].Value = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        SqlDataReader dr = TryExReader(conn, cmd);
                        p = 0;                  // Default  0:新規作成(Insert)
                        if (dr.HasRows) p = 1;  // Data存在 1:更新(Update)
                        dr.Close();

                        // 20171010 Asakawa 
                        // 以下、空行を読み込む不具合対策のため追加
                        //   書き出したファイルをそのまま読み込むだけでも１行多く取り込まれる。
                        //   （oWSheet.LastRowUsed().RowNumber() が１多い）
                        //   ただし、同じことを２回以上行っても、２回目以降は正常（しかしデータは異常のまま）。
                        //   この理由が不明であるため、事業所名が記載されていない行は空行とみなすこととした。
                        if (oWSheet.Cell(i, 2).Value.Equals(""))
                            continue;

                        // ここまで

                        if (p == 0)
                        {
                            cmd = new SqlCommand(sqlStr, conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(officeUpd, conn);
                        }

                        cmd = addParaOffice(cmd);
                        cmd = storeOfficeParaValue(cmd, oWSheet, i, proArray[p]);
                        if (TryExecute(conn, cmd) < 0)
                        {
                            procCount[0] = -1;
                            return procCount;
                        }
                        procCount[p]++;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    procCount[0] = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }
            return procCount;
        }


        private SqlCommand storeOfficeParaValue(SqlCommand cmd, ClosedXML.Excel.IXLWorksheet oWSheet, int line, string type)
        {
            int WorkInt = 0;

            for (int j = 0; j < ofcPArray.Length; j++)
            {
                switch (j)
                {
                    case 9:
                    case 10:
                    case 11:
                        if (Convert.ToString(oWSheet.Cell(line, j + 1).Value) == "")
                        {
                            cmd.Parameters[(ofcPArray[j])].Value = 0;
                        }
                        else
                        {
                            if (int.TryParse(Convert.ToString(oWSheet.Cell(line, j + 1).Value), out  WorkInt) == true )
                            {
                                cmd.Parameters[(ofcPArray[j])].Value = WorkInt;
                            }
                            else
                            {
                                cmd.Parameters[(ofcPArray[j])].Value = 0;
                            }
                        }
                        break;

                    case 12:
                        cmd.Parameters[(ofcPArray[j])].Value = DateTime.Today.StripTime();
                        break;

                    default:
                        cmd.Parameters[(ofcPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j + 1).Value);
                        break;
                }
            }
            return cmd;
        }


        //----------------------------------
        // 社員マスタ（M_Members）
        //----------------------------------
        public int[] MaintMembersByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            int p;
            int[] procCount = new int[] { 0, 0 };
            string sqlStr = editSql(membersIns, memPArray);
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= rowLimit; i++)
                    {
                        cmd = new SqlCommand(membersExistence, conn);
                        cmd.Parameters.Add("@mCod", SqlDbType.Char);
                        cmd.Parameters["@mCod"].Value = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        SqlDataReader dr = TryExReader(conn, cmd);
                        p = 0;                  // Default  0:新規作成(Insert)
                        if (dr.HasRows) p = 1;  // Data存在 1:更新(Update)
                        dr.Close();

                        if (p == 0)
                        {
                            cmd = new SqlCommand(sqlStr, conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(membersUpd, conn);
                        }
                        cmd = addParaMembers(cmd);
                        cmd = storeMembersParaValue(cmd, oWSheet, i);
                        idx = i;
                        if (TryExecute(conn, cmd) < 0)
                        {
                            procCount[0] = -1;
                            return procCount;
                        }
                        procCount[p]++;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message + "Line:" + Convert.ToString(idx));
                    conn.Close();
                    procCount[0] = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }
            return procCount;
        }


        public bool AllMMembers_Delete()
        {
            return allRecord_Delete(allMembersDel);
        }


        // varchar 0,10
        // nvarchar 1,2,7,9,13,14,17,23,24,27,34
        // char 3,4,6,8,11,12,15,19,20,21,26
        // date 5,16,18,22,28,29,35
        // int 25,30,31,32,33
        private SqlCommand storeMembersParaValue(SqlCommand cmd, ClosedXML.Excel.IXLWorksheet oWSheet, int line)
        {
            char tempOc;
            char tempDp;
            DateTime WorkDate = DateTime.MinValue;
            int WorkInt = 0;
            for (int j = 0; j < memPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 1:
                    case 2:
                        cmd.Parameters[(memPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j + 1).Value);
                        break;
                    case 3:
                        tempOc = Convert.ToString(oWSheet.Cell(line, j + 1).Value)[0];
                        cmd.Parameters[(memPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j + 1).Value)[0];
                        break;
                    case 4:
                        tempDp = Convert.ToString(oWSheet.Cell(line, j).Value)[1];
                        cmd.Parameters[(memPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j).Value)[1];
                        break;
                    case 5:
                    case 16:
                    case 18:
                    case 22:
                    case 28:
                    case 29:
                        if (Convert.ToString(oWSheet.Cell(line, j).Value) == "")
                        {
                            cmd.Parameters[(memPArray[j])].Value = DateTime.MinValue.StripTime();
                        }
                        else
                        {
                            if (DateTime.TryParse(Convert.ToString(oWSheet.Cell(line, j).Value), out WorkDate) == true )
                            {
                                cmd.Parameters[(memPArray[j])].Value = WorkDate;
                            }
                            else
                            {
                                cmd.Parameters[(memPArray[j])].Value = DateTime.MinValue.StripTime();
                            }
                        }
                        break;

                    case 25:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                        if (Convert.ToString(oWSheet.Cell(line, j).Value) == "")
                        {
                            cmd.Parameters[(memPArray[j])].Value = 0;
                        }
                        else
                        {
                            if (int.TryParse(Convert.ToString(oWSheet.Cell(line, j).Value), out WorkInt) == true )
                            {
                                cmd.Parameters[(memPArray[j])].Value = WorkInt;
                            }
                            else
                            {
                                cmd.Parameters[(memPArray[j])].Value = 0;
                            }
                        }
                        break;

                    case 35:            // 更新日
                        cmd.Parameters[(memPArray[j])].Value = System.DateTime.Today.StripTime();
                        break;

                    default:
                        cmd.Parameters[(memPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j).Value);
                        break;
                }
            }
            return cmd;
        }


        //----------------------------------
        // 取引先マスタ（M_Partners）
        //----------------------------------
        public int[] MaintPartnersByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            int p;
            int[] procCount = new int[] { 0, 0 };
            string debugstr;
            string sqlStr = editSql(partnersIns, ptnPArray);
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= rowLimit; i++)
                    {

                        cmd = new SqlCommand(partnersExistence, conn);
                        cmd.Parameters.Add("@pCod", SqlDbType.Char);
                        cmd.Parameters["@pCod"].Value = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        debugstr = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        SqlDataReader dr = TryExReader(conn, cmd);
                        p = 0;                  // Default  0:新規作成(Insert)
                        if (dr.HasRows) p = 1;  // Data存在 1:更新(Update)
                        dr.Close();

                        if (p == 0)
                        {
                            cmd = new SqlCommand(sqlStr, conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(partnersUpd, conn);
                        }
                        cmd = addParaPartners(cmd);
                        cmd = storePartnersParaValue(cmd, oWSheet, i);
                        if (TryExecute(conn, cmd) < 0)
                        {
                            procCount[0] = -1;
                            return procCount;
                        }
                        procCount[p]++;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    procCount[0] = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }
            return procCount;
        }


        public bool AllMPartners_Delete()
        {
            return allRecord_Delete(allPartnersDel);
        }


        private SqlCommand storePartnersParaValue(SqlCommand cmd, ClosedXML.Excel.IXLWorksheet oWSheet, int line)
        {
            decimal WorkDecimal = 0;
            DateTime WorkDate = DateTime.MinValue;
            int WorkInt = 0;
            //       0       1        2        3        4       5        6        7        8        9        10      11       12       13
            // { "@pCod", "@name", "@phn", "@cFrm", "@sPos", "@post", "@addr", "@telN", "@faxN", "@cptl", "@rPre", "@ttl", "@celN", "@email",
            //      14       15       16      17       18       19       20       21      22       23          24        25       26       27       28      29
            //   "@bNam", "@bBrn", "@aTyp", "@aNo", "@cDay", "@pDay", "@pTyp", "@pLT", "@rCust", "@rSubC", "@rSupl", "@rOther", "@sDt", "@aCod", "@uDt", "@cTrn" };
            for (int j = 0; j < ptnPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 17:
                    case 27:
                        cmd.Parameters[(ptnPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j + 1).Value);
                        break;

                    case 9:
                        if (Convert.ToString(oWSheet.Cell(line, j + 1).Value) == "")
                        {
                            cmd.Parameters[(ptnPArray[j])].Value = 0;
                        }
                        else
                        {
                            if (decimal.TryParse(Convert.ToString(oWSheet.Cell(line, j + 1).Value), out WorkDecimal) == true )
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = WorkDecimal;
                            }
                            else
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = 0;
                            }
                        }
                        break;

                    case 26:
                        if (Convert.ToString(oWSheet.Cell(line, j + 1).Value) == "")
                        {
                            cmd.Parameters[(ptnPArray[j])].Value = DateTime.MinValue.StripTime();
                        }
                        else
                        {
                            if (DateTime.TryParse(Convert.ToString(oWSheet.Cell(line, j + 1).Value), out WorkDate) == true )
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = WorkDate;
                            }
                            else
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = DateTime.MinValue.StripTime();
                            }
                        }
                        break;

                    case 28:
                        cmd.Parameters[(ptnPArray[j])].Value = System.DateTime.Today.StripTime();
                        break;

                    case 29:
                        cmd.Parameters[(ptnPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j).Value);
                        break;

                    default:
                        if (Convert.ToString(oWSheet.Cell(line, j + 1).Value) == "")
                        {
                            cmd.Parameters[(ptnPArray[j])].Value = 0;
                        }
                        else
                        {
                            if (int.TryParse(Convert.ToString(oWSheet.Cell(line, j + 1).Value), out WorkInt) == true )
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = WorkInt;
                            }
                            else
                            {
                                cmd.Parameters[(ptnPArray[j])].Value = 0;
                            }
                        }
                        break;
                }
            }
            return cmd;
        }


        //----------------------------------
        // 原価項目マスタ（M_Cost）
        //----------------------------------
        //costPArray = new string[] { "@cCod", "@item", "@iDtl", "@unit", "@cost", "@oCod", "@mCod", "@uDt" };
        //                               0        1        2        3        4        5        6        7
        public int[] MaintCostByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            int p;
            int[] procCount = new int[] { 0, 0 };
            string debugstr;
            string sqlStr = editSql(costIns, costPArray);
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= rowLimit; i++)
                    {
                        cmd = new SqlCommand(costExistence, conn);
                        cmd.Parameters.Add("@cCod", SqlDbType.VarChar);
                        cmd.Parameters["@cCod"].Value = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        cmd.Parameters.Add("@oCod", SqlDbType.Char);
                        cmd.Parameters["@oCod"].Value = Convert.ToString(oWSheet.Cell(i, 6).Value);
                        cmd.Parameters.Add("@mCod", SqlDbType.VarChar);
                        cmd.Parameters["@mCod"].Value = Convert.ToString(oWSheet.Cell(i, 7).Value);
                        debugstr = Convert.ToString(oWSheet.Cell(i, 1).Value);
                        SqlDataReader dr = TryExReader(conn, cmd);
                        p = 0;                  // Default  0:新規作成(Insert)
                        if (dr.HasRows) p = 1;  // Data存在 1:更新(Update)
                        dr.Close();

                        if (p == 0)
                        {
                            cmd = new SqlCommand(sqlStr, conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(costUpd, conn);
                        }
                        cmd = addParaCost(cmd);
                        cmd = storeCostParaValue(cmd, oWSheet, i);
                        if (TryExecute(conn, cmd) < 0)
                        {
                            procCount[0] = -1;
                            return procCount;
                        }
                        procCount[p]++;
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    procCount[0] = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }
            return procCount;
        }


        //costPArray = new string[] { "@cCod", "@item", "@iDtl", "@unit", "@cost", "@oCod", "@mCod", "@uDt" };
        //                               0        1        2        3        4        5        6        7
        private SqlCommand storeCostParaValue(SqlCommand cmd, IXLWorksheet oWSheet, int line)
        {
            decimal WorkDecimal = 0;

            for (int j = 0; j < costPArray.Length; j++)
            {
                switch (j)
                {
                    case 4:
                        if (Convert.ToString(oWSheet.Cell(line, j + 1).Value) == "")
                        {
                            cmd.Parameters[(costPArray[j])].Value = 0;
                        }
                        else
                        {
                            if (decimal.TryParse(Convert.ToString(oWSheet.Cell(line, j + 1).Value), out WorkDecimal) == true )
                            {
                                cmd.Parameters[(costPArray[j])].Value = WorkDecimal;
                            }
                            else
                            {
                                cmd.Parameters[(costPArray[j])].Value = 0;
                            }
                        }
                        break;

                    case 7:
                        cmd.Parameters[(costPArray[j])].Value = System.DateTime.Today.StripTime();
                        break;

                    default:
                        cmd.Parameters[(costPArray[j])].Value = Convert.ToString(oWSheet.Cell(line, j + 1).Value).TrimEnd();
                        break;
                }
            }
            return cmd;
        }


        public bool AllMCost_Delete()
        {
            return allRecord_Delete(allCostDel);
        }


        public bool MaintCostByUIData(DataGridView TargetDgv, string DelID, string OfficeCode)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = null;
                    string SetSQL = "";

                    // INSERT OR UPDATE
                    for (int i = 0; i < TargetDgv.Rows.Count; i++)
                    {
                        SetSQL = "";
                        switch (Convert.ToString(TargetDgv.Rows[i].Cells[9].Value))
                        {
                            case "I":
                                // INSERT
                                SetSQL += "INSERT INTO M_Cost(";
                                SetSQL += "CostCode, Item, ItemDetail, Unit, Cost, ";
                                SetSQL += "OfficeCode, MemberCode, UpdateDate) ";
                                SetSQL += "VALUES('" + Convert.ToString(TargetDgv.Rows[i].Cells[2].Value) + "', ";
                                SetSQL += "'" + Convert.ToString(TargetDgv.Rows[i].Cells[3].Value) + "', ";
                                SetSQL += "'" + Convert.ToString(TargetDgv.Rows[i].Cells[4].Value) + "', ";
                                SetSQL += "'" + Convert.ToString(TargetDgv.Rows[i].Cells[5].Value) + "', ";
                                SetSQL += Convert.ToDecimal(TargetDgv.Rows[i].Cells[6].Value) + ", ";
                                SetSQL += "'" + OfficeCode + "', ";
                                SetSQL += "'" + Convert.ToString(TargetDgv.Rows[i].Cells[7].Value) + "', ";
                                SetSQL += "'" + System.DateTime.Today.StripTime() + "')";
                                cmd = new SqlCommand(SetSQL, conn);
                                break;

                            case "U":
                                // UPDATE
                                SetSQL += "UPDATE M_Cost ";
                                SetSQL += "SET Item = '" + Convert.ToString(TargetDgv.Rows[i].Cells[3].Value) + "', ";
                                SetSQL += "ItemDetail = '" + Convert.ToString(TargetDgv.Rows[i].Cells[4].Value) + "', ";
                                SetSQL += "Unit = '" + Convert.ToString(TargetDgv.Rows[i].Cells[5].Value) + "', ";
                                SetSQL += "Cost = " + Convert.ToDecimal(TargetDgv.Rows[i].Cells[6].Value) + ", ";
                                SetSQL += "MemberCode = '" + Convert.ToString(TargetDgv.Rows[i].Cells[7].Value) + "', ";
                                SetSQL += "UpdateDate = '" + System.DateTime.Today.StripTime() + "' ";
                                SetSQL += "WHERE CostID = " + Convert.ToInt32(TargetDgv.Rows[i].Cells[0].Value);
                                cmd = new SqlCommand(SetSQL, conn);
                                break;
                            default:
                                cmd = null;
                                break;
                        }

                        if (cmd != null)
                        {
                            if (TryExecute(conn, cmd) < 0) return false;
                        }
                    }

                    string[] DelIDList = DelID.Split(',');

                    // DELETE
                    for (int i = 0; i < DelIDList.Length; i++)
                    {
                        SetSQL = "";
                        if (DelIDList[i] != "")
                        {
                            SetSQL += "DELETE FROM M_Cost ";
                            SetSQL += "WHERE CostID = " + DelIDList[i];
                            // DELETE
                            cmd = new SqlCommand(SetSQL, conn);
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

        //----------------------------------
        // 共通マスタ（M_Common）
        //----------------------------------
        public int MaintCommonByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            int procCount = 0;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    // Wakamatsu 20170227
                    // テーブル全削除
                    if (!allRecord_Delete(allCommonDel, tran)) return -2;
                    // Wakamatsu 20170227
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(editSql(commonIns, comPArray), conn);
                    cmd = addParaCommon(cmd);
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i <= rowLimit; i++)
                    {
                        for (int j = 0; j < comPArray.Length; j++)
                        {
                            switch (j)
                            {
                                case 5:
                                    cmd.Parameters[(comPArray[j])].Value = DateTime.Today.StripTime();
                                    break;

                                default:
                                    cmd.Parameters[(comPArray[j])].Value = Convert.ToString(oWSheet.Cell(i, j + 1).Value);
                                    break;
                            }
                        }
                        if (TryExecute(conn, cmd) < 0) return -1;
                        procCount++;
                    }
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
            return procCount;
        }


        public bool AllMCommon_Delete()
        {
            return allRecord_Delete(allCommonDel);
        }



        //----------------------------------
        // 作業項目マスタ（M_WorkItems）
        //----------------------------------
        public bool MWorkItems_Delete(string args)
        {
            return pointRecord_Delete(wkItemsDel, "@mCod", "Char", args);
        }


        public int MaintWorkItemsByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet, string member)
        {
            int procCount = 0;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(editSql(wkItemsIns, wkItmPArray), conn);
                    cmd = addParaWorkItems(cmd);
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    for (int i = 2; i < rowLimit + 1; i++)
                    {
                        if (DHandling.IsNumeric(Convert.ToString(oWSheet.Cell(i, 1).Value)))
                        {


                            for (int j = 0; j < wkItmPArray.Length; j++)
                            {
                                switch (j)
                                {
                                    case 5:
                                        if (Convert.ToString(oWSheet.Cell(i, j + 1).Value) == "")
                                        {
                                            cmd.Parameters[(wkItmPArray[j])].Value = 0;
                                        }
                                        else
                                        {
                                            cmd.Parameters[(wkItmPArray[j])].Value = Convert.ToDecimal(oWSheet.Cell(i, j + 1).Value);
                                        }
                                        break;
                                    case 6:
                                        cmd.Parameters[(wkItmPArray[j])].Value = member;
                                        break;
                                    case 7:
                                        cmd.Parameters[(wkItmPArray[j])].Value = DateTime.Today.StripTime();
                                        break;
                                    default:
                                        cmd.Parameters[(wkItmPArray[j])].Value = Convert.ToString(oWSheet.Cell(i, j + 1).Value);
                                        break;
                                }
                            }
                            if (TryExecute(conn, cmd) < 0) return -1;
                            procCount++;
                        }

                    }
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

            return procCount;
        }


        //----------------------------------
        // テスト用
        //-----------------------------------
        public int MaintWorkItemsByCSVData(string fileName, string member)
        {
            int procCount = 0;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(editSql(wkItemsIns, wkItmPArray), conn);
                    cmd = addParaWorkItems(cmd);

                    using (var streamReader = new StreamReader(fileName, System.Text.Encoding.Default))
                    {

                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            var valArray = line.Split(',');

                            if (DHandling.IsNumeric(Convert.ToString(valArray[0])))
                            {
                                for (int i = 0; i < valArray.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 5:
                                            if (Convert.ToString(valArray[i]) == "")
                                            {
                                                cmd.Parameters[(wkItmPArray[i])].Value = 0;
                                            }
                                            else
                                            {
                                                cmd.Parameters[(wkItmPArray[i])].Value = Convert.ToDecimal(valArray[i]);
                                            }
                                            break;
                                        case 6:
                                            cmd.Parameters[(wkItmPArray[i])].Value = member;
                                            break;
                                        case 7:
                                            cmd.Parameters[(wkItmPArray[i])].Value = DateTime.Today.StripTime();
                                            break;
                                        default:
                                            cmd.Parameters[(wkItmPArray[i])].Value = Convert.ToString(valArray[i]);
                                            break;
                                    }
                                }

                                if (TryExecute(conn, cmd) < 0) return -1;
                                procCount++;
                            }

                        }

                    }
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

            return procCount;
        }


        //----------------------------------
        // カレンダ（M_Calendar）
        //----------------------------------
        public int MaintCalendarByExcelData(ClosedXML.Excel.IXLWorksheet oWSheet)
        {
            int procCount = 0;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    // Wakamatsu 20170227
                    // テーブル全削除
                    if (!allRecord_Delete(allCalendarDel, tran)) return -2;
                    // Wakamatsu 20170227
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(editSql(calendarIns, calPArray), conn);
                    cmd = addParaCalendar(cmd);
                    int rowLimit = oWSheet.LastRowUsed().RowNumber();
                    // Wakamatsu 20170227
                    DateTime WorkDate = DateTime.MinValue;
                    int WorkInt = 0;
                    // Wakamatsu 20170227
                    for (int i = 2; i < rowLimit + 1; i++)
                    {
                        // Wakamatsu 20170227
                        //cmd.Parameters[(calPArray[0])].Value = Convert.ToDateTime(oWSheet.Cell(i, 1).Value);
                        if (DateTime.TryParse(Convert.ToString(oWSheet.Cell(i, 1).Value), out WorkDate) == true)
                            cmd.Parameters[(calPArray[0])].Value = WorkDate;
                        else
                            cmd.Parameters[(calPArray[0])].Value = DateTime.MinValue;
                        // Wakamatsu 20170227

                        if (Convert.ToString(oWSheet.Cell(i, 2).Value) == "")
                        {
                            cmd.Parameters[(calPArray[1])].Value = 9;
                        }
                        else
                        {
                            // Wakamatsu 20170227
                            //cmd.Parameters[(calPArray[1])].Value = Convert.ToInt32(oWSheet.Cell(i, 2).Value);
                            if (int.TryParse(Convert.ToString(oWSheet.Cell(i, 2).Value), out WorkInt) == true)
                                cmd.Parameters[(calPArray[1])].Value = WorkInt;
                            else
                                cmd.Parameters[(calPArray[1])].Value = 9;
                            // Wakamatsu 20170227
                        }

                        cmd.Parameters[(calPArray[2])].Value = DateTime.Today.StripTime();

                        if (TryExecute(conn, cmd) < 0) return -1;
                        procCount++;
                    }
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
            return procCount;
        }


        public bool MCalendar_Delete()
        {
            return allRecord_Delete(allCalendarDel);
        }


        //--------------------------------------------------------//
        //      商魂得意先マスタからTask関連データを作成する
        //--------------------------------------------------------//
        public int[] MaintTaskDataByCSVData(string fileName)
        {
            int[] procCount = { 0, 0 };
            int holdTaskID;
            //string narrowStr;

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(taskExistence, conn);
                    cmd.Parameters.Add("@tBC", SqlDbType.Char);

                    SqlCommand cmd1 = new SqlCommand(editSql(taskIns, taskPArray) + ";SELECT CAST(SCOPE_IDENTITY() AS int)", conn);
                    cmd1 = addParaTask(cmd1);

                    SqlCommand cmd2 = new SqlCommand(taskIndIns, conn);
                    cmd2 = addParaTaskInd(cmd2);

                    string editTaskName = "";

                    using (var streamReader = new StreamReader(fileName, System.Text.Encoding.Default))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            var valArray = line.Split(',');
                            editTaskName = "";

                            cmd.Parameters["@tBC"].Value = util.SubstringByte(Convert.ToString(valArray[0]), 1, 6);
                            SqlDataReader dr = TryExReader(conn, cmd);
                            holdTaskID = 0;
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    holdTaskID = Convert.ToInt32(dr["TaskID"]);
                                }
                            }
                            dr.Close();

                            if (holdTaskID == 0)
                            {
                                for (int i = 0; i < valArray.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            cmd1.Parameters[(taskPArray[0])].Value = util.SubstringByte(Convert.ToString(valArray[i]), 1, 6);
                                            break;
                                        case 1:
                                            editTaskName = Strings.StrConv(Convert.ToString(valArray[i]), VbStrConv.Wide, 0x0411);
                                            break;
                                        case 2:
                                            cmd1.Parameters[(taskPArray[3])].Value = util.SubstringByte(Strings.StrConv(Convert.ToString(valArray[i]), VbStrConv.Narrow, 0x0411), 0, 3);
                                            break;
                                        case 3:
                                            editTaskName += Strings.StrConv(Convert.ToString(valArray[i]), VbStrConv.Wide, 0x0411);
                                            cmd1.Parameters[(taskPArray[1])].Value = editTaskName.TrimEnd();
                                            break;
                                        case 9:
                                            if ((Convert.ToString(valArray[9])).Trim() != "")
                                                cmd1.Parameters[(taskPArray[2])].Value = Convert.ToString(valArray[9]);
                                            break;
                                        case 10:
                                            if ((Convert.ToString(valArray[10])).Trim() != "")
                                                cmd1.Parameters[(taskPArray[4])].Value = Convert.ToString(valArray[10]);
                                            break;
                                        case 11:
                                            if ((Convert.ToString(valArray[11])).Trim() != "")
                                                cmd1.Parameters[(taskPArray[5])].Value = Convert.ToString(valArray[11]);
                                            break;
                                        case 14:
                                            cmd1.Parameters[(taskPArray[6])].Value = "0" + Convert.ToString(valArray[14]);
                                            break;
                                        default:
                                            break;
                                    }
                                    cmd1.Parameters[(taskPArray[7])].Value = 0;
                                    cmd1.Parameters[(taskPArray[8])].Value = 0;
                                    cmd1.Parameters[(taskPArray[9])].Value = 0;
                                }

                                //if (TryExecute(conn, cmd) < 0)
                                if ((holdTaskID = TryExScalar(conn, cmd1)) < 0)
                                {
                                    procCount[0] = -1;
                                    return procCount;
                                }
                                procCount[0]++;
                            }
                            else
                            {
                                editTaskName = Strings.StrConv(Convert.ToString(valArray[1]), VbStrConv.Wide, 0x0411);
                                editTaskName += Strings.StrConv(Convert.ToString(valArray[3]), VbStrConv.Wide, 0x0411);
                            }

                            cmd2.Parameters[(taskIndPArray[0])].Value = Convert.ToString(valArray[0]);
                            cmd2.Parameters[(taskIndPArray[1])].Value = ((Convert.ToString(valArray[8])).Trim() == "") ? 0M : Convert.ToDecimal(valArray[8]);
                            cmd2.Parameters[(taskIndPArray[2])].Value = holdTaskID;
                            cmd2.Parameters[(taskIndPArray[3])].Value = "0" + Convert.ToString(valArray[18]);
                            cmd2.Parameters[(taskIndPArray[4])].Value = Convert.ToString(valArray[31]);
                            cmd2.Parameters[(taskIndPArray[5])].Value = editTaskName.TrimEnd();
                            cmd2.Parameters[(taskIndPArray[6])].Value = 0;
                            cmd2.Parameters[(taskIndPArray[7])].Value = 0;
                            cmd2.Parameters[(taskIndPArray[9])].Value = 0;
                            switch (Convert.ToInt32(valArray[13]))
                            {
                                case 1:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "2";
                                    break;
                                case 2:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "1";
                                    break;
                                case 4:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "7";
                                    break;
                                case 7:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "0";
                                    break;
                                case 8:
                                case 12:
                                case 17:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "8";
                                    break;
                                case 11:
                                case 14:
                                case 18:
                                    cmd2.Parameters[(taskIndPArray[8])].Value = "9";
                                    break;
                                default:
                                    break;
                            }

                            if (TryExecute(conn, cmd2) < 0)
                            {
                                procCount[1] = -1;
                                return procCount;
                            }
                            procCount[1]++;
                        }

                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    procCount[0] = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }

            addPartnersData(fileName);

            return procCount;
        }



        public int MaintTaskDataDateByCSVData(string fileName)
        {
            int procCount = 0;

            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(taskUpdtDate, conn);
                    cmd.Parameters.Add("@tBC", SqlDbType.Char);
                    cmd.Parameters.Add("@iDat", SqlDbType.Date);
                    cmd.Parameters.Add("@sDat", SqlDbType.Date);
                    cmd.Parameters.Add("@eDat", SqlDbType.Date);

                    using (var streamReader = new StreamReader(fileName, System.Text.Encoding.Default))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            var valArray = line.Split(',');

                            cmd.Parameters["@tBC"].Value = util.SubstringByte(Convert.ToString(valArray[0]), 1, 6);

                            if ((Convert.ToString(valArray[9])).Trim() != "")
                                cmd.Parameters["@iDat"].Value = Convert.ToDateTime(valArray[9]);
                            if ((Convert.ToString(valArray[10])).Trim() != "")
                                cmd.Parameters["@sDat"].Value = Convert.ToDateTime(valArray[10]);
                            if ((Convert.ToString(valArray[11])).Trim() != "")
                                cmd.Parameters["@eDat"].Value = Convert.ToDateTime(valArray[11]);

                            if (TryExecute(conn, cmd) < 0)
                            {
                                procCount = -1;
                                return procCount;
                            }
                            procCount++;
                        }
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                    procCount = -1;
                    return procCount;
                }
                conn.Close();
                tran.Complete();
            }
            return procCount;
        }


        private void addPartnersData(string fileName)
        {
            string partnersCode;
            string partnersName;
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(partnersExistence, conn);
                    cmd.Parameters.Add("@pCod", SqlDbType.VarChar);

                    SqlCommand cmd1 = new SqlCommand(editSql(partnersIns, ptnPArray), conn);
                    cmd1 = addParaPartners(cmd1);

                    using (var streamReader = new StreamReader(fileName, System.Text.Encoding.Default))
                    {
                        int i = 0;
                        while (!streamReader.EndOfStream)
                        {
                            var line = streamReader.ReadLine();
                            var valArray = line.Split(',');

                            partnersCode = util.SubstringByte(Strings.StrConv(Convert.ToString(valArray[2]), VbStrConv.Narrow, 0x0411), 0, 3);
                            if (partnersCode == "") continue;
                            //cmd.Parameters["@pCod"].Value = util.SubstringByte(Strings.StrConv(Convert.ToString(valArray[3]), VbStrConv.Narrow, 0x0411), 0, 3);
                            cmd.Parameters["@pCod"].Value = partnersCode;
                            SqlDataReader dr = TryExReader(conn, cmd);

                            if (dr.HasRows)
                            {
                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                                cmd1.Parameters["@pCod"].Value = partnersCode;
                                //cmd1.Parameters["@pCod"].Value = util.SubstringByte(Strings.StrConv(Convert.ToString(valArray[3]), VbStrConv.Narrow, 0x0411), 0, 3);
                                cmd1.Parameters["@name"].Value = Convert.ToString(valArray[2]);
                                cmd1.Parameters["@phn"].Value = (DHandling.NumberOfCharacters(Convert.ToString(valArray[2]), 3)).TrimEnd();
                                partnersName = (DHandling.NumberOfCharacters(Convert.ToString(valArray[2]), 3)).TrimEnd();
                                cmd1.Parameters["@cFrm"].Value = 1;
                                cmd1.Parameters["@sPos"].Value = 0;
                                cmd1.Parameters["@post"].Value = "";
                                cmd1.Parameters["@addr"].Value = "";
                                cmd1.Parameters["@telN"].Value = "";
                                cmd1.Parameters["@faxN"].Value = "";
                                cmd1.Parameters["@cptl"].Value = 0M;
                                cmd1.Parameters["@rpre"].Value = "";
                                cmd1.Parameters["@ttl"].Value = "";
                                cmd1.Parameters["@celN"].Value = "";
                                cmd1.Parameters["@email"].Value = "";
                                cmd1.Parameters["@bNam"].Value = "";
                                cmd1.Parameters["@bBrn"].Value = "";
                                cmd1.Parameters["@aTyp"].Value = 1;
                                cmd1.Parameters["@aNo"].Value = "";
                                cmd1.Parameters["@cDay"].Value = 0;
                                cmd1.Parameters["@pDay"].Value = 0;
                                cmd1.Parameters["@pTyp"].Value = 0;
                                cmd1.Parameters["@pLT"].Value = 0;
                                cmd1.Parameters["@rCust"].Value = 1;
                                cmd1.Parameters["@rSubC"].Value = 0;
                                cmd1.Parameters["@rSupl"].Value = 0;
                                cmd1.Parameters["@rOther"].Value = 0;
                                cmd1.Parameters["@sDt"].Value = DateTime.MinValue.StripTime();
                                cmd1.Parameters["@aCod"].Value = "";
                                cmd1.Parameters["@uDt"].Value = DateTime.Today.StripTime();
                                cmd1.Parameters["@cTrn"].Value = "";

                                //if (TryExecute(conn, cmd1, partnersCode) < 0) return;
                                if (TryExecute(conn, cmd1) < 0) return;
                            }
                            i++;

                        }
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("SQLエラー errorno " + Convert.ToString(sqle.Number) + " " + sqle.Message);
                    conn.Close();
                }
                conn.Close();
                tran.Complete();
            }

        }


        //--------------------------------------------------------//
        //      SubRoutine
        //--------------------------------------------------------//
        // 指定レコードの削除
        private bool pointRecord_Delete(string delSql, string scalar, string sType, string args)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(delSql, conn))
                    {
                        switch (sType)
                        {
                            case "Int":
                                cmd.Parameters.Add(scalar, SqlDbType.Int);
                                break;
                            case "Date":
                                cmd.Parameters.Add(scalar, SqlDbType.Date);
                                break;
                            case "NVarChar":
                                cmd.Parameters.Add(scalar, SqlDbType.NVarChar);
                                break;
                            case "NChar":
                                cmd.Parameters.Add(scalar, SqlDbType.NChar);
                                break;
                            case "VarChar":
                                cmd.Parameters.Add(scalar, SqlDbType.VarChar);
                                break;
                            default:
                                cmd.Parameters.Add(scalar, SqlDbType.Char);
                                break;

                        }
                        cmd.Parameters[scalar].Value = args;
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


        // 全レコードの削除
        private bool allRecord_Delete(string delSql)
        {
            using (TransactionScope tran = new TransactionScope())
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(delSql, conn))
                    {
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

        // Wakamatsu 20170227
        /// <summary>
        /// テーブル全削除
        /// </summary>
        /// <param name="delSql">実行SQL</param>
        /// <param name="tran">トランザクションスコープ</param>
        /// <returns></returns>
        private bool allRecord_Delete(string delSql, TransactionScope tran)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(delSql, conn))
                    {
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
            }
            return true;
        }
        // Wakamatsu 20170227


        private string editSql(string baseSql, string[] paraArray)
        {
            string strSql = baseSql;
            for (int i = 0; i < paraArray.Length; i++)
            {
                if (i > 0) strSql += ",";
                strSql += paraArray[i];
            }
            return strSql += ")";
        }


        // 事務所マスタ（M_Office）
        private SqlCommand addParaOffice(SqlCommand cmd)
        {
            for (int j = 0; j < ofcPArray.Length; j++)
            {
                switch (j)
                {
                    case 1:
                    case 3:
                    case 4:
                    case 6:
                        cmd.Parameters.Add(ofcPArray[j], SqlDbType.NVarChar);
                        break;
                    case 7:
                    case 8:
                        cmd.Parameters.Add(ofcPArray[j], SqlDbType.VarChar);
                        break;
                    case 9:
                    case 10:
                    case 11:
                        cmd.Parameters.Add(ofcPArray[j], SqlDbType.Int);
                        break;
                    case 12:
                        cmd.Parameters.Add(ofcPArray[j], SqlDbType.Date);
                        break;
                    default:
                        cmd.Parameters.Add(ofcPArray[j], SqlDbType.Char);
                        break;
                }
            }
            return cmd;
        }

        // Wakamatsu 20170227
        //private SqlCommand addParaOfficeUpd(SqlCommand cmd)
        //{
        //    for (int j = 0; j < ofcPArray.Length; j++)
        //    {
        //        switch (j)
        //        {
        //            case 1:
        //            case 3:
        //            case 4:
        //            case 6:
        //                cmd.Parameters.Add(ofcPArray[j], SqlDbType.NVarChar);
        //                break;
        //            case 7:
        //            case 8:
        //                cmd.Parameters.Add(ofcPArray[j], SqlDbType.VarChar);
        //                break;
        //            case 9:
        //            case 10:
        //            case 11:
        //                break;
        //            case 12:
        //                cmd.Parameters.Add(ofcPArray[j], SqlDbType.Date);
        //                break;
        //            default:
        //                cmd.Parameters.Add(ofcPArray[j], SqlDbType.Char);
        //                break;
        //        }
        //    }
        //    return cmd;
        //}
        // Wakamatsu 20170227

        // 社員マスタ（M_Members）
        private SqlCommand addParaMembers(SqlCommand cmd)
        {
            for (int j = 0; j < memPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 10:
                        cmd.Parameters.Add(memPArray[j], SqlDbType.VarChar);
                        break;
                    case 1:
                    case 2:
                    case 7:
                    case 9:
                    case 13:
                    case 14:
                    case 17:
                    case 23:
                    case 24:
                    case 27:
                    case 34:
                        cmd.Parameters.Add(memPArray[j], SqlDbType.NVarChar);
                        break;
                    case 3:
                    case 4:
                    case 6:
                    case 8:
                    case 11:
                    case 12:
                    case 15:
                    case 19:
                    case 20:
                    case 21:
                    case 26:
                        cmd.Parameters.Add(memPArray[j], SqlDbType.Char);
                        break;
                    case 5:
                    case 16:
                    case 18:
                    case 22:
                    case 28:
                    case 29:
                    case 35:
                        cmd.Parameters.Add(memPArray[j], SqlDbType.Date);
                        break;
                    case 25:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                        cmd.Parameters.Add(memPArray[j], SqlDbType.Int);
                        break;
                    default:
                        break;
                }
            }
            return cmd;
        }

        // 取引先マスタ（M_Partners）
        private SqlCommand addParaPartners(SqlCommand cmd)
        {
            //       0       1        2        3        4       5        6        7        8        9        10      11       12       13
            // { "@pCod", "@name", "@phn", "@cFrm", "@sPos", "@post", "@addr", "@telN", "@faxN", "@cptl", "@rPre", "@ttl", "@celN", "@email",
            //      14       15       16      17       18       19       20       21      22       23          24        25       26       27       28      29
            //   "@bNam", "@bBrn", "@aTyp", "@aNo", "@cDay", "@pDay", "@pTyp", "@pLT", "@rCust", "@rSubC", "@rSupl", "@rOther", "@sDt", "@aCod", "@uDt", "@cTrn" };
            for (int j = 0; j < ptnPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 5:
                    case 7:
                    case 8:
                    case 12:
                    case 17:
                    case 27:
                    case 29:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.Char);
                        break;
                    case 13:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.VarChar);
                        break;
                    case 1:
                    case 2:
                    case 6:
                    case 10:
                    case 11:
                    case 14:
                    case 15:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.NVarChar);
                        break;
                    /*
                    case 3:
                    case 4:
                    case 16:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.Int);
                        break;
                    */
                    case 9:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.Decimal);
                        break;
                    case 26:
                    case 28:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.Date);
                        break;
                    default:
                        cmd.Parameters.Add(ptnPArray[j], SqlDbType.Int);
                        break;
                }
            }
            return cmd;
        }


        // 原価項目マスタ（M_Cost）

        //costPArray = new string[] { "@cCod", "@item", "@iDtl", "@unit", "@cost", "@oCod", "@mCod", "@uDt" };
        //                               0        1        2        3        4        5        6        7
        private SqlCommand addParaCost(SqlCommand cmd)
        {
            for (int j = 0; j < costPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 6:
                        cmd.Parameters.Add(costPArray[j], SqlDbType.VarChar);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        cmd.Parameters.Add(costPArray[j], SqlDbType.NVarChar);
                        break;
                    case 4:
                        cmd.Parameters.Add(costPArray[j], SqlDbType.Decimal);
                        break;
                    case 5:
                        cmd.Parameters.Add(costPArray[j], SqlDbType.Char);
                        break;
                    case 7:
                        cmd.Parameters.Add(costPArray[j], SqlDbType.Date);
                        break;
                    default:
                        break;
                }
            }
            return cmd;
        }


        // 共通マスタ（M_Common）
        private SqlCommand addParaCommon(SqlCommand cmd)
        {
            for (int j = 0; j < comPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 4:
                        cmd.Parameters.Add(comPArray[j], SqlDbType.VarChar);
                        break;
                    case 5:
                        cmd.Parameters.Add(comPArray[j], SqlDbType.Date);
                        break;
                    default:
                        cmd.Parameters.Add(comPArray[j], SqlDbType.NVarChar);
                        break;
                }
            }
            return cmd;
        }



        // 作業項目マスタ（M_WorkItems）
        private SqlCommand addParaWorkItems(SqlCommand cmd)
        {
            for (int j = 0; j < wkItmPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        cmd.Parameters.Add(wkItmPArray[j], SqlDbType.VarChar);
                        break;
                    case 5:
                        cmd.Parameters.Add(wkItmPArray[j], SqlDbType.Decimal);
                        break;
                    case 6:
                        cmd.Parameters.Add(wkItmPArray[j], SqlDbType.Char);
                        break;
                    case 7:
                        cmd.Parameters.Add(wkItmPArray[j], SqlDbType.Date);
                        break;
                    default:
                        cmd.Parameters.Add(wkItmPArray[j], SqlDbType.NVarChar);
                        break;
                }
            }
            return cmd;
        }


        // カレンダ（M_Calendar）
        private SqlCommand addParaCalendar(SqlCommand cmd)
        {
            for (int j = 0; j < calPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 2:
                        cmd.Parameters.Add(calPArray[j], SqlDbType.Date);
                        break;
                    default:
                        cmd.Parameters.Add(calPArray[j], SqlDbType.Int);
                        break;
                }
            }
            return cmd;
        }


        // 業務データ（D_Task）
        //private SqlCommand addParaTask(SqlCommand cmd)
        //{
        //    for (int j = 0; j < taskPArray.Length; j++)
        //    {
        //        switch (j)
        //        {
        //            case 0:
        //            case 2:
        //            case 3:
        //                cmd.Parameters.Add(taskPArray[j], SqlDbType.Char);
        //                break;
        //            case 1:
        //                cmd.Parameters.Add(taskPArray[j], SqlDbType.NVarChar);
        //                break;
        //            case 4:
        //            case 5:
        //                cmd.Parameters.Add(taskPArray[j], SqlDbType.Int);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return cmd;
        //}

        private SqlCommand addParaTask(SqlCommand cmd)
        {
            for (int j = 0; j < taskPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 3:
                    case 6:
                        cmd.Parameters.Add(taskPArray[j], SqlDbType.Char);
                        break;
                    case 1:
                        cmd.Parameters.Add(taskPArray[j], SqlDbType.NVarChar);
                        break;
                    case 2:
                    case 4:
                    case 5:
                        cmd.Parameters.Add(taskPArray[j], SqlDbType.Date);
                        break;
                    case 7:
                    case 8:
                    case 9:
                        cmd.Parameters.Add(taskPArray[j], SqlDbType.Int);
                        break;
                    default:
                        break;
                }
            }
            return cmd;
        }

        // 業務個別データ（D_taskInd）
        private SqlCommand addParaTaskInd(SqlCommand cmd)
        {
            for (int j = 0; j < taskIndPArray.Length; j++)
            {
                switch (j)
                {
                    case 0:
                    case 3:
                    case 4:
                    case 8:
                        cmd.Parameters.Add(taskIndPArray[j], SqlDbType.Char);
                        break;
                    case 1:
                        cmd.Parameters.Add(taskIndPArray[j], SqlDbType.Decimal);
                        break;
                    case 2:
                    case 6:
                    case 7:
                    case 9:
                        cmd.Parameters.Add(taskIndPArray[j], SqlDbType.Int);
                        break;
                    case 5:
                        cmd.Parameters.Add(taskIndPArray[j], SqlDbType.NVarChar);
                        break;
                    default:
                        break;
                }
            }
            return cmd;
        }
    }
}

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
    public class PartnersData:DbAccess
    {

        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//

        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public PartnersData()
        {
        }

        public PartnersData(DataRow dr)
        {
            PartnerID = Convert.ToInt32(dr["PartnerID"]);
            PartnerCode = Convert.ToString(dr["PartnerCode"]);
            PartnerName = Convert.ToString(dr["PartnerName"]);
            PartnerPhonetic = Convert.ToString(dr["PartnerPhonetic"]);
            CorporateForm = Convert.ToInt32(dr["CorporateForm"]);
            SignPosition = Convert.ToInt32(dr["SignPosition"]);
            PostCode = Convert.ToString(dr["PostCode"]);
            Address = Convert.ToString(dr["Address"]);
            TelNo = Convert.ToString(dr["TelNo"]);
            FaxNo = Convert.ToString(dr["FaxNo"]);
            Capital = Convert.ToDecimal(dr["Capital"]);
            Representative = Convert.ToString(dr["Representative"]);
            Title = Convert.ToString(dr["Title"]);
            CellularNo = Convert.ToString(dr["CellularNo"]);
            EMail = Convert.ToString(dr["EMail"]);
            BankName = Convert.ToString(dr["BankName"]);
            BBranchName = Convert.ToString(dr["BBranchName"]);
            AccountType = Convert.ToInt32(dr["AccountType"]);
            AccountNo = Convert.ToString(dr["AccountNo"]);
            ClosingDay = Convert.ToInt32(dr["ClosingDay"]);
            PayDay = Convert.ToInt32(dr["PayDay"]);
            PayType = Convert.ToInt32(dr["PayType"]);
            PayLT = Convert.ToInt32(dr["PayLT"]);
            RelCusto = Convert.ToInt32(dr["RelCusto"]);
            RelSubco = Convert.ToInt32(dr["RelSubco"]);
            RelSuppl = Convert.ToInt32(dr["RelSuppl"]);
            RelOther = Convert.ToInt32(dr["RelOther"]);
            StartDate = Convert.ToDateTime(dr["StartDate"]);
            AccountCode = Convert.ToString(dr["AccountCode"]);
            ChiefTrans = Convert.ToString(dr["ChiefTrans"]);
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public int PartnerID { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhonetic { get; set; }
        public int CorporateForm { get; set; }
        public int SignPosition { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public decimal Capital { get; set; }
        public string Representative { get; set; }
        public string Title { get; set; }
        public string CellularNo { get; set; }
        public string EMail { get; set; }
        public string BankName { get; set; }
        public string BBranchName { get; set; }
        public int AccountType { get; set; }
        public string AccountNo { get; set; }
        public int ClosingDay { get; set; }
        public int PayDay { get; set; }
        public int PayType { get; set; }
        public int PayLT { get; set; }
        public int RelCusto { get; set; }
        public int RelSubco { get; set; }
        public int RelSuppl { get; set; }
        public int RelOther { get; set; }
        public DateTime StartDate { get; set; }
        public string AccountCode { get; set; }
        public string ChiefTrans { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public PartnersData SelectPartnersData(string partnerCode)
        {
            SqlHandling sh = new SqlHandling("M_Partners");
            DataTable dt = sh.SelectAllData("WHERE PartnerCode = '" + partnerCode + "'");
            if (dt == null || dt.Rows.Count < 1) return null;
            PartnersData pd = new PartnersData(dt.Rows[0]);
            return pd;
        }

        public string SelectPartnerName(string partnerCode)
        {
            SqlHandling sh = new SqlHandling();
            DataTable dt = sh.SelectFullDescription(" PartnerName FROM M_Partners WHERE PartnerCode = '" + partnerCode + "'");
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;
            DataRow dr = dt.Rows[0];
            return Convert.ToString(dr["PartnerName"]);
        }


        public bool InsertPartnersData()
        {
            string sqlStr = "INSERT INTO M_Partners ("
                        + "PartnerCode, PartnerName, PartnerPhonetics, CorporateForm, SignPosition, PostCode, Address, TelNo, FaxNo, "
                        + "Capital, Representative, Title, CellularNo, Email, BankName, BBranchName, AccountType, AccountNo, "
                        + "ClosingDay, PayDay, PayType, PayLt, RelCusto, RelSubco, RelSuppl, RelOhter, StartDate, UpdateDate, ChiefTrans"
                        + " ) VALUES ("
                        + "@pCod, @pNam, @pPho, @cFrm, @sPos, @post, @addr, @tlNo, @fxNo, "
                        + "@capt, @repr, @titl, @clNo, @emal, @bNam, @bBra, @aTyp, @acNo, "
                        + "@cDay, @pDay, @paTp, @paLt, @rCst, @rSub, @rSpl, @rOth, @sDat, @uDat, @cTrn )";

            if (!executeProcess(sqlStr)) return false;

            return true;
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
            cmd.Parameters.Add("@pCod", SqlDbType.VarChar);
            cmd.Parameters.Add("@pNam", SqlDbType.NVarChar);
            cmd.Parameters.Add("@pPho", SqlDbType.NVarChar);
            cmd.Parameters.Add("@cFrm", SqlDbType.Int);
            cmd.Parameters.Add("@sPos", SqlDbType.Int);
            cmd.Parameters.Add("@post", SqlDbType.Char);
            cmd.Parameters.Add("@addr", SqlDbType.NVarChar);
            cmd.Parameters.Add("@tlNo", SqlDbType.Char);
            cmd.Parameters.Add("@fxNo", SqlDbType.Char);
            cmd.Parameters.Add("@capt", SqlDbType.Decimal);
            cmd.Parameters.Add("@repr", SqlDbType.NVarChar);
            cmd.Parameters.Add("@titl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@clNo", SqlDbType.Char);
            cmd.Parameters.Add("@emal", SqlDbType.NVarChar);
            cmd.Parameters.Add("@bNam", SqlDbType.NVarChar);
            cmd.Parameters.Add("@bBra", SqlDbType.NVarChar);
            cmd.Parameters.Add("@aTyp", SqlDbType.Int);
            cmd.Parameters.Add("@acNo", SqlDbType.Char);
            cmd.Parameters.Add("@cDay", SqlDbType.Int);
            cmd.Parameters.Add("@pDay", SqlDbType.Int);
            cmd.Parameters.Add("@paTp", SqlDbType.Int);
            cmd.Parameters.Add("@paLt", SqlDbType.Int);
            cmd.Parameters.Add("@rCst", SqlDbType.Int);
            cmd.Parameters.Add("@rSub", SqlDbType.Int);
            cmd.Parameters.Add("@rSpl", SqlDbType.Int);
            cmd.Parameters.Add("@rOth", SqlDbType.Int);
            cmd.Parameters.Add("@sDat", SqlDbType.Date);
            cmd.Parameters.Add("@uDat", SqlDbType.Date);
            cmd.Parameters.Add("@cTrn", SqlDbType.Char);

            return cmd;
        }


        private SqlCommand addValue(SqlCommand cmd)
        {
            cmd.Parameters["@pCod"].Value = PartnerCode;
            cmd.Parameters["@pNam"].Value = PartnerName;
            cmd.Parameters["@pPho"].Value = PartnerPhonetic;
            cmd.Parameters["@cFrm"].Value = CorporateForm;
            cmd.Parameters["@sPos"].Value = CorporateForm;
            cmd.Parameters["@post"].Value = PostCode;
            cmd.Parameters["@addr"].Value = Address;
            cmd.Parameters["@tlNo"].Value = TelNo;
            cmd.Parameters["@fxNo"].Value = FaxNo;
            cmd.Parameters["@capt"].Value = Capital;
            cmd.Parameters["@repr"].Value = Representative;
            cmd.Parameters["@titl"].Value = Title;
            cmd.Parameters["@clNo"].Value = CellularNo;
            cmd.Parameters["@emal"].Value = EMail;
            cmd.Parameters["@bNam"].Value = BankName;
            cmd.Parameters["@bBra"].Value = BBranchName;
            cmd.Parameters["@aTyp"].Value = AccountType;
            cmd.Parameters["@acNo"].Value = AccountNo;
            cmd.Parameters["@cDay"].Value = ClosingDay;
            cmd.Parameters["@pDay"].Value = PayDay;
            cmd.Parameters["@paTp"].Value = PayType;
            cmd.Parameters["@paLt"].Value = PayLT;
            cmd.Parameters["@rCst"].Value = RelCusto;
            cmd.Parameters["@rSub"].Value = RelSubco;
            cmd.Parameters["@rSpl"].Value = RelSuppl;
            cmd.Parameters["@rOth"].Value = RelOther;
            cmd.Parameters["@sDat"].Value = DateTime.Today.StripTime();
            cmd.Parameters["@uDat"].Value = DateTime.Today.StripTime();
            cmd.Parameters["@cTrn"].Value = ChiefTrans;

            return cmd;
        }


    }
}

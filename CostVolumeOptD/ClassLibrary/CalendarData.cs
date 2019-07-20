using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class CalendarData:DbAccess
    {

        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string holidayExistence = "SELECT * FROM M_Calendar WHERE MDate = @date";
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public CalendarData()
        {
        }

        public CalendarData(DataRow dr)
        {
            MDate = Convert.ToDateTime(dr["MDate"]);
            DType = Convert.ToInt32(dr["DType"]);
            if (Convert.ToString(dr["UpdateDate"]) == "")
            {
                UpdateDate = DateTime.Today.StripTime();
            }
            else
            {
                UpdateDate = Convert.ToDateTime(dr["UpdateDate"]);
            }
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public DateTime MDate { get; set; }
        public int DType { get; set; }
        public DateTime UpdateDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//
        public bool ExitstenceHoliday(DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    cmd = new SqlCommand(holidayExistence, conn);
                    cmd.Parameters.Add("@date", SqlDbType.DateTime);
                    cmd.Parameters["@date"].Value = date;
                    SqlDataReader dr = TryExReader(conn, cmd);
                    if (!dr.HasRows) return false;
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












    }
}

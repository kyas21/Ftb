using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary
{
    public class CostVolOp : DbAccess
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        private string tableName;
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public CostVolOp()
        {
        }

        public CostVolOp(string tableName)
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
        
        /*--------------------------------------------------------*/
        //      Method
        /*--------------------------------------------------------*/
       
    }
}

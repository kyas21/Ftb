using System;
using System.Data;

namespace ClassLibrary
{
    public class TaskCodeNameData
    {
        //--------------------------------------------------------//
        //      Field
        //--------------------------------------------------------//
        //--------------------------------------------------------//
        //      Construction
        //--------------------------------------------------------//
        public TaskCodeNameData()
        {
        }

        public TaskCodeNameData(DataRow dr)
        {
            TaskCode = Convert.ToString(dr["TaskCode"]);
            TaskName = Convert.ToString(dr["TaskName"]);
            LeaderMCode = Convert.ToString(dr["LeaderMCode"]);
            OfficeCode = Convert.ToString(dr["OfficeCode"]);
            Department = Convert.ToString(dr["Department"]);
            //if (!String.IsNullOrEmpty(Convert.ToString(dr["TaskID"]))) TaskID = Convert.ToInt32(dr["TaskID"]);
            //if (!String.IsNullOrEmpty(Convert.ToString(dr["TaskIndID"]))) TaskIndID = Convert.ToInt32(dr["TaskIndID"]);
            TaskID = dr.Field<Int32?>("TaskID") ?? default(Int32);
            TaskIndID = dr.Field<Int32?>("TaskIndID") ?? default(Int32);
            Partner = "";
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }


        public TaskCodeNameData( DataRow dr, int idx )
        {
            TaskCode = dr.Field<String>( "TaskCode" ) ?? default( String );
            TaskName = dr.Field<String>( "TaskName" ) ?? default( String );
            LeaderMCode = "";
            OfficeCode = "";
            Department = "";
            TaskID = -1;
            TaskIndID = -1;
            Partner = dr.Field<String>( "PartnerName" ) ?? default( String );
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }
        //--------------------------------------------------------//
        //      Property
        //--------------------------------------------------------//
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string LeaderMCode { get; set; }
        public string OfficeCode { get; set; }
        public string Department { get; set; }
        public int TaskID { get; set; }
        public int TaskIndID { get; set; }
        public string Partner { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //--------------------------------------------------------//
        //      Method
        //--------------------------------------------------------//

        public object Clone()
        {
            TaskCodeNameData cloneData = new TaskCodeNameData();
            cloneData.TaskCode = this.TaskCode;
            cloneData.TaskName = this.TaskName;
            cloneData.LeaderMCode = this.LeaderMCode;
            cloneData.OfficeCode = this.OfficeCode;
            cloneData.Department = this.Department;
            cloneData.TaskID = this.TaskID;
            cloneData.TaskIndID = this.TaskIndID;
            cloneData.Partner = this.Partner;
            cloneData.StartDate = this.StartDate;
            cloneData.EndDate = this.EndDate;
            return cloneData;
        }
    }
}

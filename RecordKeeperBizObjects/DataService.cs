using CPUFramework;
using System.Data;
using System.Data.SqlClient;

namespace RecordKeeperBizObjects
{
    public static class DataService
    {


        public static DataTable GetUSGovRecordSummary()
        {

            string sql = "select Num = count(*), TableDesc = 'Parties' from Party union select Num = count(*), TableDesc = 'Presidents' from President";
            return CPUFramework.SQLUtility.GetDataTable(DataUtility.ConnectionString, sql);

        }

        public static DataTable GetPartyList(bool IncludeBlank = false)
        {

            SqlCommand cmd = SQLUtility.GetSQLCommand(DataUtility.ConnectionString, "PartyGet");

            cmd.Parameters["@All"].Value = 1;
            cmd.Parameters["@IncludeBlank"].Value = IncludeBlank;
            return CPUFramework.SQLUtility.GetDataTable(cmd, DataUtility.ConnectionString );
        }

        public static DataTable GetPresidentList()
        {
            SqlCommand cmd = SQLUtility.GetSQLCommand(DataUtility.ConnectionString, "PresidentGet");
          
            cmd.Parameters["@All"].Value = 1;

            return SQLUtility.GetDataTable(cmd,DataUtility.ConnectionString);
        }

        public static DataTable GetPresidentDetail(int id)
        {
            SqlCommand cmd = SQLUtility.GetSQLCommand(DataUtility.ConnectionString,"PresidentGet");
            cmd.Parameters["@PresidentId"].Value = id;
         
            return SQLUtility.GetDataTable(cmd,DataUtility.ConnectionString);
         }

        public static DataTable GetExecutiveOrdersForPresident(int presidentid)
        {
            SqlCommand cmd = SQLUtility.GetSQLCommand(DataUtility.ConnectionString, "ExecutiveOrderGet");
            cmd.Parameters["@PresidentId"].Value = presidentid;
            return SQLUtility.GetDataTable(cmd,DataUtility.ConnectionString);
        }

        public static DataTable GetPresidentWithRawSQL(int num )
        {
            DataTable dt = SQLUtility.GetDataTable(DataUtility.ConnectionString, "Select * from President where num = 13");
            return dt;
        }
    }
}

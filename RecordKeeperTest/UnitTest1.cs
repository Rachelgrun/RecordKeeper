using NUnit.Framework;
using RecordKeeperBizObjects;
using System.Data;
using CPUFramework;
using Dapper;
using System.Data.SqlClient;

namespace RecordKeeperTest
{
    public class Tests
    {
        int totalprez = 0;
        string connstring = "";
        int maxpreznum = 0;
        int maxprezid = 0;
        int prezwithexecorderid = 0;

        [SetUp]
        public void Setup()
        {
           connstring =  DataUtility.SetConnectionString("rgrunwald.database.windows.net,1433", "RecordKeeperDB", "rgrunwaldadmin", "CPU123!@#");
            DataTable dt =SQLUtility.GetDataTable(connstring, "select total = count(*) from president");
            totalprez = (int)dt.Rows[0]["total"];

            dt = SQLUtility.GetDataTable(connstring, "select top 1 p.presidentId, p.Num from president p order by p.num desc");
            maxpreznum = (int)dt.Rows[0]["Num"];
            maxprezid = (int)dt.Rows[0]["PresidentId"];
            dt = SQLUtility.GetDataTable(connstring, "select top 1 p.presidentId, p.Num from president p join ExecutiveOrders e on e.PresidentId = p.PresidentId order by p.num desc");
            prezwithexecorderid = (int)dt.Rows[0]["PresidentId"];
        }

        //[Test]
        //public void TestPresidentList()
        //{
        //    DataTable dt = DataService.GetPresidentList();
        //    TestContext.WriteLine("Num president " + dt.Rows.Count.ToString());
        //    Assert.IsTrue(dt.Rows.Count == totalprez);
           
        //}
        ////[Test]
        ////public void TestPartyList()
        ////{
        ////    DataTable dt = DataService.GetPartyList();
        ////    TestContext.WriteLine("Num Parties " + dt.Rows.Count.ToString());
        ////    Assert.IsTrue(dt.Rows.Count == 7);
          
        ////}
        ////[Test]
        ////public void TestUsGovSummary()
        ////{
        ////    DataTable dt = DataService.GetUSGovRecordSummary();
        ////    TestContext.WriteLine("Num records for summary  " + dt.Rows.Count.ToString());
        ////    Assert.IsTrue(dt.Rows.Count == 2);
            
        ////}
        //[Test]
        //public void TestLoadPresidentDapper()
        //{
        //    using (SqlConnection conn = new SqlConnection(connstring))
        //    {
        //        bizPresident prez = conn.QueryFirstOrDefault<bizPresident>("select top 1 * from president");
        //        TestContext.WriteLine(prez.FullDesc());
        //    }
        //}

        //[Test]
        //public void TestLoadPresident()
        //{
         
        //    bizPresident prez =bizPresident.Get(maxprezid);
         
        //    TestContext.WriteLine(" Prez FullDesc = " + prez.FullDesc());
        //    Assert.IsTrue(prez.Num == maxpreznum);
        //}
        //[Test]
        //public void TestNewPresident()
        //{
        //    bizPresident prez = new bizPresident();
        //    prez.FirstName = "Yonason";
        //    prez.LastName = "Adams";
        //    prez.DateBorn = System.DateTime.Now.AddMonths(-450);
        //    prez.DateDied = System.DateTime.Now;
        //    prez.TermStart = System.DateTime.Now.Year;
        //    prez.Save();
        //    TestContext.WriteLine("New President num  = " + prez.Num.ToString() + ". Should be " + (maxpreznum + 1));
        //    Assert.IsTrue(prez.Num == maxpreznum + 1);
        //}

        //[Test]
        //public void TestChangePresidentNumber()
        //{
        //    bizPresident prez =  bizPresident.Get(maxprezid);
         

        //    prez.Num = prez.Num + 1;
        //    Assert.Throws<CPUException>(() => prez.Save(), "You cannot change presidents number");
            

        //    TestContext.WriteLine(prez.FullDesc());
        //}
        //[Test]
        //public void TestDeletPresidentWithExecutiveOrder()
        //{
        //    bizPresident prez = bizPresident.Get(prezwithexecorderid);
           
        //   Assert.Throws<CPUException>(()=> prez.Delete());
        //}


    }
}
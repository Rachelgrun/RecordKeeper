using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CPUFramework;
using RecordKeeperBizObjects;
using System.Data;
using NUnit.Framework;
using System.Data.SqlClient;

namespace RecordKeeperTest
{
    internal class DapperTest
    {
      
            int totalprez = 0;
            string connstring = "";
            int maxpreznum = 0;
            int maxprezid = 0;
            int prezwithexecorderid = 0;
            int maxpartyid = 0;

            [SetUp]
            public void Setup()
            {
           
                connstring = DataUtility.SetConnectionString("rgrunwald.database.windows.net,1433", "RecordKeeperDB", "rgrunwaldadmin", "CPU123!@#");
                DataTable dt = SQLUtility.GetDataTable(connstring, "select total = count(*) from president");
                totalprez = (int)dt.Rows[0]["total"];

                dt = SQLUtility.GetDataTable(connstring, "select top 1 p.presidentId, p.Num from president p order by p.num desc");
                maxpreznum = (int)dt.Rows[0]["Num"];
                maxprezid = (int)dt.Rows[0]["PresidentId"];
                dt = SQLUtility.GetDataTable(connstring, "select top 1 p.presidentId, p.Num from president p join ExecutiveOrders e on e.PresidentId = p.PresidentId order by p.num desc");
                prezwithexecorderid = (int)dt.Rows[0]["PresidentId"];
                dt = SQLUtility.GetDataTable(connstring, "select top 1 * from party p order by p.partyid desc");
                maxpartyid = (int)dt.Rows[0]["partyid"];
              
        }

            //Crud

            //Load by primary key


            private bizPresident LoadPrez(int Id)
            {
         
            bizPresident prez =bizPresident.Get(Id);
        
            return prez;

            }

            //Load List
            private List<bizPresident>LoadListPrez()
            {
            return bizPresident.GetAll();

            }
            //Insert new
            private bizPresident InsertNewPrez()
            {
            bizPresident prez =new bizPresident();
                prez.PartyId = maxpartyid;
                prez.FirstName = "Yonason";
                prez.LastName = "Adams";
                prez.DateBorn = System.DateTime.Now.AddMonths(-450);
                prez.DateDied = System.DateTime.Now;
                prez.TermStart = System.DateTime.Now.Year;



          

            prez.Save();


                return prez;
            }
            //Delete

            private int DeletePrez(int Id = 0)
            {
                bizPresident p;
                if (Id == 0)
                {
                    p = this.InsertNewPrez();
                }
                else
                {
                    p = this.LoadPrez(Id);
                }

                Id = p.PresidentId;

            p.Delete();

                return Id;

            }




            //Update
            private void SwapPrezFirstName(bizPresident prez)
            {
                string lastname = prez.LastName;
                string firstname = prez.FirstName;
                prez.LastName = firstname;
                prez.FirstName = lastname;
            prez.Save();
        
                

            }
            private void ChangePrezNum()
            {
                bizPresident prez = this.LoadPrez(maxprezid);
                prez.Num = prez.Num + 1;
            prez.Save();
                //DynamicParameters dp = new DynamicParameters(prez);
                //dp.Add("Message", "", direction: ParameterDirection.InputOutput);
                //dp.Add("return_value", "", direction: ParameterDirection.ReturnValue);
                //using (SqlConnection conn = GetConnection())
                //{
                //    conn.Execute("PresidentUpdate", dp, commandType: CommandType.StoredProcedure);
                //}
                //int ret = dp.Get<int>("return_value");
                //string msg = dp.Get<string>("Message");

                //if(ret == 1 )
                //{
                //    throw new CPUException(msg);
                //}

            }

            //delete that fails
            //update that fails
            //update validation

            //tests
            [Test]
            public void TestLoadPrezList()
            {
                List<bizPresident> lst = this.LoadListPrez();
                TestContext.WriteLine("num of prez " + lst.Count);
                Assert.IsTrue(lst.Count == totalprez);
            }

            [Test]
            public void TestLoadPrez()
            {
                bizPresident p = this.LoadPrez(maxprezid);
                TestContext.WriteLine(p.FullDesc());
                Assert.IsTrue(p.Num == maxpreznum);
            }

        [Test]
        public void TestLoadPrezByNum()
        {
            bizPresident p = bizPresident.Get("Num", maxpreznum);
            TestContext.WriteLine(p.FullDesc());
            Assert.IsTrue(p.PresidentId ==maxprezid);
        }
        [Test]
            public void TestInsertNewPrez()
            {
                bizPresident p = this.InsertNewPrez();
            TestContext.WriteLine("Expected PresidentId > " + (maxpreznum) + " value = " + p.PresidentId);
            TestContext.WriteLine("Expected = " + (maxpreznum + 1) + " value = " + p.Num);
            TestContext.WriteLine("Expected code not blank. Code = " + p.Code);
                Assert.IsTrue(p.Num == maxpreznum + 1 && p.PresidentId > maxprezid && string.IsNullOrEmpty(p.Code) ==false);
            }
            [Test]
            public void TestUpdatePrez()
            {
                bizPresident prez = this.LoadPrez(maxprezid);
                string lastname = prez.LastName;
                string firstname = prez.FirstName;
                this.SwapPrezFirstName(prez);
                prez = this.LoadPrez(maxprezid);
                TestContext.WriteLine("Original " + firstname + " " + lastname + " Current " + prez.FirstName + " " + prez.LastName);
                Assert.IsTrue(prez.LastName == firstname && prez.FirstName == lastname);
            }
            [Test]
            public void TestDeleteNewPrez()
            {
                int id = this.DeletePrez();
                bizPresident p = this.LoadPrez(id);
                Assert.IsTrue(p.PresidentId == 0);
            }
            [Test]
            public void TestDeletePrezWithExecOrder()
            {

               CPUException? ex =  Assert.Throws<CPUException>(() => this.DeletePrez(prezwithexecorderid));
                TestContext.WriteLine(ex?.Message);
            }


            [Test]
            public void TestChangePrezNum()
            {
                CPUException? ex = Assert.Throws<CPUException>(() => this.ChangePrezNum());
                TestContext.WriteLine(ex?.Message);
            }
        
        [Test]
        public void TestSearchPrez()
        {
            List<bizPresident> lst = bizPresident.Search("bush");
            TestContext.WriteLine("List Count= " + lst.Count.ToString());
            Assert.IsTrue(lst.Count == 2);
        }

    }


}

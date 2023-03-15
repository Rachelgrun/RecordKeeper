using CPUFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace RecordKeeperBizObjects
{
    public class bizPresident:bizObject<bizPresident>
    {
       
        string lastnameval = "";
        string firstnameval = "";
        int numval;
        int termstartval;
        int termendval;
        int partyidval=0;
        DateTime datebornval;
        DateTime? datediedval;
        string codeval = "";

        public bizPresident() 
        {
           
            this.DynamParamForUpdates.Output(this, x => x.Num);
            this.DynamParamForUpdates.Output(this, x => x.Code);
           // this.DynamParamForUpdates.Output(this, x => x.DateDied, size: 20);
        }

        //public void Load(int Id)
        //{
        //    using (SqlConnection conn = new SqlConnection(DataService.ConnectionString))
        //    {
        //        DynamicParameters dp = new DynamicParameters();
        //        dp.Add(this.PrimaryKeyName, Id);
        //       bizPresident prez =  conn.QueryFirstOrDefault<bizPresident>(this.SprocName(SprocActionEnum.Get), dp, commandType: CommandType.StoredProcedure);
        //        // return conn.Query<bizPresident>("PresidentGet", new {All = true},commandType: CommandType.StoredProcedure).ToList();
        //        //this.PrimaryKeyName = prez.PrimaryKeyValue
        //        //this.LastName = prez.LastName;
        //        }
        //}



        public static List<bizPresident> Search(string criteria)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("SearchCriteria", criteria);
            return GetAllFromsproc("PresidentSearch", dp); 
        }

       
        public int PresidentId { get => this.PrimaryKeyValue; set => this.PrimaryKeyValue = value; }

        [Required]
        [Display(Name = "Party")]
        public int PartyId
        {
            get
            {
                return partyidval;
            }
            set
            {
                partyidval = value;
                InvokePropertyChanged();
            }
        }

        [Display (Name = "Last Name")]
        public string LastName
        {
            get
            {
                return lastnameval;
            }
            set
            {
                lastnameval = value;
                InvokePropertyChanged();
            }
        }
        
        [Display(Name ="First Name")]
        public string FirstName
        {
            get
            {
                return firstnameval;
            }
            set
            {
                firstnameval = value;
                InvokePropertyChanged();
            }
        }
        public int Num
        {
            get
            {
                return numval;
            }
            set
            {
                numval = value;
                InvokePropertyChanged();
            }
        }
        public int TermStart
        {
            get
            {
                return termstartval;
            }
            set
            {
                termstartval = value;
                InvokePropertyChanged();
            }
        }
        public int TermEnd
        {
            get
            {
                return termendval;
            }
            set
            {
                termendval= value;
                InvokePropertyChanged();
            }
        }
        public DateTime DateBorn
        {
            get
            {
                return datebornval;
            }
            set
            {
              datebornval= value;
                InvokePropertyChanged();
            }
        }
        public DateTime? DateDied
        {
            get
            {
                return datediedval;
            }
            set
            {
                datediedval = value;
                InvokePropertyChanged();
            }
        }
        public string Code
        {
            get
            {
                return codeval;
            }
            set
            {
                codeval = value;
                InvokePropertyChanged();
            }
        }
        
        public string FullDesc(){return this.Num + " " + this.LastName + " " + this.FirstName; }
        public List<bizParty> PartyList()
        {
            return bizParty.GetAll();
        }

    }
}

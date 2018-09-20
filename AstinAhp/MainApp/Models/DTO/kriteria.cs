using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainApp.Domains;
using Ocph.DAL;

namespace MainApp.Models.DTO  
{ 
     [TableName("kriteria")] 
     public class kriteria :BaseNotify
   {
          [PrimaryKey("Id")] 
          [DbColumn("Id")] 
          public int Id 
          { 
               get{return _id;} 
               set{ 

                    SetProperty(ref _id, value);
                     }
          } 

          [DbColumn("Kode")] 
          public string Kode 
          { 
               get{return _kode;} 
               set{ 

                    SetProperty(ref _kode, value);
                     }
          } 

          [DbColumn("Nama")] 
          public string Nama 
          { 
               get{return _nama;} 
               set{ 

                    SetProperty(ref _nama, value);
                     }
          } 

          [DbColumn("Nilai")] 
          public double Nilai 
          { 
               get{return _nilai;} 
               set{ 

                    SetProperty(ref _nilai, value);
                     }
          }

        public SubKriteriaCollection SubKriterias { get;  set; }

        private int  _id;
           private string  _kode;
           private string  _nama;
           private double  _nilai;
      }
}



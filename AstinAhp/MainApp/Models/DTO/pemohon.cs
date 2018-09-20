using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainApp.Domains;
using Ocph.DAL;

namespace MainApp.Models.DTO  
{ 
     [TableName("pemohon")] 
     public class pemohon :BaseNotify
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

          [DbColumn("Nama")] 
          public string Nama 
          { 
               get{return _nama;} 
               set{ 

                    SetProperty(ref _nama, value);
                     }
          } 

          [DbColumn("JenisKelamin")] 
          public string JenisKelamin 
          { 
               get{return _jeniskelamin;} 
               set{ 

                    SetProperty(ref _jeniskelamin, value);
                     }
          } 

          [DbColumn("Alamat")] 
          public string Alamat 
          { 
               get{return _alamat;} 
               set{ 

                    SetProperty(ref _alamat, value);
                     }
          }

        public DataPemohonCollecion Datas { get; internal set; }

        private int  _id;
           private string  _nama;
           private string  _jeniskelamin;
           private string  _alamat;
      }
}



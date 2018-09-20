using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace MainApp.Models.DTO  
{ 
     [TableName("subkriteria")] 
     public class subkriteria :BaseNotify 
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

          [DbColumn("Kode")] 
          public string Kode 
          { 
               get{return _kode;} 
               set{ 

                    SetProperty(ref _kode, value);
                     }
          } 

          [DbColumn("KriteriaId")] 
          public int KriteriaId 
          { 
               get{return _kriteriaid;} 
               set{ 

                    SetProperty(ref _kriteriaid, value);
                     }
          } 

          [DbColumn("NIlai")] 
          public double NIlai 
          { 
               get{return _nilai;} 
               set{ 

                    SetProperty(ref _nilai, value);
                     }
          } 

          private int  _id;
           private string  _nama;
           private string  _kode;
           private int  _kriteriaid;
           private double  _nilai;
      }
}



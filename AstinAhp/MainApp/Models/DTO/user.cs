using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace MainApp.Models.DTO  
{ 
     [TableName("user")] 
     public class user :BaseNotify
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

          [DbColumn("UserName")] 
          public string UserName 
          { 
               get{return _username;} 
               set{ 

                    SetProperty(ref _username, value);
                     }
          } 

          [DbColumn("Password")] 
          public string Password 
          { 
               get{return _password;} 
               set{ 

                    SetProperty(ref _password, value);
                     }
          } 

          private int  _id;
           private string  _username;
           private string  _password;
      }
}



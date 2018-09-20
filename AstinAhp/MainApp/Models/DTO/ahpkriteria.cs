using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace MainApp.Models.DTO  
{ 
     [TableName("ahpkriteria")] 
     public class ahpkriteria :BaseNotify
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

          [DbColumn("Nilai")] 
          public double Nilai 
          { 
               get{return _nilai;} 
               set{ 

                    SetProperty(ref _nilai, value);
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

        [DbColumn("PasanganId")]
        public int PasanganId
        {
            get { return _PasanganId; }
            set
            {

                SetProperty(ref _PasanganId, value);
            }
        }

        private int  _id;
           private double  _nilai;
           private int  _kriteriaid;
        private int _PasanganId;
    }
}



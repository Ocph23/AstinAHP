using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace MainApp.Models.DTO  
{
    [TableName("datapemohon")]
    public class datapemohon : BaseNotify
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id
        {
            get { return _id; }
            set
            {

                SetProperty(ref _id, value);
            }
        }

        [DbColumn("Nilai")]
        public double Nilai
        {
            get { return _nilai; }
            set
            {

                SetProperty(ref _nilai, value);
            }
        }

        [DbColumn("Value")]
        public string Value
        {
            get { return _value; }
            set
            {

                SetProperty(ref _value, value);
            }
        }

        [DbColumn("PemohonId")]
        public int PemohonId
        {
            get { return _pemohonid; }
            set
            {

                SetProperty(ref _pemohonid, value);
            }
        }

        [DbColumn("KriteriaId")]
        public int KriteriaId
        {
            get { return _kriteriaid; }
            set
            {

                SetProperty(ref _kriteriaid, value);
            }
        }

        private int _id;
        private double _nilai;
        private int _pemohonid;
        private int _kriteriaid;
        private string _value;
    }
}



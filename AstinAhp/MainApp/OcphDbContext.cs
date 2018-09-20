using MainApp.Models.DTO;
using Ocph.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp
{
    public class OcphDbContext: Ocph.DAL.Provider.MySql.MySqlDbConnection
    {
        public OcphDbContext()
        {

            this.ConnectionString = "Server =localhost; database =dbbantuanmodal; UID =root; password =; CharSet = utf8; Persist Security Info = True";
        }

        public IRepository<kriteria> Kriterias {get { return new Repository<kriteria>(this); }}
        public IRepository<ahpkriteria> AHPKriterias { get { return new Repository<ahpkriteria>(this); } }
        public IRepository<subkriteria> SubKriterias { get { return new Repository<subkriteria>(this); } }
        public IRepository<ahpsub> AHPSubKriterias { get { return new Repository<ahpsub>(this); } }
        public IRepository<pemohon> Pemohons{ get { return new Repository<pemohon>(this); } }
        public IRepository<datapemohon> DataPemohons{ get { return new Repository<datapemohon>(this); } }
        public IRepository<user> Users{ get { return new Repository<user>(this); } }

    }
}

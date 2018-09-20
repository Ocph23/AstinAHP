using AHPLib;
using AHPLib.Models;
using MainApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Domains
{
     
    public class MainProject:BaseProject
    {
        public MainWindowViewModel Main { get; private set; }
     
        public void Initialitation()
        {
            Main = Helper.GetMainViewModel();
            var  DataCriteria = new Domains.KriteriaDataCollection();

            foreach(var item in Main.Kriterias)
            {
                var criteria = new MCriteria(item.Id, item.Kode, item.Nama);

                var subDataCriteria = new Domains.SubKriteriaDataCollection(item);

                var listSubBobotCriteria = new List<MBobot>();

                foreach (var subData in subDataCriteria)
                {
                    listSubBobotCriteria.Add(new MBobot { BobotType = BobotType.Criteria, Row = subData.SubKriteriaId - 1, Column = subData.PasanganId - 1 });
                }

                criteria.SetBobot(listSubBobotCriteria);
                criteria.SubCriteria.Add(criteria);
                
            }

            var listBobotCriteria = new List<MBobot>();

            foreach(var item in DataCriteria)
            {
                listBobotCriteria.Add(new MBobot { BobotType = BobotType.Criteria, Row = item.KriteriaId - 1, Column = item.PasanganId - 1 });
            }

            this.SetBobot(listBobotCriteria);

        }

       
    }
}

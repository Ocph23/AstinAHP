﻿using AHPLib;
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
        public MainWindowViewModel Main { get;  set; }

        public void Initialitation()
        {
            Main = Helper.GetMainViewModel();
            var DataCriteria = new Domains.KriteriaDataCollection();
            //Set Criteria
            foreach (var item in Main.Kriterias)
            {
                var criteria = new MCriteria(item.Id, item.Kode, item.Nama);
                foreach (var sub in item.SubKriterias)
                {
                    var subcriteria = new MCriteria(sub.Id, sub.Kode, sub.Nama);
                    criteria.AddSubCriteria(subcriteria);
                }
                this.AddSubCriteria(criteria);
            }

            var listBobotCriteria = new List<MBobot>();
            foreach (var item in DataCriteria)
            {
                listBobotCriteria.Add(new MBobot { Value = item.Nilai, BobotType = BobotType.Criteria, Row = item.KriteriaId - 1, Column = item.PasanganId - 1 });
            }

            this.SetBobot(listBobotCriteria);

            foreach (var criteria in SubCriteria)
            {

                var subDataCriteria = new Domains.SubKriteriaDataCollection(criteria.Id);
                var listSubBobotCriteria = new List<MBobot>();
                int r = 0;
                foreach (var subCriteria in criteria.SubCriteria)
                {
                    int c = 0;
                    foreach (var subData in subDataCriteria.Where(O => O.SubKriteriaId == subCriteria.Id))
                    {

                        listSubBobotCriteria.Add(new MBobot { ParentId = criteria.Id, Value = subData.Nilai, BobotType = BobotType.Criteria, Row = r, Column = c });
                        c++;
                    }
                    r++;
                }
                criteria.SetBobot(listSubBobotCriteria);
            }

            this.Calculate(0);
            var res = this.ConsistencyIndex;
        }

        internal void Clear()
        {
            this.Clear();
        }
    }
}

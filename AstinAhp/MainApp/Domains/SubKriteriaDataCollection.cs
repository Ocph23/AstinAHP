using MainApp.Models.DTO;
using Ocph.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Domains
{
  public class SubKriteriaDataCollection : BaseNotify, IOcphCollection<ahpsub>
    {
        private List<ahpsub> datas;
        private ahpsub _selected;
        public event delOnSelected OnSelected;
        public SubKriteriaDataCollection(int kriteriaId)
        {
            KriteriaId = kriteriaId;
            if (datas == null)
                datas = GetDatas();
        }

        private List<ahpsub> GetDatas()
        {
            using (var db = new OcphDbContext())
            {
                var result = from a in db.SubKriterias.Where(O => O.KriteriaId == KriteriaId)
                             join b in db.AHPSubKriterias.Select() on a.Id equals b.SubKriteriaId
                             select b;
                return result.ToList();
            }
        }

        public ahpsub SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
                OnSelected?.Invoke(value);
            }
        }

        public int Count => datas.Count;

        public bool IsReadOnly => false;

        public int KriteriaId { get; }

        public void Add(ahpsub item)
        {
            datas.Add(item);
        }

        public void Clear()
        {
            datas.Clear();
        }

        public bool Contains(ahpsub item)
        {
            return datas.Contains(item);
        }

        public void CopyTo(ahpsub[] array, int arrayIndex)
        {
            datas.CopyTo(array, arrayIndex);
        }

        public bool Remove(ahpsub item)
        {
            try
            {
                var selected = datas.Where(O => O.Id == item.Id).FirstOrDefault();
                if (selected != null)
                {
                    using (var db = new OcphDbContext())
                    {
                        if (db.Kriterias.Delete(O => O.Id == item.Id))
                        {
                            datas.Remove(item);
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerator<ahpsub> GetEnumerator()
        {
            return datas.GetEnumerator();
        }

      

        public bool Update(ahpsub t)
        {

            using (var db = new OcphDbContext())
            {
                var trans = db.BeginTransaction();
                try
                {
                    foreach (var item in datas)
                    {
                        if (item.Id <= 0)
                        {
                            item.Id = db.AHPSubKriterias.InsertAndGetLastID(item);
                            if (item.Id <= 0)
                                throw new SystemException("Data Tidak Tersimpan");
                        }
                        else
                        {
                            if (!db.AHPSubKriterias.Update(O => new { O.Nilai }, item, O => O.Id == item.Id))
                                throw new SystemException("Data Tidak Tersimpan");
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new SystemException(ex.Message);

                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return datas.GetEnumerator();
        }
    }
}

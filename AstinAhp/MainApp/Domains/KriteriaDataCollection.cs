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
    public class KriteriaDataCollection :BaseNotify, IOcphCollection<ahpkriteria>
    {
        private List<ahpkriteria> datas;
        private ahpkriteria _selected;
        public event delOnSelected OnSelected;
        public KriteriaDataCollection()
        {
            if (datas == null)
                datas = GetDatas();
        }

        private List<ahpkriteria> GetDatas()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.AHPKriterias.Select().ToList();
                return result;
            }
        }

        public ahpkriteria SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
                OnSelected?.Invoke(value);
            }
        }

        public int Count =>datas.Count;

        public bool IsReadOnly => false;


        public void Add(ahpkriteria item)
        {
            datas.Add(item);
        }

        public void Clear()
        {
            datas.Clear();
        }

        public bool Contains(ahpkriteria item)
        {
            return datas.Contains(item);
        }

        public void CopyTo(ahpkriteria[] array, int arrayIndex)
        {
            datas.CopyTo(array, arrayIndex);
        }

        public bool Remove(ahpkriteria item)
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

        public IEnumerator<ahpkriteria> GetEnumerator()
        {
            return datas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return datas.GetEnumerator();
        }

        public bool Update(ahpkriteria t)
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
                          item.Id= db.AHPKriterias.InsertAndGetLastID(item);
                            if (item.Id <= 0)
                                throw new SystemException("Data Tidak Tersimpan");
                        }
                        else
                        {
                            if (!db.AHPKriterias.Update(O => new { O.Nilai }, item, O => O.Id == item.Id))
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
    }
}

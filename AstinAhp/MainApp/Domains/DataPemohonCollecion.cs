using MainApp.Models.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Domains
{
  public  class DataPemohonCollecion:IOcphCollection<datapemohon>
    {
        public DataPemohonCollecion(pemohon data)
        {
            this.Data = data;
            if (list == null)
                list = GetKriterias();

        }
        private List<datapemohon> list;
        private List<datapemohon> GetKriterias()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.DataPemohons.Where(O=>O.PemohonId==Data.Id).ToList();
                if (result == null)
                    result = new List<datapemohon>();
                return result;
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public datapemohon SelectedItem { get; set; }
        private pemohon Data { get; set; }

        public void Add(datapemohon item)
        {
            using (var db = new OcphDbContext())
            {
                item.Id = db.DataPemohons.InsertAndGetLastID(item);
                if (item.Id > 0)
                {
                    list.Add(item);
                }
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(datapemohon item)
        {
            return list.Contains(item);
        }

        public void CopyTo(datapemohon[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(datapemohon item)
        {
            try
            {
                var selected = list.Where(O => O.Id == item.Id).FirstOrDefault();
                if (selected != null)
                {
                    using (var db = new OcphDbContext())
                    {
                        if (db.DataPemohons.Delete(O => O.Id == item.Id))
                        {
                            list.Remove(item);
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

        public IEnumerator<datapemohon> GetEnumerator()
        {
            return list.GetEnumerator();
        }



        public bool Update(datapemohon t)
        {
            if (SelectedItem != null)
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        return db.DataPemohons.Update(O => new { O.Nilai }, SelectedItem, O => O.Id == t.Id );
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

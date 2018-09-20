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

    public delegate void delOnSelected(object item);
  public class PemohonCollection:BaseNotify, IOcphCollection<pemohon>
    {
        public event delOnSelected OnSelected;
        public PemohonCollection()
        {
            if (list == null)
                list = GetItes();
        }
        private List<pemohon> list;
        private pemohon _selected;

        private List<pemohon> GetItes()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Pemohons.Select().ToList();
                foreach (var item in result)
                {
                    item.Datas = new DataPemohonCollecion(item);
                }
                return result;
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public pemohon SelectedItem {
            get { return _selected; }
            set
            {
             SetProperty(ref   _selected ,value);
                OnSelected?.Invoke(value);
            }
        }

        public void Add(pemohon item)
        {
            using (var db = new OcphDbContext())
            {
                item.Id = db.Pemohons.InsertAndGetLastID(item);
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

        public bool Contains(pemohon item)
        {
            return list.Contains(item);
        }

        public void CopyTo(pemohon[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(pemohon item)
        {
            try
            {
                var selected = list.Where(O => O.Id == item.Id).FirstOrDefault();
                if (selected != null)
                {
                    using (var db = new OcphDbContext())
                    {
                        if (db.Pemohons.Delete(O => O.Id == item.Id))
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

        public IEnumerator<pemohon> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Update(pemohon t)
        {
            if (SelectedItem != null)
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        t.Id = SelectedItem.Id;
                        var pemohonSaved = db.Pemohons.Update(O => new { O.Nama, O.Alamat, O.JenisKelamin }, t, O => O.Id == t.Id);
                        if(pemohonSaved)
                        {
                            SelectedItem.Nama = t.Nama;
                            SelectedItem.Alamat = t.Alamat;
                            SelectedItem.JenisKelamin = t.JenisKelamin;
                            return true;
                        }
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
            return list.GetEnumerator();
        }
    }
}

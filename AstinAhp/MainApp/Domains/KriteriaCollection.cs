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
    public class KriteriaCollection:BaseNotify, IOcphCollection<kriteria>
    {
        public event delOnSelected OnSelected;
        public KriteriaCollection()
        {
            if (kriterias == null)
                kriterias = GetKriterias();

        }
        private List<kriteria> kriterias;
        private kriteria _selected;

        private List<kriteria> GetKriterias()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Kriterias.Select().ToList();
                foreach(var item in result)
                {
                    item.SubKriterias = new SubKriteriaCollection(item);
                }
                return result;
            }
        }
        
        public int Count => kriterias.Count;

        public bool IsReadOnly => false;

        public kriteria SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
                OnSelected?.Invoke(value);
            }
        }


        public void Add(kriteria item)
        {
            using (var db = new OcphDbContext())
            {
                item.Id = db.Kriterias.InsertAndGetLastID(item);
                if (item.Id > 0)
                {
                    kriterias.Add(item);
                }
            }
        }

        public void Clear()
        {
            kriterias.Clear();
        }

        public bool Contains(kriteria item)
        {
            return kriterias.Contains(item);
        }

        public void CopyTo(kriteria[] array, int arrayIndex)
        {
            kriterias.CopyTo(array, arrayIndex);
        }

        public bool Remove(kriteria item)
        {
            try
            {
                var selected = kriterias.Where(O => O.Id == item.Id).FirstOrDefault();
                if (selected != null)
                {
                    using (var db = new OcphDbContext())
                    {
                        if(db.Kriterias.Delete(O=>O.Id==item.Id))
                        {
                            kriterias.Remove(item);
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

        public IEnumerator<kriteria> GetEnumerator()
        {
               return kriterias.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return kriterias.GetEnumerator();
        }

        public bool Update(kriteria t)
        {
          if(SelectedItem!=null)
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        var isSaved= db.Kriterias.Update(O => new { O.Kode, O.Nama, O.Nilai }, t, O => O.Id == t.Id);
                        if(isSaved)
                        {
                            SelectedItem.Kode = t.Kode;
                            SelectedItem.Nama = t.Nama;
                            SelectedItem.Nilai = t.Nilai;
                            
                        }
                        return isSaved;
                    }
                    catch 
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}

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
    public class SubKriteriaCollection : BaseNotify, IOcphCollection<subkriteria>
    {
        public event delOnSelected OnSelected;
        public SubKriteriaCollection(kriteria item)
        {
            this.Kriteria = item;
            if (list == null)
                list = GetItems();

        }

        private List<subkriteria> GetItems()
        {
            using(var db = new OcphDbContext())
            {
                var result= db.SubKriterias.Where(O => O.KriteriaId == Kriteria.Id).ToList();
                if (result == null)
                    result = new List<subkriteria>();
                return result;
            }
        }

        private List<subkriteria> list;
        private subkriteria _selected;

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public subkriteria SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
                OnSelected?.Invoke(value);
            }
        }



        public kriteria Kriteria { get; }

        public void Add(subkriteria item)
        {
            using (var db = new OcphDbContext())
            {
                item.Id = db.SubKriterias.InsertAndGetLastID(item);
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

        public bool Contains(subkriteria item)
        {
            return list.Contains(item);
        }

        public void CopyTo(subkriteria[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(subkriteria item)
        {
            try
            {
                var selected = list.Where(O => O.Id == item.Id).FirstOrDefault();
                if (selected != null)
                {
                    using (var db = new OcphDbContext())
                    {
                        if (db.SubKriterias.Delete(O => O.Id == item.Id))
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

        public IEnumerator<subkriteria> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Update(subkriteria t)
        {
            if (SelectedItem != null)
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        var saved= db.SubKriterias.Update(O => new { O.Kode, O.Nama}, t, O => O.Id == t.Id);
                        if(saved)
                        {
                            SelectedItem.Kode = t.Kode;
                            SelectedItem.Nama = t.Nama;
                            
                        }
                        return saved;
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

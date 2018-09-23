using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MainApp.Models
{
    public class BobotItem : TextBox, INotifyPropertyChanged
    {
        private double nilai;
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public double Nilai
        {
            get { return nilai; }
            set
            {
                SetProperty(ref nilai, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged



        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AHPLib.Models;
using MainApp.Domains;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for BobotCriteriaView.xaml
    /// </summary>
    public partial class BobotCriteriaView : Window
    {
        public BobotCriteriaView()
        {
            InitializeComponent();
            DataContext = this;
            KriteriaData = new KriteriaDataCollection();
            Criterias = Helper.GetMainViewModel().Kriterias;
            Loaded += BobotCriteriaView_Loaded;
        }

        private void BobotCriteriaView_Loaded(object sender, RoutedEventArgs e)
        {
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new Label() {Width= width });
          foreach(var item in Criterias)
            {
                headers.Children.Add(new Label() {Width= width, Content= item.Nama });
            }

            main.Children.Add(headers);
            foreach (var item in Criterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new Label { Width= width, Content = item.Nama });
                foreach (var item1 in Criterias)
                {

                    var dbData = KriteriaData.Where(O => O.KriteriaId == item.Id && O.PasanganId == item1.Id).FirstOrDefault();
                    var data = new BobotItem() { Width = width, Row = item.Id, Column = item1.Id };
                    if (dbData!=null)
                    {
                        data.Nilai = dbData.Nilai;
                    }

                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("Nilai");
                    binding.Mode = BindingMode.TwoWay;
                    binding.StringFormat = "N4";
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Source = data;  // view model?
                   
                    BindingOperations.SetBinding(data,TextBox.TextProperty, binding);
                    if (item.Id == item1.Id)
                    {
                        data.Nilai = 1.00;
                    }
                   

                    rows.Children.Add(data);
                }
                main.Children.Add(rows);
            }

           
           
        }

        public KriteriaDataCollection KriteriaData { get; }
        public KriteriaCollection Criterias { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var project = new AHPLib.BaseProject();
                var bobots = new List<MBobot>();
                foreach (var item in Criterias)
                {
                    var k = new MCriteria(item.Id, item.Kode, item.Nama);
                    project.AddSubCriteria(k);
                    foreach (WrapPanel wrp in main.Children)
                    {
                        var results = wrp.Children.OfType<BobotItem>().Where(O => O.Row == item.Id);
                        foreach (var ctrl in results)
                        {
                            var bobot = new MBobot() { Row = ctrl.Row - 1, Column = ctrl.Column - 1, BobotType = BobotType.Criteria, ParentId = ctrl.Row, Value = ctrl.Nilai };
                            bobots.Add(bobot);

                        }
                    }
                }


                foreach(var item in bobots)
                {
                    var data = KriteriaData.Where(O => O.KriteriaId == item.Row + 1 && O.PasanganId == item.Column + 1).FirstOrDefault();
                    if (data != null)
                        data.Nilai = item.Value;
                    else
                        KriteriaData.Add(new Models.DTO.ahpkriteria { KriteriaId = item.Row + 1, PasanganId = item.Column + 1, Nilai = item.Value });
                }

                if ( KriteriaData.Update(new Models.DTO.ahpkriteria()))
                {
                    project.SetBobot(bobots);
                }
            }
            catch (Exception ex)
            {

                Helper.Error(ex.Message);
            }




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


    public class BobotItem:TextBox,INotifyPropertyChanged
    {
        private double nilai;

        public int Row { get; set; }
        public int Column { get; set; }
        public double Nilai {
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

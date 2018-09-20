using AHPLib.Models;
using MainApp.Domains;
using MainApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for BobotSubCriteriaView.xaml
    /// </summary>
    public partial class BobotSubCriteriaView : Window
    {
        public BobotSubCriteriaView()
        {
            InitializeComponent();
            DataContext = this;
           
            Kriterias = Helper.GetMainViewModel().Kriterias;
            Kriterias.OnSelected += Criterias_OnSelected;
     
        }

        private void Criterias_OnSelected(object obj)
        {
            var item = obj as kriteria;
            if(item!=null)
            {
                Datas = new Domains.SubKriteriaDataCollection(item);
                main.Children.Clear();
                LoadedData();

            }
        }

        private void LoadedData()
        {
            var headers = new WrapPanel();
            headers.Children.Add(new Label() { Width = 100 });
            foreach (var item in Kriterias.SelectedItem.SubKriterias)
            {
                headers.Children.Add(new Label() { Width = 100, Content = item.Nama });
            }

            main.Children.Add(headers);
            foreach (var item in Kriterias.SelectedItem.SubKriterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new Label { Width = 100, Content = item.Nama });
                foreach (var item1 in Kriterias.SelectedItem.SubKriterias)
                {

                    var dbData = Datas.Where(O => O.SubKriteriaId == item.Id && O.PasanganId == item1.Id).FirstOrDefault();
                    var data = new BobotItem() { Width = 100, Row = item.Id, Column = item1.Id };
                    if (dbData != null)
                    {
                        data.Nilai = dbData.Nilai;
                    }

                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("Nilai");
                    binding.Mode = BindingMode.TwoWay;
                    binding.StringFormat = "N4";
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Source = data;  // view model?

                    BindingOperations.SetBinding(data, TextBox.TextProperty, binding);
                    if (item.Id == item1.Id)
                    {
                        data.Nilai = 1.00;
                    }


                    rows.Children.Add(data);
                }
                main.Children.Add(rows);
            }



        }

   
        public KriteriaCollection Kriterias { get; }
        public SubKriteriaDataCollection Datas { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var project = new AHPLib.BaseProject();
                var bobots = new List<MBobot>();
                foreach (var item in Kriterias.SelectedItem.SubKriterias)
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


                foreach (var item in bobots)
                {
                    var data = Datas.Where(O => O.SubKriteriaId == item.Row + 1 && O.PasanganId == item.Column + 1).FirstOrDefault();
                    if (data != null)
                        data.Nilai = item.Value;
                    else
                        Datas.Add(new ahpsub { SubKriteriaId = item.Row + 1, PasanganId = item.Column + 1, Nilai = item.Value });
                }

                if (Datas.Update(new ahpsub()))
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
}

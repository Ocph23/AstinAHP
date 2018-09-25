using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AHPLib;
using AHPLib.Models;
using MainApp.Domains;
using MainApp.Models;

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
            headers.Children.Add(new TextBox() {Width= width });
          foreach(var item in Criterias)
            {
                headers.Children.Add(new TextBox() {Width= width, Text= item.Nama });
            }

            main.Children.Add(headers);
            int i = 0;
            foreach (var item in Criterias)
            {
                int j = 0;
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width= width, Text= item.Nama });
                foreach (var item1 in Criterias)
                {

                    var dbData = KriteriaData.Where(O => O.KriteriaId == item.Id && O.PasanganId == item1.Id).FirstOrDefault();
                    var data = new BobotItem() { RowId=item.Id, ColumnId=item1.Id, Width = width, Row = i, Column = j};
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
                    j++;
                }
                main.Children.Add(rows);
                i++;
            }
         
           
           
        }

        public KriteriaDataCollection KriteriaData { get; }
        public KriteriaCollection Criterias { get; }
        public BaseProject Project { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Project = new AHPLib.BaseProject();
                var bobots = new List<MBobot>();
                foreach (var item in Criterias)
                {
                    var k = new MCriteria(item.Id, item.Kode, item.Nama);
                    Project.AddSubCriteria(k);
                    foreach (WrapPanel wrp in main.Children)
                    {
                        var results = wrp.Children.OfType<BobotItem>().Where(O => O.RowId == item.Id);
                        foreach (var ctrl in results)
                        {
                            var bobot = new MBobot() { Row = ctrl.Row, Column = ctrl.Column, BobotType = BobotType.Criteria,  RowId=ctrl.RowId, ColumnId=ctrl.ColumnId, ParentId = 0, Value = ctrl.Nilai };
                            bobots.Add(bobot);

                        }
                    }
                }


                foreach(var item in bobots)
                {
                    var data = KriteriaData.Where(O => O.KriteriaId == item.RowId && O.PasanganId == item.ColumnId).FirstOrDefault();
                    if (data != null)
                        data.Nilai = item.Value;
                    else
                        KriteriaData.Add(new Models.DTO.ahpkriteria { KriteriaId = item.RowId, PasanganId = item.ColumnId, Nilai = item.Value });
                }

                if ( KriteriaData.Update(new Models.DTO.ahpkriteria()))
                {
                    Project.SetBobot(bobots);
                }
            }
            catch (Exception ex)
            {

                Helper.Error(ex.Message);
            }


            Project.Calculate(0);

            SetTableNormalisasi(Project);
            SetTableKonsistency(Project);
            SetTableKonsistencyMatrix(Project);
            ci.Text = Project.ConsistencyIndex.ToString();
            cr.Text = Project.ConsistencyRatio.ToString();

        }

        private void SetTableKonsistencyMatrix(BaseProject project)
        {
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            headers.Children.Add(new TextBox() { Width = width, Text = "Jumlah" });
            headers.Children.Add(new TextBox() { Width = width, Text = "Prioritas" });
            headers.Children.Add(new TextBox() { Width = width, Text = "Hasil" });

            matrix.Children.Add(headers);
            int i = 0;
            foreach (var item in Criterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width = width, Text = item.Nama });
                var prio1 = new BobotItem() { Width = width, Nilai = Project.SumOfConsistensyTable[i] };
                var prio2 = new BobotItem() { Width = width, Nilai = Project.PriorityVector[i] };
                var prio3 = new BobotItem() { Width = width, Nilai = Project.Lamdas[i] };

                SetBindingData(prio1);
                rows.Children.Add(prio1);
                SetBindingData(prio2);
                rows.Children.Add(prio2);
                SetBindingData(prio3);
                rows.Children.Add(prio3);

                i++;
                matrix.Children.Add(rows);
            }
        }

        private void SetTableKonsistency(BaseProject project)
        {
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            foreach (var item in Criterias)
            {
                headers.Children.Add(new TextBox() { Width = width, Text = item.Nama });
            }
            headers.Children.Add(new TextBox() { Width = width, Text = "Jumlah" });

            tblConsistency.Children.Add(headers);
            int i = 0;
            foreach (var item in Criterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width = width, Text = item.Nama });
                for (int j = 0; j < Criterias.Count; j++)
                {
                    var data = new BobotItem() { Width = width, Row = i, Column = j };
                    data.Nilai = project.KonsistensiTable[i, j];
                    SetBindingData(data);

                    rows.Children.Add(data);
                }
                var prio = new BobotItem() { Width = width, Nilai = Project.SumOfConsistensyTable[i] };
                SetBindingData(prio);
                rows.Children.Add(prio);

                i++;
                tblConsistency.Children.Add(rows);
            }
        }

        private void SetTableNormalisasi(BaseProject project)
        {
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            foreach (var item in Criterias)
            {
                headers.Children.Add(new TextBox() { Width = width, Text = item.Nama });
            }
            headers.Children.Add(new TextBox() { Width = width, Text = "Priority" });

            normalisasi.Children.Add(headers);
            int i = 0;
            foreach (var item in Criterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width = width, Text = item.Nama });
                for (int j = 0; j < Criterias.Count; j++)
                {
                    var data = new BobotItem() { Width = width, Row = i, Column = j };
                    data.Nilai =project.TableNormalisasi[i, j];
                    SetBindingData(data);

                    rows.Children.Add(data);
                }
                var prio = new BobotItem() { Width = width, Nilai=Project.PriorityVector[i] };
                SetBindingData(prio);
                rows.Children.Add(prio);

                i++;
                normalisasi.Children.Add(rows);
            }

        }

        private void SetBindingData(BobotItem data)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath("Nilai");
            binding.Mode = BindingMode.TwoWay;
            binding.StringFormat = "N4";
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.Source = data;  // view model?

            BindingOperations.SetBinding(data, TextBox.TextProperty, binding);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


   
}

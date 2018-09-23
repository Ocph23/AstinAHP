using AHPLib.Models;
using MainApp.Domains;
using MainApp.Models;
using MainApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            Kriterias.SelectedItem = null;
            Kriterias.OnSelected += Criterias_OnSelected;
            AppProject = Helper.GetMainViewModel().Project;
            AppProject.Calculate(0);
     
        }

        private void Criterias_OnSelected(object obj)
        {
            var item = obj as kriteria;
            if(item!=null)
            {
                Datas = new Domains.SubKriteriaDataCollection(item.Id);
                main.Children.Clear();
                normalisasi.Children.Clear();
                tblConsistency.Children.Clear();
                matrix.Children.Clear();
                SelectedProject = AppProject.SubCriteria.Where(O => O.Id == item.Id).FirstOrDefault();
                pv.Content = SelectedProject.Value.ToString();
                LoadedData();
            }
        }

        private void LoadedData()
        {
            
            var Criterias = Kriterias.SelectedItem.SubKriterias;
            if(Criterias.Count>0)
            {
                int width = 75;
                var headers = new WrapPanel();
                headers.Children.Add(new TextBox() { Width = width });
                foreach (var item in Criterias)
                {
                    headers.Children.Add(new TextBox() { Width = width, Text = item.Nama });
                }

                main.Children.Add(headers);
                int i = 0;
                foreach (var item in Criterias)
                {
                    var rows = new WrapPanel();
                    rows.Children.Add(new TextBox { Width = width, Text = item.Nama });

                    var j = 0;
                    foreach (var item1 in Criterias)
                    {

                        var dbData = Datas.Where(O => O.SubKriteriaId == item.Id && O.PasanganId == item1.Id).FirstOrDefault();
                        var data = new BobotItem() { RowId = item.Id, ColumnId = item1.Id, Width = width, Row = i, Column = j };
                        if (dbData != null)
                        {
                            data.Nilai = dbData.Nilai;
                        }
                        SetBindingData(data);
                        if (item.Id == item1.Id)
                        {
                            data.Nilai = 1.00;
                        }

                        j++;
                        rows.Children.Add(data);
                    }
                    i++;
                    main.Children.Add(rows);
                }

            }


        }


        public KriteriaCollection Kriterias { get; }
        public SubKriteriaDataCollection Datas { get; private set; }
        public MCriteria Project { get; private set; }
        public MainProject AppProject { get; }
        public MCriteria SelectedProject { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                normalisasi.Children.Clear();
                tblConsistency.Children.Clear();
                matrix.Children.Clear();

                var criteriaSelected = Kriterias.SelectedItem;
                Project = new MCriteria(criteriaSelected.Id, criteriaSelected.Kode, criteriaSelected.Nama);
           
                var bobots = new List<MBobot>();
                foreach (var item in criteriaSelected.SubKriterias)
                {
                    var k = new MCriteria(item.Id, item.Kode, item.Nama);
                    Project.AddSubCriteria(k);
                    foreach (WrapPanel wrp in main.Children)
                    {
                        var results = wrp.Children.OfType<BobotItem>().Where(O => O.RowId == item.Id);
                        foreach (var ctrl in results)
                        {
                            var bobot = new MBobot() { RowId=ctrl.RowId,ColumnId=ctrl.ColumnId, Row = ctrl.Row, Column = ctrl.Column, BobotType = BobotType.Criteria, ParentId = ctrl.Row, Value = ctrl.Nilai };
                            bobots.Add(bobot);

                        }
                    }
                }


                foreach (var item in bobots)
                {
                    var data = Datas.Where(O => O.SubKriteriaId == item.RowId && O.PasanganId == item.ColumnId).FirstOrDefault();
                    if (data != null)
                        data.Nilai = item.Value;
                    else
                        Datas.Add(new Models.DTO.ahpsub { SubKriteriaId = item.RowId,  PasanganId = item.ColumnId, Nilai = item.Value });
                }

                if (Datas.Update(new Models.DTO.ahpsub()))
                {
                    Project.SetBobot(bobots);
                }
            }
            catch (Exception ex)
            {

                Helper.Error(ex.Message);
            }

   
            Project.Calculate(SelectedProject.Value);

            SetTableNormalisasi(Project);
            SetTableKonsistency(Project);
            SetTableKonsistencyMatrix(Project);
            ci.Text = Project.ConsistencyIndex.ToString();
            cr.Text = Project.ConsistencyRatio.ToString();



        }

        private void SetTableKonsistencyMatrix(MCriteria project)
        {
            var criteriaSelected = Kriterias.SelectedItem;
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            headers.Children.Add(new TextBox() { Width = width, Text = "Jumlah" });
            headers.Children.Add(new TextBox() { Width = width, Text = "Prioritas" });
            headers.Children.Add(new TextBox() { Width = width, Text = "Hasil" });

            matrix.Children.Add(headers);
            int i = 0;
            foreach (var item in criteriaSelected.SubKriterias)
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

        private void SetTableKonsistency(MCriteria project)
        {
            var criteriaSelected = Kriterias.SelectedItem;
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            foreach (var item in criteriaSelected.SubKriterias)
            {
                headers.Children.Add(new TextBox() { Width = width, Text = item.Nama });
            }
            headers.Children.Add(new TextBox() { Width = width, Text = "Jumlah" });

            tblConsistency.Children.Add(headers);
            int i = 0;
            foreach (var item in criteriaSelected.SubKriterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width = width, Text = item.Nama });
                for (int j = 0; j < criteriaSelected.SubKriterias.Count; j++)
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

        private void SetTableNormalisasi(MCriteria project)
        {
            var criteriaSelected = Kriterias.SelectedItem;
            int width = 75;
            var headers = new WrapPanel();
            headers.Children.Add(new TextBox() { Width = width });
            foreach (var item in criteriaSelected.SubKriterias)
            {
                headers.Children.Add(new TextBox() { Width = width, Text = item.Nama });
            }
            headers.Children.Add(new TextBox() { Width = width, Text = "Priority" });
            headers.Children.Add(new TextBox() { Width = width, Text = "Sub Priority" });

            normalisasi.Children.Add(headers);
            int i = 0;
            foreach (var item in criteriaSelected.SubKriterias)
            {
                var rows = new WrapPanel();
                rows.Children.Add(new TextBox { Width = width, Text = item.Nama });
                for (int j = 0; j < criteriaSelected.SubKriterias.Count; j++)
                {
                    var data = new BobotItem() { Width = width, Row = i, Column = j };
                    data.Nilai = project.TableNormalisasi[i, j];
                    SetBindingData(data);

                    rows.Children.Add(data);
                }
                var prio = new BobotItem() { Width = width, Nilai = Project.PriorityVector[i] };
                SetBindingData(prio);
                rows.Children.Add(prio);


                var prio2 = new BobotItem() { Width = width, Nilai = Project.PriorityVector[i]*SelectedProject.Value };
                SetBindingData(prio2);
                rows.Children.Add(prio2);

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

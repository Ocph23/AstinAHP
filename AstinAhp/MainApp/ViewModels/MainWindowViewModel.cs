using System;
using System.Threading.Tasks;
using AHPLib;
using MainApp.Domains;

namespace MainApp.ViewModels
{
    public class MainWindowViewModel
    {

        public MainWindowViewModel()
        {
            Pemohons = new PemohonCollection();
            Kriterias = new KriteriaCollection();
            Project = new MainProject();
            LoadAsync();
        }

        private async void LoadAsync()
        {
            await Task.Delay(3000);
            Project.Initialitation();
        }

        public PemohonCollection Pemohons { get; }
        public KriteriaCollection Kriterias { get; }
        public MainProject Project { get; }
    }
}
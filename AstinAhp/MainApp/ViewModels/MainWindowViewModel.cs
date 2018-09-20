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
        }

        public PemohonCollection Pemohons { get; }
        public KriteriaCollection Kriterias { get; }
        public MainProject Project { get; }
    }
}
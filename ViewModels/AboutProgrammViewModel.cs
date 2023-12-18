using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AboutProgrammViewModel : NotifyPropertyChangedBase
    {
        public string ApplicationVersion
        {
            get => appVersion;
            set
            {
                appVersion = value;
                OnPropertyChanged(nameof(ApplicationVersion));
            }
        }

        public ICommand CloseCommand => new RelayCommand((object? obj) => view.Close());

        private string appVersion;
        private readonly AboutProgramView view;

        public AboutProgrammViewModel()
        {
            ApplicationVersion = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion") ?? "0.0.0.0";
        }

        public AboutProgrammViewModel(AboutProgramView parent) : this()
        {
            view = parent;
        }
    }
}

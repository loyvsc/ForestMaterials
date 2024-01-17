using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AboutProgrammViewModel : ViewModelBase
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

        public ICommand CloseCommand => new AsyncRelayCommand(AsyncClose);

        private string appVersion;
        private readonly AboutProgramView view;

        private async Task AsyncClose(object? obj)
        {
            view.Close();
        }

        public AboutProgrammViewModel(AboutProgramView parent)
        {
            Title = "О программе";
            view = parent;
            ApplicationVersion = "Версия: " + Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion") ?? "0.0.0.0";
        }
    }
}

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

        public string Text { get; }

        private async Task AsyncClose(object? obj) => view.Close();

        public AboutProgrammViewModel(AboutProgramView parent)
        {
            Title = "О программе";
            view = parent;
            ApplicationVersion = "Версия: " + Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion") ?? "0.0.0.0";

            Text = "Приложение разработано для сотрудников ГЛХУ Молодечненский лесхоз\r\n" +
                "Для разработки было использованы:\r\n• .NET 7\r\n• Windows Presentation Foundation (WPF)" +
                "\r\n• СУБД MySQL\r\n• Библиотека MySQL.Data (подключение к СУБД)\r\n"+ApplicationVersion;
        }
    }
}

using BuildMaterials.Extensions;
using BuildMaterials.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialResponseViewModel : NotifyPropertyChangedBase
    {
        private readonly Window _window;
        public MaterialResponse MaterialResponse { get; set; }
        public Material? SelectedMaterial { get; set; }
        public Employee? SelectedFinResponEmployee
        {
            get => finrespempl;
            set
            {
                finrespempl = value;
                OnPropertyChanged(nameof(SelectedFinResponEmployee));
            }
        }
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<Employee> FinResponsibleEmployees => App.DbContext.Employees.Select("SELECT * FROM Employees WHERE FinResponsible = 1");

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        private Employee? finrespempl;

        public AddMaterialResponseViewModel()
        {
            MaterialResponse = new MaterialResponse();
        }

        public AddMaterialResponseViewModel(Window window) : this()
        {
            _window = window;
        }

        private async Task AddMaterial(object? obj)
        {
            if (SelectedMaterial == null || SelectedFinResponEmployee == null)
            {
                _window.ShowDialogAsync("Введите всю требуемую информацию!", "");
                return;
            }
            else
            {
                MaterialResponse.MaterialID = SelectedMaterial.ID;
                MaterialResponse.FinResponseEmployeeID = SelectedFinResponEmployee.ID;
            }
            if (MaterialResponse.IsValid)
            {
                App.DbContext.MaterialResponse.Add(MaterialResponse);
                System.Windows.MessageBox.Show("Материально-ответственный отчет успешно добавлен!", "Материально-ответственный отчет", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Добавление материально-ответственного отчета завершено с ошибкой!\nПопробуйте позже...", "Материально-ответственный отчет", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}

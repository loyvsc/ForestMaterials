using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialViewModel : NotifyPropertyChangedBase
    {
        public Models.Material Material { get; set; }

        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        private readonly Window _window = null!;
        private List<string> cul;

        public List<string> CountUnits
        {
            get => cul;
            set
            {
                cul = value;
                OnPropertyChanged();
            }
        }

        public AddMaterialViewModel()
        {
            Material = new Models.Material();
        }

        public AddMaterialViewModel(Window window, Models.Material material)
        {
            Material = material;
            _window = window;
            CountUnits = new List<string>()
        {
            "Кубический метр", "Килограм"
        };
        }

        public AddMaterialViewModel(Window window) : this()
        {
            _window = window;
            CountUnits = new List<string>()
        {
            "Кубический метр", "Килограм"
        };
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial(object? obj)
        {
            if (Material.ID != 0)
            {
                try
                {
                    App.DbContext.Materials.Update(Material);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, "Редактирование материала", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Close();
            }
            else
            {
                if (Material.IsValid())
                {
                    Material.EnterDate = DateTime.Now.Date;
                    App.DbContext.Materials.Add(Material);
                    Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Введена не вся требуемая информация!", "Добавление материала", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
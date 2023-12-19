using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialViewModel
    {
        public Models.Material Material { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;

        public AddMaterialViewModel()
        {
            Material = new Models.Material();
        }

        public AddMaterialViewModel(Window window, Models.Material material)
        {
            Material = material;
            _window = window;
        }

        public AddMaterialViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Material.IsValid)
            {
                Material.Count = Material.Count;
                Material.EnterDate = DateTime.Now.Date;
                App.DbContext.Materials.Add(Material);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый материал", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
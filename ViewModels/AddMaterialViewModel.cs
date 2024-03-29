﻿using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BuildMaterials.ViewModels
{
    public class AddMaterialViewModel : ViewModelBase
    {
        public Models.Material Material { get; set; }

        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        private readonly AddMaterialView _window = null!;
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

        public string? CountUnit
        {
            get => Material.CountUnits;
            set
            {
                if (value != null)
                {
                    _window.text.Visibility = Visibility.Collapsed;
                    Material.CountUnits = value;
                    OnPropertyChanged();
                }
            }
        }

        private AddMaterialViewModel()
        {
            CountUnits = new List<string>() { "Кубический метр", "Килограмм" };
        }

        public AddMaterialViewModel(AddMaterialView window, Models.Material material) : this()
        {
            Title = "Изменение лесопродукции";
            Material = material;
            _window = window;
            _window.text.Visibility = Visibility.Collapsed;
        }

        public AddMaterialViewModel(AddMaterialView window) : this()
        {
            Title = "Добавление лесопродукции";
            Material = new Models.Material();
            _window = window;
        }

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        private async Task AddMaterial(object? obj)
        {
            if (Material.IsValid() == false)
            {
                _window.ShowDialogAsync("Не вся информация была введена!", Title);
            }
            else
            {
                try
                {
                    if (Material.ID != 0)
                    {
                        App.DbContext.Materials.Update(Material);
                    }
                    else
                    {
                        Material.EnterDate = DateTime.Now.Date;
                        App.DbContext.Materials.Add(Material);
                    }
                    Close();
                }
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
                }
            }            
        }
    }
}
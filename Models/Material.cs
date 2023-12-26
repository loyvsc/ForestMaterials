﻿using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Material : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }

        [ExportColumnName("Наименование")]
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Производитель")]
        public string? Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Цена")]
        public float Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Количество")]
        public float Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Ед. измерения")]
        public string? CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Дата поставки")]
        public DateTime EnterDate
        {
            get => enterDate;
            set
            {
                enterDate = value;
                OnPropertyChanged();
            }
        }

        private string? name;
        private string? manufacturer;
        private float price;
        private float count;
        private string? countUnits;
        private DateTime enterDate;

        public Material()
        {
            Name = string.Empty;
            Manufacturer = string.Empty;
            CountUnits = string.Empty;
            Price = 0;
            Count = 0;
            ID = 0;
        }

        public Material(int id)
        {
            ID = id;
        }

        public Material(int id, string name, string manufacturer, float price, float count, string countUnits, DateTime enterDate)
        {
            ID = id;
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Count = count;
            CountUnits = countUnits;
            EnterDate = enterDate;
        }

        [IgnoreProperty]
        public bool IsValid() => Name != string.Empty &&
            Manufacturer != string.Empty &&
            CountUnits != string.Empty;

        public override string ToString() => Name!;
    }
}
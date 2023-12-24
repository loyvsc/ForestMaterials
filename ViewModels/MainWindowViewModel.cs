using BuildMaterials.BD;
using BuildMaterials.Models;
using BuildMaterials.Views;
using SpreadsheetLight;
using System.ComponentModel;
using BuildMaterials.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace BuildMaterials.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Lists
        public bool CanUserEditEmployeeConf => CurrentEmployee?.AccessLevel == 3;
        public List<Material> MaterialsList
        {
            get => materials;
            set
            {
                materials = value;
                OnPropertyChanged(nameof(MaterialsList));
            }
        }
        public List<Employee> EmployeesList
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged(nameof(EmployeesList));
            }
        }
        public List<Trade> TradesList
        {
            get => trades;
            set
            {
                trades = value;
                OnPropertyChanged(nameof(TradesList));
            }
        }
        public List<TTN> TTNList
        {
            get => ttns;
            set
            {
                ttns = value;
                OnPropertyChanged(nameof(TTNList));
            }
        }
        public List<Account> AccountsList
        {
            get => accounts;
            set
            {
                accounts = value;
                OnPropertyChanged(nameof(AccountsList));
            }
        }
        public List<Contract> ContractsList
        {
            get => contracts;
            set
            {
                contracts = value;
                OnPropertyChanged(nameof(ContractsList));
            }
        }
        public List<MaterialResponse> MaterialResponsesList
        {
            get => materialResponses;
            set
            {
                materialResponses = value;
                OnPropertyChanged(nameof(MaterialResponsesList));
            }
        }
        public List<Organization> OrganizationsList
        {
            get => custlist;
            set
            {
                custlist = value;
                OnPropertyChanged(nameof(OrganizationsList));
            }
        }
        public List<PayType> PayTypesList
        {
            get => paytypeslist;
            set
            {
                paytypeslist = value;
                OnPropertyChanged(nameof(PayTypesList));
            }
        }
        public List<Individual> IndividualsList
        {
            get => invlst;
            set
            {
                invlst = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AboutProgrammCommand => new RelayCommand(OpenAboutProgram);
        public ICommand ExitCommand => new RelayCommand((sener) => System.Windows.Application.Current.MainWindow.Close());
        public ICommand SettingsCommand => new RelayCommand(OpenSettings);
        public ICommand AddRowCommand => new RelayCommand(AddRow);
        public ICommand EditRowCommand => new RelayCommand(EditRow);
        public ICommand DeleteRowCommand => new RelayCommand(DeleteRow);
        public ICommand ExportWithSaveCommand => new RelayCommand(ExportExcelWithSave);
        //public ICommand PrintCommand => new RelayCommand((sender) => PrintContract());
        #endregion

        public bool CanViewConfidentional => CurrentEmployee?.AccessLevel == 3;
        public string SearchText
        {
            get => _searchtext;
            set
            {
                value = value.Trim();
                if (value != _searchtext)
                {
                    _searchtext = value;
                    OnPropertyChanged(nameof(SearchText));
                    Search(_searchtext);
                }
            }
        }

        public Employee? CurrentEmployee
        {
            get => currentEmployee;
            set
            {
                currentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
                OnPropertyChanged(nameof(CanViewConfidentional));
            }
        }
        public Settings Settings { get; private set; }

        public Visibility IsConfidentionNotView => CurrentEmployee?.AccessLevel < 2 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPrintEnabled
        {
            get => isPrintEnabled;
            set
            {
                isPrintEnabled = value;
                OnPropertyChanged(nameof(IsPrintEnabled));
            }
        }
        public ITable? SelectedTableItem { get; set; }

        public string SelectedTabAsString => selectedTab;

        #region Private vars
        private List<Individual> invlst;
        private Visibility isPrintEnabled;
        private List<PayType> paytypeslist;
        private string selectedTab = string.Empty;
        private List<Organization> provlist;
        private List<Organization> custlist;
        private string _searchtext = string.Empty;
        private Employee? currentEmployee;
        private List<Material> materials = null!;
        private List<Employee> employees = null!;
        private List<Trade> trades = null!;
        private List<TTN> ttns = null!;
        private List<Account> accounts = null!;
        private List<Contract> contracts = null!;
        private List<MaterialResponse> materialResponses = null!;
        private readonly MainWindow view;
        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            CurrentEmployee = new Employee();
            OrganizationsList = App.DbContext.Organizations.ToList();
            MaterialsList = App.DbContext.Materials.ToList();
            EmployeesList = App.DbContext.Employees.ToList();
            TradesList = App.DbContext.Trades.ToList();
            TTNList = App.DbContext.TTNs.ToList();
            AccountsList = App.DbContext.Accounts.ToList();
            ContractsList = App.DbContext.Contracts.ToList();
            PayTypesList = App.DbContext.PayTypes.ToList();
            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
            IndividualsList = App.DbContext.Individuals.ToList();

            Settings = new Settings();
        }

        public MainWindowViewModel(MainWindow view) : this()
        {
            this.view = view;
        }

        public MainWindowViewModel(MainWindow view, Employee employee) : this(view)
        {
            App.Current.Resources["IsConfView"] = CurrentEmployee?.AccessLevel > 1;
            App.Current.Resources["IsConfAddEdit"] = CurrentEmployee?.AccessLevel == 3;
            CurrentEmployee = employee;
        }
        #endregion

        #region ApplicationFunctions
        private void OpenAboutProgram(object? obj)
        {
            AboutProgramView aboutWindow = new AboutProgramView();
            aboutWindow.ShowDialog();
        }
        private void OpenSettings(object? obj)
        {
            SettingsView settingsWindow = new SettingsView();
            settingsWindow.ShowDialog();
        }
        public void ExitFromProgramm(CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Выйти из программы?", "АРМ Менеджера Строительной Компании", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        #endregion

        public void OnTabChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is TabItem)
            {
                string tabName = (e.AddedItems[0] as TabItem)!.Name;
                selectedTab = tabName;
                SearchText = string.Empty;
                switch (selectedTab)
                {
                    case "materialResponsibleTab":
                        {
                            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                            break;
                        }
                    case "materialsTab":
                        {
                            MaterialsList = App.DbContext.Materials.ToList();
                            break;
                        }
                    case "employersTab":
                        {
                            EmployeesList = App.DbContext.Employees.ToList();
                            break;
                        }
                    case "orgTab":
                        {
                            OrganizationsList = App.DbContext.Organizations.Select("SELECT * FROM sellers");
                            break;
                        }
                    case "uchetTab":
                        {
                            TradesList = App.DbContext.Trades.ToList();
                            break;
                        }
                    case "ttnTab":
                        {
                            TTNList = App.DbContext.TTNs.ToList();
                            break;
                        }
                    case "accountTab":
                        {
                            AccountsList = App.DbContext.Accounts.ToList();
                            break;
                        }
                    case "contractTab":
                        {
                            ContractsList = App.DbContext.Contracts.ToList();
                            break;
                        }
                }
            }
        }

        #region Private methods
        private void DeleteRow(object? obj)
        {
            try
            {
                if (SelectedTableItem == null) return;
                switch (selectedTab)
                {
                    case "individualsTab":
                        {
                            if (IndividualsList.Count.Equals(0))
                            {
                                return;
                            }
                            Individual buf = (Individual)SelectedTableItem;
                            App.DbContext.Individuals.Remove(buf);
                            IndividualsList = App.DbContext.Individuals.ToList();
                            break;
                        }
                    case "materialResponsibleTab":
                        {
                            if (MaterialResponsesList.Count.Equals(0))
                            {
                                return;
                            }
                            MaterialResponse buf = (MaterialResponse)SelectedTableItem;
                            App.DbContext.MaterialResponse.Remove(buf);
                            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                            break;
                        }
                    case "materialsTab":
                        {
                            if (MaterialsList.Count.Equals(0))
                            {
                                return;
                            }
                            Material buf = (Material)SelectedTableItem;
                            App.DbContext.Materials.Remove(buf);
                            MaterialsList = App.DbContext.Materials.ToList();
                            break;
                        }
                    case "employersTab":
                        {
                            if (EmployeesList.Count.Equals(0))
                            {
                                return;
                            }
                            Employee buf = (Employee)SelectedTableItem;
                            if (CurrentEmployee == buf)
                            {
                                System.Windows.MessageBox.Show("Нельзя удалять пользователя под которым был выполнен вход!");
                                return;
                            }
                            App.DbContext.Employees.Remove(buf);
                            EmployeesList = App.DbContext.Employees.ToList();
                            break;
                        }
                    case "orgTab":
                        {
                            if (OrganizationsList.Count.Equals(0))
                            {
                                return;
                            }
                            Organization buf = (Organization)SelectedTableItem;
                            App.DbContext.Organizations.Remove(buf);
                            OrganizationsList = App.DbContext.Organizations.Select("SELECT * FROM sellers;");
                            break;
                        }
                    case "uchetTab":
                        {
                            if (TradesList.Count.Equals(0))
                            {
                                return;
                            }
                            Trade buf = (Trade)SelectedTableItem;
                            App.DbContext.Trades.Remove(buf);
                            TradesList = App.DbContext.Trades.ToList();
                            break;
                        }
                    case "ttnTab":
                        {
                            if (TTNList.Count.Equals(0))
                            {
                                return;
                            }
                            TTN buf = (TTN)SelectedTableItem;
                            App.DbContext.TTNs.Remove(buf);
                            TTNList = App.DbContext.TTNs.ToList();
                            break;
                        }
                    case "accountTab":
                        {
                            if (AccountsList.Count.Equals(0))
                            {
                                return;
                            }
                            Account buf = (Account)SelectedTableItem;
                            App.DbContext.Accounts.Remove(buf);
                            AccountsList = App.DbContext.Accounts.ToList();
                            break;
                        }
                    case "contractTab":
                        {
                            if (ContractsList.Count.Equals(0))
                            {
                                return;
                            }
                            Contract buf = (Contract)SelectedTableItem;
                            App.DbContext.Contracts.Remove(buf);
                            ContractsList = App.DbContext.Contracts.ToList();
                            break;
                        }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            finally
            {
                SelectedTableItem = null;
            }
        }
        private void AddRow(object? obj)
        {
            switch (selectedTab)
            {
                case "materialResponsibleTab":
                    {
                        AddMaterialResponseView addMaterial = new AddMaterialResponseView();
                        if (addMaterial.ShowDialog() == true)
                        {
                            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                        }
                        break;
                    }
                case "materialsTab":
                    {
                        AddMaterialView addMaterial = new AddMaterialView();
                        if (addMaterial.ShowDialog() == true)
                        {
                            MaterialsList = App.DbContext.Materials.ToList();
                        }
                        break;
                    }
                case "employersTab":
                    {
                        AddEmployeeView add = new AddEmployeeView();
                        if (add.ShowDialog() == true)
                        {
                            EmployeesList = App.DbContext.Employees.ToList();
                        }
                        break;
                    }
                case "individualsTab":
                    {
                        AddIndividualView add = new AddIndividualView();
                        if (add.ShowDialog() == true)
                        {
                            IndividualsList = App.DbContext.Individuals.ToList();
                        }
                        break;
                    }
                case "orgTab":
                    {
                        AddOrganizationView add = new AddOrganizationView();
                        if (add.ShowDialog() == true)
                        {
                            OrganizationsList = App.DbContext.Organizations.Select("SELECT * FROM sellers;");
                        }
                        break;
                    }
                case "uchetTab":
                    {
                        AddTradeView add = new AddTradeView();
                        if (add.ShowDialog() == true)
                        {
                            TradesList = App.DbContext.Trades.ToList();
                        }
                        break;
                    }
                case "ttnTab":
                    {
                        AddTTNView add = new AddTTNView();
                        if (add.ShowDialog() == true)
                        {
                            TTNList = App.DbContext.TTNs.ToList();
                        }
                        break;
                    }
                case "accountTab":
                    {
                        AddAccountView add = new AddAccountView();
                        if (add.ShowDialog() == true)
                        {
                            AccountsList = App.DbContext.Accounts.ToList();
                        }
                        break;
                    }
                case "contractTab":
                    {
                        AddContractView add = new AddContractView();
                        if (add.ShowDialog() == true)
                        {
                            ContractsList = App.DbContext.Contracts.ToList();
                        }
                        break;
                    }
            }
        }
        private void EditRow(object? obj)
        {
            if (SelectedTableItem != null)
            {
                int id = SelectedTableItem.ID;
                switch (selectedTab)
                {
                    case "individualsTab":
                        {
                            AddIndividualView add = new AddIndividualView(App.DbContext.Individuals.ElementAt(id));
                            if (add.ShowDialog() == true)
                            {
                                IndividualsList = App.DbContext.Individuals.ToList();
                            }
                            break;
                        }
                    case "materialsTab":
                        {
                            var window = new AddMaterialView(App.DbContext.Materials.ElementAt(id));
                            window.ShowDialog();
                            if (window != null)
                            {
                                MaterialsList = App.DbContext.Materials.ToList();
                            }
                            break;
                        }
                    case "employersTab":
                        {
                            var window = new AddEmployeeView(App.DbContext.Employees.ElementAt(id));
                            window.ShowDialog();
                            if (window != null)
                            {
                                EmployeesList = App.DbContext.Employees.ToList();
                            }
                            break;
                        }
                    case "orgTab":
                        {
                            var window = new AddOrganizationView(App.DbContext.Organizations.ElementAt(id));
                            window.ShowDialog();
                            if (window != null)
                            {
                                OrganizationsList = App.DbContext.Organizations.ToList();
                            }
                            break;
                        }
                    case "uchetTab":
                        {
                            var window = new AddTradeView(App.DbContext.Trades.ElementAt(id));
                            window.ShowDialog();
                            if (window != null)
                            {
                                TradesList = App.DbContext.Trades.ToList();
                            }
                            break;
                        }
                }
            }
        }
        private async void ExportExcelWithSave(object? obj)
        {
            SaveFileDialog savefile = new SaveFileDialog()
            {
                Filter = "Файлы Excel(*.xlsx)|*.xlsx|All files(*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = false,
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                await Task.Run(() => ExportToExcel(savefile.FileName));
            }
        }

        private void ExportToExcel(string filename)
        {
            switch (selectedTab)
            {
                case "materialsTab":
                    {
                        var materials = view.materialsDataGrid.CollectionViewSource.Cast<Material>().ToArray();
                        using (SLDocument document = new SLDocument())
                        {                       
                            string[] header = new string[7]
                            { "№","Наименование","Производитель","Цена","Количество","Ед. измерения","Дата поставки"};                            
                            for (int ind = 1; ind < 7; ind++)
                            {
                                document.SetCellValueNumeric(1, ind, header[ind - 1]);
                            }
                            int matCount = materials.Length;
                            for (int i = 0; i < matCount; i++)
                            {
                                Material material = materials[i];
                                document.SetCellValueNumeric(i + 2, 1, material.ID.ToString());
                                document.SetCellValueNumeric(i + 2, 2, material.Name);
                                document.SetCellValueNumeric(i + 2, 3, material.Manufacturer);
                                document.SetCellValueNumeric(i + 2, 4, material.Price.ToString());
                                document.SetCellValueNumeric(i + 2, 5, material.Count.ToString());
                                document.SetCellValueNumeric(i + 2, 6, material.CountUnits);
                                document.SetCellValueNumeric(i + 2, 7, material.EnterDate.ToMySQLDate());
                            }
                            try
                            {
                                document.SaveAs(filename);
                            }
                            catch (System.IO.IOException)
                            {
                                System.Windows.MessageBox.Show("Произошла ошибка при сохранении файла", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        break;
                    }
            }
        }
        private void Search(string text)
        {
            if (text.Equals(string.Empty))
            {
                switch (selectedTab)
                {
                    case "materialsTab":
                        {
                            MaterialsList = App.DbContext.Materials.ToList();
                            break;
                        }
                    case "employersTab":
                        {
                            EmployeesList = App.DbContext.Employees.ToList();
                            break;
                        }
                    case "orgTab":
                        {
                            OrganizationsList = App.DbContext.Organizations.ToList();
                            break;
                        }
                }
                return;
            }
            switch (selectedTab)
            {
                case "materialsTab":
                    {
                        MaterialsList = App.DbContext.Materials.Search(text);
                        break;
                    }
                case "employersTab":
                    {
                        EmployeesList = App.DbContext.Employees.Search(text);
                        break;
                    }
                case "orgTab":
                    {
                        OrganizationsList = App.DbContext.Organizations.Search(text);
                        break;
                    }
            }
        }

        #endregion
    }
}
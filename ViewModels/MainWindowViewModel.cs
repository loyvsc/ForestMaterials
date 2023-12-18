using BuildMaterials.BD;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        #endregion

        #region Commands
        public ICommand AboutProgrammCommand => new RelayCommand((sender) => OpenAboutProgram());
        public ICommand ExitCommand => new RelayCommand((sener) => System.Windows.Application.Current.MainWindow.Close());
        public ICommand SettingsCommand => new RelayCommand((sener) => OpenSettings());
        public ICommand AddRowCommand => new RelayCommand((sender) => AddRow());
        public ICommand DeleteRowCommand => new RelayCommand((sender) => DeleteRow());
        public ICommand PrintCommand => new RelayCommand((sender) => PrintContract());
        #endregion

        public bool CanViewConfidentional => CurrentEmployee?.AccessLevel == 3;
        public string SearchText
        {
            get => _searchtext;
            set
            {
                if (value.Trim() != _searchtext)
                {
                    _searchtext = value.Trim();
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
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            CurrentEmployee = new Employee();
            OrganizationsList = App.DbContext.Sellers.Select("SELECT * FROM sellers;");
            MaterialsList = App.DbContext.Materials.ToList();
            EmployeesList = App.DbContext.Employees.ToList();
            TradesList = App.DbContext.Trades.ToList();
            TTNList = App.DbContext.TTNs.ToList();
            AccountsList = App.DbContext.Accounts.ToList();
            ContractsList = App.DbContext.Contracts.ToList();
            PayTypesList = App.DbContext.PayTypes.ToList();
            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();

            IsPrintEnabled = Visibility.Collapsed;
            Settings = new Settings();
        }

        public MainWindowViewModel(Employee employee) : this()
        {
            App.Current.Resources["IsConfView"] = CurrentEmployee?.AccessLevel > 1;
            App.Current.Resources["IsConfAddEdit"] = CurrentEmployee?.AccessLevel == 3;
            CurrentEmployee = employee;
        }
        #endregion

        private void PrintContract()
        {
            if (SelectedTableItem == null)
            {
                return;
            }
        }

        #region ApplicationFunctions
        private void OpenAboutProgram()
        {
            AboutProgramView aboutWindow = new AboutProgramView();
            aboutWindow.ShowDialog();
        }
        private void OpenSettings()
        {
            SettingsView settingsWindow = new SettingsView();
            settingsWindow.ShowDialog();
        }
        public void ExitFromProgramm(CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Выйти из программы?", "АРМ Менеджера Строительной Компании", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                            OrganizationsList = App.DbContext.Sellers.Select("SELECT * FROM sellers");
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
        private void DeleteRow()
        {
            try
            {
                if (SelectedTableItem == null) return;
                switch (selectedTab)
                {
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
                            if(OrganizationsList.Count.Equals(0))
                            {
                                return;
                            }
                            Organization buf = (Organization)SelectedTableItem;
                            App.DbContext.Sellers.Remove(buf);
                            OrganizationsList = App.DbContext.Sellers.Select("SELECT * FROM sellers;");
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
        private void AddRow()
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
                case "orgTab":
                    {
                        AddOrganizationView add = new AddOrganizationView();
                        if (add.ShowDialog() == true)
                        {
                            OrganizationsList = App.DbContext.Sellers.Select("SELECT * FROM sellers;");
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
        public void Search(string text)
        {
            try
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
                                OrganizationsList = App.DbContext.Sellers.Select("SELECT * FROM sellers;");
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
                            OrganizationsList = App.DbContext.Sellers.Search(text);
                            break;
                        }
                }
            }
            catch (System.ArgumentNullException)
            {
                return;
            }
        }
    }
}
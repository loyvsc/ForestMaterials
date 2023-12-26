using BuildMaterials.BD;
using BuildMaterials.Export;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        #region Lists
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
        public ICommand AddRowCommand => new RelayCommand(AddRow);
        public ICommand EditRowCommand => new RelayCommand(EditRow);
        public ICommand DeleteRowCommand => new RelayCommand(DeleteRow);
        public ICommand ExportWithSaveCommand => new RelayCommand(ExportExcelWithSave); 
        public ICommand AddCopyRowCommand => new RelayCommand(AddRowCopy);
        #endregion        

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
            }
        }

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
        }

        public MainWindowViewModel(MainWindow view) : this()
        {
            this.view = view;
        }

        public MainWindowViewModel(MainWindow view, Employee employee) : this(view)
        {
            CurrentEmployee = employee;
        }
        #endregion

        #region ApplicationFunctions
        private void OpenAboutProgram(object? obj)
        {
            AboutProgramView aboutWindow = new AboutProgramView();
            aboutWindow.ShowDialog();
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
            if (CurrentEmployee.CanUserDelete == false)
            {
                System.Windows.MessageBox.Show("У Вас отсутствуют нужные права", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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
            if (CurrentEmployee.CanUserAdd == false)
            {
                System.Windows.MessageBox.Show("У Вас отсутствуют нужные права", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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
                            if (CurrentEmployee.IsUserAdmin)
                            {
                                System.Windows.MessageBox.Show("У Вас отсутствуют нужные права", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                return;
                            }
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
            if (CurrentEmployee.CanUserEdit == false)
            {
                System.Windows.MessageBox.Show("У Вас отсутствуют нужные права", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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
        private void ExportExcelWithSave(object? obj)
        {
            SaveFileDialog savefile = new SaveFileDialog()
            {
                Filter = "Файлы Excel(*.xlsx)|*.xlsx|All files(*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = false,
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FilterDataGrid.FilterDataGrid fdatagrid = null!;
                switch (selectedTab)
                {
                    case "materialsTab":
                        {
                            fdatagrid = view.materialsDataGrid;
                            break;
                        }
                    case "employersTab":
                        {
                            fdatagrid = view.employersDataGrid;
                            break;
                        }
                    case "orgTab":
                        {
                            fdatagrid = view.organizationsDataGrid;
                            break;
                        }
                    case "uchetTab":
                        {
                            fdatagrid = view.uchetDataGrid;
                            break;
                        }
                    case "ttnTab":
                        {
                            fdatagrid = view.ttnsDataGrid;
                            break;
                        }
                    case "accountTab":
                        {
                            fdatagrid = view.accountsDataGrid;
                            break;
                        }
                    case "contractTab":
                        {
                            fdatagrid = view.contractsDataGrid;
                            break;
                        }
                    case "materialResponsibleTab":
                        {
                            fdatagrid = view.materialresponsesDataGrid;
                            break;
                        }
                }

                var myClassType = fdatagrid.ItemsSource.GetType().GetGenericArguments().Single();

                var method = typeof(ExportToExcel).GetMethod("ExportFromDataGrid", BindingFlags.Static | BindingFlags.Public);
                var genericMethod = method.MakeGenericMethod(myClassType);

                try
                {
                    // вызов статического метода
                    genericMethod.Invoke(null, new object[2] { savefile.FileName, fdatagrid });
                }
                catch
                {
                    System.Windows.MessageBox.Show("Во время экспорта произошла ошибка...", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                System.Windows.MessageBox.Show("Экспорт успешно завершен", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void AddRowCopy(object? obj)
        {
            if (CurrentEmployee.CanUserAdd == false || (CurrentEmployee.IsUserAdmin && selectedTab == "employersTab") == false)
            {
                System.Windows.MessageBox.Show("У Вас отсутствуют нужные права", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            switch (selectedTab)
            {
                case "materialResponsibleTab":
                    {
                        var copy = SelectedTableItem as MaterialResponse;
                        copy.ID = 0;
                        AddMaterialResponseView addMaterial = new AddMaterialResponseView();
                        (addMaterial.DataContext as AddMaterialResponseViewModel).MaterialResponse = copy;
                        if (addMaterial.ShowDialog() == true)
                        {
                            MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                        }
                        break;
                    }
                case "materialsTab":
                    {
                        var copy = SelectedTableItem as Material;
                        copy.ID = 0;
                        AddMaterialView addMaterial = new AddMaterialView(copy);
                        if (addMaterial.ShowDialog() == true)
                        {
                            MaterialsList = App.DbContext.Materials.ToList();
                        }
                        break;
                    }
                case "employersTab":
                    {
                        var copy = SelectedTableItem as Employee;
                        copy.ID = 0;
                        AddEmployeeView add = new AddEmployeeView(copy);
                        if (add.ShowDialog() == true)
                        {
                            EmployeesList = App.DbContext.Employees.ToList();
                        }
                        break;
                    }
                case "individualsTab":
                    {
                        var copy = SelectedTableItem as Individual;
                        copy.ID = 0;
                        AddIndividualView add = new AddIndividualView(copy);
                        if (add.ShowDialog() == true)
                        {
                            IndividualsList = App.DbContext.Individuals.ToList();
                        }
                        break;
                    }
                case "orgTab":
                    {
                        var copy = SelectedTableItem as Organization;
                        copy.ID = 0;
                        AddOrganizationView add = new AddOrganizationView(copy);
                        if (add.ShowDialog() == true)
                        {
                            OrganizationsList = App.DbContext.Organizations.Select("SELECT * FROM sellers;");
                        }
                        break;
                    }
                case "uchetTab":
                    {
                        var copy = SelectedTableItem as Trade;
                        copy.ID = 0;
                        AddTradeView add = new AddTradeView(copy);
                        if (add.ShowDialog() == true)
                        {
                            TradesList = App.DbContext.Trades.ToList();
                        }
                        break;
                    }
                case "ttnTab":
                    {
                        var copy = SelectedTableItem as TTN;
                        copy.ID = 0;
                        AddTTNView add = new AddTTNView(copy);
                        if (add.ShowDialog() == true)
                        {
                            TTNList = App.DbContext.TTNs.ToList();
                        }
                        break;
                    }
                case "accountTab":
                    {
                        var copy = SelectedTableItem as Account;
                        copy.ID = 0;
                        AddAccountView add = new AddAccountView();
                        (add.DataContext as AddAccountViewModel).Account = copy;
                        if (add.ShowDialog() == true)
                        {
                            AccountsList = App.DbContext.Accounts.ToList();
                        }
                        break;
                    }
                case "contractTab":
                    {
                        var copy = SelectedTableItem as Contact;
                        copy.ID = 0;
                        AddContractView add = new AddContractView();
                        (add.DataContext as AddContactViewModel).Contact = copy;
                        if (add.ShowDialog() == true)
                        {
                            ContractsList = App.DbContext.Contracts.ToList();
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
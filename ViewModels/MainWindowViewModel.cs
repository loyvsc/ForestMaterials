using BuildMaterials.BD;
using BuildMaterials.Export;
using BuildMaterials.Export.Documents;
using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using Microsoft.Win32;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Lists
        public List<Automobile> AutomobilesList
        {
            get => automobs;
            set
            {
                automobs = value;
                OnPropertyChanged();
            }
        }
        public List<TN> TNsList
        {
            get => tns;
            set
            {
                tns = value;
                OnPropertyChanged();
            }
        }
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
        public ICommand AboutProgrammCommand => new AsyncRelayCommand(OpenAboutProgram);
        public ICommand ExitCommand => new AsyncRelayCommand(Close);
        public ICommand AddRowCommand => new AsyncRelayCommand(AddRow);
        public ICommand EditRowCommand => new AsyncRelayCommand(EditRow);
        public ICommand DeleteRowCommand => new AsyncRelayCommand(DeleteRow);
        public ICommand ExportWithSaveCommand => new AsyncRelayCommand(ExportExcelWithSave);
        public ICommand AddCopyRowCommand => new AsyncRelayCommand(AddRowCopy);
        public ICommand ExportCommand => new AsyncRelayCommand(ExportDocument);
        #endregion        

        public bool IsUpdating
        {
            get => isUpdating;
            set
            {
                isUpdating = value;
                OnPropertyChanged();
            }
        }
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
        public Visibility IsDocumentSelect
        {
            get => isdocsel;
            set
            {
                isdocsel = value;
                OnPropertyChanged();
            }
        }

        #region Private vars
        private bool isUpdating;
        private List<Automobile> automobs;
        private List<TN> tns;
        private Visibility isdocsel;
        private List<Individual> invlst;
        private Visibility isPrintEnabled;
        private List<PayType> paytypeslist;
        private string selectedTab = string.Empty;
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
            MaterialsList = App.DbContext.Materials.ToList();
            IsDocumentSelect = Visibility.Collapsed;
            Title = "ПС \"Учет товарооборота лесхоза\"";
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
        private async Task OpenAboutProgram(object? obj)
        {
            AboutProgramView aboutWindow = new AboutProgramView();
            aboutWindow.ShowDialog();
        }

        public async Task ExitFromProgramm(CancelEventArgs e)
        {
            var result = view.ShowDialogAsync("Выйти из программы?", Title, "Да", SymbolRegular.Desktop20);
            if ((result == Wpf.Ui.Controls.MessageBoxResult.Secondary) || (result == Wpf.Ui.Controls.MessageBoxResult.None))
            {
                e.Cancel = true;
            }
        }
        #endregion

        public async Task OnTabChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is TabItem)
            {
                IsUpdating = true;
                SelectedTableItem = null;
                string tabName = (e.AddedItems[0] as TabItem)!.Name;
                selectedTab = tabName;
                SearchText = string.Empty;
                IsDocumentSelect = Visibility.Collapsed;
                switch (selectedTab)
                {
                    case "automobilesTab":
                        {
                            new Task(() =>
                            {
                                AutomobilesList = App.DbContext.Automobiles.ToList();
                                CurrentDataGrid = view.automobilesDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "materialsTab":
                        {
                            new Task(() =>
                            {
                                MaterialsList = App.DbContext.Materials.ToList();
                                CurrentDataGrid = view.materialsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "employersTab":
                        {
                            new Task(() =>
                            {
                                EmployeesList = App.DbContext.Employees.ToList();
                                CurrentDataGrid = view.employersDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "orgTab":
                        {
                            new Task(() =>
                            {
                                OrganizationsList = App.DbContext.Organizations.ToList();
                                CurrentDataGrid = view.organizationsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "uchetTab":
                        {
                            new Task(() =>
                            {
                                TradesList = App.DbContext.Trades.ToList();
                                CurrentDataGrid = view.uchetDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "ttnTab":
                        {
                            new Task(() =>
                            {
                                IsDocumentSelect = Visibility.Visible;
                                CurrentDataGrid = view.ttnsDataGrid;
                                TTNList = App.DbContext.TTNs.ToList();
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "acountTab":
                        {
                            new Task(() =>
                            {
                                IsDocumentSelect = Visibility.Visible;
                                AccountsList = App.DbContext.Accounts.ToList();
                                CurrentDataGrid = view.accountsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "contractTab":
                        {
                            new Task(() =>
                            {
                                IsDocumentSelect = Visibility.Visible;
                                ContractsList = App.DbContext.Contracts.ToList();
                                CurrentDataGrid = view.contractsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "materialResponsibleTab":
                        {
                            new Task(() =>
                            {
                                IsDocumentSelect = Visibility.Visible;
                                MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                                CurrentDataGrid = view.materialsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "tnTab":
                        {
                            new Task(() =>
                            {
                                IsDocumentSelect = Visibility.Visible;
                                TNsList = App.DbContext.TNs.ToList();
                                CurrentDataGrid = view.tnsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                    case "individualsTab":
                        {
                            new Task(() =>
                            {
                                IndividualsList = App.DbContext.Individuals.ToList();
                                CurrentDataGrid = view.individualsDataGrid;
                                IsUpdating = false;
                            }).Start();
                            break;
                        }
                }
            }
        }

        #region Private methods
        private async Task Close(object? obj)
        {
            System.Windows.Application.Current.MainWindow.Close();
        }
        private async Task ExportDocument(object? obj)
        {
            if (SelectedTableItem == null)
            {
                view.ShowDialogAsync("Выбрите документ!", "Экспорт документа");
                return;
            }

            SaveFileDialog save = new SaveFileDialog()
            {
                Filter = "Файл Word(*.docx)|*.docx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = false,
            };
            if (save.ShowDialog() != true) return;

            string path = save.FileName;

            DocumentExport export = new DocumentExport(view);

            try
            {
                switch (selectedTab)
                {
                    case "tnTab":
                        {
                            export.SaveReport(path, SelectedTableItem!as TN);
                            break;
                        }
                    case "ttnTab":
                        {
                            export.SaveReport(path, SelectedTableItem as TTN);
                            break;
                        }
                    case "acountTab":
                        {
                            if ((SelectedTableItem as Account).Buyer.ID == 0)
                            {
                                view.ShowDialogAsync("Выбрите документ!", "Экспорт документа");
                            }
                            export.SaveReport(path, SelectedTableItem as Account);
                            break;
                        }
                    case "contractTab":
                        {
                            export.SaveReport(path, SelectedTableItem as Contract);
                            break;
                        }
                    case "materialResponsibleTab":
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message,Title);
            }
        }
        private async Task DeleteRow(object? obj)
        {
            if (CurrentEmployee.CanUserDelete == false)
            {
                view.ShowDialogAsync("У Вас отсутствуют нужные права!", Title);
                return;
            }
            try
            {
                if (SelectedTableItem == null) return;
                switch (selectedTab)
                {
                    case "automobilesTab":
                        {
                            if (AutomobilesList.Count.Equals(0))
                            {
                                return;
                            }
                            Automobile buf = (Automobile)SelectedTableItem;
                            App.DbContext.Automobiles.Remove(buf);
                            AutomobilesList = App.DbContext.Automobiles.ToList();
                            break;
                        }
                    case "tnTab":
                        {
                            if (TNsList.Count.Equals(0))
                            {
                                return;
                            }
                            TN buf = (TN)SelectedTableItem;
                            App.DbContext.TNs.Remove(buf);
                            TNsList = App.DbContext.TNs.ToList();
                            break;
                        }
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
                            if (EmployeesList.Count == 0) return;
                            Employee buf = (Employee)SelectedTableItem;
                            if (CurrentEmployee == buf)
                            {
                                view.ShowDialogAsync("Нельзя удалять сотрудника под которым был выполнен вход!", Title);
                                return;
                            }
                            if (buf.IsUserAdmin == true && CurrentEmployee.IsUserAdmin == false)
                            {
                                view.ShowDialogAsync("Удаление администратора запрещено!", Title);
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
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
            }
            finally
            {
                SelectedTableItem = null;
            }
        }
        private async Task AddRow(object? obj)
        {
            if (CurrentEmployee.CanUserAdd == false)
            {
                view.ShowDialogAsync("У Вас отсутствуют нужные права!", Title);
                return;
            }

            try
            {
                switch (selectedTab)
                {
                    case "automobilesTab":
                        {
                            AddAutomobileView addMaterial = new AddAutomobileView();
                            if (addMaterial.ShowDialog() == true)
                            {
                                AutomobilesList = App.DbContext.Automobiles.ToList();
                            }
                            break;
                        }
                    case "tnTab":
                        {
                            AddTNView addMaterial = new AddTNView();
                            if (addMaterial.ShowDialog() == true)
                            {
                                TNsList = App.DbContext.TNs.ToList();
                            }
                            break;
                        }
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
                            if (CurrentEmployee.IsUserAdmin)
                            {
                                if (add.ShowDialog() == true)
                                {
                                    EmployeesList = App.DbContext.Employees.ToList();
                                }
                            }
                            else
                            {
                                view.ShowDialogAsync("У Вас отсутствуют нужные права!", Title);
                                return;
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
                    case "acountTab":
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
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message,Title);
            }
        }
        private async Task EditRow(object? obj)
        {
            if (CurrentEmployee.CanUserEdit == false)
            {
                view.ShowDialogAsync("У Вас отсутствуют нужные права!", Title);
                return;
            }
            if (SelectedTableItem == null) return;            
                int id = SelectedTableItem.ID;
            try
            {
                switch (selectedTab)
                {
                    case "automobilesTab":
                        {
                            AddAutomobileView addMaterial = new AddAutomobileView(SelectedTableItem as Automobile);
                            if (addMaterial.ShowDialog() == true)
                            {
                                AutomobilesList = App.DbContext.Automobiles.ToList();
                            }
                            break;
                        }
                    case "tnTab":
                        {
                            AddTNView addMaterial = new AddTNView(SelectedTableItem as TN);
                            if (addMaterial.ShowDialog() == true)
                            {
                                TNsList = App.DbContext.TNs.ToList();
                            }
                            break;
                        }
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
                    case "contractTab":
                        {
                            AddContractView add = new AddContractView((Contract)SelectedTableItem);
                            if (add.ShowDialog() == true)
                            {
                                ContractsList = App.DbContext.Contracts.ToList();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
            }
        }
        public FilterDataGrid.FilterDataGrid CurrentDataGrid { get; set; }
        private async Task ExportExcelWithSave(object? obj)
        {
            SaveFileDialog savefile = new SaveFileDialog()
            {
                Filter = "Файлы Excel(*.xlsx)|*.xlsx|All files(*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = false,
            };
            if (savefile.ShowDialog() == true)
            {
                try
                {
                    var myClassType = CurrentDataGrid.ItemsSource.GetType().GetGenericArguments().Single();

                var method = typeof(ExportToExcel).GetMethod("ExportFromDataGrid", BindingFlags.Static | BindingFlags.Public);
                var genericMethod = method.MakeGenericMethod(myClassType);

                    // вызов статического метода
                    genericMethod.Invoke(null, new object[3] { savefile.FileName, CurrentDataGrid, view });
                }
                catch
                {
                    view.ShowDialogAsync("Во время экспорта произошла ошибка...", "Экспорт в Excel");
                    return;
                }
                view.ShowDialogAsync("Экспорт успешно завершен!", Title);
            }
        }
        private async Task AddRowCopy(object? obj)
        {
            if (CurrentEmployee.CanUserAdd == false)
            {
                view.ShowDialogAsync("У Вас отсутствуют нужные права", Title);
                return;
            }
            if (SelectedTableItem == null)
            {
                view.ShowDialogAsync("Выберите запись!", Title);
                return;
            }
            try
            {
                switch (selectedTab)
                {
                    case "automobilesTab":
                        {
                            var copy = SelectedTableItem as Automobile;
                            copy.ID = 0;
                            AddAutomobileView addMaterial = new AddAutomobileView();
                            (addMaterial.DataContext as AddAutomobileViewModel).Automobile = copy;
                            if (addMaterial.ShowDialog() == true)
                            {
                                AutomobilesList = App.DbContext.Automobiles.ToList();
                            }
                            break;
                        }
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
                            if (CurrentEmployee.IsUserAdmin == false)
                            {
                                view.ShowDialogAsync("У Вас отсутствуют нужные права", Title);
                                return;
                            }
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
                            Contact copy = SelectedTableItem is Contact ? (Contact)SelectedTableItem : new Contact();
                            copy.ID = 0;
                            AddContractView add = new AddContractView();
                            (add.DataContext as AddContactViewModel)!.Contact = copy;
                            if (add.ShowDialog() == true)
                            {
                                ContractsList = App.DbContext.Contracts.ToList();
                            }
                            break;
                        }
                    case "tnTab":
                        {
                            TN copy = SelectedTableItem is TN ? (TN)SelectedTableItem : new TN();
                            copy.ID = 0;
                            AddTNView add = new AddTNView();
                            (add.DataContext as AddTNViewModel)!.TN = copy;
                            if (add.ShowDialog() == true)
                            {
                                TNsList = App.DbContext.TNs.ToList();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
            }
        }
        private async Task Search(string text)
        {
            if (text.Equals(string.Empty))
            {
                try
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
                }
                catch (Exception ex)
                {
                    view.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
                }
                return;
            }
            try
            {
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
            catch (Exception ex)
            {
                view.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
            }
        }
        #endregion
    }
}
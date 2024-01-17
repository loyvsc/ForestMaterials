namespace BuildMaterials.Views
{
    public partial class AddIndividualView : FluentWindow
    {
        public AddIndividualView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this);
        }
        public AddIndividualView(Models.Individual employee)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this, employee);
        }
    }
}
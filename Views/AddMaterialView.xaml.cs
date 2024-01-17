namespace BuildMaterials.Views
{
    public partial class AddMaterialView : FluentWindow
    {
        public AddMaterialView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddMaterialViewModel(this);
        }

        public AddMaterialView(Models.Material material)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddMaterialViewModel(this, material);
        }
    }
}
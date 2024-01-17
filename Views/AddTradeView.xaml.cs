namespace BuildMaterials.Views
{
    public partial class AddTradeView : FluentWindow
    {
        public AddTradeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTradeViewModel(this);
        }

        public AddTradeView(Models.Trade trade)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTradeViewModel(this, trade);
        }
    }
}
using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddTradeView : Window
    {
        public AddTradeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTradeViewModel(this);
        }

        public AddTradeView(Models.Trade trade)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTradeViewModel(this,trade);
        }
    }
}
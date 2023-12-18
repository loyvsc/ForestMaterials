using System.Windows;

namespace BuildMaterials.Views
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }

    public partial class AboutProgramView : Window
    {
        public AboutProgramView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.AboutProgrammViewModel(this);
        }
    }
}
using System.Text.RegularExpressions;
using System.Windows;
using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    public partial class AddContactView : FluentWindow
    {
        public AddContactView()
        {
            InitializeComponent();
            DataContext = new AddContactViewModel();
        }

        public AddContactView(Organization organization)
        {
            InitializeComponent();
            DataContext = new AddContactViewModel(this,organization);
        }

        public AddContactView(Contact contact)
        {
            InitializeComponent();
            DataContext = new AddContactViewModel(this, contact);
        }

        private void phoneTextbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

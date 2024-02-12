using System.Text.RegularExpressions;
using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    public partial class AddContactView : FluentWindow
    {
        public AddContactView(Organization organization,AddOrganizationViewModel organizationViewModel)
        {
            InitializeComponent();
            DataContext = new AddContactViewModel(this,organization, organizationViewModel);
        }

        public AddContactView(Contact contact, AddOrganizationViewModel organizationViewModel)
        {
            InitializeComponent();
            DataContext = new AddContactViewModel(this, contact, organizationViewModel);
        }

        private void phoneTextbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = PreviewTextInputregex().IsMatch(e.Text);
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex PreviewTextInputregex();
    }
}

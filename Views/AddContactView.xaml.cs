using System.Windows;
using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    public partial class AddContactView : Window
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
    }
}

using System.Windows;

namespace BuildMaterials
{
    public partial class App
    {
        public static BD.ApplicationContext DbContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            DbContext = new BD.ApplicationContext();
            base.OnStartup(e);
        }
    }
}
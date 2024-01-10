using System.Windows;

namespace BuildMaterials
{
    public partial class App
    {
        public static BD.ApplicationContext DbContext { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            DbContext = new BD.ApplicationContext();
            base.OnStartup(e);
        }
    }
}
namespace MAUI_MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("NewPage", typeof(NewPage1));
            Routing.RegisterRoute("NewPage2", typeof(NewPage2));
        }
    }
}

using ProfileAss.Data;

namespace ProfileAss
{
    public partial class App : Application
    {
      
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

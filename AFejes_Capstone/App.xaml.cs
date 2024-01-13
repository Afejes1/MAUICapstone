using AFejes_Capstone.Models;
using AFejes_Capstone.Services;
using Application = Microsoft.Maui.Controls.Application;

namespace AFejes_Capstone
{
    public partial class App : Application
    {
        public static DatabaseService DatabaseServiceInstance { get; private set; }
        public static ReportService ReportServiceInstance { get; private set; }
        public static string CurrentUser { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            Shell.Current.GoToAsync("//LoginPage");  // This will navigate to LoginPage on app start


        }


        public static void InitializeDatabaseService(string databasePath)
        {
            DatabaseServiceInstance = new DatabaseService(Constants.GetDatabasePath(databasePath));
            DatabaseServiceInstance.InitializeDatabaseAsync();
        }


        public static void InitializeReportService()
        {
            ReportServiceInstance = new ReportService();
        }

        public static void SetCurrentUser(string user)
        {
            CurrentUser = user;
        }

        public static void ClearCurrentUser()
        {
            CurrentUser = null;
        }

    }

}

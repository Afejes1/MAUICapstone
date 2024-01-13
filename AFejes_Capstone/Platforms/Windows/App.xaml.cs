using AFejes_Capstone.WinUI;
using Microsoft.UI.Xaml;
using Plugin.LocalNotification;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
//[assembly: Dependency(typeof(DatabasePath))]

namespace AFejes_Capstone.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        //public App()
        //{
        //    this.InitializeComponent();
        //}
        // For Windows in App.xaml.cs or equivalent
        public App()
        {
            SQLitePCL.Batteries.Init();
            DependencyService.Register<IFileService, FileService>();

            InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    }
    public class DatabasePath : IDatabasePath
    {
        public string GetDatabasePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, filename);
        }
    }
  
}


using Android.App;
using Android.Runtime;
using Plugin.LocalNotification;

namespace AFejes_Capstone.Droid
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            SQLitePCL.Batteries.Init();

        }
        protected override MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification();
            DependencyService.Register<IFileService, FileService>();

            return builder.Build();
        }
    }

}

using Android.OS;
using AFejes_Capstone;

[assembly: Dependency(typeof(FileService))]
namespace AFejes_Capstone
{
    public class FileService : IFileService
    {

        public string GetDownloadFolderPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }
    }
}

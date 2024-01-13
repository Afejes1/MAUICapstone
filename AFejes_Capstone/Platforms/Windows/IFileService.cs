using AFejes_Capstone;
using AFejes_Capstone.WinUI;

[assembly: Dependency(typeof(FileService))]
namespace AFejes_Capstone.WinUI
{
    public class FileService : IFileService
    {
        public string GetDownloadFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}

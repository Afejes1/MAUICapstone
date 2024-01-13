public static class Constants
{
    public const string DatabaseFilenameTemplate = "{0}_AFejes_Capstone.db3"; 
    public const string ReportFilename = "report.pdf";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string GetDatabasePath(string username)
    {
        var databaseFilename = string.Format(DatabaseFilenameTemplate, username);
        return Path.Combine(FileSystem.AppDataDirectory, databaseFilename);
    }

    public static string ReportPath
    {
        get
        {
            return Path.Combine(FileSystem.AppDataDirectory, ReportFilename);
        }
    }

    public static class DateValidator
    {
        public static bool ValidateDates(DateTime startDate, DateTime endDate, ContentPage page)
        {
            if (endDate < startDate)
            {
                page.DisplayAlert("Invalid Date", "End date cannot be earlier than the start date.", "OK");
                return false;
            }
            return true;
        }
    }
}

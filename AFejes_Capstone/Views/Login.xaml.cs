using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SQLite;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using static AFejes_Capstone.MainPage;

namespace AFejes_Capstone
{
    public partial class LoginPage : ContentPage
    {
        private readonly SQLiteAsyncConnection _userDb;

        public LoginPage()
        {
            InitializeComponent();
            _userDb = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "users.db3"));
            _userDb.CreateTableAsync<User>().Wait();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var user = await _userDb.Table<User>().Where(u => u.Username == UsernameEntry.Text).FirstOrDefaultAsync();
            if (user != null && VerifyPassword(PasswordEntry.Text, user.PasswordHash, user.PasswordSalt))
            {
                // Set the current user in the session
                App.SetCurrentUser(user.Username);
                App.InitializeDatabaseService(user.Username);
                App.InitializeReportService();

                InitializeMainPageResources();
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                ErrorLabel.Text = "Invalid username or password.";
            }
        }

        private void InitializeMainPageResources()
        {
            string userDatabasePath = Constants.GetDatabasePath(App.CurrentUser);

        }

        private async void OnNewUserClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                ErrorLabel.Text = "Username and password are required.";
                return;
            }

            var existingUser = await _userDb.Table<User>().Where(u => u.Username == UsernameEntry.Text).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                ErrorLabel.Text = "Username already exists.";
                return;
            }

            var (hash, salt) = HashPassword(PasswordEntry.Text);

            var newUser = new User
            {
                Username = UsernameEntry.Text,
                PasswordHash = Convert.ToBase64String(hash),
                PasswordSalt = Convert.ToBase64String(salt)
            };

            await _userDb.InsertAsync(newUser);

            ErrorLabel.Text = "User created successfully.";
        }

        private (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (hash, salt);
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var saltBytes = Convert.FromBase64String(storedSalt);

            if (string.IsNullOrEmpty(password)) { return false; }

            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashBytes[i]) return false;
                }
            }

            return true;
        }
    }

    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}

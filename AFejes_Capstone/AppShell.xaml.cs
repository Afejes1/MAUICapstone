using Microsoft.Maui.Controls;

namespace AFejes_Capstone
{
    public partial class AppShell : Shell
    {
        public static DatabaseService DatabaseService;

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this; // Set the BindingContext for bindings to work

            // Initially hide the flyout if the login page is the first page
            FlyoutBehavior = FlyoutBehavior.Disabled;

            // Listen to navigation events
            Navigated += OnNavigated;
        }

        // Create a FlyoutBehavior property
        FlyoutBehavior _flyoutBehavior = FlyoutBehavior.Flyout;
        public FlyoutBehavior FlyoutBehavior
        {
            get => _flyoutBehavior;
            set
            {
                _flyoutBehavior = value;
                OnPropertyChanged();
            }
        }

        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Check if the current page is the LoginPage
            if (CurrentItem?.CurrentItem?.CurrentItem is ShellContent content &&
                content.Content is LoginPage)
            {
                FlyoutBehavior = FlyoutBehavior.Disabled; // Disable the flyout
            }
            else
            {
                FlyoutBehavior = FlyoutBehavior.Flyout; // Enable the flyout
            }




            //var loginPageShellItem = (from item in Shell.Current.Items
            //                          where item.Route == "LoginPage"
            //                          select item).FirstOrDefault();

            //if (loginPageShellItem != null)
            //{
            //    loginPageShellItem.IsVisible = false;
            //}



        }



        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Clear session
            App.ClearCurrentUser();

            // Navigate back to LoginPage
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

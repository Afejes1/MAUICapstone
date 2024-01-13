using AFejes_Capstone.Models;
using AFejes_Capstone.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AFejes_Capstone
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Term> Terms { get; set; }
        DatabaseService _databaseService { get; set; }
        ReportService _reportService { get; set; }
      

        public MainPage()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(App.CurrentUser))
            {
                return;
            }

            _databaseService = App.DatabaseServiceInstance;
            _reportService = App.ReportServiceInstance;
            Terms = new ObservableCollection<Term>();
            TermsList.ItemsSource = Terms;
            LoadTermsFromDatabase();
        }


        private async void LoadTermsFromDatabase()
        {
            var terms = await App.DatabaseServiceInstance.GetTermsAsync();
            Terms.Clear();
            foreach (var term in terms)
            {
                Terms.Add(term);
            }
        }
        //private async void OnSearchButtonPressed(object sender, EventArgs e)
        //{
        //    string searchType = (string)searchTypePicker.SelectedItem;
        //    string query = searchBar.Text;

        //    if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(query))
        //    {
        //        searchStatusLabel.Text = "Please select a search type and enter a query.";
        //        return;
        //    }

        //    searchStatusLabel.Text = $"Searching for {query} in {searchType}...";

        //    var results = await SearchAsync(searchType, query);

        //    if (results.Any())
        //    {
        //        await Navigation.PushAsync(new SearchResultsPage(results.Cast<SearchResultItem>()));
        //    }
        //    else
        //    {
        //        searchStatusLabel.Text = $"No {searchType} found for '{query}'.";
        //    }
        //}
        //private void OnGenerateReportClicked(object sender, EventArgs e)
        //{
        //    // Navigate to the new ReportPage
        //    Navigation.PushAsync(new Reports(_databaseService, _reportService));
        //}

        //private async void OnGenerateReportClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var fileService = DependencyService.Get<IFileService>();
        //        string filePath = fileService.GetExternalStoragePath();

        //        //string filePath = Constants.ReportPath;
        //        //var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
        //        //File.WriteAllText($"{docsDirectory.AbsoluteFile.Path}/atextfile.txt", "contents are here");

        //        var reportService = new ReportService(App.DatabaseServiceInstance);
        //        await reportService.GenerateReportAsync(filePath);

        //        await DisplayAlert("Report Generated", $"The report has been saved to {filePath}.", "OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Error", $"An error occurred while generating the report: {ex.Message}", "OK");
        //    }
        //}


        private async Task<IEnumerable<object>> SearchAsync(string searchType, string query)
        {
            // The actual search logic depending on the search type and query
            switch (searchType)
            {
                case "Terms":
                    return await _databaseService.SearchTermsAsync(query);  // Implement this method in your DatabaseService

                case "Courses":
                    return await _databaseService.SearchCoursesAsync(query);  // Implement this method in your DatabaseService

                case "Assessments":
                    return await _databaseService.SearchAssessmentsAsync(query);  // Implement this method in your DatabaseService

                default:
                    return new List<object>();
            }
        }

        private void OnAddTermClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new TermFormPage(null, _databaseService));
        }


        private void OnEditTermClicked(object sender, System.EventArgs e)
        {
            var term = (sender as Button).BindingContext as Term;
            Navigation.PushAsync(new TermFormPage(term, _databaseService));
        }

        private async void OnDeleteTermClicked(object sender, System.EventArgs e)
        {
            var term = (sender as Button).BindingContext as Term;
            if (term != null)
            {
                Terms.Remove(term);
                await _databaseService.DeleteTermAsync(term);
            }
        }

        private void OnViewCoursesClicked(object sender, System.EventArgs e)
        {
            var term = (sender as Button).BindingContext as Term;
            if (term != null)
            {
                Navigation.PushAsync(new CoursesPage(term.Id, term.Title, _databaseService));
            }
        }
        private void OnGenerateTestDataClicked(object sender, EventArgs e)
        {
            TestDataGenerator.GenerateTestData(_databaseService);
            LoadTermsFromDatabase();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Assuming UserSession.CurrentUser is still valid
            if (!string.IsNullOrEmpty(App.CurrentUser))
            {
                _databaseService = App.DatabaseServiceInstance;
                _reportService = App.ReportServiceInstance;

                Terms = new ObservableCollection<Term>();
                TermsList.ItemsSource = Terms;
                LoadTermsFromDatabase();
            }
            else
            {
                // Handle the scenario where UserSession.CurrentUser is not set

                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");

            }
        }



    }
}

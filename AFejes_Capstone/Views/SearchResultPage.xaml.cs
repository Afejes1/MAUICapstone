using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AFejes_Capstone
{
    public partial class SearchResultsPage : ContentPage
    {
        //DatabaseService _databaseService;

        public SearchResultsPage()
        {
            InitializeComponent();
            //SearchResultsList.ItemsSource = results;

            //_databaseService = App.DatabaseServiceInstance;
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string searchType = (string)searchTypePicker.SelectedItem;
            string query = searchBar.Text;

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(query))
            {
                searchStatusLabel.Text = "Please select a search type and enter a query.";
                return;
            }

            searchStatusLabel.Text = $"Searching for {query} in {searchType}...";

            var results = await SearchAsync(searchType, query);

            if (results.Any())
            {
                SearchResultsList.ItemsSource = results.Cast<SearchResultItem>();
            }
            else
            {
                searchStatusLabel.Text = $"No {searchType} found for '{query}'.";
            }
        }

        private async Task<IEnumerable<object>> SearchAsync(string searchType, string query)
        {
            // The actual search logic depending on the search type and query
            switch (searchType)
            {
                case "Terms":
                    return await App.DatabaseServiceInstance.SearchTermsAsync(query);

                case "Courses":
                    return await App.DatabaseServiceInstance.SearchCoursesAsync(query);

                case "Assessments":
                    return await App.DatabaseServiceInstance.SearchAssessmentsAsync(query);

                default:
                    return new List<object>();
            }
        }
    }
}

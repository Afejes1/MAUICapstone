using AFejes_Capstone.Models;
using AFejes_Capstone.Services;
using AFejes_Capstone;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AFejes_Capstone
{
    public partial class Reports : ContentPage
    {
        //private readonly DatabaseService _databaseService;
        //private readonly ReportService _reportService;

        
        public Reports()
        {
            InitializeComponent();
            //_databaseService = App.DatabaseServiceInstance;
            //_reportService = App.ReportServiceInstance;
        }



        private async void OnGeneratePdfClicked(object sender, EventArgs e)
        {
            IFileService fileService = DependencyService.Get<IFileService>();
            var downloadsPath = fileService.GetDownloadFolderPath();
            var filePath = Path.Combine(downloadsPath, "SAM_Report.pdf");

            await App.ReportServiceInstance.GenerateReportAsync(filePath);

            await DisplayAlert("Report Generated", $"The report has been saved at {filePath}", "OK");
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAssessments();
          
        }

        private async Task LoadAssessments()
        {
            var assessments = await App.DatabaseServiceInstance.GetAssessmentsAsync();
            dataCollectionView.ItemsSource = assessments;
        }

    }
}

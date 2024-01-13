using AFejes_Capstone.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace AFejes_Capstone
{
    public partial class TermFormPage : ContentPage
    {
        public Term ExistingTerm { get; set; }
        DatabaseService _databaseService;

        public TermFormPage(Term term, DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;

            if (term != null)
            {
                ExistingTerm = term;

                TermTitleEntry.Text = term.Title;
                StartDatePicker.Date = DateTime.Parse(term.StartDate);
                AnticipatedEndDatePicker.Date = DateTime.Parse(term.AnticipatedEndDate);

                CoursesList.ItemsSource = term.Courses;
            }
        }

        private async void OnViewCourseDetailsClicked(object sender, EventArgs e)
        {
            var course = (sender as Button).BindingContext as Course;
            if (course != null)
            {
                await Navigation.PushAsync(new CourseDetailsPage(course, _databaseService));
            }
        }

        private async void OnSaveTermClicked(object sender, EventArgs e)
        {
            if (!Constants.DateValidator.ValidateDates(StartDatePicker.Date, AnticipatedEndDatePicker.Date, this))
            {
                return;
            }
            if (ExistingTerm == null)
            {
                // Create a new term
                Term newTerm = new Term
                {
                    Title = TermTitleEntry.Text,
                    StartDate = StartDatePicker.Date.ToShortDateString(),
                    AnticipatedEndDate = AnticipatedEndDatePicker.Date.ToShortDateString(),
                    Courses = new ObservableCollection<Course>()
                };

                // Save the new term to the database
                await _databaseService.SaveTermAsync(newTerm);
            }
            else
            {
                // Update existing term
                ExistingTerm.Title = TermTitleEntry.Text;
                ExistingTerm.StartDate = StartDatePicker.Date.ToShortDateString();
                ExistingTerm.AnticipatedEndDate = AnticipatedEndDatePicker.Date.ToShortDateString();

                await _databaseService.SaveTermAsync(ExistingTerm);
            }

            await Navigation.PopAsync();
        }
    }
}

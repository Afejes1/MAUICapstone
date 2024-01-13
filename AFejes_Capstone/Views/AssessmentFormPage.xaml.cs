using AFejes_Capstone.Models;
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace AFejes_Capstone
{
    public partial class AssessmentFormPage : ContentPage
    {
        public Assessment ExistingAssessment { get; set; }
        DatabaseService _databaseService { get; set; }
        int _courseId;

        public AssessmentFormPage(int courseId, Assessment assessment, DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _courseId = courseId;

            if (assessment != null)
            {
                ExistingAssessment = assessment;
                AssessmentNameEntry.Text = assessment.AssessmentName;
                TypePicker.SelectedItem = assessment.Type;
                StartDatePicker.Date = DateTime.Parse(assessment.StartDate);
                EndDatePicker.Date = DateTime.Parse(assessment.EndDate);
                NotifyStartDateSwitch.IsToggled = assessment.NotifyStartDate;
                NotifyEndDateSwitch.IsToggled = assessment.NotifyEndDate;
            }
        }
        private async Task<bool> CanAddAssessment(string type)
        {
            var assessments = await _databaseService.GetAssessmentsByCourseIdAsync(_courseId);
            return !assessments.Any(a => a.Type == type);
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (await CanAddAssessment(TypePicker.SelectedItem.ToString()) || (ExistingAssessment != null && ExistingAssessment.Type == TypePicker.SelectedItem.ToString())
                )
            {
                Assessment savedAssessment;
                if (ExistingAssessment == null)
                {
                    // Create a new assessment
                    savedAssessment = new Assessment
                    {
                        CourseId = _courseId,
                        AssessmentName = AssessmentNameEntry.Text,
                        Type = TypePicker.SelectedItem.ToString(),
                        StartDate = StartDatePicker.Date.ToShortDateString(),
                        EndDate = EndDatePicker.Date.ToShortDateString(),
                        NotifyStartDate = NotifyStartDateSwitch.IsToggled,
                        NotifyEndDate = NotifyEndDateSwitch.IsToggled
                    };
                    if (!Constants.DateValidator.ValidateDates(StartDatePicker.Date, EndDatePicker.Date, this))
                    {
                        return;
                    }
                    await _databaseService.SaveAssessmentAsync(savedAssessment);
                }
                else
                {
                    // Update existing assessment
                    ExistingAssessment.AssessmentName = AssessmentNameEntry.Text;
                    ExistingAssessment.Type = TypePicker.SelectedItem.ToString();
                    ExistingAssessment.StartDate = StartDatePicker.Date.ToShortDateString();
                    ExistingAssessment.EndDate = EndDatePicker.Date.ToShortDateString();
                    ExistingAssessment.NotifyStartDate = NotifyStartDateSwitch.IsToggled;
                    ExistingAssessment.NotifyEndDate = NotifyEndDateSwitch.IsToggled;

                    savedAssessment = ExistingAssessment;

                    await _databaseService.SaveAssessmentAsync(savedAssessment);
                }


                foreach (var page in Navigation.NavigationStack)
                {
                    if (page is CourseFormPage courseFormPage)
                    {
                        if (ExistingAssessment == null)
                        {
                            courseFormPage.Assessments.Add(savedAssessment);
                        }
                        else
                        {
                            var existingItem = courseFormPage.Assessments.FirstOrDefault(a => a.Id == ExistingAssessment.Id);
                            if (existingItem != null)
                            {
                                var index = courseFormPage.Assessments.IndexOf(existingItem);
                                courseFormPage.Assessments[index] = savedAssessment;
                            }
                        }
                        break;
                    }
                }

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "You can only have one Objective Assessment and one Performance Assessment for each course.", "OK");
            }
        }
    }
}

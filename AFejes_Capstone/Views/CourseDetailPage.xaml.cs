using AFejes_Capstone.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace AFejes_Capstone
{
    public partial class CourseDetailsPage : ContentPage
    {
        public ObservableCollection<Assessment> Assessments { get; set; }
        DatabaseService _databaseService { get; set; }
        Course _course { get; set; }

        public CourseDetailsPage(Course course, DatabaseService databaseService)
        {
            InitializeComponent();
            _course = course;
            _databaseService = databaseService;
            Assessments = new ObservableCollection<Assessment>();

            CourseNameLabel.Text = $"Course: {course.CourseName}";
            StartDateLabel.Text = $"Start Date: {course.StartDate}";
            EndDateLabel.Text = $"Anticipated End Date: {course.AnticipatedEndDate}";
            StatusLabel.Text = $"Status: {course.CourseStatus}";
            InstructorNameLabel.Text = $"Instructor: {course.InstructorName}";
            InstructorEmailLabel.Text = $"Email: {course.InstructorEmail}";
            InstructorPhoneLabel.Text = $"Phone: {course.InstructorPhone}";
            NotesLabel.Text = $"Notes: {course.Notes}";

            LoadAssessmentsFromDatabase(course.Id);
        }

        private async void LoadAssessmentsFromDatabase(int courseId)
        {
            var assessments = await _databaseService.GetAssessmentsByCourseIdAsync(courseId);
            Assessments.Clear();
            foreach (var assessment in assessments)
            {
                Assessments.Add(assessment);
            }
        }

        private void OnAddAssessmentClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AssessmentFormPage(_course.Id, null, _databaseService));
        }

        private void OnEditAssessmentClicked(object sender, System.EventArgs e)
        {
            var assessment = (sender as Button).BindingContext as Assessment;
            Navigation.PushAsync(new AssessmentFormPage(_course.Id, assessment, _databaseService));
        }

        private async void OnDeleteAssessmentClicked(object sender, System.EventArgs e)
        {
            var assessment = (sender as Button).BindingContext as Assessment;
            if (assessment != null)
            {
                Assessments.Remove(assessment);
                await _databaseService.DeleteAssessmentAsync(assessment);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAssessmentsFromDatabase(_course.Id);
        }
    }
}

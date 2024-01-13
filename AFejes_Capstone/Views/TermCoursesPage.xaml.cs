using AFejes_Capstone.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AFejes_Capstone
{
    public partial class CoursesPage : ContentPage
    {
        public ObservableCollection<Course> Courses { get; set; }
        DatabaseService _databaseService { get; set; }
        int _termId;
        string _termName;
        public CoursesPage(int termId, string termName, DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            HeaderLabel.Text = $"Courses for {termName}";
            Courses = new ObservableCollection<Course>();
            CoursesList.ItemsSource = Courses;
            LoadCoursesFromDatabase();
            _termId = termId;
            _termName = termName;
        }

        private async void LoadCoursesFromDatabase()
        {
            var courses = await _databaseService.GetCoursesForTermAsync(_termId);
            Courses.Clear();
            foreach (var course in courses)
            {
                Courses.Add(course);
            }
        }

        private void OnAddCourseClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CourseFormPage(_termId, _termName, null, _databaseService));
        }

        private void OnEditCourseClicked(object sender, System.EventArgs e)
        {
            var course = (sender as Button).BindingContext as Course;
            Navigation.PushAsync(new CourseFormPage(_termId, _termName, course, _databaseService));
        }

        private async void OnDeleteCourseClicked(object sender, System.EventArgs e)
        {
            var course = (sender as Button).BindingContext as Course;
            if (course != null)
            {
                Courses.Remove(course);
                await _databaseService.DeleteCourseAsync(course);
            }
        }

        private void OnDetailedViewClicked(object sender, System.EventArgs e)
        {
            var course = (sender as Button).BindingContext as Course;
            Navigation.PushAsync(new CourseDetailsPage(course, _databaseService));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCoursesFromDatabase();

        }
    }
}

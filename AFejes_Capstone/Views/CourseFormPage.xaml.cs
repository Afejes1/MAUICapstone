using AFejes_Capstone.Models;
using System.Collections.ObjectModel;
using static Constants;

namespace AFejes_Capstone
{
    public partial class CourseFormPage : ContentPage
    {
        public Course ExistingCourse { get; set; }
        public ObservableCollection<Assessment> Assessments { get; set; }
        DatabaseService _databaseService { get; set; }
        int _termId;
        string _termName;

        int _courseId;
        string _courseNotes;


        public CourseFormPage(int termId, string termName, Course course, DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _termId = termId;
            _termName = termName;

            Assessments = new ObservableCollection<Assessment>();
            AssessmentsList.ItemsSource = Assessments;
            PageTitleLabel.Text = course != null ?
                              $"Edit Course for Term {_termName}" :
                              $"Add Course for Term {_termName}";
            if (course != null)
            {
                ExistingCourse = course;
                _courseId = course.Id;
                _courseNotes = course.Notes;
                CourseNameEntry.Text = course.CourseName;
                StartDatePicker.Date = DateTime.Parse(course.StartDate);
                EndDatePicker.Date = DateTime.Parse(course.AnticipatedEndDate);
                StatusPicker.SelectedItem = course.CourseStatus;
                InstructorNameEntry.Text = course.InstructorName;
                InstructorEmailEntry.Text = course.InstructorEmail;
                InstructorPhoneEntry.Text = course.InstructorPhone;
                NotifyStartDateSwitch.IsToggled = course.NotifyStartDate;
                NotifyEndDateSwitch.IsToggled = course.NotifyEndDate;
                NotesEditor.Text = course.Notes;

                LoadAssessmentsFromDatabase(course.Id);
            }
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

        private void OnEditAssessmentClicked(object sender, EventArgs e)
        {
            var assessment = (sender as Button).BindingContext as Assessment;
            Navigation.PushAsync(new AssessmentFormPage(_courseId, assessment, _databaseService));
        }

        private async void OnDeleteAssessmentClicked(object sender, EventArgs e)
        {
            var assessment = (sender as Button).BindingContext as Assessment;
            if (assessment != null)
            {
                Assessments.Remove(assessment);
                await _databaseService.DeleteAssessmentAsync(assessment);
            }
        }
        private async Task<bool> SaveCourseAsync()
        {
            if (string.IsNullOrEmpty(CourseNameEntry.Text) ||
                StatusPicker.SelectedItem == null ||
                string.IsNullOrEmpty(InstructorNameEntry.Text) ||
                string.IsNullOrEmpty(InstructorEmailEntry.Text) ||
                string.IsNullOrEmpty(InstructorPhoneEntry.Text))
            {
                await DisplayAlert("Missing Data", "Please fill in all fields.", "OK");
                return false;
            }

            if (!DateValidator.ValidateDates(StartDatePicker.Date, EndDatePicker.Date, this))
            {
                return false;
            }


            Course savedCourse;

            if (ExistingCourse == null)
            {
                savedCourse = new Course
                {
                    TermId = _termId,
                    CourseName = CourseNameEntry.Text,
                    StartDate = StartDatePicker.Date.ToShortDateString(),
                    AnticipatedEndDate = EndDatePicker.Date.ToShortDateString(),
                    DueDate = DueDatePicker.Date.ToLongDateString(),
                    CourseStatus = StatusPicker.SelectedItem.ToString(),
                    InstructorName = InstructorNameEntry.Text,
                    InstructorEmail = InstructorEmailEntry.Text,
                    InstructorPhone = InstructorPhoneEntry.Text,
                    NotifyStartDate = NotifyStartDateSwitch.IsToggled,
                    NotifyEndDate = NotifyEndDateSwitch.IsToggled,
                    Notes = NotesEditor.Text
                };

                await _databaseService.SaveCourseAsync(savedCourse);
                _courseId = savedCourse.Id;
                NotificationService.ScheduleCourseNotifications(savedCourse);
            }
            else
            {
                ExistingCourse.CourseName = CourseNameEntry.Text;
                ExistingCourse.StartDate = StartDatePicker.Date.ToShortDateString();
                ExistingCourse.AnticipatedEndDate = EndDatePicker.Date.ToShortDateString();
                ExistingCourse.DueDate = DueDatePicker.Date.ToShortDateString();
                ExistingCourse.CourseStatus = StatusPicker.SelectedItem.ToString();
                ExistingCourse.InstructorName = InstructorNameEntry.Text;
                ExistingCourse.InstructorEmail = InstructorEmailEntry.Text;
                ExistingCourse.InstructorPhone = InstructorPhoneEntry.Text;
                ExistingCourse.Notes = NotesEditor.Text;
                ExistingCourse.NotifyStartDate = NotifyStartDateSwitch.IsToggled;
                ExistingCourse.NotifyEndDate = NotifyEndDateSwitch.IsToggled;

                savedCourse = ExistingCourse;

                await _databaseService.SaveCourseAsync(savedCourse);
                NotificationService.ScheduleCourseNotifications(savedCourse);

            }

            return true;
        }

        private async void OnAddAssessmentClicked(object sender, EventArgs e)
        {
            var isSaved = await SaveCourseAsync();
            if (isSaved)
            {
                Navigation.PushAsync(new AssessmentFormPage(_courseId, null, _databaseService));
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var isSaved = await SaveCourseAsync();
            if (isSaved)
            {
                await Navigation.PopAsync();
            }
        }
        private async void OnShareNotesClicked(object sender, EventArgs e)
        {
            var notes = _courseNotes ?? NotesEditor.Text;

            if (string.IsNullOrEmpty(notes))
            {
                await DisplayAlert("Nothing to share!", "Please save some notes in order to share them.", "OK");

                return;
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = notes,
                Title = "Share Notes"
            });
        }

    }
}

using AFejes_Capstone;
using AFejes_Capstone.Models;
using System.Collections.Generic;

public static class TestDataGenerator
{
    public static async void GenerateTestData(DatabaseService databaseService)
    {
        var term = new Term
        {
            Title = "Fall 2023",
            StartDate = "2023-09-01",
            AnticipatedEndDate = "2023-12-31"
        };
        await databaseService.SaveTermAsync(term);

        var course = new Course
        {
            TermId = term.Id,
            CourseName = "Introduction to Computer Science",
            StartDate = "2023-09-01",
            AnticipatedEndDate = "2023-12-15",
            CourseStatus = "In Progress",
            InstructorName = "Anika Patel",
            InstructorEmail = "anika.patel@strimeuniversity.edu",
            InstructorPhone = "555-123-4567",
            Notes = "This is a sample course.",
            NotifyStartDate = true,
            NotifyEndDate = true
        };

        databaseService.SaveCourseAsync(course).GetAwaiter().GetResult();

        var assessments = new List<Assessment>
        {
            new Assessment
            {
                CourseId = course.Id,
                AssessmentName = "Midterm Exam",
                Type = "Objective",
                StartDate = "2023-10-15",
                EndDate = "2023-10-15",
                NotifyStartDate = true,
                NotifyEndDate = true
            },
            new Assessment
            {
                CourseId = course.Id,
                AssessmentName = "Final Exam",
                Type = "Performance",
                StartDate = "2023-12-10",
                EndDate = "2023-12-10",
                NotifyStartDate = true,
                NotifyEndDate = true
            }
        };
        foreach (var assessment in assessments)
        {
            await databaseService.SaveAssessmentAsync(assessment);

        }

        NotificationService.ScheduleCourseNotifications(course);
        foreach (var assessment in assessments)
        {
            NotificationService.ScheduleAssessmentNotifications(assessment);
        }
    }


}

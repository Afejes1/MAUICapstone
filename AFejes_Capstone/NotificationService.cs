using System;
using AFejes_Capstone.Models;
using Plugin.LocalNotification;

namespace AFejes_Capstone
{

    public static class NotificationService
    {
        public static async void ScheduleCourseNotifications(Course course)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {


                if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
                {
                    await LocalNotificationCenter.Current.RequestNotificationPermission();
                }

                if (course.NotifyStartDate)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = course.Id,
                        Title = "Course Start Reminder",
                        Description = $"The course {course.CourseName} starts on {course.StartDate}",
                        Schedule =
                {
                    NotifyTime = DateTime.Parse(course.StartDate)
                    
                    //Used for testing notifications. Send alert 5 seconds from now
                    //NotifyTime = DateTime.Now.AddSeconds(5) 

                }
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                }

                if (course.NotifyEndDate)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = course.Id + 1000,
                        Title = "Course End Reminder",
                        Description = $"The course {course.CourseName} ends on {course.AnticipatedEndDate}",
                        Schedule =
                {
                    NotifyTime = DateTime.Parse(course.AnticipatedEndDate)
                    
                    //Used for testing notifications. Send alert 5 seconds from now
                    //NotifyTime = DateTime.Now.AddSeconds(5) 

                }
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                }
            }
        }

        public static async void ScheduleAssessmentNotifications(Assessment assessment)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {

                if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
                {
                    await LocalNotificationCenter.Current.RequestNotificationPermission();
                }

                if (assessment.NotifyStartDate)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = assessment.Id,
                        Title = "Assessment Start Reminder",
                        Description = $"The assessment {assessment.AssessmentName} starts on {assessment.StartDate}",
                        Schedule =
                {
                    NotifyTime = DateTime.Parse(assessment.StartDate)
                    
                    //Used for testing notifications. Send alert 5 seconds from now
                    //NotifyTime = DateTime.Now.AddSeconds(5) 
                }
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                }

                if (assessment.NotifyEndDate)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = assessment.Id + 1000,
                        Title = "Assessment End Reminder",
                        Description = $"The assessment {assessment.AssessmentName} ends on {assessment.EndDate}",
                        Schedule =
                {
                    NotifyTime = DateTime.Parse(assessment.EndDate)
                    
                    //Used for testing notifications. Send alert 5 seconds from now
                    //NotifyTime = DateTime.Now.AddSeconds(5) 

                }
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                }
            }
        }
    }

}
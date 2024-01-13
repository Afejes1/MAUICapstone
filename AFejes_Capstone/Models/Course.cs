using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AFejes_Capstone.Models
{
    public class Course : SearchResultItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }  // Foreign Key

        public string CourseName
        {
            get => Name;
            set => Name = value;
        }
        public string StartDate { get; set; }
        public string AnticipatedEndDate
        {
            get => EndDate;
            set => EndDate = value;
        }
        public string CourseStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }
        public string Notes { get; set; }
        public string DueDate { get; set; }
        public bool NotifyStartDate { get; set; }
        public bool NotifyEndDate { get; set; }
    }

}

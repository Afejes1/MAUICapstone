using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFejes_Capstone.Models
{

    public class Assessment : SearchResultItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }  // Foreign Key

        public string AssessmentName
        {
            get => Name;
            set => Name = value;
        }
        public string Type { get; set; }  // Objective or Performance
        public string StartDate { get; set; }

        private string _endDate;
        public string EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        public bool NotifyStartDate { get; set; }
        public bool NotifyEndDate { get; set; }
    }
}

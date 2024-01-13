using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFejes_Capstone.Models
{
    public class Term : SearchResultItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title
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
        [Ignore]
        public ObservableCollection<Course> Courses { get; set; }
    }

}



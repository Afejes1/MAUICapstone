using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFejes_Capstone.Models
{
    public class UserSession
    {
        public string CurrentUser { get; set; } = null;
        public UserSession(string currentUser)
        {
            CurrentUser = currentUser;
        }
        public UserSession() { }
    }
}

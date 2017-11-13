using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.DB
{
    public class UserPractice
    {
        public string UserId { get; set; }
        public Guid PracticeId { get; set; }
        public User User { get; set; }
        public Practice Practice { get; set; }
    }
}

using System;

namespace WebApp.DB
{
    public class UserActivity
    {
        public string  UserId{ get; set; }
        public Guid AcitvityId { get; set; }

        public User User { get; set; }
        public Activity Activity { get; set; }
    }
}
using System;

namespace ServicesCourse.Models
{
    public class History
    {
        public string Login { get; set; }   
        public User User { get; set; }

        public int ServiceId { get; set; }  
        public Service Service { get; set; }    

        public DateTime AccessTime { get; set; }
    }
}

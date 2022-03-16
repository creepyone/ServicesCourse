using System.Collections.Generic;

namespace ServicesCourse.Models
{
    public class Service
    {
        public int Id { get; set; } 
        public string ServiceName { get; set; }
        public string AboutService { get; set; }
        public int SubsectionId { get; set; }  
        public string Version { get; set; }
        public bool ActivityStatus { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<History> HistoryRecords { get; set; } = new List<History>();
    }
}

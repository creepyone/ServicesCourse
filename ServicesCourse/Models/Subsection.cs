using System.Collections.Generic;

namespace ServicesCourse.Models
{
    public class Subsection
    {
        public int Id { get; set; } 
        public string SubscetionName { get; set; }
        public int SectionId { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
    }
}

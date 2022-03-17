using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class Subsection
    {
        public int Id { get; set; }


        [Display(Name = "Название подраздела")]
        public string SubscetionName { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
    }
}

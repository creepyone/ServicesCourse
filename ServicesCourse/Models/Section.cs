using System.Collections.Generic;

namespace ServicesCourse.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public List<Subsection> Subsections { get; set; } = new List<Subsection>();
    }
}

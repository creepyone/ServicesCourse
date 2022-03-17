using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Display(Name = "Название раздела")]
        [Required(ErrorMessage="Введите название раздела")]
        public string SectionName { get; set; }
        public List<Subsection> Subsections { get; set; } = new List<Subsection>();
    }
}

﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class Subsection
    {
        public int Id { get; set; }


        [Display(Name = "Название подраздела")]
        [Required(ErrorMessage = "Введите название подраздела")]
        public string SubscetionName { get; set; }

        [Display(Name = "Название раздела")]
        public int SectionId { get; set; }

        public Section Section { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
    }
}

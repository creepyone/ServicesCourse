using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название сервиса")]
        [Display(Name = "Название сервиса")]
        public string ServiceName { get; set; }
        [Display(Name = "О сервисе")]
        public string AboutService { get; set; }

        [Display(Name = "Версия сервиса")]
        [Required(ErrorMessage = "Введите версию сервиса")]
        public string Version { get; set; }

        [Display(Name = "Активен")]
        public bool ActivityStatus { get; set; }

        [Display(Name = "Название подраздела")]
        public Subsection Subsection { get; set; }
        [Display(Name = "Название подраздела")]
        public int SubsectionId { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<History> HistoryRecords { get; set; } = new List<History>();
    }
}

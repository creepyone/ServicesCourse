using System;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class History
    {
        public string Login { get; set; }

        [Display(Name = "Логин")]
        public User User { get; set; }

        public int ServiceId { get; set; }
        [Display(Name = "Название сервиса")]
        public Service Service { get; set; }

        [Display(Name = "Время обращения")]
        public DateTime AccessTime { get; set; }
    }
}

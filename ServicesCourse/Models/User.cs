using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class User
    { 
        [Required(ErrorMessage = "Не указан логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Активен")]
        public bool ActivityStatus { get; set; }

        public int UserTypeId { get; set; }

        [Display(Name = "Тип пользователя")]
        public UserType UserType { get; set; }

        public UserProfile UserProfile { get; set; }

        public List<Service> Services { get; set; } = new List<Service>();
        public List<History> HistoryRecords { get; set; } = new List<History>();
    }
}

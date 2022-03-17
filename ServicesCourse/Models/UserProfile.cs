using System;
using System.ComponentModel.DataAnnotations;

namespace ServicesCourse.Models
{
    public class UserProfile
    {
        [Display(Name="Логин")]
        public string Login { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }


        [Display(Name = "Пол")]
        public Sex Sex { get; set; }
        public int? SexId { get; set; }


        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Почта")]
        public string Email { get; set; }

        public User User { get; set; }

    }
}

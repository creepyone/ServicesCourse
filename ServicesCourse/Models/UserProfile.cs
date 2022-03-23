using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ServicesCourse.Models
{
    public class UserProfile
    {
        [Required(ErrorMessage = "Не указан логин")]
        [Display(Name = "Логин")]
        [MaxLength(20, ErrorMessage = "Максимальная длина логина 20 символов")]
        [StringLength(20)]
        public string Login { get; set; }

        [Display(Name = "Телефон")]
        [RegularExpression(@"^((\+7|7|8)+([0-9]){10})$", ErrorMessage = "Неверный формат номера телефона. +79999999999 или 89999999999")]
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
        [RegularExpression(@"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$", ErrorMessage = "Неверный формат электронной почты. Например: 123@mail.ru")]
        public string Email { get; set; }

        public User User { get; set; }

    }

    public class PhoneNumberAttribute : ValidationAttribute
    {   
        public new string ErrorMessage { get; } = "Неверный формат номера";
        public override bool IsValid(object value)
        {
            string phoneNumber = value as string;
            if (phoneNumber == null) return false;
            var reg = new Regex(@"^((\+7|7|8)+([0-9]){10})$");
            if (reg.IsMatch(phoneNumber)) return true;
            return false;
        }
    }


}

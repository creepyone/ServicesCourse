using System;

namespace ServicesCourse.Models
{
    public class UserProfile
    {
        public string Login { get; set; }
        public string PhoneNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public int? SexId { get; set; }

        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }

        public User User { get; set; }

    }
}

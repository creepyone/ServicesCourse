using System.Collections.Generic;

namespace ServicesCourse.Models
{
    public enum UserTypes
    {
        Администратор,
        Пользователь
    }

    public class UserType
    {
        public int Id { get; set; }
        public UserTypes UserTypeName { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}

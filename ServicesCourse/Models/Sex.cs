using System.Collections.Generic;

namespace ServicesCourse.Models
{

    public enum SexNames
    {
        Мужской,
        Женский
    }
    public class Sex
    {
        public int Id { get; set; } 
        public SexNames SexName { get; set; }

        public List<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

    }
}
    
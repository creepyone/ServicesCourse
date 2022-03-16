namespace ServicesCourse.Models
{
    public class DataBaseRepository
    {
        private readonly ApplicationDbContext _context;
        public DataBaseRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public User GetUserByLogin(string login)
        {
            return _context.User.Find(login);
        }

        public UserTypes GetUserType(int id)
        {
            return _context.UserType.Find(id).UserTypeName;
        }

    }
}

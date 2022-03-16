using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServicesCourse.Models
{
    public class DataBaseRepository
    {
        private readonly ApplicationDbContext _context;
        public DataBaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByLogin(string login)
        {
            return await _context.User
                .Where(x => x.Login == login)
                .Include(x => x.UserType)
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfile> GetUserProfileByLogin(string login)
        {
            return await _context.UserProfile
                .Where(x => x.Login == login)
                .Include(x => x.SexId)
                .FirstOrDefaultAsync();
        }

        public async Task AddNewUser(string login, string password, int userTypeId = 2, bool activityStatus = true)
        {
            var user = new User { Login = login, Password = password, UserTypeId = userTypeId, ActivityStatus = activityStatus};
            await _context.User.AddAsync(user);
            await _context.UserProfile.AddAsync(new UserProfile { Login = login });
            await _context.SaveChangesAsync();
        }

    }
}

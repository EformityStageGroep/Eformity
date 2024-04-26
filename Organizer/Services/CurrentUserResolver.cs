using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;

namespace Organizer.Services
{
    public class CurrentUserResolver : ICurrentUserService
    {
        private readonly UserDbContext _context;

        public CurrentUserResolver(UserDbContext context)
        {
            _context = context;
        }

        public string? userid { get; set; }

        public async Task<bool> SetUser(string? User)
        {
            var UserInfo = await _context.Users.Where(x => x.id == User).FirstOrDefaultAsync();
            if (UserInfo != null)
            {
               
                return true;
            }
            else
            {
                throw new Exception("User invalid");
            }
        }
    }
}
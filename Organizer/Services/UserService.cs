using Organizer.Contexts;

namespace Organizer.Services
{
    public interface IUserService
    {
        void InsertUserOnStartup();
    }

    public class UserService : IUserService
    {
        private readonly OrganizerContext _context;
        private readonly IGraphClientService _graphClientService;

        public UserService(OrganizerContext context, IGraphClientService graphClientService)
        {
            _context = context;
            _graphClientService = graphClientService;
        }

        public void InsertUserOnStartup()
        {
            Microsoft.Graph.User me = _graphClientService.GetUserProfile();
            var user = _context.Users.Where(u => u.email == me.Mail).FirstOrDefault();

            if (user == null)
            {
                user = new();
                user.fullname = me.DisplayName;
                user.email = me.Mail;
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }
    }
}
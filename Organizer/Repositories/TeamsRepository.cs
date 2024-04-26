using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Services;

namespace Organizer.Repositories
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentTenantService _currentTenantService;

        public TeamsRepository(OrganizerContext context, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _currentTenantService = currentTenantService;
        }
        public async Task CreateTeam(Entities.Team team)
        {
     
            team.tenant_id = _currentTenantService.tenantid;

            // Check if a team with the same ID already exists in the database
            var existingTeam = await _context.Teams.FindAsync(team.id);

            if (existingTeam != null)
            {
                // A team with the same ID already exists, throw an exception or handle the case appropriately
                throw new InvalidOperationException("A team with the same ID already exists.");
            }

            // Add the new team to the database
            _context.Teams.Add(team); 
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the team
        }

        public async Task CreateUserTeam(Entities.UserTeam userteam)
        {
            _context.Users_Teams.Add(userteam);
            await _context.SaveChangesAsync();
        }




        public async Task<List<Entities.Team>> GetTeamsByUser()
        {

            var userId = _currentUserService.userid;
            Console.WriteLine(userId);
            // Find the user in the database

            var user = _context.Users
            .Include(u => u.Users_Teams)
            .ThenInclude(ut => ut.Team)
            .FirstOrDefault(u => u.id == userId);
            Console.WriteLine(user);
            // Check if the user is found
            if (user == null)
            {
                // Handle the case where the user is not found (return an empty list or handle as needed)
                return new List<Entities.Team>();
            }
            
            // Extract teams from the join table and return them
            var teams = user.Users_Teams.Select(ut => ut.Team).ToList();
            Console.WriteLine(teams);
            if (teams.Count > 0)
            {
                Console.WriteLine("zit iets in");
            }
            else
            {
                Console.WriteLine("helemaal niks");
            }
            foreach (var task in teams)
            {
                Console.WriteLine($"TaskId: {task.id}");
            }
            return teams;
        }
        public async Task Edit(Entities.User user)
        {
            var users = await _context.Users.FindAsync(user);
            _context.Users.Update(users);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string? id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.id == id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task UserExists(string? id)
        {
            _context.Users.Any(e => e.id == id);
            await _context.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

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

        public async Task UpdateTeam(Guid teamId, string userIdsString)
        {
            if (userIdsString == null)
            {
                userIdsString = ""; // Treat null as an empty string
            }
            var userIds = userIdsString.Split(',').ToList();

            var currentUsers = _context.Users_Teams
          .Where(tu => tu.team_id == teamId)
          .Select(tu => tu.user_id)
          .ToList();
            Console.WriteLine("Current Users:");
            foreach (var user in currentUsers)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Provided Users:");
            foreach (var user in userIds)
            {
                Console.WriteLine(user);
            }
            // Add new user IDs to the team
            var newUsers = userIds.Except(currentUsers).ToList();
            foreach (var userId in newUsers)
            {
                _context.Users_Teams.Add(new Entities.UserTeam { team_id = teamId, user_id = userId });
            }

            int usersDeletedCount = 0; // Track the number of users deleted
            // Remove user IDs not in the provided list
            var removedUsers = currentUsers.Except(userIds).ToList();
            Console.WriteLine($"Number of removed users: {removedUsers.Count}");

            foreach (var userId in removedUsers)
            {
                var teamUser = _context.Users_Teams.FirstOrDefault(tu => tu.team_id == teamId && tu.user_id == userId);
                if (teamUser != null)
                {
                    await DeleteUserFromTeam(userId, teamId);
                    usersDeletedCount++; // Increment the count of deleted users
                }
            }
            if (usersDeletedCount == currentUsers.Count)
            {
                await DeleteAllTasks(teamId);
                await DeleteTeam(teamId);
                Console.WriteLine("All users deleted. Deleting all tasks and team.");
            }
            // Save changes to the database 10d3a1b6-babf-4261-aae8-794a710d675a 102 41
            _context.SaveChanges();
        }
        public async Task<bool> DeleteUserFromTeam(string user_id, Guid team_id)
        {
            // Find the user-team relationship entry based on both user_id and team_id
            var userTeamEntry = await _context.Users_Teams
                .FirstOrDefaultAsync(ut => ut.user_id == user_id && ut.team_id == team_id);
            Console.Write(userTeamEntry);
            // Check if the entry was found
            if (userTeamEntry != null)
            {
                // Remove the found entry
                _context.Users_Teams.Remove(userTeamEntry);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Check if the user is the last one in the team
                int remainingUsersInTeam = await _context.Users_Teams
                    .CountAsync(ut => ut.team_id == team_id);
                Console.WriteLine("Usersleft: " + remainingUsersInTeam);
                // Return whether the user was the last one in the team
                return remainingUsersInTeam == 0;
            }
            // Return false if no entry was found
            return false;
        }
        public async Task DeleteTeam(Guid team_id)
        {
            var team = await _context.Teams.FindAsync(team_id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Entities.Team>> GetUsersByTeam()
        {
            var userId = _currentUserService.userid;
            var usersByTeam = await _context.Users
                .Include(u => u.Users_Teams)
                .ThenInclude(ut => ut.Team)
                .ToListAsync();

            var teams = usersByTeam
                .SelectMany(u => u.Users_Teams.Where(ut => ut.user_id == userId).Select(ut => new { Team = ut.Team, User = u }))
                .GroupBy(ut => ut.Team)
                .Select(g => new Entities.Team
                {
                    id = g.Key.id,
                    tenant_id = g.Key.tenant_id,
                    title = g.Key.title,
                    Users_Teams = g.Key.Users_Teams, // Retain the original Users_Teams navigation property

                })
                .ToList();
            foreach (var team in teams)
            {
                Console.WriteLine($"Team ID: {team.id}");
                Console.WriteLine($"Tenant ID: {team.tenant_id}");
                Console.WriteLine($"Title: {team.title}");

                // Print other properties if needed

                Console.WriteLine("Users in the team:");
                foreach (var userTeam in team.Users_Teams)
                {
                    Console.WriteLine($"User ID: {userTeam.user_id}");
                    // Print other user-related properties if needed
                }

                Console.WriteLine(); // Add a blank line for readability between teams
            }
            return teams;
        }

        public async Task DeleteAllTasks(Guid teamid)
        {
            var tasks = await _context.Task.Where(t => t.team_id == teamid).ToListAsync();
            _context.Task.RemoveRange(tasks);
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
        public async Task EditTeam(Entities.Team team)
        {
            _context.Teams.Update(team);
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
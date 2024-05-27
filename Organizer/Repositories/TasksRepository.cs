using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Models;
using Organizer.Services;


namespace Organizer.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRoleRepository _roleRepository;
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TasksRepository(OrganizerContext context, ICurrentTenantService currentTenantService, ICurrentUserService currentUserService, IRoleRepository roleRepository, ITeamsRepository teamRepository, IUserRepository userRepository)
        {
            _context = context;
            _currentTenantService = currentTenantService;
            _currentUserService = currentUserService;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<Entities.Task>> GetTasksAsync()
        {
            var tenantId = _currentTenantService.tenantid;
            var userid = _currentUserService.userid;
            var userTeams = await _context.Users_Teams
                .Where(ut => ut.user_id == userid) // Filter by the user's ID
                .Select(ut => ut.team_id) // Select the TeamId
                .ToListAsync();

            var task = await _context.Task
                .Where(t => t.tenantid == tenantId) // Filter by tenantId
                .Where(t => t.userid == userid) // Filter by userId
                .Where(t => userTeams.Contains((Guid)t.teamid) || t.teamid == null) // Filter by teams user belongs to, including the specific team, and tasks with null TeamId
                .ToListAsync();


            var tasks = await _context.Task.Where(t => t.tenantid == tenantId).ToListAsync();
            var Users = await _context.Task.Where(t => t.userid == userid).ToListAsync();
            // Debugging: Print the fetched tasks

            return task;
        }
        public async Task<List<Entities.Task>> GetTaskIdsByUser()
        {

            var userId = _currentUserService.userid;
            Console.WriteLine(userId);
            // Find the user in the database
            var Users = await _context.Task.Where(t => t.userid == userId).ToListAsync();

            Console.WriteLine(Users);
            // Check if the user is found
            if (Users == null)
            {
                // Handle the case where the user is not found (return an empty list or handle as needed)
                return new List<Entities.Task>();
            }

            // Extract teams from the join table and return them

            Console.WriteLine(Users);
            if (Users.Count > 0)
            {
                Console.WriteLine("zit iets in");
            }
            else
            {
                Console.WriteLine("helemaal niks");
            }
            foreach (var task in Users)
            {
                Console.WriteLine($"TaskId: {task.id}");
            }
            return Users;
        }

        public async System.Threading.Tasks.Task Create(Entities.Task task)
        {
            task.id = Guid.NewGuid();
            task.tenantid = _currentTenantService.tenantid; // Set tenantid
            _context.Task.Add(task);
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the task
        }

        public async System.Threading.Tasks.Task Edit(Entities.Task task)
        {
            _context.Task.Update(task);
            await _context.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task<ParentViewModel> ParentViewModel(string pageValue)
        {
            ParentViewModel mymodel = new ParentViewModel();
            List<Entities.User> users = await _userRepository.GetUserIdsByTenant();
            List<Entities.User> currentuser = await _userRepository.GetUserInfo();
            List<Entities.Team> teams = await _teamRepository.GetTeamsByUser();
            List<Entities.Team> teamslist = await _teamRepository.GetUsersByTeam();
            List<Entities.Task> tasks = await GetTasksAsync();
            List<Entities.Role> roles = await _roleRepository.GetAllRolesAsync();

            var viewModel = new PageIdentifier();
            viewModel.PageValue = pageValue;
            if(pageValue == "Users")
            {

                var model = new ParentViewModel
                {
                    CurrentUser = currentuser,
                    Users = users,
                    Teams = teams,
                    Tasks = tasks,
                    TeamsList = teamslist,
                    PageIdentifier = viewModel,
                    Roles = roles
                };
                return model;
            }
            else
            {
                var model = new ParentViewModel
                {
                    Users = users,
                    Teams = teams,
                    Tasks = tasks,
                    TeamsList = teamslist,
                    PageIdentifier = viewModel,
                    Roles = roles
                };
                return model;
            }
           
            
        }
        public async System.Threading.Tasks.Task Delete(Guid id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}
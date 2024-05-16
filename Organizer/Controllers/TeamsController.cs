using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;
using Organizer.Entities;


namespace Organizer.Controllers
{

    public class TeamsController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITasksRepository _tasksRepository;

        public TeamsController(ITeamsRepository teamRepository, IUserRepository userRepository, ITasksRepository tasksRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _tasksRepository = tasksRepository;
        }


 
        public async Task<IActionResult> Index()
        {
       
            // Return the view with the model
            return View();
        }


        public async Task<IActionResult> CreateTeam([Bind("title, tenant_id, Users_Teams")] Team team, string user_id)
        {


            if (ModelState.IsValid)
            {
                // Generate a new GUID for the team..
                var guid = Guid.NewGuid();
                team.id = guid;
                string input = user_id;
                List<string> users = input.Split(',').ToList();
                // Add users to the team if user IDs are provided
                if (user_id != null && user_id.Any())
                {

                    foreach (var userId in users)
                    {
                        Console.WriteLine($"Number of user IDs: {users.Count}");

                        // Create a new UserTeam object for each user ID
                        var userTeam = new UserTeam { user_id = userId, team_id = guid };
                        Console.WriteLine(userTeam);
                        team.Users_Teams.Add(userTeam);

                        // Save each userTeam separately
                        await _teamRepository.CreateUserTeam(userTeam);
                        await _teamRepository.SaveChangesAsync();
                    }
                }
                Console.WriteLine(user_id);
                // Save the team to the database
                await _teamRepository.CreateTeam(team);
                await _teamRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Teams));
            }

            return View();
        }
        public async Task<IActionResult> LeaveTeam(string user_id, Guid team_id)
        {
            await _teamRepository.DeleteUserFromTeam(user_id, team_id);
            // Call the DeleteUserFromTeam method and get whether the user was the last one in the team
            bool isLastUser = await _teamRepository.DeleteUserFromTeam(user_id, team_id);
            
            // Save changes
            await _teamRepository.SaveChangesAsync();

            // If the user was the last one in the team, execute another line
            if (isLastUser)
            {
                await _teamRepository.DeleteAllTasks(team_id);
                await _teamRepository.DeleteTeam(team_id);
                Console.WriteLine("testestststsetsts");
            }

            // Redirect to the Index action
            return RedirectToAction(nameof(Index));
        }
  
        public async Task<IActionResult> EditTeam(Guid id, [Bind("id,title,tenant_id,Users_Teams")] Team team)
        {
            if (id != team.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Current tenantid EDIT: {team}");
                    await _teamRepository.EditTeam(team);
                    await _teamRepository.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Teams));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current tenantid EDITtt: {team}");
            return View(team);
        }

        public async Task<IActionResult> Teams()
        {
            var ParentViewModel = await _tasksRepository.ParentViewModel("Teams");

            return View(ParentViewModel);
        }
          public IActionResult teamMultiSelectSlideover()
        {
            return View();
        }
    }
}

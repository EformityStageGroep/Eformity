using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;
using Organizer.Entities;
using Organizer.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace Organizer.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;


        public TeamsController(ITeamsRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            // Fetch data for the view
            ParentViewModel mymodel = new ParentViewModel();
            List<User> users = await _userRepository.GetUserIdsByTenant();
            List<Team> teams = await _teamRepository.GetTeamsByUser();

            // Create the ParentViewModel and populate it with data
            var model = new ParentViewModel
            {
                Users = users,
                Teams = teams
            };

            // Return the view with the model
            return View(model);
        }
        public async Task<IActionResult> GetTeamsByUser()

        {
            try
            {
                var users =  _teamRepository.GetTeamsByUser(); // Fetch tasks based on current tenant




                return View(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<IActionResult> CreateTeam([Bind("title, tenant_id, Users_Teams")] Team team, string user_id)
        {
            if (ModelState.IsValid)
            {
                // Generate a new GUID for the team
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
            }

            return View();
        }
        public async Task<IActionResult> LeaveTeam(string user_id, Guid team_id)
        {
            await _teamRepository.DeleteUserFromTeam(user_id, team_id);
            await _teamRepository.SaveChangesAsync();
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
                    // Handle exception, log, etc.
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
            ParentViewModel mymodel = new ParentViewModel();
            List<User> users = await _userRepository.GetUserIdsByTenant();
            List<Team> teams = await _teamRepository.GetTeamsByUser();

            // Create the ParentViewModel and populate it with data
            var model = new ParentViewModel
            {
                Users = users,
                Teams = teams
            };

            // Return the view with the model
            return View(model);
        }


        [HttpGet("/Teams/GetUserIdsByTeamId/{teamId}")]
        public async Task<IActionResult> GetUserIdsByTeamId(Guid teamId)
        {
            Console.WriteLine(teamId);
            List<Team> Users = await _teamRepository.GetUsersByTeam(teamId);
            Console.WriteLine($"{Users.Count} users");
            return View(Users);
        }
    }
}

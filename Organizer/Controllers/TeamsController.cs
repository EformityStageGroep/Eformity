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
        public async Task<IActionResult> CreateTeam([Bind("user_id,title")] Team team)
        {

            if (ModelState.IsValid)
            {
                //CreateGUID()
                await _teamRepository.CreateTeam(new Team { title = team.title });
                await _teamRepository.SaveChangesAsync();

            }
            return View();
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;
using Organizer.Entities;
using Organizer.Models;
using System.Collections.Generic;

namespace Organizer.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamsRepository _teamRepository;

        public TeamsController(ITeamsRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task<IActionResult> Index()

        {
            // Assuming you have methods to retrieve data from your data source
            List<Organizer.Entities.Team> teams = GetTeamsByUser(); // Replace with your data retrieval logic
            
            // Create an instance of the view model
            TeamUserViewModel viewModel = new TeamUserViewModel
            {
                Teams = teams,
               
            };

            // Pass the view model to the view
            return View(viewModel);
        }
        public async Task <List<Organizer.Entities.Team>> GetTeamsByUser()

        {
            try
            {
                var users = await _teamRepository.GetTeamsByUser(); // Fetch tasks based on current tenant




                return View(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<IActionResult> CreateTeam([Bind("id,tenant_id,fullname,email")] Team team)
        {
            if (ModelState.IsValid)
            {
                await _teamRepository.Create(team);
                await _teamRepository.SaveChangesAsync();

            }
            return View();
        }
        public IActionResult Teams()
        {
            /*var viewModel = new PageIdentifier();
            viewModel.PageValue = "Teams";*/
            return View();
        }
    }
}

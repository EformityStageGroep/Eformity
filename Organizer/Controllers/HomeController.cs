using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Organizer.Models;
using Organizer.Repositories;
using System.Diagnostics;

namespace Organizer.Controllers
{
 
    [Authorize]
    public class HomeController : Controller
    {

        SqlConnection con = new SqlConnection();
       // List<Databron> Databronnen = new List<Databron>();
        private readonly ITasksRepository _tasksRepository;

        public HomeController(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
            con.ConnectionString = Properties.Resources.ConnectionString;
        }
      
        [Authorize(Roles = "SuperAdmin,Tasks")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Homepage()
        {
           var ParentViewModel = await _tasksRepository.ParentViewModel("Homepage");

            // Return the view with the model
            return View(ParentViewModel);
        }

        
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
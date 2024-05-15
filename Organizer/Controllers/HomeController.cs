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
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            con.ConnectionString = Properties.Resources.ConnectionString;
        }
      
        [Authorize(Roles = "SuperAdmin,Employee")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Homepage()
        {
           var ParentViewModel = await _employeeRepository.ParentViewModel("Homepage");

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
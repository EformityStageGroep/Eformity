using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Organizer.Models;
using Organizer.Entities;
using Organizer.Services;
using System.Diagnostics;

namespace Organizer.Controllers
{
 
    [Authorize]
    public class HomeController : Controller
    {
       
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
       // List<Databron> Databronnen = new List<Databron>();
        private readonly ILogger<HomeController> _logger;
        private readonly IGraphClientService _graphClientService;
        private readonly GraphServiceClient graphServiceClient;

        public HomeController(ILogger<HomeController> logger, IGraphClientService graphClientService)
        {
            _logger = logger;
            _graphClientService = graphClientService;
            graphServiceClient = _graphClientService.GetGraphServiceClient();
            con.ConnectionString = Organizer.Properties.Resources.ConnectionString;
        }
        public IActionResult Index()
        {
             if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("Details", "Tasks");
            }
            else if (User.IsInRole("Employee"))
            {
                return View("EmployeeDashboard");
            }
            else
            {
                // Handle other roles or unauthorized access
                return RedirectToAction("Unauthorized", "Error");
            }
            
        }
    
        public IActionResult Teams()
        {
            /*var viewModel = new PageIdentifier();
            viewModel.PageValue = "Teams";*/
            return View();
        }

        public IActionResult Homepage()
        {
           /* var viewModel = new PageIdentifier();
            viewModel.PageValue = "Homepage";*/
            return View();
        }

        public IActionResult Profile()
        {/*
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Profile";*/
            return View();
        }
        
        public IActionResult PostitPage()
        {
            /*var viewModel = new PageIdentifier();
            viewModel.PageValue = "PostitPage";*/
            return View();
        }

        public IActionResult Settings()
        {
           /* var viewModel = new PageIdentifier();
            viewModel.PageValue = "Settings";*/
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
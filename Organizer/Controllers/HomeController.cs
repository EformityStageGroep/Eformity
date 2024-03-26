using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Organizer.Models;
using Organizer.Services;
using System.Diagnostics;

namespace Organizer.Controllers
{
 
    [Authorize]
    public class HomeController : Controller
    {
        public static string _variableToChange = "_sectionHeading";
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
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Index";
            return View(viewModel);
        }
    
        public IActionResult Teams()
        {
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Teams";
            return View(viewModel);
        }

        public IActionResult Homepage()
        {
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Homepage";
            return View(viewModel);
        }

        public IActionResult Profile()
        {
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Profile";
            return View(viewModel);
        }
        
        public IActionResult PostitPage()
        {
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "PostitPage";
            return View(viewModel);
        }

        public IActionResult Settings()
        {
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Settings";
            return View(viewModel);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
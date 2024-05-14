using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Organizer.Contexts;
using Organizer.Models;
using Organizer.Repositories;
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
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _taskRepository;
        private readonly OrganizerContext _context;

        public HomeController(ITeamsRepository teamRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _taskRepository = employeeRepository;
        }
        public HomeController(ILogger<HomeController> logger, IGraphClientService graphClientService)
        {
            _logger = logger;
            _graphClientService = graphClientService;
            graphServiceClient = _graphClientService.GetGraphServiceClient();
            con.ConnectionString = Organizer.Properties.Resources.ConnectionString;
        }

        [Authorize(Roles = "SuperAdmin,Employee")]
        public IActionResult Index()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("EmployeeDashboard", "Employee");
            }
            else if (User.IsInRole("CompanyAdmin"))
            {
                return View("CompanyAdminDashboard");
            }
            else if (User.IsInRole("EmployeeAdmin"))
            {
                return View("EmployeeAdminDashboard");
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



        public async Task<IActionResult> Homepage()
        {


            ParentViewModel mymodel = new ParentViewModel();
            List<Entities.User> users = await _userRepository.GetUserIdsByTenant();
            List<Entities.Team> teams = await _teamRepository.GetTeamsByUser();
            List<Entities.Task> tasks = await _taskRepository.GetTasksAsync();

            var model = new ParentViewModel
            {
                Users = users,
                Teams = teams,
                Tasks = tasks
            };
            return View(model);
        }

        public IActionResult Profile()
        {
/*            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Profile";*/
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
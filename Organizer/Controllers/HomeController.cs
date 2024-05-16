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
        private readonly ITasksRepository _taskRepository;
        private readonly OrganizerContext _context;

        public HomeController(ITeamsRepository teamRepository, IUserRepository userRepository, ITasksRepository tasksRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _taskRepository = tasksRepository;
        }
        public HomeController(ILogger<HomeController> logger, IGraphClientService graphClientService)
        {
            _logger = logger;
            _graphClientService = graphClientService;
            graphServiceClient = _graphClientService.GetGraphServiceClient();
            con.ConnectionString = Organizer.Properties.Resources.ConnectionString;
        }

        [Authorize(Roles = "SuperAdmin,Tasks")]
        public IActionResult Index()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("TasksDashboard", "Tasks");
            }
            else if (User.IsInRole("CompanyAdmin"))
            {
                return View("CompanyAdminDashboard");
            }
            else if (User.IsInRole("TasksAdmin"))
            {
                return View("TasksAdminDashboard");
            }
            else if (User.IsInRole("Tasks"))
            {
                return View("TasksDashboard");
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
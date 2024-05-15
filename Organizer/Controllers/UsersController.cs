using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Entities;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class UsersController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public UsersController(ITeamsRepository teamRepository, IRoleRepository roleRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository, ICurrentTenantService currentTenantService)

        {
            _teamRepository = teamRepository;
            _roleRepository = roleRepository;
            _currentTenantService = currentTenantService;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;

        }
            // GET: Users
        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }
        [HttpPost]
        // GET: Users/Create
        public async Task<IActionResult> Create([Bind("id,tenant_id,fullname,email,Users_Teams,role_id")] User user)
        {
            // Initialize Users_Teams if it's null
            if (user.Users_Teams == null)
            {
                user.Users_Teams = new HashSet<UserTeam>();
            }

            if (ModelState.IsValid)
            {
                await _userRepository.Create(user);
                await _userRepository.SaveChangesAsync();
            }

            // If ModelState is invalid, handle errors
            foreach (var state in ModelState)
            {
                var key = state.Key; // Property name
                var errors = state.Value.Errors; // List of errors for the property

                foreach (var error in errors)
                {
                    // Log the error message or handle it as needed
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }

            // Return the view with the form and error messages
            return RedirectToAction("EmployeeDashboard", "Employee");
        }
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("id,tenant_id,fullname,email")] User user)
        {
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.Edit(user);
                    await _userRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> CompanyAdminDashboard()
        {
            var ParentViewModel = await _employeeRepository.ParentViewModel("Dashboard");

            return View(ParentViewModel);
        }

        public async Task<IActionResult> Teams()
        {
            {
                try
                {
                    var users = await _userRepository.GetUserIdsByTenant(); // Fetch tasks based on current tenant

                    if (users == null || !users.Any())
                    {
                        return View(new List<User>()); // Return an empty list to the view
                    }
                    return View(users);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as required
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
        }
        public async Task<IActionResult> Settings()
        {
            {
                var ParentViewModel = await _employeeRepository.ParentViewModel("Users");


                // Return the view with the model
                return View(ParentViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRole(string userId, Guid roleId)
        {
            try
            {
                // Find the user by ID
                var user = await _userRepository.GetUserById(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Update the user's role ID
                user.role_id = roleId;

                // Save changes to the database
                await _userRepository.SaveChangesAsync();

                return RedirectToAction("Index", "Roles");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

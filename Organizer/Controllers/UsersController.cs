#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;
using System.Threading.Tasks;

namespace Organizer.Views.Shared.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public async Task<IActionResult> Create([Bind("id,tenant_id,fullname,email,Users_Teams")] User user)
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

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            return View();
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }



            return View();
        }

        public IActionResult CompanyAdminDashboard()
        {/*
            var viewModel = new PageIdentifier();
            viewModel.PageValue = "Profile";*/
            return View();
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
                try
                {
                    var users = await _userRepository.GetUserInfo(); // Fetch tasks based on current tenant


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

    }
}

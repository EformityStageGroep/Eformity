#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;
using System.Net.Http.Headers;
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
                var users = await _userRepository.GetTeamsByUser(); // Fetch tasks based on current tenant


               

                return View(users);
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
        public async Task<IActionResult> Create([Bind("Id,TenantID,FullName,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.Create(user);
                await _userRepository.SaveChangesAsync();
             
            }
            return View();
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
        public async Task<IActionResult> Edit(string? id, [Bind("Id,TenantID,FullName,Email")] User user)
        {
            if (id != user.Id)
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
        [Route("api/UserController")]
        [HttpGet("GetUserProfilePicture/{UserId}")]
        public async Task<string> GetUserProfilePicture(string UserId)
        {
            // ConfidentialClientApplicationBuilder should be configured with your app's details
            var app = ConfidentialClientApplicationBuilder.Create("92314824-59b0-4d59-b8fb-b4028c90f7bc")
                .WithClientSecret("2R58Q~NU6TiDr1lZ-opf6nSdmmdBsudA.5BBMbQ-")
                .WithAuthority(new Uri("https://login.microsoftonline.com/common"))
                .Build();

            // Attempt to acquire a token
            var result = await app.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();

            // Setup HTTP client to call Graph API
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // Replace 'UserIdentifier' with 'UserId' to use the method's parameter
            var response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/users/{UserId}/photo/$value");

            if (response.IsSuccessStatusCode)
            {
                // Assuming you want the picture as a base64 string to embed in an HTML img tag
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return Convert.ToBase64String(imageBytes);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                // Handle the 'No Content' scenario
                // Optionally, return a default image's base64 string or a special indicator
                return "No profile picture available"; // Or load a default image if you have one
            }
            else
            {
                // Optionally handle other statuses or log errors
                return null; // Indicate failure or other error condition
            }
        }


    }





}

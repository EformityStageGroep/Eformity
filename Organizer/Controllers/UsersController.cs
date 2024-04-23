#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;
using System.Threading.Tasks;

namespace Organizer.Controllers
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
            return RedirectToAction("CompanyAdminDashboard", "Users");
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
        public async Task<IActionResult> Create([Bind("Id,FullName,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.Create(user);
                await _userRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
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
        public async Task<IActionResult> Edit(string? id, [Bind("Id,FullName,Email")] User user)
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




    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public interface IUserRepository
    {
        User GetById(int id);
        void Create(User user);
        void Update(User user);
        void Delete(int id);
        // Other methods...
    }
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
        // GET: Tasks
        
    }
}

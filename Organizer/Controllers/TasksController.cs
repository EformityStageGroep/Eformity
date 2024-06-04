using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;
using Organizer.Attributes;

namespace Organizer.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITasksRepository _taskRepository;
       
        public TasksController(ITeamsRepository teamRepository, IUserRepository userRepository, ITasksRepository tasksRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _taskRepository = tasksRepository;
        }
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View();
        }
        // GET: Tasks/Details/5
        public async Task<IActionResult> TasksDashboard()
        {
            await _taskRepository.GetTasksAsync();
            var ParentViewModel = await _taskRepository.ParentViewModel("Tasks");

            return View(ParentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireRoleProperty("create_task")]
        public async Task<IActionResult> Create([Bind("id,title,description,priority,datetime,selectstatus,tenant_id,user_id,team_id")] Entities.Task task)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Task ID: {task.id}, Title: {task.title}, Description: {task.description}, Priority: {task.priority},  TeamId: {task.team_id},tenantttt: {task.tenant_id},etc.");
                task.id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                return RedirectToAction(nameof(TasksDashboard));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorr: {modelError.ErrorMessage}");
                }
            }
            return View(task); // Return the same view if ModelState is invalid
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(Guid id, [Bind("id,title,description,priority,datetime,selectstatus,tenant_id,user_id,team_id")] Entities.Task task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Current tenantid EDIT: {task}");
                    await _taskRepository.Edit(task);
                    await _taskRepository.SaveChangesAsync();
                    return RedirectToAction(nameof(TasksDashboard));
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error");
                }
            }
            else
            {
                return View(task);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskRepository.Delete(id);
            await _taskRepository.SaveChangesAsync();
            return RedirectToAction(nameof(TasksDashboard));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using TaskMangementSystem.Models;
using TaskMangementSystem.Repositories;

namespace TaskMangementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private UserModel _user;

        public HomeController(ITaskRepository taskRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            string sessionUserId = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(sessionUserId))
            {
                context.Result = RedirectToAction("Login", "User");
                return;
            }

            if (int.TryParse(sessionUserId, out int userId))
            {
                _user = _userRepository.GetUserByIdAsync(userId).Result;

                if (_user != null)
                {
                    ViewData["username"] = _user.Name;
                }
                else
                {
                    ViewData["username"] = "Guest";
                }
            }
            else
            {
                context.Result = RedirectToAction("Login", "User");
            }
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskRepository.GetTasksByAssignedUserIdAsync(_user.UserID);

            foreach (var task in tasks)
            {
                await _taskRepository.UpdateUserTaskStatusesAsync(_user.UserID); 
            }

            var sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();

            if (sortedTasks.Any())
            {
                return View(sortedTasks); 
            }

           
            ViewData["msg"] = "No tasks are assigned.";
            return View(sortedTasks); 
        }


        // Mark a task as completed
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task != null)
            {
                task.Status = "Completed";
                await _taskRepository.UpdateTask(task);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TodayTasks()
        {
            var tasksDueToday = await _taskRepository.GetTasksDueTodayAsync();
            return View(tasksDueToday);
        }

    }
}

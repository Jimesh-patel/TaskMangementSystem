using Microsoft.AspNetCore.Mvc;
using TaskMangementSystem.Models;
using System.Linq;
using TaskMangementSystem.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskMangementSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private UserModel _user;

        public TaskController(ITaskRepository taskRepository, IProjectRepository projectRepository, IUserRepository userRepository)
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

        [HttpGet]
        public IActionResult AddTask(int projectId)
        {
            ViewBag.ProjectID = projectId; // Pass the ProjectID to the view
            return View(new TaskModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel task, string AssignedUserEmail, int ProjectID)
        {
            
                var assignedUser = await _userRepository.GetUserByEmailAsync(AssignedUserEmail);

                if (assignedUser == null)
                {
                    ViewBag.ErrorMessage = "User with the specified email was not found.";
                    return View();
                }

                if (ProjectID == null)
                {
                    ViewBag.ErrorMessage = "User with the specified Project was not found.";
                    return View(task);
                }

                var project = _projectRepository.GetProjectById(ProjectID);
                task.AssignedUserID = assignedUser.UserID;
                task.ProjectID = ProjectID;
                task.DueDate = project.EndDate;
                task.Status = project.Status;

                await _taskRepository.AddTask(task);

                return RedirectToAction("ProjectDetails", "Project", new { projectId = ProjectID });


        }


    }
}

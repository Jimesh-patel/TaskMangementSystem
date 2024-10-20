using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using System;
using TaskMangementSystem.Models;
using TaskMangementSystem.Repositories;

namespace TaskMangementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private UserModel _user;

        public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _taskRepository = taskRepository;
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
                ViewData["username"] = _user?.Name ?? "Guest";
            }
            else
            {
                context.Result = RedirectToAction("Login", "User");
            }
        }

        // GET: Project/CreateProject
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject(ProjectModel project)
        {
            if (project.EndDate < project.StartDate)
            {
                ViewData["DateError"] = "End date must be greater than or equal to start date.";
                return View(project); 
            }

            if (project.StartDate > DateTime.Today)
            {
                project.Status = "Not Started";
            }
            else if (project.StartDate <= DateTime.Today && project.EndDate >= DateTime.Today)
            {
                project.Status = "In Process";
            }
            else if (project.EndDate < DateTime.Today)
            {
                project.Status = "Missed";
            }

            project.LeaderID = _user.UserID;

            _projectRepository.CreateProject(project);

            return RedirectToAction("UserProjects", "Project");
        }


        // GET: Project/UserProjects
        public async Task<IActionResult> UserProjects()
        {
            var projects = await _projectRepository.GetProjectsByUserIdAsync(_user.UserID);
            foreach (var project in projects)
            {
                await _projectRepository.UpdateProjectStatusAsync(project.ProjectID);
            }
            return View(projects);
        }

        [HttpGet]
        [Route("Project/ProjectDetails/{projectId}")]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {

            await _taskRepository.UpdateProjectTaskStatusesAsync(projectId);
            await _projectRepository.UpdateProjectStatusAsync(projectId);

            var project = _projectRepository.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);

            if (tasks != null && tasks.Any()) 
            {
                var viewModel = new ProjectDetailsViewModel
                {
                    Project = project,
                    Tasks = tasks,
                };
                return View(viewModel);
            }
            else
            {
                
                var viewModel = new ProjectDetailsViewModel
                {
                    Project = project,
                    Tasks = new List<TaskModel>(), 
                };
                return View(viewModel);
            }
        }

    }
}

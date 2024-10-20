using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskRepository _taskRepository;

        public ProjectRepository(ApplicationDbContext context, ITaskRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        // Create a new project
        public void CreateProject(ProjectModel project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        // Get all projects
        public IEnumerable<ProjectModel> GetAllProjects()
        {
            return _context.Projects.ToList();
        }

        // Get project by Id
        public ProjectModel GetProjectById(int projectId)
        {
            return _context.Projects.Find(projectId);
        }
       

        // Delete a project by Id
        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
        }

        // Get projects by user Id (Leader)
        public async Task<List<ProjectModel>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.Projects
                                 .Where(p => p.LeaderID == userId)
                                 .ToListAsync();
        }

        public async Task UpdateProjectStatusAsync(int projectId)
        {
            var project = GetProjectById(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            await _taskRepository.UpdateProjectTaskStatusesAsync(projectId);
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);

            if (tasks.Any())
            {
                bool allTasksCompleted = tasks.All(t => t.Status == "Completed");

                if (allTasksCompleted && project.Status != "Completed")
                {
                    project.Status = "Completed";
                    await UpdateProjectAsync(project);
                }
                else
                {
                    if (project.EndDate < DateTime.Today && project.Status != "Completed")
                    {
                        project.Status = "Missed";
                    }
                    else if (project.StartDate <= DateTime.Today && project.EndDate >= DateTime.Today && project.Status != "Completed")
                    {
                        project.Status = "In Process";
                    }

                    await UpdateProjectAsync(project);
                }
            }
            else
            {
                
                if (project.EndDate < DateTime.Today && project.Status != "Completed")
                {
                    project.Status = "Missed";
                }
                else if (project.StartDate <= DateTime.Today && project.EndDate >= DateTime.Today && project.Status != "Completed")
                {
                    project.Status = "In Process";
                }

                await UpdateProjectAsync(project);
            }
        }



        public async Task UpdateProjectAsync(ProjectModel project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}

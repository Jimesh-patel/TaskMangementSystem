using System.Collections.Generic;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Repositories
{
    public interface IProjectRepository
    {
        void CreateProject(ProjectModel project);
        IEnumerable<ProjectModel> GetAllProjects();
        ProjectModel GetProjectById(int projectId);
        void DeleteProject(int projectId);
        Task<List<ProjectModel>> GetProjectsByUserIdAsync(int userId);
        Task UpdateProjectStatusAsync(int projectId);
        Task UpdateProjectAsync(ProjectModel project);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Repositories
{
    public interface ITaskRepository
    {
        Task AddTask(TaskModel task);
        Task<TaskModel> GetTaskByIdAsync(int taskId);
        Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId);
        Task<IEnumerable<TaskModel>> GetTasksByAssignedUserIdAsync(int userId);
        Task UpdateTask(TaskModel task);
        Task DeleteTask(int taskId);
        Task UpdateUserTaskStatusesAsync(int UsertId);
        Task UpdateProjectTaskStatusesAsync(int projectId);
        Task<IEnumerable<TaskModel>> GetTasksDueTodayAsync();
    }
}

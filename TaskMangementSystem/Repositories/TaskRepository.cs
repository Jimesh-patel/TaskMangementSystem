using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMangementSystem.Models;

namespace TaskMangementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTask(TaskModel task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskModel> GetTaskByIdAsync(int taskId)
        {
            return await _context.Tasks
                .Include(t => t.Project)  
                .Include(t => t.AssignedUser) 
                .FirstOrDefaultAsync(t => t.TaskID == taskId);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)  
                .Where(t => t.ProjectID == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByAssignedUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)  
                .Where(t => t.AssignedUserID == userId)
                .ToListAsync();
        }

        public async Task UpdateTask(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(int taskId)
        {
            var task = await GetTaskByIdAsync(taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserTaskStatusesAsync(int userId)
        {
            var tasks = await GetTasksByAssignedUserIdAsync(userId);

            foreach (var task in tasks)
            {
                if (task.Status != "Completed")
                {
                    if (task.DueDate < DateTime.Today)
                    {
                        task.Status = "Missed"; 
                    }
                    else if (task.DueDate >= DateTime.Today)
                    {
                        task.Status = "In Process";
                    }

                    await UpdateTask(task);
                }
            }
        }

        public async Task UpdateProjectTaskStatusesAsync(int projectId)
        {
            var tasks = await GetTasksByProjectIdAsync(projectId);

            foreach (var task in tasks)
            {
                if (task.Status != "Completed")
                {
                    if (task.DueDate < DateTime.Today)
                    {
                        task.Status = "Missed";
                    }
                    else if (task.DueDate >= DateTime.Today)
                    {
                        task.Status = "In Progress";
                    }

                    await UpdateTask(task);
                }
            }
        }

        public async Task<IEnumerable<TaskModel>> GetTasksDueTodayAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.DueDate.Date == DateTime.Today) 
                .ToListAsync();
        }

    }
}

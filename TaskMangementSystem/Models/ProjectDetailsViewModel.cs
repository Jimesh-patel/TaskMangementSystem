using System.Collections.Generic;

namespace TaskMangementSystem.Models
{
    public class ProjectDetailsViewModel
    {
        public ProjectModel Project { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; }
    }
}

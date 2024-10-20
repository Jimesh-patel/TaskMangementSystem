using System;
using System.ComponentModel.DataAnnotations;

namespace TaskMangementSystem.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        [StringLength(200)]
        public string TaskTitle { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public string Status { get; set; }  

        public int ProjectID { get; set; }
        public ProjectModel Project { get; set; }  

        public int AssignedUserID { get; set; }
        public UserModel AssignedUser { get; set; } 
    }
}

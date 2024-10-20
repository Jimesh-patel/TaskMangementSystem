using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskMangementSystem.Models
{
    public class ProjectModel
    {
        [Key]
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public string ProjectName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        public int LeaderID { get; set; }
        public UserModel Leader { get; set; }  

        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}

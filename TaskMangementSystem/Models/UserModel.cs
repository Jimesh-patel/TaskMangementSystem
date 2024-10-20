using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskMangementSystem.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        public ICollection<TaskModel> AssignedTasks { get; set; } = new List<TaskModel>();
    }
}

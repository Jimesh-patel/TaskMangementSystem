using Microsoft.EntityFrameworkCore;
using TaskMangementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<ProjectModel> Projects { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectModel>()
            .HasOne(p => p.Leader) // A project has one leader
            .WithMany(u => u.Projects) // A user can lead many projects
            .HasForeignKey(p => p.LeaderID)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

        modelBuilder.Entity<TaskModel>()
            .HasOne(t => t.Project) // A task belongs to one project
            .WithMany(p => p.Tasks) // A project has many tasks
            .HasForeignKey(t => t.ProjectID)
            .OnDelete(DeleteBehavior.Cascade); // Cascading delete on project deletion

        modelBuilder.Entity<TaskModel>()
            .HasOne(t => t.AssignedUser) // A task is assigned to one user
            .WithMany(u => u.AssignedTasks) // A user can have many assigned tasks
            .HasForeignKey(t => t.AssignedUserID)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete on user deletion

        base.OnModelCreating(modelBuilder);
    }
}

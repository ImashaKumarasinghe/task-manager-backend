using Microsoft.EntityFrameworkCore;
using mini_task_manager_backend.Models;

namespace mini_task_manager_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
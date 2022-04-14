using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Configurations;
using ToDo.Infrastructure.Entities;

namespace ToDo.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoTaskConfiguration).Assembly);
        }
    }
}

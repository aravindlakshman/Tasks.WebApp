using Microsoft.EntityFrameworkCore;
using Tasks.WebAPI.Datalayer.Entities;

namespace Tasks.WebAPI.Datalayer
{
    /// <summary>
    /// TasksDbContext for API's database context
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class TasksDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TasksDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {

        }
        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public DbSet<GeneralTask> Tasks { get; set; }
    }
}

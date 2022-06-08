using DiffingAPITask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiffingAPITask.Data.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
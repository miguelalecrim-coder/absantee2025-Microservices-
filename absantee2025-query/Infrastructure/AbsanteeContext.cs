using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        
        public virtual DbSet<ProjectDataModel> Projects { get; set; }
        

        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ProjectDataModel>()
                .OwnsOne(a => a.PeriodDate);

           
            base.OnModelCreating(modelBuilder);
        }
    }
}

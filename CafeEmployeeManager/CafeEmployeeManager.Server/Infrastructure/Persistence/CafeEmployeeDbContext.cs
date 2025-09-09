using CafeEmployeeManager.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeEmployeeManager.Server.Infrastructure.Persistence
{
    public class CafeEmployeeDbContext : DbContext
    {
        public CafeEmployeeDbContext(DbContextOptions<CafeEmployeeDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<EmployeeCafe> EmployeeCafe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeCafe>()
                .HasKey(ec => new { ec.EmployeeId, ec.CafeId });

            modelBuilder.Entity<EmployeeCafe>()
                .HasOne(ec => ec.Employee)
                .WithOne(e => e.EmployeeCafe)
                .HasForeignKey<EmployeeCafe>(ec => ec.EmployeeId);

            modelBuilder.Entity<EmployeeCafe>()
                .HasOne(ec => ec.Cafe)
                .WithMany(c => c.EmployeeCafe)
                .HasForeignKey(ec => ec.CafeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
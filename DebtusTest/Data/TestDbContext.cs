using DebtusTest.Model;
using Microsoft.EntityFrameworkCore;

namespace DebtusTest.Data;

public class TestDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    
    public DbSet<Position> Positions { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Position>()
            .Property(p => p.positionName)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.LastName)
            .IsRequired();

        modelBuilder.Entity<Employee>()
            .Property(e => e.FirstName)
            .IsRequired();

        modelBuilder.Entity<Shift>()
            .Property(s => s.StartTime)
            .IsRequired();
    }
}
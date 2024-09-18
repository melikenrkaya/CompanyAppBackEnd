using companyappbasic.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace companyappbasic.Data.Context
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Tasks> Taskss { get; set; }
        public DbSet<AppUser> AppUserss { get; set; }
        public DbSet<LoginLog> LoginLogss { get; set; }
        public DbSet<RecordOfBackgroungJobs> RecordOfBackgroungJobss { get ; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rolleri ekleyin
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole {Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" }
            );
 

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.AssignedEmployee)
                .WithMany(t=>t.AssignedTasks)
                .HasForeignKey(t => t.AssignedToEmployeeId);
        }
    }
}

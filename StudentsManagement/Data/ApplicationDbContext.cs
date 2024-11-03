using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<SystemCodeDetail> SystemCodeDetails { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<BookIssuance> BookIssuances { get; set; }
        public DbSet<AcademicYears> AcademicYears { get; set; }
        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<HostelRoom> HostelRooms { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintNote> ComplaintNotes { get; set; }
        public DbSet<AdminMessage> AdminMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the relationship for CreatedBy in ApplicationRole
            modelBuilder.Entity<ApplicationRole>()
                .HasOne(role => role.CreatedBy)
                .WithMany()
                .HasForeignKey(role => role.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationRole>()
                .HasOne(role => role.ReviewedBy)
                .WithMany()
                .HasForeignKey(role => role.ReviewedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Gender)
                .WithMany()
                .HasForeignKey(u => u.GenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

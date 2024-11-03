using Microsoft.AspNetCore.Identity;
using StudentsManagement.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsManagement.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Photo { get; set; }
        public int GenderId { get; set; }
        public SystemCodeDetail Gender {  get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public bool IsActive { get; set; }
        public DateTime? DeactivatedOn {  get; set; }
        public DateTime LastActivityDate { get; set; }
        public string AccountStatus { get; set; } = "Pending";
        public string? RoleId { get; set; }
        [NotMapped]
        public List<string>? Roles { get; set; }

    }

}

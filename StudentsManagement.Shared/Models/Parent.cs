using StudentsManagement.Shared.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentsManagement.Shared.Models
{
    public class Parent
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]  // Giới hạn độ dài cho tên
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@gmail\.com$", ErrorMessage = "Email must be a valid gmail.com address.")]
        public string EmailAddress { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be 10 digits and start with '0'.")]
        public string PhoneNumber { get; set; }

        [Required]
        public int GenderId { get; set; }

        public SystemCodeDetail Gender { get; set; }

        [Required]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [Required]
        public int ClassId { get; set; }

        public SystemCodeDetail Class { get; set; }

        [Required]
        public int ParentTypeId { get; set; }

        public SystemCodeDetail ParentType { get; set; }

        [Required]
        [ValidAge(18, 100)]  // Ràng buộc tuổi từ 18 đến 100
        public DateTime DOB { get; set; }
    }
}

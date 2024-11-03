using System;
using System.ComponentModel.DataAnnotations;
using StudentsManagement.Shared.Validators;

namespace StudentsManagement.Shared.Models
{
    public class Teacher
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
        public int MaritalStatusId { get; set; }

        public SystemCodeDetail MaritalStatus { get; set; }

        [Required]
        [ValidAge(18, 100)]  // Ràng buộc độ tuổi từ 18 đến 100
        public DateTime DOB { get; set; }

        [Url(ErrorMessage = "Please enter a valid Facebook URL.")]
        public string FacebookLink { get; set; }

        [Url(ErrorMessage = "Please enter a valid Twitter URL.")]
        public string TwitterLink { get; set; }

        [Url(ErrorMessage = "Please enter a valid LinkedIn URL.")]
        public string LinkedInLink { get; set; }

        [Required]
        public int DesignationId { get; set; }

        public SystemCodeDetail Designation { get; set; }
    }
}

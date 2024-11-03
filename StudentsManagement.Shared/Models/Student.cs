using StudentsManagement.Shared.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsManagement.Shared.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^SV\d{4}$", ErrorMessage = "Registration No must be in the format 'SVxxxx' where xxxx are 4 digits.")]
        public string RegistrationNo { get; set; }

        [Required]
        [StringLength(100)]  // Giới hạn độ dài tối đa cho tên
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required]
        [EmailAddress]  // Đảm bảo đây là một địa chỉ email hợp lệ
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@gmail\.com$", ErrorMessage = "Email must be a valid gmail.com address.")]
        public string EmailAddress { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]  // Kiểm tra định dạng số điện thoại
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be 10 digits and start with '0'.")]
        public string PhoneNumber { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Required]
        public int GenderId { get; set; }

        public SystemCodeDetail Gender { get; set; }

        [Required]
        [ValidAge(18, 100)]
        public DateTime DOB { get; set; }  // Ngày sinh
        [Required]
        public int ClassId { get; set; }

        public SystemCodeDetail Class { get; set; }
    }
}

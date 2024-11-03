using StudentsManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Validators
{
    public class ValidBookIssuanceDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bookIssuance = (BookIssuance)validationContext.ObjectInstance;

            // Kiểm tra IssueDate <= DueDate
            if (bookIssuance.IssueDate > bookIssuance.DueDate)
            {
                return new ValidationResult("Issue Date must be less than or equal to Due Date.");
            }

            // Kiểm tra ReturnDate >= DueDate (nếu ReturnDate không null)
            if (bookIssuance.ReturnDate.HasValue && bookIssuance.ReturnDate.Value < bookIssuance.DueDate)
            {
                return new ValidationResult("Return Date must be greater than or equal to Due Date.");
            }

            // Kiểm tra các ngày không vượt quá khoảng thời gian hợp lý (ví dụ, 1 năm)
            if ((bookIssuance.IssueDate - DateTime.Now).Days > 365)
            {
                return new ValidationResult("Issue Date cannot be more than 1 year in the future.");
            }

            if ((DateTime.Now - bookIssuance.IssueDate).Days > (4 * 365))
            {
                return new ValidationResult("Issue Date cannot be more than 4 years in the past.");
            }

            return ValidationResult.Success;
        }
    }
}

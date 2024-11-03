using System;
using System.ComponentModel.DataAnnotations;

namespace StudentsManagement.Shared.Validators
{
    public class ValidAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public ValidAgeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                var age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--; // Điều chỉnh nếu chưa đến ngày sinh nhật trong năm

                if (age < _minAge || age > _maxAge)
                {
                    return new ValidationResult($"Age must be between {_minAge} and {_maxAge} years.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date of birth.");
        }
    }
}

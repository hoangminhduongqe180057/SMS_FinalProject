using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class SearchParameters
    {
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string NoOfCopy { get; set; }
        public string StudentName { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateOn { get; set; }
        // Thuộc tính để lưu trường sắp xếp và thứ tự sắp xếp
        public string? SortField { get; set; } = "RegistrationNo";
        public bool SortAscending { get; set; } = true;
    }
}

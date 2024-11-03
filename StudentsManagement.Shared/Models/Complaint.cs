using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class Complaint : UserCreateActivity
    {
        public int Id { get; set; }
        public int ComplaintTypeId { get; set; }
        public SystemCodeDetail ComplaintType { get; set; }
        public int SourceId { get; set; }
        public SystemCodeDetail Source { get; set; }
        public string ComplaintBy {  get; set; }
        [Phone]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be 10 digits and start with '0'.")]
        public string? PhoneNumber { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public SystemCodeDetail Status { get; set; }
    }
}

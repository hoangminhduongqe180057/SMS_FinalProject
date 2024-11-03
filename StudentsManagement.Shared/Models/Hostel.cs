using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class Hostel : UserCreateActivity
    {
        public int Id { get; set; }
        public string HostelName { get; set; }
        public int HostelTypeId { get; set; }
        public SystemCodeDetail HostelType { get; set; }
        public string address { get; set; }
        public string Description { get; set; }
    }
}

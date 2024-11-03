using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class AcademicYears
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // public int Status { get; set; }
        //public SystemCodeDetail Status {  get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}

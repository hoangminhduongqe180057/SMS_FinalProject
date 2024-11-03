using System;
using System.ComponentModel.DataAnnotations;

namespace StudentsManagement.Shared.Models
{
    public class HostelRoom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [RegularExpression(@"^R\d{3}$", ErrorMessage = "Room number must be in the format 'Rxxx', where 'xxx' are 3 digits.")]
        public string RoomNo { get; set; }

        [Required]
        public int HostelId { get; set; }

        public Hostel Hostel { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        public SystemCodeDetail RoomType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of beds must be greater than 0.")]
        public int NoOfBeds { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost per bed must be greater than 0.")]
        public decimal CostPerBed { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using StudentsManagement.Shared.Validators;

namespace StudentsManagement.Shared.Models
{
    [ValidBookIssuanceDate]
    public class BookIssuance : UserCreateActivity
    {
        public int Id { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public int ClassId { get; set; }

        public SystemCodeDetail Class { get; set; }

        [Required]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public string Notes { get; set; }

        public int StatusId { get; set; }

        public SystemCodeDetail Status { get; set; }
    }
}

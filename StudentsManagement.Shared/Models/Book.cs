using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Book Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Book Author is required.")]
        public string Auther { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of books must be greater than 0.")]
        public string NoOfCopy { get; set; }

        [Required(ErrorMessage = "Please select a book category.")]
        public int BookCateGoryId { get; set; }
        public SystemCodeDetail BookCategory { get; set; }
    }
}

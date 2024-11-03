using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class PaginationModel<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }   // Tổng số mục (items)
        public int CurrentPage { get; set; }  // Trang hiện tại
        public int PageSize { get; set; }     // Kích thước mỗi trang

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}

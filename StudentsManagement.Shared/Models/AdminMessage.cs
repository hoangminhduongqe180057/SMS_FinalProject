using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManagement.Shared.Models
{
    public class AdminMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AdminName { get; set; }  // Tên của admin gửi tin nhắn

        [Required]
        public string Message { get; set; }  // Nội dung tin nhắn

        public DateTime SentAt { get; set; }  // Thời gian gửi tin nhắn
        public bool IsOwnMessage { get; set; }  // Để phân biệt tin nhắn của admin đăng nhập hiện tại
        public string AvatarUrl { get; set; }  // URL ảnh đại diện
        public string AdminRole { get; set; }
        public string AdminEmail { get; set; }
    }

}
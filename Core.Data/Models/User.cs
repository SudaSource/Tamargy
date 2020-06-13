using System;
using Core.Data.Types;

namespace Core.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Serial { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? CreateDate { get; set; }
        public byte[] Img { get; set; }
        public bool IsActive { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public bool SetNewPassword { get; set; }
        public bool IsDeleted { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
    }
}
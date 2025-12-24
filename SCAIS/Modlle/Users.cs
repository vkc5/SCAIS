using System;

namespace SCAIS.Model
{
    internal class User
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; } 
    }
}

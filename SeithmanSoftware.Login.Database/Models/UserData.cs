using System;

namespace SeithmanSoftware.Login.Database.Models
{
    public class UserData
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime LastLogin { get; set; }

        public byte[] PwSalt { get; set; }

        public byte[] PwHash { get; set; }
    }
}

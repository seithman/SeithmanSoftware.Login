using System;

namespace SeithmanSoftware.Login.Database.Models
{
    public class TokenData
    {
        public int Id { get; set; }

        public int Owner { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }
    }
}

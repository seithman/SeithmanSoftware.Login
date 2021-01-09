using System;

namespace SeithmanSoftware.Login.Database.Models
{
    public class CreateTokenRequest
    {
        public int Owner { get; set; }

        public DateTime Expires { get; set; }

        public string Token { get; set; }
    }
}

using System;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class TokenInfo
    {
        public int OwnerId { get; set; }

        public DateTime Expires { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}

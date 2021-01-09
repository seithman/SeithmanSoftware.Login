using System;

namespace SeithmanSoftware.Login.Database.Models
{
    public class GetTokenResponse
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}

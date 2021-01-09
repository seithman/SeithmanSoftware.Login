using System;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}

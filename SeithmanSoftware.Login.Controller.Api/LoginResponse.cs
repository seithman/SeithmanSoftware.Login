using System;

namespace SeithmanSoftware.Login.Controller.Api
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Must include a username or email address for the account to log into.")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Must provide password.")]
        public string Password { get; set; }
    }
}

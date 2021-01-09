using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "You must provide a username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You must provide an email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a password.")]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "You must provide your access token.")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "You must provide a new password.")]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class LogOutRequest
    {
        [Required(ErrorMessage = "You must provide an access token.")]
        public string Token { get; set; }
    }
}

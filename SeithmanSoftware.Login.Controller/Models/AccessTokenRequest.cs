using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class AccessTokenRequest
    {
        [Required(ErrorMessage = "You must provide an access token.")]
        public string AccessToken { get; set; }
    }
}

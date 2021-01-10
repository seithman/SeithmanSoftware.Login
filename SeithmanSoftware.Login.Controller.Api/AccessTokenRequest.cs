using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    public class AccessTokenRequest
    {
        [Required(ErrorMessage = "You must provide an access token.")]
        public string AccessToken { get; set; }
    }
}

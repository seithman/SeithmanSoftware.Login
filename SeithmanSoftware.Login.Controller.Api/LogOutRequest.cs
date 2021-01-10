using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    public class LogOutRequest
    {
        [Required(ErrorMessage = "You must provide an access token.")]
        public string Token { get; set; }
    }
}

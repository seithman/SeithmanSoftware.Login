using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Data for requesting information about an acces token
    /// </summary>
    public class GetTokenInfoRequest
    {
        /// <summary>
        /// The access token to get information about
        /// </summary>
        [Required(ErrorMessage = "You must provide an access token.")]
        public string AccessToken { get; set; }
    }
}

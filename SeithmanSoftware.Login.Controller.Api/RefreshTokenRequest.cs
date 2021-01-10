using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Data for refreshing an acces token
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// The access token to refresh
        /// </summary>
        [Required(ErrorMessage = "You must provide an access token.")]
        public string AccessToken { get; set; }
    }
}

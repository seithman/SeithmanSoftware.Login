using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Request data for logging out of a session
    /// </summary>
    public class LogOutRequest
    {
        /// <summary>
        /// The current access token for the session
        /// </summary>
        [Required(ErrorMessage = "You must provide an access token.")]
        public string Token { get; set; }
    }
}

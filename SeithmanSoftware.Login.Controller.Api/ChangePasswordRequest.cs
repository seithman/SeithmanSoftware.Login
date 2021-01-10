using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Request data to change a password
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// User's current token for this session
        /// </summary>
        [Required(ErrorMessage = "You must provide your access token.")]
        public string AccessToken { get; set; }

        /// <summary>
        /// New password for user account
        /// </summary>
        [Required(ErrorMessage = "You must provide a new password.")]
        public string Password { get; set; }
    }
}

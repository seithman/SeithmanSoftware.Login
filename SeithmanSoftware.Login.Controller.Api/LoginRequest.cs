using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Request data for logging in
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The username or email associated with the account to log into
        /// </summary>
        [Required(ErrorMessage = "Must include a username or email address for the account to log into.")]
        public string UserNameOrEmail { get; set; }

        /// <summary>
        /// The password for the account
        /// </summary>
        [Required(ErrorMessage = "Must provide password.")]
        public string Password { get; set; }
    }
}

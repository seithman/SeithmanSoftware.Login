using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Request data for creating a new user account
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Username for the account
        /// </summary>
        [Required(ErrorMessage = "You must provide a username.")]
        public string UserName { get; set; }

        /// <summary>
        /// Email address associated with the account
        /// </summary>
        [Required(ErrorMessage = "You must provide an email.")]
        public string Email { get; set; }

        /// <summary>
        /// Password for the new account;
        /// </summary>
        [Required(ErrorMessage = "You must provide a password.")]
        public string Password { get; set; }
    }
}

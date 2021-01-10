using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Request data for deleting a user account
    /// </summary>
    public class DeleteUserRequest
    {
        /// <summary>
        /// The ID of the user to delete
        /// </summary>
        [Required(ErrorMessage = "You must provide the ID of the user to delete.")]
        public int IdToDelete { get; set; }

        /// <summary>
        /// The current token for this session
        /// </summary>
        [Required(ErrorMessage = "You must provide your access token.")]
        public string AccessToken { get; set; }
    }
}

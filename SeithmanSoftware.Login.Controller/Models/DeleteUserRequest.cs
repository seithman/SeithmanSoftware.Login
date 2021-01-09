using System.ComponentModel.DataAnnotations;

namespace SeithmanSoftware.Login.Controller.Models
{
    public class DeleteUserRequest
    {
        [Required(ErrorMessage = "You must provide the ID of the user to delete.")]
        public int IdToDelete { get; set; }

        [Required(ErrorMessage = "You must provide your access token.")]
        public string AccessToken { get; set; }
    }
}

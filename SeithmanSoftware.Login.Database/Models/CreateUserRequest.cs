namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// Data sent when requesting to create a new user in the database
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// The username of the new user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email of the new user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The salt used to hash the new user's password
        /// </summary>
        public byte[] PwSalt { get; set; }

        /// <summary>
        /// The salted hash of the new user
        /// </summary>
        public byte[] PwHash { get; set; }
    }
}

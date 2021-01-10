namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// The data to be passed when saving a new password
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// The ID of the user whose password should be updated
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The salt used to hash the password
        /// </summary>
        public byte[] PwSalt { get; set; }

        /// <summary>
        /// The salted hash of the new password
        /// </summary>
        public byte[] PwHash { get; set; }
    }
}

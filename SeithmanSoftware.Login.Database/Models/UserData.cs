using System;

namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// Data retrieved when retrieving a user for the database
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The date and time in UTC that the user logged in
        /// </summary>
        /// <remarks>Not currently implemented</remarks>
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// The salt used to hash the user's password
        /// </summary>
        public byte[] PwSalt { get; set; }

        /// <summary>
        /// The salted hash of the user's password
        /// </summary>
        public byte[] PwHash { get; set; }
    }
}

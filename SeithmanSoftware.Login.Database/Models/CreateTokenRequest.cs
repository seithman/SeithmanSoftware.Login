using System;

namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// Data to pass when requesting to create a token
    /// </summary>
    public class CreateTokenRequest
    {
        /// <summary>
        /// The ID of the user who owns this token
        /// </summary>
        public int Owner { get; set; }

        /// <summary>
        /// The date and time in UTC that the token expires
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// The token value
        /// </summary>
        public string Token { get; set; }
    }
}

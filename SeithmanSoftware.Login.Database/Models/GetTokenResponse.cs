using System;

namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// The response received when a token is successfully retrived
    /// </summary>
    public class GetTokenResponse
    {
        /// <summary>
        /// The ID of the token
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who owns the token
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// The token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The date and time in UTC that the token expires
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// The username of the user who owns the token
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email of the user who owns the token
        /// </summary>
        public string Email { get; set; }
    }
}

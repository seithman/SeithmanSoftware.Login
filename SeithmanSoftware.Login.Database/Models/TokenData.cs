using System;

namespace SeithmanSoftware.Login.Database.Models
{
    /// <summary>
    /// Data retrieved when retrieving a token from the database
    /// </summary>
    public class TokenData
    {
        /// <summary>
        /// ID of the token
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the user who owns the token
        /// </summary>
        public int Owner { get; set; }

        /// <summary>
        /// The token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Date and time in UTC when the token expires
        /// </summary>
        public DateTime Expires { get; set; }
    }
}

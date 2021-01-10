using System;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    /// Response data for a login request
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// The ID of the user account logged into
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The access token for the session
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The date and time in UTC that the acess token expires
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}

using System;

namespace SeithmanSoftware.Login.Controller.Api
{
    /// <summary>
    ///  Response data for getting information about an access token
    /// </summary>
    public class GetTokenInfoResponse
    {
        /// <summary>
        /// The ID of the user who owns this token
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// The date and time in UTC that this token expires
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// The username associated with this token
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email associated with this token
        /// </summary>
        public string Email { get; set; }
    }
}

using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Database
{
    using Models;

    /// <summary>
    /// The interface for interacting with the user databse
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Delete a token from the database
        /// </summary>
        /// <param name="token">The token to delete</param>
        /// <returns>A <see cref="Task"/> object for task synchronization</returns>
        Task DeleteToken(string token);

        /// <summary>
        /// Create a token in the database
        /// </summary>
        /// <param name="createTokenRequest">The token creation data</param>
        /// <returns>A <see cref="Task"/> object for task snchronization</returns>
        Task CreateToken(CreateTokenRequest createTokenRequest);

        /// <summary>
        /// Retrieve a token from the database
        /// </summary>
        /// <param name="token">The value of the token to retrive</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization retrieving the token information</returns>
        Task<GetTokenResponse> GetToken(string token);

        /// <summary>
        /// Change the password for a given user
        /// </summary>
        /// <param name="newPasswordData">The password change request data</param>
        /// <returns>A <see cref="Task"/> object for task snchronization</returns>
        Task ChangePassword(ChangePasswordRequest newPasswordData);

        /// <summary>
        /// Create a new user in the database
        /// </summary>
        /// <param name="newUserData">The new user's information</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the new user's ID</returns>
        Task<UserId> CreateUser(CreateUserRequest newUserData);

        /// <summary>
        /// Delete a user from the database
        /// </summary>
        /// <param name="Id">The ID of the user to delete</param>
        /// <returns>A <see cref="Task"/> object for task snchronization</returns>
        Task DeleteUser(int Id);

        /// <summary>
        /// Search for a user by username or email
        /// </summary>
        /// <param name="usernameOrEmail">The username or email to search on</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the user's details (if found)</returns>
        Task<UserData> GetUserByUserNameOrEmail(string usernameOrEmail);

        /// <summary>
        /// Retrive a user by ID
        /// </summary>
        /// <param name="id">The ID of the user to retrive</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the user's details (if found)</returns>
        Task<UserData> GetUserById(int id);
    }
}

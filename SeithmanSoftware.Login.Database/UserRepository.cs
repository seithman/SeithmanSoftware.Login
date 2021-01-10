using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SeithmanSoftware.Login.Database
{
    using Models;

    /// <summary>
    /// Implementation of the interface to interact with the user database
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Connection string for the database
        /// </summary>
        private readonly string _connectionSTring;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">The configuration interface that contains the connection string for the database</param>
        public UserRepository(IConfiguration configuration)
        {
            _connectionSTring = configuration["ConnectionStrings:UsersDbConnection"];
        }

        /// <summary>
        /// Change a user's password
        /// </summary>
        /// <param name="newPasswordData">The password change request data</param>
        /// <returns>A <see cref="Task"/> object for task synchronization</returns>
        public async Task ChangePassword(ChangePasswordRequest newPasswordData)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.User_ChangePassword @Id = @Id, @PwSalt = @PwSalt, @PwHash = @PwHas", newPasswordData);
        }

        /// <summary>
        /// Create a token in the database
        /// </summary>
        /// <param name="createTokenRequest">Token creation request data</param>
        /// <returns>A <see cref="Task"/> object for task synchronization</returns>
        public async Task CreateToken(CreateTokenRequest createTokenRequest)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.Token_Create @Owner = @Owner, @Expires = @Expires, @Token = @Token", createTokenRequest);
        }

        /// <summary>
        /// Create a user in the database
        /// </summary>
        /// <param name="newUserData">User creation request data</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the created user's ID</returns>
        public async Task<UserId> CreateUser(CreateUserRequest newUserData)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserId>("EXEC dbo.User_Create @UserName = @UserName, @Email = @Email, @PwSalt = @PwSalt, @PwHash = @PwHash", newUserData);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Delete a token from the database
        /// </summary>
        /// <param name="token">The token to delete</param>
        /// <returns>A <see cref="Task"/> object for task synchronization</returns>
        public async Task DeleteToken(string token)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.Token_Delete_ByToken @Token = @Token", new { Token = token });
        }

        /// <summary>
        /// Delete a user from the database
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>A <see cref="Task"/> object for task synchronization</returns>
        public async Task DeleteUser(int id)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.User_Delete @Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Retrieve a token from the database
        /// </summary>
        /// <param name="token">The token to retrive</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the token information</returns>
        public async Task<GetTokenResponse> GetToken(string token)
        {
            using var connection = new SqlConnection(_connectionSTring);
            var getTokenResponse = new Dictionary<int, GetTokenResponse>();
            await connection.OpenAsync();
            var result = await connection.QueryAsync<TokenData, UserData, GetTokenResponse>("EXEC dbo.Token_Get_ByToken @Token = @Token", (t, u) =>
            {
                if (!getTokenResponse.TryGetValue(t.Id, out GetTokenResponse response))
                {
                    response = new GetTokenResponse() { Id = t.Id, OwnerId = t.Owner, Expires = t.Expires, Token = t.Token };
                }
                if (u != null)
                {
                    response.UserName = u.UserName;
                    response.Email = u.Email;
                }

                return response;
            }, new { Token = token }, splitOn: "Id");
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve a user from the database by ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the user's details (if found)</returns>
        public async Task<UserData> GetUserById(int id)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserData>("EXEC dbo.User_Get_ById @Id = @Id", new { Id = id });
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve a user from the database by username or eemail
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user to retrieve</param>
        /// <returns>A <see cref="Task&lt;&g;"/> object for task synchronization and retrieving the user's details (if found)</returns>
        public async Task<UserData> GetUserByUserNameOrEmail(string usernameOrEmail)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserData>("EXEC dbo.User_GetByUserNameOrEmail @UserNameOrEmail = @UserNameOrEmail", new { UserNameOrEmail = usernameOrEmail });
            return result.FirstOrDefault();
        }
    }
}

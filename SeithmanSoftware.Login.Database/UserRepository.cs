using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SeithmanSoftware.Login.Database
{
    using Models;

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionSTring;

        public UserRepository(IConfiguration configuration)
        {
            _connectionSTring = configuration["ConnectionStrings:UsersDbConnection"];
        }

        public async Task ChangePassword(NewPasswordData newPasswordData)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.User_ChangePassword @Id = @Id, @PwSalt = @PwSalt, @PwHash = @PwHas", newPasswordData);
        }

        public async Task CreateToken(CreateTokenRequest createTokenRequest)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.Token_Create @Owner = @Owner, @Expires = @Expires, @Token = @Token", createTokenRequest);
        }

        public async Task<UserId> CreateUser(NewUserData newUserData)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserId>("EXEC dbo.User_Create @UserName = @UserName, @Email = @Email, @PwSalt = @PwSalt, @PwHash = @PwHash", newUserData);
            return result.FirstOrDefault();
        }

        public async Task DeleteToken(string token)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.Token_Delete_ByToken @Token = @Token", new { Token = token });
        }

        public async Task DeleteUser(int id)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            await connection.ExecuteAsync("EXEC dbo.User_Delete @Id = @Id", new { Id = id });
        }

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

        public async Task<UserData> GetUserById(int id)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserData>("EXEC dbo.User_Get_ById @Id = @Id", new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task<UserData> GetUserByUserNameOrEmail(string usernameOrEmail)
        {
            using var connection = new SqlConnection(_connectionSTring);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<UserData>("EXEC dbo.User_GetByUserNameOrEmail @UserNameOrEmail = @UserNameOrEmail", new { UserNameOrEmail = usernameOrEmail });
            return result.FirstOrDefault();
        }
    }
}

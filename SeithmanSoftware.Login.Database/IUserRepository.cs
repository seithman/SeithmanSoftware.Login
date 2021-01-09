using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Database
{
    using Models;
    public interface IUserRepository
    {
        Task DeleteToken(string token);

        Task CreateToken(CreateTokenRequest createTokenRequest);

        Task<GetTokenResponse> GetToken(string token);

        Task ChangePassword(NewPasswordData newPasswordData);

        Task<UserId> CreateUser(NewUserData newUserData);

        Task DeleteUser(int Id);

        Task<UserData> GetUserByUserNameOrEmail(string usernameOrEmail);

        Task<UserData> GetUserById(int id);
    }
}

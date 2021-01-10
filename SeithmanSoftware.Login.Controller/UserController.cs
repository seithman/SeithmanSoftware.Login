using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SeithmanSoftware.Login.Database;
using SeithmanSoftware.Login.Database.Models;

namespace SeithmanSoftware.Login.Controller
{
    using Helpers;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected enum TokenValidationResult
        {
            Ok,
            NoToken,
            TokenExpired
        }

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<TokenInfo>> GetTokenInfo([Required(ErrorMessage = "You must provide an access token")] AccessTokenRequest accessTokenRequest)
        {
            if (accessTokenRequest == null)
            {
                return BadRequest();
            }
            var getTokenResponse = await _userRepository.GetToken(accessTokenRequest.AccessToken);
            var validationResult = await ValidateToken(getTokenResponse);

            if (validationResult != TokenValidationResult.Ok)
            {
                return NotFound();
            }

            return new TokenInfo()
            {
                OwnerId = getTokenResponse.OwnerId,
                Expires = getTokenResponse.Expires,
                Email = getTokenResponse.Email,
                UserName = getTokenResponse.UserName
            };
        }

        [HttpGet("{userNameOrEmail}")]
        public async Task<ActionResult<UserId>> GetUser(string userNameOrEmail)
        {
            UserData user;
            if (int.TryParse(userNameOrEmail, out int id))
            {
                user = await _userRepository.GetUserById(id);
            }
            else
            {
                user = await _userRepository.GetUserByUserNameOrEmail(userNameOrEmail);
            }

            if (user == null)
            {
                return NotFound();
            }

            return new UserId() { Id = user.Id };
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> RefreshToken(AccessTokenRequest accessTokenRequest)
        {
            var getTokenResponse = await _userRepository.GetToken(accessTokenRequest.AccessToken);
            var validationResult = await ValidateToken(getTokenResponse);

            if (validationResult != TokenValidationResult.Ok)
            {
                return Unauthorized();
            }

            var loginResponse = await CreateToken(getTokenResponse.OwnerId);

            await _userRepository.DeleteToken(getTokenResponse.Token);

            return loginResponse;

        }

        // POST api/<UserContoller>/create
        [HttpPost("create")]
        public async Task<ActionResult<UserId>> CreateUser(CreateUserRequest createUserRequest)
        {
            var user = await _userRepository.GetUserByUserNameOrEmail(createUserRequest.UserName);
            if (user != null)
            {
                return Conflict();
            }
            user = await _userRepository.GetUserByUserNameOrEmail(createUserRequest.Email);
            if (user != null)
            {
                return Conflict();
            }

            CryptoHelper.HashNewPassword(createUserRequest.Password, out byte[] pwSalt, out byte[] pwHash);
            var newUserData = new Database.Models.CreateUserRequest()
            {
                UserName = createUserRequest.UserName,
                Email = createUserRequest.Email,
                PwSalt = pwSalt,
                PwHash = pwHash
            };
            var result = await _userRepository.CreateUser(newUserData);
            return new UserId() { Id = result?.Id ?? -1 };
        }

        // POST api/<UserContoller>/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUser(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByUserNameOrEmail(loginRequest.UserNameOrEmail);
            if (user == null)
            {
                return NotFound();
            }
            CryptoHelper.HashPassword(loginRequest.Password, user.PwSalt, out byte[] pwHash);
            if (!Enumerable.SequenceEqual(pwHash, user.PwHash))
            {
                return Unauthorized();
            }

            return await CreateToken(user.Id);
        }

        [HttpPut("password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var token = await _userRepository.GetToken(changePasswordRequest.AccessToken);
            var validationResponse = await ValidateToken(token);
            if (validationResponse != TokenValidationResult.Ok)
            {
                return Unauthorized();
            }

            CryptoHelper.HashNewPassword(changePasswordRequest.Password, out byte[] pwSalt, out byte[] pwHash);

            await _userRepository.ChangePassword(new Database.Models.ChangePasswordRequest() { Id = token.OwnerId, PwHash = pwHash, PwSalt = pwSalt });
            return Ok();
        }

        // DELETE api/<UserController>/logout
        [HttpDelete("logout")]
        public async Task<ActionResult> LogOutUser(LogOutRequest logOutRequest)
        {
            var tokenResponse = await _userRepository.GetToken(logOutRequest.Token);
            if (tokenResponse == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteToken(logOutRequest.Token);
            return Ok();
        }

        // DELETE api/<UserController>
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(DeleteUserRequest deleteUserRequest)
        {
            var deleter = await _userRepository.GetToken(deleteUserRequest.AccessToken);
            var validationResult = await ValidateToken(deleter);
            if ((validationResult != TokenValidationResult.Ok) || (deleter.OwnerId != deleteUserRequest.IdToDelete))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetUserById(deleteUserRequest.IdToDelete);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUser(deleteUserRequest.IdToDelete);

            return Ok();
        }

        protected async Task<LoginResponse> CreateToken(int ownerId)
        {
            var token = CryptoHelper.CreateToken();
            var expires = DateTime.UtcNow.AddHours(1);
            await _userRepository.CreateToken(new CreateTokenRequest()
            {
                Token = token,
                Expires = expires,
                Owner = ownerId
            });

            return new LoginResponse()
            {
                AccessToken = token,
                Expiration = expires,
                UserId = ownerId
            };
        }

        protected async Task<TokenValidationResult> ValidateToken(GetTokenResponse getTokenResponse)
        {
            if (getTokenResponse == null)
            {
                return TokenValidationResult.NoToken;
            }

            if (getTokenResponse.Expires < DateTime.UtcNow)
            {
                await _userRepository.DeleteToken(getTokenResponse.Token);
                return TokenValidationResult.TokenExpired;
            }

            return TokenValidationResult.Ok;
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using SeithmanSoftware.Login.Controller.Api;

namespace SeithmanSoftware.Login.Client
{
    using Json;
    using Events;

    public class UserClient
    {
        string token;
        int userId;
        DateTime tokenExpires;
        readonly string baseUrl;
        readonly JsonSerializerOptions jsonSerializerOptions;

        public EventHandler<UserClientErrorEventArgs> OnError;

        public UserClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
            token = string.Empty;
            userId = -1;
            tokenExpires = DateTime.MinValue;
            jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = new LowerCaseNamingPolicy() };
        }

        public bool LoggedIn => userId != -1;

        public bool TokenExpired => tokenExpires < DateTime.UtcNow;

        public string Token
        {
            get
            {
                if (!string.IsNullOrEmpty(token) && (tokenExpires < DateTime.UtcNow.AddMinutes(5)))
                {
                    RefreshToken().Wait();
                }
                return token;
            }
        }

        public async Task<TokenInfo> GetTokenInfo (string accessToken)
        {
            using var client = GetHttpClient();
            var accessTokenRequest = new AccessTokenRequest() { AccessToken = accessToken };
            var content = GetHttpContent(JsonSerializer.Serialize<AccessTokenRequest>(accessTokenRequest, jsonSerializerOptions));
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/api/user")
            {
                Content = content
            };
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(responseContent, jsonSerializerOptions);
                return tokenInfo;
            }

            DispatchEvent(OnError, new UserClientErrorEventArgs($"Error getting token info: {response.StatusCode} : {responseContent}"));
            return null;
        }

        public async Task<int> GetUser(string userNameOrEmail)
        {
            using var client = GetHttpClient();
            var response = await client.GetAsync($"{baseUrl}/api/user/{WebUtility.UrlEncode(userNameOrEmail)}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userInfo = JsonSerializer.Deserialize<UserIdResponse>(content, jsonSerializerOptions);
                return userInfo?.Id ?? -1;
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                DispatchEvent(OnError, new UserClientErrorEventArgs($"Error getting user: {response.StatusCode} : {content}"));
            }
            return -1;
        }

        public async Task<int>GetUser(int id)
        {
            using var client = GetHttpClient();
            var response = await client.GetAsync($"{baseUrl}/api/user/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userInfo = JsonSerializer.Deserialize<UserIdResponse>(content, jsonSerializerOptions);
                return userInfo?.Id ?? -1;
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                DispatchEvent(OnError, new UserClientErrorEventArgs($"Error getting user: {response.StatusCode} : {content}"));
            }
            return -1;
        }

        public async Task<bool> RefreshToken()
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            var accessTokenRequest = new AccessTokenRequest()
            {
                AccessToken = token
            };
            using var client = GetHttpClient();
            var content = GetHttpContent(JsonSerializer.Serialize<AccessTokenRequest>(accessTokenRequest, jsonSerializerOptions));
            var response = await client.PostAsync($"{baseUrl}/api/user", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, jsonSerializerOptions);
                this.token = loginResponse.AccessToken;
                this.tokenExpires = loginResponse.Expiration;
                this.userId = loginResponse.UserId;
                return true;
            }

            DispatchEvent(OnError, new UserClientErrorEventArgs($"Could not refresh token. {response.StatusCode} : { responseContent }"));
            token = string.Empty;
            tokenExpires = DateTime.MinValue;
            userId = -1;
            return false;
        }

        public async Task<int> CreateUser(string userName, string email, string password)
        {
            var createUserRequest = new CreateUserRequest()
            {
                UserName = userName,
                Email = email,
                Password = password
            };
            using var client = GetHttpClient();
            var requestContent = GetHttpContent(JsonSerializer.Serialize<CreateUserRequest>(createUserRequest, jsonSerializerOptions));
            var response = await client.PostAsync($"{baseUrl}/api/user/create", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userId = JsonSerializer.Deserialize<UserIdResponse>(responseContent, jsonSerializerOptions);
                return userId?.Id ?? -1;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                DispatchEvent(OnError, new UserClientErrorEventArgs("That user already exists."));
            }
            else
            {
                DispatchEvent(OnError, new UserClientErrorEventArgs($"Error creating user: {response.StatusCode} : {responseContent}"));
            }

            return -1;
        }


        public async Task<bool> Login (string userNameOrEmail, string password)
        {
            var loginRequest = new LoginRequest()
            {
                UserNameOrEmail = userNameOrEmail,
                Password = password
            };
            using var client = GetHttpClient();
            var requestContent = GetHttpContent(JsonSerializer.Serialize<LoginRequest>(loginRequest, jsonSerializerOptions));
            var response = await client.PostAsync($"{baseUrl}/api/user/login", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, jsonSerializerOptions);
                userId = loginResponse?.UserId ?? -1;
                token = loginResponse?.AccessToken ?? string.Empty;
                tokenExpires = loginResponse?.Expiration ?? DateTime.MinValue;
                return LoggedIn;
            }

            DispatchEvent(OnError, new UserClientErrorEventArgs($"Error logging in: {response.StatusCode} : {responseContent}"));

            userId = -1;
            token = string.Empty;
            tokenExpires = DateTime.MinValue;
            return false;
        }

        public async Task<bool> ChangePassword(string newPassword)
        {
            if (!LoggedIn)
            {
                return false;
            }

            var changePasswordRequest = new ChangePasswordRequest()
            {
                AccessToken = Token,
                Password = newPassword
            };

            using var client = GetHttpClient();
            var response = await client.PutAsync($"{baseUrl}/api/user/password", GetHttpContent(JsonSerializer.Serialize<ChangePasswordRequest>(changePasswordRequest, jsonSerializerOptions)));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            DispatchEvent(OnError, new UserClientErrorEventArgs($"Error changing password: {response.StatusCode} : {await response.Content.ReadAsStringAsync()}"));
            return false;
        }

        public async Task LogOut()
        {
            if (!LoggedIn)
            {
                return;
            }

            var logOutRequest = new LogOutRequest()
            {
                Token = token
            };

            using var Client = GetHttpClient();
            var requestContent = GetHttpContent(JsonSerializer.Serialize<LogOutRequest>(logOutRequest));
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}/api/user/logout") { Content = requestContent };
            var response = await Client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                DispatchEvent(OnError, new UserClientErrorEventArgs($"Error logging out: {response.StatusCode} : {await response.Content.ReadAsStringAsync()}"));
            }
        }

        public async Task DeleteUser ()
        {
            if (userId == -1)
            {
                return;
            }

            var deleteUserRequest = new DeleteUserRequest()
            {
                IdToDelete = userId,
                AccessToken = token
            };

            using var client = GetHttpClient();
            var requestContent = GetHttpContent(JsonSerializer.Serialize<DeleteUserRequest>(deleteUserRequest, jsonSerializerOptions));
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}/api/user") { Content = requestContent };
            var response = await client.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                DispatchEvent(OnError, new UserClientErrorEventArgs($"Error deleting user: {response.StatusCode} : {responseContent}"));
            }
            
            userId = -1;
            token = string.Empty;
            tokenExpires = DateTime.MinValue;
        }

        protected static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        protected static HttpContent GetHttpContent(string jsonString)
        {
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        protected void DispatchEvent<T> (EventHandler<T> handler, T args) where T: EventArgs
        {
            handler?.Invoke(this, args);
        }
    }
}

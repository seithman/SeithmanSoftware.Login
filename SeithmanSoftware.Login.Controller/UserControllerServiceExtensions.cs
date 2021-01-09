using Microsoft.Extensions.DependencyInjection;
using SeithmanSoftware.Login.Database;

namespace SeithmanSoftware.Login.Controller
{
    public static class UserControllerServiceExtensions
    {
        public static void AddUserControllerServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}

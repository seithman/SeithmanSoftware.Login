using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Database.SqlServer
{
    public static class DependencyInjectionExtensions
    {
        public static void AddUserDatabaseSqlDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}

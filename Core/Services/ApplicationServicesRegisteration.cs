using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(AssemblyReferences).Assembly);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}

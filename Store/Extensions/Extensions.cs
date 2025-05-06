using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Persistence;
using Shared.ErrorModels;
using Domain.Contracts;
using Store.middlewares;
using Microsoft.Extensions.DependencyInjection;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Store.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddBuiltInServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            services.AddSwaggerServices();
            services.AddIdentityServices();

            services.AddInfrastructureServices(configuration);

            services.ConfigureJwtServices(configuration);

            services.AddApplicationServices(configuration);


            services.ConfigureServices();


            return services;

        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {

            services.AddControllers();
  
            return services;

        }

        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services,IConfiguration configuration)
        {

            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer=jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))

                };

            }

            );

            return services;

        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;

        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                        .Select(m => new ValidationError()
                        {
                            Field = m.Key,
                            Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                        });
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;

        }

        public async static Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {

           await app.InitializeDbAsync();

            app.UseGlobalErrorHandling();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            

            return app;
        }

        private async static Task<WebApplication> InitializeDbAsync(this WebApplication app)
        {

            var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();


            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}

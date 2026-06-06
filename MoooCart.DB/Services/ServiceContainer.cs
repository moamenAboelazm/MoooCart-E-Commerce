using Microsoft.EntityFrameworkCore;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoooCart.DB.Base;
using MoooCart.DB.Contexts;
using MoooCart.DB.Models;
using MoooCart.DB.Repositories;
using Microsoft.AspNetCore.Builder;
using MoooCart.lib.Exceptions;
using MoooCart.lib.Base;
using MoooCart.lib.Services;
using MoooCart.id.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Authentication.Repositories;


namespace MoooCart.DB.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInjectionOptionsDB(this IServiceCollection services, IConfiguration conf)
        {
            string MyConnectionStr = "MyConnectionStr";
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conf.GetConnectionString(MyConnectionStr),
                sqlOption =>
                {
                    sqlOption.MigrationsAssembly(typeof(AppDbContext).Assembly);
                    sqlOption.EnableRetryOnFailure();
                }).UseExceptionProcessor(),
                ServiceLifetime.Scoped
                );
            services.AddScoped<IGenericRepository<ClsCategory>, GenericRepository<ClsCategory>>();
            services.AddScoped<IGenericRepository<ClsProduct>, GenericRepository<ClsProduct>>();
            services.AddScoped(typeof(IAppLoger<>), typeof(SerilogerAppAdapter<>));

            services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = conf["JWT:Issuer"],
                    ValidAudience = conf["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["JWT:Key"]!))
                };
            });

            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagement>();
            

            return services;
        }

        public static IApplicationBuilder AddMiddleWareDb(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}

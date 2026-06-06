using FluentValidation;
using FluentValidation.AspNetCore;
using MoooCart.id.Authentication.Base;
using MoooCart.id.Authentication.Repositories;
using MoooCart.id.Validation;
using MoooCart.lib.Base;
using MoooCart.lib.Services;
using MoooCart.MappingProfiles;

namespace MoooCart.Services
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInjectionOptions(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfile<MappingConfig>());
            services.AddScoped<IProductService, ProductServices>();
            services.AddScoped<ICategoryService, CategoryServices>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();

            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

    }
}
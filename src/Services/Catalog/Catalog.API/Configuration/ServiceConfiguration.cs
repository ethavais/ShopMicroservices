using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void ConfigureApiServices(this IServiceCollection services)
        {
            services.AddCarter();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
        }
    }
} 
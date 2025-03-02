using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Database
{
    public static class DatabaseMonitoringExtensions
    {
        public static IApplicationBuilder UseDataBaseMonitoring(
            this IApplicationBuilder app,
            IHostEnvironment environment)
        {
            if (!environment.IsProduction())
            {
                app.UseMiddleware<EnhancedDatabaseMonitoringMiddleware>();
            }
            
            return app;
        }
    }
} 
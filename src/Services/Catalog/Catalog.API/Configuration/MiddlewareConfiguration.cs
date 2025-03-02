using BuildingBlocks.Database;

namespace Catalog.API.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(
            this WebApplication app,
            IHostEnvironment environment)
        {
            app.MapCarter();
            
            // Sử dụng middleware giám sát từ BuildingBlocks
            app.UseDataBaseMonitoring(environment);
        }
    }
} 
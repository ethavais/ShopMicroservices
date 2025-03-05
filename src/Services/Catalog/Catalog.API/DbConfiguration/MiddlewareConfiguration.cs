namespace Catalog.API.DbConfiguration
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(
            this WebApplication app,
            IHostEnvironment environment)
        {
            app.MapCarter();
            
            app.UseDataBaseMonitoring(environment);
        }
    }
} 
using BuildingBlocks.Database;

namespace Catalog.API.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            // Sử dụng cấu hình Marten từ BuildingBlocks
            services.AddOptimizedMarten(
                configuration.GetConnectionString("DB")!,
                environment,
                options =>
                {
                    // Cấu hình bổ sung đặc biệt cho Catalog API
                    options.Schema.For<Product>().Index(x => x.Id);
                    options.Schema.For<Product>().Index(x => x.Name);
                    
                    // Thêm Listener cho Catalog API
                    options.Listeners.Add(new MartenDiagnostics());
                });
        }
    }
} 
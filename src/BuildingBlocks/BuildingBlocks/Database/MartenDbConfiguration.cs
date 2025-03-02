using System;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weasel.Core;
using Marten.Services;

namespace BuildingBlocks.Database
{
    public static class MartenDbConfiguration
    {
        public static IServiceCollection AddOptimizedMarten(
            this IServiceCollection services,
            string connectionString, 
            IHostEnvironment environment,
            Action<StoreOptions>? additionalConfiguration = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }

            services.AddMarten(sp =>
            {
                var opts = new StoreOptions();
                opts.Connection(connectionString);
                
                // 1. Tối ưu hóa Schema Migration dựa trên môi trường
                if (environment.IsProduction())
                {
                    opts.AutoCreateSchemaObjects = AutoCreate.None;
                }
                else
                {
                    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                }
                
                // 2. Tối ưu Document Storage với System.Text.Json
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = null, // Avoid camelCase transformation
                    WriteIndented = false // Minimize JSON size
                };
                opts.Serializer(new SystemTextJsonSerializer(jsonOptions));
                // 3. Tối ưu hóa Command Timeout
                opts.CommandTimeout = 30;
                
                // 4. Session Resource Stripping đã được bật mặc định với UseLightweightSessions()
                
                // 5. Tắt PLV8 nếu không cần thiết
                // opts.PLV8Enabled = false;
                
                // 6. Bật Diagnostics trong môi trường phi Production
                if (!environment.IsProduction())
                {
                    //opts.EnableDiagnostics = true;
                    opts.DisableNpgsqlLogging = true;

                }

                // Cho phép cấu hình bổ sung từ service cụ thể
                additionalConfiguration?.Invoke(opts);

                return opts;
            })
            .UseLightweightSessions()
            .OptimizeArtifactWorkflow();
            
            return services;
        }
    }
} 
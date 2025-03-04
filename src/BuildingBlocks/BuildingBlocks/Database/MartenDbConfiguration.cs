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

                // 1. Optimz Schema Migration base on env
                if (environment.IsProduction())
                    opts.AutoCreateSchemaObjects = AutoCreate.None;
                else
                    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

                // 2. Optimz Document Storage with System.Text.Json
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = null, // Avoid camelCase transformation
                    WriteIndented = false // Minimize JSON size
                };
                opts.Serializer(new SystemTextJsonSerializer(jsonOptions));
                // 3. Optimz Command Timeout
                opts.CommandTimeout = 30;
                
                if (!environment.IsProduction())
                    opts.DisableNpgsqlLogging = true;

                // Allow config from specific Service
                additionalConfiguration?.Invoke(opts);

                return opts;
            })
            .UseLightweightSessions()
            .OptimizeArtifactWorkflow();
            
            return services;
        }
    }
} 
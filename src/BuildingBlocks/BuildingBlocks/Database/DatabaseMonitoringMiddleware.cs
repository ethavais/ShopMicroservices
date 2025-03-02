using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Database
{
    public class DatabaseMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        
        public DatabaseMonitoringMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            await _next(context);
            
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 500)
            {
                // Log slow requests
                Console.WriteLine($"Slow request: {context.Request.Path} took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }

    public class EnhancedDatabaseMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, List<long>> _requestTimings = new();
        private static readonly object _lock = new();
        
        // Thresholds for request classification
        private const int SLOW_REQUEST_THRESHOLD_MS = 500;
        private const int VERY_SLOW_REQUEST_THRESHOLD_MS = 1000;
        
        public EnhancedDatabaseMonitoringMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            var method = context.Request.Method;
            var endpoint = $"{method} {path}";
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            try
            {
                // Continue with the next middleware
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMs = stopwatch.ElapsedMilliseconds;
                
                // Store timing for statistical analysis
                lock (_lock)
                {
                    if (!_requestTimings.ContainsKey(endpoint))
                    {
                        _requestTimings[endpoint] = new List<long>();
                    }
                    
                    _requestTimings[endpoint].Add(elapsedMs);
                    
                    // Keep only the last 100 requests per endpoint to avoid memory issues
                    if (_requestTimings[endpoint].Count > 100)
                    {
                        _requestTimings[endpoint].RemoveAt(0);
                    }
                }
                
                // Log based on thresholds
                if (elapsedMs > VERY_SLOW_REQUEST_THRESHOLD_MS)
                {
                    Console.WriteLine($"WARNING - VERY SLOW REQUEST: {endpoint} took {elapsedMs}ms");
                    
                    // Calculate stats for this endpoint
                    if (_requestTimings[endpoint].Count > 1)
                    {
                        var avg = _requestTimings[endpoint].Average();
                        var min = _requestTimings[endpoint].Min();
                        var max = _requestTimings[endpoint].Max();
                        Console.WriteLine($"Stats for {endpoint}: Avg={avg:F2}ms, Min={min}ms, Max={max}ms, Count={_requestTimings[endpoint].Count}");
                    }
                }
                else if (elapsedMs > SLOW_REQUEST_THRESHOLD_MS)
                {
                    Console.WriteLine($"Slow request: {endpoint} took {elapsedMs}ms");
                }
                
                // Special monitoring for /products endpoint
                if (path.Contains("/products"))
                {
                    Console.WriteLine($"Products endpoint performance: {method} {path} took {elapsedMs}ms, Status: {context.Response.StatusCode}");
                }
            }
        }
    }
} 
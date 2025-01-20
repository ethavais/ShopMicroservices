var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder, builder.Services);

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder, IServiceCollection services)
{
    services.AddCarter();

    services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });

    services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("DB")!);
    }).UseLightweightSessions();
}

void ConfigureMiddleware(WebApplication app)
{
    app.MapCarter();
}

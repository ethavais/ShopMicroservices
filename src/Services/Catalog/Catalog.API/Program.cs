using Catalog.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Sử dụng các extension methods từ các lớp cấu hình
builder.Services.ConfigureApiServices();
builder.Services.ConfigureDatabaseServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureMiddleware(builder.Environment);

app.Run();
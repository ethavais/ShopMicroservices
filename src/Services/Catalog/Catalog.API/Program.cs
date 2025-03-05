using Catalog.API.DbConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Sử dụng các extension methods từ các lớp cấu hình
builder.Services.ConfigureApiServices();
builder.Services.ConfigureDatabaseServices(builder.Configuration, builder.Environment);
builder.Services.AddInjectableLogger();

//TypeAdapterConfig.GlobalSettings.Default
//    .IgnoreMember((member, side) => member.Name.StartsWith("EqualityContract"));

var app = builder.Build();

app.ConfigureMiddleware(builder.Environment);

app.Run();



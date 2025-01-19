var builder = WebApplication.CreateBuilder(args);



#region Add Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
#endregion



var app = builder.Build();



#region Config HTTP request
app.MapCarter();
#endregion



app.Run();

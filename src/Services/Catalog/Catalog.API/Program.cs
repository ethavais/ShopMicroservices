var builder = WebApplication.CreateBuilder(args);

//Add Services to container 




var app = builder.Build();



// Config HTTP request
app.Run();

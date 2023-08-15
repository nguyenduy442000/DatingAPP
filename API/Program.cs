using API.Extensions;
using API.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add extensions file  "ApplicationServiceExtensions.css"
builder.Services.AddApplicationServices(builder.Configuration);

// Add extensions  file "IdentityServiceExtensions.cs"
builder.Services.AddIdentityServices(builder.Configuration);




var app = builder.Build();

// override response from  Development to Production
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

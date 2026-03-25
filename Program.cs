var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello from .NET Core 🚀");
app.MapGet("/add", (int a, int b) => a + b);

app.Run();

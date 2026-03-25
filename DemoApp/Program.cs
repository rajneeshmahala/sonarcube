using DemoApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ── Health check endpoint (used by Docker HEALTHCHECK) ──
app.MapGet("/health", () => Results.Ok(new
{
    status  = "healthy",
    app     = "DemoApp",
    version = "1.0.0",
    time    = DateTime.UtcNow
}));

app.Run();

// Needed for integration test project visibility
public partial class Program { }

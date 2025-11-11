var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGRPCMappings();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);
builder.Services.AddGRPCServerServices(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowCredentials()
              .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddUserSecrets<Program>();

var app = builder.Build();

app.AddApplicationServicesUsage();
app.AddGRPCServerServicesUsage();

app.UseRouting();

app.UseCors();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.Run();
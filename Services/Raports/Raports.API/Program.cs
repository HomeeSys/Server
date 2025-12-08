var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(policy =>
    {
        var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>();

        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>();

var app = builder.Build();

app.UseApplicationServices();

app.UseRouting();

app.UseCors();

app.MapHealthChecks("/raports/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.Run();

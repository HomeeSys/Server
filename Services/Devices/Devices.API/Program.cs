var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);
builder.Services.AddGRPCServerServices(builder.Configuration);

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

var app = builder.Build();

app.AddApplicationServicesUsage();
app.AddGRPCServerServicesUsage();

app.UseRouting();
app.UseCors();

app.MapHealthChecks("/devices/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.Run();

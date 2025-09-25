var builder = WebApplication.CreateBuilder(args);

//  We are going to be using Dependancy Injection to inject necessary dependacies from/to projects
//  related to `Devices`
builder.Services.AddInfrastructureServices(builder.Configuration).AddApplicationServices();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DevicesDB")!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.AddApplicationServicesUsage();

app.UseRouting();

app.UseCors("AllowReactApp");

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.Run();

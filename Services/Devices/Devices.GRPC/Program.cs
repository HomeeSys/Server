using CommonServiceLibrary.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapGrpcService<GrpcDevicesService>();
app.UseExceptionHandler(x => { });

app.Run();
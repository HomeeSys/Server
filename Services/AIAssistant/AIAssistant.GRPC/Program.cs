var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddScoped<OllamaRaportChat>();

var app = builder.Build();

app.MapGrpcService<AiResponseService>();

app.Run();

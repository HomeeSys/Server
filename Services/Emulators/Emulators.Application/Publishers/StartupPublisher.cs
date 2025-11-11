namespace Emulators.Application.Publishers;

internal class StartupPublisher(IServiceScopeFactory scopeFactory) : IHostedLifecycleService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        await publisher.Publish(new ActivateDevices(), cancellationToken);
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
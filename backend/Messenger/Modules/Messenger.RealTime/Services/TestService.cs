using Microsoft.Extensions.Hosting;

namespace Messenger.RealTime.Services;

public interface ITwitterPoller
{
    public Task<string?> PollUpdates();
}

public class TestService : BackgroundService
{
    private readonly ITwitterPoller _twitterPoller;

    public TestService(ITwitterPoller twitterPoller)
    {
        _twitterPoller = twitterPoller;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = await _twitterPoller.PollUpdates();
            if (data != null)
            {
                // Do actions
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}

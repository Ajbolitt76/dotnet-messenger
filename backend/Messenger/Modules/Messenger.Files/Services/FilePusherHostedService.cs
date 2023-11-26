using MediatR;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Core.Requests.Abstractions;
using Messenger.Files.Features;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Files.Services;

public class FilePusherHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FilePusherHostedService> _logger;

    public FilePusherHostedService(IServiceProvider serviceProvider, ILogger<FilePusherHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            try
            {
                await LoopHandler(stoppingToken);
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "Unknown error while pushing files");
            }
        }
    }

    private async Task LoopHandler(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var mediator = sp.GetRequiredService<IMediator>();
        var dbContext = sp.GetRequiredService<IDbContext>();

        var file = await dbContext.Files.FromSql(
                $"""
    select * from "Files"
    WHERE ("FileLocation" ->> '$type') = {TusFileLocation.Discriminator}
    limit 1
""")
            .FirstOrDefaultAsync(cancellationToken: stoppingToken);

        if (file is null)
        {
            _logger.LogInformation("No files to push");
            return;
        }

        try
        {
            await mediator.Send(new PushToS3Command(file.Id), stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while pushing file {FileId}", file.Id);
            throw;
        }
        
        
        _logger.LogInformation("Pushed file to s3. FileId = {FileId}", file.Id);
    }
}

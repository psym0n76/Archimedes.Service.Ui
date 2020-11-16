using System;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Library.Domain;
using Archimedes.Library.Message.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Archimedes.Service.Ui
{
    public class MarketSubscriptionService : IHostedService
    {
        private readonly ILogger<MarketSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<MarketHub> _context;
        private readonly Config _config;

        public MarketSubscriptionService(ILogger<MarketSubscriptionService> logger,
            IHubContext<MarketHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.RepositoryUrl}hubs/market")
                .Build();

            _connection.On<MarketDto>("Update", metric => { Update(metric); });
            _connection.On<MarketDto>("Add", metric => { Add(metric); });
            _connection.On<MarketDto>("Delete", metric => { Delete(metric); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Repo hub {_config.RepositoryUrl}hubs/market");

            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Error from connection start: {e.Message}");
                    await Task.Delay(10000, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _connection.DisposeAsync();
        }

        public Task Add(MarketDto metric)
        {
            _context.Clients.All.SendAsync("Add", metric);
            return Task.CompletedTask;
        }

        public Task Delete(MarketDto metric)
        {
            _context.Clients.All.SendAsync("Delete", metric);
            return Task.CompletedTask;
        }

        public Task Update(MarketDto metric)
        {
            _logger.LogInformation($"Update received from one of the metric apis {metric}");
            _context.Clients.All.SendAsync("Update", metric);
            return Task.CompletedTask;
        }
    }
}

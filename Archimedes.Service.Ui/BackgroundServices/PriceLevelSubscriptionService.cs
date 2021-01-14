using System;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Library.Domain;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Archimedes.Service.Ui
{
    public class PriceLevelSubscriptionService : IHostedService
    {
        private readonly ILogger<PriceLevelSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<PriceLevelHub> _context;
        private readonly Config _config;

        public PriceLevelSubscriptionService(ILogger<PriceLevelSubscriptionService> logger,
            IHubContext<PriceLevelHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.TradeUrl}hubs/price-level")
                .Build();

            _connection.On<PriceLevelDto>("Update", level => { Update(level); });
            _connection.On<PriceLevelDto>("Add", level => { Add(level); });
            _connection.On<PriceLevelDto>("Delete", level => { Delete(level); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise PriceLevel hub {_config.TradeUrl}/hubs/price-level");

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

        public Task Add(PriceLevelDto level)
        {
            _context.Clients.All.SendAsync("Add", level);
            return Task.CompletedTask;
        }

        public Task Delete(PriceLevelDto level)
        {
            _context.Clients.All.SendAsync("Delete", level);
            return Task.CompletedTask;
        }

        public Task Update(PriceLevelDto level)
        {
            _logger.LogInformation($"Update received from TradeService  {level}");
            _context.Clients.All.SendAsync("Update", level);
            return Task.CompletedTask;
        }
    }
}

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
    public class TradeSubscriptionService : IHostedService
    {
        private readonly ILogger<TradeSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<TradeHub> _context;
        private readonly Config _config;

        public TradeSubscriptionService(ILogger<TradeSubscriptionService> logger,
            IHubContext<TradeHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.TradeUrl}hubs/trade")
                .Build();

            _connection.On<TradeDto>("Update", trade => { Update(trade); });
            _connection.On<TradeDto>("Add", trade => { Add(trade); });
            _connection.On<TradeDto>("Delete", trade => { Delete(trade); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Trade hub {_config.TradeUrl}/hubs/trade");

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

        public Task Add(TradeDto level)
        {
            _context.Clients.All.SendAsync("Add", level);
            return Task.CompletedTask;
        }

        public Task Delete(TradeDto level)
        {
            _context.Clients.All.SendAsync("Delete", level);
            return Task.CompletedTask;
        }

        public Task Update(TradeDto level)
        {
            _logger.LogInformation($"Update received from TradeService  {level}");
            _context.Clients.All.SendAsync("Update", level);
            return Task.CompletedTask;
        }
    }
}

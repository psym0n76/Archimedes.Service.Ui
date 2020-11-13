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
    public class StrategySubscriptionService : IHostedService
    {
        private readonly ILogger<StrategySubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<StrategyHub> _context;
        private readonly Config _config;

        public StrategySubscriptionService(ILogger<StrategySubscriptionService> logger,
            IHubContext<StrategyHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.StrategyUrl}hubs/strategy")
                .Build();

            _connection.On<StrategyDto>("Update", strategy => { Update(strategy); });
            _connection.On<StrategyDto>("Add", strategy => { Add(strategy); });
            _connection.On<StrategyDto>("Delete", strategy => { Delete(strategy); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Strategy hub {_config.StrategyUrl}hubs/strategy");

            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);

                    break;
                }
                catch
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.DisposeAsync();
            return Task.CompletedTask;
        }

        public Task Add(StrategyDto strategy)
        {
            _context.Clients.All.SendAsync("Add", strategy);
            return Task.CompletedTask;
        }

        public Task Delete(StrategyDto strategy)
        {
            _context.Clients.All.SendAsync("Delete", strategy);
            return Task.CompletedTask;
        }

        public Task Update(StrategyDto strategy)
        {
            _logger.LogInformation($"Update received from one of the strategy apis {strategy}");
            _context.Clients.All.SendAsync("Update", strategy);
            return Task.CompletedTask;
        }
    }
}

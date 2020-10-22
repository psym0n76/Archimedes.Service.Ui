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
    public class HealthSubscriptionService : IHostedService
    {
        private readonly ILogger<HealthSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<HealthHub> _context;
        private readonly Config _config;

        public HealthSubscriptionService(ILogger<HealthSubscriptionService> logger,
            IHubContext<HealthHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.HealthUrl}Hubs/Health")
                .Build();

            _connection.On<HealthMonitorDto>("Update", health => { Update(health); });
            _connection.On<HealthMonitorDto>("Add", health => { Add(health); });
            _connection.On<HealthMonitorDto>("Delete", health => { Delete(health); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Health hub {_config.HealthUrl}Hubs/Health");

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
            return _connection.DisposeAsync();
        }

        public Task Add(HealthMonitorDto health)
        {
            _context.Clients.All.SendAsync("Add", health);
            return Task.CompletedTask;
        }

        public Task Delete(HealthMonitorDto health)
        {
            _context.Clients.All.SendAsync("Delete", health);
            return Task.CompletedTask;
        }

        public Task Update(HealthMonitorDto health)
        {
            //_logger.LogInformation("Update reveived from one of the health apis");
            _context.Clients.All.SendAsync("Update", health);
            return Task.CompletedTask;
        }
    }
}

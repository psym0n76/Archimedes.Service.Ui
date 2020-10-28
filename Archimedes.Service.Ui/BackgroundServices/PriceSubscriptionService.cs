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
    public class PriceSubscriptionService : IHostedService
    {
        private readonly ILogger<PriceSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<PriceHub> _context;
        private readonly Config _config;

        public PriceSubscriptionService(ILogger<PriceSubscriptionService> logger,
            IHubContext<PriceHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.RepositoryUrl}hubs/price")
                .Build();

            _connection.On<PriceDto>("Update", price => { Update(price); });
            _connection.On<PriceDto>("Add", price => { Add(price); });
            _connection.On<PriceDto>("Delete", price => { Delete(price); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Price hub {_config.RepositoryUrl}hubs/price");

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

        public Task Add(PriceDto price)
        {
            _context.Clients.All.SendAsync("Add", price);
            return Task.CompletedTask;
        }

        public Task Delete(PriceDto price)
        {
            _context.Clients.All.SendAsync("Delete", price);
            return Task.CompletedTask;
        }

        public Task Update(PriceDto price)
        {
            //_logger.LogInformation($"Update received from one of the strategy apis {price}");
            _context.Clients.All.SendAsync("Update", price);
            return Task.CompletedTask;
        }
    }
}

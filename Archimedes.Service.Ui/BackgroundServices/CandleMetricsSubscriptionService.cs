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
    public class CandleMetricsSubscriptionService : IHostedService
    {
        private readonly ILogger<CandleMetricsSubscriptionService> _logger;
        private readonly HubConnection _connection;
        private readonly IHubContext<CandleMetricHub> _context;
        private readonly Config _config;

        public CandleMetricsSubscriptionService(ILogger<CandleMetricsSubscriptionService> logger,
            IHubContext<CandleMetricHub> context, IOptions<Config> config)
        {
            _logger = logger;
            _context = context;
            _config = config.Value;
            _connection = new HubConnectionBuilder().WithUrl($"{config.Value.RepositoryUrl}hubs/candle-metric")
                .Build();

            _connection.On<CandleMetricDto>("Update", metric => { Update(metric); });
            _connection.On<CandleMetricDto>("Add", metric => { Add(metric); });
            _connection.On<CandleMetricDto>("Delete", metric => { Delete(metric); });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Initialise Repo hub {_config.RepositoryUrl}hubs/candle-metric");

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

        public Task Add(CandleMetricDto metric)
        {
            _context.Clients.All.SendAsync("Add", metric);
            return Task.CompletedTask;
        }

        public Task Delete(CandleMetricDto metric)
        {
            _context.Clients.All.SendAsync("Delete", metric);
            return Task.CompletedTask;
        }

        public Task Update(CandleMetricDto metric)
        {
            _logger.LogInformation($"Update received from one of the metric apis {metric}");
            _context.Clients.All.SendAsync("Update", metric);
            return Task.CompletedTask;
        }
    }
}

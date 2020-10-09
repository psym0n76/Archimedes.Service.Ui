using System.Threading;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui
{
    public class HealthSubscriptionService : BackgroundService
    {
        private readonly IHubContext<HealthHub> _context;
        private readonly ILogger<HealthSubscriptionService> _logger;


        public HealthSubscriptionService(IHubContext<HealthHub> context, ILogger<HealthSubscriptionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // experimental trying to piggy back the update from the health service to the UI.Service  to the UI

            //    "HealthUrl": "http://health-service.dev.archimedes.com/"

            using (var hubConnection = new HubConnection("http://health-service.dev.archimedes.com/Hubs/Health"))
            {
                var hubProxy = hubConnection.CreateHubProxy("Health");

                hubProxy.On<HealthMonitorDto>("Update", health =>
                {
                    _logger.LogInformation($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Update", health, stoppingToken);
                });

                hubProxy.On<HealthMonitorDto>("Add", health =>
                {
                    _logger.LogInformation($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Add", health, stoppingToken);
                });

                hubProxy.On<HealthMonitorDto>("Delete", health =>
                {
                    _logger.LogInformation($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Delete", health, stoppingToken);
                });

                await hubConnection.Start();
            }
        }
    }
}

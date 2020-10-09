using System;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Archimedes.Service.Ui
{
    public class HealthSubscriptionService : BackgroundService
    {
        private readonly IHubContext<HealthHub> _context;

        public HealthSubscriptionService(IHubContext<HealthHub> context)
        {
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // experimental trying to piggy back the update from the health service to the UI.Service  to the UI

            //    "HealthUrl": "http://health-service.dev.archimedes.com/"

            using (var hubConnection = new HubConnection("http://health-service.dev.archimedes.com/Hubs/Health")) 
            {
                var hubProxy = hubConnection.CreateHubProxy("HealthHub");

                hubProxy.On<HealthMonitorDto>("Update", health =>
                {
                    Console.WriteLine($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Update", health,stoppingToken);
                });

                hubProxy.On<HealthMonitorDto>("Add", health =>
                {
                    Console.WriteLine($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Add", health,stoppingToken);
                });

                hubProxy.On<HealthMonitorDto>("Delete", health =>
                {
                    Console.WriteLine($"Received update from health service {health.AppName}");
                    _context.Clients.All.SendAsync("Delete", health,stoppingToken);
                });

                await hubConnection.Start();
            }
        }
    }
}

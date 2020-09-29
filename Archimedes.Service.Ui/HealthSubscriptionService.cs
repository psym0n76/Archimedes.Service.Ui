using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace Archimedes.Service.Ui
{
    public class HealthSubscriptionService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            //subscribe to the health hub 

            using (var hubConnection = new HubConnection("http://health-service.dev.archimedes.com/Hubs/Health")) 
            {
                var stockTickerHubProxy = hubConnection.CreateHubProxy("HealthHub");
                stockTickerHubProxy.On<HealthMonitorDto>("Update", health =>
                {
                    Console.WriteLine($"Received update from health service {health.AppName}");
                    //push to next hub
                });
                await hubConnection.Start();
            }




        }
    }
}

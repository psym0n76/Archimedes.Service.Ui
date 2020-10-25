using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Archimedes.Service.Ui
{
    public class CandleMetricHub : Hub<ICandleMetricHub>
    {
        public async Task Add(CandleMetricDto value)
        {
            await Clients.All.Add(value);
        }

        public async Task Delete(CandleMetricDto value)
        {
            await Clients.All.Delete(value);
        }

        public async Task Update(CandleMetricDto value)
        {
            await Clients.All.Update(value);
        }
    }
}
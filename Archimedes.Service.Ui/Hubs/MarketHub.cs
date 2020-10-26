using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Archimedes.Service.Ui
{
    public class MarketHub : Hub<IMarketHub>
    {
        public async Task Add(MarketDto value)
        {
            await Clients.All.Add(value);
        }

        public async Task Delete(MarketDto value)
        {
            await Clients.All.Delete(value);
        }

        public async Task Update(MarketDto value)
        {
            await Clients.All.Update(value);
        }
    }
}
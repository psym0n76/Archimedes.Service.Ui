using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Archimedes.Service.Ui.Hubs
{
    public class ValuesHub : Hub<IValuesClient>
    {
        public async Task Add(string value)
        {
            await Clients.All.Add(value);
        }

        public async Task Delete(string value)
        {
            await Clients.All.Delete(value);
        }
    }
}
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface ITradeHub
    {
        Task Add(TradeDto value);
        Task Delete(TradeDto value);
        Task Update(TradeDto value);
    }
}
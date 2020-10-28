using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Price
{
    public interface IPriceRequestManager
    {
        void SendToQueue(MarketDto market);
    }
}
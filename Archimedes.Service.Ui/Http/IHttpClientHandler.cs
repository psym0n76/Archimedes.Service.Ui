using System.Collections.Generic;
using System.Threading.Tasks;
using Archimedes.Library.Candles;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Http
{
    public interface IHttpClientHandler
    {
        Task<IEnumerable<Price>> GetPrices();
        Task<IEnumerable<Candle>> GetCandles();
        Task<IEnumerable<MarketDto>> GetMarkets();
    }
}
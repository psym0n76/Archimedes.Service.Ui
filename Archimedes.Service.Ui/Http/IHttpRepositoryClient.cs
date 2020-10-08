using System.Collections.Generic;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Archimedes.Service.Ui.Http
{
    public interface IHttpRepositoryClient
    {
        Task<IEnumerable<PriceDto>> GetPrices();
        Task<IEnumerable<CandleDto>> GetCandles();
        Task<IEnumerable<MarketDto>> GetMarkets();

        Task UpdateMarket(MarketDto market);
        Task<IEnumerable<CandleDto>> GetCandlesByGranularityMarket(string market, string granularity);

        Task<IEnumerable<string>> GetGranularityDistinct();
        Task<IEnumerable<string>> GetMarketDistinct();
    }
}
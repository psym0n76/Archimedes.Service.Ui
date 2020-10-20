using System.Collections.Generic;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Http
{
    public interface IHttpRepositoryClient
    {
        Task<IEnumerable<PriceDto>> GetPrices();
        Task<IEnumerable<CandleDto>> GetCandles();
        Task<IEnumerable<MarketDto>> GetMarkets();
        Task<IEnumerable<StrategyDto>> GetStrategies();

        Task<IEnumerable<PriceLevelDto>> GetPriceLevels();

        Task UpdateMarket(MarketDto market);
        Task<IEnumerable<CandleDto>> GetCandlesByGranularityMarket(string market, string granularity);

        Task<IEnumerable<string>> GetGranularityDistinct();
        Task<IEnumerable<string>> GetMarketDistinct();


    }
}
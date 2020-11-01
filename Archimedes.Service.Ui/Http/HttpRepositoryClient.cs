using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Archimedes.Library.Domain;
using Archimedes.Library.Extensions;
using Archimedes.Library.Message.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Archimedes.Service.Ui.Http
{
    public class HttpRepositoryClient : IHttpRepositoryClient
    {
        private readonly ILogger<HttpRepositoryClient> _logger;
        private readonly HttpClient _client;

        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1

        public HttpRepositoryClient(IOptions<Config> config, HttpClient httpClient, ILogger<HttpRepositoryClient> logger)
        {
            httpClient.BaseAddress = new Uri($"{config.Value.ApiRepositoryUrl}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
            _logger = logger;
        }


        public async Task<IEnumerable<PriceDto>> GetPrices()
        {
            var response = await _client.GetAsync("price");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var prices = await response.Content.ReadAsAsync<IEnumerable<PriceDto>>();
            return prices;
        }

        public async Task<IEnumerable<CandleDto>> GetCandles()
        {
            var response = await _client.GetAsync("candle");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var candles = await response.Content.ReadAsAsync<IEnumerable<CandleDto>>();

            return candles;
        }

        public async Task UpdateMarket(MarketDto market)
        {
            var payload = new JsonContent(market);
            var response = await _client.PutAsync("market", payload);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"PUT Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri} \n {market}");
            }
        }

        public async Task UpdateStrategy(StrategyDto strategy)
        {
            var payload = new JsonContent(strategy);
            var response = await _client.PutAsync("strategy", payload);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"PUT Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri} \n {strategy}");
            }
        }

        public async Task<IEnumerable<CandleDto>> GetCandlesByGranularityMarket(string market, string granularity)
        {
            var response = await _client.GetAsync($"candle/bymarket_bygranularity?market={market}&granularity={granularity}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var candles = await response.Content.ReadAsAsync<IEnumerable<CandleDto>>();

            return candles;
        }

        public async Task<IEnumerable<CandleDto>> GetCandlesByPage(int page, int size)
        {
            var response = await _client.GetAsync($"candle/bypage?page={page}&size={size}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var candles = await response.Content.ReadAsAsync<IEnumerable<CandleDto>>();

            return candles;
        }

        public async Task<IEnumerable<MarketDto>> GetMarkets()
        {
            var response = await _client.GetAsync("market");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var markets = await response.Content.ReadAsAsync<IEnumerable<MarketDto>>();

            return markets;
        }

        public async Task<IEnumerable<StrategyDto>> GetStrategies()
        {
            var response = await _client.GetAsync("strategy");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var strategies = await response.Content.ReadAsAsync<IEnumerable<StrategyDto>>();

            return strategies;
        }

        public async Task<IEnumerable<PriceDto>> GetPricesDistinct()
        {
            var response = await _client.GetAsync("price/bymarket_distinct");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var prices = await response.Content.ReadAsAsync<IEnumerable<PriceDto>>();

            return prices;
        }

        public async Task<IEnumerable<string>> GetMarketDistinct()
        {
            var response = await _client.GetAsync("market/bymarket_distinct");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var markets = await response.Content.ReadAsAsync<IEnumerable<string>>();

            return markets;
        }
        

        public async Task<IEnumerable<string>> GetGranularityDistinct()
        {
            var response = await _client.GetAsync("market/bygranularity_distinct");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var markets = await response.Content.ReadAsAsync<IEnumerable<string>>();

            return markets;
        }

        public async Task<IEnumerable<PriceLevelDto>> GetPriceLevels()
        {
            var response = await _client.GetAsync("price-level");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                return null;
            }

            var priceLevels = await response.Content.ReadAsAsync<IEnumerable<PriceLevelDto>>();

            return priceLevels;
        }
    }
}

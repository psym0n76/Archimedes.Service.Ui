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

        public HttpRepositoryClient(IOptions<Config> config, HttpClient httpClient,
            ILogger<HttpRepositoryClient> logger)
        {
            httpClient.BaseAddress = new Uri($"{config.Value.ApiRepositoryUrl}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
            _logger = logger;
        }


        public async Task<IEnumerable<PriceDto>> GetPrices()
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<CandleDto>> GetCandles()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;

        }

        public async Task UpdateMarket(MarketDto market)
        {

            try
            {
                var payload = new JsonContent(market);
                var response = await _client.PutAsync("market", payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        $"PUT Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri} \n {market}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

        }

        public async Task UpdateStrategy(StrategyDto strategy)
        {

            try
            {
                var payload = new JsonContent(strategy);
                var response = await _client.PutAsync("strategy", payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        $"PUT Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri} \n {strategy}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }
        }

        public async Task<IEnumerable<CandleDto>> GetCandlesByGranularityMarket(string market, string granularity)
        {

            try
            {
                var response =
                    await _client.GetAsync($"candle/bymarket_bygranularity?market={market}&granularity={granularity}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                    return null;
                }

                var candles = await response.Content.ReadAsAsync<IEnumerable<CandleDto>>();

                return candles;
            }
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<CandleDto>> GetCandlesByPage(int page, int size)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<MarketDto>> GetMarkets()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<StrategyDto>> GetStrategies()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<PriceDto>> GetPricesDistinct()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<string>> GetMarketDistinct()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }


        public async Task<IEnumerable<string>> GetGranularityDistinct()
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }

        public async Task<IEnumerable<PriceLevelDto>> GetPriceLevels()
        {
            try
            {
                var response = await _client.GetAsync("price-level");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                    return new List<PriceLevelDto>();
                }

                var priceLevels = await response.Content.ReadAsAsync<IEnumerable<PriceLevelDto>>();

                return priceLevels;
            }
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
            }

            return null;
        }
    }
}

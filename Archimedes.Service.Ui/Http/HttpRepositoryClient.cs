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
    }
}

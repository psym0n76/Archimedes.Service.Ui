using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Archimedes.Library.Candles;
using Archimedes.Library.Domain;
using Archimedes.Library.Extensions;
using Archimedes.Library.Message.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Archimedes.Service.Ui.Http
{
    public class HttpClientHandler : IHttpClientHandler
    {
        private readonly ILogger<HttpClientHandler> _logger;
        private readonly HttpClient _client;

        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1

        public HttpClientHandler(IOptions<Config> config, HttpClient httpClient, ILogger<HttpClientHandler> logger)
        {
            httpClient.BaseAddress = new Uri($"{config.Value.ApiRepositoryUrl}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
            _logger = logger;
        }


        public async Task<IEnumerable<Price>> GetPrices()
        {
            var response = await _client.GetAsync("price");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed:  {response.ReasonPhrase} from {_client.BaseAddress}/price");
                return null;
            }

            var prices = await response.Content.ReadAsAsync<IEnumerable<Price>>();

            //var json = await response.Content.ReadAsStringAsync();
            //var prices = JsonConvert.DeserializeObject<IEnumerable<Price>>(json);

            return prices;
        }

        public async Task<IEnumerable<Candle>> GetCandles()
        {
            var response = await _client.GetAsync("candle");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed:  {response.ReasonPhrase} from {_client.BaseAddress}/candle");
                return null;
            }

            var candles = await response.Content.ReadAsAsync<IEnumerable<Candle>>();
            //var json = await response.Content.ReadAsStringAsync();
            //var candles = JsonConvert.DeserializeObject<IEnumerable<Candle>>(json);

            return candles;
        }

        public async Task<IEnumerable<MarketDto>> GetMarkets()
        {
            var response = await _client.GetAsync("market");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Failed: {response.ReasonPhrase} from {_client.BaseAddress}/market");
                return null;
            }

            var markets = await response.Content.ReadAsAsync<IEnumerable<MarketDto>>();

            //var json = await response.Content.ReadAsStringAsync();
            //var markets = JsonConvert.DeserializeObject<IEnumerable<MarketDto>>(json);

            return markets;
        }
    }
}

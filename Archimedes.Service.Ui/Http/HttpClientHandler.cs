﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Archimedes.Library.Candles;
using Archimedes.Library.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
                _logger.LogError($"Failed to Post {response.ReasonPhrase} from {_client.BaseAddress}price");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var prices = JsonConvert.DeserializeObject<IEnumerable<Price>>(json);

            return prices;
        }
    }
}
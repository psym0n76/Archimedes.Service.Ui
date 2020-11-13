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
    public class HttpHealthMonitorClient : IHttpHealthMonitorClient
    {
        private readonly ILogger<HttpHealthMonitorClient> _logger;
        private readonly HttpClient _client;

        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1

        public HttpHealthMonitorClient(IOptions<Config> config, HttpClient httpClient, ILogger<HttpHealthMonitorClient> logger)
        {
            httpClient.BaseAddress = new Uri($"{config.Value.HealthUrl}api/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
            _logger = logger;
        }


        public async Task<IEnumerable<HealthMonitorDto>> GetHealthMonitor()
        {
            try
            {
                var response = await _client.GetAsync("health");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"GET Failed: {response.ReasonPhrase} from {response.RequestMessage.RequestUri}");
                    return null;
                }

                var health = await response.Content.ReadAsAsync<IEnumerable<HealthMonitorDto>>();
                return health;
            }
            catch (Exception e)
            {
                _logger.LogError($"GET Failed: {e.Message} from {e.InnerException}");
                return null;
            }
        }
    }
}

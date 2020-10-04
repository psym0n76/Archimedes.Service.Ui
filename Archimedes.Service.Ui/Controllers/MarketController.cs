using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketController : ControllerBase
    {

        private readonly ILogger<MarketController> _logger;
        private readonly IHttpRepositoryClient _client;

        public MarketController(ILogger<MarketController> logger, IHttpRepositoryClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task <IActionResult> Get()
        {
            try
            {
                var markets = await _client.GetMarkets();
                return Ok(markets);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }


        [HttpGet("bygranularity_distinct", Name = nameof(GetGranularityDistinctAsync))]
        public async Task<ActionResult<IEnumerable<string>>> GetGranularityDistinctAsync(CancellationToken ct)
        {
            try
            {
                var markets = await _client.GetGranularityDistinct();
                return Ok(markets);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }

        [HttpGet("bymarket_distinct", Name = nameof(GetMarketDistinctAsync))]
        public async Task<ActionResult<IEnumerable<string>>> GetMarketDistinctAsync(CancellationToken ct)
        {
            try
            {
                var markets = await _client.GetMarketDistinct();
                return Ok(markets);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }
    }
}
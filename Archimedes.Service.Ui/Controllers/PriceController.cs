using System;
using System.Threading.Tasks;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {

        private readonly ILogger<PriceController> _logger;
        private readonly IHttpRepositoryClient _client;

        public PriceController(ILogger<PriceController> logger, IHttpRepositoryClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task <IActionResult> Get()
        {
            try
            {
                var prices = await _client.GetPrices();
                return Ok(prices);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }

        [HttpGet("bymarket_distinct", Name = nameof(GetPriceMarketDistinctAsync))]
        public async Task <IActionResult> GetPriceMarketDistinctAsync()
        {
            try
            {
                var prices = await _client.GetPricesDistinct();
                return Ok(prices);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }
    }
}
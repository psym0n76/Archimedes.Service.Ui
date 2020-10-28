using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Price;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Http;
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
        private readonly IPriceRequestManager _request;

        public PriceController(ILogger<PriceController> logger, IHttpRepositoryClient client, IPriceRequestManager request)
        {
            _logger = logger;
            _client = client;
            _request = request;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PriceDto>>> GetPriceMarketDistinctAsync()
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


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost("subscribe", Name = nameof(PostPriceSubscription))]
        public ActionResult<IEnumerable<PriceDto>> PostPriceSubscription([FromBody] MarketDto marketDto )
        {
            try
            {
                _request.SendToQueue(marketDto);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }
    }
}
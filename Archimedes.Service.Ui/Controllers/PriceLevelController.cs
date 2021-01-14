using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{

    [Route("api/price-level")]
    [ApiController]
    public class PriceLevelController : ControllerBase
    {
        private readonly ILogger<PriceLevelController> _logger;
        private readonly IHttpRepositoryClient _client;

        public PriceLevelController(ILogger<PriceLevelController> logger, IHttpRepositoryClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<List<PriceLevelDto>>> GetPriceLevels()
        {
            try
            {
                var priceLevels = await _client.GetPriceLevels();

                if (!priceLevels.Any())
                {
                    return BadRequest("No Price Levels");
                }
                
                return Ok(priceLevels);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error from {nameof(PriceController)} {e.Message} {e.StackTrace}");
                return BadRequest(e.Message);
            }
        }
    }
}
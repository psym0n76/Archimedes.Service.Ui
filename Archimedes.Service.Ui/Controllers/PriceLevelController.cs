using System;
using System.Threading.Tasks;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task <IActionResult> GetPriceLevels()
        {
            try
            {
                var priceLevels = await _client.GetPriceLevels();
                return Ok(priceLevels);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }
    }
}
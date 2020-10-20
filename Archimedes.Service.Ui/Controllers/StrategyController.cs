using System;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StrategyController : ControllerBase
    {
        private readonly ILogger<StrategyController> _logger;
        private readonly IHttpRepositoryClient _client;

        public StrategyController(ILogger<StrategyController> logger, IHttpRepositoryClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task <IActionResult> GetStrategies()
        {
            try
            {
                var strategies = await _client.GetStrategies();
                return Ok(strategies);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStrategy([FromBody] StrategyDto strategyDto)
        {
            try
            {
                _logger.LogInformation($"Market update request {strategyDto}");
                await _client.UpdateStrategy(strategyDto);
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
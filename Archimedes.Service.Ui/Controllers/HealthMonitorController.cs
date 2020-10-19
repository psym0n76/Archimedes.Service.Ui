using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Archimedes.Library.Message.Dto;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Archimedes.Service.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthMonitorController : ControllerBase
    {
        private readonly ILogger<HealthMonitorController> _logger;
        private readonly IHttpHealthMonitorClient _client;

        public HealthMonitorController(ILogger<HealthMonitorController> logger, IHttpHealthMonitorClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HealthMonitorDto>> Get()
        {
            try
            {
                var response = await _client.GetHealthMonitor();

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error {e.Message} {e.StackTrace}");
                return BadRequest("Error");
            }
        }
    }
}
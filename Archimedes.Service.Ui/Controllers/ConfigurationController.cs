using System.Collections.Generic;
using Archimedes.Library.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Archimedes.Service.Ui.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly Config _config;

        public ConfigurationController(IOptions<Config> config)
        {
            _config = config.Value;
        }

        [HttpGet()]
        public IActionResult ConfigurationData()
        {
            var config = new Dictionary<string, string>()
            {
                {nameof(_config.UserInterfaceBaseUrl), _config.UserInterfaceBaseUrl}
            };

            return Ok(config);
        }
    }
}
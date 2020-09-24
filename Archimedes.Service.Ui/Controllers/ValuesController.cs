using System.Collections.Generic;
using System.Linq;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Archimedes.Service.Ui.Controllers
{

    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHubContext<ValuesHub> _context;

        public ValuesController(IHubContext<ValuesHub> context)
        {
            _context = context;
        }

        public static List<MarketData> Source { get; set; } = new List<MarketData>();

        [HttpGet]
        public ActionResult<IEnumerable<MarketData>> Get()
        {
            if (Source.Any())
            {
                return Ok(Source);
            }

            return Ok(new List<string>(){});
        }

        [HttpPost]
        public async void Post([FromBody] MarketData value)
        {
            Source.Add(value);
            await _context.Clients.All.SendAsync("Add", value);
        }

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public async void Delete(string id)
        //{
        //    Source.Remove(item);
        //    await _context.Clients.All.SendAsync("Delete", item);
        //}
    }
}
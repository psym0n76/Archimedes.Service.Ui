using System.Collections.Generic;
using System.Linq;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Archimedes.Service.Ui.Controllers
{


    public class TestData
    {
        public string Forecast { get; set; }
    }

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

        public static List<TestData> Source { get; set; } = new List<TestData>();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (Source.Any())
            {
                return Ok(Source);
            }

            return Ok(new List<string>(){});
            //return Ok( new List<string>(){"No Data"});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return Source.Find(x => x.Forecast == id).Forecast;



        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] string value)
        {


            Source.Add( new TestData(){Forecast = value});
            await _context.Clients.All.SendAsync("Add", value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //Source[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            //var item = Source.Remove(id);

            var item = new TestData(){Forecast = id};

            Source.Remove(item);
            await _context.Clients.All.SendAsync("Delete", item);

        }
    }
}
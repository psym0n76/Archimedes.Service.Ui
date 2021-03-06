﻿using System;
using System.Threading.Tasks;
using Archimedes.Service.Ui.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandleController : ControllerBase
    {
        private readonly ILogger<CandleController> _logger;
        private readonly IHttpRepositoryClient _client;

        public CandleController(ILogger<CandleController> logger, IHttpRepositoryClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task <IActionResult> Get()
        {
            try
            {
                var candles = await _client.GetCandles();
                return Ok(candles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }

        [HttpGet("bymarket_bygranularity", Name = nameof(GetCandlesByMarketAndGranularity))]
        public async Task <IActionResult> GetCandlesByMarketAndGranularity(string market, string granularity)
        {
            try
            {
                var candles = await _client.GetCandlesByGranularityMarket(market, granularity);
                return Ok(candles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }

        
        [HttpGet("bypage", Name = nameof(GetCandlesByPage))]
        public async Task <IActionResult> GetCandlesByPage(int page, int size)
        {
            try
            {
                var candles = await _client.GetCandlesByPage(page, size);
                return Ok(candles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }

            return BadRequest();
        }
    }
}
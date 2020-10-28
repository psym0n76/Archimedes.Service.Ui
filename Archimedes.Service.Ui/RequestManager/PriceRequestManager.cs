using Archimedes.Library.Message;
using Archimedes.Library.Message.Dto;
using Archimedes.Library.RabbitMq;
using Microsoft.Extensions.Logging;

namespace Archimedes.Service.Price
{
    public class PriceRequestManager : IPriceRequestManager
    {
        private readonly ILogger<PriceRequestManager> _logger;
        private readonly IProducer<PriceMessage> _producer;

        public PriceRequestManager(ILogger<PriceRequestManager> logger, IProducer<PriceMessage> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public void SendToQueue(MarketDto market)
        {
            var request = new PriceMessage()
            {
                Market = market.Name
            };

            _producer.PublishMessage(request, "PriceRequestQueue");
            _logger.LogInformation($"Published to PriceRequestQueue: {request}");
        }
    }
}
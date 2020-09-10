using System.Collections.Generic;
using System.Threading.Tasks;
using Archimedes.Library.Candles;

namespace Archimedes.Service.Ui.Http
{
    public interface IHttpClientHandler
    {
        Task<IEnumerable<Price>> GetPrices();
    }
}
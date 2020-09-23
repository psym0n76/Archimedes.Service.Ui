using System.Collections.Generic;
using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Http
{
    public interface IHttpHealthMonitorClient
    {
        Task<IEnumerable<HealthMonitorDto>> GetHealthMonitor();
    }
}
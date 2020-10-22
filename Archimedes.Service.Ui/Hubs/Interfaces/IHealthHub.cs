using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface IHealthHub
    {
        Task Add(HealthMonitorDto health);
        Task Delete(HealthMonitorDto health);
        Task Update(HealthMonitorDto health);
    }
}

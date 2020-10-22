using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface IStrategyHub
    {
        Task Add(StrategyDto value);
        Task Delete(StrategyDto value);
        Task Update(StrategyDto value);
    }
}
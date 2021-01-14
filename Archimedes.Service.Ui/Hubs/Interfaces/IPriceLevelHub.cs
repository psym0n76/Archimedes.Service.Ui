using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface IPriceLevelHub
    {
        Task Add(PriceLevelDto level);
        Task Delete(PriceLevelDto level);
        Task Update(PriceLevelDto level);
    }
}

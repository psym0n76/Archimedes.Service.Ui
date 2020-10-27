using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface IPriceHub
    {
        Task Add(PriceDto value);
        Task Delete(PriceDto value);
        Task Update(PriceDto value); 
    }
}
using System.Threading.Tasks;

namespace Archimedes.Service.Ui.Hubs
{
    public interface IValuesClient
    {
        Task Add(string value);
        Task Delete(string value);
    }
}

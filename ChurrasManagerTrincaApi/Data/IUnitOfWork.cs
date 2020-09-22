using System.Threading.Tasks;

namespace ChurrasManagerTrincaApi.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
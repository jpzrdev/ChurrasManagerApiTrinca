using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Models;

namespace ChurrasManagerTrincaApi.Repositories
{
    public interface IChurrascoUserRepository
    {
        Task<ChurrascoUser> GetEntityByChurrascoIdAndUserId(int churrascoId, int userId);
        void Add(ChurrascoUser churrascoUser);
        Task<ChurrascoUserGetUserDto> Update(ChurrascoUserUpdateDto churrascoUserDto);
        void Delete(ChurrascoUser churrascoUser);
    }
}
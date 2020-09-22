using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Models;

namespace ChurrasManagerTrincaApi.Repositories
{
    public interface IChurrascoRepository
    {
        Task<Churrasco> GetEntityById(int id);
        Task<ChurrascoGetDto> GetById(int id);
        Task<List<ChurrascoGetDto>> GetAll();
        Task<List<ChurrascoGetDto>> GetAllByUserId(int id);
        Task<Churrasco> Add(ChurrascoAddDto churrasco);
        Task<ChurrascoGetDto> Update(ChurrascoUpdateDto churrascoDto);
        void Delete(Churrasco churrasco);
    }
}

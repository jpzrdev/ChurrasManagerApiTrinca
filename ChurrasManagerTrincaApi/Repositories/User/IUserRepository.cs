using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Models;

namespace ChurrasManagerTrincaApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetEntityById(int id);
        Task<UserGetDto> GetById(int id);
        Task<List<UserGetDto>> GetAll();
        Task<List<UserGetDto>> GetAllByChurrascoId(int id);
        Task<User> Add(UserAddDto user);
        Task<UserGetDto> Authenticate(UserAuthenticateDto user);
        Task<UserGetDto> Update(UserUpdateDto userDto);
        void Delete(User user);

    }
}
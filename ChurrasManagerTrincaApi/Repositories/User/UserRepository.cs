using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurrasManagerTrincaApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetEntityById(int id)
        {
            return await _context.Usuarios.Include(chuser => chuser.Churrascos)
                                          .AsNoTracking()
                                          .Where(u => u.Id == id)
                                          .FirstOrDefaultAsync();
        }
        public async Task<UserGetDto> GetById(int id)
        {
            var user = await _context.Usuarios.Include(chuser => chuser.Churrascos)
                                               .ThenInclude(c => c.Churrasco)
                                               .AsNoTracking()
                                               .Where(u => u.Id == id)
                                               .FirstOrDefaultAsync();

            if (user == null)
                return null;

            return CreateUserGetDtoModel(user);
        }

        public async Task<List<UserGetDto>> GetAll()
        {
            var users = await _context.Usuarios.Include(chuser => chuser.Churrascos)
                                           .ThenInclude(c => c.Churrasco)
                                           .AsNoTracking()
                                           .ToListAsync();

            var usersGetDto = new List<UserGetDto>();
            foreach (var user in users)
            {
                usersGetDto.Add(CreateUserGetDtoModel(user));
            }

            return usersGetDto;

        }


        public async Task<List<UserGetDto>> GetAllByChurrascoId(int id)
        {
            var users = await _context.Usuarios.Include(chuser => chuser.Churrascos)
                                           .ThenInclude(c => c.Churrasco)
                                            .AsNoTracking()
                                            .Where(c => c.Churrascos.Any(chuser => chuser.ChurrascoId == id))
                                            .ToListAsync();

            var usersGetDto = new List<UserGetDto>();
            foreach (var user in users)
            {
                usersGetDto.Add(CreateUserGetDtoModel(user));
            }

            return usersGetDto;
        }

        public async Task<UserGetDto> Authenticate(UserAuthenticateDto authDto)
        {
            var user = await _context.Usuarios.Include(chuser => chuser.Churrascos)
                                               .ThenInclude(c => c.Churrasco)
                                               .AsNoTracking()
                                               .Where(u => u.Email == authDto.Email && u.Password == authDto.Password)
                                               .FirstOrDefaultAsync();

            if (user == null)
                return null;

            return CreateUserGetDtoModel(user, true);

        }

        public async Task<User> Add(UserAddDto userAddDto)
        {
            var user = new User
            {
                Email = userAddDto.Email,
                Password = userAddDto.Password,
                Nome = userAddDto.Nome
            };

            await _context.AddAsync(user);

            return user;
        }

        public async Task<UserGetDto> Update(UserUpdateDto userDto)
        {
            var user = await _context.Usuarios.AsNoTracking()
                                            .Where(u => u.Id == userDto.Id)
                                            .FirstOrDefaultAsync();

            user.Email = !string.IsNullOrEmpty(userDto.Email) ? userDto.Email : user.Email;
            user.Password = !string.IsNullOrEmpty(userDto.Password) ? userDto.Password : user.Password;
            user.Nome = !string.IsNullOrEmpty(userDto.Nome) ? userDto.Nome : user.Nome;

            _context.Update(user);

            return CreateUserGetDtoModel(user, true);
        }

        public void Delete(User user)
        {
            var churrascosUser = user.Churrascos;
            foreach (var churrascoUser in churrascosUser)
            {
                _context.Remove(churrascoUser);
            }

            _context.Remove(user);
        }

        private UserGetDto CreateUserGetDtoModel(User user, bool somenteUsuario = false)
        {
            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                Email = user.Email,
                Nome = user.Nome,
                Churrascos = new List<ChurrascoUserGetChurrascoDto>()
            };

            if (!somenteUsuario)
            {
                foreach (var churrascoUser in user.Churrascos)
                {
                    userGetDto.Churrascos.Add(new ChurrascoUserGetChurrascoDto
                    {
                        Churrasco = new ChurrascoGetDto
                        {
                            Id = churrascoUser.Churrasco.Id,
                            DataChurras = churrascoUser.Churrasco.DataChurras,
                            Motivo = churrascoUser.Churrasco.Motivo,
                            Observacoes = churrascoUser.Churrasco.Observacoes,
                            ValorSugerido = churrascoUser.Churrasco.ValorSugerido,
                            ValorSugeridoSemBebida = churrascoUser.Churrasco.ValorSugeridoSemBebida,
                            TotalConvidados = churrascoUser.Churrasco.Convidados.Count(),
                            TotalArrecadado = churrascoUser.Churrasco.Convidados.Sum(chuser => chuser.ValorContribuicao),
                        }
                    });
                }
            }



            return userGetDto;
        }
    }
}
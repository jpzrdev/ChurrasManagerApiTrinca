using System.Linq;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChurrasManagerTrincaApi.Repositories
{
    public class ChurrascoUserRepository : IChurrascoUserRepository
    {
        private readonly DatabaseContext _context;
        public ChurrascoUserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ChurrascoUser> GetEntityByChurrascoIdAndUserId(int churrascoId, int userId)
        {
            return await _context.ChurrascoUsers.Include(chuser => chuser.Churrasco)
                                                .Include(chuser => chuser.User)
                                                .AsNoTracking()
                                                .Where(chuser => chuser.ChurrascoId == churrascoId && chuser.UserId == userId)
                                                .FirstOrDefaultAsync();
        }

        public void Add(ChurrascoUser churrascoUser)
        {
            var churrasco = _context.Churrascos.AsNoTracking()
                                               .Where(c => c.Id == churrascoUser.ChurrascoId)
                                               .FirstOrDefault();


            if (churrascoUser.ValorContribuicao == 0)
            {
                if (churrascoUser.BebidaInclusa)
                    churrascoUser.ValorContribuicao = churrasco.ValorSugerido;
                else
                    churrascoUser.ValorContribuicao = churrasco.ValorSugeridoSemBebida;
            }

            _context.Add(churrascoUser);
        }

        public async Task<ChurrascoUserGetUserDto> Update(ChurrascoUserUpdateDto churrascoUserDto)
        {
            var churrascoUser = await _context.ChurrascoUsers.Include(chuser => chuser.Churrasco)
                                                .Include(chuser => chuser.User)
                                                .AsNoTracking()
                                                .Where(chuser => chuser.ChurrascoId == churrascoUserDto.ChurrascoId && chuser.UserId == churrascoUserDto.UserId)
                                                .FirstOrDefaultAsync();

            if (churrascoUser == null)
                return null;

            churrascoUser.BebidaInclusa = churrascoUserDto.BebidaInclusa;
            churrascoUser.Pago = churrascoUserDto.Pago;

            if (churrascoUserDto.ValorContribuicao > 0)
            {
                if (churrascoUserDto.ValorContribuicao < churrascoUser.Churrasco.ValorSugeridoSemBebida)
                {
                    churrascoUser.ValorContribuicao = churrascoUserDto.ValorContribuicao;
                    churrascoUser.BebidaInclusa = false;
                }
                else
                {
                    churrascoUser.ValorContribuicao = churrascoUserDto.ValorContribuicao;
                }
            }

            _context.Update(churrascoUser);

            return CreateChurrascoUserGetUserDtoModel(churrascoUser);
        }

        public void Delete(ChurrascoUser churrascoUser)
        {
            _context.Remove(churrascoUser);
        }

        private ChurrascoUserGetUserDto CreateChurrascoUserGetUserDtoModel(ChurrascoUser churrascoUser)
        {
            var churrascoUserGetUserDto = new ChurrascoUserGetUserDto
            {
                User = new UserGetDto
                {
                    Id = churrascoUser.User.Id,
                    Email = churrascoUser.User.Email,
                    Nome = churrascoUser.User.Nome
                },
                BebidaInclusa = churrascoUser.BebidaInclusa,
                Pago = churrascoUser.Pago,
                ValorContribuicao = churrascoUser.ValorContribuicao
            };

            return churrascoUserGetUserDto;
        }
    }
}
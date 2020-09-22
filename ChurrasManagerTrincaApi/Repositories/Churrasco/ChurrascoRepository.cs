using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChurrasManagerTrincaApi.Repositories
{
    public class ChurrascoRepository : IChurrascoRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        public ChurrascoRepository(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Churrasco> GetEntityById(int id)
        {
            return await _context.Churrascos.Include(chuser => chuser.Convidados)
                                            .AsNoTracking()
                                            .Where(c => c.Id == id)
                                            .FirstOrDefaultAsync();


        }
        public async Task<ChurrascoGetDto> GetById(int id)
        {
            var churrasco = await _context.Churrascos.Include(chuser => chuser.Convidados)
                                            .ThenInclude(u => u.User)
                                            .AsNoTracking()
                                            .Where(c => c.Id == id)
                                            .FirstOrDefaultAsync();

            if (churrasco == null)
                return null;

            return CreateChurrascoGetDtoModel(churrasco);

        }

        public async Task<List<ChurrascoGetDto>> GetAll()
        {
            var churrascos = await _context.Churrascos.Include(chuser => chuser.Convidados)
                                            .ThenInclude(u => u.User)
                                            .AsNoTracking()
                                            .ToListAsync();

            var churrascosGetDto = new List<ChurrascoGetDto>();
            foreach (var churrasco in churrascos)
            {
                churrascosGetDto.Add(CreateChurrascoGetDtoModel(churrasco));
            }

            return churrascosGetDto;

        }

        public async Task<List<ChurrascoGetDto>> GetAllByUserId(int id)
        {
            var churrascos = await _context.Churrascos.Include(chuser => chuser.Convidados)
                                            .ThenInclude(u => u.User)
                                            .AsNoTracking()
                                            .OrderBy(c => c.DataChurras)
                                            .Where(c => c.Convidados.Any(chuser => chuser.UserId == id))
                                            .ToListAsync();

            var churrascosGetDto = new List<ChurrascoGetDto>();
            foreach (var churrasco in churrascos)
            {
                churrascosGetDto.Add(CreateChurrascoGetDtoModel(churrasco));
            }

            return churrascosGetDto;
        }

        public async Task<Churrasco> Add(ChurrascoAddDto churrascoAddDto)
        {

            Churrasco churrasco = new Churrasco
            {
                DataChurras = churrascoAddDto.DataChurras,
                Motivo = churrascoAddDto.Motivo,
                Observacoes = churrascoAddDto.Observacoes
            };

            if (churrascoAddDto.ValorSugerido <= 0)
            {
                churrasco.ValorSugerido = decimal.Parse(_configuration["ValorSugerido"]);
                churrasco.ValorSugeridoSemBebida = churrasco.ValorSugerido - 10;
            }
            else if (churrascoAddDto.ValorSugerido < 10)
            {
                churrasco.ValorSugeridoSemBebida = 0;
            }
            else
            {
                churrasco.ValorSugeridoSemBebida = churrascoAddDto.ValorSugerido - 10;
            }

            await _context.AddAsync(churrasco);

            return churrasco;
        }

        public async Task<ChurrascoGetDto> Update(ChurrascoUpdateDto churrascoDto)
        {
            var churrasco = await _context.Churrascos.Include(chuser => chuser.Convidados).AsNoTracking().Where(c => c.Id == churrascoDto.Id).FirstOrDefaultAsync();

            churrasco.DataChurras = churrascoDto.DataChurras;
            churrasco.Motivo = churrascoDto.Motivo;
            churrasco.Observacoes = churrascoDto.Observacoes;

            if (churrascoDto.ValorSugerido > 0)
            {
                if (churrascoDto.ValorSugerido >= 10)
                {
                    churrasco.ValorSugerido = churrascoDto.ValorSugerido;
                    churrasco.ValorSugeridoSemBebida = churrasco.ValorSugerido - 10;
                }
                else
                {
                    churrasco.ValorSugerido = churrascoDto.ValorSugerido;
                    churrasco.ValorSugeridoSemBebida = 0;
                }
            }

            _context.Update(churrasco);

            return CreateChurrascoGetDtoModel(churrasco, true);
        }

        public void Delete(Churrasco churrasco)
        {
            var churrascoUsers = churrasco.Convidados;
            foreach (var churrascoUser in churrascoUsers)
            {
                _context.Remove(churrascoUser);
            }

            _context.Remove(churrasco);
        }


        private ChurrascoGetDto CreateChurrascoGetDtoModel(Churrasco churrasco, bool somenteChurrasco = false)
        {
            var churrascoGetDto = new ChurrascoGetDto
            {
                Id = churrasco.Id,
                DataChurras = churrasco.DataChurras.Value,
                Motivo = churrasco.Motivo,
                Observacoes = churrasco.Observacoes,
                TotalConvidados = churrasco.Convidados.Count(),
                TotalArrecadado = churrasco.Convidados.Sum(chuser => chuser.ValorContribuicao),
                ValorSugerido = churrasco.ValorSugerido,
                ValorSugeridoSemBebida = churrasco.ValorSugeridoSemBebida,
                Convidados = new List<ChurrascoUserGetUserDto>()
            };

            if (!somenteChurrasco)
            {
                foreach (var churrascoUser in churrasco.Convidados)
                {
                    churrascoGetDto.Convidados.Add(new ChurrascoUserGetUserDto
                    {
                        User = new UserGetDto
                        {
                            Id = churrascoUser.User.Id,
                            Nome = churrascoUser.User.Nome,
                            Email = churrascoUser.User.Email
                        },
                        Pago = churrascoUser.Pago,
                        BebidaInclusa = churrascoUser.BebidaInclusa,
                        ValorContribuicao = churrascoUser.ValorContribuicao
                    });
                }
            }


            return churrascoGetDto;
        }

    }
}
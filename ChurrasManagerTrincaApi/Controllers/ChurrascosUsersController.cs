using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using ChurrasManagerTrincaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChurrasManagerTrincaApi.Controllers
{
    [ApiController]
    [Route("v1/churrascosusers")]
    public class ChurrascosUsersController : ControllerBase
    {

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ChurrascoUser>> Post([FromServices] IChurrascoUserRepository churrascoUserRepository,
                                                            [FromServices] IChurrascoRepository churrascoRepository,
                                                            [FromServices] IUserRepository userRepository,
                                                            [FromServices] IUnitOfWork unitOfWork,
                                                            [FromBody] ChurrascoUser model)
        {
            try
            {
                var user = await userRepository.GetEntityById(model.UserId.Value);
                var churrasco = await churrascoRepository.GetEntityById(model.ChurrascoId.Value);

                if (user == null || churrasco == null)
                    return BadRequest("Usuário ou Churrasco não encontrado.");

                churrascoUserRepository.Add(model);

                if (await unitOfWork.Commit())
                {
                    return model;
                }
                else
                {
                    return BadRequest("Erro desconhecido.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<ChurrascoUserGetUserDto>> Update([FromServices] IChurrascoUserRepository churrascoUserRepository,
                                                              [FromServices] IChurrascoRepository churrascoRepository,
                                                              [FromServices] IUnitOfWork unitOfWork,
                                                              [FromBody] ChurrascoUserUpdateDto model)
        {
            try
            {

                var churrascoUserDto = await churrascoUserRepository.Update(model);

                if (churrascoUserDto == null)
                    return BadRequest("Relação de churrasco com usuário não encontrada.");

                if (await unitOfWork.Commit())
                {
                    return churrascoUserDto;
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{idChurrasco:int}&{idUser:int}")]
        public async Task<ActionResult> Delete([FromServices] IChurrascoUserRepository churrascoUserRepository,
                                               [FromServices] IChurrascoRepository churrascoRepository,
                                               [FromServices] IUnitOfWork unitOfWork,
                                               int idChurrasco,
                                               int idUser)
        {
            try
            {
                var churrascoUser = await churrascoUserRepository.GetEntityByChurrascoIdAndUserId(idChurrasco, idUser);
                if (churrascoUser == null)
                    return BadRequest("Relação de churrasco com usuário não encontrada.");

                churrascoUserRepository.Delete(churrascoUser);

                if (await unitOfWork.Commit())
                {
                    return Ok("Usuário deletado do churrasco.");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }

        }
    }
}
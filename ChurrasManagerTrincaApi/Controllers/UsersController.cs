using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using ChurrasManagerTrincaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChurrasManagerTrincaApi.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UsersController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<UserGetDto>>> Get([FromServices] IUserRepository userRepository)
        {
            return await userRepository.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<UserGetDto>> GetById([FromServices] IUserRepository userRepository, int id)
        {
            return await userRepository.GetById(id);
        }

        [HttpGet]
        [Route("churrascos/{id:int}")]
        public async Task<ActionResult<List<UserGetDto>>> GetByUserId([FromServices] IUserRepository userRepository, int id)
        {
            return await userRepository.GetAllByChurrascoId(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<UserGetDto>> Post([FromServices] IUserRepository userRepository,
                                                          [FromServices] IUnitOfWork unitOfWork,
                                                          [FromBody] UserAddDto model)
        {
            try
            {

                var user = await userRepository.Add(model);
                if (await unitOfWork.Commit())
                {
                    return await userRepository.GetById(user.Id);
                }
                else
                {
                    return BadRequest("Unexpected Error!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<UserGetDto>> UpdateIgnorePassword([FromServices] IUserRepository userRepository,
                                                                [FromServices] IUnitOfWork unitOfWork,
                                                                [FromBody] UserUpdateDto model)
        {

            try
            {
                var user = await userRepository.GetEntityById(model.Id.Value);
                if (user == null)
                    return BadRequest("Usuário não encontrado.");

                var userGetDto = await userRepository.Update(model);

                if (await unitOfWork.Commit())
                {
                    return userGetDto;
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
        [Route("{id:int}")]
        public async Task<ActionResult> Delete([FromServices] IUserRepository userRepository,
                                               [FromServices] IUnitOfWork unitOfWork,
                                               int id)
        {
            try
            {
                var user = await userRepository.GetEntityById(id);
                if (user == null)
                    return BadRequest("Usuário não encontrado");

                userRepository.Delete(user);

                if (await unitOfWork.Commit())
                {
                    return Ok("Usuário deletado!");
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
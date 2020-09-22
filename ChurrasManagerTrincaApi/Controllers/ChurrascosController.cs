using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using ChurrasManagerTrincaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurrasManagerTrincaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/churrascos")]
    public class ChurrascosController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<ChurrascoGetDto>>> Get([FromServices] IChurrascoRepository churrascoRepository)
        {
            return await churrascoRepository.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ChurrascoGetDto>> GetById([FromServices] IChurrascoRepository churrascoRepository, int id)
        {
            return await churrascoRepository.GetById(id);
        }

        [HttpGet]
        [Route("users/{id:int}")]
        public async Task<ActionResult<List<ChurrascoGetDto>>> GetByUserId([FromServices] IChurrascoRepository churrascoRepository, int id)
        {
            return await churrascoRepository.GetAllByUserId(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ChurrascoGetDto>> Post([FromServices] IChurrascoRepository churrascoRepository,
                                                          [FromServices] IUnitOfWork unitOfWork,
                                                          [FromBody] ChurrascoAddDto model)
        {
            try
            {
                var churrasco = await churrascoRepository.Add(model);
                if (await unitOfWork.Commit())
                {
                    return await churrascoRepository.GetById(churrasco.Id);
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
        public async Task<ActionResult<ChurrascoGetDto>> Update([FromServices] IChurrascoRepository churrascoRepository,
                                                                [FromServices] IUnitOfWork unitOfWork,
                                                                [FromBody] ChurrascoUpdateDto model)
        {
            try
            {
                var churrasco = await churrascoRepository.GetEntityById(model.Id.Value);
                if (churrasco == null)
                    return BadRequest("Churrasco not found.");

                var churrascoGetDto = await churrascoRepository.Update(model);

                if (await unitOfWork.Commit())
                {
                    return churrascoGetDto;
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
        public async Task<ActionResult> Delete([FromServices] IChurrascoRepository churrascoRepository,
                                               [FromServices] IUnitOfWork unitOfWork,
                                               int id)
        {
            try
            {
                var churrasco = await churrascoRepository.GetEntityById(id);
                if (churrasco == null)
                    return BadRequest("Churrasco não encontrado.");

                churrascoRepository.Delete(churrasco);
                if (await unitOfWork.Commit())
                {
                    return Ok("Churrasco deletado!");
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
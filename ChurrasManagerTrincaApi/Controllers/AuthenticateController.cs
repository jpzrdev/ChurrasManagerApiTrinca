using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChurrasManagerTrincaApi.Data;
using ChurrasManagerTrincaApi.Models;
using ChurrasManagerTrincaApi.Repositories;
using ChurrasManagerTrincaApi.Services.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChurrasManagerTrincaApi.Controllers
{
    [ApiController]
    [Route("v1/auth")]
    public class Authenticate : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Login([FromServices] IUserRepository userRepository,
                                                        [FromServices] IJwtService jwtService,
                                                        [FromBody] UserAuthenticateDto model)
        {
            var user = await userRepository.Authenticate(model);
            if (user == null)
                return BadRequest("Usuário ou senha inválidos.");

            return new
            {
                user = user,
                token = jwtService.GerarToken()
            };
        }

    }
}
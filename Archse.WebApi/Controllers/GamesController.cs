using Archse.Application;
using Archse.Exception;
using Archse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Archse.WebApi.Controllers
{
    //teste
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamesApplication _gamesApplication;


        public GamesController(IGamesApplication gamesApplication)
        {
            _gamesApplication = gamesApplication;
        }

        [AllowAnonymous]
        [HttpGet("/Infra", Name = "Infra")]
        public ActionResult<string> Infra()
        {
            return Ok(new { Hora = DateTime.Now.ToString() });
        }

        // [Authorize(Roles = "user")]
        [HttpGet(Name = "GetAll")]
        public ActionResult<List<GameResponse>> GetAll()
        {
            try
            {
                var game = _gamesApplication.GetAll();
                return Ok(game); //200
            }
            catch (NotFoundDataException ex)
            {
                return NotFound(); //404
            }
            catch (System.Exception ex)
            {
                return BadRequest(); //500
            }
        }

        //     [Authorize(Roles = "user")]
        [HttpPost("/PingHost", Name = "PingHost")]
        public ActionResult<string> PingHostC([FromBody] string url)
        {
            return Ok(_gamesApplication.PingHost(url));
        }

        //    [Authorize(Roles = "user")]
        [HttpGet("{identificador}", Name = "GetGame")]
        public ActionResult<GameResponse> Get(string identificador)
        {
            var game = _gamesApplication.Get(identificador);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        //        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult<GameResponse> Create(GameRequest game)
        {
            string identificador = _gamesApplication.Create(game);
            var gameResponse = _gamesApplication.Get(identificador);
            return gameResponse;

        }

        //      [Authorize(Roles = "user")]
        [HttpPost("Create")]
        public ActionResult<string> CreateAsync(GameRequest game)
        {
            string identificador = _gamesApplication.Publish(game);
            return identificador;

        }

        //      [Authorize(Roles = "user")]
        [HttpPut("{identificador}")]
        public IActionResult Update(string identificador, GameRequest gameIn)
        {
            var game = _gamesApplication.Get(identificador);

            if (game == null)
            {
                return NotFound();
            }

            _gamesApplication.Update(identificador, gameIn);

            return NoContent();
        }

        ///     [Authorize(Roles = "user")]
        [HttpDelete("{identificador}")]
        public IActionResult Delete(string identificador)
        {
            var game = _gamesApplication.Get(identificador);

            if (game == null)
            {
                return NotFound();
            }

            _gamesApplication.Delete(identificador);

            return NoContent();
        }
    }
}

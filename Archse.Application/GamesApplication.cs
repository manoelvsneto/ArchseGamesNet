using Archse.Events;
using Archse.Exception;
using Archse.Models;
using Archse.Publisher;
using Archse.Service;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Archse.Application
{
    public class GamesApplication : IGamesApplication
    {
        private readonly IGamesService _gameService;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;
        private readonly ILogger<GamesApplication> _logger;

        public GamesApplication(IGamesService gameService, IMapper mapper, ILogger<GamesApplication> logger, IPublisher publisher)
        {
            _gameService = gameService;
            _mapper = mapper;
            _logger = logger;
            _publisher = publisher;

        }
        public string Create(GameRequest game)
        {
            Game gameData = _mapper.Map<Game>(game);
            _gameService.Create(game);

            _logger.LogInformation($"Send GamesApplication inserted: {gameData.Identificador}");

            return gameData.Identificador;
        }
        public string PingHost(string nameOrAddress)
        {
            string pingable = "true";
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status.ToString();
            }
            catch (PingException ex)
            {
                pingable = ex.Message + " " + ex.InnerException;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
        public string Publish(GameRequest game)
        {
            GameInsertedEvent gameData = _mapper.Map<GameInsertedEvent>(game);
            gameData.Identificador = Guid.NewGuid().ToString();
            _publisher.Publish<GameInsertedEvent>(gameData,"game-inserted");
            return gameData.Identificador;
        }
        public void Delete(string identificador)
        {
            _gameService.Delete(identificador);
        }
        public GameResponse Get(string identificador)
        {
            GameResponse gameData = _gameService.Get(identificador);
            return gameData;
        }

        public void Update(string identificador, GameRequest gameIn)
        {
            Game gameData = _gameService.Get(identificador);
            gameData.Price = gameIn.Price;
            gameData.Category = gameIn.Category;
            gameData.Name = gameIn.Name;
            GameRequest gameDataN = _mapper.Map<GameRequest>(gameData);
            _gameService.Update(identificador, gameDataN);
        }

        public void Create(GameRequest game, string chave)
        {
            GameRequest gameData = _mapper.Map<GameRequest>(game);
            _gameService.Create(gameData, chave);
        }

        public List<GameResponse> GetAll()
        {
            try
            {
                List<GameResponse> gameData = _gameService.GetAll();
                return gameData;
            }
            catch (NotFoundDataException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
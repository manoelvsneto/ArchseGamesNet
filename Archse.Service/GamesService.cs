using Archse.Models;
using Archse.Repository;
using AutoMapper;

namespace Archse.Service
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IMapper _mapper;

        public GamesService(IGamesRepository gamesRepository, IMapper mapper)
        {
            _gamesRepository = gamesRepository;
            _mapper = mapper;
        }
        public string Create(GameRequest game)
        {
            Game gameData = _mapper.Map<Game>(game);
            gameData.Identificador = Guid.NewGuid().ToString();
            _gamesRepository.Adicionar(gameData);
            return gameData.Identificador;
        }
        public bool Create(GameRequest game, string chave)
        {
            Game gameData = _mapper.Map<Game>(game);
            gameData.Identificador = chave;
            _gamesRepository.Adicionar(gameData);
            return true;
        }
        public bool Delete(string identificador)
        {
            Game gameData = _gamesRepository.ObterPorIdentificador(identificador);
            _gamesRepository.Remover(gameData);
            return true;
        }

        public GameResponse Get(string identificador)
        {
            Game gameData = _gamesRepository.ObterPorIdentificador(identificador);
            GameResponse game = _mapper.Map<GameResponse>(gameData);
            return game;
        }

        public List<GameResponse> GetAll()
        {
            List<Game> gameData = _gamesRepository.ObterLista();
            List<GameResponse> game = _mapper.Map<List<GameResponse>>(gameData);
            return game;
        }

        public bool Update(string identificador, GameRequest gameIn)
        {
            Game gameData = _gamesRepository.ObterPorIdentificador(identificador);
            gameData.Price = gameIn.Price;
            gameData.Category = gameIn.Category;
            gameData.Name = gameIn.Name;
            _gamesRepository.Atualizar(gameData);
            return true;
        }
    }
}
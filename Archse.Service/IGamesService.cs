using Archse.Models;

namespace Archse.Service
{
    public interface IGamesService
    {
        public string Create(GameRequest game);
        public bool Create(GameRequest game, string chave);

        public bool Delete(string identificador);
        public GameResponse Get(string identificador);
        public bool Update(string identificador, GameRequest gameIn);

        public List<GameResponse> GetAll();
    }
}
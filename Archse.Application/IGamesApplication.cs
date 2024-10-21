using Archse.Models;

namespace Archse.Application
{
    public interface IGamesApplication
    {
        public string Create(GameRequest game);
        public void Delete(string identificador);
        public GameResponse Get(string identificador);

        public List<GameResponse> GetAll();

        public void Update(string identificador, GameRequest gameIn);
        public string Publish(GameRequest game);
        public void Create(GameRequest game, string chave);
        public string PingHost(string nameOrAddress);
    }
}
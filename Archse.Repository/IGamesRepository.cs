using Archse.Models;

namespace Archse.Repository
{
    public interface IGamesRepository
    {
        public IEnumerable<Game> ObterTodos();

        public Game ObterPorIdentificador(string identificador);

        public void Adicionar(Game game);

        public void Atualizar(Game game);

        public void Remover(Game game);

        public List<Game> ObterLista();
    }
}
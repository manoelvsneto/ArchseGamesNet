using Archse.Data;
using Archse.Models;
using Microsoft.EntityFrameworkCore;

namespace Archse.Repository
{
    public class GamesRepository : IGamesRepository
    {
        private readonly DataContext _dbContext;

        public GamesRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  IEnumerable<Game> ObterTodos()
        {
            return  _dbContext.Games.ToList();
        }

        public  Game ObterPorIdentificador(string identificador)
        {
            return  _dbContext.Games.Where(x => x.Identificador == identificador).FirstOrDefault();
        }

        public List<Game> ObterLista()
        {
            return _dbContext.Games.ToList<Game>();
        }

        public  void Adicionar(Game game)
        {
             _dbContext.Games.Add(game);
             _dbContext.SaveChanges();
        }

        public  void Atualizar(Game game)
        {
            _dbContext.Entry(game).State = EntityState.Modified;
             _dbContext.SaveChanges();
        }

        public  void Remover(Game game)
        {
            _dbContext.Games.Remove(game);
             _dbContext.SaveChanges();
        }
    }
}
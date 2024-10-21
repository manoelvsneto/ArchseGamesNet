
namespace Archse.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Identificador { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public static implicit operator Game(GameResponse v)
        {
            throw new NotImplementedException();
        }
    }

  
}

using Archse.Models;

namespace Archse.Events;

public class GameInsertedEvent
{
    public string Identificador { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}
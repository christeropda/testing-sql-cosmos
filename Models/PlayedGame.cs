using cosmosdb_test.Enums;

namespace cosmosdb_test.Models;

public class PlayedGame
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Moves PlayerMove {  get; set; }
    public Moves MachineMove { get; set; }
    public Result MatchResult { get; set; }
}

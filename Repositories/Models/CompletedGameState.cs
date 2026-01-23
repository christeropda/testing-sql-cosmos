using cosmosdb_test.Enums;

namespace cosmosdb_test.Repositories.Models;

public class CompletedGameState
{
    public Guid Id { get; set; }
    public Moves PlayerMove {  get; set; }
    public Moves MachineMove { get; set; }
    public Result MatchResult { get; set; }
}

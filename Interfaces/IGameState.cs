using cosmosdb_test.Enums;

namespace cosmosdb_test.Interfaces;

public interface IGameState
{
    public Moves MachineMove { get; }
    public Moves PlayerMove { get; }
    public Result MatchResult { get; set; }
    public void EvaluateWinner(string? playerMove);
}

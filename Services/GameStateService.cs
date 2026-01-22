using cosmosdb_test.Enums;
using cosmosdb_test.Interfaces;

namespace cosmosdb_test.Services;

public class GameStateService : IGameState
{
    public Moves MachineMove { get; }
    public Moves PlayerMove { get; set; }
    public Result MatchResult { get; set; }

    public GameStateService()
    {

        Random rnd = new Random();
        (MachineMove) = rnd.Next(0, 3) switch
        {
            0 => Moves.Stein,
            1 => Moves.Saks,
            _ => Moves.Papir,
        };
    }

    public void EvaluateWinner(string? playerMove)
    {
        if (string.IsNullOrEmpty(playerMove))
        {
            throw new ArgumentNullException("Spillers trekk kan ikke være null eller tom");
        }

        PlayerMove = playerMove switch
        {
            _ when playerMove.Equals("stein", StringComparison.OrdinalIgnoreCase) => Moves.Stein,
            _ when playerMove.Equals("saks", StringComparison.OrdinalIgnoreCase) => Moves.Saks,
            _ when playerMove.Equals("papir", StringComparison.OrdinalIgnoreCase) => Moves.Papir,
            _ => throw new ArgumentException($"Feil: {playerMove} er et ukjent trekk."),
        };

        MatchResult = (PlayerMove, MachineMove) switch 
        {
            (Moves.Stein, Moves.Stein) or
            (Moves.Papir, Moves.Papir) or
            (Moves.Saks, Moves.Saks) => MatchResult = Result.Uavgjort,
            (Moves.Stein, Moves.Saks) or
            (Moves.Papir, Moves.Stein) or
            (Moves.Saks, Moves.Papir) => Result.SpillerVinner,
            _ => Result.MaskinVinner,
        };
    }
}
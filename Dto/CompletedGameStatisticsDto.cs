using cosmosdb_test.Enums;
using cosmosdb_test.Repositories.Models;

namespace cosmosdb_test.Dto;

public class CompletedGameStatisticsDto
{
    public int Rock         { get; private set; }
    public int Paper        { get; private set; }
    public int Scissor      { get; private set; }
    public int PlayerWin    { get; private set; }
    public int MachineWin   { get; private set; }
    public int Draw         { get; private set; }

    public CompletedGameStatisticsDto(IEnumerable<CompletedGameState> completedGames)
    {

        if (completedGames is null) throw new ArgumentNullException(nameof(completedGames));

        foreach (var game in completedGames)
        {
            AddMove(game.PlayerMove, game.MachineMove);
            AddWinner(game.MatchResult);
        }
    }

    public void AddMove(Moves player, Moves machine)
    {

        switch (player)
        {
            case Moves.Stein: Rock++; break;
            case Moves.Papir: Paper++; break;
            case Moves.Saks: Scissor++; break;
            default: throw new ArgumentOutOfRangeException(nameof(player));
        }

        switch (machine)
        {
            case Moves.Stein: Rock++; break;
            case Moves.Papir: Paper++; break;
            case Moves.Saks: Scissor++; break;
            default: throw new ArgumentOutOfRangeException(nameof(machine));
        }
    }

    public void AddWinner(Result state)
    {
        switch (state)
        {
            case Result.SpillerVinner: PlayerWin += 1; break;
            case Result.MaskinVinner: MachineWin += 1; break;
            case Result.Uavgjort: Draw += 1; break;
            default: throw new ArgumentOutOfRangeException(nameof(state));
        }
    }
}

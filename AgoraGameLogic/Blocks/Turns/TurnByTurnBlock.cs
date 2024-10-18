using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Turns;

public class TurnByTurnBlock : TurnBlockBlockBase
{
    private int _currentPlayerIndex = 0;
    private bool _isClockwise = true;
    
    public TurnByTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        StartBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
        UpdateBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[1].AsValidArray(), gameData);
        EndBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[2].AsValidArray(), gameData);
    }

    protected override async Task<Result> ExecuteAsync(IContext context)
    {
        try
        {
            var exitCondition = HasOption<ExitConditionTurnBlock>() 
                ? GetOptionOrThrow<ExitConditionTurnBlock>().GetExitConditionOrThrow(context) 
                : Value<bool>.FromOrThrow(true);
        
            var numberOfRounds = HasOption<NumberOfRoundsTurnOption>() 
                ? GetOptionOrThrow<NumberOfRoundsTurnOption>().GetNumberOfRoundsOrThrow(context) * Players.Count
                : int.MaxValue;

            // number of rounds or exit condition
            var currentRound = 0;
            while (currentRound < numberOfRounds && exitCondition.GetValueOrThrow(context))
            {
                // execute player turn
                var playerTurnResult = await ExecutePlayerTurn(context);
                if (!playerTurnResult.IsSuccess)
                {
                    return Result.Failure(playerTurnResult.Error);
                }
            
                // update number of rounds
                currentRound++; 
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    private GameModule GetNextPlayer()
    {
        if (_isClockwise)
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Count;
        }
        else
        {
            _currentPlayerIndex = (_currentPlayerIndex - 1 + Players.Count) % Players.Count;
        }

        // Return the next player
        return Players[_currentPlayerIndex];
    }
    
    private async Task<Result> ExecutePlayerTurn(IContext context) 
    {
        var currentPlayer = GetNextPlayer();
        context.AddOrUpdate("player", ref currentPlayer);
        
        // start, update, end
        var executionResult = await ExecuteStart(context, currentPlayer)
        .ThenAsync(async () => await ExecuteUpdate(context, currentPlayer))
        .ThenAsync(async () => await ExecuteEnd(context, currentPlayer));

        return executionResult;
    }
    
    // METHODS

    public void ChangeRotation()
    {
        _isClockwise = !_isClockwise;
    }
    
    public void MakeNextPlayer(GameModule player, bool stopCurrentTurn)
    {
        throw new NotImplementedException();
    }
}
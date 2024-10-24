using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Turns;

public class TurnByTurnBlock : TurnBlock
{
    private Dictionary<GameModule, Task<Result>> _executeTurnTaskByPlayer = new Dictionary<GameModule, Task<Result>>();
    private int _currentPlayerIndex = 0;
    private bool _isClockwise = true;
    
    public TurnByTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        StartBranch = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[0].AsValidArray(), gameData);
        UpdateBranch = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[1].AsValidArray(), gameData);
        EndBranch = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[2].AsValidArray(), gameData);
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            // exit condition
            var exitCondition = HasOption<ExitConditionTurnBlock>() 
                ? GetOptionOrThrow<ExitConditionTurnBlock>().GetExitConditionOrThrow(Context) 
                : Value<bool>.FromOrThrow(true);
        
            // number of turns
            var maxNumberOfTurns = HasOption<NumberOfRoundsTurnOption>() 
                ? GetOptionOrThrow<NumberOfRoundsTurnOption>().GetNumberOfRoundsOrThrow(Context) * Players.Count
                : int.MaxValue;

            // number of turns or exit condition
            var currentTurn = 0;
            while (currentTurn < maxNumberOfTurns && exitCondition.GetValueOrThrow(Context))
            {
                // execute player turn
                var currentPlayer = GetNextPlayer();
                _executeTurnTaskByPlayer[currentPlayer] = ExecutePlayerTurn(Context, currentPlayer);
                
                // await task
                var playerTurnResult = await _executeTurnTaskByPlayer[currentPlayer];
                if (!playerTurnResult.IsSuccess)
                {
                    return Result.Failure(playerTurnResult.Error);
                }
            
                // update number of rounds
                currentTurn++; 
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    protected override Result EndCurrentTurnCore(GameModule player)
    {
        try
        {
            ResumeCurrentTurn(player);
                
            while (!_executeTurnTaskByPlayer[player].IsCompleted)
            {
                // force inputs
                var currentPlayer = Players[_currentPlayerIndex];
                while (InputService.HasUnresolvedInputs(currentPlayer))
                {
                    InputService.ResolveNextInput(currentPlayer);
                }
                
                // force an action
                ActionService.ForcePerformActionAsync(player.Id);
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    /// <summary>
    /// PUBLIC
    /// </summary>
    #region Public Methods

    public void ChangeRotation()
    {
        _isClockwise = !_isClockwise;
    }
    
    public void SetCurrentPlayerIndex(GameModule player, bool stopCurrentTurn)
    {
        // Find the player's index in the player list
        var playerIndex = Players.FindIndex(p => p.Equals(player));

        if (playerIndex == -1)
        {
            throw new ArgumentException("Player not found in the player list");
        }

        // Set the next player index
        _currentPlayerIndex = playerIndex;

        // Optionally stop the current turn and force the next player immediately
        if (stopCurrentTurn)
        {
            // Add logic here to immediately end the current player's turn
            // and proceed to the next player, if necessary
        }
    }

    #endregion

    /// <summary>
    /// PRIVATE
    /// </summary>
    #region Private Methods

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
    
    private async Task<Result> ExecutePlayerTurn(IContext context, GameModule player)
    {
        context.AddOrUpdate("player", ref player);
        
        // start, update, end
        var startResult = await ExecuteStart(context, player);
        if (!startResult.IsSuccess)
        {
            return Result.Failure(startResult.Error);
        }
        
        var updateResult = await ExecuteUpdate(context, player);
        if (!updateResult.IsSuccess)
        {
            return Result.Failure(updateResult.Error);
        }
                
        var endResult = await ExecuteEnd(context, player);
        if (!endResult.IsSuccess)
        {
            return Result.Failure(endResult.Error);
        }
            
        return Result.Success();
    }

    #endregion
}
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class TurnBlock : StatementBlock
{
    protected StatementBlock[] StartBranch;
    protected StatementBlock[] UpdateBranch;
    protected StatementBlock[] EndBranch;
    
    private Dictionary<GameModule, int> _numberOfActionByPlayer = new Dictionary<GameModule, int>(); // for NumberOfActionOption

    protected abstract Result EndCurrentTurnCore(GameModule player);
    
    /// <summary>
    /// Constructor
    /// </summary>
    public TurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    #region State, Update, End

    protected async Task<Result> ExecuteStart(IContext context, GameModule player)
    {
        // define scope
        var startScope = new TurnScope()
        {
            TurnBlock = this,
            TurnState = TurnState.Start,
            Player = player,
        };
        
        var executeSequenceResult = await ExecuteSequenceAsync(StartBranch, scope: startScope);
        if (!executeSequenceResult.IsSuccess)
        {
            return Result.Failure(executeSequenceResult.Error);
        }
        
        return Result.Success();
    }
    
    protected async Task<Result> ExecuteUpdate(IContext context, GameModule player)
    {
        try
        {
            // NumberOfAction
            var numberOfAllowedAction = HasOption<NumberOfActionTurnOption>() 
                ? GetOptionOrThrow<NumberOfActionTurnOption>().GetNumberOfActionOrThrow(context) 
                : 1;
            
            _numberOfActionByPlayer[player] = 0;
            ResetCompletionSource(player.Id);
            
            while (_numberOfActionByPlayer[player] < numberOfAllowedAction)
            {
                // remove previous update command for this player
                FilterCommands(TurnState.Update, player);
        
                // define update scope
                var updateScope = new TurnScope()
                {
                    TurnBlock = this,
                    Player = player,
                    TurnState = TurnState.Update
                };
                
                // execute update
                var executeSequenceResult = await ExecuteSequenceAsync(UpdateBranch, scope: updateScope);
                if (!executeSequenceResult.IsSuccess)
                {
                    return Result.Failure(executeSequenceResult.Error);
                }
                
                // wait for an action to be made
                await GetOrCreateCompletionSource(player.Id).Task;
                ResetCompletionSource(player.Id);
            }
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    protected async Task<Result> ExecuteEnd(IContext context, GameModule player)
    {
        try
        {
            // remove all commands from start or update
            FilterCommands(TurnState.Start, player);
            FilterCommands(TurnState.Update, player);

            // define end scope
            var endScope = new TurnScope()
            {
                TurnBlock = this,
                Player = player,
                TurnState = TurnState.End
            };
            
            // execute end
            var executeSequenceResult = await ExecuteSequenceAsync(EndBranch, scope: endScope);
            if (!executeSequenceResult.IsSuccess)
            {
                return Result.Failure(executeSequenceResult.Error);
            }

            FilterCommands(TurnState.End, player);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    #endregion

    /// <summary>
    /// PUBLIC METHODS
    /// </summary>
    #region Methods

    public Result EndCurrentTurn(GameModule player)
    {
        return EndCurrentTurnCore(player);
    }

    public Result ResumeCurrentTurn(GameModule player)
    {
        ValidateCompletionSource(player.Id);
        return Result.Success();
    }
    
    public Result ResumeAllPlayerCurrentTurn()
    {
        foreach (var player in Players)
        {
            ValidateCompletionSource(player.Id);
        }
        
        return Result.Success();
    }

    public Result RegisterActionCount(GameModule player)
    {
        try
        {
            if (!_numberOfActionByPlayer.ContainsKey(player))
            {
                _numberOfActionByPlayer[player] = 0;
            }

            _numberOfActionByPlayer[player]++;

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    #endregion

    private void FilterCommands(TurnState state, GameModule player)
    {
        var filterScope = new TurnScope()
        {
            TurnBlock = this,
            TurnState = state,
            Player = player
        };
        
        ActionService.FilterCommands(filterScope);
        InputService.FilterCommands(filterScope);
    }

    // public async Task<Result> EndCurrentTurn(GameModule player)
    // {
    //     // trigger the remaining steps
    //     // if is in Start
    //     if (_turnScopeByPlayer[player] == TurnState.Start)
    //     {
    //         // filter inputs
    //         _inputService.FilterCommands(this, TurnState.Start, player);
    //         
    //         // trigger update
    //         var updateTask = ExecuteUpdate(_context, player);
    //         while (!updateTask.IsCompleted)
    //         {
    //             _inputService.FilterCommands(this, TurnState.Update, player);
    //         }
    //     }
    //     
    //     // filter start and update commands
    //     _actionService.FilterCommands(this, TurnState.Start, player);
    //     _inputService.FilterCommands(this, TurnState.Start, player);
    //     _actionService.FilterCommands(this, TurnState.Update, player);
    //     _inputService.FilterCommands(this, TurnState.Update, player);
    //     
    //     if (_turnScopeByPlayer[player] != TurnState.End)
    //     {
    //         // trigger update
    //         var endTurnTask = ExecuteEnd(_context, player);
    //         while (!endTurnTask.IsCompleted)
    //         {
    //             _inputService.FilterCommands(this, TurnState.End, player);
    //         }
    //     }
    //
    //     return Result.Success();
    // }
}
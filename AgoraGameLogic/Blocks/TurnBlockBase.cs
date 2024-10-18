using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Blocks.Turns;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class TurnBlockBlockBase : StatementBlockBase
{
    protected StatementBlockBase[] StartBranch;
    protected StatementBlockBase[] UpdateBranch;
    protected StatementBlockBase[] EndBranch;
    
    private IActionService _actionService;
    private IInputService _inputService;
    
    private Dictionary<GameModule, int> _numberOfActionByPlayer = new Dictionary<GameModule, int>(); // for NumberOfActionOption
    private Dictionary<GameModule, TurnState> _turnScopeByPlayer = new Dictionary<GameModule, TurnState>();
    
    public TurnBlockBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _actionService = gameData.ActionService;
        _inputService = gameData.InputService;
    }

    #region State managemenent

    protected async Task<Result> ExecuteStart(IContext context, GameModule player)
    {
        _turnScopeByPlayer[player] = TurnState.Start;
        
        // define scope
        var startScope = new Scope()
        {
            Context = context,
            TurnBlock = this,
            TurnState = TurnState.Start,
            Player = player,
        };
        
        var executeSequenceResult = await ExecuteSequenceAsync(StartBranch, startScope);
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
            _turnScopeByPlayer[player] = TurnState.Update;
            
            var numberOfAllowedAction = HasOption<NumberOfActionTurnOption>() 
                ? GetOptionOrThrow<NumberOfActionTurnOption>().GetNumberOfActionOrThrow(context) 
                : 1;
            
            _numberOfActionByPlayer[player] = 0;
        
            while (_numberOfActionByPlayer[player] < numberOfAllowedAction)
            {
                // remove previous update command for this player
                FilterCommands(TurnState.Update, player);
        
                // define update scope
                var updateScope = new Scope()
                {
                    Context = context,
                    TurnBlock = this,
                    Player = player,
                    TurnState = TurnState.Update
                };
                
                // execute update
                var executeSequenceResult = await ExecuteSequenceAsync(UpdateBranch, updateScope);
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
            _turnScopeByPlayer[player] = TurnState.End;
            
            // remove all commands from start or update
            FilterCommands(TurnState.Start, player);
            FilterCommands(TurnState.Update, player);

            // define end scope
            var endScope = new Scope()
            {
                Context = context,
                TurnBlock = this,
                Player = player,
                TurnState = TurnState.End
            };
            
            // execute end
            var executeSequenceResult = await ExecuteSequenceAsync(EndBranch, endScope);
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
    
    // fwfe
    
    public Result RegisterActionCount(GameModule player)
    {
        try
        {
            if (!_numberOfActionByPlayer.ContainsKey(player))
            {
                _numberOfActionByPlayer[player] = 0;
            }

            _numberOfActionByPlayer[player]++;
            ValidateCompletionSource();

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public void FilterCommands(TurnState turnState, GameModule player)
    {
        _actionService.FilterCommands(this, turnState, player);
        _inputService.FilterCommands(this, turnState, player);
    }

    public async Task<Result> EndCurrentTurn(Scope scope, GameModule? playerToContinueFrom)
    {
        // trigger the remaining steps
        // if is in Start
        if (_turnScopeByPlayer[scope.Player] == TurnState.Start)
        {
            // filter inputs
            _inputService.FilterCommands(this, TurnState.Start, scope.Player);
            
            // trigger update
            var updateTask = ExecuteUpdate(scope.Context, scope.Player); // !!!
            while (!updateTask.IsCompleted)
            {
                _inputService.FilterCommands(this, TurnState.Update, scope.Player);
            }
        }
        
        // filter start and update commands
        _actionService.FilterCommands(this, TurnState.Start, scope.Player);
        _inputService.FilterCommands(this, TurnState.Start, scope.Player);
        _actionService.FilterCommands(this, TurnState.Update, scope.Player);
        _inputService.FilterCommands(this, TurnState.Update, scope.Player);
        
        // 
        
        
        
        
        
        // if is in Update (will be if was in start)
        if (_turnScopeByPlayer[scope.Player] == TurnState.Update)
        {
            // trigger end without waiting
            scope.TurnState = TurnState.Update;
            var executeSequenceResult = await ExecuteSequenceAsync(UpdateBranch, scope);
            if (!executeSequenceResult.IsSuccess)
            {
                return Result.Failure(executeSequenceResult.Error);
            }
            
            // trigger update without waiting, only once
            var updateTask = ExecuteUpdate(scope.Context, scope.Player, false); // should not Await!!!
            while (!updateTask.IsCompleted)
            {
                
            }
            
            if (!updateResult.IsSuccess)
            {
                return Result.Failure(updateResult.Error);
            }
        }
        
        
        // 
        if (playerToContinueFrom != null && this is TurnByTurnBlock turnByTurnBlock)
        {
            turnByTurnBlock.SetCurrentPlayerIndex(playerToContinueFrom);
        }
        
        // TODO 
    }
}
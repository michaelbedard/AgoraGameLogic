using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Services;
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
    
    // for NumberOfActionOption
    private Dictionary<GameModule, int> _numberOfActionByPlayer = new Dictionary<GameModule, int>();
    
    public TurnBlockBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _actionService = gameData.ActionService;
        _inputService = gameData.InputService;
    }
    
    protected async Task<Result> ExecuteStart(IContext context, GameModule player)
    {
        var startScope = new Scope(this, ScopeType.Start, player.Id);
        var executeSequenceResult = await ExecuteSequenceAsync(StartBranch, context, startScope);
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
            var numberOfAllowedAction = HasOption<NumberOfActionTurnOption>() ? GetOptionOrThrow<NumberOfActionTurnOption>().GetNumberOfActionOrThrow(context) : 1;
            _numberOfActionByPlayer[player] = 0;
        
            while (_numberOfActionByPlayer[player] < numberOfAllowedAction)
            {
                // remove previous update command for this player
                FilterCommands(ScopeType.Update, player);
        
                // execute update
                var updateScope = new Scope(this, ScopeType.Update, player.Id);
                var executeSequenceResult = await ExecuteSequenceAsync(UpdateBranch, context, updateScope);
                if (!executeSequenceResult.IsSuccess)
                {
                    return Result.Failure(executeSequenceResult.Error);
                }
                
                // wait for an action to be made
                await CompletionSource.Task;
                ResetCompletionSource();
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
            FilterCommands(ScopeType.Start, player);
            FilterCommands(ScopeType.Update, player);

            // execute end
            var endScope = new Scope(this, ScopeType.End, player.Name);
            var executeSequenceResult = await ExecuteSequenceAsync(EndBranch, context, endScope);
            if (!executeSequenceResult.IsSuccess)
            {
                return Result.Failure(executeSequenceResult.Error);
            }

            FilterCommands(ScopeType.End, player);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
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

    public void FilterCommands(ScopeType scopeType, GameModule player)
    {
        _actionService.FilterActions(this, scopeType, player);
        _inputService.FilterActions(this, scopeType, player);
    }
}
using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Logic.Blocks.Turns.Option;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseTurnBlock : BaseStatementBlock
{
    protected BaseStatementBlock[] StartBranch;
    protected BaseStatementBlock[] UpdateBranch;
    protected BaseStatementBlock[] EndBranch;
    
    private AnimationService _animationService;
    
    // for NumberOfActionOption
    private Dictionary<GameModule, int> _numberOfActionByPlayer = new Dictionary<GameModule, int>();
    
    public BaseTurnBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _animationService = gameData.AnimationService;
    }
    
    protected async Task ExecuteStart(Context context, GameModule player)
    {
        var startScope = new Scope(this, ScopeType.Start, player.Id);
        await ExecuteSequenceAsync(StartBranch, context, startScope);
    }
    
    protected async Task ExecuteUpdate(Context context, GameModule player)
    {
        var numberOfAllowedAction = HasOption<NumberOfActionOption>() ? GetOption<NumberOfActionOption>().GetNumberOfAction(context) : 1;
        _numberOfActionByPlayer[player] = 0;
        
        while (_numberOfActionByPlayer[player] < numberOfAllowedAction)
        {
            // remove previous update command for this player
            FilterCommands(ScopeType.Update, player);
        
            // execute update
            var updateScope = new Scope(this, ScopeType.Update, player.Id);
            await ExecuteSequenceAsync(UpdateBranch, context, updateScope);
                
            // wait for an action to be made
            await CompletionSource.Task;
            ResetCompletionSource();
        }
    }
    
    protected async Task ExecuteEnd(Context context, GameModule player)
    {
        // remove all commands from start or update
        FilterCommands(ScopeType.Start, player);
        FilterCommands(ScopeType.Update, player);
        
        // execute end
        var endScope = new Scope(this, ScopeType.End, player.Name);
        await ExecuteSequenceAsync(EndBranch, context, endScope);
        
        FilterCommands(ScopeType.End, player);
    }
    
    // fwfe
    
    public void RegisterActionCount(GameModule player)
    {
        if (!_numberOfActionByPlayer.ContainsKey(player))
        {
            _numberOfActionByPlayer[player] = 0;
        }
        
        _numberOfActionByPlayer[player]++;
        ValidateCompletionSource();
    }

    protected void FilterCommands(ScopeType scopeType, GameModule player)
    {
        _animationService.FilterActions(this, scopeType, player);
    }
}
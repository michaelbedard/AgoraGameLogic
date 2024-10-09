using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseBlock
{
    private ScoringService _scoringService;
    
    // PROTECTED FIELD 
    protected BlockType BlockType;
    protected BaseOptionBlock[] Options;
    
    protected IEnumerable<GameModule> Players;
    

    public BaseBlock(BlockDefinition definition, GameData gameData)
    {
        _scoringService = gameData.ScoringService;
        
        Options = BlockFactory.CreateArray<BaseOptionBlock>(definition.Options, gameData);
        Players = gameData.Players;
    }
    
    // OPTIONS
    
    public bool HasOption<T>() where T : BaseOptionBlock
    {
        return Options.Any(option => option is T);
    }

    public IEnumerable<T> GetOptions<T>() where T : BaseOptionBlock
    {
        return Options.OfType<T>();
    }
    
    public T GetOption<T>() where T : BaseOptionBlock
    {
        return GetOptions<T>().ToList()[0];
    }
    
    // SCORING
    
    protected int GetScore(GameModule player)
    {
        return _scoringService.GetScore(player);
    }
    
    protected int GetScore(GameModule player, string tag)
    {
        return _scoringService.GetScoreForTag(player, tag);
    }
    
    // SEQUENCE
    
    protected async Task ExecuteSequenceAsync(BaseStatementBlock[] blocks, Context context, Scope? scope)
    {
        foreach (var block in blocks)
        {
            await block.ExecuteAsync(context, scope);
        }
    }
}
using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BlockBase
{
    private ScoringService _scoringService;
    
    // PROTECTED FIELD 
    protected BlockType BlockType;
    protected OptionBlockBase[] Options;
    
    protected IEnumerable<GameModule> Players;
    

    public BlockBase(BlockDefinition definition, GameData gameData)
    {
        _scoringService = gameData.ScoringService;
        
        Options = BlockFactory.CreateArray<OptionBlockBase>(definition.Options, gameData);
        Players = gameData.Players;
    }
    
    // OPTIONS
    
    public bool HasOption<T>() where T : OptionBlockBase
    {
        return Options.Any(option => option is T);
    }

    public IEnumerable<T> GetOptions<T>() where T : OptionBlockBase
    {
        return Options.OfType<T>();
    }
    
    public T GetOption<T>() where T : OptionBlockBase
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
    
    protected async Task ExecuteSequenceAsync(StatementBlockBase[] blocks, IContext context, Scope? scope)
    {
        foreach (var block in blocks)
        {
            await block.ExecuteAsync(context, scope);
        }
    }
}
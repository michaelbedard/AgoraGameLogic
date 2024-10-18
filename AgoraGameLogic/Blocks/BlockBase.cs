using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Services;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class BlockBase
{
    private ScoringService _scoringService;
    
    // PROTECTED FIELD 
    protected BlockType BlockType;
    protected OptionBlockBase[] Options;
    
    protected List<GameModule> Players;
    

    public BlockBase(BlockBuildData buildData, GameData gameData)
    {
        _scoringService = gameData.ScoringService;
        
        Options = BlockFactory.CreateArrayOrThrow<OptionBlockBase>(buildData.Options, gameData);
        Players = gameData.Players;
    }
    
    // OPTIONS
    
    public bool HasOption<T>() where T : OptionBlockBase
    {
        return Options.Any(option => option is T);
    }

    public IEnumerable<T> GetOptionsOrThrow<T>() where T : OptionBlockBase
    {
        return Options.OfType<T>();
    }
    
    public T GetOptionOrThrow<T>() where T : OptionBlockBase
    {
        var options = GetOptionsOrThrow<T>();
        if (options.Count() == 0)
        {
            throw new Exception($"block has option {typeof(T)}");
        }
        return GetOptionsOrThrow<T>().ToList()[0];
    }
    
    // SCORING
    
    protected int GetScoreOrThrow(GameModule player)
    {
        var scoreResult = _scoringService.GetScoreForPlayer(player);
        if (!scoreResult.IsSuccess)
        {
            throw new Exception(scoreResult.Error);
        }

        return scoreResult.Value;
    }
    
    protected int GetScoreOrThrow(GameModule player, string tag)
    {
        var scoreResult = _scoringService.GetScoreForPlayerForTag(player, tag);
        if (!scoreResult.IsSuccess)
        {
            throw new Exception(scoreResult.Error);
        }

        return scoreResult.Value;
    }
    
    // SEQUENCE
    
    protected async Task<Result> ExecuteSequenceAsync(StatementBlockBase[] blocks, IContext context, Scope? scope)
    {
        foreach (var block in blocks)
        {
            var executeResult = await block.ExecuteAsync(context, scope);
            if (!executeResult.IsSuccess)
            {
                return Result.Failure(executeResult.Error);
            }
        }
        
        return Result.Success();
    }
    
    protected async Task ExecuteSequenceOrThrowAsync(StatementBlockBase[] blocks, IContext context, Scope? scope)
    {
        var executeSequenceResult = await ExecuteSequenceAsync(blocks, context, scope);
        if (!executeSequenceResult.IsSuccess)
        {
            throw new Exception(executeSequenceResult.Error);
        }
    }
}
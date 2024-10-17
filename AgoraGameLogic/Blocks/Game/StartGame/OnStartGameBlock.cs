using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Game;

public class OnStartGameBlock : EventBlockBase<StartGameCommand>
{
    public OnStartGameBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(IContext context, StartGameCommand command, Scope? scope)
    {
        return await ExecuteSequenceAsync(Blocks, context, scope);
    }
}
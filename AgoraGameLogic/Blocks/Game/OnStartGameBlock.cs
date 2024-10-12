using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Game;

public class OnStartGameBlock : EventBlockBase
{
    public OnStartGameBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        Blocks = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task TriggerAsync(IContext context, object[] args, Scope? scope)
    {
        await ExecuteSequenceAsync(Blocks, context, scope);
    }
}
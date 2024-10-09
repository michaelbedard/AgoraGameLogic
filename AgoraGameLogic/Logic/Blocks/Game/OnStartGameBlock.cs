using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Game;

public class OnStartGameBlock : BaseEventBlock
{
    public OnStartGameBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        Blocks = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task TriggerAsync(Context context, object[] args, Scope? scope)
    {
        await ExecuteSequenceAsync(Blocks, context, scope);
    }
}
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class OnChoiceBlock : EventBlockBase
{
    public OnChoiceBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }

    protected override async Task TriggerAsync(IContext context, object[] args, Scope? scope)
    {
        throw new NotImplementedException();
    }
}
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class OnChoiceBlock : EventBlockBase<ChoiceCommand>
{
    public OnChoiceBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> TriggerAsync(IContext context, ChoiceCommand command, Scope? scope)
    {
        throw new NotImplementedException();
    }
}
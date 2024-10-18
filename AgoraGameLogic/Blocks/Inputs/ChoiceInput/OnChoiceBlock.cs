using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Inputs.ChoiceInput;

public class OnChoiceBlock : EventBlockBase<ChoiceCommand>
{
    public OnChoiceBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> TriggerAsync(Scope scope, ChoiceCommand command)
    {
        throw new NotImplementedException();
    }
}
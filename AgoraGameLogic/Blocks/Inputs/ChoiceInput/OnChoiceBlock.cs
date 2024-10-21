using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Inputs.ChoiceInput;

public class OnChoiceBlock : EventBlock<ChoiceCommand>
{
    public OnChoiceBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> TriggerAsyncCore(ChoiceCommand command)
    {
        throw new NotImplementedException();
    }
}
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks._options;

public class OnlyTriggerIfTargetedBlock : OptionBlockBase
{
    public OnlyTriggerIfTargetedBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        
    }
}
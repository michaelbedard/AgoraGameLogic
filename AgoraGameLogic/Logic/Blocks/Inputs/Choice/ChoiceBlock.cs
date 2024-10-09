using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceBlock : BaseInputBlock<ChoiceCommand, ChoiceBlock>
{
    private Value<GameModule> _player;
    private Value<object[]> _choices;
    
    public ChoiceBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _player = Value<GameModule>.Parse(definition.Inputs[0], gameData);
        _choices = Value<object[]>.Parse(definition.Inputs[1], gameData);
    }

    public override ChoiceCommand GetCommand(Context context)
    {
        var player = _player.GetValue(context);
        var choices = _choices.GetValue(context);
        
        var options = new Dictionary<string, object>();

        return new ChoiceCommand(this, Scope)
        {
            Player = player,
            Choices = choices,
            Options = options,
        };
    }

    protected override void CompletePendingRequest(PendingRequest<BaseInputCommand> pendingRequest, ChoiceCommand command)
    {
        pendingRequest.For(command.Player);
    }
}
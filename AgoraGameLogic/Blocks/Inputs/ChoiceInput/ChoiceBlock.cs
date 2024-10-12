using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceBlock : InputBlockBase<Choice, ChoiceBlock>
{
    private Value<GameModule> _player;
    private Value<object[]> _choices;
    
    public ChoiceBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _player = Value<GameModule>.Parse(definition.Inputs[0], gameData);
        _choices = Value<object[]>.Parse(definition.Inputs[1], gameData);
    }

    public override Choice GetCommand(IContext context)
    {
        var player = _player.GetValue(context);
        var choices = _choices.GetValue(context);
        
        var options = new Dictionary<string, object>();

        return new Choice(this, Scope)
        {
            Player = player,
            Choices = choices,
            Options = options,
        };
    }

    protected override void CompletePendingRequest(PendingRequest<InputCommandBase> pendingRequest, Choice command)
    {
        pendingRequest.For(command.Player);
    }
}
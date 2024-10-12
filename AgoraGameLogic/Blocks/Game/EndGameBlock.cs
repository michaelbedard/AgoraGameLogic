using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Game;

public class EndGameBlock : StatementBlockBase
{
    private Value<EndGameMethod> _method;
    
    public EndGameBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _method = Value<EndGameMethod>.Parse(definition.Inputs[0], gameData);
    }

    public override async Task ExecuteAsync(IContext context, Scope? scope)
    {
        var method = _method.GetValue(context);
        GameModule[] winners;
        object[] args;

        switch (method)
        {
            case EndGameMethod.LeastCardsInHand:
            {
                winners = Players.OrderBy(p => p.Fields.Get<List<GameModule>>("Hand").Count).ToArray();
                args = winners.Select(p => (object)p.Fields.Get<List<GameModule>>("Hand").Count).ToArray();;
                break;
            }
            case EndGameMethod.LeastPoints:
            {
                throw new Exception();
                break;
            }
            case EndGameMethod.MostPoints:
            {
                throw new Exception();
                break;
            }
            default:
            {
                throw new Exception();
            }
        }
        
        // EndGame(method, winners, args);
    }
}
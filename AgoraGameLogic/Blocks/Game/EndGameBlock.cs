using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Game;

public class EndGameBlock : StatementBlockBase
{
    private Value<EndGameMethod> _method;
    
    public EndGameBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _method = Value<EndGameMethod>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        try
        {
            GameModule[] winners;
            object[] args;

            var method = _method.GetValueOrThrow(context);
            switch (method)
            {
                case EndGameMethod.LeastCardsInHand:
                {
                    winners = Players.OrderBy(p => p.Fields.Get<List<GameModule>>("Hand").Count).ToArray();
                    args = winners.Select(p => (object)p.Fields.Get<List<GameModule>>("Hand").Count).ToArray();
                    ;
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

            // TODO
            // EndGame(method, winners, args);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}
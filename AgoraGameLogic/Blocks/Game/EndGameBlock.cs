using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Game;

public class EndGameBlock : StatementBlock
{
    private Value<EndGameMethod> _method;
    
    public EndGameBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _method = Value<EndGameMethod>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            GameModule[] winners;
            object[] args;

            var method = _method.GetValueOrThrow(Context);
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
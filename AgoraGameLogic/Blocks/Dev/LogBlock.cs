using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks._dev;

public class LogBlock : StatementBlockBase
{
    private Value<object> Data { get; set; }

    public LogBlock(BlockDefinition definition, GameData gameData) : base (definition, gameData)
    {
        Data = Value<object>.Parse(definition.Inputs[0], gameData);
    }

    public override async Task ExecuteAsync(IContext context, Scope? scope)
    {
        Data.GetValue(context).PrintToConsole();
    }
}
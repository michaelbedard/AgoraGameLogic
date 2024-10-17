using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks._dev;

public class LogBlock : StatementBlockBase
{
    private Value<object> Data { get; set; }

    public LogBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        Data = Value<object>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        try
        {
            var dataResult = Data.GetValue(context);
            if (!dataResult.IsSuccess)
            {
                return Result.Failure(dataResult.Error);
            }

            Console.WriteLine(dataResult.Value);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure($"Unexpected Error: {e.Message}", new ErrorBuilder()
            {
                ClassName = nameof(LogBlock),
                Context = context
            });
        }
    }
}
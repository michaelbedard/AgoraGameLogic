using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Dev;

public class LogBlock : StatementBlockBase
{
    private Value<object> Data { get; set; }

    public LogBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        Data = Value<object>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public override async Task<Result> ExecuteAsync(Scope scope)
    {
        try
        {
            var dataResult = Data.GetValue(scope.Context);
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
                Scope = scope
            });
        }
    }
}
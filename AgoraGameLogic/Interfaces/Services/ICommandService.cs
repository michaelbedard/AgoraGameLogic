using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Interfaces.Services;

public interface ICommandService<T> where T : Command
{
    void SetGlobalContext(IContext context);
    Result PushCommand(T item, GameModule player);
    Result PullCommand(T item, GameModule player);
    Result RemoveCommand(int commandId);
    Result<T> GetCommand(string playerName, int commandId);
    Result FilterCommands(TurnScope turnScope);
    Result<Dictionary<string, CommandDto[]>> GetDtos();
    Result InitializeDictionaryEntries(List<GameModule> players);
}
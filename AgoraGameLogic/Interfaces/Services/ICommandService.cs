using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Interfaces;

public interface ICommandService<T> where T : Command
{
    Result PushCommand(T item, GameModule player);
    Result PullCommand(T item, GameModule player);
    Result RemoveCommand(int commandId);
    Result<T> GetCommand(string playerName, int commandId);
    Result FilterActions(TurnBlockBlockBase turnBlock, ScopeType scopeType, GameModule player);
    Result<Dictionary<string, CommandDto[]>> GetDtos();
}
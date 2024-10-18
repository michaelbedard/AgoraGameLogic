using System.Collections.Generic;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Actors;

public class PendingRequest<T> where T : Command
{
    private readonly T _item;
    private readonly ICommandService<T> _commandService;
    private readonly IEnumerable<GameModule> _players;
    private readonly bool _isPush;

    public PendingRequest(T item, ICommandService<T> commandService, IEnumerable<GameModule> players, bool isPush = true)
    {
        _item = item;
        _commandService = commandService;
        _players = players;
        _isPush = isPush;
    }

    public Result For(GameModule player)
    {
        if (_isPush)
        {
            var pushResult = _commandService.PushCommand(_item, player);
            if (!pushResult.IsSuccess)
            {
                return Result.Failure(pushResult.Error);
            }
        }
        else
        {
            var pullResult = _commandService.PullCommand(_item, player);
            if (!pullResult.IsSuccess)
            {
                return Result.Failure(pullResult.Error);
            }
        }

        return Result.Success();
    }
    
    public Result For(IEnumerable<GameModule> players)
    {
        foreach (var player in players)
        {
            var pushOrPullResult = For(player);
            if (!pushOrPullResult.IsSuccess)
            {
                return Result.Failure(pushOrPullResult.Error);
            }
        }

        return Result.Success();
    }
    
    public Result ForAll()
    {
        foreach (var player in _players)
        {
            var pushOrPullResult = For(player);
            if (!pushOrPullResult.IsSuccess)
            {
                return Result.Failure(pushOrPullResult.Error);
            }
        }

        return Result.Success();
    }
    
    public Result ForAllExcept(GameModule playerToExempt)
    {
        foreach (var player in _players)
        {
            if (player == playerToExempt) continue;
            
            var pushOrPullResult = For(player);
            if (!pushOrPullResult.IsSuccess)
            {
                return Result.Failure(pushOrPullResult.Error);
            }
        }

        return Result.Success();
    }
}
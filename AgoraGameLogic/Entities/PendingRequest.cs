using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Core.Entities.Utility;

public class PendingRequest<T> where T : CommandBase
{
    private readonly T _item;
    private readonly CommandService<T> _commandService;
    private readonly IEnumerable<GameModule> _players;
    private readonly bool _isPush;

    public PendingRequest(T item, CommandService<T> commandService, IEnumerable<GameModule> players, bool isPush = true)
    {
        _item = item;
        _commandService = commandService;
        _players = players;
        _isPush = isPush;
    }

    public void For(GameModule player)
    {
        if (_isPush)
        {
            _commandService.PushCommand(_item, player);
        }
        else
        {
            _commandService.PullCommand(_item, player);
        }
    }
    
    public void For(IEnumerable<GameModule> players)
    {
        foreach (var player in players)
        {
            if (_isPush)
            {
                _commandService.PushCommand(_item, player);
            }
            else
            {
                _commandService.PullCommand(_item, player);
            }
        }
    }
    
    public void ForAll()
    {
        foreach (var player in _players)
        {
            if (_isPush)
            {
                _commandService.PushCommand(_item, player);
            }
            else
            {
                _commandService.PullCommand(_item, player);
            }
        }
    }
    
    public void ForAllExcept(GameModule playerToExempt)
    {
        foreach (var player in _players)
        {
            if (player == playerToExempt) continue;
            
            if (_isPush)
            {
                _commandService.PushCommand(_item, player);
            }
            else
            {
                _commandService.PullCommand(_item, player);
            }
        }
    }
}
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IPushService<T>
{
    void Push(T item, GameModule player);
    void Pull(T item, GameModule player);
}
using AgoraGameLogic.Actors;

namespace AgoraGameLogic.Interfaces.Services;

public interface IPushService<T>
{
    void Push(T item, GameModule player);
    void Pull(T item, GameModule player);
}
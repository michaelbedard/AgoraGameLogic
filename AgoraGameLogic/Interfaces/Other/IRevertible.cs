using AgoraGameLogic.Actors;

namespace AgoraGameLogic.Interfaces.Other;

public interface IRevertible
{
    void Revert(Context context);
}
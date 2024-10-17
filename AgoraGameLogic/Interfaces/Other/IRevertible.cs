using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IRevertible
{
    void Revert(Context context);
}
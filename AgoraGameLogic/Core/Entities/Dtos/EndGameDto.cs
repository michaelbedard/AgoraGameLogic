using AgoraGameLogic.Domain.Enums;

namespace AgoraGameLogic.Domain.Entities.DataObject;

public class EndGameDto
{
    public EndGameMethod EndGameMethod;
    public string[] Winners;
    public object[] Args;

    public EndGameDto(EndGameMethod endGameMethod, string[] winners, object[] args)
    {
        EndGameMethod = endGameMethod;
        Winners = winners;
        Args = args;
    }
}
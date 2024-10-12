using Newtonsoft.Json;

namespace AgoraGameLogic.Domain.Extensions;

public static class ObjectExtensions
{
    public static void PrintToConsole(this object obj)
    {
        Console.WriteLine(obj.ToString());
    }
}
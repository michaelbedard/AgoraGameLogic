namespace AgoraGameLogic.Domain.Extensions;

using System;
using System.Collections.Generic;

public static class ListExtensions
{
    private static Random rng = new Random();
    
    // Generic shuffle method for a list of any type
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

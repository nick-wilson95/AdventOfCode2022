using System.Numerics;

namespace AdventOfCode2022.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Cumulate<T>(this IEnumerable<T> input) where T : INumber<T>
    {
        var sum = T.Zero;
        
        foreach (var element in input)
        {
            sum += element;
            yield return sum;
        }
    }
}
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
    
    public static List<List<T>> Split<T>(this IEnumerable<T> input, Func<T, bool> splitOn)
    {
        var result = new List<List<T>>{ new() };

        input.ToList().ForEach(x =>
        {
            if (splitOn(x))
            {
                result.Add(new List<T>());
            }
            else
            {
                result.Last().Add(x);
            }
        });

        return result;
    }

    public static T Product<T>(this IEnumerable<T> input) where T : INumber<T>
        => input.Aggregate(T.One, (x, y) => x * y);
}
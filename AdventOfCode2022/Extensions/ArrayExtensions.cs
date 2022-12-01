namespace AdventOfCode2022.Solutions.Extensions;

public static class ArrayExtensions
{
    public static List<List<T>> Split<T>(this IEnumerable<T> input, T splitOn)
    {
        var result = new List<List<T>>{ new() };

        input.ToList().ForEach(x =>
        {
            if (x.Equals(splitOn))
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
}
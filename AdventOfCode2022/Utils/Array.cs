namespace AdventOfCode2022.Utils;

public static class Array
{
    public static T[] Of<T>(Func<int, T> generator, int size)
    {
        return Enumerable.Range(0, size)
            .Select(generator)
            .ToArray();
    }
}